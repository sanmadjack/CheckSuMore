using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CheckSuMore {
    class CheckSuMoreFile {
        private const string FileName = "checksumore";
        private const string FileNameWildcard = FileName + ".*";

        private Dictionary<string, ACheckSumFile> csfs;
        private Dictionary<string,CheckSuMoreFileItem> Items;

        private Version VersionSupported = new Version(1, 0, 0);

        public RecursionType Recursion { get; protected set; }
        private DirectoryInfo RootFolder { get; protected set; }
        private CheckSumType Type;
        
        public CheckSuMoreFile(DirectoryInfo directory, RecursionType recursion, CheckSumType type) {
            if (!directory.Exists) {
                throw new DirectoryNotFoundException(directory);
            }
            RootFolder = directory;
            csfs = new Dictionary<string, ACheckSumFile>();
            Items = new Dictionary<string, CheckSuMoreFileItem>();
            this.Type = type;

            FileInfo[] files;
            ACheckSumFile csf;
            CheckSuMoreFileItem item;
            CheckSumCommentRecord comment;
            string path;
            string file_name;

            if (recursion == RecursionType.None) {
                // If no recursion, then we are only ever concerned with the top level folder
                files = directory.GetFiles(FileNameWildcard, SearchOption.TopDirectoryOnly);
            } else {
                // If there is recursion, then we are concerned with all folders
                // If it's recursion with a root file, then we still scan the subfolder for possible old files to concatenate into the root
                files = directory.GetFiles(FileNameWildcard, SearchOption.AllDirectories);
            }

            foreach (FileInfo file in files) {
                if (file.Extension.ToLower() != GetCheckSumExtension().ToLower()) {
                    continue;
                }

                csf = OpenCheckSumFile(file);
                path = GetPathRelativeToFile(csf.File);
                csfs.Add(path, csf);

                foreach (ACheckSumRecord record in csf.GetRecords()) {
                    if (record is CheckSumCommentRecord) {
                        comment = (record as CheckSumCommentRecord);
                        file_name = CheckSuMoreFileItemHistoryEntry.GetFileFromInput(comment.Comment);

                        if (!Items.ContainsKey(file_name)) {
                            Items.Add(file_name, new CheckSuMoreFileItem(comment));
                        } else {
                            item = Items[file_name];
                            item.AddHistoryEntry(comment);
                        }
                    }
                }
            }
        }

        public void Save() {
            csf.ClearFile();
            csf.AddCommentRecord("CheckSuMore File Format " + VersionSupported.ToString());
            csf.AddCommentRecord(Items.Count + " files listed in this file");
            foreach (string name in Items.Keys) {
                CheckSuMoreFileItem item = Items[name];

            }
            csf.AddCommentRecord("End Of File");
            csf.Save();
        }

		public void Validate() {
            if (!csf.Directory.Exists) {
                throw new DirectoryNotFoundException(csf.Directory);
            }
		}
        
        private void ValidateTraveler(DirectoryInfo dir, ACheckSumFile csf, RecursionType recursion, CheckSumType type) {
            if (!dir.Exists) {
                throw new Exception(dir.FullName + " not found!");
            }
            
            ValidationResult result;
            if (csmf == null) {
                ACheckSumFile csf;
                string new_file_name = Path.Combine(dir.FullName, String.Concat(FileName, ".", MD5CheckSumFile.Extension));
                switch (type) {
                    case CheckSumType.CRC32:
                        csf = new SFVChecksumFile(new_file_name);
                        break;
                    case CheckSumType.MD5:
                        csf = new MD5CheckSumFile(new_file_name);
                        break;
                    default:
                        throw new NotSupportedException(type.ToString() + " not supported");
                }
                csmf = new CheckSuMoreFile(csf);
            }

            foreach (FileInfo file in dir.GetFiles()) {
                if (file.Name == String.Concat(FileName, ".", MD5CheckSumFile.Extension)) {
                    continue;
                }

                System.Console.Out.WriteLine("Checking " + csmf.GetPathRelativeToFile(file));
                result = csmf.Validate(file);
                switch (result) {
                    case ValidationResult.Failed:
                        Console.Out.WriteLine("File failed validation");
                        break;
                    case ValidationResult.Passed:
                        Console.Out.WriteLine("File passed validation");
                        break;
                    case ValidationResult.Changed:
                        Console.Out.WriteLine("File has been changed, updating");
                        break;
                    case ValidationResult.NewFile:
                        Console.Out.WriteLine("File is new, adding to checksums");
                        break;
                    case ValidationResult.Error:
                        Console.Out.WriteLine("Error while processing file");
                        break;
                }
                csmf.Save();
            }

            if (recursion > RecursionType.None) {
                foreach (DirectoryInfo subdir in dir.GetDirectories()) {
                    if (recursion == RecursionType.WithRootFile) {
                        CheckPathHelper(subdir, csmf, recursion, type);
                    } else {
                        CheckPathHelper(subdir, null, recursion, type);
                    }
                }
            }
        }



        private ValidationResult Validate(FileInfo file) {
            CheckSumFileRecord record;
            ACheckSum checksum;
            if (!csf.HasFileRecordFor(file)) {
                checksum = csf.AddFileRecord(file);
                return ValidationResult.New;
            }
            
            record = FileCheckSums[file.Name];
            using (Stream input = file.OpenRead()) {
                file_checksum = CheckSumer.Generate(input);
            }

            if (record.CheckSum.Equals(file_checksum)) {
                return ValidationResult.Passed;
            } else {
                return ValidationResult.Failed;
            }

        }


        public string GetPathRelativeToFile(FileInfo file) {
            DirectoryInfo this_file_dir = this.RootFolder;

            if (file.FullName.StartsWith(this_file_dir.FullName)) {
                return file.FullName.Substring(this_file_dir.FullName.Length).Trim(Path.DirectorySeparatorChar);
            }
            throw new Exception("File does not appear to be a in the root or any subfolder");
        }

        private string GetCheckSumExtension() {
            switch (Type) {
                case CheckSumType.CRC32:
                    return SFVChecksumFile.Extension;
                case CheckSumType.MD5:
                    return MD5CheckSumFile.Extension;
                default:
                    throw new NotSupportedException(Type.ToString() + " not supported");
            }
        }
        private ACheckSumFile OpenCheckSumFile(FileInfo file) {
            switch (Type) {
                case CheckSumType.CRC32:
                    return new SFVChecksumFile(file);
                case CheckSumType.MD5:
                    return new MD5CheckSumFile(file);
                default:
                    throw new NotSupportedException(Type.ToString() + " not supported");
            }
        }


    }
}

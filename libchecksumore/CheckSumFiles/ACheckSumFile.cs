using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
namespace CheckSuMore {
    public abstract class ACheckSumFile {
        protected abstract string GetExtension { get; }

        protected FileInfo File;
        protected List<ACheckSumRecord> Records = new List<ACheckSumRecord>();
        protected Dictionary<string, CheckSumFileRecord> FileCheckSums = new Dictionary<string, CheckSumFileRecord>();

        public void Save() {
            StringBuilder output = new StringBuilder();
            string line;
            foreach (ACheckSumRecord record in Records) {
                line = "";
                if (record is CheckSumFileRecord) {
                    line = RecordFile(record as CheckSumFileRecord);
                } else if (record is CheckSumCommentRecord) {
                    line = RecordComment(record as CheckSumCommentRecord);
                } else if (record is CheckSumBlankRecord) {
                    // Nothing!
                }
                output.AppendLine(line);
                
                System.IO.File.WriteAllText(File.FullName,output.ToString());
            }
        }

        protected abstract string RecordFile(CheckSumFileRecord record);
        protected abstract string RecordComment(CheckSumCommentRecord record);

        public abstract ValidationResult Validate(FileInfo file);
    }
    public abstract class ACheckSumFile<T>: ACheckSumFile where T:ACheckSum {

        protected abstract Regex GetCommentRecordRegex { get; }
        protected abstract Regex GetFileRecordRegex { get; }
        protected abstract Regex GetCheckSumRegex { get; }

        protected ACheckSumer<T> CheckSumer;

        protected ACheckSumFile(string file_path, ACheckSumer<T> checksumer) {
            File = new FileInfo(file_path);
            if (File.Extension.ToLower().TrimStart('.') != GetExtension.ToLower()) {
                throw new NotSupportedException("File does not have the expected extension. " + GetExtension + " was expected, " + File.Extension + " found");
            }
            this.CheckSumer = checksumer;
            if (File.Exists) {
                Records = Open();
            }
        }

        protected List<ACheckSumRecord> Open() {
            List<ACheckSumRecord> records = new List<ACheckSumRecord>();
            ACheckSumRecord record = null;
            MatchCollection matches;
            using (TextReader text = File.OpenText()) {
                string line = text.ReadLine();
                while (line != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        record = new CheckSumBlankRecord();
                    } else if (GetCommentRecordRegex.IsMatch(line)) {
                        matches = GetCommentRecordRegex.Matches(line);
                        record = new CheckSumCommentRecord(matches[0].Groups["comment"].Value);
                    } else if (GetFileRecordRegex.IsMatch(line)) {
                        matches = GetFileRecordRegex.Matches(line);
                        string filename = matches[0].Groups["filename"].Value;
                        string checksum = matches[0].Groups["checksum"].Value;
                        record = new CheckSumFileRecord(filename, this.PrepareCheckSum(checksum));
                        FileCheckSums.Add(filename, record as CheckSumFileRecord);
                    } else {
                        throw new NotSupportedException("A line in this file is not supported");
                    }
                    records.Add(record);
                    line = text.ReadLine();
                }
            }
            return records;
        }

        protected abstract ACheckSum PrepareCheckSum(string hash);

        private string GetPathRelativeToFile(FileInfo file) {
            StringBuilder output = new StringBuilder();
            DirectoryInfo this_file_dir = this.File.Directory;
            DirectoryInfo file_dir = file.Directory;

            if (file.FullName.StartsWith(this_file_dir.FullName)) {
                return file.FullName.Substring(this_file_dir.FullName.Length).Trim(Path.DirectorySeparatorChar);
            }
            throw new Exception("File doe not appear to be a in the root or any subfolder");
        }
        

        public override ValidationResult Validate(FileInfo file) {
            ACheckSum file_checksum;
            CheckSumFileRecord record;
            string relative_path = GetPathRelativeToFile(file);
            if (!FileCheckSums.ContainsKey(relative_path)) {
                using (Stream input = file.OpenRead()) {
                    file_checksum = CheckSumer.Generate(input);
                }
                record = new CheckSumFileRecord(relative_path, file_checksum);
                FileCheckSums.Add(relative_path, record);
                Records.Add(record);
                return ValidationResult.NewFile;
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


            return ValidationResult.Error;
        }


    }
}

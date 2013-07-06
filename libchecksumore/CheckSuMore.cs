using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace CheckSuMore {
    public class CheckSuMore {
        private const string FileName = "checksumore";

        private List<CheckSuMoreFile> files;


        public void CheckPath(string dir_path, RecursionType recursion) {
            CheckPathHelper(new DirectoryInfo(dir_path), null, recursion, ChecksumType.MD5);
        }

        private void CheckPathHelper(DirectoryInfo dir, CheckSuMoreFile csmf, RecursionType recursion, ChecksumType type) {
            if (!dir.Exists) {
                throw new Exception(dir.FullName + " not found!");
            }
            ValidationResult result;
            if (csmf == null) {
                ACheckSumFile csf;
                string new_file_name = Path.Combine(dir.FullName, String.Concat(FileName, ".", MD5CheckSumFile.Extension));
                switch (type) {
                    case ChecksumType.CRC32:
                        csf = new SFVChecksumFile(new_file_name);
                        break;
                    case ChecksumType.MD5:
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

                System.Console.Out.WriteLine("Checking " +  csmf.GetPathRelativeToFile(file));
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
    }
}

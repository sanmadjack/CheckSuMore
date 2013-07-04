using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace CheckSuMore {
    public class CheckSuMore {
        private const string FileName = "checksumore";
        

        public void CheckPath(string dir_path, RecursionType recursion) {
            CheckPathHelper(new DirectoryInfo(dir_path), null, recursion);
            
        }

        private void CheckPathHelper(DirectoryInfo dir, ACheckSumFile csf, RecursionType recursion) {
            if (!dir.Exists) {
                throw new Exception(dir.FullName + " not found!");
            }
            ValidationResult result;
            if (csf == null) {
                csf = new MD5CheckSumFile(Path.Combine(dir.FullName, String.Concat(FileName, ".", MD5CheckSumFile.Extension)));
            }

            foreach (FileInfo file in dir.GetFiles()) {
                if (file.Name == String.Concat(FileName, ".", MD5CheckSumFile.Extension)) {
                    continue;
                }

                System.Console.Out.WriteLine("Checking " +  csf.GetPathRelativeToFile(file));
                result = csf.Validate(file);
                switch (result) {
                    case ValidationResult.Failed:
                        Console.Out.WriteLine("File failed validation");
                        break;
                    case ValidationResult.Passed:
                        Console.Out.WriteLine("File passed validation");
                        break;
                    case ValidationResult.NewFile:
                        Console.Out.WriteLine("File is new, adding to checksums");
                        break;
                    case ValidationResult.Error:
                        Console.Out.WriteLine("Error while processing file");
                        break;
                }
                csf.Save();
            }

            if (recursion > RecursionType.None) {
                foreach (DirectoryInfo subdir in dir.GetDirectories()) {
                    if (recursion == RecursionType.WithRootFile) {
                        CheckPathHelper(subdir, csf, recursion);
                    } else {
                        CheckPathHelper(subdir, null, recursion);
                    }
                }
            }
        }
    }
}

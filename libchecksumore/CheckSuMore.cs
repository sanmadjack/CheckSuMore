using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace CheckSuMore {
    public class CheckSuMore {
        private const string WatchPath = "C:\test";

        public string CheckFiles(string path) {
            DirectoryInfo dir = new DirectoryInfo(path);
            Stream input = null;
            StringBuilder output = new StringBuilder();
            using (MD5 md5 = MD5.Create()) {
                foreach (FileInfo file in dir.GetFiles()) {
                    System.Console.Out.WriteLine("Checking " + file.FullName);
                    using (input = file.OpenRead()) {
                        output.AppendLine(BitConverter.ToString(md5.ComputeHash(input)).Replace("-",""));
                    }
                }
            }
            string return_me = output.ToString();
            return return_me;
        }

    }
}

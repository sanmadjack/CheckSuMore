using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace CheckSuMore {
    public class CheckSuMore {

        private List<CheckSuMoreFile> files;


        public void CheckPath(string dir_path, RecursionType recursion) {
            CheckPathHelper(new DirectoryInfo(dir_path), null, recursion, CheckSumType.MD5);
        }


    }
}

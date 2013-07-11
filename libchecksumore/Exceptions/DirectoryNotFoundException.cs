using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    class DirectoryNotFoundException: Exception {
        private const string msg = " not found!";
        public DirectoryNotFoundException(string directory): base(String.Concat(directory, msg)) {

        }
        public DirectoryNotFoundException(string directory, Exception inner): base(String.Concat(directory, msg),inner) {

        }
        public DirectoryNotFoundException(System.IO.DirectoryInfo directory): base(String.Concat(directory.FullName, msg)) {

        }
        public DirectoryNotFoundException(System.IO.DirectoryInfo directory, Exception inner): base(String.Concat(directory.FullName, msg),inner) {

        }
    }
}

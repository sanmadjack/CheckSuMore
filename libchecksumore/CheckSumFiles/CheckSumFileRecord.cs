using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    public class CheckSumFileRecord: ACheckSumRecord {
        public string FileName { get; protected set; }
        public ACheckSum CheckSum { get; protected set; }

        public CheckSumFileRecord(string file_name, ACheckSum checksum) {
            FileName = file_name;
            this.CheckSum = checksum;
        }
    }
}

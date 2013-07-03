using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace CheckSuMore {
    public class CRC32CheckSumer: ACheckSumer<CRC32CheckSum> {
        

        public override CRC32CheckSum Generate(System.IO.Stream input) {
            throw new NotImplementedException();
        }
    }
}

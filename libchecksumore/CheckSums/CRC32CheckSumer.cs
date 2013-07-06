using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace CheckSuMore {
    public class CRC32CheckSumer: ACheckSumer {


        public override ACheckSum Generate(System.IO.Stream input) {
            throw new NotImplementedException();
        }

        public override ACheckSum Interpret(string input) {
            throw new NotImplementedException();
        }

        protected override string TranslateBytesToHash(byte[] input) {
            throw new NotImplementedException();
        }
    }
}

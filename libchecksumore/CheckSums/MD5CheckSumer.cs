using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
namespace CheckSuMore {
    public class MD5CheckSumer: ACheckSumer {
        private static MD5 md5 = MD5.Create();

        public override ACheckSum Generate(System.IO.Stream input) {
            string hash = TranslateBytesToHash(md5.ComputeHash(input));
            MD5CheckSum checksum = new MD5CheckSum(hash);
            return checksum;
        }

        public override ACheckSum Interpret(string input) {
            return new MD5CheckSum(input);
        }

        protected override string TranslateBytesToHash(byte[] input) {
            return BitConverter.ToString(input).Replace("-", "");
        }
        
    }
}

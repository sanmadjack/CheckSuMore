using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
namespace CheckSuMore {
    public class MD5CheckSumer: ACheckSumer<MD5CheckSum> {
        private static MD5 md5 = MD5.Create();

        public override MD5CheckSum Generate(System.IO.Stream input) {
            string hash = BitConverter.ToString(md5.ComputeHash(input)).Replace("-", "");
            MD5CheckSum checksum = new MD5CheckSum(hash);
            return checksum;
        }
    }
}

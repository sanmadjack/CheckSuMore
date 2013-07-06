using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace CheckSuMore {
    public class MD5CheckSum: ACheckSum {
        public static Regex CheckSumRegex = new Regex("^[a-f0-9]{32}$");

        protected override Regex GetCheckSumRegex {
            get { return CheckSumRegex; }
        }

        public MD5CheckSum(string hash) : base(hash.ToLower()) { }


    }
}

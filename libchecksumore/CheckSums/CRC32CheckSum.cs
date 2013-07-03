﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace CheckSuMore {
    public class CRC32CheckSum: ACheckSum {
        public static Regex CheckSumRegex = new Regex("^[0-9A-F]{8}$");

        protected override Regex GetCheckSumRegex {
            get { return CheckSumRegex; }
        }

        public CRC32CheckSum(string hash) : base(hash) { }
    }
}

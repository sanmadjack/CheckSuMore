using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    public class CheckSumTypeMismatchException: Exception {
        public string OffendingHash;
        public string ExpectedFormat;
        public CheckSumTypeMismatchException(string offending_hash, System.Text.RegularExpressions.Regex correct_format): base("The provided checksum hash does not match the expected format") {
            OffendingHash = offending_hash;
            ExpectedFormat = correct_format.ToString();
        }

        public override string Message {
            get {
                StringBuilder output = new StringBuilder();
                output.AppendLine(base.Message);
                output.AppendLine("Hash: " + OffendingHash);
                output.AppendLine("Expected: " + ExpectedFormat);
                return output.ToString();
            }
        }
    }
}

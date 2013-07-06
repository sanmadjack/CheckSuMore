using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace CheckSuMore {
    public abstract class ACheckSum: IEquatable<ACheckSum> {
        public string Hash { get; protected set; }
        public byte[] HashBytes { get; protected set; }

        protected abstract Regex GetCheckSumRegex { get; }

        protected ACheckSum(string hash) {
            if (GetCheckSumRegex.IsMatch(hash)) {
                this.Hash = hash;
            } else {
                throw new CheckSumTypeMismatchException(hash, GetCheckSumRegex);
            }
        }

        public bool Equals(ACheckSum obj) {
            return this.Hash.Equals(obj.Hash);
        }
        public override string ToString() {
            return Hash;
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
namespace CheckSuMore {
    public class MD5CheckSumFile: ACheckSumFile {
        public static Regex CommentRecordRegex = new Regex(";[ ]?(?<comment>.*)$");
        public static Regex FileRecordRegex = new Regex(@"^(?<checksum>[a-f0-9]{32}) \*(?<filename>.+)$");

        protected override System.Text.RegularExpressions.Regex GetCommentRecordRegex {
            get { return CommentRecordRegex; }
        }
        protected override Regex GetFileRecordRegex {
            get { return FileRecordRegex; }
        }
        protected override Regex GetCheckSumRegex {
            get { return MD5CheckSum.CheckSumRegex; }
        }

        public const string Extension = "md5";
        protected override string GetExtension {
            get {
                return Extension;
            }
        }

        public MD5CheckSumFile(FileInfo file)
            : base(file, new MD5CheckSumer()) {
        }

        protected override string RecordComment(CheckSumCommentRecord record) {
            return string.Concat("; ", record.Comment);
        }

        protected override string RecordFile(CheckSumFileRecord record) {
            return String.Concat(record.CheckSum.Hash, " *", record.FileName);
        }

        public override ACheckSum PrepareCheckSum(string hash) {
            return new MD5CheckSum(hash);
        }
    }
}

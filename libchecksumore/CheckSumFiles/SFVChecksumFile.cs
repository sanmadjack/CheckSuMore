using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
namespace CheckSuMore {
    public class SFVChecksumFile: ACheckSumFile {
        public static Regex CommentRecordRegex = new Regex(";[ ]?(?<comment>.*)$");
        public static Regex FileRecordRegex = new Regex("^(?<filename>.+) (?<checksum>[0-9A-F]{8})$");

        protected override System.Text.RegularExpressions.Regex GetCommentRecordRegex {
            get { return CommentRecordRegex; }
        }
        protected override Regex GetFileRecordRegex {
            get { return FileRecordRegex; }
        }
        protected override Regex GetCheckSumRegex {
            get { return CRC32CheckSum.CheckSumRegex; }
        }

        protected override string GetExtension {
            get {
                return "sfv";
            }
        }

        public SFVChecksumFile(string file_path)
            : base(file_path, new CRC32CheckSumer()) {
        }


        protected override string RecordComment(CheckSumCommentRecord record) {
            throw new NotImplementedException();
        }

        protected override string RecordFile(CheckSumFileRecord record) {
            throw new NotImplementedException();
        }

        public override ACheckSum PrepareCheckSum(string hash) {
            throw new NotImplementedException();
        }
    }
}

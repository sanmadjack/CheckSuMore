using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CheckSuMore {
    public class SFVChecksumFile: ACheckSumFile {
        private const string Seperator = "     ";

        public override string Extension {
            get {
                return "sfv";
            }
        }

        public SFVChecksumFile(string file_path)
            : base(file_path) {
        }

        protected override ValidationResult ValidateStream(System.IO.Stream input) {
            throw new NotImplementedException();
        }

        protected override string RecordComment(CheckSumCommentRecord record) {
            throw new NotImplementedException();
        }

        protected override string RecordFile(CheckSumFileRecord record) {
            throw new NotImplementedException();
        }

        protected override List<ACheckSumRecord> Open() {
            List<ACheckSumRecord> records = new List<ACheckSumRecord>();
            ACheckSumRecord record;
            using (TextReader text = File.OpenText()) {
                string line = text.ReadLine();
                while (line != null) {
                    if (line.StartsWith(";")) {
                        record = new CheckSumCommentRecord(line.TrimStart(';'));
                    } else if (String.IsNullOrEmpty(line)) {
                        record = new CheckSumBlankRecord();
                    } else {
                        string[] split = line.Split("   ");

                        record = new CheckSumFileRecord();
                    }
                    records.Add(record);
                }
            }
            return records;
        }
    }
}

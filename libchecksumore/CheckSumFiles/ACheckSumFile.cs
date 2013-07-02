using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CheckSuMore {
    public abstract class ACheckSumFile {
        protected FileInfo File;
        protected List<ACheckSumRecord> Records = new List<ACheckSumRecord>();

        public abstract string Extension { get; }

        protected ACheckSumFile(string file_path) {
            File = new FileInfo(file_path);
            if (File.Exists) {
                Records = Open();
            }
        }

        public void Save() {
            StringBuilder output = new StringBuilder();
            string line;
            foreach (ACheckSumRecord record in Records) {
                line = "";
                if (record is CheckSumFileRecord) {
                    line = RecordFile(record as CheckSumFileRecord);
                } else if (record is CheckSumCommentRecord) {
                    line = RecordComment(record as CheckSumCommentRecord);
                } else if (record is CheckSumBlankRecord) {
                    // Nothing!
                }
                output.AppendLine(line);
            }
        }

        public ValidationResult Validate(FileInfo file) {
            using (Stream input = file.OpenRead()) {
                ValidateStream(input);
            }
            return ValidationResult.Error;
        }
        protected abstract ValidationResult ValidateStream(Stream input);

        protected abstract List<ACheckSumRecord> Open();
        protected abstract string RecordFile(CheckSumFileRecord record);
        protected abstract string RecordComment(CheckSumCommentRecord record);



        
    }
}

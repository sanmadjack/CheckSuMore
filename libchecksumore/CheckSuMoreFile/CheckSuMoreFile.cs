using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CheckSuMore {
    class CheckSuMoreFile {
        private ACheckSumFile csf;

        private Dictionary<string,CheckSuMoreFileItem> Items;
        private Version VersionSupported = new Version(1, 0, 0);

        
        public CheckSuMoreFile(ACheckSumFile csf) {
            this.csf = csf;
            Items = new Dictionary<string, CheckSuMoreFileItem>();
            string line;
            string file_name;
            CheckSuMoreFileItem item;
            foreach (ACheckSumRecord record in csf.GetRecords()) {
                if(record is CheckSumCommentRecord) {
                    line = (record as CheckSumCommentRecord).Comment;
                    file_name = CheckSuMoreFileItemHistoryEntry.GetFileFromInput(line);

                    if (!Items.ContainsKey(file_name)) {
                        Items.Add(file_name, new CheckSuMoreFileItem(line, csf.CheckSumer));
                    } else {
                        item = Items[file_name];
                        item.AddHistoryEntry(line);
                    }
                }
            }
        }

        public void Save() {
            csf.ClearFile();
            csf.AddCommentRecord("CheckSuMore File Format " + VersionSupported.ToString());
            csf.AddCommentRecord(Items.Count + " files listed in this file");
            foreach (string name in Items.Keys) {
                CheckSuMoreFileItem item = Items[name];

            }
            csf.AddCommentRecord("End Of File");
            csf.Save();
        }

		public void Validate() {



		}

        private ValidationResult Validate(FileInfo file) {
            CheckSumFileRecord record;
            ACheckSum checksum;
            if (!csf.HasFileRecordFor(file)) {
                checksum = csf.AddFileRecord(file);
                return ValidationResult.NewFile;
            }
            
            record = FileCheckSums[file.Name];
            using (Stream input = file.OpenRead()) {
                file_checksum = CheckSumer.Generate(input);
            }

            if (record.CheckSum.Equals(file_checksum)) {
                return ValidationResult.Passed;
            } else {
                return ValidationResult.Failed;
            }

        }


    }
}

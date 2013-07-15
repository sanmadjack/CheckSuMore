using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CheckSuMore {
    class CheckSuMoreFileItem: IComparable {
        public FileInfo File { get; protected set; }
        public string FileName { get; protected set; }

		public ValidationResult Result = ValidationResult.Unchecked;

        private List<CheckSuMoreFileItemHistoryEntry> History;

        public CheckSuMoreFileItem(CheckSumCommentRecord record) {
            FileName = AddHistoryEntry(record).File;
        }

        public void Add(CheckSuMoreFileItemHistoryEntry item) {
            History.Add(item);
            History.Sort();
        }

        public override string ToString() {
            return FileName;
        }

        public CheckSuMoreFileItemHistoryEntry AddHistoryEntry(CheckSumCommentRecord record) {
            CheckSuMoreFileItemHistoryEntry entry = new CheckSuMoreFileItemHistoryEntry(record);
            if (FileName != null && FileName != entry.File) {
                throw new Exception("This file is not for this item!");
            }
            History.Add(entry);
            return entry;
        }

		public ValidationResult Validate() {

		}


        public int CompareTo(object obj) {
            return this.ToString().CompareTo(obj.ToString());
        }
    }
}

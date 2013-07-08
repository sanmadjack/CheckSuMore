using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    class CheckSuMoreFileItem: IComparable {
        public string FileName { get; protected set; }

		public ValidationResult Result = ValidationResult.Unchecked;

        private List<CheckSuMoreFileItemHistoryEntry> History;
        private ACheckSumer CheckSumer;

        public CheckSuMoreFileItem(string primer_line, ACheckSumer checksumer) {
            this.CheckSumer = checksumer;
            FileName = AddHistoryEntry(primer_line).File;
        }

        public void Add(CheckSuMoreFileItemHistoryEntry item) {
            History.Add(item);
            History.Sort();
        }

        public override string ToString() {
            return FileName;
        }

        public CheckSuMoreFileItemHistoryEntry AddHistoryEntry(string history_line) {
            CheckSuMoreFileItemHistoryEntry entry = new CheckSuMoreFileItemHistoryEntry(history_line, CheckSumer);
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

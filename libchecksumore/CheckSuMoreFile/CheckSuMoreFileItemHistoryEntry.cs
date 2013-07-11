using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    class CheckSuMoreFileItemHistoryEntry: IComparable<CheckSuMoreFileItemHistoryEntry> {
        public string File { get; protected set; }
        public ACheckSum Hash { get; protected set; }
        public DateTime LastModified { get; protected set; }
        public DateTime LastSeen { get; protected set; }

        private const char FieldSeperator = ';';
        private const char ValueSeperator = '=';
        private const string FileFieldName = "File";
        private const string HashFieldName = "Hash";
        private const string LastModifiedFieldName = "LastModified";
        private const string LastSeenFieldName = "LastSeen";

        public CheckSuMoreFileItemHistoryEntry(CheckSumCommentRecord input) {
            Dictionary<string, string> fields = DecodeInput(input.Comment);
            foreach (string field in fields.Keys) {
                switch (field) {
                    case FileFieldName:
                        File = fields[field];
                        break;
                    case HashFieldName:
                        Hash = input.File.CheckSumer.Interpret(fields[field]);
                        break;
                    case LastModifiedFieldName:
                        LastModified = DateTime.Parse(fields[field]);
                        break;
                    case LastSeenFieldName:
                        LastSeen = DateTime.Parse(fields[field]);
                        break;
                }
            }
        }

        public static string GetFileFromInput(string input) {
            Dictionary<string, string> fields = DecodeInput(input);
            return fields[FileFieldName];
        }

        private static Dictionary<string, string> DecodeInput(string input) {
            Dictionary<string, string> output = new Dictionary<string, string>();
            string[] split = input.Split(FieldSeperator);
            foreach (string field in split) {
                string[] pair = field.Split(ValueSeperator);
                output.Add(pair[0], pair[1]);
            }
            return output;
        }

        public override string ToString() {
            StringBuilder output = new StringBuilder();

            output.Append(FileFieldName);
            output.Append(ValueSeperator);
            output.Append(File);
            output.Append(FieldSeperator);

            output.Append(HashFieldName);
            output.Append(ValueSeperator);
            output.Append(Hash.ToString());
            output.Append(FieldSeperator);

            output.Append(LastModifiedFieldName);
            output.Append(ValueSeperator);
            output.Append(LastModified.ToString("s"));
            output.Append(FieldSeperator);

            output.Append(LastSeenFieldName);
            output.Append(ValueSeperator);
            output.Append(LastSeen.ToString("s"));
            output.Append(FieldSeperator);
            
            return output.ToString();
        }

        public int CompareTo(CheckSuMoreFileItemHistoryEntry obj) {
            return this.File.CompareTo(obj.File);
        }

        
    }
}

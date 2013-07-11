﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
namespace CheckSuMore {
    public abstract class ACheckSumFile {
        protected abstract Regex GetCheckSumRegex { get; }
        protected abstract Regex GetCommentRecordRegex { get; }
        protected abstract string GetExtension { get; }
        protected abstract Regex GetFileRecordRegex { get; }

        public ACheckSumer CheckSumer { get; protected set; }
		public DirectoryInfo Directory {
			get {
				return File.Directory;
			}
		}
        public FileInfo File { get; protected set; }
        protected Dictionary<string, CheckSumFileRecord> FileCheckSumRecords = new Dictionary<string, CheckSumFileRecord>();
        public Dictionary<string, ACheckSum> FileCheckSums {
            get {
                Dictionary<string, ACheckSum> output = new Dictionary<string, ACheckSum>();
                foreach (string name in FileCheckSumRecords.Keys) {
                    output.Add(name, FileCheckSumRecords[name].CheckSum);
                }
                return output;
            }
        }

        protected List<ACheckSumRecord> Records = new List<ACheckSumRecord>();

		// Abstract functions
        public abstract ACheckSum PrepareCheckSum(string hash);
        protected abstract string RecordFile(CheckSumFileRecord record);
        protected abstract string RecordComment(CheckSumCommentRecord record);



        protected ACheckSumFile(FileInfo file, ACheckSumer checksumer) {
            this.File = file;
            if (File.Extension.ToLower().TrimStart('.') != GetExtension.ToLower()) {
                throw new NotSupportedException("File does not have the expected extension. " + GetExtension + " was expected, " + File.Extension + " found");
            }
            this.CheckSumer = checksumer;
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
                
                System.IO.File.WriteAllText(File.FullName,output.ToString());
            }
        }

        public List<ACheckSumRecord> GetRecords() {
            return new List<ACheckSumRecord>(Records);
        }

        protected List<ACheckSumRecord> Open() {
            List<ACheckSumRecord> records = new List<ACheckSumRecord>();
            ACheckSumRecord record = null;
            MatchCollection matches;
            using (TextReader text = File.OpenText()) {
                string line = text.ReadLine();
                while (line != null) {
                    if (string.IsNullOrWhiteSpace(line)) {
                        record = new CheckSumBlankRecord();
                    } else if (GetCommentRecordRegex.IsMatch(line)) {
                        matches = GetCommentRecordRegex.Matches(line);
                        record = new CheckSumCommentRecord(matches[0].Groups["comment"].Value, this);
                    } else if (GetFileRecordRegex.IsMatch(line)) {
                        matches = GetFileRecordRegex.Matches(line);
                        string filename = matches[0].Groups["filename"].Value;
                        string checksum = matches[0].Groups["checksum"].Value;
                        record = new CheckSumFileRecord(filename, this.PrepareCheckSum(checksum));
                        FileCheckSumRecords.Add(filename, record as CheckSumFileRecord);
                    } else {
                        throw new NotSupportedException("A line in this file is not supported");
                    }
                    records.Add(record);
                    line = text.ReadLine();
                }
            }
            return records;
        }

        public bool HasFileRecordFor(FileInfo file) {
            string relative_path = GetPathRelativeToFile(file);
            return FileCheckSumRecords.ContainsKey(relative_path);
        }

        public ACheckSum GetCheckSumForFile(FileInfo file) {
            string relative_path = GetPathRelativeToFile(file);
            return FileCheckSumRecords[relative_path].CheckSum;
        }

        public string GetPathRelativeToFile(FileInfo file) {
            DirectoryInfo this_file_dir = this.File.Directory;

            if (file.FullName.StartsWith(this_file_dir.FullName)) {
                return file.FullName.Substring(this_file_dir.FullName.Length).Trim(Path.DirectorySeparatorChar);
            }
            throw new Exception("File does not appear to be a in the root or any subfolder");
        }
        public void AddCommentRecord(string comment) {
            CheckSumCommentRecord record = new CheckSumCommentRecord(comment);
            Records.Add(record);
        }

        public void ClearFile() {
            Records.Clear();
            FileCheckSumRecords.Clear();
        }


        public ACheckSum AddFileRecord(FileInfo file) {
            string relative_path = GetPathRelativeToFile(file);
            ACheckSum file_checksum;
            CheckSumFileRecord record;

            using (Stream input = file.OpenRead()) {
                file_checksum = CheckSumer.Generate(input);
            }

            record = new CheckSumFileRecord(relative_path, file_checksum);
            FileCheckSumRecords.Add(relative_path, record);
            Records.Add(record);

            return file_checksum;
        }


    }
}

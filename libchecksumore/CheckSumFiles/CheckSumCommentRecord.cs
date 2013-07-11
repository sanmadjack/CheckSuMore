using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    public class CheckSumCommentRecord: ACheckSumRecord {
        public string Comment { get; set; }

        public CheckSumCommentRecord(string comment, ACheckSumFile parent_file): base(parent_file) {
            this.Comment = comment;
        }

        public override string ToString() {
            return Comment;
        }
    }
}

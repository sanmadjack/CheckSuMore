using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    public class CheckSumCommentRecord: ACheckSumRecord {
        public string Comment { get; set; }
        public CheckSumCommentRecord(string comment) {
            this.Comment = comment;
        }
    }
}

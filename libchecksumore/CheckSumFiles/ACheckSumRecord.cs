using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    public abstract class ACheckSumRecord {
        public ACheckSumFile File { get; protected set; }
        protected ACheckSumRecord(ACheckSumFile ParentFile) {
            this.File = ParentFile;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    public enum ValidationResult {
		Unchecked,
        Error,
        Failed,
		Missing,
        Changed,
        New,
		Moved,
        Passed
    }
}

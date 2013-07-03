using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    public abstract class ACheckSumer {
    }
    public abstract class ACheckSumer<T>: ACheckSumer where T : ACheckSum {
        public abstract T Generate(System.IO.Stream input);
    }

}

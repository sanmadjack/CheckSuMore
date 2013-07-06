using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore {
    public abstract class ACheckSumer {
        public abstract ACheckSum Generate(System.IO.Stream input);
        public abstract ACheckSum Interpret(string input);
        protected abstract string TranslateBytesToHash(byte[] input);

    }

}

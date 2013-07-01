using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckSuMore
{
    class Program
    {
        private static CheckSuMore csm = new CheckSuMore();
        static void Main(string[] args)
        {
            csm.CheckFiles("E:\\Docs");
        }
    }
}

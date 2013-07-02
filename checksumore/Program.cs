using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CheckSuMore
{
    class Program
    {
        private static CheckSuMore csm = new CheckSuMore();

        private const string HelpText = "CheckSumMore\n" +
                                        "Copyleft 2013 Matthew Barbour\n" +
                                        "\n" + 
                                        "Args:" +
                                        "-r -R - Operate recursively, automatically descending into subfolders";

        private static bool Recursive = false;

        static void Main(string[] args)
        {
            List<string> paths = new List<string>();
            if (args.Length == 0) {
                Console.Out.WriteLine(HelpText);
                Console.In.ReadLine();
                return;
            } else {
                foreach (string arg in args) {
                    switch (arg) {
                        case "-r":
                        case "-R":
                            Recursive = true;
                            break;
                        default:
                            paths.Add(arg);
                            break;
                    }
                }
            }

            paths.Add("E:\\Docs");

            if (paths.Count == 0) { // If not scan lcoation is specified, the current folder is used
                paths.Add(Environment.CurrentDirectory);
            }
            try {
                foreach (string path in paths) {
                    csm.CheckPath(path, Recursive);
                }
            } catch (Exception e) {
                do {
                    System.Console.Out.WriteLine(e.Message);
                    System.Console.Out.WriteLine(e.StackTrace);
                    e = e.InnerException;
                } while (e != null); 
            }
            System.Console.In.ReadLine();
        }
    }
}

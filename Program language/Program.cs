using System;
using System.IO;

namespace Program_language
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1 || Path.GetExtension(args[0]) != ".pl")
                {
                    throw new ArgumentException("Usage:  <path to this program> file.pl", nameof(args));
                }
                Interpreter interpreter = new();
                interpreter.Interpret(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press enter to close...");
#if !DEBUG
                Console.ReadLine();
#else
                throw;
#endif
            }
        }
    }
}

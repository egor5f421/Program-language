using System;

namespace Program_language
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if ((args.Length is not (1 or 2)) || System.IO.Path.GetExtension(args[0]) is not ".pl")
                {
                    throw new ArgumentException("Usage:  <path to this program> file.pl\n" +
                                                "Flags:  --create                     - Creates a file with the code that the interpreter actually executed.", nameof(args));
                }
                bool createPlRuined = args.Length is 2 && args[1] is "--create";
                Interpreter interpreter = new();
                interpreter.Interpret(args[0], createPlRuined: createPlRuined);
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

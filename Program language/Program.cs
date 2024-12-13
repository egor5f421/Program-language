namespace Program_language
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length is not 1)
                    throw new ArgumentException($"""Usage:  "{Environment.ProcessPath}" <file's path>""");
                Interpreter interpreter = new(File.ReadAllText(args[0]).Replace("\r", ""));
                interpreter.Interpret();
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

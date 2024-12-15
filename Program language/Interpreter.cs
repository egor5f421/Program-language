using Program_language.Exceptions;
using System.Data;
using System.Text.RegularExpressions;
using InvalidOperationException = Program_language.Exceptions.InvalidOperationException;

namespace Program_language
{
    /// <summary>
    /// Interpreter for code execution
    /// </summary>
    public partial class Interpreter
    {
        private readonly string fileContent;
        /// <summary>
        /// Stack
        /// </summary>
        public Stack<long> stack = new();
        /// <summary>
        /// Current function
        /// </summary>
        public Function currentFunction = new(0, 0, []);
        /// <summary>
        /// Functions
        /// </summary>
        public Dictionary<string, Function> functions = [];
        /// <summary>
        /// Lines with a code
        /// </summary>
        public string[] lines = [];

        /// <summary>
        /// Variables declared in the code
        /// </summary>
        public readonly Variables variables = new() { ["R"] = 0 };
        /// <summary>
        /// Register
        /// </summary>
        public long Register
        {
            get => GetValue("R", 0);
            set => SetVariable("R", value);
        }

        internal static Func<long> Input = static () => { while (true) if (long.TryParse(Console.ReadLine(), out long value)) return value; };
        internal static Action<long> Print = Console.WriteLine;

        /// <summary>
        /// Creates an interpreter
        /// </summary>
        /// <param name="Code">Code for interpretation</param>
        public Interpreter(string Code)
        {
            fileContent = Code.Replace("\r", string.Empty);
            fileContent = DeleteComments().Replace(fileContent, string.Empty);
            fileContent = DeleteExtraSpaces().Replace(fileContent, string.Empty);
        }

        private long GetValue(string name, int line) => GetValue(name, out long value)
            ? value
            : throw new InvalidOperationException(string.Format(Excepts.varOrLabelNotExist, name), line);
        private bool GetValue(string name, out long value) =>
            long.TryParse(name, out value) || currentFunction.Arguments.TryGetValue(name, out value) || variables.TryGetValue(name, out value) || TryMathOperation(name, out value);
        private bool TryMathOperation(string operation, out long value)
        {
            foreach (var variable in currentFunction.Arguments.Keys)
                operation = operation.Replace(variable, GetValue(variable, 0).ToString());
            foreach (var variable in variables.Keys)
                operation = operation.Replace(variable, GetValue(variable, 0).ToString());
            try
            {
                value = Convert.ToInt64(new DataTable().Compute(operation, null));
            }
            catch { value = 0; return false; }
            return true;
        }
        private void SetVariable(string name, long value)
        {
            if (currentFunction.Arguments.ContainsKey(name)) { currentFunction.Arguments[name] = value; return; }
            variables[name] = value;
        }

        /// <summary>
        /// Interpret code
        /// </summary>
        /// <param name="commandHandler">Command handler</param>
        /// <param name="pseudoCommandHandler">Pseudo command handler</param>
        /// <param name="newLineSeparator">Line separator</param>
        /// <param name="commandSeparator">Command separator</param>
        public void Interpret(Func<string, int, string[], int>? commandHandler = null,
                              Func<string, string[], string>? pseudoCommandHandler = null,
                              char newLineSeparator = '\n',
                              char commandSeparator = ';')
        {
            commandHandler ??= ExecuteCommand;
            pseudoCommandHandler ??= PseudoOperation;

            string[] oldLines = fileContent.Split(newLineSeparator);//, StringSplitOptions.TrimEntries);
            string[] lines = new string[oldLines.Length];

            for (int i = 0; i < oldLines.Length; i++)
            {
                string[] words = oldLines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (words.Length == 0) continue;

                lines[i] = pseudoCommandHandler(words[0], words[1..^0]);
            }
            this.lines = lines;
#if DEBUG
            File.WriteAllLines("debug.pl", lines);
#endif
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;
                line = line.Trim();

                foreach (string command in line.Split(commandSeparator, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] words = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 0) continue;
                    string operation = words[0];
                    int lineNumber = i + 1;
                    string[] args = words[1..^0];

                    i = commandHandler(operation, lineNumber, args) - 1;
                }
            }
        }

        [GeneratedRegex(@"(//((?!$).)*)|(/\*(((?!\*/).)*)\*/)", RegexOptions.Singleline | RegexOptions.Multiline)]
        private static partial Regex DeleteComments();

        [GeneratedRegex(@" +(?=[+\-/*])|(?<=[+\-/*])\s+(?=.+>)|>")]
        private static partial Regex DeleteExtraSpaces();
    }
}

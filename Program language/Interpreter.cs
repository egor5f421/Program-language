using Program_language.Exceptions;
using System;
using System.IO;
using System.Text.RegularExpressions;
using InvalidOperationException = Program_language.Exceptions.InvalidOperationException;

namespace Program_language
{
    /// <summary>
    /// Interpreter for code execution
    /// </summary>
    public partial class Interpreter
    {
        /// <summary>
        /// Stack
        /// </summary>
        public readonly Stack<long> stack = new();

        /// <summary>
        /// Variables declared in the code
        /// </summary>
        public readonly Variables variables = [];
        /// <summary>
        /// Register
        /// </summary>
        public long Register
        {
            get => variables["R"];
            set => variables["R"] = value;
        }

        internal static Func<long> Input = static () => { while (true) if (long.TryParse(Console.ReadLine(), out long value)) return value; };
        internal static Action<long> Print = Console.WriteLine;

        private long GetValue(string name, int line) => GetValue(name, out long value)
            ? value
            : throw new InvalidOperationException(string.Format(Excepts.varOrLabelNotExist, name), line);
        private bool GetValue(string name, out long value) =>
            long.TryParse(name, out value) || variables.TryGetValue(name, out value);

        internal void Interpret(string filePath,
                                Func<string, int, string[], int>? commandHandler = null,
                                Func<string, string[], string>? pseudoCommandHandler = null,
                                char newLineSeparator = '\n',
                                char commandSeparator = ';',
                                bool createPlRuined = false)
        {
            commandHandler ??= ExecuteCommand;
            pseudoCommandHandler ??= PseudoOperation;

            string fileContent = File.ReadAllText(filePath).Replace("\r", string.Empty);
            fileContent = DeleteComments().Replace(fileContent, string.Empty);
            string[] oldLines = fileContent.Split(newLineSeparator, StringSplitOptions.TrimEntries);
            string[] lines = new string[oldLines.Length];

            for (int i = 0; i < oldLines.Length; i++)
            {
                string[] words = oldLines[i].Split(' ');//, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length == 0) continue;

                lines[i] = pseudoCommandHandler(words[0], words[1..^0]);
            }

            if (createPlRuined) File.WriteAllLines(filePath + "_", lines);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

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
    }
}

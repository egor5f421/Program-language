using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using InvalidOperationException = Program_language.Exceptions.InvalidOperationException;

namespace Program_language
{
    internal partial class Interpreter()
    {
        private long Register
        {
            get => variables["R"];
            set => variables["R"] = value;
        }

        private long[] stack = [];
        private long StackTop
        {
            get
            {
                if (stack.Length == 0)
                    return 0;

                long stackTop = stack[^1];
                stack = stack.SkipLast(1).ToArray();
                return stackTop;
            }
            set
            {
                stack = [.. stack, value];
            }
        }

        private readonly Dictionary<string, long> variables = new() { ["R"] = 0 };

        private static string[] GetFileContent(string filePath)
        {
            string[] fileContent = File.ReadAllLines(filePath);
            return fileContent;
        }

        private void Operation(string operation, int line, params string[] args)
        {
            if (variables.ContainsKey(operation) && args[0] == "=")
            {
                variables[operation] = GetValue(args[1], out long variableValue)
                    ? variableValue
                    : throw new InvalidOperationException("The value of the variable must be a number, a variable, or R", line);
                return;
            }

            operation = operation.ToUpper();
            if (args.Length != 0 && (operation == "ADD"
                                     || operation == "SUB"
                                     || operation == "PUSH"
                                     || operation == "POP"
                                     || operation == "PRINT"))
            {
                throw new InvalidOperationException($"Nothing should go after the {operation} command", line);
            }

            switch (operation)
            {
                case "ADD":
                    Register += StackTop;
                    break;
                case "PUSH":
                    StackTop = Register;
                    break;
                case "POP":
                    Register = StackTop;
                    break;
                case "PRINT":
                    Console.WriteLine(Register);
                    break;
                case "LDI":
                    if (args.Length != 1 || !GetValue(args[0], out long value))
                        throw new InvalidOperationException($"The {operation} command should be followed by number", line);
                    Register = value;
                    break;
                case "VAR":
                    if (args.Length != 3 || args[1] != "=")
                        throw new InvalidOperationException($"Variables are created like this: \"{operation} 'name' = 'value being a number, or a variable, or R'\"", line);
                    if (variables.ContainsKey(args[0]))
                        throw new InvalidOperationException($"The \"{args[0]}\" variable already exists", line);

                    variables[args[0]] = GetValue(args[2], out long variableValue)
                        ? variableValue
                        : throw new InvalidOperationException($"The \"{args[2]}\" variable does not exist", line);
                    break;
                default:
                    throw new InvalidOperationException("This operation does not exist: " + operation, line);
            }
        }

        private bool GetValue(string name, out long value) =>
            long.TryParse(name, out value) || variables.TryGetValue(name, out value);

        internal void Interpret(string filePath)
        {
            string[] lines = GetFileContent(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                line = DeleteComments().Replace(line, "");
                if (string.IsNullOrEmpty(line)) continue;
                string[] words = line.Split(' ');
                Operation(words[0], i + 1, words.Skip(1).ToArray());
                continue;
            }
        }

        [GeneratedRegex("( *//.*)|(/\\*.*\\*/)")]
        private static partial Regex DeleteComments();
    }
}

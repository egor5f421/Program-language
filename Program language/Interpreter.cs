using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Program_language.Exceptions;
using InvalidOperationException = Program_language.Exceptions.InvalidOperationException;

namespace Program_language
{
    internal partial class Interpreter()
    {
        private readonly Stack<long> stack = new();

        private readonly Variables variables = [];
        private long Register
        {
            get => variables["R"];
            set => variables["R"] = value;
        }

        private static bool CheckSyntax(Command command, string[] args, out string syntaxError)
        {
            switch (command)
            {
                case Command.ADD:
                case Command.SUB:
                case Command.DIV:
                case Command.MUL:
                case Command.PUSH:
                case Command.POP:
                case Command.INPUT:
                case Command.PRINT:
                    if (args.Length is not 0)
                    {
                        syntaxError = string.Format(Excepts.noArgs, command); return false;
                    }
                    break;
                case Command.JMP:
                case Command.LABEL:
                    if (args.Length is not 1)
                    {
                        syntaxError = string.Format(Excepts.valueNotExist, command); return false;
                    }
                    break;
                case Command.LDI:
                    if (args.Length != 1)
                    {
                        syntaxError = string.Format(Excepts.valueNotExist, command); return false;
                    }
                    break;
                case Command.VAR:
                    if (args.Length != 3 || args[1] != "=")
                    {
                        syntaxError = string.Format(Excepts.newVarValue, command); return false;
                    }
                    break;
            }
            syntaxError = string.Empty;
            return true;
        }

        private int ExecuteCommand(string operation, int line, params string[] args)
        {
            if (!Enum.TryParse(operation.ToUpper(), out Command command))
            {
                string errorMessage = string.Format(Excepts.operationNotExist, operation);
                throw new InvalidOperationException(errorMessage, line);
            }

            if (!CheckSyntax(command, args, out string syntaxError))
                throw new InvalidOperationException(syntaxError, line);

            switch (command)
            {
                case Command.ADD: Register += stack.Pop(); break;
                case Command.SUB: Register -= stack.Pop(); break;
                case Command.DIV: Register /= stack.Pop(); break;
                case Command.MUL: Register *= stack.Pop(); break;

                case Command.PUSH: stack.Push(Register); break;
                case Command.POP: Register = stack.Pop(); break;

                case Command.INPUT: Register = Input(); break;
                case Command.PRINT: Print(Register); break;

                case Command.LDI: Register = GetValue(args[0], line); break;
                case Command.VAR: variables[args[0]] = GetValue(args[2], line); break;

                case Command.JMP: return (int)GetValue(args[0], line);
                case Command.LABEL: variables[args[0]] = line; break;
            }

            return line;
        }

        public static bool CheckPseudoSyntax(PseudoCommand command, string[] args)
        {
            switch (command)
            {
                case PseudoCommand.ADD:
                case PseudoCommand.SUB:
                case PseudoCommand.MUL:
                case PseudoCommand.DIV:
                    if (args.Length != 1) return false;
                    break;

                case PseudoCommand.PUSH:
                case PseudoCommand.POP:
                    if (args.Length != 1) return false;
                    break;
            }
            return true;
        }

        public static string PseudoOperation(string operation, params string[] args)
        {
            operation = operation switch
            {
                "+" => PseudoCommand.ADD.ToString(),
                "-" => PseudoCommand.SUB.ToString(),
                "*" => PseudoCommand.MUL.ToString(),
                "/" => PseudoCommand.DIV.ToString(),
                _ => operation,
            };
            if ( args.Length == 2 && args[0].Equals("=", StringComparison.CurrentCultureIgnoreCase)) {
                return $"VAR {operation} = {args[1]};";
            }
            string old = operation + (args.Length != 0 ? " " + string.Join(' ', args) : string.Empty);
            return !Enum.TryParse(operation.ToUpper(), out PseudoCommand command) || !CheckPseudoSyntax(command, args)
                ? old
                : command switch
            {
                PseudoCommand.ADD or PseudoCommand.SUB or PseudoCommand.MUL or PseudoCommand.DIV => $"VAR TEMP = R; LDI {args[0]}; PUSH; LDI TEMP; {command};",
                PseudoCommand.PUSH => $"VAR TEMP = R; LDI {args[0]}; {command}; LDI TEMP;",
                PseudoCommand.POP => $"VAR TEMP = R; {command}; {args[0]} = R; LDI TEMP;",
                _ => old,
            };
        }

        public static Func<long> Input = static () => { while (true) if (long.TryParse(Console.ReadLine(), out long value)) return value; };
        public static Action<long> Print = Console.WriteLine;

        private long GetValue(string name, int line) =>
            GetValue(name, out long value)
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

            if (createPlRuined) File.WriteAllLines(Path.ChangeExtension(filePath, ".pl_ruined"), lines);
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

        [GeneratedRegex(@"(//((?!$).)*)|(/\*(((?!\*/).)*)\*/)", RegexOptions.Multiline | RegexOptions.Singleline)]
        private static partial Regex DeleteComments();
    }
}

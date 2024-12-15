using Program_language.Exceptions;
using InvalidOperationException = Program_language.Exceptions.InvalidOperationException;

namespace Program_language
{
    public partial class Interpreter
    {
        /// <summary>
        /// Replace old line to new line
        /// </summary>
        /// <param name="operation">Operation</param>
        /// <param name="args">Args</param>
        /// <returns>New line</returns>
        public static string PseudoOperation(string operation, params string[] args)
        {
            operation = operation switch
            {
                "+" => PseudoCommand.ADD.ToString(),
                "-" => PseudoCommand.SUB.ToString(),
                "*" => PseudoCommand.MUL.ToString(),
                "/" => PseudoCommand.DIV.ToString(),
                _ => operation.ToUpper(),
            };
            if (args.Length == 2 && args[0].Equals("=", StringComparison.CurrentCultureIgnoreCase))
                return $"VAR {operation} = {args[1]};";
            string old = operation + (args.Length != 0 ? " " + string.Join(' ', args) : string.Empty);
            return !Enum.TryParse(operation, out PseudoCommand command) || !Syntax.CheckPseudoSyntax(command, args)
                ? old
                : command switch
                {
                    PseudoCommand.ADD or PseudoCommand.SUB or PseudoCommand.MUL or PseudoCommand.DIV => $"VAR TEMP = R; LDI {args[0]}; PUSH; LDI TEMP; {command};",
                    PseudoCommand.PUSH => $"VAR TEMP = R; LDI {args[0]}; {command}; LDI TEMP;",
                    PseudoCommand.POP => $"VAR TEMP = R; {command}; {args[0]} = R; LDI TEMP;",
                    _ => old,
                };
        }

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="operation">Operation</param>
        /// <param name="line">Line's number</param>
        /// <param name="args">Args</param>
        /// <returns>New line's number</returns>
        /// <exception cref="InvalidOperationException">Throw if invalid operation</exception>
        public int ExecuteCommand(string operation, int line, params string[] args)
        {
            if (operation == "{") return line;
            if (operation == "}") operation = "END";
            if (!Enum.TryParse(operation, out Command command))
                throw new InvalidOperationException(string.Format(Excepts.operationNotExist, operation), line);

            if (!Syntax.CheckSyntax(command, args, out string syntaxError, functions))
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
                case Command.VAR: SetVariable(args[0], GetValue(args[2], line)); break;

                case Command.JMP: return (int)GetValue(args[0], line);
                case Command.LABEL: SetVariable(args[0], line); break;

                case Command.FUNC: functions[args[0]] = new(line, 0, new(args[1..^0].ToDictionary(s => s, s => 0L)), lines); return functions[args[0]].ToEnd(false);
                case Command.END: return currentFunction.Return();
                case Command.CALL: return functions[args[0]].Call(line, out currentFunction, args[1..^0].Select(s => GetValue(s, line)));
            }

            return line;
        }
    }
}
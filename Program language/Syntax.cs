using Program_language.Exceptions;

namespace Program_language
{
    internal static class Syntax
    {
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

        public static bool CheckSyntax(Command command, string[] args, out string syntaxError)
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
            syntaxError = "";
            return true;
        }
    }
}
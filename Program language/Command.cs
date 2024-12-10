namespace Program_language
{
    /*public static class Commands
    {
        public const string ADD = "ADD";
        public const string SUB = "SUB";
        public const string DIV = "DIV";
        public const string MUL = "MUL";

        public const string PUSH = "PUSH";
        public const string POP = "POP";

        public const string INPUT = "INPUT";
        public const string PRINT = "PRINT";

        public const string LDI = "LDI";
        public const string VAR = "VAR";

        public const string LABEL = "LABEL";
        public const string JMP = "JMP";
    }*/

    public enum Command
    {
        ADD,
        SUB,
        MUL,
        DIV,

        PUSH,
        POP,

        INPUT,
        PRINT,

        LDI,
        VAR,

        LABEL,
        JMP,
    }
    public enum PseudoCommand
    {
        ADD,
        SUB,
        MUL,
        DIV,

        PUSH,
        POP,
    }
}

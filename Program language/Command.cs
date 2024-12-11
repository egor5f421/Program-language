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

    /// <summary>
    /// Command
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// Addition
        /// </summary>
        ADD,
        /// <summary>
        /// Subtraction
        /// </summary>
        SUB,
        /// <summary>
        /// Multiplication
        /// </summary>
        MUL,
        /// <summary>
        /// Division
        /// </summary>
        DIV,

        /// <summary>
        /// Unloading to the stack
        /// </summary>
        PUSH,
        /// <summary>
        /// Unloading from the stack
        /// </summary>
        POP,

        /// <summary>
        /// Input
        /// </summary>
        INPUT,
        /// <summary>
        /// Print
        /// </summary>
        PRINT,

        /// <summary>
        /// Writing to the register
        /// </summary>
        LDI,
        /// <summary>
        /// Creating a variable
        /// </summary>
        VAR,

        /// <summary>
        /// Creating a label
        /// </summary>
        LABEL,
        /// <summary>
        /// Switching to line
        /// </summary>
        JMP,
    }
    /// <summary>
    /// Pseudo command
    /// </summary>
    public enum PseudoCommand
    {
        /// <summary>
        /// Addition
        /// </summary>
        ADD,
        /// <summary>
        /// Subtraction
        /// </summary>
        SUB,
        /// <summary>
        /// Multiplication
        /// </summary>
        MUL,
        /// <summary>
        /// Division
        /// </summary>
        DIV,

        /// <summary>
        /// Unloading to the stack
        /// </summary>
        PUSH,
        /// <summary>
        /// Unloading from the stack
        /// </summary>
        POP,
    }
}

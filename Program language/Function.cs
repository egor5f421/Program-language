namespace Program_language
{
    public class Function
    {
        public int LineNumber { get; set; }
        public int ReturnToLineNumber { get; set; }

        public Function(int LineNumber/*, IDictionary<string, long>? Arguments*/, int ReturnToLineNumber)
        {
            this.LineNumber = LineNumber;
            this.ReturnToLineNumber = ReturnToLineNumber;
        }

        public int Call(int currentLine, out Function newCurrentFunction)
        {
            ReturnToLineNumber = currentLine;
            newCurrentFunction = this;
            return LineNumber;
        }
        public int Return() => ReturnToLineNumber;
        public static int ToEnd(string[] lines, int currentLine, bool executeEnd = true) => Array.IndexOf(lines, Command.END.ToString(), currentLine) + (executeEnd ? 0 : 1);
    }
}

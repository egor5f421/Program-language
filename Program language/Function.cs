namespace Program_language
{
    /// <summary>
    /// A class representing a function
    /// </summary>
    public class Function
    {
        /// <summary>
        /// The line with the function declaration
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// The line that the function will return to when it finishes working
        /// </summary>
        public int ReturnToLineNumber { get; set; }

        /// <summary>
        /// Designer
        /// </summary>
        /// <param name="LineNumber">The line with the function declaration</param>
        /// <param name="ReturnToLineNumber">The line that the function will return to when it finishes working</param>
        public Function(int LineNumber/*, IDictionary<string, long>? Arguments*/, int ReturnToLineNumber)
        {
            this.LineNumber = LineNumber;
            this.ReturnToLineNumber = ReturnToLineNumber;
        }

        /// <summary>
        /// Starts the function
        /// </summary>
        /// <param name="currentLine">The current line</param>
        /// <param name="newCurrentFunction">The parameter that will set the current function</param>
        /// <returns>The line to go to to run the function</returns>
        public int Call(int currentLine, out Function newCurrentFunction)
        {
            ReturnToLineNumber = currentLine;
            newCurrentFunction = this;
            return LineNumber;
        }
        /// <summary>
        /// Returns to the string from which the function was called
        /// </summary>
        /// <returns>The string from which the function was called</returns>
        public int Return() => ReturnToLineNumber;
        /// <summary>
        /// Move to the end of the function
        /// </summary>
        /// <param name="lines">Lines with a code</param>
        /// <param name="currentLine">The current line</param>
        /// <param name="executeEnd">Run the END command</param>
        /// <returns>The line to go to</returns>
        public static int ToEnd(string[] lines, int currentLine, bool executeEnd = true) => Array.IndexOf(lines, Command.END.ToString(), currentLine) + (executeEnd ? 0 : 1);
    }
}

#pragma warning disable IDE0290
using Program_language.Exceptions;
using System.Linq;
using InvalidOperationException = Program_language.Exceptions.InvalidOperationException;

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
        /// Function arguments
        /// </summary>
        public Variables Arguments { get; set; }
        private CurlyBraces CurlyBraces { get; }

        /// <summary>
        /// Designer
        /// </summary>
        /// <param name="LineNumber">The line with the function declaration</param>
        /// <param name="ReturnToLineNumber">The line that the function will return to when it finishes working</param>
        /// <param name="Arguments">Function arguments</param>
        /// <param name="lines">Lines with a code</param>
        public Function(int LineNumber, int ReturnToLineNumber, Variables Arguments, string[] lines)
        {
            this.LineNumber = LineNumber;
            this.ReturnToLineNumber = ReturnToLineNumber;
            this.Arguments = Arguments;
            CurlyBraces = new(LineNumber - 1, lines);
        }
        /// <summary>
        /// Designer
        /// </summary>
        /// <param name="LineNumber">The line with the function declaration</param>
        /// <param name="ReturnToLineNumber">The line that the function will return to when it finishes working</param>
        /// <param name="lines">Lines with a code</param>
        public Function(int LineNumber, int ReturnToLineNumber, string[] lines) : this(LineNumber, ReturnToLineNumber, [], lines) { }

        /// <summary>
        /// Starts the function
        /// </summary>
        /// <param name="currentLine">The current line</param>
        /// <param name="newCurrentFunction">The parameter that will set the current function</param>
        /// <param name="arguments">Function arguments</param>
        /// <returns>The line to go to to run the function</returns>
        public int Call(int currentLine, out Function newCurrentFunction, IEnumerable<long> arguments)
        {
            ReturnToLineNumber = currentLine;
            newCurrentFunction = this;
            if (arguments.Count() != Arguments.Count)
                throw new InvalidOperationException(Excepts.wrongNumberOfArguments, currentLine);
            for (int i = 0; i < Arguments.Count; i++)
                Arguments[Arguments.ElementAt(i).Key] = arguments.ToArray()[i];
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
        /// <param name="executeEnd">Run the END command</param>
        /// <returns>The line to go to</returns>
        public int ToEnd(bool executeEnd = true) => CurlyBraces.CloseLine + (executeEnd ? 0 : 1);//return Array.IndexOf(lines, Command.END.ToString(), currentLine) + (executeEnd ? 0 : 1);
    }
}

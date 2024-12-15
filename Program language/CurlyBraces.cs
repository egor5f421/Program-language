#pragma warning disable IDE0290
using Program_language.Exceptions;

namespace Program_language
{
    /// <summary>
    /// A class representing curly braces
    /// </summary>
    public class CurlyBraces
    {
        private readonly IEnumerable<string> lines;

        /// <summary>
        /// Designer
        /// </summary>
        /// <param name="OpenLine">The line with the opening bracket</param>
        /// <param name="lines">Lines with a code</param>
        public CurlyBraces(int OpenLine, IEnumerable<string> lines)
        {
            this.lines = lines;
            this.OpenLine = OpenLine;
        }

        /// <summary>
        /// The line with the opening bracket
        /// </summary>
        public int OpenLine { get; init; }
        /// <summary>
        /// A line with a closing bracket
        /// </summary>
        public int CloseLine
        {
            get
            {
                int count = 0;
                int line = OpenLine;
                do
                {
                    line++;
                    if (line >= lines.Count()) throw new CurlBracesException(false);
                    if (lines.ElementAt(line) is "{") count++;
                    if (lines.ElementAt(line) is "}") count--;
                } while (count != 0);
                return line;
            }
        }
    }
}

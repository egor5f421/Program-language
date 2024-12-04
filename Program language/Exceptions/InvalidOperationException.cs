using System;

namespace Program_language.Exceptions
{
    [Serializable]
    public class InvalidOperationException(string message, int line) : Exception($"{message.Trim()} (Line {line})")
    {
        public int line = line;

        public InvalidOperationException(int line) : this("Invalid operation", line) { }
    }
}

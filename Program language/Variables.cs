using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Program_language
{
    public class Variables : Dictionary<string, long>
    {
        public new long this[string key]
        {
            get
            {
                return key.Equals(Command.INPUT.ToString(), StringComparison.CurrentCultureIgnoreCase) ? Interpreter.Input() : base[key];
            }
            set
            {
                if (key.Equals(Command.PRINT.ToString(), StringComparison.CurrentCultureIgnoreCase)) Interpreter.Print(value);
                base[key] = value;
            }
        }

        public Variables() : base([new("R", 0)]) { }

        public new bool TryGetValue(string key, [MaybeNullWhen(false)] out long value)
        {
            if (key.Equals(Command.INPUT.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                value = Interpreter.Input();
                return true;
            }
            return base.TryGetValue(key, out value);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Program_language
{
    /// <summary>
    /// A class for storing variables
    /// </summary>
    public class Variables : Dictionary<string, long>
    {
        /// <inheritdoc/>
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

        internal Variables() : base([new("R", 0)]) { }

        /// <inheritdoc/>
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

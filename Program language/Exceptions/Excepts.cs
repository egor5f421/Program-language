namespace Program_language.Exceptions
{
    internal static class Excepts
    {
        //public const string variableValue = "The value of the variable must be a number, a variable, or R";
        public const string noArgs = "Nothing should go after the {0} command";
        //public const string numOrVar = "The {0} command should be followed by number, or variable";
        public const string newVarValue = "Variables are created like this: \"{0} 'name' = 'value'\"";
        //public const string varOrLabelExist = "The variable or label \"{0}\" already exists";
        public const string varOrLabelNotExist = "The variable or label \"{0}\" does not exist";
        public const string operationNotExist = "This operation does not exist: {0}";
        public const string valueNotExist = "The command {0} is followed by the value";
        public const string commentInComment = "You cannot create a comment in a comment";
    }
}

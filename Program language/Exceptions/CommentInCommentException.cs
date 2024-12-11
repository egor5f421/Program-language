using System;

namespace Program_language.Exceptions
{
    [Serializable]
    internal class CommentInCommentException() : Exception($"{Excepts.commentInComment.Trim()}") { }
}

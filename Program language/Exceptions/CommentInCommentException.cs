using System;

namespace Program_language.Exceptions
{
    [Serializable]
    public class CommentInCommentException() : Exception($"{Excepts.commentInComment.Trim()}") { }
}

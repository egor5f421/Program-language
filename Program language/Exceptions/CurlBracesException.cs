namespace Program_language.Exceptions
{
    [Serializable]
    internal class CurlBracesException(bool openOrClose) : Exception($"{(openOrClose ? Excepts.curlBracesNotOpened : Excepts.curlBracesNotClosed)}");
}

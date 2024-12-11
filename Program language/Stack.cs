using System.Linq;

namespace Program_language
{
    /// <summary>
    /// Stack
    /// </summary>
    /// <typeparam name="T">Stack's values</typeparam>
    public struct Stack<T>
    {
        private T?[] _stack;
        /// <summary>
        /// Unloading to the stack
        /// </summary>
        public void Push(T item) => _stack = [.. _stack, item];
        /// <summary>
        /// Unloading from the stack
        /// </summary>
        public readonly T? Pop() => _stack.Length != 0 ? _stack.Last() : default;
    }
}

using System;

namespace JsonStreamLogger.Internal
{
    internal class NullDisposable : IDisposable
    {
        public static readonly IDisposable Instance = new NullDisposable();

        public void Dispose() { }
    }
}

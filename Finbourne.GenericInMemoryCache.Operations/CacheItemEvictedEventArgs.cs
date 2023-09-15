using Finbourne.GenericInMemoryCache.Abstractions;

namespace Finbourne.GenericInMemoryCache
{
    public class CacheItemEvictedEventArgs : EventArgs, ICacheItemEvictedEventArgs
    {
        public object Key { get; }
        public object Value { get; }

        public CacheItemEvictedEventArgs(object key, object value)
        {
            Key = key;
            Value = value;
        }

    }
}

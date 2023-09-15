
namespace Finbourne.GenericInMemoryCache.Abstractions
{
    public interface ICacheItemEvictedEventArgs
    {
        public object Key { get; }
        public object Value { get; }

    }
}

using System.Threading;
using System.Threading.Tasks;

namespace JsonStreamLogger.Serialization
{
    public interface IEntryWriter
    {
        ValueTask WriteEntryAsync(in LogEntry entry, CancellationToken cancellationToken);
    }
}

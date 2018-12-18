using System.Threading;
using System.Threading.Tasks;

namespace JsonStreamLogger.Serialization
{
    public interface IEntryWriter
    {
        ValueTask WriteEntryAsync(LogEntry entry, CancellationToken cancellationToken);
    }
}

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using JsonStreamLogger.Serialization;

namespace JsonStreamLogger.Internal
{
    internal class JsonStreamLoggerBroker : IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private readonly Channel<LogEntry> _channel;
        private readonly Stream _stream;
        private readonly Func<Stream, IEntryWriter> _writerFactory;
        private readonly TimeSpan _shutdownDelay;

        private int _isRunning;
        private Task? _writerTask;

        public JsonStreamLoggerBroker(Stream stream, int bufferSize, Func<Stream, IEntryWriter> writerFactory, TimeSpan shutdownDelay)
        {
            _stream = stream;
            _writerFactory = writerFactory;
            _shutdownDelay = shutdownDelay;

            _cts = new CancellationTokenSource();
            _channel = Channel.CreateBounded<LogEntry>(new BoundedChannelOptions(bufferSize)
            {
                SingleReader = true,
                SingleWriter = false,
                AllowSynchronousContinuations = true,
                FullMode = BoundedChannelFullMode.Wait,
            });
        }

        public void Post(in LogEntry entry)
        {
            while (!_channel.Writer.TryWrite(entry))
            {
                Thread.Sleep(1);
            }
        }

        public void EnsureRunning()
        {
            if (Interlocked.CompareExchange(ref _isRunning, 1, 0) != 0)
            {
                return;
            }

            _writerTask = Task.Run(async () =>
            {
                var writer = _writerFactory(_stream);

#if NETCOREAPP3_0
                await foreach (var entry in _channel.Reader.ReadAllAsync(_cts.Token))
                {
                    try
                    {
                        await writer.WriteEntryAsync(entry, _cts.Token);
                    }
                    catch (OperationCanceledException) { }
                    catch (Exception) { }
                }
#else
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        if (!await _channel.Reader.WaitToReadAsync(_cts.Token))
                        {
                            return;
                        }

                        while (_channel.Reader.TryRead(out var entry))
                        {
                            await writer.WriteEntryAsync(in entry, _cts.Token);
                        }
                    }
                    catch (OperationCanceledException) { }
                    catch (Exception) { }
                }
#endif
            }, _cts.Token);
        }

        public void Dispose()
        {
            _cts.CancelAfter(_shutdownDelay);

            try
            {
                _writerTask?.Wait();
            }
            catch (AggregateException)
            { }
        }
    }
}

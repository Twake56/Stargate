using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

public class SQLiteLoggerProvider : ILoggerProvider
{
    private readonly SQLiteLoggerConfiguration _config;
    private readonly ConcurrentDictionary<string, SQLiteLogger> _loggers = new ConcurrentDictionary<string, SQLiteLogger>();

    public SQLiteLoggerProvider(IOptions<SQLiteLoggerConfiguration> config)
    {
        _config = config.Value;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new SQLiteLogger(name, GetCurrentConfig));
    }

    private SQLiteLoggerConfiguration GetCurrentConfig() => _config;

    public void Dispose()
    {
        _loggers.Clear();
    }
}

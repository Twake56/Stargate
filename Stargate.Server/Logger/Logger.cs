using System;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

public class SQLiteLogger : ILogger
{
    private readonly string _name;
    private readonly Func<SQLiteLoggerConfiguration> _getCurrentConfig;

    public SQLiteLogger(string name, Func<SQLiteLoggerConfiguration> getCurrentConfig)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _getCurrentConfig = getCurrentConfig ?? throw new ArgumentNullException(nameof(getCurrentConfig));
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var config = _getCurrentConfig();
        if (config.LogLevel > logLevel)
        {
            return;
        }

        var message = formatter(state, exception);
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        var logRecord = $"[{logLevel}] {message}";
        if (exception != null)
        {
            logRecord += Environment.NewLine + exception;
        }
        
        SaveLog(logLevel.ToString(), logRecord);
    }

    private void SaveLog(string logLevel, string message)
    {
        //using var connection = new SQLiteConnection("Data Source=starbase.db");
        //connection.Open();

        //var getLogs = connection.CreateCommand();
        //getLogs.CommandText = "SELECT * FROM Log";
        //using var logs = getLogs.ExecuteReader();

        //while (logs.Read())
        //{
        //    var id = logs["Id"];
        //    var level = logs["LogLevel"];
        //    var logMessage = logs["Message"];
        //    var logDate = logs["date"];
        //}

        //var command = connection.CreateCommand();
        //command.CommandText = "INSERT INTO Log (LogLevel, Message, Date) VALUES (@logLevel, @message, @date)";
        //command.Parameters.AddWithValue("@logLevel", logLevel);
        //command.Parameters.AddWithValue("@message", message);
        //command.Parameters.AddWithValue("@date", DateTime.UtcNow);

        //command.ExecuteNonQuery();
    }
}


public class SQLiteLoggerConfiguration
{
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}

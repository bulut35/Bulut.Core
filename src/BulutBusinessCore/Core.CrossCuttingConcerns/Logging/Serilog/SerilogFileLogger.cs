﻿using BulutBusinessCore.Core.CrossCuttingConcerns.Logging.Configurations;
using Serilog;

namespace BulutBusinessCore.Core.CrossCuttingConcerns.Logging.Serilog;

public class SerilogFileLogger : SerilogLoggerServiceBase
{
    public SerilogFileLogger(FileLogConfiguration configuration)
        : base(logger: null!)
    {
        Logger = new LoggerConfiguration()
            .WriteTo.File(
                path: $"{Directory.GetCurrentDirectory() + configuration.FolderPath}.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: null,
                fileSizeLimitBytes: 5000000,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
            )
            .CreateLogger();
    }
}

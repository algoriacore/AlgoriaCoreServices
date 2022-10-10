using System;
using System.Collections.Generic;

namespace AlgoriaCore.Domain.Interfaces.Logger
{
    public interface ICoreLogger
    {
        void LogCritical(string message, object parameters);
        void LogCritical(string message, Dictionary<string, string> args = null);
        void LogCritical(Exception exception, string message, Dictionary<string, string> args = null);

        void LogDebug(string message, object parameters);
        void LogDebug(string message, Dictionary<string, string> args = null);
        void LogDebug(Exception exception, string message, Dictionary<string, string> args = null);

        void LogError(string message, object parameters);
        void LogError(string message, Dictionary<string, string> args = null);
        void LogError(Exception exception, string message, Dictionary<string, string> args = null);

        void LogInformation(string message, object parameters);
        void LogInformation(string message, Dictionary<string, string> args = null);
        void LogInformation(Exception exception, string message, Dictionary<string, string> args = null);

        void LogTrace(string message, object parameters);
        void LogTrace(string message, Dictionary<string, string> args = null);
        void LogTrace(Exception exception, string message, Dictionary<string, string> args = null);

        void LogWarning(string message, object parameters);
        void LogWarning(string message, Dictionary<string, string> args = null);
        void LogWarning(Exception exception, string message, Dictionary<string, string> args = null);
    }
}

using System;

namespace XApplicationCore
{
    /// <summary>
    /// This interface defines a localization service.
    /// </summary>
    public interface ILogService : IService
    {
        /// <summary>
        /// This method logs a message at debug level.
        /// </summary>
        /// <param name="pMessageToLog">The message to log.</param>
        void LogDebug(string pMessageToLog);

        /// <summary>
        // This method logs an exception at debug level.
        /// </summary>
        /// <param name="pExceptionToLog">The exception to log.</param>
        void LogDebug(Exception pExceptionToLog);

        /// <summary>
        // This method logs an object at debug level.
        /// </summary>
        /// <param name="pObjectToLog">The object to log.</param>
        void LogDebug(Object pObjectToLog);

        /// <summary>
        /// This method logs a message at info level.
        /// </summary>
        /// <param name="pMessageToLog">The message to log.</param>
        void LogInfo(string pMessageToLog);

        /// <summary>
        /// This method logs a message at info level.
        /// </summary>
        /// <param name="pExceptionToLog">The message to log.</param>
        void LogInfo(Exception pExceptionToLog);

        /// <summary>
        /// This method logs a message at info level.
        /// </summary>
        /// <param name="pObjectToLog">The message to log.</param>
        void LogInfo(Object pObjectToLog);

        /// <summary>
        /// This method logs a message at performance level.
        /// </summary>
        /// <param name="pMessageToLog">The message to log.</param>
        void LogPerformance(string pMessageToLog);

        /// <summary>
        /// This method logs a message at performance level.
        /// </summary>
        /// <param name="pExceptionToLog">The exception to log.</param>
        void LogPerformance(Exception pExceptionToLog);

        /// <summary>
        /// This method logs a message at performance level.
        /// </summary>
        /// <param name="pObjectToLog">The performance to log.</param>
        void LogPerformance(Object pObjectToLog);

        /// <summary>
        /// This method logs a message at warn level.
        /// </summary>
        /// <param name="pMessageToLog">The message to log.</param>
        void LogWarn(string pMessageToLog);

        /// <summary>
        /// This method logs a message at warn level.
        /// </summary>
        /// <param name="pExceptionToLog">The exception to log.</param>
        void LogWarn(Exception pExceptionToLog);

        /// <summary>
        /// This method logs a message at warn level.
        /// </summary>
        /// <param name="pObjectToLog">The object to log.</param>
        void LogWarn(Object pObjectToLog);

        /// <summary>
        /// This method logs a message at error level.
        /// </summary>
        /// <param name="pMessageToLog">The message to log.</param>
        void LogError(string pMessageToLog);

        /// <summary>
        /// This method logs a message at error level.
        /// </summary>
        /// <param name="pExceptionToLog">The exception to log.</param>
        void LogError(Exception pExceptionToLog);

        /// <summary>
        /// This method logs a message at error level.
        /// </summary>
        /// <param name="pObjectToLog">The object to log.</param>
        void LogError(Object pObjectToLog);

        /// <summary>
        /// This method logs a message at fatal level.
        /// </summary>
        /// <param name="pMessageToLog">The message to log.</param>
        void LogFatal(string pMessageToLog);

        /// <summary>
        /// This method logs a message at fatal level.
        /// </summary>
        /// <param name="pObjectToLog">The exception to log.</param>
        void LogFatal(Exception pObjectToLog);

        /// <summary>
        /// This method logs a message at fatal level.
        /// </summary>
        /// <param name="pObjectToLog">The object to log.</param>
        void LogFatal(Object pObjectToLog);
    }
}

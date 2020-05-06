using System;
using UnityEngine;

namespace Networking.Framework.Utils
{
    /// <summary>
    /// Custom logger
    /// </summary>
    public static class Logger
    {
        public static void LogError(string message, NetworkLogType networkLogType = NetworkLogType.Other)
        {
            Log(message, LogType.Error, networkLogType);
        }

        public static void LogWarning(string message, NetworkLogType networkLogType = NetworkLogType.Other)
        {
            Log(message, LogType.Warning, networkLogType);
        }

        public static void Log(string message, NetworkLogType networkLogType = NetworkLogType.Other)
        {
            Log(message, LogType.Log, networkLogType);
        }

        private static void Log(string message, LogType logType = LogType.Log, NetworkLogType networkLogType = NetworkLogType.Other)
        {
            if (networkLogType == NetworkLogType.Broadcasting && !Config.PrintBroadcastingDetailsToConsole)
            {
                return;
            }

            switch (logType)
            {
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.Log:
                    Debug.Log(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }
    }
}
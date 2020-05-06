namespace Networking.Framework.Utils
{
    /// <summary>
    /// Static configuration variables
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Logging can have significant overhead for real-time broadcasting
        /// </summary>
        public static bool PrintBroadcastingDetailsToConsole = false;
        public static uint ProtocolVersion = 1;
        public const int ReconnectIntevalMs = 500;
    }
}
using System.Net;

namespace Networking.Framework
{
    /// <summary>
    /// Describes connection details
    /// </summary>
    public struct ConnectionInfo
    {
        public IPAddress IpAddress { get; }
        public ushort Port { get; }

        public ConnectionInfo(IPAddress ipAddress, ushort port)
        {
            IpAddress = ipAddress;
            Port = port;
        }
    }
}
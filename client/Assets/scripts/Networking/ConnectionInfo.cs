using System.Net;

namespace Networking
{
    /// <summary>
    ///
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
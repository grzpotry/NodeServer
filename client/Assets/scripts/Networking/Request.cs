namespace Domain.Networking
{

    //TODO: these protocol-based structures should be generated eg. by protobuff to provide platform portability
    public enum RequestCode : byte
    {
        Handshake = 0,
        Login = 1,
        Register = 2,
        //...
    }

    public enum ResponseCode : byte
    {

    }

    public struct Response
    {
        public Header Header { get; }
        public RequestCode RequestCode { get; }
        public ushort ResponseCode { get; }
        public byte[] Payload { get; }
    }

    public struct Request
    {
        public Header Header { get; }
        public RequestCode RequestCode { get; }
        public byte[] Payload { get; }
    }


    public struct Header
    {
        public ushort PayloadSize;
    }


}
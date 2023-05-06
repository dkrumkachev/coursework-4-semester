using System.Net.Sockets;

namespace Common
{
    public static class Constants
    {
        public const AddressFamily AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork; 
        public const SocketType SocketType = System.Net.Sockets.SocketType.Stream;
        public const ProtocolType ProtocolType = System.Net.Sockets.ProtocolType.Tcp;
        public static int TransferBlockSize { get; } = 1024;
        public static int ServerPort { get; } = 4886;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class SocketMethods
    {
        public static bool IsConnected(Socket socket)
        {
            const int TimeToWait = 500;
            return !(socket.Poll(TimeToWait, SelectMode.SelectRead) && socket.Available == 0);
        }
    }
}

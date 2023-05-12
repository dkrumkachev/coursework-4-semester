using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
            
        }
    }
}

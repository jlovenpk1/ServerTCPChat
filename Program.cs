using System;
using ClientTCPServer.Server;

namespace ClientTCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server.Server().Start();
        }
    }
}

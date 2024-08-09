using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.Sockets.Kcp.Simple;
using System.Threading.Tasks;
using MemoryPack;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
 
            GameServer server = new GameServer();
            server.Start();

            while (true)
            {
                server.Update();
            }
            
        }
        
        
    }
    
    
    

}
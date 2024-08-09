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
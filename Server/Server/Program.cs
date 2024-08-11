class Program
{
    public static bool isRunning = true;
    static void Main(string[] args)
    {
        ConsoleCommandListener commandListener = new ConsoleCommandListener();
        commandListener.Start();
        
        GameServer server = new GameServer();
        server.Init();
        server.Start();

        while (isRunning)
        {
            server.Update();
        }
        
        server.Dispose();
    }
    
}
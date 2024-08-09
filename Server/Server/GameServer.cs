using HotUpdate.Network;

namespace TestServer;

public class GameServer:IDisposable
{
    private  NetworkService m_networkService;
    public GameServer()
    {
        
    }

    public void Start()
    {
        NetworkDispatcher networkDispatcher = new NetworkDispatcher();
        m_networkService = new NetworkService(40001, networkDispatcher);
        m_networkService.Start();
        
        Console.WriteLine("GameServer Start");
    }

    public void Update()
    {
        m_networkService.Update();
    }

    public void Dispose()
    {
        m_networkService.Destroy();
        m_networkService = null;
    }
}
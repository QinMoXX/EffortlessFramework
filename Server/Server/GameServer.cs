using HotUpdate.Network;
using HotUpdate.Network.Message;
using Server.Service;

public class GameServer:IDisposable
{
    private  NetworkService m_networkService;
    private DBService m_dbService;

    public void Init()
    {
        m_networkService = new NetworkService(40001, NetworkDispatcher.Instance);
        m_networkService.Init();
        
        m_dbService = new DBService();
        m_dbService.Init();
        Console.WriteLine("GameServer Initialized");
    }
    public void Start()
    {
        m_networkService.Start();
        m_dbService.Start();
        
        
        
        NetworkDispatcher.Instance.SubscribeMessage<ReqLogin>((int)NetworkMessageIds.ReqLogin, NetworkDispatcher.OnReqLogin);
        Console.WriteLine("GameServer Started");
    }

    public void Update()
    {
        m_networkService.Update();
        m_dbService.Update();
    }

    public void Dispose()
    {
        m_networkService.Destroy();
        m_networkService = null;
        
        m_dbService.Destroy();
        m_dbService = null;
    }
}
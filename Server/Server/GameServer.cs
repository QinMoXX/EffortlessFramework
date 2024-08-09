using HotUpdate.Network;
using HotUpdate.Network.Message;

public class GameServer:IDisposable
{
    private  NetworkService m_networkService;
    public GameServer()
    {
        
    }

    public void Start()
    {
        m_networkService = new NetworkService(40001, NetworkDispatcher.Instance);
        m_networkService.Start();
        
        Console.WriteLine("GameServer Start");
        
        NetworkDispatcher.Instance.SubscribeMessage<ReqLogin>((int)NetworkMessageIds.ReqLogin, NetworkDispatcher.OnReqLogin);
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
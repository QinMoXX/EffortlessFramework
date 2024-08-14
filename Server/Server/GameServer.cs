using HotUpdate.Network;
using HotUpdate.Network.Message;
using MongoDB.Driver;
using Server.DBModel;
using Server.Service;

public class GameServer:IDisposable
{
    private  NetworkService m_networkService;
    private DBService m_dbService;
    private HallsService m_hallsService;

    public void Init()
    {
        m_networkService = new NetworkService(40001, NetworkDispatcher.Instance);
        m_networkService.Init();
        
        m_dbService = new DBService();
        m_dbService.Init();
        
        m_hallsService = new HallsService();
        m_hallsService.Init();
        
        Console.WriteLine("GameServer Initialized");
    }
    public void Start()
    {
        m_networkService.Start();
        m_dbService.Start();
        m_hallsService.Start();
        
        NetworkDispatcher.Instance.SubscribeMessage<ReqLogin>((int)NetworkMessageIds.ReqLogin, NetworkDispatcher.OnReqLogin);
        NetworkDispatcher.Instance.SubscribeMessage<ReqEntryRoom>((int)NetworkMessageIds.ReqEntryRoom, NetworkDispatcher.OnReqEntryRoom);
        Console.WriteLine("GameServer Started");
        
        // DBService.Instance.InsertData("User", 
        //     new User{UID = Guid.NewGuid().GetHashCode(),CreateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), UserName = "test", Password = "123456"});
        //
        // DBService.Instance.InsertData("User", 
        //     new User{UID = Guid.NewGuid().GetHashCode(),CreateTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), UserName = "test2", Password = "123456"});
        //
        // DBService.Instance.InsertData("User", 
        //     new User{UID = Guid.NewGuid().GetHashCode(),CreateT=ime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), UserName = "test3", Password = "123456"});
    }

    public void Update()
    {
        m_networkService.Update();
        m_dbService.Update();
        m_hallsService.Update();
    }

    public void Dispose()
    {
        m_networkService.Destroy();
        m_networkService = null;
        
        m_dbService.Destroy();
        m_dbService = null;
        
        m_hallsService.Destroy();
        m_hallsService = null;
    }
}
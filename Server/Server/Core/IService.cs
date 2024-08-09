namespace GameServer.Core
{
    public interface IService
    {
        public  void Init();

        public void Start();

        public void Update();
        
        public void Destroy();
    }
}


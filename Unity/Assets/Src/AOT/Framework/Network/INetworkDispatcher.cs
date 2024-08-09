namespace AOT.Framework.Network
{
    public interface INetworkDispatcher
    {
        public void Dispatch(int messageId, byte[] data);
        
        public void Dispatch(int messageId, string data);
        
        public void Dispatch(INetSession session, int messageId, byte[] data);
        
        public  void Dispatch(INetSession session, int messageId, string data);
    }
}
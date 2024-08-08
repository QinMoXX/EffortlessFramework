using System;
using System.Collections.Generic;
using AOT.Framework.Network;
using HotUpdate.Network.Message;
using MemoryPack;


namespace HotUpdate.Network
{
    public partial class NetworkDispatcher:INetworkDispatcher
    {
        private readonly  Dictionary<int, Action<byte[]>> m_messageHandlers;

        public NetworkDispatcher()
        {
            m_messageHandlers = new Dictionary<int, Action<byte[]>>()
            {
                // begin messageHandler
                { 2, bytes => ResLogin(MemoryPackSerializer.Deserialize<ResLogin>(bytes)) }
                
                // end messageHandler
            };
        }
        
        
        public void Dispatch(int messageId, byte[] data)
        {
            if (m_messageHandlers.TryGetValue(messageId, out var handler))
            {
                handler(data);
            }
        }
    }
}
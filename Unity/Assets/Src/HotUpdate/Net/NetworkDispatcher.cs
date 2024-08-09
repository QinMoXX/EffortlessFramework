using System;
using System.Collections.Generic;
using AOT.Framework;
using AOT.Framework.Network;
using HotUpdate.Network.Message;
using MemoryPack;


namespace HotUpdate.Network
{
    public partial class NetworkDispatcher:SingletonInstance<NetworkDispatcher>,INetworkDispatcher
    {
        public delegate void MessageHandler(byte[] data);
        
        private readonly  Dictionary<int, MessageHandler> m_messageHandlers;
        public NetworkDispatcher()
        {
            m_messageHandlers = new Dictionary<int, MessageHandler>();
        }
        
        /// <summary>
        /// 订阅网络消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        /// <typeparam name="T"></typeparam>
        public void SubscribeMessage<T>(int id,Action<T> handler) where T : NetPacket
        {
            m_messageHandlers.TryAdd(id, bytes => handler(MemoryPackSerializer.Deserialize<T>(bytes)));
        }

        /// <summary>
        /// 取消网络消息订阅
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        public void UnSubscribeMessage<T>(int id) where T : NetPacket
        {
            if (m_messageHandlers.TryGetValue(id, out var value))
            {
                m_messageHandlers.Remove(id);
            }
        }
        
        
        public void Dispatch(int messageId, byte[] data)
        {
            if (m_messageHandlers.TryGetValue(messageId, out var handler))
            {
                handler(data);
            }
        }

        public void Dispatch(int messageId, string data)
        {
            
        }

        public void Dispatch(INetSession session, int messageId, byte[] data)
        {
            
        }

        public void Dispatch(INetSession session, int messageId, string data)
        {
            
        }
    }
}
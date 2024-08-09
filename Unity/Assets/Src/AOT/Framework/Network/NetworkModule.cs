using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using AOT.Framework.Debug;
using Cysharp.Threading.Tasks;
using System.Net.Sockets.Kcp;
using MemoryPack;
using UnityEngine;

namespace AOT.Framework.Network
{
    
    public sealed class NetworkManager:SingletonInstance<NetworkManager>,IGameModule,INetworkManager
    {
        public short Priority { get; } = 10;
        private INetworkChannel[] m_channels;
        private Dictionary<int, INetworkChannel> m_channelDict;
        
        public void Init()
        {
            m_channels = Array.Empty<INetworkChannel>();
            m_channelDict = new Dictionary<int, INetworkChannel>();
        }

        public INetworkManager Initialize()
        {
            return this;
        }
        
        public void Update(float virtualElapse, float realElapse)
        {

            if (m_channels == null)
            {
                return;
            }

            for (int i = 0; i < m_channels.Length; i++)
            {
                m_channels[i].Update(virtualElapse, realElapse);
            }
            
        }

        public void Destroy()
        {
            for (int i = 0; i < m_channels.Length; i++)
            {
                m_channels[i].Close();
            }
            m_channels = null;
            m_channelDict.Clear();
            m_channelDict = null;
        }


        public INetworkChannel AddChannel(int channelId, INetworkChannel channel)
        {
            if (channel == null)
            {
                throw new GameFrameworkException("channel is invalid.");
            }

            if (m_channelDict.TryGetValue(channelId,out var value))
            {
                throw  new GameFrameworkException("channel id is exist.");
            }
            
            m_channelDict.TryAdd(channelId,channel);

            INetworkChannel[] networkChannels = new INetworkChannel[m_channels.Length + 1];
            Array.Copy(m_channels, networkChannels, m_channels.Length);
            networkChannels[m_channels.Length] = channel;
            m_channels = networkChannels;
            return channel;
        }

        public INetworkChannel GetChannel(int channelId)
        {
            if (!m_channelDict.TryGetValue(channelId, out var value))
            {
                return null;
            }
            return value;
        }

        public IList<INetworkChannel> GetAllNetworkChannels()
        {
            if (m_channels == null)
            {
                throw new GameFrameworkException("NetworkManager is not initialized.");
            }
            return m_channels.ToList();
        }
    }
}
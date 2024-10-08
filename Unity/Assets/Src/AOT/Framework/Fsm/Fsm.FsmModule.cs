﻿using System;
using System.Collections.Generic;
using UnityEngine.Profiling;

namespace AOT.Framework.Fsm
{
    public sealed class FsmManager:IGameModule
    {

        private readonly Dictionary<string, FsmBase> m_Fsms;
        private readonly List<FsmBase> m_FsmsCache;

        public FsmManager()
        {
            m_Fsms = new Dictionary<string, FsmBase>();
            m_FsmsCache = new List<FsmBase>();
        }

        public int Count
        {
            get => m_Fsms.Count;
        }

        public short Priority => 1;

        public void Init()
        {
            
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            Profiler.BeginSample("FsmManager.Update");
            foreach (var fsm in m_FsmsCache)
            {
                if (!fsm.IsRunning)
                {
                    continue;
                }
                (fsm as IFsm).Update(elapseSeconds,realElapseSeconds);
            }
            Profiler.EndSample();
        }

        public void Destroy()
        {
            Shutdown();
        }
        

        public void Shutdown()
        {
            foreach (var kp in m_Fsms)
            {
                (kp.Value as IFsm).Shutdown();
            }
            
            m_Fsms.Clear();
            m_FsmsCache.Clear();
        }

        public bool HasFsm(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Fsm name is invalid.");
            }
            
            return m_Fsms.ContainsKey(name);
        }
        

        /// <summary>
        /// 获取所有状态机数组
        /// </summary>
        /// <returns></returns>
        public FsmBase[] GetAllFsms()
        {
            return m_FsmsCache.ToArray();
        }

        public IFsm CreateFsm<T>(string name, params Type[] states) where T : FsmBase
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Fsm name is invalid.");
            }
            
            if (m_Fsms.ContainsKey(name))
            {
                throw new Exception($"Fsm '{name}' already exists.");
            }

            FsmBase fsm = FsmBase.Create<T>(name, states);
            m_Fsms.Add(name, fsm);
            m_FsmsCache.Add(fsm);
            return fsm;
        }

        public bool RemoveFsm(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Fsm name is invalid.");
            }

            FsmBase fsm = null;
            if (!m_Fsms.TryGetValue(name,out fsm))
            {
                return true;
            }

            (fsm as IFsm).Shutdown();
            m_Fsms.Remove(name);
            m_FsmsCache.Remove(fsm);
            return true;
        }


    }

}
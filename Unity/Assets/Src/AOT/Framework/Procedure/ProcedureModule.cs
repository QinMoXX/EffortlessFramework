using System;
using AOT.Framework.Fsm;


namespace AOT.Framework.Procedure
{
    [DependencyModule(typeof(FsmManager))]
    public class ProcedureManager:IGameModule
    {
        public const string ProcedureFsmName = "Procedure";
        private FsmManager m_FsManager;
        private IFsm m_ProcedureFsm;

        public ProcedureManager()
        {
            m_FsManager = null;
            m_ProcedureFsm = null;
        }
        
        public short Priority => 10;
        public void Init()
        {
            
        }

        
        public void Update(float virtualElapse, float realElapse)
        {
            
        }

        /// <summary>
        /// 销毁流程模块
        /// </summary>
        public void Destroy()
        {
            if (m_FsManager != null)
            {
                if (m_ProcedureFsm != null)
                {
                    m_FsManager.RemoveFsm(ProcedureFsmName);
                    m_ProcedureFsm = null;
                }

                m_FsManager = null;
            }
        }

        public ProcedureBase CurrentProcedure
        {
            get
            {
                if (m_ProcedureFsm == null)
                {
                    throw new Exception("You must initialize procedure first.");
                }

                return (ProcedureBase)m_ProcedureFsm.CurrentState;
            }
        }

        /// <summary>
        /// 初始化流程
        /// </summary>
        /// <param name="procedures">流程类型</param>
        /// <exception cref="Exception"></exception>
        public void Initialize(params Type[] procedures)
        {
            if (m_FsManager == null)
            {
                m_FsManager = GameEntry.GetModule<FsmManager>();
            }

            if (m_FsManager == null)
            {
                throw new Exception("You must create Fsm manager first.");
            }

            if (m_ProcedureFsm != null)
            {
                //存在流程状态机先关闭，再重新创建
                m_ProcedureFsm.Shutdown();
                m_FsManager.RemoveFsm(ProcedureFsmName);
                m_ProcedureFsm = null;
            }
            m_ProcedureFsm = m_FsManager.CreateFsm<ProcedureFsm>(ProcedureFsmName, procedures);
        }
        
        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <typeparam name="T">要开始的流程类型。</typeparam>
        public void StartProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start<T>();
        }
        
        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <param name="procedureType">要开始的流程类型。</param>
        public void StartProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start(procedureType);
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <typeparam name="T">要获取的流程类型</typeparam>
        /// <returns>要获取的流程</returns>
        /// <exception cref="Exception"></exception>
        public ProcedureBase GetProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }
            return m_ProcedureFsm.GetState<T>();
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="procedureType">要获取的流程类型</param>
        /// <returns>要获取的流程</returns>
        /// <exception cref="Exception"></exception>
        public ProcedureBase GetProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }
            return m_ProcedureFsm.GetState(procedureType) as ProcedureBase;
        }

        /// <summary>
        /// 判断流程是否存在
        /// </summary>
        /// <typeparam name="T">要判断的流程类型</typeparam>
        /// <returns>是否存在</returns>
        /// <exception cref="Exception"></exception>
        public bool HasProcedure<T>() where T : ProcedureBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return m_ProcedureFsm.HasState<T>();
        }

        /// <summary>
        /// 判断流程是否存在
        /// </summary>
        /// <param name="procedureType">要判断的流程类型</param>
        /// <returns>是否存在</returns>
        /// <exception cref="Exception"></exception>
        public bool HasProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new Exception("You must initialize procedure first.");
            }

            return m_ProcedureFsm.HasState(procedureType);
        }
    }
}
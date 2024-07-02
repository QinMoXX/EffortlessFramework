using AOT.Framework;
using AOT.Framework.Resource;
using Cysharp.Threading.Tasks;
using Luban;
using SimpleJSON;
using UnityEngine;

namespace HotUpdate
{
    [DependencyModule(typeof(ResourceManager))]
    public sealed partial class TableManager:SingletonInstance<TableManager>,IGameModule
    {
        public short Priority => 10;

        private cfg.Tables m_ClientTable;
        
        public cfg.Tables CfgClientTable
        {
            get
            {
                if (m_ClientTable == null)
                {
                    throw new GameFrameworkException("TableManager.ClientTable does not exist");
                }
                return m_ClientTable;
            }
        }
        
        private ResourceManager m_ResourceManager;
        public void Init()
        {
            
        }

        public void Initialize(ResourceManager resourceManager)
        {
            m_ResourceManager = resourceManager;
            m_ClientTable = new cfg.Tables(LoadByteBuf);
        }

        public void Update(float virtualElapse, float realElapse)
        {
        }

        public void Destroy()
        {
            
        }
        

        private ByteBuf LoadByteBuf(string file)
        {
            TextAsset textAsset = m_ResourceManager.LoadAssetSync<TextAsset>(file);
            return new ByteBuf(textAsset.bytes);
        }
    }
}
using AOT.Framework.Resource;
using AOT.Framework.Table;
using Luban;
using UnityEngine;

namespace HotUpdate
{
    public class TableHelper:ITableHelper<cfg.Tables>
    {
        private  IResourceManager m_ResourceManager;
        private cfg.Tables m_ClientTable;

        public cfg.Tables CfgClientTable => m_ClientTable;
        
        public TableHelper(IResourceManager gameModule)
        {
            m_ResourceManager = gameModule;
            m_ClientTable = new cfg.Tables(LoadByteBuf);
        }
        
        private ByteBuf LoadByteBuf(string file)
        {
            TextAsset textAsset = m_ResourceManager.LoadAssetSync<TextAsset>(file);
            return new ByteBuf(textAsset.bytes);
        }
    }
}
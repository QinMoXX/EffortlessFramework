using AOT.Framework;
using AOT.Framework.Resource;
using AOT.Framework.Table;
using cfg;

namespace HotUpdate
{
    [DependencyModule(typeof(ResourceManager))]
    public sealed class TableManager:SingletonInstance<TableManager>,ITableManager<Tables>,IGameModule
    {
        public short Priority => 10;

        private TableHelper m_TableHelper;

        public Tables CfgClientTable
        {
            get
            {
                if (m_TableHelper == null)
                {
                    throw new GameFrameworkException("TableManager.ClientTable does not exist");
                }
                return m_TableHelper.CfgClientTable;
            }
        }
        

        public ITableManager<Tables> Initialize(IResourceManager gameModule)
        {
            m_TableHelper = new TableHelper(gameModule);
            return this;
        }

        public void Init()
        {
            
        }
        

        public void Update(float virtualElapse, float realElapse)
        {
        }

        public void Destroy()
        {
            
        }
        
        
    }
}
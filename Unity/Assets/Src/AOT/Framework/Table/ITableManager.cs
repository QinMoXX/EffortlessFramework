
using AOT.Framework.Resource;

namespace AOT.Framework.Table
{
    public interface ITableManager<T>
    {
        public T CfgClientTable { get; }
        
        public ITableManager<T> Initialize(IResourceManager gameModule);
        
    }
}
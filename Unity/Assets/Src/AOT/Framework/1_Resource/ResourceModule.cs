using Framework;
using YooAsset;

namespace Src.AOT.Framework.Resource
{
    public class ResourceManager:IGameModule
    {
        public uint Priority { get => 1; }
        public void Init()
        {
            // 初始化资源系统
            YooAssets.Initialize();
            // 创建默认的资源包
            var package = YooAssets.CreatePackage("DefaultPackage");
            // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
            YooAssets.SetDefaultPackage(package);
        }

        public void Entry()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        
        public void Update(float virtualElapse, float realElapse)
        {
            throw new System.NotImplementedException();
        }
    }
}
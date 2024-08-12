using Cysharp.Threading.Tasks;
using YooAsset;

namespace AOT.Framework.Resource
{
    public interface IResourceManager
    {
        ResourcePackage DefaultPackage { get; }
        bool IsInitialize { get; }
        
        void SetMode(ResourceManager.ResourceRunningMode mode);
        
        UniTask Initialize();

        UniTask<T> LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object;
        
        T LoadAssetSync<T>(string assetName) where T : UnityEngine.Object;

        bool Exist(string assetName);
    }
}
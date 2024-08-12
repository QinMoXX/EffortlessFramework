using System.Threading;
using Cysharp.Threading.Tasks;
using YooAsset;

namespace AOT.Framework.Scene
{
    public interface ISceneManager
    {
        public delegate void OnSceneLoaded(bool isSuccess,string sceneName);
        
        public delegate void OnSceneUnloaded(bool isSuccess, string sceneName);
        
        public delegate void OnSceneLoading(float progress, string sceneName);
        
        public  bool LoadScene(string sceneName, OnSceneLoaded onSceneLoaded = null);
        
        public UniTask<bool> LoadSceneAsync(string sceneName , CancellationToken ct = default, OnSceneLoaded onSceneLoaded = null, OnSceneLoading onSceneLoading = null,bool suspendLoad = false);
        
        public  bool UnloadScene(string sceneName, OnSceneUnloaded onSceneUnloaded = null);
        
        public UniTask<bool> UnloadSceneAsync(string sceneName, OnSceneUnloaded onSceneUnloaded = null);
        
        public bool IsSceneLoaded(string sceneName);
        
        public UnityEngine.SceneManagement.Scene GetActiveScene();
        
        public bool IsSceneActive(string sceneName);
        
        public bool ExistWithinResource(string sceneName);
    }
}
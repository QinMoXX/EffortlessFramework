using System.Threading;
using AOT.Framework;
using AOT.Framework.Resource;
using AOT.Framework.Scene;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using YooAsset;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace AOT.Framework.Scene
{
    [DependencyModule(typeof(ResourceManager))]
    public class SceneManager:IGameModule,ISceneManager
    {
        public short Priority { get; } = 11;
        
        private string m_activeSceneName;
        
        
        public void Init()
        {
            m_activeSceneName = "Main";
        }

        
        public void Update(float virtualElapse, float realElapse)
        {
            
        }

        public void Destroy()
        {
            
        }

        /// <summary>
        /// 获取场景资源完整路径
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static string GetSceneFullPath(string sceneName)
        {
            return string.Format("{0}_{1}", sceneName,sceneName);
        }

        public bool LoadScene(string sceneName, ISceneManager.OnSceneLoaded onSceneLoaded = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw new GameFrameworkException("Scene Name is invalid!");
            }
            var handle = GameEntry.GetModule<ResourceManager>().LoadSceneSync(GetSceneFullPath(sceneName), LoadSceneMode.Additive);
            if (handle.Status != EOperationStatus.Succeed)
            {
                onSceneLoaded?.Invoke(false, sceneName);
                return false;
            }
            onSceneLoaded?.Invoke(true, sceneName);
            return true;
        }
        

        public async UniTask<bool> LoadSceneAsync(string sceneName,CancellationToken ct = default, ISceneManager.OnSceneLoaded onSceneLoaded = null,
            ISceneManager.OnSceneLoading onSceneLoading = null,bool suspendLoad = false)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw new GameFrameworkException("Scene Name is invalid!");
            }
            var handle = GameEntry.GetModule<ResourceManager>().LoadSceneAsync(GetSceneFullPath(sceneName), LoadSceneMode.Additive);
            handle.WithCancellation(ct);
            while (!handle.IsDone)
            {
                onSceneLoading?.Invoke(handle.Progress, sceneName);
                await UniTask.NextFrame();
            }
            onSceneLoaded?.Invoke(true, sceneName);
            return handle.Status == EOperationStatus.Succeed;
        }

        
        public bool UnloadScene(string sceneName, ISceneManager.OnSceneUnloaded onSceneUnloaded = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw new GameFrameworkException("Scene Name is invalid!");
            }

            if (!IsSceneLoaded(sceneName))
            {
                onSceneUnloaded?.Invoke(false, sceneName);
                return false;
            }

            var asyncOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(
                UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
            while (asyncOperation.isDone)
            {
                return true;
            }

            return false;
        }
        

        public async UniTask<bool> UnloadSceneAsync(string sceneName, ISceneManager.OnSceneUnloaded onSceneUnloaded = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw  new GameFrameworkException("Scene Name is invalid!");
            }

            if (!IsSceneLoaded(sceneName))
            {
                return false;
            }

            var asyncOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(
                UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
            await asyncOperation.ToUniTask();
            onSceneUnloaded?.Invoke(true, sceneName);
            return true;
        }

        public bool IsSceneLoaded(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw  new GameFrameworkException("Scene Name is invalid!");
            }
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
            return scene.isLoaded;
        }

        public UnityEngine.SceneManagement.Scene GetActiveScene()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        public bool IsSceneActive(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw  new GameFrameworkException("Scene Name is invalid!");
            }

            return m_activeSceneName == sceneName;
        }
        

        public bool ExistWithinResource(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw new GameFrameworkException("Scene Name is invalid!");
            }
            return GameEntry.GetModule<ResourceManager>().Exist(GetSceneFullPath(sceneName));
        }
    }
}


using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using YooAsset;

namespace AOT.Framework.Resource
{

    public partial class ResourceManager:IGameModule,IResourceManager
    {
        /// <summary>
        /// 同步加载场景,同步加载完会立刻激活场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="sceneMode">加载模式</param>
        /// <returns>SceneHandle</returns>
        public SceneHandle LoadSceneSync(string sceneName,LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            if (DefaultPackage == null || !IsInitialize)
            {
                throw new GameFrameworkException("package is null or not initialize.");
            }

            return DefaultPackage.LoadSceneSync(sceneName,sceneMode);
        }

        /// <summary>
        /// 异步加载场景，加载完需要手动激活场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="sceneMode">加载模式</param>
        /// <param name="suspendLoad">是否挂起加载</param>
        /// <returns>SceneHandle</returns>
        public SceneHandle LoadSceneAsync(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            if (DefaultPackage == null || !IsInitialize)
            {
                throw new GameFrameworkException("package is null or not initialize.");
            }
            return DefaultPackage.LoadSceneAsync(sceneName, sceneMode);
        }


    }
}
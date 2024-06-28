using Framework;

namespace Src.AOT.Framework.Resource
{
    public partial class ResourceManager:IGameModule
    {
        public enum ResourceRunningMode
        {
            EDITOR, //在编辑器下，不需要构建资源包，来模拟运行游戏
            STANDALONE, //构建资源包使用单机运行模式
            ONLINE, //构建热更新资源包使用联机模式运行
        }
    }
}
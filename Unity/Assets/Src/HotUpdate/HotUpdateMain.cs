using AOT.Framework;
using AOT.Framework.Debug;

public class HotUpdateMain:IEntry
{
    public void Entry()
    {
        EDebug.Log("热更新代码加载成功");
    }

    public void Exit()
    {
        EDebug.Log("退出热更新代码");
    }
}

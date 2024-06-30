using AOT.Framework;
using AOT.Framework.Debug;
using AOT.Framework.Procedure;

public class HotUpdateMain:IEntry
{
    public void Entry()
    {
        EDebug.Log("热更新代码加载成功");
        GameEntry.GetModule<ProcedureManager>().Initialize(typeof(HomeMenuProcedure));
        GameEntry.GetModule<ProcedureManager>().StartProcedure<HomeMenuProcedure>();
    }

    public void Exit()
    {
        EDebug.Log("退出热更新代码");
    }
}

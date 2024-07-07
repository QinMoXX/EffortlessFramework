using AOT.Framework;
using AOT.Framework.Debug;
using AOT.Framework.Procedure;
using AOT.Framework.Resource;
using HotUpdate;

public class HotUpdateMain:IEntry
{
    public void Entry()
    {
        EDebug.Log("热更新代码加载成功");
        GameEntry.CreatModule<TableManager>().Initialize(GameEntry.GetModule<ResourceManager>());
        GameEntry.GetModule<ProcedureManager>().Initialize(typeof(HomeMenuProcedure)).StartProcedure<HomeMenuProcedure>();
        
    }

    public void Exit()
    {
        EDebug.Log("退出热更新代码");
    }
}

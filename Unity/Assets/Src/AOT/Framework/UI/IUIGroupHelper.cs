namespace AOT.Framework.UI
{
    /// <summary>
    /// 界面组辅助接口
    /// </summary>
    public interface IUIGroupHelper
    {
        
        IUIGroup Create(string uiGroupName);

        public void SetDepth(string uiGroupName,int depth);

    }
}
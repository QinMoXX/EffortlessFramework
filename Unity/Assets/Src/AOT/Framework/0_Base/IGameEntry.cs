namespace Framework
{
    public interface IEntry
    {
        /// <summary>
        /// 初始化执行
        /// </summary>
        public void Init();
        
        /// <summary>
        /// 进入调用
        /// </summary>
        public void Entry();
        

        /// <summary>
        /// 离开调用
        /// </summary>
        public void Exit();
    }
}
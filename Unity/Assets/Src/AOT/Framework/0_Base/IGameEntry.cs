namespace Framework
{
    public interface IEntry
    {
        
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
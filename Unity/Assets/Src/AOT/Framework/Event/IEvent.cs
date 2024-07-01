namespace AOT.Framework.Event
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// 事件数量
        /// </summary>
        public int Count { get; }
        
        /// <summary>
        /// 清除事件
        /// </summary>
        public void Clear();

        /// <summary>
        /// 轮询事件
        /// </summary>
        public void Update();
    }
}
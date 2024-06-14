namespace Framework
{
    public interface IGameModule:IEntry
    {
        /// <summary>
        /// 模块优先级,0表示优先级最高
        /// </summary>
        public uint Priority { get; }

        /// <summary>
        /// 模块轮询
        /// </summary>
        /// <param name="virtualElapse">虚拟流逝时间,单位秒</param>
        /// <param name="realElapse">真实流逝时间,单位秒</param>
        public void Update(float virtualElapse, float realElapse);
    }
}
namespace AOT.Framework.Event
{
    /// <summary>
    /// 事件结点。
    /// </summary>
    internal sealed class EventNode<T> : IReference
    {
        private object m_Sender;
        private T m_EventArgs;

        public EventNode()
        {
            m_Sender = null;
            m_EventArgs = default;
        }

        public object Sender
        {
            get
            {
                return m_Sender;
            }
        }

        public T EventArgs
        {
            get
            {
                return m_EventArgs;
            }
        }

        public static EventNode<T> Create(object sender, T e)
        {
            EventNode<T> eventNode = ReferencePool.Acquire<EventNode<T>>();
            eventNode.m_Sender = sender;
            eventNode.m_EventArgs = e;
            return eventNode;
        }

        public void Clear()
        {
            m_Sender = null;
            m_EventArgs = default;
        }
    }
}
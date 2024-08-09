using System;
using System.Reflection;

namespace AOT.Framework
{
    
    /// <summary>
    /// 模块依赖特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PacketIdAttribute:Attribute
    {
        public  int MessageId { get;}
        
        public PacketIdAttribute(int messageId)
        {
            MessageId = messageId;
        }

        public static int GetPacketId(Type type)
        {
            var attr = type.GetCustomAttribute<PacketIdAttribute>();
            if (attr == null)
            {
                return -1;
            }

            return attr.MessageId;
        }
    }
}
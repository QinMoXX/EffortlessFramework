using System;

namespace AOT.Framework
{
    /// <summary>
    /// 模块依赖特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependencyModuleAttribute:Attribute
    {
        public  Type[] Dependencies { get;}
        
        public DependencyModuleAttribute(params Type[] dependencies)
        {
            Dependencies = dependencies ?? throw new  ArgumentNullException(nameof(dependencies));
        }
    }
}
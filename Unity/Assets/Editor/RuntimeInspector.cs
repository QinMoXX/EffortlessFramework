using System;
using System.Collections.Generic;
using System.Reflection;
using Framework;
using AOT.Framework;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 运行时代码检查器
/// </summary>
[InitializeOnLoad]
public class RuntimeInspector
{
    static RuntimeInspector()
    {
        DependencyTracker dependencyTracker = new DependencyTracker();
        //获取AOT程序集，如有需要可以增加程序集
        Assembly assembly = Assembly.GetAssembly(typeof(DependencyModuleAttribute));
        //追踪模块依赖特性是否正确应用于模块接口
        //避免编写意义不明确的代码
        dependencyTracker.TrackDependencies(assembly);
        foreach (var type in dependencyTracker.GetDependencies().Keys)
        {
            if (!typeof(IGameModule).IsAssignableFrom(type))
            {
                throw new InvalidOperationException(
                    $"The type '{type.Name}' must implement 'IGameModule' to use the DependencyModuleAttribute.");
            }
        }
        Debug.Log("代码检查无误");
        
        
        MessageIdTracker messageIdTracker = new MessageIdTracker();
        messageIdTracker.TrackMessageId(assembly);
        Debug.Log("消息ID检查无误");
    }
    
    
}

public class DependencyTracker
{
    private readonly Dictionary<Type, HashSet<Type>> _dependencies;

    public DependencyTracker()
    {
        _dependencies = new Dictionary<Type, HashSet<Type>>();
    }

    public void TrackDependencies(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            var dependencyAttributes = (DependencyModuleAttribute[])type.GetCustomAttributes(typeof(DependencyModuleAttribute), false);
            if (dependencyAttributes.Length > 0)
            {
                foreach (var attribute in dependencyAttributes)
                {
                    foreach (var dependency in attribute.Dependencies)
                    {
                        if (!_dependencies.ContainsKey(type))
                        {
                            _dependencies[type] = new HashSet<Type>();
                        }
                        _dependencies[type].Add(dependency);
                    }
                }
            }
        }
    }

    public Dictionary<Type, HashSet<Type>> GetDependencies()
    {
        return _dependencies;
    }
}

public class MessageIdTracker
{
    private readonly Dictionary<int, Type> _messageId;

    public MessageIdTracker()
    {
        _messageId = new Dictionary<int, Type>();
    }

    public void TrackMessageId(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            var packetIdAttributes = (PacketIdAttribute[])type.GetCustomAttributes(typeof(PacketIdAttribute), false);
            if (packetIdAttributes.Length > 0)
            {
                foreach (var attribute in packetIdAttributes)
                {
                    Debug.Log($"{type.FullName} MessageId:{attribute.MessageId}");
                    if (_messageId.TryGetValue(attribute.MessageId, out var attributeType))
                    {
                        throw new GameFrameworkException(
                            $"{type.FullName} and {attributeType.FullName} have the same message ID:{attribute.MessageId}");
                    }

                    _messageId.TryAdd(attribute.MessageId, type);
                }
            }
        }
    }
}


using System;

namespace AOT.Framework
{
    [Serializable]
    public delegate void UltraEventHandler(object sender);
    [Serializable]
    public delegate void UltraEventHandler<TEventArgs>(object sender, TEventArgs e);
    [Serializable]
    public delegate void UltraEventHandler<TEventArgs, TEventArgs2>(object sender, TEventArgs e,TEventArgs2 e2);
}
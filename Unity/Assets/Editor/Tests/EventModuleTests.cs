using AOT.Framework;
using AOT.Framework.Event;
using NUnit.Framework;
using UnityEngine;


public class EventModuleTests
{
    private sealed class EventGroup1:EventBasis { }
    private sealed class EventGroup2:EventBasis { }
    
    private struct Event1
    {
        public int id;
    } 
    
    private struct Event2
    {
        public int id;
    } 
    
    //事件系统测试
    [Test]
    public void TestEventSend()
    {
        GameEntry.CreatModule<EventManager>();
        EventManager.Instance.Register<Event1,EventGroup1>(OnEvent1);
        EventManager.Instance.Register<Event2,EventGroup2>(OnEvent2);
        EventManager.Instance.Register<Event1,EventGroup2>(OnEvent1);
        EventManager.Instance.SendNow<Event1>(this,new Event1{id = 10});
        EventManager.Instance.SendNow<Event2,EventGroup2>(this,new Event2{id = 12});
        EventManager.Instance.SendNow<Event1,EventGroup2>(this,new Event1{id = 10});
    }

    private void OnEvent2(object sender, Event2 e)
    {
        Assert.AreEqual(e.id,12);
        Debug.Log(($"OnEvent2  id = {e.id}"));
    }

    private void OnEvent1(object sender, Event1 e)
    {
        Assert.AreEqual(e.id,10);
        Debug.Log(($"OnEvent1  id = {e.id}"));
    }
}
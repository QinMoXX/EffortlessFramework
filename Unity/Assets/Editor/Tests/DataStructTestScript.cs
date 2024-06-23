using System.Collections;
using DataStructure;
using NUnit.Framework;
using UnityEngine.TestTools;

public partial class DataStructTestScript
{
    [Test]
    public void SortListTestPasses()
    {
        SortList<int, string> sortList = new SortList<int, string>();
        sortList.Add(7,"DI7");
        sortList.Add(4,"DI4");
        Assert.AreEqual( 4,sortList[0].Item1);
        Assert.AreEqual(sortList[1].Item1, 7);
        Assert.AreEqual(sortList.Count, 2);
        sortList.Add(1,"DI1");
        Assert.AreEqual(sortList[0].Item2, "DI1");
        sortList.Add(2,"DI2");
        Assert.AreEqual(sortList[1].Item1, 2);
        sortList.Remove("DI4");
        Assert.AreEqual(sortList[2].Item1, 7);
        sortList.Add(19,"DI19");
        Assert.AreEqual(sortList[3].Item1, 19);
        Assert.AreEqual(sortList.List.Length,4);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

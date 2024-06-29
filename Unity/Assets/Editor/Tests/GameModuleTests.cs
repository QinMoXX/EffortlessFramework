using NUnit.Framework;
using AOT.Framework;

public class GameModuleTests
{
    class TestModule1:IGameModule
    {
        public void Init()
        {
                
        }

        public short Priority => 1;
        public void Update(float virtualElapse, float realElapse)
        {
        }

        public void Destroy()
        {
        }
    }
        
    [DependencyModule(typeof(TestModule1))]
    class TestModule2:IGameModule
    {
        public void Init()
        {
                
        }

        public short Priority => 2;
        public void Update(float virtualElapse, float realElapse)
        {
        }

        public void Destroy()
        {
        }
    }
        
    //模块创建依赖单元测试
    [Test]
    public void TestCreatModule()
    {
        GameEntry.CreatModule<TestModule1>();
        Assert.IsTrue(GameEntry.GetModule<TestModule1>() != null, "模块创建失败");
    }
        
    //模块创建依赖测试
    [Test]
    public void TestCreatModuleDependency()
    {
        try
        {
            GameEntry.CreatModule<TestModule2>();
        }
        catch(GameFrameworkException e)
        {
            Assert.IsTrue(GameEntry.GetModule<TestModule2>() == null, "模块创建失败");
            Assert.IsTrue(GameEntry.GetModule<TestModule1>() == null, "模块创建失败");
            return;
        }
        GameEntry.CreatModule<TestModule1>();
        GameEntry.CreatModule<TestModule2>();
        Assert.IsTrue(GameEntry.GetModule<TestModule2>() != null, "模块创建失败");
        Assert.IsTrue(GameEntry.GetModule<TestModule1>() != null, "模块创建失败");
    }
}
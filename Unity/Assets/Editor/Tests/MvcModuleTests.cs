using Framework;
using NUnit.Framework;
using Src.AOT.Framework.Debug;
using Src.AOT.Framework.Mvc;

public class MvcModuleTests
{
    public class MockModel : IModel
    {
        public void Init()
        {
            EDebug.Log("MockModel Init");
        }
    }
    public class MockView : IView
    {
        public void Init()
        {
            EDebug.Log("MockView Init");
        }
    }
    public class MockController : IController
    {
        public void Init()
        {
            EDebug.Log("MockController Init");
        }
    }

    [Test]
    public void TestGetView_Generic_ReturnsSameInstance()
    {
        MvcModule mvcModule = GameEntry.CreatModule<MvcModule>();
        // Arrange
        var viewType = typeof(MockView);

        // Act
        var view1 = mvcModule.GetView<MockView>();
        var view2 = mvcModule.GetView<MockView>();

        // Assert
        Assert.AreSame(view1, view2, "Should return the same instance when called twice.");
    }

    [Test]
    public void TestGetModel_Generic_ReturnsSameInstance()
    {
        MvcModule mvcModule = GameEntry.CreatModule<MvcModule>();
        // Arrange
        var modelType = typeof(MockModel);

        // Act
        var model1 = mvcModule.GetModel<MockModel>();
        var model2 = mvcModule.GetModel<MockModel>();

        // Assert
        Assert.AreSame(model1, model2, "Should return the same instance when called twice.");
    }

    [Test]
    public void TestGetController_Generic_ReturnsSameInstance()
    {
        MvcModule mvcModule = GameEntry.CreatModule<MvcModule>();
        // Arrange
        var controllerType = typeof(MockController);

        // Act
        var controller1 = mvcModule.GetController<MockController>();
        var controller2 = mvcModule.GetController<MockController>();

        // Assert
        Assert.AreSame(controller1, controller2, "Should return the same instance when called twice.");
    }
}

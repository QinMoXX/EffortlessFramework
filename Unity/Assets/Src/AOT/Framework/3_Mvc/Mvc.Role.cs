namespace Src.AOT.Framework.Mvc
{
    public interface IModel
    {
        public void Init();
    }

    public interface IView
    {
        public void Init();
    }

    public interface IController
    {
        public void Init();
    }
}
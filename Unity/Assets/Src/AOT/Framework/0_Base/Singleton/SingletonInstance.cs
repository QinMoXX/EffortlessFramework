
namespace AOT.Framework
{
	public abstract class SingletonInstance<T> where T : class, ISingleton
	{
		private static T _instance;
		public static T Instance
		{
			get
			{
				return _instance;
			}
			set
			{
				_instance = value;
			}
		}

		protected SingletonInstance()
		{
			if (_instance != null)
				throw new GameFrameworkException(Utility.String.Format("{0} instance already created.",typeof(T)));
			_instance = this as T;
		}
		protected void DestroyInstance()
		{
			_instance = null;
		}
	}
}
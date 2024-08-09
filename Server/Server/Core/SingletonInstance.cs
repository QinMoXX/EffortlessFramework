
namespace AOT.Framework
{
	public abstract class SingletonInstance<T> where T : class
	{
		private static T _instance;
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Activator.CreateInstance<T>();
				}
				return _instance;
			}
			set
			{
				_instance = value;
			}
		}
		
		protected void DestroyInstance()
		{
			_instance = null;
		}
	}
}
using SimpleJSON;

namespace AOT.Framework.Persistent
{
    public interface IConfigManager
    {
        public  void Load();
        
        public  void Save();
        
        public  bool HasConfig();
        
        public  void Clear();
        
        public  void SetInt(string configKey, int value);
        
        public  void SetFloat(string configKey, float value);
        
        public  void SetString(string configKey, string value);
        
        public  void SetBool(string configKey, bool value);
        
        public  int GetInt(string configKey, int defaultValue);
        
        public  float GetFloat(string configKey, float defaultValue);
        
        public  string GetString(string configKey, string defaultValue);
        
        public  bool GetBool(string configKey, bool defaultValue);
        
        public  void SetJson(string configKey, JSONNode value);
        
        public  JSONNode GetJson(string configKey);
    }
}
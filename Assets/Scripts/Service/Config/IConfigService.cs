using System.Collections.Generic;

namespace Service.Config
{
    public interface IConfigService
    {
        string GetStringValue(string key);

        int GetIntValue(string key);

        bool GetBoolValue(string key);

        float GetFloatValue(string key);

        void Load();

        void SetDefaults(Dictionary<string, object> list);
    }
}
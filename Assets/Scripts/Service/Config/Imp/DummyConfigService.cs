using System.Collections.Generic;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Service.Config.Imp
{
    public class DummyConfigService : IConfigService
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        private Dictionary<string, object> _list;

        public string GetStringValue(string key)
        {
            return (string)_list[key];
        }

        public int GetIntValue(string key)
        {
            return (int)_list[key];
        }

        public bool GetBoolValue(string key)
        {
            return (bool)_list[key];
        }

        public float GetFloatValue(string key)
        {
            return (float)_list[key];
        }

        public void Load()
        {
            dispatcher.Dispatch(ConfigEvent.DataReady);
        }

        public void SetDefaults(Dictionary<string, object> list)
        {
            _list = list;
        }
    }
}
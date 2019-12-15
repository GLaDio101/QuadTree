using System;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace Core.Testing.Integration
{
    public class Interaction
    {
        public string Key { get; set; }

        public IEventDispatcher Dispatcher { get; set; }

        public string EventName { get; set; }

        public Type EventType { get; set; }

        public TestParameter Parameter { get; set; }
    }
}
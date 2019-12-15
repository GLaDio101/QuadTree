using System;

namespace Core.Testing.Integration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IntegrationTestable : Attribute
    {
        public IntegrationTestable(Type eventType)
        {
            EventType = eventType;
        }

        public Type EventType { get; set; }
    }
}
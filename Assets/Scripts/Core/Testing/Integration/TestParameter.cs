using System;

namespace Core.Testing.Integration
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TestParameter : Attribute
    {
        public TestParameter(Type paramType)
        {
            ParamType = paramType;
        }

        public TestParameter(Type paramType, string[] textValues) : this(paramType)
        {
            TextValues = textValues;
        }

        public TestParameter(Type paramType, float minValue, float maxValue) : this(paramType)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public TestParameter(Type paramType, int minValue, int maxValue) : this(paramType)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public Type ParamType { get; set; }

        public float MinValue { get; set; }

        public float MaxValue { get; set; }

        public string[] TextValues { get; set; }
    }
}
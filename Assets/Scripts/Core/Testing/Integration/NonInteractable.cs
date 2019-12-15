using System;

namespace Core.Testing.Integration
{
    [AttributeUsage(AttributeTargets.Field)]
    public class NonInteractable : Attribute
    {
    }
}
using System;

namespace Service.Exceptions
{
    public class ServiceNotInitializated : Exception
    {
        public ServiceNotInitializated(string message) : base(message)
        {
        }
    }
}
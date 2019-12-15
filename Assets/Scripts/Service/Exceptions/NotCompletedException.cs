using System;

namespace Service.Exceptions
{
    public class NotCompletedException : Exception
    {
        public NotCompletedException(string message) : base(message)
        {
        }
    }
}
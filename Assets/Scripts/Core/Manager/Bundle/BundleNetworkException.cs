using System;

namespace Core.Manager.Bundle
{
    public class BundleNetworkException : Exception
    {
        public BundleLoadData BundleData { get; private set; }

        public BundleNetworkException(string message, BundleLoadData data) : base(message)
        {
            BundleData = data;
        }
    }
}
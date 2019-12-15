using System;

namespace Core.Manager.Bundle
{
    public class BundleAlreadyLoadedException : Exception
    {
        public BundleLoadData BundleData { get; private set; }

        public BundleAlreadyLoadedException(BundleLoadData data)
        {
            BundleData = data;
        }
    }
}
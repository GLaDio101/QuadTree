#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    public abstract class MaxFragmentLength
    {
        /*
         * RFC 3546 3.2.
         */
        public const byte pow2_9 = 1;
        public const byte pow2_10 = 2;
        public const byte pow2_11 = 3;
        public const byte pow2_12 = 4;

        public static bool IsValid(byte maxFragmentLength)
        {
            return maxFragmentLength >= pow2_9 && maxFragmentLength <= pow2_12;
        }
    }
}

#endif

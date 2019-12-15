#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    public abstract class NameType
    {
        /*
         * RFC 3546 3.1.
         */
        public const byte host_name = 0;

        public static bool IsValid(byte nameType)
        {
            return nameType == host_name;
        }
    }
}

#endif

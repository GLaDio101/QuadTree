#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    /*
     * RFC 3546 3.3.
     */
    public abstract class CertChainType
    {
        public const byte individual_certs = 0;
        public const byte pkipath = 1;

        public static bool IsValid(byte certChainType)
        {
            return certChainType >= individual_certs && certChainType <= pkipath;
        }
    }
}

#endif

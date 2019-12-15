#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    public abstract class CertificateStatusType
    {
        /*
         *  RFC 3546 3.6
         */
        public const byte ocsp = 1;
    }
}

#endif

#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using Standard_Assets.Core.BestHTTP.SecureProtocol.security;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    internal class TlsClientContextImpl
        :   AbstractTlsContext, TlsClientContext
    {
        internal TlsClientContextImpl(SecureRandom secureRandom, SecurityParameters securityParameters)
            :   base(secureRandom, securityParameters)
        {
        }

        public override bool IsServer
        {
            get { return false; }
        }
    }
}

#endif

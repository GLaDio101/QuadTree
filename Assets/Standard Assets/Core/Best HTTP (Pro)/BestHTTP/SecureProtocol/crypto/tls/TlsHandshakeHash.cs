#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    public interface TlsHandshakeHash
        :   IDigest
    {
        void Init(TlsContext context);

        TlsHandshakeHash NotifyPrfDetermined();

        void TrackHashAlgorithm(byte hashAlgorithm);

        void SealHashAlgorithms();

        TlsHandshakeHash StopTracking();

        IDigest ForkPrfHash();

        byte[] GetFinalHash(byte hashAlgorithm);
    }
}

#endif

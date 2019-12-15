#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System.IO;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    public abstract class AbstractTlsEncryptionCredentials
        : AbstractTlsCredentials, TlsEncryptionCredentials
    {
        /// <exception cref="IOException"></exception>
        public abstract byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret);
    }
}

#endif

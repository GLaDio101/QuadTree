#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System.IO;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    public class AbstractTlsCipherFactory
        :   TlsCipherFactory
    {
        /// <exception cref="IOException"></exception>
        public virtual TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm)
        {
            throw new TlsFatalAlert(AlertDescription.internal_error);
        }
    }
}

#endif

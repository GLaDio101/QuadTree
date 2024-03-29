#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using System.IO;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    public abstract class AbstractTlsSignerCredentials
        : AbstractTlsCredentials, TlsSignerCredentials
    {
        /// <exception cref="IOException"></exception>
        public abstract byte[] GenerateCertificateSignature(byte[] hash);

        public virtual SignatureAndHashAlgorithm SignatureAndHashAlgorithm
        {
            get
            {
                throw new InvalidOperationException("TlsSignerCredentials implementation does not support (D)TLS 1.2+");
            }
        }
    }
}

#endif

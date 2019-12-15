#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.parameters;
using Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.signers;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    public class TlsDssSigner
        :   TlsDsaSigner
    {
        public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
        {
            return publicKey is DsaPublicKeyParameters;
        }

        protected override IDsa CreateDsaImpl(byte hashAlgorithm)
        {
            return new DsaSigner(new HMacDsaKCalculator(TlsUtilities.CreateHash(hashAlgorithm)));
        }

        protected override byte SignatureAlgorithm
        {
            get { return tls.SignatureAlgorithm.dsa; }
        }
    }
}

#endif

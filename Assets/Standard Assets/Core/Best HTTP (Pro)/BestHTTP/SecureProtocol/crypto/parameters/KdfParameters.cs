#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.parameters
{
    /**
     * parameters for Key derivation functions for IEEE P1363a
     */
    public class KdfParameters : IDerivationParameters
    {
        byte[]  iv;
        byte[]  shared;

        public KdfParameters(
            byte[]  shared,
            byte[]  iv)
        {
            this.shared = shared;
            this.iv = iv;
        }

        public byte[] GetSharedSecret()
        {
            return shared;
        }

        public byte[] GetIV()
        {
            return iv;
        }
    }

}

#endif

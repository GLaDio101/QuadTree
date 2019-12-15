#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using Standard_Assets.Core.BestHTTP.SecureProtocol.math;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto
{
    /**
     * The basic interface that basic Diffie-Hellman implementations
     * conforms to.
     */
    public interface IBasicAgreement
    {
        /**
         * initialise the agreement engine.
         */
        void Init(ICipherParameters parameters);

        /**
         * return the field size for the agreement algorithm in bytes.
         */
        int GetFieldSize();

        /**
         * given a public key from a given party calculate the next
         * message in the agreement sequence.
         */
        BigInteger CalculateAgreement(ICipherParameters pubKey);
    }

}

#endif

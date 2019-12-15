#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.parameters;
using Standard_Assets.Core.BestHTTP.SecureProtocol.math;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.generators
{
    /**
     * a Diffie-Hellman key pair generator.
     *
     * This generates keys consistent for use in the MTI/A0 key agreement protocol
     * as described in "Handbook of Applied Cryptography", Pages 516-519.
     */
    public class DHKeyPairGenerator
		: IAsymmetricCipherKeyPairGenerator
    {
		private DHKeyGenerationParameters param;

		public virtual void Init(
			KeyGenerationParameters parameters)
        {
            this.param = (DHKeyGenerationParameters)parameters;
        }

		public virtual AsymmetricCipherKeyPair GenerateKeyPair()
        {
			DHKeyGeneratorHelper helper = DHKeyGeneratorHelper.Instance;
			DHParameters dhp = param.Parameters;

			BigInteger x = helper.CalculatePrivate(dhp, param.Random);
			BigInteger y = helper.CalculatePublic(dhp, x);

			return new AsymmetricCipherKeyPair(
                new DHPublicKeyParameters(y, dhp),
                new DHPrivateKeyParameters(x, dhp));
        }
    }
}

#endif

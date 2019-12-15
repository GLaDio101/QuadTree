#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using Standard_Assets.Core.BestHTTP.SecureProtocol.security;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.parameters
{
    public class DHKeyGenerationParameters
		: KeyGenerationParameters
    {
        private readonly DHParameters parameters;

		public DHKeyGenerationParameters(
            SecureRandom	random,
            DHParameters	parameters)
			: base(random, GetStrength(parameters))
        {
            this.parameters = parameters;
        }

		public DHParameters Parameters
        {
            get { return parameters; }
        }

		internal static int GetStrength(
			DHParameters parameters)
		{
			return parameters.L != 0 ? parameters.L : parameters.P.BitLength;
		}
    }
}

#endif

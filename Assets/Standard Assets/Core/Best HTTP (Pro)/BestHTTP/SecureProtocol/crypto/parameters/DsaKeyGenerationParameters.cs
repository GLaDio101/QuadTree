#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using Standard_Assets.Core.BestHTTP.SecureProtocol.security;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.parameters
{
    public class DsaKeyGenerationParameters
		: KeyGenerationParameters
    {
        private readonly DsaParameters parameters;

        public DsaKeyGenerationParameters(
            SecureRandom	random,
            DsaParameters	parameters)
			: base(random, parameters.P.BitLength - 1)
        {
            this.parameters = parameters;
        }

		public DsaParameters Parameters
        {
            get { return parameters; }
        }
    }

}

#endif

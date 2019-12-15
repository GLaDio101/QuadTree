#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.parameters
{
	/**
	* parameters for Key derivation functions for ISO-18033
	*/
	public class Iso18033KdfParameters
		: IDerivationParameters
	{
		byte[]  seed;

		public Iso18033KdfParameters(
			byte[]  seed)
		{
			this.seed = seed;
		}

		public byte[] GetSeed()
		{
			return seed;
		}
	}
}

#endif

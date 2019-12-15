#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.engines
{
	/// <remarks>
	/// An implementation of the AES Key Wrapper from the NIST Key Wrap Specification.
	/// <p/>
	/// For further details see: <a href="http://csrc.nist.gov/encryption/kms/key-wrap.pdf">http://csrc.nist.gov/encryption/kms/key-wrap.pdf</a>.
	/// </remarks>
	public class AesWrapEngine
		: Rfc3394WrapEngine
	{
		public AesWrapEngine()
			: base(new AesEngine())
		{
		}
	}
}

#endif

#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.engines
{
	/// <remarks>
	/// An implementation of the SEED key wrapper based on RFC 4010/RFC 3394.
	/// <p/>
	/// For further details see: <a href="http://www.ietf.org/rfc/rfc4010.txt">http://www.ietf.org/rfc/rfc4010.txt</a>.
	/// </remarks>
	public class SeedWrapEngine
		: Rfc3394WrapEngine
	{
		public SeedWrapEngine()
			: base(new SeedEngine())
		{
		}
	}
}

#endif

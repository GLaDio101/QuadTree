#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
namespace Standard_Assets.Core.BestHTTP.SecureProtocol.asn1
{
	public interface Asn1SetParser
		: IAsn1Convertible
	{
		IAsn1Convertible ReadObject();
	}
}

#endif

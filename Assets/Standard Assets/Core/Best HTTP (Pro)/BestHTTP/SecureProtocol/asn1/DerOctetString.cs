#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
namespace Standard_Assets.Core.BestHTTP.SecureProtocol.asn1
{
    public class DerOctetString
        : Asn1OctetString
    {
		/// <param name="str">The octets making up the octet string.</param>
        public DerOctetString(
			byte[] str)
			: base(str)
        {
        }

		public DerOctetString(
			Asn1Encodable obj)
			: base(obj)
        {
        }

        internal override void Encode(
            DerOutputStream derOut)
        {
            derOut.WriteEncoded(Asn1Tags.OctetString, str);
        }

		internal static void Encode(
			DerOutputStream	derOut,
			byte[]			bytes,
			int				offset,
			int				length)
		{
			derOut.WriteEncoded(Asn1Tags.OctetString, bytes, offset, length);
		}
	}
}

#endif

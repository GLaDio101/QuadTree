#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
using Standard_Assets.Core.BestHTTP.SecureProtocol.math;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.asn1.x509
{
    /**
     * The CRLNumber object.
     * <pre>
     * CRLNumber::= Integer(0..MAX)
     * </pre>
     */
    public class CrlNumber
        : DerInteger
    {
        public CrlNumber(
			BigInteger number)
			: base(number)
        {
        }

		public BigInteger Number
		{
			get { return PositiveValue; }
		}

		public override string ToString()
		{
			return "CRLNumber: " + Number;
		}
	}
}

#endif

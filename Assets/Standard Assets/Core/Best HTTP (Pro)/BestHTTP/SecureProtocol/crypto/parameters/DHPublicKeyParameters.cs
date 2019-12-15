#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using Standard_Assets.Core.BestHTTP.SecureProtocol.asn1;
using Standard_Assets.Core.BestHTTP.SecureProtocol.math;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.parameters
{
    public class DHPublicKeyParameters
		: DHKeyParameters
    {
        private readonly BigInteger y;

		public DHPublicKeyParameters(
            BigInteger		y,
            DHParameters	parameters)
			: base(false, parameters)
        {
			if (y == null)
				throw new ArgumentNullException("y");

			this.y = y;
        }

		public DHPublicKeyParameters(
            BigInteger			y,
            DHParameters		parameters,
		    DerObjectIdentifier	algorithmOid)
			: base(false, parameters, algorithmOid)
        {
			if (y == null)
				throw new ArgumentNullException("y");

			this.y = y;
        }

        public BigInteger Y
        {
            get { return y; }
        }

		public override bool Equals(
			object  obj)
        {
			if (obj == this)
				return true;

			DHPublicKeyParameters other = obj as DHPublicKeyParameters;

			if (other == null)
				return false;

			return Equals(other);
        }

		protected bool Equals(
			DHPublicKeyParameters other)
		{
			return y.Equals(other.y) && base.Equals(other);
		}

		public override int GetHashCode()
        {
            return y.GetHashCode() ^ base.GetHashCode();
        }
    }
}

#endif

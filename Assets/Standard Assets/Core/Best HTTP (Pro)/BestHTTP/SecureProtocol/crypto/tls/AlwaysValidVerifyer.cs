#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using Standard_Assets.Core.BestHTTP.SecureProtocol.asn1.x509;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
	/// <remarks>
	/// A certificate verifyer, that will always return true.
	/// <pre>
	/// DO NOT USE THIS FILE UNLESS YOU KNOW EXACTLY WHAT YOU ARE DOING.
	/// </pre>
	/// </remarks>
	//[Obsolete("Perform certificate verification in TlsAuthentication implementation")]
	public class AlwaysValidVerifyer : ICertificateVerifyer
	{
		/// <summary>Return true.</summary>
		public bool IsValid(Uri targetUri, X509CertificateStructure[] certs)
		{
			return true;
		}
	}
}

#endif

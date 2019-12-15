#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using Standard_Assets.Core.BestHTTP.SecureProtocol.asn1.x509;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
	/// <remarks>
	/// This should be implemented by any class which can find out, if a given
	/// certificate chain is being accepted by an client.
	/// </remarks>
	//[Obsolete("Perform certificate verification in TlsAuthentication implementation")]
	public interface ICertificateVerifyer
	{
		/// <param name="certs">The certs, which are part of the chain.</param>
        /// <param name="targetUri"></param>
		/// <returns>True, if the chain is accepted, false otherwise</returns>
		bool IsValid(Uri targetUri, X509CertificateStructure[] certs);
	}
}

#endif

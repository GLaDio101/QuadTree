#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System.IO;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.util.encoders
{
	/**
	 * Encode and decode byte arrays (typically from binary to 7-bit ASCII
	 * encodings).
	 */
	public interface IEncoder
	{
		int Encode(byte[] data, int off, int length, Stream outStream);

		int Decode(byte[] data, int off, int length, Stream outStream);

		int DecodeString(string data, Stream outStream);
	}
}

#endif

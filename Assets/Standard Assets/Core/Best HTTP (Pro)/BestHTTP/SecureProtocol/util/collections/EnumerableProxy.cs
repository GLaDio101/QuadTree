#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using System.Collections;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.util.collections
{
	public sealed class EnumerableProxy
		: IEnumerable
	{
		private readonly IEnumerable inner;

		public EnumerableProxy(
			IEnumerable inner)
		{
			if (inner == null)
				throw new ArgumentNullException("inner");

			this.inner = inner;
		}

		public IEnumerator GetEnumerator()
		{
			return inner.GetEnumerator();
		}
	}
}

#endif

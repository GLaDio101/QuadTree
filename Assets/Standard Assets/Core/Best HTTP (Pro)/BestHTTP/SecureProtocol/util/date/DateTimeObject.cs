#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.util.date
{
	public sealed class DateTimeObject
	{
		private readonly DateTime dt;

		public DateTimeObject(
			DateTime dt)
		{
			this.dt = dt;
		}

		public DateTime Value
		{
			get { return dt; }
		}

		public override string ToString()
		{
			return dt.ToString();
		}
	}
}

#endif

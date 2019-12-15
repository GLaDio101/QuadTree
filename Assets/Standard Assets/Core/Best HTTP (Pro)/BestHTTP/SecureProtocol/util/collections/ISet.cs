#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System.Collections;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.util.collections
{
	public interface ISet
		: ICollection
	{
		void Add(object o);
		void AddAll(IEnumerable e);
		void Clear();
		bool Contains(object o);
		bool IsEmpty { get; }
		bool IsFixedSize { get; }
		bool IsReadOnly { get; }
		void Remove(object o);
		void RemoveAll(IEnumerable e);
	}
}

#endif

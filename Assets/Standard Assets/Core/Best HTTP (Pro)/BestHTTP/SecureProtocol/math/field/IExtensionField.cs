#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.math.field
{
    public interface IExtensionField
        : IFiniteField
    {
        IFiniteField Subfield { get; }

        int Degree { get; }
    }
}

#endif
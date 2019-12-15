#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.math.ec
{
    public interface ECPointMap
    {
        ECPoint Map(ECPoint p);
    }
}

#endif

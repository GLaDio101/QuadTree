#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
namespace Standard_Assets.Core.BestHTTP.SecureProtocol.asn1.anssi
{
    public sealed class AnssiObjectIdentifiers
    {
        private AnssiObjectIdentifiers()
        {
        }

        public static readonly DerObjectIdentifier FRP256v1 = new DerObjectIdentifier("1.2.250.1.223.101.256.1");
    }
}

#endif

#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using System.IO;

namespace Standard_Assets.Core.BestHTTP.SecureProtocol.crypto.tls
{
    public class TlsFatalAlert
        : IOException
    {
        private readonly byte alertDescription;

        public TlsFatalAlert(byte alertDescription)
            : this(alertDescription, null)
        {
        }

        public TlsFatalAlert(byte alertDescription, Exception alertCause)
            : base(tls.AlertDescription.GetText(alertDescription), alertCause)
        {
            this.alertDescription = alertDescription;
        }

        public virtual byte AlertDescription
        {
            get { return alertDescription; }
        }
    }
}

#endif

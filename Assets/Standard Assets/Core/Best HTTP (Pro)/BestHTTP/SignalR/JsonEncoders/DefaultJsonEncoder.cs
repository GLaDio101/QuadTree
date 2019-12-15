#if !BESTHTTP_DISABLE_SIGNALR

using System.Collections.Generic;
using Standard_Assets.Core.BestHTTP.JSON;

namespace Standard_Assets.Core.BestHTTP.SignalR.JsonEncoders
{
    public sealed class DefaultJsonEncoder : IJsonEncoder
    {
        public string Encode(object obj)
        {
            return Json.Encode(obj);
        }

        public IDictionary<string, object> DecodeMessage(string json)
        {
            bool ok = false;
            IDictionary<string, object> result = Json.Decode(json, ref ok) as IDictionary<string, object>;
            return ok ? result : null;
        }
    }
}

#endif
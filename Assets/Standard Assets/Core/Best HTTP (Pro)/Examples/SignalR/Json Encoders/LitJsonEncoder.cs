#if !BESTHTTP_DISABLE_SIGNALR

using System.Collections.Generic;
using Standard_Assets.Core.BestHTTP.SignalR.JsonEncoders;
using Standard_Assets.Core.Examples.LitJson;

namespace Standard_Assets.Core.Examples.SignalR.Json_Encoders
{
    public sealed class LitJsonEncoder : IJsonEncoder
    {
        public string Encode(object obj)
        {
            JsonWriter writer = new JsonWriter();
            JsonMapper.ToJson(obj, writer);

            return writer.ToString();
        }

        public IDictionary<string, object> DecodeMessage(string json)
        {
            JsonReader reader = new JsonReader(json);

            return JsonMapper.ToObject<Dictionary<string, object>>(reader);
        }
    }
}

#endif
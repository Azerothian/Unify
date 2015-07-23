using System;
using Unify.Network.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Unify.Serialiser.JsonNet
{
  public class JsonNetSerialiser : ISerialiser
  {

    public class JsonContainer // had to write proxy container to declare item type for proper deserialisation
    {
      public string Type;
      public string Data;
    }


    public byte[] ObjectToByteArray(object data)
    {
      var jo = new JsonContainer()
      {
        Data = JsonConvert.SerializeObject(data),
        Type = data.GetType().AssemblyQualifiedName
      };
      return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(jo));
    }

    public object ByteArrayToObject(byte[] data)
    {

      string text = System.Text.Encoding.UTF8.GetString(data);
      var result = JsonConvert.DeserializeObject<JsonContainer>(text);
      Type t = Type.GetType(result.Type);
      return JsonConvert.DeserializeObject(result.Data, t);
    }
  }
}

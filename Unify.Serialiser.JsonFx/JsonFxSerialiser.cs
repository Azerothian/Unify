using JsonFx.Json;
using System;
using Unify.Network.Interfaces;

namespace Unify.Serialiser.Json
{
  public class JsonFxSerialiser : ISerialiser
  {

    public class JsonContainer // had to write proxy container to declare item type for proper deserialisation
    {
      public string Type;
      public string Data;
    }

    public byte[] ObjectToByteArray(object data)
    {
      var writer = new JsonFx.Json.JsonWriter();
      var jo = new JsonContainer()
      {
        Data = writer.Write(data),
        Type = data.GetType().AssemblyQualifiedName
      };
      var result = writer.Write(jo);
      return System.Text.Encoding.UTF8.GetBytes(result);
    }

    public object ByteArrayToObject(byte[] data)
    {
      JsonReader reader = new JsonReader();
      string text = System.Text.Encoding.UTF8.GetString(data);
      var jo = reader.Read<JsonContainer>(text);
      Type t = Type.GetType(jo.Type);
      var result = reader.Read(jo.Data, t);

      return result;
    }
  }
}

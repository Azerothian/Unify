using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network.Interfaces;

namespace Unify.Serialiser.Json
{
    public class JsonSerialiser : ISerialiser
    {
			public byte[] ObjectToByteArray(object data)
			{
				var writer = new JsonFx.Json.JsonWriter();
				return System.Text.Encoding.UTF8.GetBytes(writer.Write(data));
			}

			public object ByteArrayToObject(byte[] data)
			{
				string result = System.Text.Encoding.UTF8.GetString(data);
				//JObject person = JObject.Parse(result);

				JsonReader reader = new JsonReader();
				return reader.Read(result);
			}
		}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unify.Network.Interfaces;
using Unify.Util;


namespace Unify.Network.Serialiser
{
	public class BinarySerialiser : ISerialiser
	{
		public byte[] ObjectToByteArray(object data)
		{
			if (data.GetType().IsSerializable)
			{
				MemoryStream fs = new MemoryStream();
				BinaryFormatter formatter = new BinaryFormatter();
				try
				{
					formatter.Serialize(fs, data);
					return fs.ToArray();
				}
				catch (SerializationException ex)
				{
					Log.Critical("ObjectToByteArray Failed", ex);
				}
				finally
				{
					fs.Close();
				}
			}
			return null;
		}

		public object ByteArrayToObject(byte[] data)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream fs = new MemoryStream(data);
			try
			{
				return formatter.Deserialize(fs);
			}
			catch (SerializationException ex)
			{
				Log.Critical( "ByteArrayToObject Failed", ex);
				return null;
			}
			finally
			{
				fs.Close();
			}
		}
	}
}

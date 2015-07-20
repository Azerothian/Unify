using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network.Interfaces;

namespace Unify.Serialiser.Protobuf
{
    public class ProtobufSerialiser : ISerialiser
    {

			public byte[] ObjectToByteArray(object data)
			{
				return null;
			}

			public object ByteArrayToObject(byte[] data)
			{
				throw new NotImplementedException();
			}
		}
}

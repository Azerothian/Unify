using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Network.Interfaces
{
	public interface ISerialiser
	{
		byte[] ObjectToByteArray(object data);
		object ByteArrayToObject(byte[] data);
	}
}

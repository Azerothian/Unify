namespace Unify.Network.Interfaces
{
	public interface ISerialiser
	{
		byte[] ObjectToByteArray(object data);
		object ByteArrayToObject(byte[] data);
	}
}

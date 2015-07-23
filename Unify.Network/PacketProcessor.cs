using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Unify.Util;


namespace Unify.Network
{
	public class PacketProcessor : ThreadHelper
	{

    private MemoryManager _dataStream = new MemoryManager();
		public event Action<Packet> OnPacketFound;

		Queue<byte[]> _dataToWrite = new Queue<byte[]>();
		public PacketProcessor()
		{
			ThreadSleep = 2;
		}
		public void Write(byte[] data)
		{
			lock (_dataToWrite)
			{
				_dataToWrite.Enqueue(data);
			}
		}

		public override void ThreadWorker(TimeSpan timeDiff)
		{
			int minLength = 20;
			Packet packet = null;

			lock (_dataToWrite)
			{
				if(_dataToWrite.Count > 0)
				{
					_dataStream.Write(_dataToWrite.Dequeue());
				}

			}

			lock (_dataStream)
			{
        var original = _dataStream.Length;
				if (_dataStream.Length > minLength) // min packet size not including headername or packet
				{
					var buffer = _dataStream.GetBuffer(); //fixed length - may be bigger then actual content
					for (var i = 0; i < _dataStream.Length; i++)
					{

						var index = i;
						if (index + minLength > _dataStream.Length)
							break; // terminate loop - not enough data to work with

						if (!CheckForByteArray(Variables.StartOfData, buffer, index))
							continue; // start not found.. continue
						index += Variables.StartOfData.Length;

						int headerNameSize = BitConverter.ToInt32(buffer, index);
						index += sizeof(int);

						if (index + headerNameSize > _dataStream.Length)
							continue; // headerNameSize + index is greater then data available - Either Invalid index or not all data has been recieved

						byte[] packetNameBytes = new byte[headerNameSize];
						Buffer.BlockCopy(buffer, index, packetNameBytes, 0, headerNameSize);
						index += headerNameSize;

						int packetSize = BitConverter.ToInt32(buffer, index);
						index += sizeof(int);

						if (index + packetSize > _dataStream.Length)
							continue; // packetSize + index is greater then data available - Either Invalid index or not all data has been recieved

						if (!CheckForByteArray(Variables.EndOfHeaderData, buffer, index))
							continue; // header end not found.. continue
						index += Variables.EndOfHeaderData.Length;

						if (!CheckForByteArray(Variables.EndOfData, buffer, index + packetSize))
							continue; // data end not found.. continue

						// All Checks Passed time to create the packet
						packet = new Packet();
						packet.Name = System.Text.Encoding.UTF8.GetString(packetNameBytes);
						packet.Data = new byte[packetSize];
						Buffer.BlockCopy(buffer, index, packet.Data, 0, packetSize);
						var totalPacketSize = Variables.StartOfData.Length
							+ 4 //headerNameLength
							+ headerNameSize
							+ Variables.EndOfHeaderData.Length
							+ packetSize
							+ Variables.EndOfData.Length;

            var startIndex = i;
            var endIndex = i + totalPacketSize;

            if (endIndex > _dataStream.Length)
              throw new ArgumentException(String.Format("Total of index and count is out of range: startIndex {0} endIndex {1} _dataStream.Length {2} original {3}", startIndex, endIndex, _dataStream.Length, original));
            //Log.Info("Removing for packet from the datastream: startIndex: {0}, endIndex: {1}, _dataStream.Length: {2}", startIndex, endIndex, _dataStream.Length);
            _dataStream.Remove(startIndex, totalPacketSize);
					}

				}

			}

			if (packet != null)
			{
				if (OnPacketFound != null)
				{
					OnPacketFound(packet);
				}
			}

		}
		private bool CheckForByteArray(byte[] arrayToCheckFor, byte[] buffer, int index)
		{
			var varArray = new byte[arrayToCheckFor.Length];
			Buffer.BlockCopy(buffer, index, varArray, 0, arrayToCheckFor.Length);
			return arrayToCheckFor.SequenceEqual(varArray);
		}



		public static byte[] CreatePacket(string name, byte[] data)
		{
			return GetPacketDataBytes(name, data);
		}
		static byte[] GetPacketDataBytes(string name, byte[] data)
		{
			var HeaderNameBytes = System.Text.Encoding.UTF8.GetBytes(name);
			var HeaderNameLength = BitConverter.GetBytes(HeaderNameBytes.Length);

			var sizeOfPacket = BitConverter.GetBytes(data.Length);
			var totalPacketSize = Variables.StartOfData.Length
				+ sizeOfPacket.Length
				+ HeaderNameLength.Length
				+ HeaderNameBytes.Length
				+ Variables.EndOfHeaderData.Length
				+ data.Length
				+ Variables.EndOfData.Length;
			var array = new byte[totalPacketSize];

			var index = 0;
			Buffer.BlockCopy(Variables.StartOfData, 0, array, index, Variables.StartOfData.Length);
			index += Variables.StartOfData.Length;

			Buffer.BlockCopy(HeaderNameLength, 0, array, index, HeaderNameLength.Length);
			index += HeaderNameLength.Length;

			Buffer.BlockCopy(HeaderNameBytes, 0, array, index, HeaderNameBytes.Length);
			index += HeaderNameBytes.Length;

			Buffer.BlockCopy(sizeOfPacket, 0, array, index, sizeOfPacket.Length);
			index += sizeOfPacket.Length;

			Buffer.BlockCopy(Variables.EndOfHeaderData, 0, array, index, Variables.EndOfHeaderData.Length);
			index += Variables.EndOfHeaderData.Length;

			Buffer.BlockCopy(data, 0, array, index, data.Length);
			index += data.Length;

			Buffer.BlockCopy(Variables.EndOfData, 0, array, index, Variables.EndOfData.Length);

			return array;
		}

	}

	/// <summary>
	/// DataStart - 4 
	/// HeaderNameLength - 4
	/// HeaderName - ? // UTF8
	/// DataLength - 4
	/// HeaderEnd - 4
	/// Contents - ?
	/// Data End - 4
	/// </summary>
	public class Packet
	{
		public int Length;
		public string Name;
		public byte[] Data;
	}

}

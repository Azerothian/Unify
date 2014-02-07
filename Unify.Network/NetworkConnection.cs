using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Network.Interfaces;
using Unify.Util;


namespace Unify.Network
{
	public class NetworkConnection : EventEmitter<NetworkConnection>
	{

		public event GenericVoidDelegate<NetworkConnection> OnConnected;
		public event GenericVoidDelegate<NetworkConnection> OnDisconnected;

		public INetworkConnectionModule ActiveNetworkClient = null;
		public List<INetworkConnectionModule> AvailableNetworkClients = new List<INetworkConnectionModule>();
		public ISerialiser Serialiser = null;
    public string Key
    {
      get
      {
        return ActiveNetworkClient.Key;
      }
    }
		private PacketProcessor packetProcesser = new PacketProcessor();

		public NetworkConnection()
		{
			Initialise();
		}

		public NetworkConnection(INetworkConnectionModule client)
		{
			SetActiveNetworkClient(client);
			Initialise();
		}

		private void Initialise()
		{
			packetProcesser.OnPacketFound += packetProcesser_OnPacketFound;
			packetProcesser.Start();
			OnEmit += UnifyClientv2_OnEmit;

		}

		public void Connect(string ip, int port)
		{
			if(ActiveNetworkClient == null)
			{
				SetActiveNetworkClient(AvailableNetworkClients.First());
			}
			ActiveNetworkClient.Connect(ip, port);
		}


		#region Field Setters
		public void AddNetworkClient<TNetworkClient>() where TNetworkClient : INetworkConnectionModule, new()
		{
			AvailableNetworkClients.Add(new TNetworkClient());
		}

		public void SetSerialiser<TSerialiser>() where TSerialiser : ISerialiser, new()
		{
			Serialiser = new TSerialiser();
		}
		public void SetSerialiser(ISerialiser serialiser)
		{
			Serialiser = serialiser;
		}
		private void SetActiveNetworkClient(INetworkConnectionModule client)
		{
			if(ActiveNetworkClient != null)
			{
				ActiveNetworkClient.OnDataReceive -= ActiveNetworkClient_OnDataReceive;
				ActiveNetworkClient.OnDisconnectedEvent -= ActiveNetworkClient_OnDisconnectedEvent;
				ActiveNetworkClient.OnConnectedEvent -= ActiveNetworkClient_OnConnectedEvent;
			}
			ActiveNetworkClient = client;
			ActiveNetworkClient.OnDataReceive += ActiveNetworkClient_OnDataReceive;
			ActiveNetworkClient.OnDisconnectedEvent += ActiveNetworkClient_OnDisconnectedEvent;
			ActiveNetworkClient.OnConnectedEvent += ActiveNetworkClient_OnConnectedEvent;

		}
		#endregion
		#region Events
		void packetProcesser_OnPacketFound(Packet packet)
		{
			if(packet != null)
			{
				
				var obj = Serialiser.ByteArrayToObject(packet.Data);
				FireAction(packet.Name, this, obj);
			}
		}

		void UnifyClientv2_OnEmit(string name, object sender, object message)
		{
			if (!ActiveNetworkClient.IsDisconnected)
			{
				var messageArray = Serialiser.ObjectToByteArray(message);
				var data = PacketProcessor.CreatePacket(name, messageArray);
				ActiveNetworkClient.Send(data);
			}
		}

		void ActiveNetworkClient_OnConnectedEvent()
		{
			FireAction("connected", this, null);
			if (OnConnected != null)
			{
				OnConnected(this);
			}
		}

		void ActiveNetworkClient_OnDisconnectedEvent()
		{
			FireAction("disconnected", this, null);
			if (OnDisconnected != null)
			{
				OnDisconnected(this);
			}
      lock (packetProcesser)
      {
        packetProcesser.Stop(); //TODO: Opt for reconnect?
      }
		}

		void ActiveNetworkClient_OnDataReceive(byte[] data)
		{
			lock (packetProcesser)
			{
				if (packetProcesser == null)
					throw new Exception("PacketProc is null");
				if (data == null)
					throw new Exception("Data is null");
        lock (packetProcesser)
        {
          packetProcesser.Write(data);
        }
			}
		}
		#endregion


		public void Disconnect()
		{

			ActiveNetworkClient.Disconnect();
		}
	}
}

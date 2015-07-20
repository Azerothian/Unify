using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unify.Network.Interfaces;
using Unify.Util;


namespace Unify.Network.Tcp
{
  public class TcpClient : INetworkConnectionModule
  {
    public event Action OnConnectedEvent;
    public event Action<byte[]> OnDataReceive;
    public event Action<int> OnDataSentEvent;
    public event Action OnDisconnectingEvent;
    public event Action OnDisconnectedEvent;

    private const int receiveBufferSize = 1024;
    private Socket _socket;

    public bool IsDisconnected { get; set; }


    public TcpClient()
    {

    }
    public TcpClient(Socket client)
    {
      _socket = client;
      BeginReceive();
    }
    public void Connect(Uri uri)
    {
      IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port);

      _socket = new Socket(AddressFamily.InterNetwork,
               SocketType.Stream, ProtocolType.Tcp);
      _socket.NoDelay = true;
      IsDisconnected = false;
      _socket.BeginConnect(ipep, EndConnect, _socket);
    }


    public void Disconnect()
    {
      FireDisconnect();
    }

    #region Callbacks
    private void EndConnect(IAsyncResult ar)
    {
      try
      {
        // Retrieve the socket from the state object.
        Socket server = (Socket)ar.AsyncState;

        // Complete the connection.
        server.EndConnect(ar);

        // Signal that the connection has been made.
        if (OnConnectedEvent != null)
        {
          OnConnectedEvent();
        }
        BeginReceive();
      }
      catch (Exception e)
      {
        Log.Critical(e.ToString(), e, this);
      }
    }
    private void BeginReceive()
    {
      try
      {
        // Begin receiving the data from the remote device.
        StateObject state = new StateObject(receiveBufferSize);
        state.ServerSocket = _socket;
        SocketError err;
        _socket.BeginReceive(state.Buffer, 0, receiveBufferSize, SocketFlags.None, out err, EndReceive, state);
        //LogManager.Info("SocketError {0}", err);
      }
      catch (Exception e)
      {

        Log.Critical(e.ToString(), e, this);

      }
    }
    private void EndReceive(IAsyncResult ar)
    {
      if (!IsDisconnected)
      {
        try
        {

          // Retrieve the state object and the client socket 
          // from the asynchronous state object.
          StateObject state = (StateObject)ar.AsyncState;

          if (!state.ServerSocket.Connected)
            return;


          Socket client = state.ServerSocket;

          // Read data from the remote device.
          int bytesRead = -1;
          //	if (client.Connected)
          //	{
          bytesRead = client.EndReceive(ar);

          if (bytesRead > 0)
          {
            //LogManager.Info("EndReceive {0}", bytesRead);
            var data = new byte[bytesRead];

            Buffer.BlockCopy(state.Buffer, 0, data, 0, bytesRead);
            if (OnDataReceive != null)
            {
              OnDataReceive(data);
            }


            client.BeginReceive(state.Buffer, 0, receiveBufferSize, 0, EndReceive, state);
          }
          else
          {
            FireDisconnect();

          }
        }
        catch (SocketException)
        {
          //LogManager.Critical(e.ToString());
          FireDisconnect();

        }
        catch (ObjectDisposedException)
        {
          //LogManager.Critical(e.ToString());
          FireDisconnect();
        }

      }
    }

    private void FireDisconnect()
    {

      if (!IsDisconnected)
      {
        if (_socket.Connected)
        {
          if (OnDisconnectingEvent != null)
          {
            OnDisconnectingEvent();
          }
          _socket.Shutdown(SocketShutdown.Both);
          _socket.Close();
        }

        if (OnDisconnectedEvent != null)
        {
          OnDisconnectedEvent();
        }

        IsDisconnected = true;
      }
    }

    #endregion


    public void Send(byte[] data)
    {
      if (!IsDisconnected)
      {
        _socket.BeginSend(data, 0, data.Length, 0, EndSend, _socket);
      }

    }
    private void EndSend(IAsyncResult ar)
    {
      try
      {
        // Retrieve the socket from the state object.
        Socket client = (Socket)ar.AsyncState;

        // Complete sending the data to the remote device.
        int bytesSent = client.EndSend(ar);
        //Fire sent event here
        if (OnDataSentEvent != null)
        {
          OnDataSentEvent(bytesSent);
        }

      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
    }


    public string Key
    {
      get { return _socket.RemoteEndPoint.ToString(); }
    }
  }
}

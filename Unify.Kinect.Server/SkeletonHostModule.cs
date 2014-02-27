using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unify.Kinect.Messages;
using Unify.Network;
using Unify.Server;
using Unify.Server.Interfaces;
using Unify.Util;

namespace Unify.Kinect.Server
{
  public class SkeletonHostModule : IModule
  {

    private NetworkConnection _currentClient;

    public bool Active = false;
    private KinectUtil _kinectUtil;
    public UnifyServer UnifyServer { get; set; }

    public void Start()
    {
      AutoMapUtil.Initialise();

      _kinectUtil = new KinectUtil();
      _kinectUtil.OnSkeletonsFound += ktil_OnSkeletonsFound;
      _kinectUtil.Start();

    }
    DateTime past = DateTime.Now;

    void ktil_OnSkeletonsFound(Microsoft.Kinect.Skeleton[] skeletons)
    {
      if (Active && ( DateTime.Now - past).TotalMilliseconds > 10)
      {

        var result = (from j in skeletons[0].Joints
                      where j.JointType == JointType.HandLeft
                      select j).FirstOrDefault();
        if (result != null)
        {
          Log.Info("Left Hand {0} {1} {2}", result.Position.X, result.Position.Y, result.Position.Z);
        }

        var messages = ConvertSkeletons(skeletons);
        foreach(var msg in messages)
        {

          _currentClient.Emit<SkeletonMessage>("skeleton", msg);
        }
        past = DateTime.Now;
        
      }
    }

    public IEnumerable<SkeletonMessage> ConvertSkeletons(Skeleton[] skeletons)
    {
      return AutoMapper.Mapper.Map<SkeletonMessage[]>(skeletons);
      
    }

    public void Update()
    {

    }


    public void Stop()
    {
      _kinectUtil.Stop();
    }

    public void OnClientConnected(Guid key, NetworkConnection client)
    {

      Log.Info("Client Connected");
      _currentClient = client;
      Active = true;
    }


    public void OnClientDisconnected(Guid key, NetworkConnection client)
    {
      _currentClient = null;
      Log.Info("Client Disconnected");
      Active = false;
    }

  }
}

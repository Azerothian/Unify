using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Client.Interfaces;
using Unify.Kinect.Messages;
using Unify.Network;
using Unify.Util;

namespace Unify.Kinect.Client
{
  public class SkeletonModule : IModule
  {
    public Unify.Client.UnifyClient UnifyClient { get; set; }
    public void Initialise()
    {
      UnifyClient.Connection.On<SkeletonMessage>("skeleton",
        (NetworkConnection connection, SkeletonMessage response) =>
        {
          var result = (from j in response.Joints
                       where j.JointType == JointTypeMessage.HandLeft
                        select j).FirstOrDefault();
          if (result != null)
          {
            Log.Info("Left Hand {0} {1} {2}", result.Position.X, result.Position.Y, result.Position.Z);
          }

          
        });
    }

    public void OnConnected()
    {
    }

    public void OnDisconnected()
    {

    }


  }
}

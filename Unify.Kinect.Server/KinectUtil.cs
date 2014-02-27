using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unify.Util;

namespace Unify.Kinect.Server
{
  class KinectUtil
  {
    public event GenericVoidDelegate<Skeleton[]> OnSkeletonsFound;
    public KinectSensor GetFirstAvailableSensor()
    {
      foreach (var potentialSensor in KinectSensor.KinectSensors)
      {
        if (potentialSensor.Status == KinectStatus.Connected)
        {
          return potentialSensor;

        }
      }
      return null;
    }

    public KinectSensor CurrentSensor = null;
    public void Start()
    {
      Log.Info("Starting Kinect...");
      CurrentSensor = GetFirstAvailableSensor();
      if (CurrentSensor == null)
        throw new ArgumentException("No Sensor found");

      CurrentSensor.SkeletonStream.Enable();

      // Add an event handler to be called whenever there is new color frame data
      CurrentSensor.SkeletonFrameReady += CurrentSensor_SkeletonFrameReady;

      // Start the sensor!
      try
      {
        CurrentSensor.Start();
        Log.Info("Kinect has started");
      }
      catch (IOException)
      {
        Log.Info("Failed to start kinect");
        CurrentSensor = null;
      }
    }

    void CurrentSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
    {
      if (OnSkeletonsFound != null)
      {
        var frame = e.OpenSkeletonFrame();
        if (frame != null)
        {
          var skeletons = new Skeleton[frame.SkeletonArrayLength];
          frame.CopySkeletonDataTo(skeletons);
          OnSkeletonsFound(skeletons);
        }
      }
    }


    internal void Stop()
    {
      if (null != this.CurrentSensor)
      {
        this.CurrentSensor.Stop();
      }
    }
  }
}

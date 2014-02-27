using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Kinect.Messages
{
  [Serializable]
  public class SkeletonPointMessage
  {
    // Summary:
    //     Gets or sets the X coordinate of the skeleton point.
    public float X { get; set; }
    //
    // Summary:
    //     Gets or sets the Y coordinate of the skeleton point.
    public float Y { get; set; }
    //
    // Summary:
    //     Gets or sets the Z coordinate of the skeleton point.
    public float Z { get; set; }
  }
}

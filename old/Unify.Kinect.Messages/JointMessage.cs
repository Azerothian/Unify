using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Kinect.Messages
{
  [Serializable]
  public class JointMessage
  {
    // Summary:
    //     Gets the joint's type.
    public JointTypeMessage JointType { get; set; }
    //
    // Summary:
    //     Gets or sets the joint's position.
    public SkeletonPointMessage Position { get; set; }
    //
    // Summary:
    //     Gets or sets the tracking state of this joint.
    public JointTrackingStateMessage TrackingState { get; set; }
  }
}

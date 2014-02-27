using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Kinect.Messages
{
  [Serializable]
  public class SkeletonMessage
  {
    // Summary:
    //     Gets or sets the skeleton's bone orientations.
    public BoneOrientationMessage[] BoneOrientations { get; set; }
    //
    // Summary:
    //     Gets or sets the edges that this skeleton is clipped on.
    public FrameEdgesMessage ClippedEdges { get; set; }
    //
    // Summary:
    //     Gets or sets the skeleton's joints.
    public JointMessage[] Joints { get; set; }
    //
    // Summary:
    //     Gets or sets the skeleton's position.
    public SkeletonPointMessage Position { get; set; }
    //
    // Summary:
    //     Gets or sets the skeleton's tracking ID.
    public int TrackingId { get; set; }
    //
    // Summary:
    //     Gets or sets the skeleton's current tracking state.
    public SkeletonTrackingStateMessage TrackingState { get; set; }

  }
}

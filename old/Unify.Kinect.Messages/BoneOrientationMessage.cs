using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Kinect.Messages
{
  [Serializable]
  public class BoneOrientationMessage
  {
    // Summary:
    //     Gets or sets the rotation of the bone relative to the camera coordinate system.
    public BoneRotationMessage AbsoluteRotation { get; set; }
    //
    // Summary:
    //     Gets the skeleton joint at which the bone ends.
    public JointTypeMessage EndJoint { get; internal set; }
    //
    // Summary:
    //     Gets or sets the rotation of the bone relative to its "parent" bone in the
    //     skeleton.
    public BoneRotationMessage HierarchicalRotation { get; set; }
    //
    // Summary:
    //     Gets the skeleton joint at which the bone starts.
    public JointTypeMessage StartJoint { get; internal set; }
  }
}

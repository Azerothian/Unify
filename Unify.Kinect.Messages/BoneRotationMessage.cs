using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Kinect.Messages
{
  [Serializable]
  public class BoneRotationMessage
  {

    // Summary:
    //     Gets or sets a matrix representation of the bone rotation.
    public Matrix4Message Matrix { get; set; }
    //
    // Summary:
    //     Gets or sets a quaternion representation of the bone rotation.
    public Vector4Message Quaternion { get; set; }
  }
}

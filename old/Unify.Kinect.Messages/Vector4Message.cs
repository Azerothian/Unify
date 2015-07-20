using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Kinect.Messages
{
  [Serializable]
  public class Vector4Message
  {
    // Summary:
    //     Gets or sets the W element.
    public float W { get; set; }
    //
    // Summary:
    //     Gets or sets the X element.
    public float X { get; set; }
    //
    // Summary:
    //     Gets or sets the Y element.
    public float Y { get; set; }
    //
    // Summary:
    //     Gets or sets the Z element.
    public float Z { get; set; }
  }
}

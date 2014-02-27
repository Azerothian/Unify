using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Kinect.Messages
{
    // Summary:
  //     These flags are used to indicate of a skeleton is passing outside of the
  //     frame.
  [Flags]
  [Serializable]
  public enum FrameEdgesMessage
  {
    // Summary:
    //     All of the player's body is in frame.
    None = 0,
    //
    // Summary:
    //     Part of the player's body is out of frame to the camera's right.
    Right = 1,
    //
    // Summary:
    //     Part of the player's body is out of frame to the camera's left.
    Left = 2,
    //
    // Summary:
    //     Part of the player's body is out of frame above the camera's field of view.
    Top = 4,
    //
    // Summary:
    //     Part of the player's body is out of frame below the camera's field of view.
    Bottom = 8,
  }
}

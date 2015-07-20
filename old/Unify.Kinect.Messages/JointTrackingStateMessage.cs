using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Kinect.Messages
{
  [Serializable]
  public enum JointTrackingStateMessage
  {
    // Summary:
    //     The joint is not tracked and no data is known about this joint.
    NotTracked = 0,
    //
    // Summary:
    //     The joint location is inferred. The data should be used with the understanding
    //     that the confidence level of the location is very low.
    Inferred = 1,
    //
    // Summary:
    //     The joint is tracked and the data can be trusted.
    Tracked = 2,
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Unify.Ui.Util
{
  public class TouchAssist
  {
    public Touch Touch;

    public Vector2 Position = Vector2.zero;
    public Vector2 LastPosition = Vector2.zero;
    public Vector2 DeltaPosition = Vector2.zero;
    public bool Reset = false;
    public TouchAssist()
    {
      
    }
    public void Update(Touch touch)
    {
      Touch = touch;
      Position = Touch.position;
      switch (Touch.phase)
      {
        case TouchPhase.Began:
          LastPosition = Position;
          break;
        case TouchPhase.Moved:
          //Debug.Log(new { Message = "Moved", Delta = Touch.deltaPosition });
          DeltaPosition = Position - LastPosition;

          break;
        case TouchPhase.Canceled:
        case TouchPhase.Ended:
          DeltaPosition = Vector2.zero;
          break;
      }
      //Debug.Log(new { Message = "SetLastPosition", Delta = Touch.deltaPosition });
      LastPosition = Position;

    }


  }
}

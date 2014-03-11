using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Unify.Ui.Util;
using Unify.Util;
using UnityEngine;

namespace Unify.Ui.Controls
{
  public class TouchPanel : Control
  {
    public TouchPanel() : base()
    {
      OnTouch += TouchPanel_OnTouch;
    }

    public float TouchDistance = 0f;
    public int TouchCount = 0;

    public float PreviousTouchDistance = 0f;

    /// <summary>
    /// [0] float - Diff
    /// </summary>
    public event GenericVoidDelegate<float> OnTouchExpand;
    /// <summary>
    /// [0] float - Diff
    /// </summary>
    public event GenericVoidDelegate<float> OnTouchShrink;

    public float TouchDistancePercentage
    {
      get
      {
        return TouchDistance/ MaxDistance;
      }
    }
    //public bool IsTouched = false;
    public float MaxDistance
    {
      get
      {
        return Vector2.Distance(new Vector2(ActualTop, ActualLeft), new Vector2(ActualTop + ActualHeight, ActualLeft + ActualWidth));
      }
    }

    //public GenericVoidDelegate OnTouch;

    void TouchPanel_OnTouch(IEnumerable<TouchAssist> touches, CancelEventArgs cea )
    {
      var canceld = (from t in touches
                     where t.Touch.phase == TouchPhase.Canceled && t.Touch.phase == TouchPhase.Ended
                select t).Count();

      if (canceld == touches.Count())
      {
        TouchDistance = 0;
        PreviousTouchDistance = 0;
        return;
      }

      var activeTouches = from t in touches
                          where (t.Touch.phase != TouchPhase.Canceled && t.Touch.phase != TouchPhase.Ended)
                   select t;
      TouchCount = touches.Count();
      var dCount = 0;
      var d = 0f;
      foreach (var touch in activeTouches)
      {
        foreach (var touch2 in activeTouches)
        {
          if (touch.Position != touch2.Position)
          {
            dCount++;
            d += Vector2.Distance(touch.Position, touch2.Position);
          }
        }
      }
      TouchDistance = d / dCount;
      
      if (TouchDistance > PreviousTouchDistance && OnTouchExpand != null)
      {
        OnTouchExpand(TouchDistance - PreviousTouchDistance);
      }
      if (TouchDistance < PreviousTouchDistance && OnTouchShrink != null)
      {
        OnTouchShrink(PreviousTouchDistance - TouchDistance);
      }
      PreviousTouchDistance = TouchDistance;
    }

    protected override void OnRender()
    {

    }
    bool Within(Vector2 v2)
    {
      if (v2.x > ActualTop && v2.x < ActualTop + ActualHeight)
      {
        if (v2.y > ActualLeft && v2.x < ActualLeft + ActualWidth)
        {
          return true;
        }
      }
      return false;
    }

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Util;
using UnityEngine;

namespace Unify.Ui.Controls
{
  public class TouchPanel : Control
  {
    public float TouchDistance = 0f;
    public int TouchCount = 0;
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

    protected override void OnStart()
    {
      OnTouch += TouchPanel_OnTouch;
    }

    void TouchPanel_OnTouch(IEnumerable<Touch> touches)
    {
      var activeTouches = from t in touches
                   where (t.phase != TouchPhase.Canceled && t.phase != TouchPhase.Ended)
                   select t;
      TouchCount = touches.Count();

      var dCount = 0;
      var d = 0f;
      foreach (var touch in activeTouches)
      {
        foreach (var touch2 in activeTouches)
        {
          if (touch.position != touch2.position)
          {
            dCount++;
            d += Vector2.Distance(touch.position, touch2.position);
          }
        }
      }
      TouchDistance = d / dCount;
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

    void LateUpdate()
    {
      


     
    }
  }
}

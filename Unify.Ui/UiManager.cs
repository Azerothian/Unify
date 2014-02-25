using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Util;
using UnityEngine;

namespace Unify.Ui
{
  public class UiManager : MonoBehaviour
  {
    public static UiManager Context = null;
    public List<Control> RegisteredControls = new List<Control>();

    public int MouseButtonScan = 3;

    //public GenericVoidDelegate<Vector2,  OnClickPressed;

    public UiManager() : base()
    {
      Context = this;
    }

    void OnGUI()
    {
      foreach (var c in RegisteredControls.OrderBy(p=>p.zIndex))
      {
        if (c.enabled && c.Parent == null)
        {
          c.CallRender();
        }
      }
    }
    void Update()
    {
      if (Input.mousePresent)
      {
        for (var i = 0; i <= MouseButtonScan; i++)
        {
          if (Input.GetMouseButton(i))
          {
            FireMouse(MouseButtonStatus.Pressed, i);
          }
          if (Input.GetMouseButtonUp(i))
          {
            FireMouse(MouseButtonStatus.Up, i);
          }
          if (Input.GetMouseButtonDown(i))
          {
            FireMouse(MouseButtonStatus.Down, i);
          }
        }
      }
      if (Input.touchCount > 0)
      {
        //var arr = from v in Input.touches select v.position;
        var events = new Dictionary<Control, IEnumerable<Touch>>();
        var rcons = RegisteredControls.ToArray();

        foreach (var t in Input.touches)
        {
          var controls = from v in rcons
                         let uiPosition = new Vector2(t.position.x, Screen.height - t.position.y)
                         where v.GetRect().Contains(uiPosition)
                         select v;
          foreach (var c in controls)
          {
            if (!events.ContainsKey(c))
            {
              events.Add(c, new[] { t });
            }
            else
            {
              events[c] = new[] { t }.Concat(events[c]);
            }
          }
        }
        foreach (var c in events.Keys)
        {
          c.FireTouch(events[c]);
        }
      }
    }
    void FireMouse(MouseButtonStatus status, int i)
    {
      var regControls  =RegisteredControls.ToArray();
      var controls = new List<Control>();
      foreach (var v in regControls)
      {
        var position = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        var rect = v.GetRect();
        if (rect.Contains(position))
        {
          Debug.Log(new {  mouse = position, realPos = Input.mousePosition, rect = rect, ActualLeft = v.ActualLeft, ActualTop = v.ActualTop });
          controls.Add(v);
        }
      }
      Debug.Log("Control Count  " + controls.Count());
      foreach (var c in controls)
      {
        switch (status)
        {
          case MouseButtonStatus.Pressed:
            c.FireMouseButtonPressed(i);
            break;
          case MouseButtonStatus.Up:
            c.FireMouseButtonUp(i);
            break;
          case MouseButtonStatus.Down:
            c.FireMouseButtonDown(i);
            break;
        }
        
      }
    }
    public enum MouseButtonStatus
    {
      Pressed,
      Up,
      Down
    }

  }
}

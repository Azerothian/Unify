using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Ui.Util;
using Unify.Util;
using UnityEngine;

namespace Unify.Ui
{
  public class UiManager : MonoBehaviour
  {
    public static UiManager Context = null;
    public List<Control> RegisteredControls = new List<Control>();

    public Control[] GetOrderedControls()
    {
        return RegisteredControls.OrderBy(p => p.zIndex).ToArray();
    }
    public Dictionary<int, TouchAssist> _touchAssists = new Dictionary<int, TouchAssist>();

    public int MouseButtonScan = 3;
    public bool DisableMouse = false;
    public bool DisableTouch = false;

    public UiManager() : base()
    {
      Context = this;
    }

    void OnGUI()
    {
      var controls = GetOrderedControls();
      foreach (var c in controls)
      {
        if (c.enabled && c.Parent == null)
        {
          c.CallRender();
        }
      }
    }
    void Update()
    {
      
      if (!DisableMouse && Input.mousePresent)
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
      if (!DisableTouch && Input.touchCount > 0)
      {
        var events = new Dictionary<Control, IEnumerable<TouchAssist>>();
        var rcons = RegisteredControls.ToArray();
        for(var i = 0; i < Input.touchCount; i ++)
        {
          var t = Input.GetTouch(i);
          if (!_touchAssists.ContainsKey(t.fingerId))
          {
            _touchAssists.Add(t.fingerId, new TouchAssist());
          }
          var touchAssist = _touchAssists[t.fingerId];
          touchAssist.Update(t);
          var controls = from v in rcons
                         let uiPosition = new Vector2(t.position.x, Screen.height - t.position.y)
                         where v.GetRect().Contains(uiPosition)
                         select v;
          foreach (var c in controls)
          {
            if (!events.ContainsKey(c))
            {
              events.Add(c, new[] { touchAssist });
            }
            else
            {
              events[c] = new[] { touchAssist }.Concat(events[c]);
            }
          }
        }
        foreach (var c in events.Keys)
        {
          if (c.FireTouch(events[c]))
            return;
        }
      }
    }
    void FireMouse(MouseButtonStatus status, int i)
    {
      var regControls = GetOrderedControls();
      var controls = new List<Control>();
      foreach (var v in regControls)
      {
        var position = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        var rect = v.GetRect();
        if (rect.Contains(position))
        {
          controls.Add(v);
        }
      }
      foreach (var c in controls)
      {
        switch (status)
        {
          case MouseButtonStatus.Pressed:
            if (c.FireMouseButtonPressed(i))
              return;
            break;
          case MouseButtonStatus.Up:
            if (c.FireMouseButtonUp(i))
              return;
            break;
          case MouseButtonStatus.Down:
            if (c.FireMouseButtonDown(i))
              return;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Unify.Ui
{
  public class UiManager : MonoBehaviour
  {
    public static UiManager Context = null;

    public List<Control> RegisteredControls = new List<Control>();

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

  }
}

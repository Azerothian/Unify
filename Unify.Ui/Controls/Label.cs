using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Unify.Ui.Controls
{
  public class Label : Control
  {
    public Font Font = null;
    public int FontSize = 16;
    GUIStyle _style = null;
    public GUIStyle Style {
      get {
        if (_style == null)
        {
          _style = new GUIStyle(GUI.skin.GetStyle("Label"));
        }
        return _style;
      }
      set
      {
        _style = value;
      }
    }
    public string Text = "";
    protected override void OnRender()
    {
      if (Style.font != Font)
        Style.font = Font;

      if (Style.fontSize != FontSize)
        Style.fontSize = FontSize;

      GUI.Label(new Rect(ActualLeft, ActualTop, ActualWidth, ActualHeight),Text, Style);

    }
  }
}

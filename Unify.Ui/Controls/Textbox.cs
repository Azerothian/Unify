using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Util;
using UnityEngine;

namespace Unify.Ui.Controls
{
  public class Textbox : Control
  {
    public event GenericVoidDelegate<string> OnEnterKeyPressed;
    GUIStyle _style = null;
    public GUIStyle Style
    {
      get
      {
        if (_style == null)
        {
          _style = new GUIStyle(GUI.skin.GetStyle("TextField"));
        }
        return _style;
      }
      set
      {
        _style = value;
      }
    }
    public string Text = "";
    public int MaxLength = 25;
    protected override void OnRender()
    {
      
      if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
      {
        if (OnEnterKeyPressed != null)
        {
          OnEnterKeyPressed(Text);
        }
      }
      Text = GUI.TextField(new Rect(ActualLeft, ActualTop, ActualWidth, ActualHeight), Text, MaxLength, Style);
    }
  }
}

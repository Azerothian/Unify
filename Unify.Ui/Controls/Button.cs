using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Unify.Ui.Controls
{
  public class Button : Control
  {
    public string Label = "";
    public Texture Texture = null;
    GUIStyle _style = null;
    public GUIStyle Style
    {
      get
      {
        if (_style == null)
        {
          _style = new GUIStyle(GUI.skin.GetStyle("Button"));
        }
        return _style;
      }
      set
      {
        _style = value;
      }
    }
    public event EventHandler OnClick;

    protected override void OnRender()
    {
      var rect = new Rect(ActualLeft, ActualTop, ActualWidth, ActualHeight);
      if (Texture != null)
      {
        if (GUI.Button(rect, Texture, Style))
        {
          ProcessOnClick();
        }
      } else
      {
        if (GUI.Button(rect, Label))
        {
          ProcessOnClick();
        }
      }
    }
    private void ProcessOnClick()
    {
      if (OnClick != null)
      {
        OnClick(this, EventArgs.Empty);
      }
    }
    public void SetTexture(Texture tex)
    {
      Texture = tex;
      this.Width = tex.width.ToString();
      this.Height = tex.height.ToString();
      this.Style = GUIStyle.none;


    }
  }
}

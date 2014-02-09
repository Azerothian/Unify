using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Unify.Ui.Controls
{
  public class Box : Control
  {

    public GUIContent Content = new GUIContent();
    GUIStyle _style = null;
    public GUIStyle Style
    {
      get
      {
        if (_style == null)
        {
          _style = new GUIStyle(GUI.skin.GetStyle("Box"));
        }
        return _style;
      }
      set
      {
        _style = value;
      }
    }
    protected override void OnRender()
    {
      GUI.Box(new Rect(ActualLeft, ActualTop, ActualWidth, ActualHeight), Content, Style);

    }
    public void SetTexture(Texture tex)
    {
      this.Content.image = tex;
      this.Width = tex.width.ToString();
      this.Height = tex.height.ToString();
      this.Style = GUIStyle.none;


    }
  }
}

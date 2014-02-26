using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Ui.Util;
using Unify.Util;
using UnityEngine;

namespace Unify.Ui
{
  public class Control : MonoBehaviour
  {

    public Control()
      : base()
    {

    }

    public event GenericVoidDelegate<int> OnMouseButtonPressed;
    public event GenericVoidDelegate<int> OnMouseButtonUp;
    public event GenericVoidDelegate<int> OnMouseButtonDown;
    public event GenericVoidDelegate<IEnumerable<TouchAssist>> OnTouch;

    void Start()
    {
      if (UiManager.Context == null)
      {
        gameObject.AddComponent<UiManager>();
      }

      lock (UiManager.Context.RegisteredControls)
      {
        UiManager.Context.RegisteredControls.Add(this);
      }
      OnStart();
    }

    public Control Parent = null;
    public List<Control> Children = new List<Control>();
    public string Name = "";
    public string Width = "100%";
    public string Height = "100%";

    public string MarginLeft = "0";
    public string MarginRight = "0";
    public string MarginTop = "0";
    public string MarginBottom = "0";
    public int zIndex = 0;
    public float ActualMarginLeft
    {
      get
      {
        return ProcessPercentileString(MarginLeft, GetParentWidth());
      }
    }

    public float ActualMarginRight
    {
      get
      {
        return ProcessPercentileString(MarginRight, GetParentWidth());
      }
    }
    public float ActualMarginTop
    {
      get
      {
        return ProcessPercentileString(MarginTop, GetParentHeight());
      }
    }

    public float ActualMarginBottom
    {
      get
      {
        return ProcessPercentileString(MarginBottom, GetParentHeight());
      }
    }

    public float ActualTop
    {
      get
      {
        if (Parent != null)
        {
          return Parent.ActualTop + ActualMarginTop;
        }
        else
        {
          return ActualMarginTop;
        }

      }

    }
    public float ActualLeft
    {
      get
      {
        if (Parent != null)
        {
          return Parent.ActualLeft + ActualMarginLeft;
        }
        else
        {
          return ActualMarginLeft;
        }

      }
    }
    public float ActualHeight
    {
      get
      {
        var height = ProcessPercentileString(Height, GetParentHeight());
        //height -= ActualMarginTop;
        //  height -= ActualMarginBottom;
        return height;
      }
    }

    public float ActualWidth
    {
      get
      {

        var width = ProcessPercentileString(Width, GetParentWidth());
        //if (Name != "")
        //{
        //  Debug.Log(String.Format("{0} : Width: {1} ActualWidth {2}, ParentWidth {3}", Name, Width, width, GetParentWidth()));
        //}

        //width -= ActualMarginLeft;
        // width -= ActualMarginRight;

        return width;
      }
    }

    public float GetParentHeight()
    {
      if (Parent == null)
      {
        return Screen.height;
      }
      else
      {
        return Parent.ActualHeight;
      }
    }

    public float GetParentWidth()
    {
      var width = 0f;
      if (Parent == null)
      {
        width = Screen.width;
      }
      else
      {
        width = Parent.ActualWidth;
      }
      //Debug.Log(width);
      return width;
    }

    public bool IsMouseOver
    {
      get
      {
        return GetRect().Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));
      }
    }

    public void AddChild(Control child)
    {
      child.Parent = this;
      Children.Add(child);
    }

    public T AddChild<T>() where T : Control, new()
    {

      var child = gameObject.AddComponent<T>();
      child.Parent = this;
      Children.Add(child);
      return child;
    }


    public T CreateControlWithoutParent<T>() where T : Control, new()
    {
      return gameObject.AddComponent<T>();;
    }

    public void CallRender()
    {
      OnRender();
      foreach (var c in Children.OrderBy(p => p.zIndex))
      {
        if (c.enabled)
        {
          c.CallRender();
        }
      }
    }
    protected virtual void OnStart() { }
    
    protected virtual void OnRender() { }

    private float ProcessPercentileString(string str, float mod)
    {
      var val = 0f;
      if (ParsePercentile(str, out val))
      {
        return mod * val;
      }
      else
      {
        return val;
      }
    }

    public bool ParsePercentile(string str, out float val)
    {
      val = 0f;
      if (str.Contains('%'))
      {
        val = float.Parse(str.Replace("%", "")) / 100;
        return true;
      }
      else
      {
        val = float.Parse(str);
      }
      return false;
    }


    internal void FireMouseButtonPressed(int button)
    {
      if (OnMouseButtonPressed != null)
      {
        OnMouseButtonPressed(button);
      }
    }
    internal void FireMouseButtonUp(int button)
    {
      if (OnMouseButtonUp != null)
      {
        OnMouseButtonUp(button);
      }
    }
    internal void FireMouseButtonDown(int button)
    {
      if (OnMouseButtonDown != null)
      {
        OnMouseButtonDown(button);
      }
    }
    internal void FireTouch(IEnumerable<TouchAssist> enumerable)
    {
      if (OnTouch != null)
      {
        OnTouch(enumerable);
      }
    }
    public Rect GetRect()
    {
      return new Rect(ActualLeft, ActualTop, ActualWidth, ActualHeight);
    }



  }
}


using System;
namespace Unify.Ui
{
  public interface IControl
  {
    float ActualHeight { get; }
    float ActualLeft { get; }
    float ActualMarginBottom { get; }
    float ActualMarginLeft { get; }
    float ActualMarginRight { get; }
    float ActualMarginTop { get; }
    float ActualTop { get; }
    float ActualWidth { get; }
    void AddChild(Control child);
    void CallRender();
    float GetParentHeight();
    float GetParentWidth();
    bool ParsePercentile(string str, out float val);
  }
}

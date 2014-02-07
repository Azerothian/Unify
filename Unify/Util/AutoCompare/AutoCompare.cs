using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Util.AutoCompare
{
  public class AutoCompare<TSource>
  {
    private TSource _source;
    List<CompareInformation<TSource>> _compareInformationList = new List<CompareInformation<TSource>>();
    public AutoCompare(TSource source)
    {
      _source = source;
    }

    //Dictionary<object, List<Func<object, object>>> Comparers = new Dictionary<object, List<Func<object, object>>>();
    public delegate void ChangedObjectDelegate(string name, object val);
    public event ChangedObjectDelegate OnChangedObject;

    public AutoCompare<TSource> CreateCompare(string name, Func<TSource, object> selectedField)
    {
      CompareInformation<TSource> newCompare = new CompareInformation<TSource>();
      newCompare.Name = name;
      newCompare.Previous = selectedField((TSource)_source);
      //.Source = source;
      //Func<object, object> newFunc = (object src) =>
      //{
      //  return selectedField((TSource)src);
      //};
      newCompare.Event = selectedField;
      _compareInformationList.Add(newCompare);
      return this;
    }
    public void ValidateObject(TSource source)
    {
      for (var i = 0; i < _compareInformationList.Count; i++)
      {
        if (_compareInformationList[i].Validate(source))
        {
          if (OnChangedObject != null)
          {
            OnChangedObject(_compareInformationList[i].Name, _compareInformationList[i].Previous);
          }
        }
      }
    }

  }
}

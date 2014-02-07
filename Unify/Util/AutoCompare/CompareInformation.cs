using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Util.AutoCompare
{
  public class CompareInformation<TSource>
  {
    public string Name;
    public Func<TSource, object> Event;
    public object Previous;

    public bool Validate(TSource source)
    {
      var result = false;
      var current = Event(source);
      result = !current.Equals(Previous);
      Previous = current;
      return result;
    }

  }
}

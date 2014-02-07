using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Interfaces
{
  public interface ICache
  {
    TValue Get<TValue>(string key, string innerkey);
    IEnumerable<string> GetInnerKeys(string key);
    IEnumerable<object> GetInnerValues(string key);
    bool Set<TValue>(string key, string innerkey, TValue value);
    bool RemoveByKey(string key, string innerkey);
    bool RemoveByKey(string key);

    void CreateKey(string key);
  }
}

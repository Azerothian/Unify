using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Interfaces;

namespace Unify.Cache.Internal
{
  public class InternalCache : ICache
  {
    private Dictionary<string, Dictionary<string, object>> _table = new Dictionary<string, Dictionary<string, object>>();
    public bool Remove(string key)
    {
      _table.Remove(key);
      return true;
    }

    public TValue Get<TValue>(string key, string innerkey)
    {
      if (!_table.ContainsKey(key))
        return default(TValue);
      if (!_table[key].ContainsKey(innerkey))
        return default(TValue);

      return (TValue)_table[key][innerkey];
    }

    public IEnumerable<string> GetInnerKeys(string key)
    {
      if (!_table.ContainsKey(key))
        return null;
      return _table[key].Keys;

    }

    public IEnumerable<object> GetInnerValues(string key)
    {
      if (!_table.ContainsKey(key))
        return null;
      return _table[key].Values.ToArray();
    }

    public bool Set<TValue>(string key, string innerkey, TValue value)
    {

      if (!_table.ContainsKey(key))
      {
        _table.Add(key, new Dictionary<string, object>());
      }
      if (!_table[key].ContainsKey(innerkey))
      {
        _table[key].Add(innerkey, (object)value);
      }
      else
      {
        _table[key][innerkey] = value;
      }
      return true;
    }

    public bool RemoveByKey(string key, string innerkey)
    {
      if (!_table.ContainsKey(key))
        return false;
      if (!_table[key].ContainsKey(innerkey))
        return false;
      return _table[key].Remove(innerkey);
    }

    public bool RemoveByKey(string key)
    {
      if (!_table.ContainsKey(key))
        return false;
      return _table.Remove(key);
    }



    public void CreateKey(string key)
    {
      if (!_table.ContainsKey(key))
      {
        _table[key] = new Dictionary<string, object>();
      }
    }
  }
}

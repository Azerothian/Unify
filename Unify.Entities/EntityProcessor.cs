using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Interfaces;
using Unify.Util;


namespace Unify.Entities
{
  public class EntityProcessor : ThreadHelper, IList<IEntity>, IDisposable, IEnumerable<IEntity>, IEntityProcessor

  {
    private List<IEntity> _items;
    private List<EntityThread> _entityThreads;
    public event Action<IEntity> OnEntityProcessed;
    public event Action<IEntity> OnEntityProcessStart;


    public event Action<IEntity> OnEntityCreated;
    public event Action<IEntity> OnEntityRemoved;


    public int Threads = 4;
    //public int CPUUsageLimit = 50;
    public EntityProcessor()
    {
      ThreadSleep = 1;
      _items = new List<IEntity>();
      _entityThreads = new List<EntityThread>();
    }
    public void Initialise()
    {
      for (int i = 0; i < Threads; i++)
      {
        var et = new EntityThread(this);
        if (OnEntityProcessed != null)
        {
          et.OnEntityProcessed += et_OnEntityProcessed;
        }
        if (OnEntityProcessStart != null)
        {
          et.OnEntityProcessStart += et_OnEntityProcessStart;
        }
        _entityThreads.Add(et);
      }
    }


    void et_OnEntityProcessed(IEntity item1)
    {
      OnEntityProcessed(item1);
    }
    void et_OnEntityProcessStart(IEntity item1)
    {
      OnEntityProcessStart(item1);
    }
    public override void ThreadWorker(TimeSpan timeDiff)
    {

      lock (_entityThreads)
      {
        Stack<IEntity> lastProcessed = null;

        var availableThreads = from v in _entityThreads where v.Entity == null select v;
        if (availableThreads.Count() > 0)
        {

          lock (_items)
          {
            //_items.RemoveAll(p => !p.IsActive);

            var result = (from v in _items
                          where v.IsActive //&& !v.IsLocked
                            && (from et in _entityThreads where v.Equals(et.Entity) select et).Count() == 0
                            && v.LastRun < DateTime.Now
                          orderby v.LastRun
                          select v).Take(Threads);

            lastProcessed = new Stack<IEntity>(result);
          }

          foreach (var et in availableThreads)
          {
            if (lastProcessed.Count > 0)
            {
              et.Entity = lastProcessed.Pop();
            }
            else
            {
              break;
            }

          }

        }
      }
    }

    public int IndexOf(IEntity item)
    {
      return _items.IndexOf(item);
    }

    public void Insert(int index, IEntity item)
    {
      if (OnEntityCreated != null)
      {
        OnEntityCreated(item);
      }
      lock (_items)
      {
        _items.Insert(index, item);
      }
    }

    public void RemoveAt(int index)
    {
      if (OnEntityRemoved != null)
      {
        OnEntityRemoved(_items[index]);
      }
      lock (_items)
      {

        _items.RemoveAt(index);
      }
    }


    public IEntity this[int index]
    {
      get
      {
        return _items[index];
      }
      set
      {
        if (!value.Equals(_items[index]))
        {
          _items[index] = value;
        }
      }
    }

    public void Add(IEntity item)
    {
      if (OnEntityCreated != null)
      {
        OnEntityCreated(item);
      }
      lock (_items)
      {
        _items.Add(item);
      }
    }

    public void Clear()
    {
      lock (_items)
      {
        if (OnEntityRemoved != null)
        {
          foreach (var i in _items)
          {
            OnEntityRemoved(i);
          }
        }
        _items.Clear();
      }
    }

    public bool Contains(IEntity item)
    {
      return _items.Contains(item);
    }

    public void CopyTo(IEntity[] array, int arrayIndex)
    {
      _items.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get { return _items.Count; }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    public bool Remove(IEntity item)
    {
      bool result = false;
      if (OnEntityRemoved != null)
      {
        OnEntityRemoved(item);
      }

      lock (_items)
      {
        result = _items.Remove(item);
      }
      return result;
    }

    public IEnumerator<IEntity> GetEnumerator()
    {
      return _items.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return _items.GetEnumerator();
    }

    public new void Dispose()
    {
      foreach (var et in _entityThreads)
      {
        et.Stop();
      }
      base.Dispose();
    }

    public new void Start()
    {
      foreach (var et in _entityThreads)
      {
        et.Start();
      }
      base.Start();
    }

    public IEnumerable<IEntity> Entities
    {
      get
      {
        return this;
      }
    }
  }
}

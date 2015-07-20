using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Interfaces;
using Unify.Util;


namespace Unify.Entities
{
  public class EntityThread : ThreadHelper
  {
    private EntityProcessor _processor;
    public event Action<IEntity> OnEntityProcessed;
    public event Action<IEntity> OnEntityProcessStart;
    public IEntity Entity { get; set; }
    public EntityThread(EntityProcessor processor)
    {
      _processor = processor;
			
      ThreadSleep = 10;
    }

    public override void ThreadWorker(TimeSpan timeDiff)
    {
      if (Entity != null && Entity.IsActive)
      {
        var diff = DateTime.Now - Entity.LastRun;
        if (diff.TotalMilliseconds > 0)
        {
          if (OnEntityProcessStart != null)
          {
            OnEntityProcessStart(Entity);
          }
          Entity.TimePassed = diff;
          Entity.Process(diff.TotalMilliseconds);
          Entity.LastRun = DateTime.Now;
          if (OnEntityProcessed != null)
          {
            OnEntityProcessed(Entity);
          }
					Entity.PropertiesUpdated = false;
          Entity = default(IEntity);
        }

      }
    }
  }
}

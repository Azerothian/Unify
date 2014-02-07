using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Interfaces;

namespace Unify.Entities
{
  public abstract class Entity : IEntity
  {
    public Entity()
    {
      LastRun = DateTime.Now;
      Properties = new Dictionary<string, object>();
    }
    public abstract void Process(double timePassed);
    public bool IsActive { get; set; }
    public DateTime LastRun { get; set; }
    public TimeSpan TimePassed { get; set; }
    public string Name { get; set; }
    public Dictionary<string, object> Properties { get; set; }
    public bool PropertiesUpdated { get; set; }
  }
}

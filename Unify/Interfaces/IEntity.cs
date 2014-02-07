using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Interfaces
{
  public interface IEntity
  {
    string Name { get; set; }
    bool PropertiesUpdated { get; set; }
    Dictionary<string, object> Properties { get; }
    void Process(double timePassed);
    DateTime LastRun { get; set; }
    TimeSpan TimePassed { get; set; }
    bool IsActive { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Messages.Entities
{
  [Serializable]
  public class EntityCreate
  {
    public string Name { get; set; }
    public string Path { get; set; }
    public Dictionary<string, object> Properties { get; set; }
    public bool IsUser { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Messages.Entities
{
  [Serializable]
  public class EntityUpdateRequest
  {
    public string Name { get; set; }
  }
}

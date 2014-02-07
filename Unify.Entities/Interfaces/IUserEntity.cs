using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Interfaces;
using Unify.Network;

namespace Unify.Entities.Interfaces
{
  public interface IUserEntity
  {
    NetworkConnection NetworkConnection { get; set; }
    bool ValidateEntityUpdate(IEntity entity);
    List<IEntity> TrackingEntities { get; set; }
  }
}

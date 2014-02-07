using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Util;

namespace Unify.Interfaces
{
  public interface IEntityProcessor
  {
    event GenericVoidDelegate<IEntity> OnEntityCreated;
    event GenericVoidDelegate<IEntity> OnEntityProcessed;
    event GenericVoidDelegate<IEntity> OnEntityRemoved;

    IEnumerable<IEntity> Entities { get; }

    void Add(IEntity entity);

    bool Remove(IEntity entity);
		void Initialise();
		void Start();
		void Stop();
	}
}

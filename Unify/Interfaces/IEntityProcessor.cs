using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Util;

namespace Unify.Interfaces
{
  public interface IEntityProcessor
  {
    event Action<IEntity> OnEntityCreated;
    event Action<IEntity> OnEntityProcessed;
    event Action<IEntity> OnEntityRemoved;

    IEnumerable<IEntity> Entities { get; }

    void Add(IEntity entity);

    bool Remove(IEntity entity);
		void Initialise();
		void Start();
		void Stop();
	}
}

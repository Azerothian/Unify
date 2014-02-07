using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Entities;
using Unify.Entities.Interfaces;
using Unify.Interfaces;
using Unify.Messages.Entities;
using Unify.Network;
using Unify.Server.Interfaces;
using Unify.Util;

namespace Unify.Server.Modules.Entities
{
  public class EntityModule : IModule
  {
    public Func<IUserEntity, IEntity, bool> UpdateFilterLogic = (IUserEntity user, IEntity entity) => { return true; };
    public List<IUserEntity> Users { get; set; }
    public IEntityProcessor EntityProcessor { get; set; }

    public UnifyServer UnifyServer { get; set; }

    public Func<NetworkConnection, IUserEntity> CreateUserFunc = (NetworkConnection connection) =>
    {
      Log.Info("[EntityModule] [CreateUserFunc] Creating Basic User Entity..");
      return new UserEntity(connection);
    };

    public void OnClientConnected(Guid key, NetworkConnection connection)
    {
      var entity = CreateUserFunc(connection);
      Users.Add(entity); // need to add to list of users first so the event from entity processor will fire.
      EntityProcessor.Add(entity as IEntity);
    }

    public void OnClientDisconnected(Guid key, Network.NetworkConnection connection)
    {
      var user = (from v in Users 
                  let entity = v is IUserEntity ? (IUserEntity)v : null
                  where entity != null && entity.NetworkConnection == connection
                  select v).FirstOrDefault();
      if (user != null)
      {
        Users.Remove(user);
        EntityProcessor.Remove(user as IEntity);
      }
    }

    public void Start()
    {
			EntityProcessor.OnEntityCreated += EntityProcessor_OnEntityCreated;
			EntityProcessor.OnEntityRemoved += EntityProcessor_OnEntityRemoved;
			EntityProcessor.OnEntityProcessed += EntityProcessor_OnEntityProcessed;

			EntityProcessor.Initialise();

			EntityProcessor.Start();
      Users = new List<IUserEntity>();

    }
    //TODO: server time syncing???

    void EntityProcessor_OnEntityProcessed(IEntity entity)
    {
      if (entity.PropertiesUpdated)
      {
        foreach (var u in Users)
        {

          if (u.TrackingEntities.Contains(entity) && !UpdateFilterLogic(u, entity))
          {

						Log.Info("[Entity][EntityProcessor_OnEntityProcessed] Fire Remove..");
            u.NetworkConnection.Emit<EntityDelete>("entity.remove", new EntityDelete() { Name = entity.Name });
            u.TrackingEntities.Remove(entity);
          }
          else if (UpdateFilterLogic(u, entity))
          {
            if (u.TrackingEntities.Contains(entity))
            {
							Log.Info("[Entity][EntityProcessor_OnEntityProcessed] Fire Update..");
              bool IsUser = IsEntityUser(entity, u.NetworkConnection);
              u.NetworkConnection.Emit<EntityUpdate>("entity.update", new EntityUpdate() { Name = entity.Name, Properties = entity.Properties, IsUser=IsUser });
            }
            else
            {
							Log.Info("[Entity][EntityProcessor_OnEntityProcessed] Fire Create..");
              bool IsUser = IsEntityUser(entity, u.NetworkConnection);
              u.NetworkConnection.Emit<EntityCreate>("entity.create", new EntityCreate() { Name = entity.Name, Properties = entity.Properties, IsUser=IsUser });
              u.TrackingEntities.Add(entity);
            }
          }
        }
      }
    }

    void EntityProcessor_OnEntityRemoved(IEntity entity)
    {
      foreach (var u in Users)
      {
        if (u.TrackingEntities.Contains(entity))
        {
					//Log.Info("[Entity][EntityProcessor_OnEntityRemoved] Fire Remove..");
          u.NetworkConnection.Emit<EntityDelete>("entity.remove", new EntityDelete() { Name = entity.Name });
          u.TrackingEntities.Remove(entity);
        }
      }
    }
    bool IsEntityUser(IEntity entity, NetworkConnection connection)
    {
      if (entity is UserEntity)
      {
        var userEntity = entity as UserEntity;
        if (userEntity.NetworkConnection == connection)
        {
          return true;
        }
      }
      return false;
    }

    void EntityProcessor_OnEntityCreated(IEntity entity)
    {
      
      foreach (var u in Users)
      {
        if (UpdateFilterLogic(u, entity))
        {
          bool IsUser = IsEntityUser(entity, u.NetworkConnection);
         
					//Log.Info("[Entity][EntityProcessor_OnEntityCreated] Fire Create..");
          u.NetworkConnection.Emit<EntityCreate>("entity.create", new EntityCreate() { Name = entity.Name, IsUser= IsUser, Properties = entity.Properties });
          u.TrackingEntities.Add(entity);
        }
      }
    }

    public void Stop()
    {
			EntityProcessor.Stop();
    }

  }
}

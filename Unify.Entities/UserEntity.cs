using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Entities.Interfaces;
using Unify.Interfaces;
using Unify.Network;
using Unify.Util;

namespace Unify.Entities
{
  public class UserEntity : Entity, IUserEntity
  {
    public NetworkConnection NetworkConnection { get; set; }
    public List<IEntity> TrackingEntities { get; set; }
    public UserEntity(NetworkConnection connection)
    {
      TrackingEntities = new List<IEntity>();
      Name = Guid.NewGuid().ToString();
      NetworkConnection = connection;
			IsActive = true;
    }
		double TotalTimePassed = 0;
    public override void Process(double timePassed)
    {
			TotalTimePassed += timePassed;
      if(TotalTimePassed > 5000)
			{
				Log.Info("[UserEntity] Processing !");
				if (Properties.ContainsKey("key"))
					Properties.Remove("key");
				Properties.Add("key", "value");
				PropertiesUpdated = true;
				TotalTimePassed = 0;
			}
    }


    public bool ValidateEntityUpdate(IEntity entity)
    {
      return true;
    }
  }
}

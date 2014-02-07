using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unify.Client.Interfaces;
using Unify.Messages.Entities;
using Unify.Network;
using Unify.Util;

namespace Unify.Client.Modules.Entities
{
	public class EntityModule  : IModule
	{

		public event GenericVoidDelegate<EntityUpdate> OnEntityUpdate;
		public event GenericVoidDelegate<EntityCreate> OnEntityCreate;
		public event GenericVoidDelegate<EntityDelete> OnEntityDelete;
		public UnifyClient UnifyClient {get;set;}
		

		public void OnConnected()
		{

		}

		public void OnDisconnected()
		{

		}

		public void Initialise()
		{
			UnifyClient.Connection.On<EntityUpdate>("entity.update", (NetworkConnection conn, EntityUpdate request) =>
			{
				if (OnEntityUpdate != null)
				{
					OnEntityUpdate(request);
				}

			});
			UnifyClient.Connection.On<EntityCreate>("entity.create", (NetworkConnection conn, EntityCreate request) =>
			{
				if (OnEntityCreate != null)
				{
					OnEntityCreate(request);
				}

			});
			UnifyClient.Connection.On<EntityDelete>("entity.delete", (NetworkConnection conn, EntityDelete request) =>
			{
				if (OnEntityDelete != null)
				{
					OnEntityDelete(request);
				}

			});
		}
	}
}

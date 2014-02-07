using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Util
{
	public class EventEmitter<TSender>
	{
		Dictionary<string, List<Action<TSender, object>>> _actions = new Dictionary<string, List<Action<TSender, object>>>();
		public void FireAction(string eventName, TSender sender, object contents)
		{
			if (_actions.Keys.Contains(eventName))
			{
				//Log.Info("UnifyEventClient - Firing Message {0}", eventName);
				foreach (var act in _actions[eventName])
				{
					act(sender, contents);
				}
			}
		}
    public Action<TSender, object> On<TMessage>(string message, Action<TSender, TMessage> callback)
		{
			if (!_actions.Keys.Contains(message))
			{
				_actions.Add(message, new List<Action<TSender, object>>());
			}
			Action<TSender, object> evnt = (TSender sender, object data) =>
			{
				if (data != null)
				{
					callback(sender, (TMessage)data);
				}
				else
				{
					callback(sender, default(TMessage));
				}
			};

			_actions[message].Add(evnt);
      return evnt;
		}


    public bool Off<TMessage>(string message, Action<TSender, object> callback)
    {
      return _actions[message].Remove(callback);
    }

		//public void Off(string message, Action<TSender, TMessage> callback)
		//{
		//	if (_actions.Keys.Contains(message))
		//	{
		//		if (_actions.Values.Contains(new[] { callback }.ToList()))
		//		{
		//			_actions[message].Remove(callback);
		//		}
		//	}
		//	_actions[message].Add(callback);
		//}

		public event GenericVoidDelegate<string, object, object> OnEmit;
		public void Emit<TMessage>(string name, TMessage contents)
		{
			if(OnEmit != null)
			{
				OnEmit(name, this, contents);
			}
		}
	}
}

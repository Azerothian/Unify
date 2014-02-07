using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Messages.Chat
{
	[Serializable]
	public class ChannelUserListResponse
	{
    public string RoomName { get; set; }
		public IEnumerable<string> Users { get; set; }
	}
}

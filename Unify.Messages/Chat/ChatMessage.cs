using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Messages.Chat
{
	[Serializable]
	public class ChatMessage
	{
		public ChatEventType ChatEventType { get; set; }
		public string Target { get; set; }
    public string From { get; set; }
		public string Message { get; set; }
	}
	public enum ChatEventType
	{
		Message,
		Action,
		PrivateMessage,
    Join,
    Quit,
    Register
    
	}
}

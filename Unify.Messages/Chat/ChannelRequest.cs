using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Messages.Chat
{
  public class ChannelRequest
  {
    public ChannelRequestType ChannelRequestType { get; set; }
    public string Channel { get; set; }

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Messages.Chat
{
  [Serializable]
  public class ChannelResponse
  {
    public bool IsSuccessful { get; set; }
    public string Reason { get; set; }
    public string Channel { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Messages.NickServ
{
  [Serializable]
  public class LoginResponse
  {
    public bool Successful { get; set; }
  }
}

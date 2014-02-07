using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Unify.Network
{
    public class Variables
    {
			public static byte[] EndOfData = new byte[] { 255, 255, 255, 255 };// BitConverter.GetBytes(-1);
			public static byte[] StartOfData = new byte[] { 254, 255, 255, 255 };//BitConverter.GetBytes(-2);
			public static byte[] EndOfHeaderData = new byte[] { 253, 255, 255, 255 };//BitConverter.GetBytes(-3);
    }
}

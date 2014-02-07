using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Unify.Extensions
{
	public static class ByteExtensions
	{
		public static IEnumerable<int> Match(this byte[] source, byte[] pattern)
		{
			for (int i = 0; i < source.Length; i++)
			{
				if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
				{
					yield return i;
				}
			}
		}
		public static byte[] ReadToEnd(this Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}
	}
}

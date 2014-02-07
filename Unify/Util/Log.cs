using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Util
{
	public class Log
	{
		public delegate void LogDelegate(LogType type, string message, params object[] objects);

		public static event LogDelegate OnLog;

		public static void Critical(string message, params object[] objects)
		{
			if (OnLog != null)
			{
				OnLog(LogType.Critical, message, objects);
			}
		}

		public static void Info(string message, params object[] objects)
		{
			if (OnLog != null)
			{
				OnLog(LogType.Information, message, objects);
			}
		}
		[Serializable]
		public enum LogType
		{
			Information,
			Warning,
			Critical

		}

		public static void LogMessage(LogType logType, string message, params object[] objects)
		{
			if (OnLog != null)
			{
				OnLog(logType, message, objects);
			}
		}
	}
}

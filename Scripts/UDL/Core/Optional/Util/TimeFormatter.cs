using System;

namespace UDL.Core
{
	public class TimeFormatter
	{
		public static string SpanToString(TimeSpan span)
		{
			return span.Minutes.ToString("D2") + ":" + span.Seconds.ToString ("D2") + ":" + (span.Milliseconds/10).ToString ("D2");
		}
	}
}


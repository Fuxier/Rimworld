using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200004B RID: 75
	public class LogMessage
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00014B7C File Offset: 0x00012D7C
		public Color Color
		{
			get
			{
				switch (this.type)
				{
				case LogMessageType.Message:
					return Color.white;
				case LogMessageType.Warning:
					return Color.yellow;
				case LogMessageType.Error:
					return ColorLibrary.LogError;
				default:
					return Color.white;
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x00014BBB File Offset: 0x00012DBB
		public string StackTrace
		{
			get
			{
				if (this.stackTrace != null)
				{
					return this.stackTrace;
				}
				return "No stack trace.";
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00014BD1 File Offset: 0x00012DD1
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00014BF5 File Offset: 0x00012DF5
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00014C19 File Offset: 0x00012E19
		public override string ToString()
		{
			if (this.repeats > 1)
			{
				return "(" + this.repeats.ToString() + ") " + this.text;
			}
			return this.text;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00014C4B File Offset: 0x00012E4B
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}

		// Token: 0x04000107 RID: 263
		public string text;

		// Token: 0x04000108 RID: 264
		public LogMessageType type;

		// Token: 0x04000109 RID: 265
		public int repeats = 1;

		// Token: 0x0400010A RID: 266
		private string stackTrace;
	}
}

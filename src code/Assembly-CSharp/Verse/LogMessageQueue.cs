using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200004C RID: 76
	public class LogMessageQueue
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00014C70 File Offset: 0x00012E70
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00014C78 File Offset: 0x00012E78
		public void Enqueue(LogMessage msg)
		{
			if (this.lastMessage != null && msg.CanCombineWith(this.lastMessage))
			{
				this.lastMessage.repeats++;
				return;
			}
			this.lastMessage = msg;
			this.messages.Enqueue(msg);
			if (this.messages.Count > this.maxMessages)
			{
				EditWindow_Log.Notify_MessageDequeued(this.messages.Dequeue());
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00014CE5 File Offset: 0x00012EE5
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}

		// Token: 0x0400010B RID: 267
		public int maxMessages = 200;

		// Token: 0x0400010C RID: 268
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x0400010D RID: 269
		private LogMessage lastMessage;
	}
}

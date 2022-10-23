using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000164 RID: 356
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x06000990 RID: 2448 RVA: 0x0002F0E0 File Offset: 0x0002D2E0
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0002F0F3 File Offset: 0x0002D2F3
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0002F122 File Offset: 0x0002D322
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}

		// Token: 0x040009E1 RID: 2529
		private List<Func<bool>> callbacks = new List<Func<bool>>();
	}
}

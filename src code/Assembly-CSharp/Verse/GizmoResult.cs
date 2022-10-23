using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200049E RID: 1182
	public struct GizmoResult
	{
		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x0600239E RID: 9118 RVA: 0x000E3FB0 File Offset: 0x000E21B0
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x0600239F RID: 9119 RVA: 0x000E3FB8 File Offset: 0x000E21B8
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x000E3FC0 File Offset: 0x000E21C0
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000E3FD0 File Offset: 0x000E21D0
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x040016ED RID: 5869
		private GizmoState stateInt;

		// Token: 0x040016EE RID: 5870
		private Event interactEventInt;
	}
}

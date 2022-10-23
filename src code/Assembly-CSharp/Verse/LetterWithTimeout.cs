using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004AA RID: 1194
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x000E5E20 File Offset: 0x000E4020
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x000E5E2E File Offset: 0x000E402E
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060023FF RID: 9215 RVA: 0x000E5E4F File Offset: 0x000E404F
		public virtual bool LastTickBeforeTimeout
		{
			get
			{
				return this.TimeoutActive && this.disappearAtTick <= Find.TickManager.TicksGame + 1;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x000E5E72 File Offset: 0x000E4072
		public override bool ShouldAutomaticallyOpenLetter
		{
			get
			{
				return this.LastTickBeforeTimeout;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002401 RID: 9217 RVA: 0x000E5E7A File Offset: 0x000E407A
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000E5E91 File Offset: 0x000E4091
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", -1, false);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000E5EAB File Offset: 0x000E40AB
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000E5EC0 File Offset: 0x000E40C0
		protected override string PostProcessedLabel()
		{
			string text = base.PostProcessedLabel();
			if (this.TimeoutActive)
			{
				int num = Mathf.RoundToInt((float)(this.disappearAtTick - Find.TickManager.TicksGame) / 2500f);
				text += " (" + num + "LetterHour".Translate() + ")";
			}
			return text;
		}

		// Token: 0x04001734 RID: 5940
		public int disappearAtTick = -1;
	}
}

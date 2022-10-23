using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000166 RID: 358
	public sealed class GameInfo : IExposable
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0002F24A File Offset: 0x0002D44A
		public float RealPlayTimeInteracting
		{
			get
			{
				return this.realPlayTimeInteracting;
			}
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0002F252 File Offset: 0x0002D452
		public void GameInfoOnGUI()
		{
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseMove || Event.current.type == EventType.KeyDown)
			{
				this.lastInputRealTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0002F288 File Offset: 0x0002D488
		public void GameInfoUpdate()
		{
			if (Time.realtimeSinceStartup < this.lastInputRealTime + 90f && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu && Current.ProgramState == ProgramState.Playing && !Find.WindowStack.IsOpen<Dialog_Options>())
			{
				this.realPlayTimeInteracting += RealTime.realDeltaTime;
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0002F2E0 File Offset: 0x0002D4E0
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.realPlayTimeInteracting, "realPlayTimeInteracting", 0f, false);
			Scribe_Values.Look<bool>(ref this.permadeathMode, "permadeathMode", false, false);
			Scribe_Values.Look<string>(ref this.permadeathModeUniqueName, "permadeathModeUniqueName", null, false);
			Scribe_Values.Look<int>(ref this.startingTile, "startingTile", -1, false);
		}

		// Token: 0x040009E3 RID: 2531
		public bool permadeathMode;

		// Token: 0x040009E4 RID: 2532
		public string permadeathModeUniqueName;

		// Token: 0x040009E5 RID: 2533
		private float realPlayTimeInteracting;

		// Token: 0x040009E6 RID: 2534
		public int startingTile = -1;

		// Token: 0x040009E7 RID: 2535
		private float lastInputRealTime;
	}
}

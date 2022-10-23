using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000168 RID: 360
	[StaticConstructorOnStartup]
	public abstract class LogEntry : IExposable, ILoadReferenceable
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0002F6E7 File Offset: 0x0002D8E7
		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0002F6FA File Offset: 0x0002D8FA
		public int Tick
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0002F702 File Offset: 0x0002D902
		public int LogID
		{
			get
			{
				return this.logID;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0002F6FA File Offset: 0x0002D8FA
		public int Timestamp
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0002F70A File Offset: 0x0002D90A
		public LogEntry(LogEntryDef def = null)
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.def = def;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				this.logID = Find.UniqueIDsManager.GetNextLogID();
			}
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0002F747 File Offset: 0x0002D947
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.logID, "logID", 0, false);
			Scribe_Defs.Look<LogEntryDef>(ref this.def, "def");
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0002F780 File Offset: 0x0002D980
		public string ToGameStringFromPOV(Thing pov, bool forceLog = false)
		{
			if (this.cachedString == null || pov == null != (this.cachedStringPov == null) || (this.cachedStringPov != null && pov != this.cachedStringPov.Target) || DebugViewSettings.logGrammarResolution || forceLog)
			{
				Rand.PushState();
				try
				{
					Rand.Seed = this.logID;
					this.cachedStringPov = ((pov != null) ? new WeakReference<Thing>(pov) : null);
					this.cachedString = this.ToGameStringFromPOV_Worker(pov, forceLog);
					this.cachedHeightWidth = 0f;
					this.cachedHeight = 0f;
				}
				finally
				{
					Rand.PopState();
				}
			}
			return this.cachedString;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0002F82C File Offset: 0x0002DA2C
		protected virtual string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			return GrammarResolver.Resolve("r_logentry", this.GenerateGrammarRequest(), null, forceLog, null, null, null, true);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0002F844 File Offset: 0x0002DA44
		protected virtual GrammarRequest GenerateGrammarRequest()
		{
			return default(GrammarRequest);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0002F85C File Offset: 0x0002DA5C
		public float GetTextHeight(Thing pov, float width)
		{
			string text = this.ToGameStringFromPOV(pov, false);
			if (this.cachedHeightWidth != width)
			{
				this.cachedHeightWidth = width;
				this.cachedHeight = Text.CalcHeight(text, width);
			}
			return this.cachedHeight;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0002F895 File Offset: 0x0002DA95
		protected void ResetCache()
		{
			this.cachedStringPov = null;
			this.cachedString = null;
			this.cachedHeightWidth = 0f;
			this.cachedHeight = 0f;
		}

		// Token: 0x060009AC RID: 2476
		public abstract bool Concerns(Thing t);

		// Token: 0x060009AD RID: 2477
		public abstract IEnumerable<Thing> GetConcerns();

		// Token: 0x060009AE RID: 2478 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool CanBeClickedFromPOV(Thing pov)
		{
			return false;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual Texture2D IconFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0002F8BC File Offset: 0x0002DABC
		public virtual Color? IconColorFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_IdeoRemoved(Ideo ideo)
		{
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0002F8D4 File Offset: 0x0002DAD4
		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(this.Age.ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst() + ".";
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool ShowInCompactView()
		{
			return true;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002F917 File Offset: 0x0002DB17
		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0002F920 File Offset: 0x0002DB20
		public string GetUniqueLoadID()
		{
			return string.Format("LogEntry_{0}_{1}", this.ticksAbs, this.logID);
		}

		// Token: 0x040009F7 RID: 2551
		protected int logID;

		// Token: 0x040009F8 RID: 2552
		protected int ticksAbs = -1;

		// Token: 0x040009F9 RID: 2553
		public LogEntryDef def;

		// Token: 0x040009FA RID: 2554
		private WeakReference<Thing> cachedStringPov;

		// Token: 0x040009FB RID: 2555
		private string cachedString;

		// Token: 0x040009FC RID: 2556
		private float cachedHeightWidth;

		// Token: 0x040009FD RID: 2557
		private float cachedHeight;

		// Token: 0x040009FE RID: 2558
		public static readonly Texture2D Blood = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Blood", true);

		// Token: 0x040009FF RID: 2559
		public static readonly Texture2D BloodTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/BloodTarget", true);

		// Token: 0x04000A00 RID: 2560
		public static readonly Texture2D Downed = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Downed", true);

		// Token: 0x04000A01 RID: 2561
		public static readonly Texture2D DownedTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/DownedTarget", true);

		// Token: 0x04000A02 RID: 2562
		public static readonly Texture2D Skull = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Skull", true);

		// Token: 0x04000A03 RID: 2563
		public static readonly Texture2D SkullTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/SkullTarget", true);
	}
}

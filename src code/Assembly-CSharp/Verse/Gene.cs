using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002AC RID: 684
	public class Gene : IExposable, ILoadReferenceable
	{
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x000776EB File Offset: 0x000758EB
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x000776F8 File Offset: 0x000758F8
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x00077705 File Offset: 0x00075905
		public bool Overridden
		{
			get
			{
				return this.overriddenByGene != null;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x00077710 File Offset: 0x00075910
		public virtual bool Active
		{
			get
			{
				if (this.Overridden)
				{
					return false;
				}
				Pawn pawn = this.pawn;
				return ((pawn != null) ? pawn.ageTracker : null) == null || (float)this.pawn.ageTracker.AgeBiologicalYears >= this.def.minAgeActive;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x0007775C File Offset: 0x0007595C
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				if (!this.Active)
				{
					yield break;
				}
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if ((this.def.disabledWorkTags & list[i].workTags) != WorkTags.None)
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PostMake()
		{
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0007776C File Offset: 0x0007596C
		public virtual void PostAdd()
		{
			if (this.def.HasGraphic)
			{
				this.pawn.Drawer.renderer.graphics.SetGeneGraphicsDirty();
			}
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0007776C File Offset: 0x0007596C
		public virtual void PostRemove()
		{
			if (this.def.HasGraphic)
			{
				this.pawn.Drawer.renderer.graphics.SetGeneGraphicsDirty();
			}
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00077798 File Offset: 0x00075998
		public virtual void Tick()
		{
			if (ModsConfig.BiotechActive && this.def.mentalBreakMtbDays > 0f && this.def.mentalBreakDef != null && this.pawn.Spawned && this.pawn.IsHashIntervalTick(60) && !this.pawn.InMentalState && !this.pawn.Downed && Rand.MTBEventOccurs(this.def.mentalBreakMtbDays, 60000f, 60f) && this.def.mentalBreakDef.Worker.BreakCanOccur(this.pawn))
			{
				this.def.mentalBreakDef.Worker.TryStart(this.pawn, "MentalStateReason_Gene".Translate() + ": " + this.LabelCap, false);
			}
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0007788D File Offset: 0x00075A8D
		public void OverrideBy(Gene overriddenBy)
		{
			if (ModsConfig.BiotechActive)
			{
				this.overriddenByGene = overriddenBy;
			}
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_IngestedThing(Thing thing, int numTaken)
		{
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Reset()
		{
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return null;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return null;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0007789D File Offset: 0x00075A9D
		public string GetUniqueLoadID()
		{
			return "Gene_" + this.loadID;
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x000778B4 File Offset: 0x00075AB4
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<GeneDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Gene>(ref this.overriddenByGene, "overriddenByGene", false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
		}

		// Token: 0x04001044 RID: 4164
		public GeneDef def;

		// Token: 0x04001045 RID: 4165
		public Pawn pawn;

		// Token: 0x04001046 RID: 4166
		public int loadID;

		// Token: 0x04001047 RID: 4167
		public Gene overriddenByGene;
	}
}

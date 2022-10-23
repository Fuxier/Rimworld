using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C6 RID: 710
	public class HediffComp
	{
		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0007CD57 File Offset: 0x0007AF57
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x0007CD64 File Offset: 0x0007AF64
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001459 RID: 5209 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual string CompLabelPrefix
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual string CompDescriptionExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x0007BE0D File Offset: 0x0007A00D
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void CompPostMake()
		{
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void CompExposeData()
		{
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
		{
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual IEnumerable<Gizmo> CompGetGizmos()
		{
			return null;
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual string CompDebugString()
		{
			return null;
		}

		// Token: 0x040010A4 RID: 4260
		public HediffWithComps parent;

		// Token: 0x040010A5 RID: 4261
		public HediffCompProperties props;
	}
}

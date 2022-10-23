using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000AD RID: 173
	public class BodyPartDef : Def
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0001FB7F File Offset: 0x0001DD7F
		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x0001FB87 File Offset: 0x0001DD87
		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x0001FB8F File Offset: 0x0001DD8F
		public string LabelShort
		{
			get
			{
				if (!this.labelShort.NullOrEmpty())
				{
					return this.labelShort;
				}
				return this.label;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0001FBAB File Offset: 0x0001DDAB
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001FBE5 File Offset: 0x0001DDE5
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.frostbiteVulnerability > 10f)
			{
				yield return "frostbitePriority > max 10: " + this.frostbiteVulnerability;
			}
			if (this.solid && this.bleedRate > 0f)
			{
				yield return "solid but bleedRate is not zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001FBF8 File Offset: 0x0001DDF8
		public bool IsSolid(BodyPartRecord part, List<Hediff> hediffs)
		{
			for (BodyPartRecord bodyPartRecord = part; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
			{
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].Part == bodyPartRecord && hediffs[i] is Hediff_AddedPart)
					{
						return hediffs[i].def.addedPartProps.solid;
					}
				}
			}
			return this.solid;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001FC5E File Offset: 0x0001DE5E
		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001FC71 File Offset: 0x0001DE71
		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001FC88 File Offset: 0x0001DE88
		public float GetHitChanceFactorFor(DamageDef damage)
		{
			if (this.conceptual)
			{
				return 0f;
			}
			if (this.hitChanceFactors == null)
			{
				return 1f;
			}
			float result;
			if (this.hitChanceFactors.TryGetValue(damage, out result))
			{
				return result;
			}
			return 1f;
		}

		// Token: 0x040002CA RID: 714
		[MustTranslate]
		public string labelShort;

		// Token: 0x040002CB RID: 715
		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		// Token: 0x040002CC RID: 716
		public int hitPoints = 10;

		// Token: 0x040002CD RID: 717
		public float permanentInjuryChanceFactor = 1f;

		// Token: 0x040002CE RID: 718
		public float bleedRate = 1f;

		// Token: 0x040002CF RID: 719
		public float frostbiteVulnerability;

		// Token: 0x040002D0 RID: 720
		private bool skinCovered;

		// Token: 0x040002D1 RID: 721
		private bool solid;

		// Token: 0x040002D2 RID: 722
		public bool alive = true;

		// Token: 0x040002D3 RID: 723
		public bool delicate;

		// Token: 0x040002D4 RID: 724
		public bool canScarify;

		// Token: 0x040002D5 RID: 725
		public bool beautyRelated;

		// Token: 0x040002D6 RID: 726
		public bool conceptual;

		// Token: 0x040002D7 RID: 727
		public bool socketed;

		// Token: 0x040002D8 RID: 728
		public ThingDef spawnThingOnRemoved;

		// Token: 0x040002D9 RID: 729
		public bool pawnGeneratorCanAmputate;

		// Token: 0x040002DA RID: 730
		public bool canSuggestAmputation = true;

		// Token: 0x040002DB RID: 731
		public bool forceAlwaysRemovable;

		// Token: 0x040002DC RID: 732
		public Dictionary<DamageDef, float> hitChanceFactors;

		// Token: 0x040002DD RID: 733
		public bool destroyableByDamage = true;

		// Token: 0x040002DE RID: 734
		[MustTranslate]
		public string removeRecipeLabelOverride;

		// Token: 0x040002DF RID: 735
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}

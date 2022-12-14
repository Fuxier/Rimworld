using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020002B7 RID: 695
	public class DamageWorker_Cut : DamageWorker_AddInjury
	{
		// Token: 0x060013DA RID: 5082 RVA: 0x00078E66 File Offset: 0x00077066
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside, null);
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x000793EC File Offset: 0x000775EC
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			if (dinfo.HitPart.depth == BodyPartDepth.Inside)
			{
				List<BodyPartRecord> list = new List<BodyPartRecord>();
				for (BodyPartRecord bodyPartRecord = dinfo.HitPart; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
				{
					list.Add(bodyPartRecord);
					if (bodyPartRecord.depth == BodyPartDepth.Outside)
					{
						break;
					}
				}
				float num = (float)(list.Count - 1) + 0.5f;
				for (int i = 0; i < list.Count; i++)
				{
					DamageInfo dinfo2 = dinfo;
					dinfo2.SetHitPart(list[i]);
					base.FinalizeAndAddInjury(pawn, totalDamage / num * ((i == 0) ? 0.5f : 1f), dinfo2, result);
				}
				return;
			}
			int num2 = (this.def.cutExtraTargetsCurve != null) ? GenMath.RoundRandom(this.def.cutExtraTargetsCurve.Evaluate(Rand.Value)) : 0;
			List<BodyPartRecord> list2;
			if (num2 != 0)
			{
				IEnumerable<BodyPartRecord> enumerable = dinfo.HitPart.GetDirectChildParts();
				if (dinfo.HitPart.parent != null)
				{
					enumerable = enumerable.Concat(dinfo.HitPart.parent);
					if (dinfo.HitPart.parent.parent != null)
					{
						enumerable = enumerable.Concat(dinfo.HitPart.parent.GetDirectChildParts());
					}
				}
				list2 = (from x in enumerable.Except(dinfo.HitPart).InRandomOrder(null).Take(num2)
				where !x.def.conceptual
				select x).ToList<BodyPartRecord>();
			}
			else
			{
				list2 = new List<BodyPartRecord>();
			}
			list2.Add(dinfo.HitPart);
			float num3 = totalDamage * (1f + this.def.cutCleaveBonus) / ((float)list2.Count + this.def.cutCleaveBonus);
			if (num2 == 0)
			{
				num3 = base.ReduceDamageToPreserveOutsideParts(num3, dinfo, pawn);
			}
			for (int j = 0; j < list2.Count; j++)
			{
				DamageInfo dinfo3 = dinfo;
				dinfo3.SetHitPart(list2[j]);
				base.FinalizeAndAddInjury(pawn, num3, dinfo3, result);
			}
		}
	}
}

using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200033D RID: 829
	public class Hediff_AddedPart : Hediff_Implant
	{
		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001629 RID: 5673 RVA: 0x00082F34 File Offset: 0x00081134
		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.TipStringExtra);
				stringBuilder.AppendLine("Efficiency".Translate() + ": " + this.def.addedPartProps.partEfficiency.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00082F94 File Offset: 0x00081194
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.pawn.health.RestorePart(base.Part, this, false);
			for (int i = 0; i < base.Part.parts.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
				hediff_MissingPart.IsFresh = true;
				hediff_MissingPart.lastInjury = HediffDefOf.SurgicalCut;
				hediff_MissingPart.Part = base.Part.parts[i];
				this.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null, null);
			}
		}
	}
}

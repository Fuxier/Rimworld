using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000360 RID: 864
	public class ImmunityRecord : IExposable
	{
		// Token: 0x06001732 RID: 5938 RVA: 0x00088414 File Offset: 0x00086614
		public void ExposeData()
		{
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Defs.Look<HediffDef>(ref this.source, "source");
			Scribe_Values.Look<float>(ref this.immunity, "immunity", 0f, false);
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x0008844C File Offset: 0x0008664C
		public void ImmunityTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			this.immunity += this.ImmunityChangePerTick(pawn, sick, diseaseInstance);
			this.immunity = Mathf.Clamp01(this.immunity);
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x00088478 File Offset: 0x00086678
		public float ImmunityChangePerTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.hediffDef.CompProps<HediffCompProperties_Immunizable>();
			if (sick)
			{
				float num = hediffCompProperties_Immunizable.immunityPerDaySick;
				num *= pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true, -1);
				if (diseaseInstance != null)
				{
					Rand.PushState();
					Rand.Seed = Gen.HashCombineInt(diseaseInstance.loadID ^ Find.World.info.persistentRandomValue, 156482735);
					num *= Mathf.Lerp(0.8f, 1.2f, Rand.Value);
					Rand.PopState();
				}
				return num / 60000f;
			}
			return hediffCompProperties_Immunizable.immunityPerDayNotSick / 60000f;
		}

		// Token: 0x040011CD RID: 4557
		public HediffDef hediffDef;

		// Token: 0x040011CE RID: 4558
		public HediffDef source;

		// Token: 0x040011CF RID: 4559
		public float immunity;
	}
}

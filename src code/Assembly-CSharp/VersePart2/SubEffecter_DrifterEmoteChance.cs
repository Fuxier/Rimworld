using System;

namespace Verse
{
	// Token: 0x0200007A RID: 122
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		// Token: 0x060004B2 RID: 1202 RVA: 0x0001A826 File Offset: 0x00018A26
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001A864 File Offset: 0x00018A64
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float chancePerTick = this.def.chancePerTick;
			if (Rand.Value < chancePerTick)
			{
				base.MakeMote(A, -1);
			}
		}
	}
}

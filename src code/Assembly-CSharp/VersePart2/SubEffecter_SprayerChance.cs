using System;

namespace Verse
{
	// Token: 0x02000551 RID: 1361
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		// Token: 0x06002991 RID: 10641 RVA: 0x00109FF4 File Offset: 0x001081F4
		public SubEffecter_SprayerChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x0010A000 File Offset: 0x00108200
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float num = this.def.chancePerTick;
			if (this.def.spawnLocType == MoteSpawnLocType.RandomCellOnTarget && B.HasThing)
			{
				num *= (float)(B.Thing.def.size.x * B.Thing.def.size.z);
			}
			if (Rand.Value < num)
			{
				base.MakeMote(A, B, -1);
			}
		}
	}
}

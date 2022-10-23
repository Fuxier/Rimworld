using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A2 RID: 674
	public class DamageFlasher
	{
		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x00073851 File Offset: 0x00071A51
		private int DamageFlashTicksLeft
		{
			get
			{
				return this.lastDamageTick + 16 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x00073867 File Offset: 0x00071A67
		public bool FlashingNowOrRecently
		{
			get
			{
				return this.DamageFlashTicksLeft >= -1;
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00073875 File Offset: 0x00071A75
		public DamageFlasher(Pawn pawn)
		{
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00073888 File Offset: 0x00071A88
		public Material GetDamagedMat(Material baseMat)
		{
			return DamagedMatPool.GetDamageFlashMat(baseMat, (float)this.DamageFlashTicksLeft / 16f);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0007389D File Offset: 0x00071A9D
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.harmsHealth)
			{
				this.lastDamageTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04000FE3 RID: 4067
		private int lastDamageTick = -9999;

		// Token: 0x04000FE4 RID: 4068
		private const int DamagedMatTicksTotal = 16;
	}
}

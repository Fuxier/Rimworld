using System;

namespace Verse
{
	// Token: 0x020000E9 RID: 233
	public class ExtraDamage
	{
		// Token: 0x06000692 RID: 1682 RVA: 0x00023911 File Offset: 0x00021B11
		public float AdjustedDamageAmount(Verb verb, Pawn caster)
		{
			return this.amount * verb.verbProps.GetDamageFactorFor(verb, caster);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00023927 File Offset: 0x00021B27
		public float AdjustedArmorPenetration(Verb verb, Pawn caster)
		{
			if (this.armorPenetration < 0f)
			{
				return this.AdjustedDamageAmount(verb, caster) * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002394B File Offset: 0x00021B4B
		public float AdjustedArmorPenetration()
		{
			if (this.armorPenetration < 0f)
			{
				return this.amount * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x040004EC RID: 1260
		public DamageDef def;

		// Token: 0x040004ED RID: 1261
		public float amount;

		// Token: 0x040004EE RID: 1262
		public float armorPenetration = -1f;

		// Token: 0x040004EF RID: 1263
		public float chance = 1f;
	}
}

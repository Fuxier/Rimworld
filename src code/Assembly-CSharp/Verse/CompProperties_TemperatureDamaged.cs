using System;

namespace Verse
{
	// Token: 0x020000C3 RID: 195
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		// Token: 0x0600060C RID: 1548 RVA: 0x000208CB File Offset: 0x0001EACB
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}

		// Token: 0x04000382 RID: 898
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		// Token: 0x04000383 RID: 899
		public int damagePerTickRare = 1;
	}
}

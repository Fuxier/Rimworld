using System;

namespace Verse
{
	// Token: 0x020000C0 RID: 192
	public class CompProperties_AffectsSky : CompProperties
	{
		// Token: 0x06000609 RID: 1545 RVA: 0x00020844 File Offset: 0x0001EA44
		public CompProperties_AffectsSky()
		{
			this.compClass = typeof(CompAffectsSky);
		}

		// Token: 0x04000377 RID: 887
		public float glow = 1f;

		// Token: 0x04000378 RID: 888
		public SkyColorSet skyColors;

		// Token: 0x04000379 RID: 889
		public float lightsourceShineSize = 1f;

		// Token: 0x0400037A RID: 890
		public float lightsourceShineIntensity = 1f;

		// Token: 0x0400037B RID: 891
		public bool lerpDarken;
	}
}

using System;

namespace Verse
{
	// Token: 0x020001DA RID: 474
	public static class PsychGlowUtility
	{
		// Token: 0x06000D30 RID: 3376 RVA: 0x0004A114 File Offset: 0x00048314
		public static string GetLabel(this PsychGlow gl)
		{
			switch (gl)
			{
			case PsychGlow.Dark:
				return "Dark".Translate();
			case PsychGlow.Lit:
				return "Lit".Translate();
			case PsychGlow.Overlit:
				return "LitBrightly".Translate();
			default:
				throw new ArgumentException();
			}
		}
	}
}

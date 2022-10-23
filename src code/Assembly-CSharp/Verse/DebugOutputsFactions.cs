using System;

namespace Verse
{
	// Token: 0x02000444 RID: 1092
	public class DebugOutputsFactions
	{
		// Token: 0x06002143 RID: 8515 RVA: 0x000CC2E2 File Offset: 0x000CA4E2
		[DebugOutput("Factions", false)]
		public static void AllFactions()
		{
			Find.FactionManager.LogAllFactions();
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x000CC2EE File Offset: 0x000CA4EE
		[DebugOutput("Factions", false)]
		public static void AllFactionsToRemove()
		{
			Find.FactionManager.LogFactionsToRemove();
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000CC2FA File Offset: 0x000CA4FA
		[DebugOutput("Factions", false)]
		public static void AllFactionsFromPawns()
		{
			Find.FactionManager.LogFactionsOnPawns();
		}
	}
}

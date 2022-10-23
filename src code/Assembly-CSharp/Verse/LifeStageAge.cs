using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D8 RID: 216
	[StaticConstructorOnStartup]
	public class LifeStageAge
	{
		// Token: 0x0600063F RID: 1599 RVA: 0x000220F0 File Offset: 0x000202F0
		public Texture2D GetIcon(Pawn forPawn)
		{
			if (this.def.iconTex != null)
			{
				return this.def.iconTex;
			}
			int count = forPawn.RaceProps.lifeStageAges.Count;
			int num = forPawn.RaceProps.lifeStageAges.IndexOf(this);
			if (num == count - 1)
			{
				return LifeStageAge.AdultIcon;
			}
			if (num == 0)
			{
				return LifeStageAge.VeryYoungIcon;
			}
			return LifeStageAge.YoungIcon;
		}

		// Token: 0x04000432 RID: 1074
		public LifeStageDef def;

		// Token: 0x04000433 RID: 1075
		public float minAge;

		// Token: 0x04000434 RID: 1076
		public SoundDef soundCall;

		// Token: 0x04000435 RID: 1077
		public SoundDef soundAngry;

		// Token: 0x04000436 RID: 1078
		public SoundDef soundWounded;

		// Token: 0x04000437 RID: 1079
		public SoundDef soundDeath;

		// Token: 0x04000438 RID: 1080
		public SoundDef soundAmbience;

		// Token: 0x04000439 RID: 1081
		private static readonly Texture2D VeryYoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/VeryYoung", true);

		// Token: 0x0400043A RID: 1082
		private static readonly Texture2D YoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Young", true);

		// Token: 0x0400043B RID: 1083
		private static readonly Texture2D AdultIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Adult", true);
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200057C RID: 1404
	public static class LifeStageUtility
	{
		// Token: 0x06002B11 RID: 11025 RVA: 0x00113790 File Offset: 0x00111990
		public static void PlayNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> lifestageGetter, Func<GeneDef, SoundDef> geneGetter, float volumeFactor = 1f)
		{
			SoundDef soundOverrideFromGenes;
			float voxPitch;
			float voxVolume;
			LifeStageUtility.GetNearestLifestageSound(pawn, lifestageGetter, out soundOverrideFromGenes, out voxPitch, out voxVolume);
			if (ModsConfig.BiotechActive && pawn.genes != null && geneGetter != null)
			{
				soundOverrideFromGenes = pawn.genes.GetSoundOverrideFromGenes(geneGetter, soundOverrideFromGenes);
				voxPitch = pawn.ageTracker.CurLifeStage.voxPitch;
				voxVolume = pawn.ageTracker.CurLifeStage.voxVolume;
			}
			if (soundOverrideFromGenes == null)
			{
				return;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.PositionHeld, pawn.MapHeld, false), MaintenanceType.None);
			info.pitchFactor = voxPitch;
			info.volumeFactor = voxVolume * volumeFactor;
			soundOverrideFromGenes.PlayOneShot(info);
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x0011382C File Offset: 0x00111A2C
		private static void GetNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, out SoundDef def, out float pitch, out float volume)
		{
			int num = pawn.ageTracker.CurLifeStageIndex;
			LifeStageAge lifeStageAge;
			for (;;)
			{
				lifeStageAge = pawn.RaceProps.lifeStageAges[num];
				def = getter(lifeStageAge);
				if (def != null)
				{
					break;
				}
				num++;
				if (num < 0 || num >= pawn.RaceProps.lifeStageAges.Count)
				{
					goto IL_84;
				}
			}
			pitch = pawn.ageTracker.CurLifeStage.voxPitch / lifeStageAge.def.voxPitch;
			volume = pawn.ageTracker.CurLifeStage.voxVolume / lifeStageAge.def.voxVolume;
			return;
			IL_84:
			def = null;
			pitch = (volume = 1f);
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x001138D0 File Offset: 0x00111AD0
		private static LifeStageAge GetLifeStageAgeForYears(Pawn pawn, float years)
		{
			List<LifeStageAge> lifeStageAges = pawn.RaceProps.lifeStageAges;
			int num = 0;
			int num2 = 1;
			while (num2 < lifeStageAges.Count && lifeStageAges[num2].minAge < years)
			{
				num++;
				num2++;
			}
			return lifeStageAges[num];
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x00113916 File Offset: 0x00111B16
		public static DevelopmentalStage CalculateDevelopmentalStage(Pawn pawn, float years)
		{
			return LifeStageUtility.GetLifeStageAgeForYears(pawn, years).def.developmentalStage;
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x0011392C File Offset: 0x00111B2C
		public static float GetMaxBabyAge(RaceProperties raceProps)
		{
			List<LifeStageAge> lifeStageAges = raceProps.lifeStageAges;
			for (int i = 0; i < lifeStageAges.Count; i++)
			{
				if (!lifeStageAges[i].def.developmentalStage.Baby())
				{
					return lifeStageAges[i].minAge;
				}
			}
			return 0f;
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x0011397C File Offset: 0x00111B7C
		public static bool AlwaysDowned(Pawn pawn)
		{
			Pawn_AgeTracker ageTracker = pawn.ageTracker;
			bool? flag;
			if (ageTracker == null)
			{
				flag = null;
			}
			else
			{
				LifeStageDef curLifeStage = ageTracker.CurLifeStage;
				flag = ((curLifeStage != null) ? new bool?(curLifeStage.alwaysDowned) : null);
			}
			return flag ?? false;
		}
	}
}

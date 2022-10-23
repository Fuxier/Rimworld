using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;

// Token: 0x02000018 RID: 24
public static class DarknessCombatUtility
{
	// Token: 0x0600007D RID: 125 RVA: 0x000057A8 File Offset: 0x000039A8
	public static bool IsOutdoorsAndDark(Thing thing)
	{
		return DarknessCombatUtility.Outdoors(thing) && !DarknessCombatUtility.IsOutdoorsLit(thing.Map);
	}

	// Token: 0x0600007E RID: 126 RVA: 0x000057C2 File Offset: 0x000039C2
	public static bool IsOutdoorsAndLit(Thing thing)
	{
		return DarknessCombatUtility.Outdoors(thing) && DarknessCombatUtility.IsOutdoorsLit(thing.Map);
	}

	// Token: 0x0600007F RID: 127 RVA: 0x000057D9 File Offset: 0x000039D9
	public static bool IsIndoorsAndDark(Thing thing)
	{
		return !DarknessCombatUtility.Outdoors(thing) && (thing.Map.glowGrid.PsychGlowAt(thing.Position) == PsychGlow.Dark || DarklightUtility.IsDarklightAt(thing.Position, thing.Map));
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00005810 File Offset: 0x00003A10
	public static bool IsIndoorsAndLit(Thing thing)
	{
		return !DarknessCombatUtility.Outdoors(thing) && thing.Map.glowGrid.PsychGlowAt(thing.Position) == PsychGlow.Lit && !DarklightUtility.IsDarklightAt(thing.Position, thing.Map);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x0000584C File Offset: 0x00003A4C
	private static bool Outdoors(Thing thing)
	{
		RoofDef roof = thing.Position.GetRoof(thing.Map);
		if (roof != null && (roof.isNatural || roof.isThickRoof))
		{
			return false;
		}
		Room room = thing.Position.GetRoom(thing.Map);
		return room != null && room.PsychologicallyOutdoors;
	}

	// Token: 0x06000082 RID: 130 RVA: 0x0000589D File Offset: 0x00003A9D
	private static bool IsOutdoorsLit(Map map)
	{
		return map.skyManager.CurSkyGlow > 0.35f;
	}

	// Token: 0x06000083 RID: 131 RVA: 0x000058B1 File Offset: 0x00003AB1
	public static IEnumerable<StatDrawEntry> GetStatEntriesForPawn(Pawn pawn)
	{
		if (pawn.Ideo == null)
		{
			yield break;
		}
		List<Precept> precepts = pawn.Ideo.PreceptsListForReading;
		DarknessCombatUtility.tmpStatDefs.Clear();
		for (int i = 0; i < precepts.Count; i++)
		{
			List<StatModifier> statOffsets = precepts[i].def.statOffsets;
			if (statOffsets != null)
			{
				for (int j = 0; j < statOffsets.Count; j++)
				{
					StatModifier statModifier = statOffsets[j];
					if (statModifier.stat == StatDefOf.ShootingAccuracyOutdoorsLitOffset || statModifier.stat == StatDefOf.ShootingAccuracyOutdoorsDarkOffset || statModifier.stat == StatDefOf.ShootingAccuracyIndoorsDarkOffset || statModifier.stat == StatDefOf.ShootingAccuracyIndoorsLitOffset)
					{
						DarknessCombatUtility.tmpStatDefs.Add(statModifier);
					}
				}
			}
		}
		if (DarknessCombatUtility.tmpStatDefs.Count > 0)
		{
			Ideo ideo = pawn.Ideo;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Stat_Pawn_DarknessCombatShooting_Desc".Translate() + " " + "Stat_PawnDarkness_FollowingOffset".Translate() + ":");
			stringBuilder.AppendLine();
			for (int k = 0; k < DarknessCombatUtility.tmpStatDefs.Count; k++)
			{
				stringBuilder.AppendLine(DarknessCombatUtility.tmpStatDefs[k].stat.LabelCap + ": " + DarknessCombatUtility.tmpStatDefs[k].ValueToStringAsOffset);
			}
			stringBuilder.AppendLine("\n" + "CausedBy".Translate() + ": " + "BeliefInIdeo".Translate() + " " + ideo.name);
			yield return new StatDrawEntry(StatCategoryDefOf.PawnCombat, "Stat_Pawn_DarknessCombatShooting_Name".Translate(), DarknessCombatUtility.tmpStatDefs.MinBy((StatModifier s) => s.value).ValueToStringAsOffset + " ~ " + DarknessCombatUtility.tmpStatDefs.MaxBy((StatModifier s) => s.value).ValueToStringAsOffset, stringBuilder.ToString(), 4051, null, new Dialog_InfoCard.Hyperlink[]
			{
				new Dialog_InfoCard.Hyperlink(ideo)
			}, false);
		}
		DarknessCombatUtility.tmpStatDefs.Clear();
		for (int l = 0; l < precepts.Count; l++)
		{
			List<StatModifier> statOffsets2 = precepts[l].def.statOffsets;
			if (statOffsets2 != null)
			{
				for (int m = 0; m < statOffsets2.Count; m++)
				{
					StatModifier statModifier2 = statOffsets2[m];
					if (statModifier2.stat == StatDefOf.MeleeHitChanceIndoorsDarkOffset || statModifier2.stat == StatDefOf.MeleeHitChanceIndoorsLitOffset || statModifier2.stat == StatDefOf.MeleeHitChanceOutdoorsLitOffset || statModifier2.stat == StatDefOf.MeleeHitChanceOutdoorsDarkOffset)
					{
						DarknessCombatUtility.tmpStatDefs.Add(statModifier2);
					}
				}
			}
		}
		if (DarknessCombatUtility.tmpStatDefs.Count > 0)
		{
			Ideo ideo2 = pawn.Ideo;
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.AppendLine("Stat_Pawn_DarknessMeleeHitChance_Desc".Translate() + " " + "Stat_PawnDarkness_FollowingOffset".Translate() + ":");
			stringBuilder2.AppendLine();
			for (int n = 0; n < DarknessCombatUtility.tmpStatDefs.Count; n++)
			{
				stringBuilder2.AppendLine(DarknessCombatUtility.tmpStatDefs[n].stat.LabelCap + ": " + DarknessCombatUtility.tmpStatDefs[n].ValueToStringAsOffset);
			}
			stringBuilder2.AppendLine("\n" + "CausedBy".Translate() + ": " + "BeliefInIdeo".Translate() + " " + ideo2.name);
			yield return new StatDrawEntry(StatCategoryDefOf.PawnCombat, "Stat_Pawn_DarknessMeleeHitChance_Name".Translate(), DarknessCombatUtility.tmpStatDefs.MinBy((StatModifier s) => s.value).ValueToStringAsOffset + " ~ " + DarknessCombatUtility.tmpStatDefs.MaxBy((StatModifier s) => s.value).ValueToStringAsOffset, stringBuilder2.ToString(), 4101, null, new Dialog_InfoCard.Hyperlink[]
			{
				new Dialog_InfoCard.Hyperlink(ideo2)
			}, false);
		}
		DarknessCombatUtility.tmpStatDefs.Clear();
		for (int num = 0; num < precepts.Count; num++)
		{
			List<StatModifier> statOffsets3 = precepts[num].def.statOffsets;
			if (statOffsets3 != null)
			{
				for (int num2 = 0; num2 < statOffsets3.Count; num2++)
				{
					StatModifier statModifier3 = statOffsets3[num2];
					if (statModifier3.stat == StatDefOf.MeleeDodgeChanceIndoorsDarkOffset || statModifier3.stat == StatDefOf.MeleeDodgeChanceIndoorsLitOffset || statModifier3.stat == StatDefOf.MeleeDodgeChanceOutdoorsLitOffset || statModifier3.stat == StatDefOf.MeleeDodgeChanceOutdoorsDarkOffset)
					{
						DarknessCombatUtility.tmpStatDefs.Add(statModifier3);
					}
				}
			}
		}
		if (DarknessCombatUtility.tmpStatDefs.Count > 0)
		{
			Ideo ideo3 = pawn.Ideo;
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.AppendLine("Stat_Pawn_DarknessMeleeDodgeChance_Desc".Translate() + " " + "Stat_PawnDarkness_FollowingOffset".Translate() + ":");
			stringBuilder3.AppendLine();
			for (int num3 = 0; num3 < DarknessCombatUtility.tmpStatDefs.Count; num3++)
			{
				stringBuilder3.AppendLine(DarknessCombatUtility.tmpStatDefs[num3].stat.LabelCap + ": " + DarknessCombatUtility.tmpStatDefs[num3].ValueToStringAsOffset);
			}
			stringBuilder3.AppendLine("\n" + "CausedBy".Translate() + ": " + "BeliefInIdeo".Translate() + " " + ideo3.name);
			yield return new StatDrawEntry(StatCategoryDefOf.PawnCombat, "Stat_Pawn_DarknessMeleeDodgeChance_Name".Translate(), DarknessCombatUtility.tmpStatDefs.MinBy((StatModifier s) => s.value).ValueToStringAsOffset + " ~ " + DarknessCombatUtility.tmpStatDefs.MaxBy((StatModifier s) => s.value).ValueToStringAsOffset, stringBuilder3.ToString(), 4101, null, new Dialog_InfoCard.Hyperlink[]
			{
				new Dialog_InfoCard.Hyperlink(ideo3)
			}, false);
		}
		DarknessCombatUtility.tmpStatDefs.Clear();
		yield break;
	}

	// Token: 0x04000034 RID: 52
	private const float SkyGlowDarkThreshold = 0.35f;

	// Token: 0x04000035 RID: 53
	private static List<StatModifier> tmpStatDefs = new List<StatModifier>();
}

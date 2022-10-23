﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

// Token: 0x02000017 RID: 23
public static class CallBossgroupUtility
{
	// Token: 0x0600007A RID: 122 RVA: 0x00005560 File Offset: 0x00003760
	public static void TryStartSummonBossgroupJob(BossgroupDef def, Pawn pawn, bool forced = true)
	{
		ThingDef bossgroupCaller = CallBossgroupUtility.GetBossgroupCaller(def);
		List<Thing> list = pawn.Map.listerThings.ThingsOfDef(bossgroupCaller);
		list.SortBy((Thing t) => t.Position.DistanceToSquared(pawn.Position));
		for (int i = 0; i < list.Count; i++)
		{
			string text;
			if (pawn.CanReserveAndReach(list[i], PathEndMode.Touch, Danger.Deadly, 1, -1, null, forced) && list[i].TryGetComp<CompUsable>().CanBeUsedBy(pawn, out text))
			{
				list[i].TryGetComp<CompUsable>().TryStartUseJob(pawn, null, forced);
				return;
			}
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00005614 File Offset: 0x00003814
	public static AcceptanceReport BossgroupEverCallable(Pawn pawn, BossgroupDef def, bool forced = true)
	{
		if (!pawn.Spawned)
		{
			return false;
		}
		ThingDef bossgroupCaller = CallBossgroupUtility.GetBossgroupCaller(def);
		List<Thing> list = pawn.Map.listerThings.ThingsOfDef(bossgroupCaller);
		if (list.Count <= 0)
		{
			return "NoSubject".Translate(bossgroupCaller.label);
		}
		bool flag = false;
		for (int i = 0; i < list.Count; i++)
		{
			if (pawn.CanReach(list[i], PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return "NoReachableBossgroupCaller".Translate(bossgroupCaller.label);
		}
		bool flag2 = false;
		for (int j = 0; j < list.Count; j++)
		{
			if (pawn.CanReserve(list[j], 1, -1, null, forced))
			{
				flag2 = true;
				break;
			}
		}
		if (!flag2)
		{
			return "NoReservableBossgroupCaller".Translate(bossgroupCaller.label);
		}
		if (bossgroupCaller.HasComp(typeof(CompPowerTrader)))
		{
			bool flag3 = false;
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k].TryGetComp<CompPowerTrader>().PowerOn)
				{
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				return "NoPoweredBossgroupCaller".Translate(bossgroupCaller.label);
			}
		}
		return true;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00005778 File Offset: 0x00003978
	public static ThingDef GetBossgroupCaller(BossgroupDef def)
	{
		return DefDatabase<ThingDef>.AllDefs.FirstOrDefault((ThingDef t) => t.GetCompProperties<CompProperties_Useable_CallBossgroup>() != null && t.GetCompProperties<CompProperties_Useable_CallBossgroup>().bossgroupDef == def);
	}
}

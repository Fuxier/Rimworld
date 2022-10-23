using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

// Token: 0x0200000F RID: 15
public static class MechanitorUtility
{
	// Token: 0x0600003C RID: 60 RVA: 0x000041E7 File Offset: 0x000023E7
	public static bool IsMechanitor(Pawn pawn)
	{
		return ModsConfig.BiotechActive && pawn.Faction.IsPlayerSafe() && pawn.health.hediffSet.HasHediff(HediffDefOf.MechlinkImplant, false);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00004217 File Offset: 0x00002417
	public static bool IsOverseerSubject(Pawn pawn)
	{
		return pawn.Faction.IsPlayerSafe() && pawn.GetComp<CompOverseerSubject>() != null;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00004231 File Offset: 0x00002431
	public static MechWorkModeDef GetMechWorkMode(this Pawn pawn)
	{
		MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
		if (mechControlGroup == null)
		{
			return null;
		}
		return mechControlGroup.WorkMode;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00004244 File Offset: 0x00002444
	public static MechanitorControlGroup GetMechControlGroup(this Pawn pawn)
	{
		Pawn overseer = pawn.GetOverseer();
		if (overseer == null)
		{
			return null;
		}
		Pawn_MechanitorTracker mechanitor = overseer.mechanitor;
		if (mechanitor == null)
		{
			return null;
		}
		return mechanitor.GetControlGroup(pawn);
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00004263 File Offset: 0x00002463
	public static Pawn GetOverseer(this Pawn pawn)
	{
		if (!ModsConfig.BiotechActive)
		{
			return null;
		}
		Pawn_RelationsTracker relations = pawn.relations;
		if (relations == null)
		{
			return null;
		}
		return relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00004285 File Offset: 0x00002485
	public static bool IsGestating(this Pawn pawn)
	{
		return pawn.ParentHolder is Building_MechGestator;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00004298 File Offset: 0x00002498
	public static AcceptanceReport CanControlMech(Pawn pawn, Pawn mech)
	{
		if (pawn.mechanitor == null || !mech.IsColonyMech || mech.Downed || mech.Dead || mech.IsAttacking())
		{
			return false;
		}
		if (!MechanitorUtility.EverControllable(mech))
		{
			return "CannotControlMechNeverControllable".Translate();
		}
		if (mech.GetOverseer() == pawn)
		{
			return "CannotControlMechAlreadyControlled".Translate(pawn.LabelShort);
		}
		float num = (float)(pawn.mechanitor.TotalBandwidth - pawn.mechanitor.UsedBandwidth);
		float statValue = mech.GetStatValue(StatDefOf.BandwidthCost, true, -1);
		if (num < statValue)
		{
			return "CannotControlMechNotEnoughBandwidth".Translate();
		}
		return true;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x0000434F File Offset: 0x0000254F
	public static bool EverControllable(Pawn mech)
	{
		return mech.TryGetComp<CompOverseerSubject>() != null;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x0000435C File Offset: 0x0000255C
	public static bool IsColonyMechRequiringMechanitor(this Pawn mech)
	{
		if (!mech.IsColonyMech)
		{
			return false;
		}
		CompOverseerSubject compOverseerSubject = mech.TryGetComp<CompOverseerSubject>();
		return compOverseerSubject != null && compOverseerSubject.State != OverseerSubjectState.Overseen;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x0000438C File Offset: 0x0000258C
	public static void Notify_MechlinkQuestRewardAvailable(Quest quest, LookTargets lookTargets = null)
	{
		if (!Find.History.mechlinkEverAvailable)
		{
			List<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
			bool flag = false;
			for (int i = 0; i < allMaps_FreeColonists.Count; i++)
			{
				if (MechanitorUtility.IsMechanitor(allMaps_FreeColonists[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelMechlinkAvailable".Translate(), "LetterMechlinkAvailable".Translate(quest.name), LetterDefOf.PositiveEvent, lookTargets, null, quest, null, null);
			}
			Find.History.Notify_MechlinkAvailable();
		}
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00004410 File Offset: 0x00002610
	public static bool TryGetBandwidthLostFromDroppingThing(Pawn pawn, Thing thing, out int bandwidthLost)
	{
		bandwidthLost = -1;
		if (thing.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
		{
			return false;
		}
		bandwidthLost = Mathf.RoundToInt(thing.def.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.MechBandwidth));
		if (bandwidthLost <= 0)
		{
			return false;
		}
		int num = pawn.mechanitor.TotalBandwidth - pawn.mechanitor.UsedBandwidth;
		return bandwidthLost > num;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00004477 File Offset: 0x00002677
	public static bool TryGetMechsLostFromDroppingThing(Pawn pawn, Thing thing, out List<Pawn> lostMechs, out int bandwidthLost)
	{
		if (!MechanitorUtility.TryGetBandwidthLostFromDroppingThing(pawn, thing, out bandwidthLost))
		{
			lostMechs = null;
			return false;
		}
		return MechanitorUtility.TryGetMechsLostFromBandwidthReduction(pawn, bandwidthLost, out lostMechs);
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00004494 File Offset: 0x00002694
	public static bool TryGetMechsLostFromBandwidthReduction(Pawn pawn, int lostBandwidth, out List<Pawn> lostMechs)
	{
		lostMechs = new List<Pawn>();
		MechanitorUtility.tmpMechs.Clear();
		MechanitorUtility.GetMechsInAssignedOrder(pawn, ref MechanitorUtility.tmpMechs);
		int num = pawn.mechanitor.TotalBandwidth - lostBandwidth;
		int num2 = pawn.mechanitor.UsedBandwidth;
		int num3 = 0;
		while (num3 < MechanitorUtility.tmpMechs.Count && num2 > num)
		{
			int num4 = Mathf.RoundToInt(MechanitorUtility.tmpMechs[num3].GetStatValue(StatDefOf.BandwidthCost, true, -1));
			num2 -= num4;
			lostMechs.Add(MechanitorUtility.tmpMechs[num3]);
			num3++;
		}
		return MechanitorUtility.tmpMechs.Count > 0;
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00004530 File Offset: 0x00002730
	public static bool TryConfirmBandwidthLossFromDroppingThing(Pawn pawn, Thing thing, Action confirmAct)
	{
		List<Pawn> source;
		int num;
		if (MechanitorUtility.IsMechanitor(pawn) && MechanitorUtility.TryGetMechsLostFromDroppingThing(pawn, thing, out source, out num))
		{
			int totalBandwidth = pawn.mechanitor.TotalBandwidth;
			string bandwidthLower = "BandwidthLower".Translate().ToString();
			Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation("DropThingBandwidthApparel".Translate(thing.LabelShort, pawn, totalBandwidth, totalBandwidth - num) + (":\n\n" + (from m in source
			select string.Concat(new object[]
			{
				m.LabelShortCap,
				" (",
				m.GetStatValue(StatDefOf.BandwidthCost, true, -1),
				" ",
				bandwidthLower,
				")"
			})).ToLineList("- ", false)), confirmAct, false, null, WindowLayer.Dialog);
			dialog_MessageBox.buttonBText = "CancelButton".Translate();
			Find.WindowStack.Add(dialog_MessageBox);
			return true;
		}
		return false;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00004610 File Offset: 0x00002810
	public static void GetMechsInAssignedOrder(Pawn pawn, ref List<Pawn> mechs)
	{
		MechanitorUtility.tmpAssignedMechs.Clear();
		for (int i = 0; i < pawn.mechanitor.controlGroups.Count; i++)
		{
			MechanitorUtility.tmpAssignedMechs.AddRange(pawn.mechanitor.controlGroups[i].AssignedMechs);
		}
		MechanitorUtility.tmpAssignedMechs.SortByDescending((AssignedMech m) => m.tickAssigned);
		for (int j = 0; j < MechanitorUtility.tmpAssignedMechs.Count; j++)
		{
			mechs.Add(MechanitorUtility.tmpAssignedMechs[j].pawn);
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000046B8 File Offset: 0x000028B8
	public static void ForceDisconnectMechFromOverseer(Pawn mech)
	{
		Pawn overseer = mech.GetOverseer();
		if (overseer != null && overseer.relations.TryRemoveDirectRelation(PawnRelationDefOf.Overseer, mech))
		{
			mech.TryGetComp<CompOverseerSubject>().Notify_DisconnectedFromOverseer();
			SoundDefOf.DisconnectedMech.PlayOneShot(new TargetInfo(overseer));
			Messages.Message("MessageMechanitorDisconnectedFromMech".Translate(overseer, mech), new LookTargets(new Pawn[]
			{
				mech,
				overseer
			}), MessageTypeDefOf.NeutralEvent, true);
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x0000473C File Offset: 0x0000293C
	public static string GetMechGestationJobString(JobDriver_DoBill job, Pawn mechanitor, Bill_Mech bill)
	{
		switch (bill.State)
		{
		case FormingCycleState.Gathering:
			if (job.AnyIngredientsQueued)
			{
				return "LoadingMechGestator".Translate() + ".";
			}
			if (job.AnyIngredientsQueued)
			{
				goto IL_7F;
			}
			break;
		case FormingCycleState.Preparing:
			break;
		case FormingCycleState.Forming:
			goto IL_7F;
		case FormingCycleState.Formed:
			return "InitMechBirth".Translate() + ".";
		default:
			goto IL_7F;
		}
		return "InitMechGestationCycle".Translate() + ".";
		IL_7F:
		Log.Error("Unknown mech gestation job state.");
		return null;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000047D3 File Offset: 0x000029D3
	public static IEnumerable<Gizmo> GetMechGizmos(Pawn mech)
	{
		if (mech.IsColonyMech && MechanitorUtility.EverControllable(mech))
		{
			Pawn overseer = mech.GetOverseer();
			bool flag = overseer == null || !overseer.Spawned;
			yield return new Command_Action
			{
				defaultLabel = "CommandSelectOverseer".Translate(),
				defaultDesc = (flag ? "CommandSelectOverseerDisabledDesc".Translate() : "CommandSelectOverseerDesc".Translate()),
				icon = MechanitorUtility.SelectOverseerIcon.Texture,
				action = delegate()
				{
					Find.Selector.ClearSelection();
					Find.Selector.Select(overseer, true, true);
				},
				disabled = flag
			};
			if (Find.Selector.IsSelected(mech))
			{
				MechanitorControlGroup mechControlGroup = mech.GetMechControlGroup();
				if (mechControlGroup != null)
				{
					yield return new MechanitorControlGroupGizmo(mechControlGroup);
				}
			}
			Gizmo activeMechShieldGizmo = RemoteShieldUtility.GetActiveMechShieldGizmo(mech);
			if (activeMechShieldGizmo != null)
			{
				yield return activeMechShieldGizmo;
			}
		}
		else if (DebugSettings.ShowDevGizmos && mech.Faction != Faction.OfPlayer)
		{
			yield return new Command_Action
			{
				defaultLabel = "DEV: Recruit",
				action = delegate()
				{
					mech.SetFaction(Faction.OfPlayer, null);
				}
			};
			yield return new Command_Action
			{
				defaultLabel = "DEV: Kill",
				action = delegate()
				{
					mech.Kill(null, null);
				}
			};
		}
		yield break;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000047E4 File Offset: 0x000029E4
	public static bool AnyMechanitorInPlayerFaction()
	{
		using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMaps_FreeColonists.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (MechanitorUtility.IsMechanitor(enumerator.Current))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x0000483C File Offset: 0x00002A3C
	public static bool AnyMechsInPlayerFaction()
	{
		return MechanitorUtility.MechsInPlayerFaction().Any<Pawn>();
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00004848 File Offset: 0x00002A48
	public static IEnumerable<Pawn> MechsInPlayerFaction()
	{
		List<Map> maps = Find.Maps;
		int num;
		for (int i = 0; i < maps.Count; i = num + 1)
		{
			List<Pawn> pawns = maps[i].mapPawns.PawnsInFaction(Faction.OfPlayer);
			for (int j = 0; j < pawns.Count; j = num + 1)
			{
				if (pawns[j].IsColonyMech)
				{
					yield return pawns[j];
				}
				num = j;
			}
			pawns = null;
			num = i;
		}
		yield break;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00004854 File Offset: 0x00002A54
	public static bool AnyMechlinkInMap()
	{
		List<Map> maps = Find.Maps;
		for (int i = 0; i < maps.Count; i++)
		{
			if (maps[i].listerThings.ThingsOfDef(ThingDefOf.Mechlink).Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x0000489C File Offset: 0x00002A9C
	public static List<ThingDefCountClass> IngredientsFromDisassembly(ThingDef mech)
	{
		MechanitorUtility.tmpIngredients.Clear();
		Predicate<ThingDefCountClass> <>9__0;
		foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
		{
			if (!recipeDef.products.NullOrEmpty<ThingDefCountClass>())
			{
				List<ThingDefCountClass> products = recipeDef.products;
				Predicate<ThingDefCountClass> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDefCountClass x) => x.thingDef == mech));
				}
				if (products.Any(predicate))
				{
					for (int i = 0; i < recipeDef.ingredients.Count; i++)
					{
						ThingDef thingDef = recipeDef.ingredients[i].filter.AllowedThingDefs.FirstOrDefault<ThingDef>();
						int count = Mathf.Max(1, Mathf.RoundToInt(recipeDef.ingredients[i].GetBaseCount() * 0.4f));
						MechanitorUtility.tmpIngredients.Add(new ThingDefCountClass(thingDef, count));
					}
					break;
				}
			}
		}
		return MechanitorUtility.tmpIngredients;
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000053 RID: 83 RVA: 0x000049B4 File Offset: 0x00002BB4
	public static List<ThingDef> MechRechargers
	{
		get
		{
			if (MechanitorUtility.cachedRechargers == null)
			{
				MechanitorUtility.cachedRechargers = (from d in DefDatabase<ThingDef>.AllDefs
				where d.IsMechRecharger
				select d).ToList<ThingDef>();
			}
			return MechanitorUtility.cachedRechargers;
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00004A00 File Offset: 0x00002C00
	public static ThingDef RechargerForMech(ThingDef mech)
	{
		foreach (ThingDef thingDef in MechanitorUtility.MechRechargers)
		{
			if (Building_MechCharger.IsCompatibleWithCharger(thingDef, mech))
			{
				return thingDef;
			}
		}
		return null;
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000055 RID: 85 RVA: 0x00004A5C File Offset: 0x00002C5C
	public static List<RecipeDef> MechRecipes
	{
		get
		{
			if (MechanitorUtility.cachedMechRecipes == null)
			{
				MechanitorUtility.cachedMechRecipes = DefDatabase<RecipeDef>.AllDefs.Where(delegate(RecipeDef r)
				{
					ThingDef producedThingDef = r.ProducedThingDef;
					if (producedThingDef == null)
					{
						return false;
					}
					RaceProperties race = producedThingDef.race;
					bool? flag = (race != null) ? new bool?(race.IsMechanoid) : null;
					bool flag2 = true;
					return flag.GetValueOrDefault() == flag2 & flag != null;
				}).ToList<RecipeDef>();
			}
			return MechanitorUtility.cachedMechRecipes;
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00004AA8 File Offset: 0x00002CA8
	public static void ClearCache()
	{
		MechanitorUtility.cachedMechRecipes = null;
		MechanitorUtility.cachedRechargers = null;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00004AB8 File Offset: 0x00002CB8
	public static bool InMechanitorCommandRange(Pawn mech, LocalTargetInfo target)
	{
		Pawn overseer = mech.GetOverseer();
		if (overseer != null)
		{
			if (mech.MapHeld != overseer.MapHeld)
			{
				return false;
			}
			if (overseer.mechanitor.CanCommandTo(target))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00004AF0 File Offset: 0x00002CF0
	public static bool AnyPlayerMechCanDoWork(WorkTypeDef workType, int skillRequired, out Pawn pawn)
	{
		if (!ModsConfig.BiotechActive)
		{
			pawn = null;
			return false;
		}
		List<Pawn> list = Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer);
		for (int i = 0; i < list.Count; i++)
		{
			Pawn pawn2 = list[i];
			if (pawn2.IsColonyMech && pawn2.GetOverseer() != null && pawn2.RaceProps.mechEnabledWorkTypes.Contains(workType) && pawn2.RaceProps.mechFixedSkillLevel >= skillRequired)
			{
				pawn = pawn2;
				return true;
			}
		}
		pawn = null;
		return false;
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00004B74 File Offset: 0x00002D74
	public static AcceptanceReport CanDraftMech(Pawn mech)
	{
		if (mech.IsColonyMech)
		{
			if (mech.needs.energy != null && mech.needs.energy.IsLowEnergySelfShutdown)
			{
				return "IsLowEnergySelfShutdown".Translate(mech.Named("PAWN"));
			}
			Pawn overseer = mech.GetOverseer();
			if (overseer != null)
			{
				AcceptanceReport canControlMechs = overseer.mechanitor.CanControlMechs;
				if (!canControlMechs)
				{
					return canControlMechs;
				}
				if (!overseer.mechanitor.ControlledPawns.Contains(mech))
				{
					return "MechControllerInsufficientBandwidth".Translate(overseer.Named("PAWN"));
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000019 RID: 25
	private static CachedTexture SelectOverseerIcon = new CachedTexture("UI/Icons/SelectOverseer");

	// Token: 0x0400001A RID: 26
	private static List<Pawn> tmpMechs = new List<Pawn>();

	// Token: 0x0400001B RID: 27
	private static List<AssignedMech> tmpAssignedMechs = new List<AssignedMech>();

	// Token: 0x0400001C RID: 28
	private static List<ThingDefCountClass> tmpIngredients = new List<ThingDefCountClass>();

	// Token: 0x0400001D RID: 29
	private static List<ThingDef> cachedRechargers;

	// Token: 0x0400001E RID: 30
	private static List<RecipeDef> cachedMechRecipes;
}

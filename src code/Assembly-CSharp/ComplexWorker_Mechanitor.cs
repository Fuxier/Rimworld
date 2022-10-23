using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

// Token: 0x0200000D RID: 13
public class ComplexWorker_Mechanitor : ComplexWorker
{
	// Token: 0x06000034 RID: 52 RVA: 0x00003BE8 File Offset: 0x00001DE8
	public override Faction GetFixedHostileFactionForThreats()
	{
		return Faction.OfMechanoids;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003BF0 File Offset: 0x00001DF0
	protected override void PreSpawnThreats(List<List<CellRect>> rooms, Map map, List<Thing> allSpawnedThings)
	{
		base.PreSpawnThreats(rooms, map, allSpawnedThings);
		ComplexWorker_Mechanitor.<>c__DisplayClass5_0 CS$<>8__locals1 = new ComplexWorker_Mechanitor.<>c__DisplayClass5_0();
		this.tmpAllRoomRects.Clear();
		this.tmpAllRoomRects.AddRange(rooms.SelectMany((List<CellRect> r) => r));
		CS$<>8__locals1.bounds = this.tmpAllRoomRects[0];
		for (int i = 0; i < this.tmpAllRoomRects.Count; i++)
		{
			CS$<>8__locals1.bounds = CS$<>8__locals1.bounds.Encapsulate(this.tmpAllRoomRects[i]);
		}
		bool flag = false;
		foreach (CellRect room in this.tmpAllRoomRects.OrderBy(new Func<CellRect, float>(CS$<>8__locals1.<PreSpawnThreats>g__OrderRoomsBy|1)))
		{
			Building_AncientCryptosleepPod item;
			if (this.TryPlaceDeceasedMechanitor(room, map, out item))
			{
				allSpawnedThings.Add(item);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Log.Error("Failed to place mechanitor in ancient mechanitor complex.");
		}
		this.tmpAllRoomRects.Clear();
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003D0C File Offset: 0x00001F0C
	private bool TryPlaceDeceasedMechanitor(CellRect room, Map map, out Building_AncientCryptosleepPod casket)
	{
		ComplexWorker_Mechanitor.<>c__DisplayClass6_0 CS$<>8__locals1;
		CS$<>8__locals1.map = map;
		foreach (IntVec3 intVec in room.Cells.InRandomOrder(null))
		{
			if (ComplexWorker_Mechanitor.<TryPlaceDeceasedMechanitor>g__CanPlaceCasketAt|6_0(intVec, ref CS$<>8__locals1))
			{
				casket = (Building_AncientCryptosleepPod)GenSpawn.Spawn(ThingDefOf.AncientCryptosleepPod, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				casket.openedSignal = "MechanitorCasketOpened" + Find.UniqueIDsManager.GetNextSignalTagID();
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Mechanitor_Basic, Faction.OfAncients, PawnGenerationContext.NonPlayer, -1, false, false, false, true, false, 1f, false, true, false, true, true, false, true, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, true, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false));
				pawn.Corpse.Age = ComplexWorker_Mechanitor.RandomMechanitorCorpseAge.RandomInRange;
				pawn.relations.hidePawnRelations = true;
				pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
				casket.TryAcceptThing(pawn.Corpse, false);
				SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
				signalAction_Message.signalTag = casket.openedSignal;
				signalAction_Message.message = "MessageMechanitorCasketOpened".Translate(pawn, HediffDefOf.MechlinkImplant);
				signalAction_Message.messageType = MessageTypeDefOf.PositiveEvent;
				signalAction_Message.lookTargets = pawn.Corpse;
				GenSpawn.Spawn(signalAction_Message, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
				signalAction_Letter.signalTag = casket.openedSignal;
				signalAction_Letter.letter = LetterMaker.MakeLetter("LetterLabelMechanitorCasketOpened".Translate(pawn), "LetterMechanitorCasketOpened".Translate(pawn), LetterDefOf.NeutralEvent, pawn.Corpse, null, null, null);
				GenSpawn.Spawn(signalAction_Letter, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				SignalAction_SoundOneShot signalAction_SoundOneShot = (SignalAction_SoundOneShot)ThingMaker.MakeThing(ThingDefOf.SignalAction_SoundOneShot, null);
				signalAction_SoundOneShot.signalTag = casket.openedSignal;
				signalAction_SoundOneShot.sound = SoundDefOf.MechlinkCorpseReveal;
				GenSpawn.Spawn(signalAction_SoundOneShot, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				TriggerUnfogged triggerUnfogged = (TriggerUnfogged)ThingMaker.MakeThing(ThingDefOf.TriggerUnfogged, null);
				triggerUnfogged.signalTag = "MechanitorCasketUnfogged" + Find.UniqueIDsManager.GetNextSignalTagID();
				GenSpawn.Spawn(triggerUnfogged, casket.Position, CS$<>8__locals1.map, WipeMode.Vanish);
				SignalAction_Letter signalAction_Letter2 = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
				signalAction_Letter2.signalTag = triggerUnfogged.signalTag;
				signalAction_Letter2.letter = LetterMaker.MakeLetter("LetterLabelMechanitorCasketFound".Translate(), "LetterMechanitorCasketFound".Translate(), LetterDefOf.NeutralEvent, casket, null, null, null);
				GenSpawn.Spawn(signalAction_Letter2, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
				ScatterDebrisUtility.ScatterFilthAroundThing(casket, CS$<>8__locals1.map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
				return true;
			}
		}
		casket = null;
		return false;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000040A0 File Offset: 0x000022A0
	[CompilerGenerated]
	internal static bool <TryPlaceDeceasedMechanitor>g__CanPlaceCasketAt|6_0(IntVec3 cell, ref ComplexWorker_Mechanitor.<>c__DisplayClass6_0 A_1)
	{
		using (CellRect.Enumerator enumerator = GenAdj.OccupiedRect(cell, Rot4.North, ThingDefOf.AncientCryptosleepPod.Size).ExpandedBy(1).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetEdifice(A_1.map) != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x04000014 RID: 20
	private const string MechanitorCasketOpenedSignal = "MechanitorCasketOpened";

	// Token: 0x04000015 RID: 21
	private const string MechanitorCasketUnfoggedSignal = "MechanitorCasketUnfogged";

	// Token: 0x04000016 RID: 22
	private static readonly IntRange RandomMechanitorCorpseAge = new IntRange(0, 360000000);

	// Token: 0x04000017 RID: 23
	private List<CellRect> tmpAllRoomRects = new List<CellRect>();
}

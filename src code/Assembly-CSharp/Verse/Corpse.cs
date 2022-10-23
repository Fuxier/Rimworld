using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C5 RID: 965
	public class Corpse : ThingWithComps, IThingHolder, IStrippable, IBillGiver, IObservedThoughtGiver
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001B6D RID: 7021 RVA: 0x000A8DBA File Offset: 0x000A6FBA
		// (set) Token: 0x06001B6E RID: 7022 RVA: 0x000A8DD8 File Offset: 0x000A6FD8
		public Pawn InnerPawn
		{
			get
			{
				if (this.innerContainer.Count > 0)
				{
					return this.innerContainer[0];
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.innerContainer.Clear();
					return;
				}
				if (this.innerContainer.Count > 0)
				{
					Log.Error("Setting InnerPawn in corpse that already has one.");
					this.innerContainer.Clear();
				}
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001B6F RID: 7023 RVA: 0x000A8E25 File Offset: 0x000A7025
		// (set) Token: 0x06001B70 RID: 7024 RVA: 0x000A8E38 File Offset: 0x000A7038
		public int Age
		{
			get
			{
				return Find.TickManager.TicksGame - this.timeOfDeath;
			}
			set
			{
				this.timeOfDeath = Find.TickManager.TicksGame - value;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001B71 RID: 7025 RVA: 0x000A8E4C File Offset: 0x000A704C
		public override string LabelNoCount
		{
			get
			{
				if (this.Bugged)
				{
					Log.ErrorOnce("Corpse.Label while Bugged", 57361644);
					return "";
				}
				return "DeadLabel".Translate(this.InnerPawn.Label, this.InnerPawn);
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001B72 RID: 7026 RVA: 0x000A8EA0 File Offset: 0x000A70A0
		public override bool IngestibleNow
		{
			get
			{
				if (this.Bugged)
				{
					Log.Error("IngestibleNow on Corpse while Bugged.");
					return false;
				}
				return base.IngestibleNow && this.InnerPawn.RaceProps.IsFlesh && this.GetRotStage() == RotStage.Fresh;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001B73 RID: 7027 RVA: 0x000A8EE0 File Offset: 0x000A70E0
		public RotDrawMode CurRotDrawMode
		{
			get
			{
				CompRottable comp = base.GetComp<CompRottable>();
				if (comp != null)
				{
					if (comp.Stage == RotStage.Rotting)
					{
						return RotDrawMode.Rotting;
					}
					if (comp.Stage == RotStage.Dessicated)
					{
						return RotDrawMode.Dessicated;
					}
				}
				return RotDrawMode.Fresh;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001B74 RID: 7028 RVA: 0x000A8F10 File Offset: 0x000A7110
		private bool ShouldVanish
		{
			get
			{
				return this.InnerPawn.RaceProps.Animal && this.vanishAfterTimestamp > 0 && this.Age >= this.vanishAfterTimestamp && base.Spawned && this.GetRoom(RegionType.Set_All) != null && this.GetRoom(RegionType.Set_All).TouchesMapEdge && !base.Map.roofGrid.Roofed(base.Position);
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001B75 RID: 7029 RVA: 0x000A8F82 File Offset: 0x000A7182
		public BillStack BillStack
		{
			get
			{
				return this.operationsBillStack;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001B76 RID: 7030 RVA: 0x000A8F8A File Offset: 0x000A718A
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001B77 RID: 7031 RVA: 0x000A8F9C File Offset: 0x000A719C
		public bool Bugged
		{
			get
			{
				return this.innerContainer.Count == 0 || this.innerContainer[0] == null || this.innerContainer[0].def == null || this.innerContainer[0].kindDef == null;
			}
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x000A8FED File Offset: 0x000A71ED
		public Corpse()
		{
			this.operationsBillStack = new BillStack(this);
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Reference);
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x000A9020 File Offset: 0x000A7220
		public bool CurrentlyUsableForBills()
		{
			return this.InteractionCell.IsValid;
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x000A903B File Offset: 0x000A723B
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x000A9043 File Offset: 0x000A7243
		public bool AnythingToStrip()
		{
			return this.InnerPawn.AnythingToStrip();
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x000A9050 File Offset: 0x000A7250
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x000A9058 File Offset: 0x000A7258
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x000A9066 File Offset: 0x000A7266
		public override void PostMake()
		{
			base.PostMake();
			this.timeOfDeath = Find.TickManager.TicksGame;
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x000A907E File Offset: 0x000A727E
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Bugged)
			{
				Log.Error(this + " spawned in bugged state.");
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.InnerPawn.Rotation = Rot4.South;
			this.NotifyColonistBar();
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x000A90B7 File Offset: 0x000A72B7
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			if (!this.Bugged)
			{
				this.NotifyColonistBar();
			}
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x000A90D0 File Offset: 0x000A72D0
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = null;
			if (!this.Bugged)
			{
				pawn = this.InnerPawn;
				this.NotifyColonistBar();
				this.innerContainer.Clear();
			}
			base.Destroy(mode);
			if (pawn != null)
			{
				Corpse.PostCorpseDestroy(pawn);
			}
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x000A9110 File Offset: 0x000A7310
		public static void PostCorpseDestroy(Pawn pawn)
		{
			if (pawn.ownership != null)
			{
				pawn.ownership.UnclaimAll();
			}
			if (pawn.equipment != null)
			{
				pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			}
			pawn.inventory.DestroyAll(DestroyMode.Vanish);
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (!PawnGenerator.IsBeingGenerated(pawn))
			{
				Ideo ideo = pawn.Ideo;
				if (ideo == null)
				{
					return;
				}
				ideo.Notify_MemberCorpseDestroyed(pawn);
			}
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x000A9180 File Offset: 0x000A7380
		public override void TickRare()
		{
			base.TickRare();
			if (base.Destroyed)
			{
				return;
			}
			if (this.Bugged)
			{
				Log.Error(this + " has null innerPawn. Destroying.");
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			this.InnerPawn.TickRare();
			if (this.vanishAfterTimestamp < 0 || this.GetRotStage() != RotStage.Dessicated)
			{
				this.vanishAfterTimestamp = this.Age + 6000000;
			}
			if (base.Spawned && this.GetRotStage() == RotStage.Rotting)
			{
				int num = GasUtility.RotStinkToSpawnForCorpse(this);
				if (num > 0)
				{
					GasUtility.AddGas(base.Position, base.Map, GasType.RotStink, num);
				}
			}
			if (this.ShouldVanish)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000A922C File Offset: 0x000A742C
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			BodyPartRecord bodyPartRecord = this.GetBestBodyPartToEat(ingester, nutritionWanted);
			if (bodyPartRecord == null)
			{
				Log.Error(string.Concat(new object[]
				{
					ingester,
					" ate ",
					this,
					" but no body part was found. Replacing with core part."
				}));
				bodyPartRecord = this.InnerPawn.RaceProps.body.corePart;
			}
			float bodyPartNutrition = FoodUtility.GetBodyPartNutrition(this, bodyPartRecord);
			if (bodyPartRecord == this.InnerPawn.RaceProps.body.corePart)
			{
				if (PawnUtility.ShouldSendNotificationAbout(this.InnerPawn) && this.InnerPawn.RaceProps.Humanlike)
				{
					Messages.Message("MessageEatenByPredator".Translate(this.InnerPawn.LabelShort, ingester.Named("PREDATOR"), this.InnerPawn.Named("EATEN")).CapitalizeFirst(), ingester, MessageTypeDefOf.NegativeEvent, true);
				}
				numTaken = 1;
			}
			else
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.InnerPawn, bodyPartRecord);
				hediff_MissingPart.lastInjury = HediffDefOf.Bite;
				hediff_MissingPart.IsFresh = true;
				this.InnerPawn.health.AddHediff(hediff_MissingPart, null, null, null);
				numTaken = 0;
			}
			nutritionIngested = bodyPartNutrition;
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x000A9367 File Offset: 0x000A7567
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			foreach (Thing thing in this.InnerPawn.ButcherProducts(butcher, efficiency))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			if (this.InnerPawn.RaceProps.BloodDef != null)
			{
				FilthMaker.TryMakeFilth(butcher.Position, butcher.Map, this.InnerPawn.RaceProps.BloodDef, this.InnerPawn.LabelIndefinite(), 1, FilthSourceFlags.None);
			}
			if (this.InnerPawn.RaceProps.Humanlike)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ButcheredHuman, new SignalArgs(butcher.Named(HistoryEventArgsNames.Doer), this.InnerPawn.Named(HistoryEventArgsNames.Victim))), true);
				TaleRecorder.RecordTale(TaleDefOf.ButcheredHumanlikeCorpse, new object[]
				{
					butcher
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x000A9388 File Offset: 0x000A7588
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.timeOfDeath, "timeOfDeath", 0, false);
			Scribe_Values.Look<int>(ref this.vanishAfterTimestamp, "vanishAfterTimestamp", 0, false);
			Scribe_Values.Look<bool>(ref this.everBuriedInSarcophagus, "everBuriedInSarcophagus", false, false);
			Scribe_Deep.Look<BillStack>(ref this.operationsBillStack, "operationsBillStack", new object[]
			{
				this
			});
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000A9405 File Offset: 0x000A7605
		public void Strip()
		{
			this.InnerPawn.Strip();
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000A9414 File Offset: 0x000A7614
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.InnerPawn.Drawer.renderer.RenderPawnAt(drawLoc, null, false);
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000029B0 File Offset: 0x00000BB0
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			return null;
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x000A9444 File Offset: 0x000A7644
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			if (!this.InnerPawn.RaceProps.Humanlike)
			{
				return null;
			}
			if (this.InnerPawn.health.killedByRitual && Find.TickManager.TicksGame - this.timeOfDeath < 60000)
			{
				return null;
			}
			if (this.StoringThing() != null)
			{
				return null;
			}
			if (this.IsNotFresh())
			{
				return HistoryEventDefOf.ObservedLayingRottingCorpse;
			}
			return HistoryEventDefOf.ObservedLayingCorpse;
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x000A94B0 File Offset: 0x000A76B0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.InnerPawn.Faction != null)
			{
				stringBuilder.AppendLineTagged("Faction".Translate() + ": " + this.InnerPawn.Faction.NameColored);
			}
			stringBuilder.AppendLine("DeadTime".Translate(this.Age.ToStringTicksToPeriodVague(true, false)));
			float num = 1f - this.InnerPawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(this.InnerPawn.RaceProps.body.corePart);
			if (num != 0f)
			{
				stringBuilder.AppendLine("CorpsePercentMissing".Translate() + ": " + num.ToStringPercent());
			}
			stringBuilder.AppendLine(base.GetInspectString());
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x000A95A4 File Offset: 0x000A77A4
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BodySize".Translate(), this.InnerPawn.BodySize.ToString("F2"), "Stat_Race_BodySize_Desc".Translate(), 500, null, null, false);
			if (this.GetRotStage() == RotStage.Fresh)
			{
				StatDef meatAmount = StatDefOf.MeatAmount;
				yield return new StatDrawEntry(meatAmount.category, meatAmount, this.InnerPawn.GetStatValue(meatAmount, true, -1), StatRequest.For(this.InnerPawn), ToStringNumberSense.Undefined, null, false);
				StatDef leatherAmount = StatDefOf.LeatherAmount;
				yield return new StatDrawEntry(leatherAmount.category, leatherAmount, this.InnerPawn.GetStatValue(leatherAmount, true, -1), StatRequest.For(this.InnerPawn), ToStringNumberSense.Undefined, null, false);
			}
			if (ModsConfig.BiotechActive && this.InnerPawn.RaceProps.IsMechanoid)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Mechanoid, "MechWeightClass".Translate(), this.InnerPawn.RaceProps.mechWeightClass.ToStringHuman().CapitalizeFirst(), "MechWeightClassExplanation".Translate(), 500, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x000A95B4 File Offset: 0x000A77B4
		public void RotStageChanged()
		{
			this.InnerPawn.Drawer.renderer.WoundOverlays.ClearCache();
			PortraitsCache.SetDirty(this.InnerPawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.InnerPawn);
			this.NotifyColonistBar();
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000A95F0 File Offset: 0x000A77F0
		private BodyPartRecord GetBestBodyPartToEat(Pawn ingester, float nutritionWanted)
		{
			IEnumerable<BodyPartRecord> source = from x in this.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside && FoodUtility.GetBodyPartNutrition(this, x) > 0.001f
			select x;
			if (!source.Any<BodyPartRecord>())
			{
				return null;
			}
			return source.MinBy((BodyPartRecord x) => Mathf.Abs(FoodUtility.GetBodyPartNutrition(this, x) - nutritionWanted));
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000A9658 File Offset: 0x000A7858
		private void NotifyColonistBar()
		{
			if (this.InnerPawn.Faction == Faction.OfPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000034B7 File Offset: 0x000016B7
		public void Notify_BillDeleted(Bill bill)
		{
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000A967E File Offset: 0x000A787E
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Resurrect",
					action = delegate()
					{
						ResurrectionUtility.Resurrect(this.InnerPawn);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x040013D6 RID: 5078
		private ThingOwner<Pawn> innerContainer;

		// Token: 0x040013D7 RID: 5079
		public int timeOfDeath = -1;

		// Token: 0x040013D8 RID: 5080
		private int vanishAfterTimestamp = -1;

		// Token: 0x040013D9 RID: 5081
		private BillStack operationsBillStack;

		// Token: 0x040013DA RID: 5082
		public bool everBuriedInSarcophagus;

		// Token: 0x040013DB RID: 5083
		private const int VanishAfterTicksSinceDessicated = 6000000;

		// Token: 0x040013DC RID: 5084
		private const int DontCauseObservedCorpseThoughtAfterRitualExecutionTicks = 60000;
	}
}

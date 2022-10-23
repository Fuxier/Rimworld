using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000375 RID: 885
	[StaticConstructorOnStartup]
	public class Pawn_InventoryTracker : IThingHolder, IExposable
	{
		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001985 RID: 6533 RVA: 0x0009A2B2 File Offset: 0x000984B2
		// (set) Token: 0x06001986 RID: 6534 RVA: 0x0009A2C4 File Offset: 0x000984C4
		public bool UnloadEverything
		{
			get
			{
				return this.unloadEverything && this.HasAnyUnloadableThing;
			}
			set
			{
				if (value && this.HasAnyUnloadableThing)
				{
					this.unloadEverything = true;
					return;
				}
				this.unloadEverything = false;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001987 RID: 6535 RVA: 0x0009A2E0 File Offset: 0x000984E0
		public bool HasAnyUnpackedCaravanItems
		{
			get
			{
				return this.unpackedCaravanItems.Count > 0;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001988 RID: 6536 RVA: 0x0009A2F0 File Offset: 0x000984F0
		private bool HasAnyUnloadableThing
		{
			get
			{
				return this.FirstUnloadableThing != default(ThingCount);
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001989 RID: 6537 RVA: 0x0009A314 File Offset: 0x00098514
		public ThingCount FirstUnloadableThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return default(ThingCount);
				}
				if (this.pawn.drugs != null && this.pawn.drugs.CurrentPolicy != null)
				{
					DrugPolicy currentPolicy = this.pawn.drugs.CurrentPolicy;
					Pawn_InventoryTracker.tmpDrugsToKeep.Clear();
					for (int i = 0; i < currentPolicy.Count; i++)
					{
						if (currentPolicy[i].takeToInventory > 0)
						{
							Pawn_InventoryTracker.tmpDrugsToKeep.Add(new ThingDefCount(currentPolicy[i].drug, currentPolicy[i].takeToInventory));
						}
					}
					for (int j = 0; j < this.innerContainer.Count; j++)
					{
						if (!this.innerContainer[j].def.IsDrug)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount);
						}
						int num = -1;
						for (int k = 0; k < Pawn_InventoryTracker.tmpDrugsToKeep.Count; k++)
						{
							if (this.innerContainer[j].def == Pawn_InventoryTracker.tmpDrugsToKeep[k].ThingDef)
							{
								num = k;
								break;
							}
						}
						if (num < 0)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount);
						}
						if (this.innerContainer[j].stackCount > Pawn_InventoryTracker.tmpDrugsToKeep[num].Count)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount - Pawn_InventoryTracker.tmpDrugsToKeep[num].Count);
						}
						Pawn_InventoryTracker.tmpDrugsToKeep[num] = new ThingDefCount(Pawn_InventoryTracker.tmpDrugsToKeep[num].ThingDef, Pawn_InventoryTracker.tmpDrugsToKeep[num].Count - this.innerContainer[j].stackCount);
					}
					return default(ThingCount);
				}
				return new ThingCount(this.innerContainer[0], this.innerContainer[0].stackCount);
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x0600198A RID: 6538 RVA: 0x0009A562 File Offset: 0x00098762
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0009A56A File Offset: 0x0009876A
		public Pawn_InventoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0009A5A8 File Offset: 0x000987A8
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.itemsNotForSale, "itemsNotForSale", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.unpackedCaravanItems, "unpackedCaravanItems", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.unloadEverything, "unloadEverything", false, false);
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0009A60D File Offset: 0x0009880D
		public void InventoryTrackerTick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0009A632 File Offset: 0x00098832
		public void InventoryTrackerTickRare()
		{
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0009A640 File Offset: 0x00098840
		public void DropAllNearPawn(IntVec3 pos, bool forbid = false, bool unforbid = false)
		{
			this.DropAllNearPawnHelper(pos, forbid, unforbid, false);
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0009A64C File Offset: 0x0009884C
		private void DropAllNearPawnHelper(IntVec3 pos, bool forbid = false, bool unforbid = false, bool caravanHaulOnly = false)
		{
			Pawn_InventoryTracker.<>c__DisplayClass24_0 CS$<>8__locals1 = new Pawn_InventoryTracker.<>c__DisplayClass24_0();
			CS$<>8__locals1.forbid = forbid;
			CS$<>8__locals1.unforbid = unforbid;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.caravanHaulOnly = caravanHaulOnly;
			if (this.pawn.MapHeld == null)
			{
				Log.Error("Tried to drop all inventory near pawn but the pawn is unspawned. pawn=" + this.pawn);
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			if (CS$<>8__locals1.caravanHaulOnly)
			{
				Pawn_InventoryTracker.tmpThingList.AddRange(this.unpackedCaravanItems);
			}
			else
			{
				Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			}
			int i;
			int j;
			for (i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i = j + 1)
			{
				if (CS$<>8__locals1.caravanHaulOnly && !this.innerContainer.Contains(Pawn_InventoryTracker.tmpThingList[i]))
				{
					this.unpackedCaravanItems.Remove(Pawn_InventoryTracker.tmpThingList[i]);
					Log.Warning("Could not drop unpacked caravan item " + Pawn_InventoryTracker.tmpThingList[i].Label + ", inventory no longer contains it");
				}
				else
				{
					Thing thing;
					this.innerContainer.TryDrop(Pawn_InventoryTracker.tmpThingList[i], pos, this.pawn.MapHeld, ThingPlaceMode.Near, out thing, delegate(Thing t, int unused)
					{
						if (CS$<>8__locals1.forbid)
						{
							t.SetForbiddenIfOutsideHomeArea();
						}
						if (CS$<>8__locals1.unforbid)
						{
							t.SetForbidden(false, false);
						}
						if (t.def.IsPleasureDrug)
						{
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
						}
						Lord formAndSendCaravanLord = CaravanFormingUtility.GetFormAndSendCaravanLord(CS$<>8__locals1.<>4__this.pawn);
						LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = ((formAndSendCaravanLord != null) ? formAndSendCaravanLord.LordJob : null) as LordJob_FormAndSendCaravan;
						if (CS$<>8__locals1.caravanHaulOnly && lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.GatheringItemsNow)
						{
							CaravanFormingUtility.TryAddItemBackToTransferables(t, lordJob_FormAndSendCaravan.transferables, Pawn_InventoryTracker.tmpThingList[i].stackCount);
						}
						CS$<>8__locals1.<>4__this.unpackedCaravanItems.Remove(Pawn_InventoryTracker.tmpThingList[i]);
					}, null);
				}
				j = i;
			}
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0009A7BC File Offset: 0x000989BC
		public void DropCount(ThingDef def, int count, bool forbid = false, bool unforbid = false)
		{
			if (this.pawn.MapHeld == null)
			{
				Log.Error("Tried to drop a thing near pawn but the pawn is unspawned. pawn=" + this.pawn);
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			int num = 0;
			Action<Thing, int> <>9__0;
			for (int i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i++)
			{
				Thing thing = Pawn_InventoryTracker.tmpThingList[i];
				if (thing.def == def)
				{
					int num2 = Math.Min(thing.stackCount, count);
					ThingOwner<Thing> thingOwner = this.innerContainer;
					Thing thing2 = Pawn_InventoryTracker.tmpThingList[i];
					IntVec3 position = this.pawn.Position;
					Map mapHeld = this.pawn.MapHeld;
					ThingPlaceMode mode = ThingPlaceMode.Near;
					int count2 = num2;
					Action<Thing, int> placedAction;
					if ((placedAction = <>9__0) == null)
					{
						placedAction = (<>9__0 = delegate(Thing t, int unused)
						{
							if (forbid)
							{
								t.SetForbiddenIfOutsideHomeArea();
							}
							if (unforbid)
							{
								t.SetForbidden(false, false);
							}
							if (t.def.IsPleasureDrug)
							{
								LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
							}
						});
					}
					Thing thing3;
					thingOwner.TryDrop(thing2, position, mapHeld, mode, count2, out thing3, placedAction, null);
					num += num2;
					if (num >= count)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0009A8B8 File Offset: 0x00098AB8
		public void RemoveCount(ThingDef def, int count, bool destroy = true)
		{
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			foreach (Thing thing in Pawn_InventoryTracker.tmpThingList)
			{
				if (thing.def == def)
				{
					if (thing.stackCount > count)
					{
						thing.stackCount -= count;
						break;
					}
					this.innerContainer.Remove(thing);
					if (destroy)
					{
						thing.Destroy(DestroyMode.Vanish);
						break;
					}
					break;
				}
			}
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0009A958 File Offset: 0x00098B58
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.innerContainer.ClearAndDestroyContents(mode);
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x0009A966 File Offset: 0x00098B66
		public bool Contains(Thing item)
		{
			return this.innerContainer.Contains(item);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x0009A974 File Offset: 0x00098B74
		public int Count(ThingDef def)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.def == def)
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x0009A9D8 File Offset: 0x00098BD8
		public int Count(Func<Thing, bool> validator)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (validator(thing))
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0009AA3C File Offset: 0x00098C3C
		public void AddHauledCaravanItem(Thing item)
		{
			Thing thing;
			if (this.pawn.carryTracker.innerContainer.TryTransferToContainer(item, this.innerContainer, item.stackCount, out thing, false) > 0)
			{
				this.unpackedCaravanItems.Add(thing);
			}
			CompForbiddable compForbiddable = (thing != null) ? thing.TryGetComp<CompForbiddable>() : null;
			if (compForbiddable != null)
			{
				compForbiddable.Forbidden = false;
			}
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0009AA94 File Offset: 0x00098C94
		public void TryAddAndUnforbid(Thing item)
		{
			CompForbiddable compForbiddable = item.TryGetComp<CompForbiddable>();
			if (this.innerContainer.TryAdd(item, true) && compForbiddable != null)
			{
				compForbiddable.Forbidden = false;
			}
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0009AAC4 File Offset: 0x00098CC4
		public void TransferCaravanItemsToCarrier(Pawn_InventoryTracker carrierInventory)
		{
			List<Thing> list = new List<Thing>();
			list.AddRange(this.pawn.inventory.unpackedCaravanItems);
			foreach (Thing thing in list)
			{
				if (MassUtility.IsOverEncumbered(carrierInventory.pawn))
				{
					break;
				}
				if (this.innerContainer.Contains(thing))
				{
					this.pawn.inventory.innerContainer.TryTransferToContainer(thing, carrierInventory.innerContainer, thing.stackCount, true);
				}
				this.unpackedCaravanItems.Remove(thing);
			}
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0009AB74 File Offset: 0x00098D74
		public void DropAllPackingCaravanThings()
		{
			if (this.pawn.Spawned)
			{
				this.DropAllNearPawnHelper(this.pawn.Position, false, false, true);
				this.ClearHaulingCaravanCache();
			}
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0009AB9D File Offset: 0x00098D9D
		public void ClearHaulingCaravanCache()
		{
			this.unpackedCaravanItems.Clear();
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0009ABAA File Offset: 0x00098DAA
		public bool NotForSale(Thing item)
		{
			return this.itemsNotForSale.Contains(item);
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0009ABB8 File Offset: 0x00098DB8
		public void TryAddItemNotForSale(Thing item)
		{
			if (this.innerContainer.TryAdd(item, false))
			{
				this.itemsNotForSale.Add(item);
			}
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0009ABD5 File Offset: 0x00098DD5
		public void Notify_ItemRemoved(Thing item)
		{
			this.itemsNotForSale.Remove(item);
			this.unpackedCaravanItems.Remove(item);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0009AC08 File Offset: 0x00098E08
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0009AC10 File Offset: 0x00098E10
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0009AC1E File Offset: 0x00098E1E
		public IEnumerable<Thing> GetDrugs()
		{
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.TryGetComp<CompDrug>() != null)
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0009AC2E File Offset: 0x00098E2E
		public IEnumerable<Thing> GetCombatEnhancingDrugs()
		{
			foreach (Thing thing in this.innerContainer)
			{
				CompDrug compDrug = thing.TryGetComp<CompDrug>();
				if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0009AC3E File Offset: 0x00098E3E
		public Thing FindCombatEnhancingDrug()
		{
			return this.GetCombatEnhancingDrugs().FirstOrDefault<Thing>();
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0009AC4B File Offset: 0x00098E4B
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.IsColonistPlayerControlled && this.pawn.Drafted && Find.Selector.SingleSelectedThing == this.pawn)
			{
				this.usableDrugsTmp.Clear();
				foreach (Thing thing in this.GetDrugs())
				{
					if (FoodUtility.WillIngestFromInventoryNow(this.pawn, thing) && this.pawn.CanTakeDrug(thing.def))
					{
						this.usableDrugsTmp.Add(thing);
					}
				}
				if (this.usableDrugsTmp.Count == 0)
				{
					yield break;
				}
				if (this.usableDrugsTmp.Count == 1)
				{
					Thing drug = this.usableDrugsTmp[0];
					yield return new Command_Action
					{
						defaultLabel = "ConsumeThing".Translate(drug.LabelNoCount, drug),
						defaultDesc = drug.LabelCapNoCount + ": " + drug.def.description.CapitalizeFirst(),
						icon = drug.def.uiIcon,
						iconAngle = drug.def.uiIconAngle,
						iconOffset = drug.def.uiIconOffset,
						action = delegate()
						{
							FoodUtility.IngestFromInventoryNow(this.pawn, drug);
						}
					};
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "TakeDrug".Translate(),
						defaultDesc = "TakeDrugDesc".Translate(),
						icon = Pawn_InventoryTracker.DrugTex,
						action = delegate()
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							using (List<Thing>.Enumerator enumerator2 = this.usableDrugsTmp.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									Thing drug = enumerator2.Current;
									list.Add(new FloatMenuOption("ConsumeThing".Translate(drug.LabelNoCount, drug), delegate()
									{
										FoodUtility.IngestFromInventoryNow(this.pawn, drug);
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x040012B5 RID: 4789
		public Pawn pawn;

		// Token: 0x040012B6 RID: 4790
		public ThingOwner<Thing> innerContainer;

		// Token: 0x040012B7 RID: 4791
		private bool unloadEverything;

		// Token: 0x040012B8 RID: 4792
		private List<Thing> itemsNotForSale = new List<Thing>();

		// Token: 0x040012B9 RID: 4793
		private List<Thing> unpackedCaravanItems = new List<Thing>();

		// Token: 0x040012BA RID: 4794
		public static readonly Texture2D DrugTex = ContentFinder<Texture2D>.Get("UI/Commands/TakeDrug", true);

		// Token: 0x040012BB RID: 4795
		private static List<ThingDefCount> tmpDrugsToKeep = new List<ThingDefCount>();

		// Token: 0x040012BC RID: 4796
		private static List<Thing> tmpThingList = new List<Thing>();

		// Token: 0x040012BD RID: 4797
		private List<Thing> usableDrugsTmp = new List<Thing>();
	}
}

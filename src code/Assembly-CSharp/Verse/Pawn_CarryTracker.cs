using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000371 RID: 881
	public class Pawn_CarryTracker : IThingHolder, IExposable
	{
		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001926 RID: 6438 RVA: 0x00097164 File Offset: 0x00095364
		public Thing CarriedThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001927 RID: 6439 RVA: 0x00097181 File Offset: 0x00095381
		public bool Full
		{
			get
			{
				return this.AvailableStackSpace(this.CarriedThing.def) <= 0;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001928 RID: 6440 RVA: 0x0009719A File Offset: 0x0009539A
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x000971A2 File Offset: 0x000953A2
		public Pawn_CarryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x000971BF File Offset: 0x000953BF
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x000971DB File Offset: 0x000953DB
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x000971E3 File Offset: 0x000953E3
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x000971F4 File Offset: 0x000953F4
		public int AvailableStackSpace(ThingDef td)
		{
			int num = this.MaxStackSpaceEver(td);
			if (this.CarriedThing != null)
			{
				num -= this.CarriedThing.stackCount;
			}
			return num;
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x00097220 File Offset: 0x00095420
		public int MaxStackSpaceEver(ThingDef td)
		{
			int b = Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.CarryingCapacity, true, -1) / td.VolumePerUnit);
			return Mathf.Min(td.stackLimit, b);
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00097258 File Offset: 0x00095458
		public bool TryStartCarry(Thing item)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				Log.Error("Dead/downed/deathresting pawn " + this.pawn + " tried to start carry item.");
				return false;
			}
			if (this.innerContainer.TryAdd(item, true))
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				return true;
			}
			return false;
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x000972DC File Offset: 0x000954DC
		public int TryStartCarry(Thing item, int count, bool reserve = true)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				Log.Error(string.Concat(new object[]
				{
					"Dead/downed/deathresting pawn ",
					this.pawn,
					" tried to start carry ",
					item.ToStringSafe<Thing>()
				}));
				return 0;
			}
			count = Mathf.Min(count, this.AvailableStackSpace(item.def));
			count = Mathf.Min(count, item.stackCount);
			bool flag = Find.Selector.IsSelected(item);
			Thing thing = item.SplitOff(count);
			int num = this.innerContainer.TryAdd(thing, count, true);
			if (num > 0 && thing != item)
			{
				this.TryUpdateTransferables(thing);
			}
			if (num > 0)
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				if (reserve)
				{
					this.pawn.Reserve(this.CarriedThing, this.pawn.CurJob, 1, -1, null, true);
				}
				if (flag)
				{
					if (!thing.Destroyed)
					{
						Find.Selector.Select(thing, true, true);
					}
					Find.Selector.Select(this.CarriedThing, true, true);
				}
			}
			return num;
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x00097410 File Offset: 0x00095610
		private void TryUpdateTransferables(Thing splitStack)
		{
			if (splitStack == null)
			{
				return;
			}
			Pawn_JobTracker jobs = this.pawn.jobs;
			JobDriver_HaulToTransporter jobDriver_HaulToTransporter;
			if ((jobDriver_HaulToTransporter = (((jobs != null) ? jobs.curDriver : null) as JobDriver_HaulToTransporter)) != null)
			{
				CompTransporter transporter = jobDriver_HaulToTransporter.Transporter;
				TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(splitStack, (transporter != null) ? transporter.leftToLoad : null, TransferAsOneMode.PodsOrCaravanPacking);
				if (transferableOneWay != null && !transferableOneWay.things.Contains(splitStack) && transferableOneWay.MaxCount + splitStack.stackCount <= transferableOneWay.CountToTransfer)
				{
					transferableOneWay.things.Add(splitStack);
				}
			}
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x00097490 File Offset: 0x00095690
		public bool TryDropCarriedThing(IntVec3 dropLoc, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x000974E8 File Offset: 0x000956E8
		public bool TryDropCarriedThing(IntVec3 dropLoc, int count, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, count, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x00097544 File Offset: 0x00095744
		public int CarriedCount(ThingDef def)
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

		// Token: 0x06001935 RID: 6453 RVA: 0x000975A8 File Offset: 0x000957A8
		public void DestroyCarriedThing()
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x000975B8 File Offset: 0x000957B8
		public void CarryHandsTick()
		{
			this.innerContainer.ThingOwnerTick(true);
			Pawn pawn;
			if ((pawn = (this.CarriedThing as Pawn)) != null && pawn.DevelopmentalStage.Baby())
			{
				Pawn_IdeoTracker ideo = pawn.ideo;
				if (ideo == null)
				{
					return;
				}
				ideo.IncreaseIdeoExposureIfBabyTick(this.pawn.Ideo, 1);
			}
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x00097609 File Offset: 0x00095809
		public IEnumerable<Gizmo> GetGizmos()
		{
			Gizmo gizmo;
			if ((gizmo = ContainingSelectionUtility.SelectCarriedThingGizmo(this.pawn, this.CarriedThing)) != null)
			{
				yield return gizmo;
			}
			if (this.pawn.Drafted && this.CarriedThing is Pawn)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandDropPawn".Translate(this.CarriedThing),
					defaultDesc = "CommandDropPawnDesc".Translate(),
					action = delegate()
					{
						Thing thing;
						this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
					},
					icon = TexCommand.DropCarriedPawn
				};
			}
			if (ModsConfig.BiotechActive && DebugSettings.ShowDevGizmos)
			{
				CompDissolution compDissolution = this.CarriedThing.TryGetComp<CompDissolution>();
				if (compDissolution != null)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Dissolution event",
						action = delegate()
						{
							compDissolution.TriggerDissolutionEvent(1);
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x0400129C RID: 4764
		public Pawn pawn;

		// Token: 0x0400129D RID: 4765
		public ThingOwner<Thing> innerContainer;
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000372 RID: 882
	public class Pawn_EquipmentTracker : IThingHolder, IExposable
	{
		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001939 RID: 6457 RVA: 0x0009764C File Offset: 0x0009584C
		// (set) Token: 0x0600193A RID: 6458 RVA: 0x00097698 File Offset: 0x00095898
		public ThingWithComps Primary
		{
			get
			{
				for (int i = 0; i < this.equipment.Count; i++)
				{
					if (this.equipment[i].def.equipmentType == EquipmentType.Primary)
					{
						return this.equipment[i];
					}
				}
				return null;
			}
			private set
			{
				if (this.Primary == value)
				{
					return;
				}
				if (value != null && value.def.equipmentType != EquipmentType.Primary)
				{
					Log.Error("Tried to set non-primary equipment as primary.");
					return;
				}
				if (this.Primary != null)
				{
					this.equipment.Remove(this.Primary);
				}
				if (value != null)
				{
					this.equipment.TryAdd(value, true);
				}
				if (this.pawn.drafter != null)
				{
					this.pawn.drafter.Notify_PrimaryWeaponChanged();
				}
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x00097713 File Offset: 0x00095913
		public CompEquippable PrimaryEq
		{
			get
			{
				if (this.Primary == null)
				{
					return null;
				}
				return this.Primary.GetComp<CompEquippable>();
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x0600193C RID: 6460 RVA: 0x0009772A File Offset: 0x0009592A
		public List<ThingWithComps> AllEquipmentListForReading
		{
			get
			{
				return this.equipment.InnerListForReading;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x00097737 File Offset: 0x00095937
		public IEnumerable<Verb> AllEquipmentVerbs
		{
			get
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					ThingWithComps thingWithComps = list[i];
					List<Verb> verbs = thingWithComps.GetComp<CompEquippable>().AllVerbs;
					for (int j = 0; j < verbs.Count; j = num + 1)
					{
						yield return verbs[j];
						num = j;
					}
					verbs = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x0600193E RID: 6462 RVA: 0x00097747 File Offset: 0x00095947
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0009774F File Offset: 0x0009594F
		public Pawn_EquipmentTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.equipment = new ThingOwner<ThingWithComps>(this);
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x0009776C File Offset: 0x0009596C
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<ThingWithComps>>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_References.Look<Thing>(ref this.bondedWeapon, "bondedWeapon", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					foreach (Verb verb in allEquipmentListForReading[i].GetComp<CompEquippable>().AllVerbs)
					{
						verb.caster = this.pawn;
					}
				}
			}
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x00097818 File Offset: 0x00095A18
		public void EquipmentTrackerTick()
		{
			List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				allEquipmentListForReading[i].GetComp<CompEquippable>().verbTracker.VerbsTick();
			}
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x00097853 File Offset: 0x00095A53
		public void EquipmentTrackerTickRare()
		{
			this.equipment.ThingOwnerTickRare(true);
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x00097861 File Offset: 0x00095A61
		public bool HasAnything()
		{
			return this.equipment.Any;
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x00097870 File Offset: 0x00095A70
		public void MakeRoomFor(ThingWithComps eq)
		{
			if (eq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				ThingWithComps thingWithComps;
				if (this.TryDropEquipment(this.Primary, out thingWithComps, this.pawn.Position, true))
				{
					if (thingWithComps != null)
					{
						thingWithComps.SetForbidden(false, true);
						return;
					}
				}
				else
				{
					Log.Error(this.pawn + " couldn't make room for equipment " + eq);
				}
			}
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x000978D1 File Offset: 0x00095AD1
		public void Remove(ThingWithComps eq)
		{
			this.equipment.Remove(eq);
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x000978E0 File Offset: 0x00095AE0
		public bool TryDropEquipment(ThingWithComps eq, out ThingWithComps resultingEq, IntVec3 pos, bool forbid = true)
		{
			if (!pos.IsValid)
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" tried to drop ",
					eq,
					" at invalid cell."
				}));
				resultingEq = null;
				return false;
			}
			if (this.equipment.TryDrop(eq, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingEq, null, null))
			{
				if (resultingEq != null)
				{
					resultingEq.SetForbidden(forbid, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x00097958 File Offset: 0x00095B58
		public void DropAllEquipment(IntVec3 pos, bool forbid = true, bool rememberPrimary = false)
		{
			for (int i = this.equipment.Count - 1; i >= 0; i--)
			{
				bool flag = this.equipment[i] == this.Primary;
				ThingWithComps droppedWeapon;
				if (this.TryDropEquipment(this.equipment[i], out droppedWeapon, pos, forbid) && rememberPrimary && flag)
				{
					this.pawn.mindState.droppedWeapon = droppedWeapon;
				}
			}
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x000979BF File Offset: 0x00095BBF
		public bool TryTransferEquipmentToContainer(ThingWithComps eq, ThingOwner container)
		{
			return this.equipment.TryTransferToContainer(eq, container, true);
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x000979CF File Offset: 0x00095BCF
		public void DestroyEquipment(ThingWithComps eq)
		{
			if (!this.equipment.Contains(eq))
			{
				Log.Warning("Tried to destroy equipment " + eq + " but it's not here.");
				return;
			}
			this.Remove(eq);
			eq.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x00097A03 File Offset: 0x00095C03
		public void DestroyAllEquipment(DestroyMode mode = DestroyMode.Vanish)
		{
			this.equipment.ClearAndDestroyContents(mode);
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x00097A11 File Offset: 0x00095C11
		public bool Contains(Thing eq)
		{
			return this.equipment.Contains(eq);
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x00097A1F File Offset: 0x00095C1F
		internal void Notify_PrimaryDestroyed()
		{
			if (this.Primary != null)
			{
				this.Remove(this.Primary);
			}
			if (this.pawn.Spawned)
			{
				this.pawn.stances.CancelBusyStanceSoft();
			}
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x00097A54 File Offset: 0x00095C54
		public void AddEquipment(ThingWithComps newEq)
		{
			if (newEq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Pawn ",
					this.pawn.LabelCap,
					" got primaryInt equipment ",
					newEq,
					" while already having primaryInt equipment ",
					this.Primary
				}));
				return;
			}
			if (this.equipment.TryAdd(newEq, true) && newEq.def.equipmentType == EquipmentType.Primary)
			{
				this.pawn.mindState.droppedWeapon = null;
			}
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x00097AE9 File Offset: 0x00095CE9
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (PawnAttackGizmoUtility.CanShowEquipmentGizmos())
			{
				try
				{
					Pawn_EquipmentTracker.tmpKeybindings.Add(KeyBindingDefOf.Misc1);
					Pawn_EquipmentTracker.tmpKeybindings.Add(KeyBindingDefOf.Misc2);
					Pawn_EquipmentTracker.tmpKeybindings.Add(KeyBindingDefOf.Misc3);
					List<ThingWithComps> list = this.AllEquipmentListForReading;
					ThingWithComps primaryMelee = list.FirstOrDefault((ThingWithComps w) => w.def.IsMeleeWeapon);
					ThingWithComps primaryRanged = list.FirstOrDefault((ThingWithComps w) => w.def.IsRangedWeapon);
					if (primaryMelee != null)
					{
						KeyBindingDef misc = KeyBindingDefOf.Misc2;
						foreach (Gizmo gizmo in Pawn_EquipmentTracker.<GetGizmos>g__YieldGizmos|30_0(primaryMelee, misc))
						{
							yield return gizmo;
						}
						IEnumerator<Gizmo> enumerator = null;
					}
					if (primaryRanged != null)
					{
						KeyBindingDef misc2 = KeyBindingDefOf.Misc1;
						foreach (Gizmo gizmo2 in Pawn_EquipmentTracker.<GetGizmos>g__YieldGizmos|30_0(primaryRanged, misc2))
						{
							yield return gizmo2;
						}
						IEnumerator<Gizmo> enumerator = null;
					}
					int num;
					for (int i = 0; i < list.Count; i = num + 1)
					{
						ThingWithComps thingWithComps = list[i];
						if (thingWithComps != primaryMelee && thingWithComps != primaryRanged)
						{
							foreach (Gizmo gizmo3 in Pawn_EquipmentTracker.<GetGizmos>g__YieldGizmos|30_0(thingWithComps, null))
							{
								yield return gizmo3;
							}
							IEnumerator<Gizmo> enumerator = null;
						}
						num = i;
					}
					list = null;
					primaryMelee = null;
					primaryRanged = null;
				}
				finally
				{
					Pawn_EquipmentTracker.tmpKeybindings.Clear();
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00097AFC File Offset: 0x00095CFC
		public void Notify_EquipmentAdded(ThingWithComps eq)
		{
			foreach (Verb verb in eq.GetComp<CompEquippable>().AllVerbs)
			{
				verb.caster = this.pawn;
				verb.Notify_PickedUp();
			}
			eq.Notify_Equipped(this.pawn);
			if (ModsConfig.RoyaltyActive && eq.def.equipmentType == EquipmentType.Primary && this.bondedWeapon != null && !this.bondedWeapon.Destroyed)
			{
				CompBladelinkWeapon compBladelinkWeapon = this.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.Notify_WieldedOtherWeapon();
				}
			}
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00097BA8 File Offset: 0x00095DA8
		public void Notify_EquipmentRemoved(ThingWithComps eq)
		{
			eq.Notify_Unequipped(this.pawn);
			if (ModsConfig.RoyaltyActive)
			{
				CompBladelinkWeapon compBladelinkWeapon = eq.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.Notify_EquipmentLost(this.pawn);
				}
			}
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00097BE0 File Offset: 0x00095DE0
		public void Notify_PawnSpawned()
		{
			if (this.HasAnything() && this.pawn.Downed && !this.pawn.GetPosture().InBed())
			{
				if (this.pawn.kindDef.destroyGearOnDrop)
				{
					this.DestroyAllEquipment(DestroyMode.Vanish);
					return;
				}
				this.DropAllEquipment(this.pawn.Position, true, false);
			}
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x00097C44 File Offset: 0x00095E44
		public void Notify_PawnDied()
		{
			if (ModsConfig.RoyaltyActive && this.bondedWeapon != null)
			{
				CompBladelinkWeapon compBladelinkWeapon = this.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.UnCode();
				}
			}
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x00097C78 File Offset: 0x00095E78
		public void Notify_KilledPawn()
		{
			foreach (ThingWithComps thingWithComps in this.equipment)
			{
				thingWithComps.Notify_KilledPawn(this.pawn);
			}
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x00097CD0 File Offset: 0x00095ED0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.equipment;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x00097CD8 File Offset: 0x00095ED8
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x00097CF2 File Offset: 0x00095EF2
		[CompilerGenerated]
		internal static IEnumerable<Gizmo> <GetGizmos>g__YieldGizmos|30_0(ThingWithComps eq, KeyBindingDef preferredHotKey)
		{
			foreach (Command command in eq.GetComp<CompEquippable>().GetVerbsCommands())
			{
				if (Pawn_EquipmentTracker.tmpKeybindings.Count > 0)
				{
					if (preferredHotKey != null && Pawn_EquipmentTracker.tmpKeybindings.Contains(preferredHotKey))
					{
						command.hotKey = preferredHotKey;
						Pawn_EquipmentTracker.tmpKeybindings.Remove(preferredHotKey);
					}
					else
					{
						command.hotKey = Pawn_EquipmentTracker.tmpKeybindings.Pop<KeyBindingDef>();
					}
				}
				yield return command;
			}
			IEnumerator<Command> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400129E RID: 4766
		public Pawn pawn;

		// Token: 0x0400129F RID: 4767
		private ThingOwner<ThingWithComps> equipment;

		// Token: 0x040012A0 RID: 4768
		public Thing bondedWeapon;

		// Token: 0x040012A1 RID: 4769
		private static List<KeyBindingDef> tmpKeybindings = new List<KeyBindingDef>();
	}
}

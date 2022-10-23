using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200026F RID: 623
	public abstract class Zone : IExposable, ISelectable, ILoadReferenceable
	{
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x000676DB File Offset: 0x000658DB
		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x000676E8 File Offset: 0x000658E8
		public IntVec3 Position
		{
			get
			{
				if (this.cells.Count == 0)
				{
					return IntVec3.Invalid;
				}
				return this.cells[0];
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060011B6 RID: 4534 RVA: 0x00067709 File Offset: 0x00065909
		public Material Material
		{
			get
			{
				if (this.materialInt == null)
				{
					this.materialInt = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
					this.materialInt.renderQueue = 3600;
				}
				return this.materialInt;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060011B7 RID: 4535 RVA: 0x00067741 File Offset: 0x00065941
		public string BaseLabel
		{
			get
			{
				return this.baseLabel;
			}
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00067749 File Offset: 0x00065949
		public IEnumerator<IntVec3> GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.cells.Count; i = num + 1)
			{
				yield return this.cells[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x00067758 File Offset: 0x00065958
		public List<IntVec3> Cells
		{
			get
			{
				if (!this.cellsShuffled)
				{
					this.cells.Shuffle<IntVec3>();
					this.cellsShuffled = true;
				}
				return this.cells;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x0006777A File Offset: 0x0006597A
		public int CellCount
		{
			get
			{
				return this.cells.Count;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x00067787 File Offset: 0x00065987
		public IEnumerable<Thing> AllContainedThings
		{
			get
			{
				ThingGrid grids = this.Map.thingGrid;
				int num;
				for (int i = 0; i < this.cells.Count; i = num + 1)
				{
					List<Thing> thingList = grids.ThingsListAt(this.cells[i]);
					for (int j = 0; j < thingList.Count; j = num + 1)
					{
						yield return thingList[j];
						num = j;
					}
					thingList = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x00067798 File Offset: 0x00065998
		public bool ContainsStaticFire
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastStaticFireCheckTick + 1000)
				{
					this.lastStaticFireCheckResult = false;
					for (int i = 0; i < this.cells.Count; i++)
					{
						if (this.cells[i].ContainsStaticFire(this.Map))
						{
							this.lastStaticFireCheckResult = true;
							break;
						}
					}
				}
				return this.lastStaticFireCheckResult;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060011BE RID: 4542
		protected abstract Color NextZoneColor { get; }

		// Token: 0x060011BF RID: 4543 RVA: 0x00067802 File Offset: 0x00065A02
		public Zone()
		{
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00067834 File Offset: 0x00065A34
		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.baseLabel = baseName;
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.ID = Find.UniqueIDsManager.GetNextZoneID();
			this.color = this.NextZoneColor;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x000678A8 File Offset: 0x00065AA8
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.baseLabel, "baseLabel", null, false);
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Undefined, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CheckAddHaulDestination();
			}
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00067944 File Offset: 0x00065B44
		public virtual void AddCell(IntVec3 c)
		{
			if (this.cells.Contains(c))
			{
				Log.Error(string.Concat(new object[]
				{
					"Adding cell to zone which already has it. c=",
					c,
					", zone=",
					this
				}));
				return;
			}
			List<Thing> list = this.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (!thing.def.CanOverlapZones)
				{
					Log.Error("Added zone over zone-incompatible thing " + thing);
					return;
				}
			}
			this.cells.Add(c);
			this.zoneManager.AddZoneGridCell(this, c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Zone);
			AutoHomeAreaMaker.Notify_ZoneCellAdded(c, this);
			this.cellsShuffled = false;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x00067A14 File Offset: 0x00065C14
		public virtual void RemoveCell(IntVec3 c)
		{
			if (!this.cells.Contains(c))
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot remove cell from zone which doesn't have it. c=",
					c,
					", zone=",
					this
				}));
				return;
			}
			this.cells.Remove(c);
			this.zoneManager.ClearZoneGridCell(c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Zone);
			this.cellsShuffled = false;
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00067AA8 File Offset: 0x00065CA8
		public virtual void Delete()
		{
			SoundDefOf.Designate_ZoneDelete.PlayOneShotOnCamera(this.Map);
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
			else
			{
				while (this.cells.Count > 0)
				{
					this.RemoveCell(this.cells[this.cells.Count - 1]);
				}
			}
			Find.Selector.Deselect(this);
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00067B11 File Offset: 0x00065D11
		public void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00067B1F File Offset: 0x00065D1F
		public virtual void PostRegister()
		{
			this.CheckAddHaulDestination();
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00067B28 File Offset: 0x00065D28
		public virtual void PostDeregister()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00067B50 File Offset: 0x00065D50
		public bool ContainsCell(IntVec3 c)
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				if (this.cells[i] == c)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00067B8C File Offset: 0x00065D8C
		public virtual string GetInspectString()
		{
			return string.Format("{0}: {1}", "Size".Translate().CapitalizeFirst(), this.CellCount);
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return null;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00067BC5 File Offset: 0x00065DC5
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield return new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/RenameZone", true),
				defaultLabel = "CommandRenameZoneLabel".Translate(),
				defaultDesc = "CommandRenameZoneDesc".Translate(),
				action = delegate()
				{
					Dialog_RenameZone dialog_RenameZone = new Dialog_RenameZone(this);
					if (KeyBindingDefOf.Misc1.IsDown)
					{
						dialog_RenameZone.WasOpenedByHotkey();
					}
					Find.WindowStack.Add(dialog_RenameZone);
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield return new Command_Toggle
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/HideZone", true),
				defaultLabel = (this.hidden ? "CommandUnhideZoneLabel".Translate() : "CommandHideZoneLabel".Translate()),
				defaultDesc = "CommandHideZoneDesc".Translate(),
				isActive = (() => this.hidden),
				toggleAction = delegate()
				{
					this.hidden = !this.hidden;
					foreach (IntVec3 loc in this.Cells)
					{
						this.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.Zone);
					}
				},
				hotKey = KeyBindingDefOf.Misc2
			};
			foreach (Gizmo gizmo in this.GetZoneAddGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Designator designator = DesignatorUtility.FindAllowedDesignator<Designator_ZoneDelete_Shrink>();
			if (designator != null)
			{
				yield return designator;
			}
			yield return new Command_Action
			{
				icon = TexButton.DeleteX,
				defaultLabel = "CommandDeleteZoneLabel".Translate(),
				defaultDesc = "CommandDeleteZoneDesc".Translate(),
				action = new Action(this.Delete),
				hotKey = KeyBindingDefOf.Designator_Deconstruct
			};
			yield break;
			yield break;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00067BD5 File Offset: 0x00065DD5
		public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00067BDC File Offset: 0x00065DDC
		public void CheckContiguous()
		{
			if (this.cells.Count == 0)
			{
				return;
			}
			if (Zone.extantGrid == null)
			{
				Zone.extantGrid = new BoolGrid(this.Map);
			}
			else
			{
				Zone.extantGrid.ClearAndResizeTo(this.Map);
			}
			if (Zone.foundGrid == null)
			{
				Zone.foundGrid = new BoolGrid(this.Map);
			}
			else
			{
				Zone.foundGrid.ClearAndResizeTo(this.Map);
			}
			for (int i = 0; i < this.cells.Count; i++)
			{
				Zone.extantGrid.Set(this.cells[i], true);
			}
			Predicate<IntVec3> passCheck = (IntVec3 c) => Zone.extantGrid[c] && !Zone.foundGrid[c];
			int numFound = 0;
			Action<IntVec3> processor = delegate(IntVec3 c)
			{
				Zone.foundGrid.Set(c, true);
				int numFound = numFound;
				numFound++;
			};
			this.Map.floodFiller.FloodFill(this.cells[0], passCheck, processor, int.MaxValue, false, null);
			if (numFound < this.cells.Count)
			{
				foreach (IntVec3 c2 in this.Map.AllCells)
				{
					if (Zone.extantGrid[c2] && !Zone.foundGrid[c2])
					{
						this.RemoveCell(c2);
					}
				}
			}
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00067D50 File Offset: 0x00065F50
		private void CheckAddHaulDestination()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x00067D78 File Offset: 0x00065F78
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00067D80 File Offset: 0x00065F80
		public string GetUniqueLoadID()
		{
			return "Zone_" + this.ID;
		}

		// Token: 0x04000F14 RID: 3860
		public ZoneManager zoneManager;

		// Token: 0x04000F15 RID: 3861
		public int ID = -1;

		// Token: 0x04000F16 RID: 3862
		public string label;

		// Token: 0x04000F17 RID: 3863
		private string baseLabel;

		// Token: 0x04000F18 RID: 3864
		public List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x04000F19 RID: 3865
		private bool cellsShuffled;

		// Token: 0x04000F1A RID: 3866
		public Color color = Color.white;

		// Token: 0x04000F1B RID: 3867
		private Material materialInt;

		// Token: 0x04000F1C RID: 3868
		public bool hidden;

		// Token: 0x04000F1D RID: 3869
		private int lastStaticFireCheckTick = -9999;

		// Token: 0x04000F1E RID: 3870
		private bool lastStaticFireCheckResult;

		// Token: 0x04000F1F RID: 3871
		private const int StaticFireCheckInterval = 1000;

		// Token: 0x04000F20 RID: 3872
		private static BoolGrid extantGrid;

		// Token: 0x04000F21 RID: 3873
		private static BoolGrid foundGrid;
	}
}

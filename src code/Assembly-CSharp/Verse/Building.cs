using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003C2 RID: 962
	public class Building : ThingWithComps
	{
		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001B46 RID: 6982 RVA: 0x000A7ADC File Offset: 0x000A5CDC
		public CompPower PowerComp
		{
			get
			{
				return base.GetComp<CompPower>();
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001B47 RID: 6983 RVA: 0x000A7AE4 File Offset: 0x000A5CE4
		public ColorDef PaintColorDef
		{
			get
			{
				return this.paintColorDef;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001B48 RID: 6984 RVA: 0x000A7AEC File Offset: 0x000A5CEC
		public override Color DrawColor
		{
			get
			{
				if (this.paintColorDef != null)
				{
					return this.paintColorDef.color;
				}
				return base.DrawColor;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001B49 RID: 6985 RVA: 0x000A7B08 File Offset: 0x000A5D08
		public virtual bool TransmitsPowerNow
		{
			get
			{
				CompPower powerComp = this.PowerComp;
				return powerComp != null && powerComp.Props.transmitsPower;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (set) Token: 0x06001B4A RID: 6986 RVA: 0x000A7B2C File Offset: 0x000A5D2C
		public override int HitPoints
		{
			set
			{
				int hitPoints = this.HitPoints;
				base.HitPoints = value;
				BuildingsDamageSectionLayerUtility.Notify_BuildingHitPointsChanged(this, hitPoints);
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001B4B RID: 6987 RVA: 0x000A7B4E File Offset: 0x000A5D4E
		public virtual int MaxItemsInCell
		{
			get
			{
				return this.def.building.maxItemsInCell;
			}
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x000A7B60 File Offset: 0x000A5D60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.canChangeTerrainOnDestroyed, "canChangeTerrainOnDestroyed", true, false);
			Scribe_Defs.Look<ColorDef>(ref this.paintColorDef, "paintColorDef");
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x000A7B8C File Offset: 0x000A5D8C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.def.IsEdifice())
			{
				map.edificeGrid.Register(this);
				if (this.def.Fillage == FillCategory.Full)
				{
					map.terrainGrid.Drawer.SetDirty();
				}
				if (this.def.AffectsFertility)
				{
					map.fertilityGrid.Drawer.SetDirty();
				}
			}
			base.SpawnSetup(map, respawningAfterLoad);
			base.Map.listerBuildings.Add(this);
			if (this.def.coversFloor)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Terrain, true, false);
			}
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					base.Map.mapDrawer.MapMeshDirty(intVec, MapMeshFlag.Buildings);
					base.Map.glowGrid.MarkGlowGridDirty(intVec);
					if (!SnowGrid.CanCoexistWithSnow(this.def))
					{
						base.Map.snowGrid.SetDepth(intVec, 0f);
					}
				}
			}
			if (base.Faction == Faction.OfPlayer && this.def.building != null && this.def.building.spawnedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(this.def.building.spawnedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
			AutoHomeAreaMaker.Notify_BuildingSpawned(this);
			if (this.def.building != null && !this.def.building.soundAmbient.NullOrUndefined())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					SoundInfo info = SoundInfo.InMap(this, MaintenanceType.None);
					this.sustainerAmbient = this.def.building.soundAmbient.TrySpawnSustainer(info);
				});
			}
			base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
			base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
			base.Map.listerBuldingOfDefInProximity.Notify_BuildingSpawned(this);
			base.Map.listerBuildingWithTagInProximity.Notify_BuildingSpawned(this);
			if (!this.CanBeSeenOver())
			{
				base.Map.exitMapGrid.Notify_LOSBlockerSpawned();
			}
			SmoothSurfaceDesignatorUtility.Notify_BuildingSpawned(this);
			map.avoidGrid.Notify_BuildingSpawned(this);
			map.lordManager.Notify_BuildingSpawned(this);
			map.animalPenManager.Notify_BuildingSpawned(this);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000A7DB4 File Offset: 0x000A5FB4
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (this.def.IsEdifice())
			{
				map.edificeGrid.DeRegister(this);
				if (this.def.Fillage == FillCategory.Full)
				{
					map.terrainGrid.Drawer.SetDirty();
				}
				if (this.def.AffectsFertility)
				{
					map.fertilityGrid.Drawer.SetDirty();
				}
			}
			if (mode != DestroyMode.WillReplace)
			{
				if (this.def.MakeFog)
				{
					map.fogGrid.Notify_FogBlockerRemoved(base.Position);
				}
				if (this.def.holdsRoof)
				{
					RoofCollapseCellsFinder.Notify_RoofHolderDespawned(this, map);
				}
				if (this.def.IsSmoothable)
				{
					SmoothSurfaceDesignatorUtility.Notify_BuildingDespawned(this, map);
				}
			}
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.End();
			}
			CellRect cellRect = this.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 loc = new IntVec3(j, 0, i);
					MapMeshFlag mapMeshFlag = MapMeshFlag.Buildings;
					if (this.def.coversFloor)
					{
						mapMeshFlag |= MapMeshFlag.Terrain;
					}
					if (this.def.Fillage == FillCategory.Full)
					{
						mapMeshFlag |= MapMeshFlag.Roofs;
						mapMeshFlag |= MapMeshFlag.Snow;
					}
					map.mapDrawer.MapMeshDirty(loc, mapMeshFlag);
					map.glowGrid.MarkGlowGridDirty(loc);
				}
			}
			map.listerBuildings.Remove(this);
			map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
			map.listerArtificialBuildingsForMeditation.Notify_BuildingDeSpawned(this);
			map.listerBuldingOfDefInProximity.Notify_BuildingDeSpawned(this);
			map.listerBuildingWithTagInProximity.Notify_BuildingDeSpawned(this);
			if (this.def.building.leaveTerrain != null && Current.ProgramState == ProgramState.Playing && this.canChangeTerrainOnDestroyed)
			{
				foreach (IntVec3 c in this.OccupiedRect())
				{
					map.terrainGrid.SetTerrain(c, this.def.building.leaveTerrain);
				}
			}
			map.designationManager.Notify_BuildingDespawned(this);
			if (!this.CanBeSeenOver())
			{
				map.exitMapGrid.Notify_LOSBlockerDespawned();
			}
			if (this.def.building.hasFuelingPort)
			{
				CompLaunchable compLaunchable = FuelingPortUtility.LaunchableAt(FuelingPortUtility.GetFuelingPortCell(base.Position, base.Rotation), map);
				if (compLaunchable != null)
				{
					compLaunchable.Notify_FuelingPortSourceDeSpawned();
				}
			}
			map.avoidGrid.Notify_BuildingDespawned(this);
			map.lordManager.Notify_BuildingDespawned(this);
			map.animalPenManager.Notify_BuildingDespawned(this);
			if (this.MaxItemsInCell >= 2)
			{
				foreach (IntVec3 intVec in this.OccupiedRect())
				{
					int itemCount = intVec.GetItemCount(map);
					if (itemCount > 1 && itemCount > intVec.GetMaxItemsAllowedInCell(map))
					{
						for (int k = 0; k < itemCount - 1; k++)
						{
							Thing firstItem = intVec.GetFirstItem(map);
							if (firstItem == null)
							{
								break;
							}
							firstItem.DeSpawn(DestroyMode.Vanish);
							GenPlace.TryPlaceThing(firstItem, intVec, map, ThingPlaceMode.Near, null, null, default(Rot4));
							if (intVec.GetItemCount(map) <= intVec.GetMaxItemsAllowedInCell(map))
							{
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x000A8100 File Offset: 0x000A6300
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			SmoothableWallUtility.Notify_BuildingDestroying(this, mode);
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_BuildingLost(this, null);
			}
			base.Destroy(mode);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
			if (spawned)
			{
				if (mode == DestroyMode.Deconstruct)
				{
					SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map, false));
				}
				else if (mode == DestroyMode.KillFinalize)
				{
					this.DoDestroyEffects(map);
				}
			}
			if (spawned)
			{
				ThingUtility.CheckAutoRebuildOnDestroyed(this, mode, map, this.def);
			}
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x000A8188 File Offset: 0x000A6388
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
				base.Map.listerBuildingWithTagInProximity.Notify_BuildingDeSpawned(this);
				base.Map.listerBuildings.Remove(this);
			}
			base.SetFaction(newFaction, recruiter);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
				base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
				base.Map.listerBuildingWithTagInProximity.Notify_BuildingSpawned(this);
				base.Map.listerBuildings.Add(this);
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.PowerGrid, true, false);
				if (newFaction == Faction.OfPlayer)
				{
					AutoHomeAreaMaker.Notify_BuildingClaimed(this);
				}
			}
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x000A8250 File Offset: 0x000A6450
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			if (base.Faction != null && base.Spawned && base.Faction != Faction.OfPlayer)
			{
				for (int i = 0; i < base.Map.lordManager.lords.Count; i++)
				{
					Lord lord = base.Map.lordManager.lords[i];
					if (lord.faction == base.Faction)
					{
						lord.Notify_BuildingDamaged(this, dinfo);
					}
				}
			}
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed && base.Faction != null)
			{
				base.Faction.Notify_BuildingTookDamage(this, dinfo);
			}
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x000A82F2 File Offset: 0x000A64F2
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (base.Spawned)
			{
				base.Map.listerBuildingsRepairable.Notify_BuildingTookDamage(this);
			}
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x000A8318 File Offset: 0x000A6518
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x000A8346 File Offset: 0x000A6546
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (((this.def.BuildableByPlayer && this.def.passability != Traversability.Impassable && !this.def.IsDoor) || this.def.building.forceShowRoomStats) && Gizmo_RoomStats.GetRoomToShowStatsFor(this) != null && Find.Selector.SingleSelectedObject == this)
			{
				yield return new Gizmo_RoomStats(this);
			}
			Gizmo selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(this);
			if (selectMonumentMarkerGizmo != null)
			{
				yield return selectMonumentMarkerGizmo;
			}
			if (this.def.Minifiable && base.Faction == Faction.OfPlayer)
			{
				yield return InstallationDesignatorDatabase.DesignatorFor(this.def);
			}
			ColorInt? glowerColorOverride = null;
			CompGlower comp;
			if ((comp = base.GetComp<CompGlower>()) != null && comp.HasGlowColorOverride)
			{
				glowerColorOverride = new ColorInt?(comp.GlowColor);
			}
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.def, base.Stuff, base.StyleSourcePrecept as Precept_Building, base.StyleDef, true, glowerColorOverride);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer || this.def.building.alwaysShowRelatedBuildCommands)
			{
				foreach (Command command2 in BuildRelatedCommandUtility.RelatedBuildCommands(this.def))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x000A8356 File Offset: 0x000A6556
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.PaintColorDef != null && !this.PaintColorDef.label.NullOrEmpty())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "Stat_Building_PaintColor".Translate(), this.PaintColorDef.LabelCap, "Stat_Building_PaintColorDesc".Translate(), 6000, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x000A8368 File Offset: 0x000A6568
		public override bool ClaimableBy(Faction by, StringBuilder reason = null)
		{
			if (!this.def.Claimable)
			{
				return false;
			}
			if (base.Faction == by)
			{
				return false;
			}
			Faction faction;
			if ((faction = base.Faction) == null)
			{
				Map map = base.Map;
				faction = ((map != null) ? map.ParentFaction : null);
			}
			if (base.FactionPreventsClaimingOrAdopting(faction, true, reason))
			{
				return false;
			}
			for (int i = 0; i < base.AllComps.Count; i++)
			{
				if (base.AllComps[i].CompPreventClaimingBy(by))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x000A83E4 File Offset: 0x000A65E4
		public virtual bool DeconstructibleBy(Faction faction)
		{
			return this.def.building.IsDeconstructible && (DebugSettings.godMode || base.Faction == faction || this.ClaimableBy(faction, null) || this.def.building.alwaysDeconstructible);
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual ushort PathWalkCostFor(Pawn p)
		{
			return 0;
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool IsDangerousFor(Pawn p)
		{
			return false;
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x000A8434 File Offset: 0x000A6634
		private void DoDestroyEffects(Map map)
		{
			if (this.def.building.destroyEffecter != null)
			{
				Effecter effecter = this.def.building.destroyEffecter.Spawn(base.Position, map, 1f);
				effecter.Trigger(new TargetInfo(base.Position, map, false), TargetInfo.Invalid, -1);
				effecter.Cleanup();
				return;
			}
			if (!this.def.IsEdifice())
			{
				return;
			}
			SoundDef destroySound = this.GetDestroySound();
			if (destroySound != null)
			{
				destroySound.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			foreach (IntVec3 intVec in this.OccupiedRect())
			{
				int num = this.def.building.isNaturalRock ? 1 : Rand.RangeInclusive(3, 5);
				for (int i = 0; i < num; i++)
				{
					FleckMaker.ThrowDustPuffThick(intVec.ToVector3Shifted(), map, Rand.Range(1.5f, 2f), Color.white);
				}
			}
			if (Find.CurrentMap == map)
			{
				float num2 = this.def.building.destroyShakeAmount;
				if (num2 < 0f)
				{
					num2 = Building.ShakeAmountPerAreaCurve.Evaluate((float)this.def.Size.Area);
				}
				CompLifespan compLifespan = this.TryGetComp<CompLifespan>();
				if (compLifespan == null || compLifespan.age < compLifespan.Props.lifespanTicks)
				{
					Find.CameraDriver.shaker.DoShake(num2);
				}
			}
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x000A85CC File Offset: 0x000A67CC
		private SoundDef GetDestroySound()
		{
			if (!this.def.building.destroySound.NullOrUndefined())
			{
				return this.def.building.destroySound;
			}
			StuffCategoryDef stuffCategoryDef;
			if (this.def.MadeFromStuff && base.Stuff != null && !base.Stuff.stuffProps.categories.NullOrEmpty<StuffCategoryDef>())
			{
				stuffCategoryDef = base.Stuff.stuffProps.categories[0];
			}
			else
			{
				if (this.def.CostList.NullOrEmpty<ThingDefCountClass>() || !this.def.CostList[0].thingDef.IsStuff || this.def.CostList[0].thingDef.stuffProps.categories.NullOrEmpty<StuffCategoryDef>())
				{
					return null;
				}
				stuffCategoryDef = this.def.CostList[0].thingDef.stuffProps.categories[0];
			}
			switch (this.def.building.buildingSizeCategory)
			{
			case BuildingSizeCategory.None:
			{
				int area = this.def.Size.Area;
				if (area <= 1 && !stuffCategoryDef.destroySoundSmall.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundSmall;
				}
				if (area <= 4 && !stuffCategoryDef.destroySoundMedium.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundMedium;
				}
				if (!stuffCategoryDef.destroySoundLarge.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundLarge;
				}
				break;
			}
			case BuildingSizeCategory.Small:
				if (!stuffCategoryDef.destroySoundSmall.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundSmall;
				}
				break;
			case BuildingSizeCategory.Medium:
				if (!stuffCategoryDef.destroySoundMedium.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundMedium;
				}
				break;
			case BuildingSizeCategory.Large:
				if (!stuffCategoryDef.destroySoundLarge.NullOrUndefined())
				{
					return stuffCategoryDef.destroySoundLarge;
				}
				break;
			}
			return null;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x000A8790 File Offset: 0x000A6990
		public override void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			if (this.def.building.paintable && Rand.Value < 0.1f)
			{
				this.ChangePaint((from x in DefDatabase<ColorDef>.AllDefs
				where x.colorType == ColorType.Structure
				select x).RandomElement<ColorDef>());
				return;
			}
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x000A8814 File Offset: 0x000A6A14
		public override string GetInspectStringLowPriority()
		{
			string text = base.GetInspectStringLowPriority();
			if (this.def.IsNonDeconstructibleAttackableBuilding)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "AttackToDestroy".Translate();
			}
			return text;
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x000A8860 File Offset: 0x000A6A60
		public void ChangePaint(ColorDef colorDef)
		{
			this.paintColorDef = colorDef;
			this.Notify_ColorChanged();
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x000A886F File Offset: 0x000A6A6F
		public static Gizmo SelectContainedItemGizmo(Thing container, Thing item)
		{
			if (!container.Faction.IsPlayerSafe())
			{
				return null;
			}
			return ContainingSelectionUtility.SelectCarriedThingGizmo(container, item);
		}

		// Token: 0x040013CA RID: 5066
		private Sustainer sustainerAmbient;

		// Token: 0x040013CB RID: 5067
		private ColorDef paintColorDef;

		// Token: 0x040013CC RID: 5068
		public bool canChangeTerrainOnDestroyed = true;

		// Token: 0x040013CD RID: 5069
		private static readonly SimpleCurve ShakeAmountPerAreaCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.07f),
				true
			},
			{
				new CurvePoint(2f, 0.07f),
				true
			},
			{
				new CurvePoint(4f, 0.1f),
				true
			},
			{
				new CurvePoint(9f, 0.2f),
				true
			},
			{
				new CurvePoint(16f, 0.5f),
				true
			}
		};

		// Token: 0x040013CE RID: 5070
		private const float ChanceToGeneratePaintedFromTrader = 0.1f;
	}
}

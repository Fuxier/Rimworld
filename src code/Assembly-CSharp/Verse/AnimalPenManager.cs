using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020001A6 RID: 422
	public class AnimalPenManager
	{
		// Token: 0x06000BA8 RID: 2984 RVA: 0x0004163B File Offset: 0x0003F83B
		public AnimalPenManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00041672 File Offset: 0x0003F872
		public void RebuildAllPens()
		{
			this.ForceRebuildPens();
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0004167A File Offset: 0x0003F87A
		public PenMarkerState GetPenMarkerState(CompAnimalPenMarker marker)
		{
			this.RebuildIfDirty();
			return this.penMarkers[marker];
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00041690 File Offset: 0x0003F890
		public CompAnimalPenMarker GetPenNamed(string name)
		{
			return this.penMarkers.Keys.FirstOrDefault((CompAnimalPenMarker marker) => marker.label == name);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x000416C8 File Offset: 0x0003F8C8
		public ThingFilter GetFixedAutoCutFilter()
		{
			if (this.cachedFixedAutoCutFilter == null)
			{
				this.cachedFixedAutoCutFilter = new ThingFilter();
				foreach (ThingDef thingDef in this.map.Biome.AllWildPlants)
				{
					if (thingDef.plant.allowAutoCut)
					{
						this.cachedFixedAutoCutFilter.SetAllow(thingDef, true);
					}
				}
				this.cachedFixedAutoCutFilter.SetAllow(ThingCategoryDefOf.Stumps, true, null, null);
			}
			return this.cachedFixedAutoCutFilter;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00041764 File Offset: 0x0003F964
		public ThingFilter GetDefaultAutoCutFilter()
		{
			if (this.cachedDefaultAutoCutFilter == null)
			{
				this.cachedDefaultAutoCutFilter = new ThingFilter();
				ThingDef plant_Grass = ThingDefOf.Plant_Grass;
				float num = plant_Grass.ingestible.CachedNutrition / plant_Grass.plant.growDays * 0.5f;
				foreach (ThingDef thingDef in this.GetFixedAutoCutFilter().AllowedThingDefs)
				{
					if (!MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(thingDef) || (thingDef.ingestible.CachedNutrition / thingDef.plant.growDays < num && (thingDef.plant.harvestedThingDef == null || !thingDef.plant.harvestedThingDef.IsIngestible)))
					{
						this.cachedDefaultAutoCutFilter.SetAllow(thingDef, true);
					}
				}
			}
			return this.cachedDefaultAutoCutFilter;
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00041840 File Offset: 0x0003FA40
		public void Notify_BuildingSpawned(Building building)
		{
			this.dirty = true;
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00041840 File Offset: 0x0003FA40
		public void Notify_BuildingDespawned(Building building)
		{
			this.dirty = true;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00041849 File Offset: 0x0003FA49
		private void RebuildIfDirty()
		{
			if (this.dirty)
			{
				this.ForceRebuildPens();
			}
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0004185C File Offset: 0x0003FA5C
		private void ForceRebuildPens()
		{
			this.dirty = false;
			this.penMarkers.Clear();
			foreach (Building thing in this.map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker = thing.TryGetComp<CompAnimalPenMarker>();
				this.penMarkers.Add(compAnimalPenMarker, new PenMarkerState(compAnimalPenMarker));
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x000418DC File Offset: 0x0003FADC
		public string MakeNewAnimalPenName()
		{
			this.existingPenNames.Clear();
			this.existingPenNames.AddRange(from marker in this.penMarkers.Keys
			select marker.label);
			int num = 1;
			string text;
			for (;;)
			{
				text = "AnimalPenMarkerDefaultLabel".Translate(num);
				if (!this.existingPenNames.Contains(text))
				{
					break;
				}
				num++;
			}
			this.existingPenNames.Clear();
			return text;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00041968 File Offset: 0x0003FB68
		public void DrawPlacingMouseAttachments(BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			if (((thingDef != null) ? thingDef.CompDefFor<CompAnimalPenMarker>() : null) == null)
			{
				return;
			}
			IntVec3 intVec = UI.MouseCell();
			if (!intVec.InBounds(this.map))
			{
				return;
			}
			if (this.cached_placingPenFoodCalculator_forPosition == null || intVec != this.cached_placingPenFoodCalculator_forPosition)
			{
				this.cached_placingPenFoodCalculator.ResetAndProcessPen(intVec, this.map, true);
				this.cached_placingPenFoodCalculator_forPosition = new IntVec3?(intVec);
			}
			AnimalPenGUI.DrawPlacingMouseAttachments(intVec, this.map, this.cached_placingPenFoodCalculator);
		}

		// Token: 0x04000AF6 RID: 2806
		private readonly Map map;

		// Token: 0x04000AF7 RID: 2807
		private Dictionary<CompAnimalPenMarker, PenMarkerState> penMarkers = new Dictionary<CompAnimalPenMarker, PenMarkerState>();

		// Token: 0x04000AF8 RID: 2808
		private bool dirty = true;

		// Token: 0x04000AF9 RID: 2809
		private ThingFilter cachedDefaultAutoCutFilter;

		// Token: 0x04000AFA RID: 2810
		private ThingFilter cachedFixedAutoCutFilter;

		// Token: 0x04000AFB RID: 2811
		private HashSet<string> existingPenNames = new HashSet<string>();

		// Token: 0x04000AFC RID: 2812
		private readonly PenFoodCalculator cached_placingPenFoodCalculator = new PenFoodCalculator();

		// Token: 0x04000AFD RID: 2813
		private IntVec3? cached_placingPenFoodCalculator_forPosition;
	}
}

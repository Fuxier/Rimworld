using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200045B RID: 1115
	public static class DebugThingPlaceHelper
	{
		// Token: 0x0600224B RID: 8779 RVA: 0x000DA8B4 File Offset: 0x000D8AB4
		public static bool IsDebugSpawnable(ThingDef def, bool allowPlayerBuildable = false)
		{
			return def.forceDebugSpawnable || (!(def.thingClass == typeof(Corpse)) && !def.IsBlueprint && !def.IsFrame && def != ThingDefOf.ActiveDropPod && !(def.thingClass == typeof(MinifiedThing)) && !(def.thingClass == typeof(MinifiedTree)) && !(def.thingClass == typeof(UnfinishedThing)) && !def.thingClass.IsSubclassOf(typeof(SignalAction)) && !def.destroyOnDrop && (def.category == ThingCategory.Filth || def.category == ThingCategory.Item || def.category == ThingCategory.Plant || def.category == ThingCategory.Ethereal || (def.category == ThingCategory.Building && def.building.isNaturalRock) || (def.category == ThingCategory.Building && !def.BuildableByPlayer) || (def.category == ThingCategory.Building && def.BuildableByPlayer && allowPlayerBuildable)));
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x000DA9CC File Offset: 0x000D8BCC
		public static void DebugSpawn(ThingDef def, IntVec3 c, int stackCount = -1, bool direct = false, ThingStyleDef thingStyleDef = null, bool canBeMinified = true, WipeMode? wipeMode = null)
		{
			if (stackCount <= 0)
			{
				stackCount = def.stackLimit;
			}
			ThingDef stuff = GenStuff.RandomStuffFor(def);
			Thing thing = ThingMaker.MakeThing(def, stuff);
			if (thingStyleDef != null)
			{
				thing.StyleDef = thingStyleDef;
			}
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(QualityUtility.GenerateQualityRandomEqualChance(), ArtGenerationContext.Colony);
			}
			if (thing.def.Minifiable && canBeMinified)
			{
				thing = thing.MakeMinified();
			}
			if (thing.def.CanHaveFaction)
			{
				if (thing.def.building != null && thing.def.building.isInsectCocoon)
				{
					thing.SetFaction(Faction.OfInsects, null);
				}
				else
				{
					thing.SetFaction(Faction.OfPlayerSilentFail, null);
				}
			}
			thing.stackCount = stackCount;
			if (wipeMode != null)
			{
				GenSpawn.Spawn(def, c, Find.CurrentMap, wipeMode.Value);
			}
			else
			{
				GenPlace.TryPlaceThing(thing, c, Find.CurrentMap, direct ? ThingPlaceMode.Direct : ThingPlaceMode.Near, null, null, default(Rot4));
			}
			thing.Notify_DebugSpawned();
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x000DAAC0 File Offset: 0x000D8CC0
		public static List<DebugActionNode> TryPlaceOptionsForStackCount(int stackCount, bool direct)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef3 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, false) && (stackCount < 0 || def.stackLimit >= stackCount)
			select def)
			{
				ThingDef localDef = localDef3;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct, null, false, null);
				}, null));
			}
			if (stackCount == 1)
			{
				foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
				where def.Minifiable
				select def)
				{
					ThingDef localDef = localDef2;
					list.Add(new DebugActionNode(localDef.defName + " (minified)", DebugActionType.ToolMap, delegate()
					{
						DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct, null, true, null);
					}, null));
				}
			}
			return list;
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x000DAC1C File Offset: 0x000D8E1C
		public static List<DebugActionNode> TryPlaceOptionsUnminified()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where def.Minifiable
			select def)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), 1, false, null, false, null);
				}, null));
			}
			return list;
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x000DACC0 File Offset: 0x000D8EC0
		public static List<DebugActionNode> TryPlaceOptionsForBaseMarketValue(float marketValue, bool direct)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, false) && def.stackLimit > 1
			select def)
			{
				ThingDef localDef = localDef2;
				int stackCount = (int)(marketValue / localDef.BaseMarketValue);
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct, null, true, null);
				}, null));
			}
			return list;
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x000DAD90 File Offset: 0x000D8F90
		public static List<DebugActionNode> SpawnOptions(WipeMode wipeMode)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, true)
			select def)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), 1, true, null, true, new WipeMode?(wipeMode));
				}, null));
			}
			return list;
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000DAE4C File Offset: 0x000D904C
		public static List<DebugActionNode> TryAbandonOptionsForStackCount()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, false)
			select def)
			{
				ThingDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode();
				debugActionNode.label = localDef.defName;
				debugActionNode.actionType = DebugActionType.Action;
				debugActionNode.AddChild(new DebugActionNode(localDef.defName + " x1", DebugActionType.ToolWorld, delegate()
				{
					DebugThingPlaceHelper.<TryAbandonOptionsForStackCount>g__DebugAbandon|6_0(localDef, GenWorld.MouseTile(false), 1);
				}, null));
				debugActionNode.AddChild(new DebugActionNode(localDef.defName + " full stack", DebugActionType.ToolWorld, delegate()
				{
					DebugThingPlaceHelper.<TryAbandonOptionsForStackCount>g__DebugAbandon|6_0(localDef, GenWorld.MouseTile(false), localDef.stackLimit);
				}, null));
				for (int i = 50; i <= 1000; i += 50)
				{
					int localCount = i;
					debugActionNode.AddChild(new DebugActionNode(localDef.defName + " x" + i, DebugActionType.ToolWorld, delegate()
					{
						DebugThingPlaceHelper.<TryAbandonOptionsForStackCount>g__DebugAbandon|6_0(localDef, GenWorld.MouseTile(false), localCount);
					}, null));
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000DAFD0 File Offset: 0x000D91D0
		[CompilerGenerated]
		internal static void <TryAbandonOptionsForStackCount>g__DebugAbandon|6_0(ThingDef def, int tile, int count)
		{
			while (count > 0)
			{
				Thing thing = ThingMaker.MakeThing(def, null);
				thing.stackCount = Mathf.Max(1, Mathf.Min(count, def.stackLimit));
				count -= thing.stackCount;
				thing.Notify_AbandonedAtTile(tile);
				thing.Destroy(DestroyMode.Vanish);
			}
		}
	}
}

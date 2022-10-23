using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200008C RID: 140
	public static class CameraJumper
	{
		// Token: 0x06000500 RID: 1280 RVA: 0x0001B777 File Offset: 0x00019977
		public static void TryJumpAndSelect(GlobalTargetInfo target, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			if (!target.IsValid)
			{
				return;
			}
			CameraJumper.TryJump(target, mode);
			CameraJumper.TrySelect(target);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001B790 File Offset: 0x00019990
		public static void TrySelect(GlobalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return;
			}
			target = CameraJumper.GetAdjustedTarget(target);
			if (target.HasThing)
			{
				CameraJumper.TrySelectInternal(target.Thing);
				return;
			}
			if (target.HasWorldObject)
			{
				CameraJumper.TrySelectInternal(target.WorldObject);
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001B7D0 File Offset: 0x000199D0
		private static void TrySelectInternal(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (thing.Spawned && thing.def.selectable)
			{
				bool flag = CameraJumper.TryHideWorld();
				bool flag2 = false;
				if (thing.Map != Find.CurrentMap)
				{
					Current.Game.CurrentMap = thing.Map;
					flag2 = true;
					if (!flag)
					{
						SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					}
				}
				if (flag || flag2)
				{
					Find.CameraDriver.JumpToCurrentMapLoc(thing.Position);
				}
				Find.Selector.ClearSelection();
				Find.Selector.Select(thing, true, true);
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001B85C File Offset: 0x00019A5C
		private static void TrySelectInternal(WorldObject worldObject)
		{
			if (Find.World == null)
			{
				return;
			}
			if (worldObject.Spawned && worldObject.SelectableNow)
			{
				CameraJumper.TryShowWorld();
				Find.WorldSelector.ClearSelection();
				Find.WorldSelector.Select(worldObject, true);
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001B894 File Offset: 0x00019A94
		public static void TryJump(GlobalTargetInfo target, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			if (!target.IsValid)
			{
				return;
			}
			target = CameraJumper.GetAdjustedTarget(target);
			if (target.HasThing)
			{
				CameraJumper.TryJumpInternal(target.Thing, mode);
				return;
			}
			if (target.HasWorldObject)
			{
				CameraJumper.TryJumpInternal(target.WorldObject);
				return;
			}
			if (target.Cell.IsValid)
			{
				CameraJumper.TryJumpInternal(target.Cell, target.Map, mode);
				return;
			}
			CameraJumper.TryJumpInternal(target.Tile);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001B912 File Offset: 0x00019B12
		public static void TryJump(IntVec3 cell, Map map, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(cell, map, false), mode);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001B922 File Offset: 0x00019B22
		public static void TryJump(int tile, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(tile), mode);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001B930 File Offset: 0x00019B30
		private static void TryJumpInternal(Thing thing, CameraJumper.MovementMode mode)
		{
			CameraJumper.TryJumpInternal(thing.PositionHeld, thing.MapHeld, mode);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001B944 File Offset: 0x00019B44
		private static void TryJumpInternal(IntVec3 cell, Map map, CameraJumper.MovementMode mode)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (!cell.IsValid)
			{
				return;
			}
			if (map == null || !Find.Maps.Contains(map))
			{
				return;
			}
			if (!cell.InBounds(map))
			{
				return;
			}
			bool flag = CameraJumper.TryHideWorld();
			bool flag2 = false;
			if (Find.CurrentMap != map)
			{
				Current.Game.CurrentMap = map;
				flag2 = true;
				if (!flag)
				{
					SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
				}
			}
			CameraJumper.JumpLocalInternal(cell, (!Prefs.SmoothCameraJumps || flag || flag2) ? CameraJumper.MovementMode.Cut : mode);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001B9C2 File Offset: 0x00019BC2
		private static void TryJumpInternal(WorldObject worldObject)
		{
			CameraJumper.TryJumpInternal(worldObject.Tile);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001B9CF File Offset: 0x00019BCF
		private static void TryJumpInternal(int tile)
		{
			if (Find.World == null)
			{
				return;
			}
			if (tile < 0)
			{
				return;
			}
			CameraJumper.TryShowWorld();
			Find.WorldCameraDriver.JumpTo(tile);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001B9EF File Offset: 0x00019BEF
		private static void JumpLocalInternal(IntVec3 localCell, CameraJumper.MovementMode mode)
		{
			if (mode != CameraJumper.MovementMode.Pan)
			{
				if (mode != CameraJumper.MovementMode.Cut)
				{
				}
				Find.CameraDriver.JumpToCurrentMapLoc(localCell);
				return;
			}
			Find.CameraDriver.PanToMapLoc(localCell);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001BA14 File Offset: 0x00019C14
		public static bool CanJump(GlobalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return false;
			}
			target = CameraJumper.GetAdjustedTarget(target);
			if (target.HasThing)
			{
				return target.Thing.MapHeld != null && Find.Maps.Contains(target.Thing.MapHeld) && target.Thing.PositionHeld.IsValid && target.Thing.PositionHeld.InBounds(target.Thing.MapHeld);
			}
			if (target.HasWorldObject)
			{
				return target.WorldObject.Spawned;
			}
			if (target.Cell.IsValid)
			{
				return target.Map != null && Find.Maps.Contains(target.Map) && target.Cell.IsValid && target.Cell.InBounds(target.Map);
			}
			return target.Tile >= 0;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001BB10 File Offset: 0x00019D10
		public static GlobalTargetInfo GetAdjustedTarget(GlobalTargetInfo target)
		{
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing.Spawned)
				{
					return thing;
				}
				GlobalTargetInfo result = GlobalTargetInfo.Invalid;
				for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
				{
					Thing thing2 = parentHolder as Thing;
					if (thing2 != null && thing2.Spawned)
					{
						result = thing2;
						break;
					}
					ThingComp thingComp = parentHolder as ThingComp;
					if (thingComp != null && thingComp.parent.Spawned)
					{
						result = thingComp.parent;
						break;
					}
					WorldObject worldObject = parentHolder as WorldObject;
					if (worldObject != null && worldObject.Spawned)
					{
						result = worldObject;
						break;
					}
				}
				if (result.IsValid)
				{
					return result;
				}
				if (target.Thing.TryGetComp<CompCauseGameCondition>() != null)
				{
					List<Site> sites = Find.WorldObjects.Sites;
					for (int i = 0; i < sites.Count; i++)
					{
						for (int j = 0; j < sites[i].parts.Count; j++)
						{
							if (sites[i].parts[j].conditionCauser == target.Thing)
							{
								return sites[i];
							}
						}
					}
				}
				if (thing.Tile >= 0)
				{
					return new GlobalTargetInfo(thing.Tile);
				}
			}
			else if (target.Cell.IsValid && target.Tile >= 0 && target.Map != null && !Find.Maps.Contains(target.Map))
			{
				MapParent parent = target.Map.Parent;
				if (parent != null && parent.Spawned)
				{
					return parent;
				}
				return GlobalTargetInfo.Invalid;
			}
			return target;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001BCC4 File Offset: 0x00019EC4
		public static GlobalTargetInfo GetWorldTarget(GlobalTargetInfo target)
		{
			GlobalTargetInfo adjustedTarget = CameraJumper.GetAdjustedTarget(target);
			if (!adjustedTarget.IsValid)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (adjustedTarget.IsWorldTarget)
			{
				return adjustedTarget;
			}
			return CameraJumper.GetWorldTargetOfMap(adjustedTarget.Map);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001BCFE File Offset: 0x00019EFE
		public static GlobalTargetInfo GetWorldTargetOfMap(Map map)
		{
			if (map == null)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (map.Parent != null && map.Parent.Spawned)
			{
				return map.Parent;
			}
			return GlobalTargetInfo.Invalid;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001BD30 File Offset: 0x00019F30
		public static bool TryHideWorld()
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				return false;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (Find.World.renderer.wantedMode != WorldRenderMode.None)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				return true;
			}
			return false;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001BD80 File Offset: 0x00019F80
		public static bool TryShowWorld()
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				return true;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (Find.World.renderer.wantedMode == WorldRenderMode.None)
			{
				AmbientSoundManager.EnsureWorldAmbientSoundCreated();
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				return true;
			}
			return false;
		}

		// Token: 0x02001CA5 RID: 7333
		public enum MovementMode
		{
			// Token: 0x040070D7 RID: 28887
			Pan,
			// Token: 0x040070D8 RID: 28888
			Cut
		}
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000429 RID: 1065
	[StaticConstructorOnStartup]
	public static class DesignatorUtility
	{
		// Token: 0x06001F75 RID: 8053 RVA: 0x000BB068 File Offset: 0x000B9268
		public static Designator FindAllowedDesignator<T>() where T : Designator
		{
			List<DesignationCategoryDef> allDefsListForReading = DefDatabase<DesignationCategoryDef>.AllDefsListForReading;
			GameRules rules = Current.Game.Rules;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<Designator> allResolvedDesignators = allDefsListForReading[i].AllResolvedDesignators;
				for (int j = 0; j < allResolvedDesignators.Count; j++)
				{
					if (rules == null || rules.DesignatorAllowed(allResolvedDesignators[j]))
					{
						T t = allResolvedDesignators[j] as T;
						if (t != null)
						{
							return t;
						}
					}
				}
			}
			Designator designator = DesignatorUtility.StandaloneDesignators.TryGetValue(typeof(T), null);
			if (designator == null)
			{
				designator = (Activator.CreateInstance(typeof(T)) as Designator);
				DesignatorUtility.StandaloneDesignators[typeof(T)] = designator;
			}
			return designator;
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x000BB13C File Offset: 0x000B933C
		public static void RenderHighlightOverSelectableCells(Designator designator, List<IntVec3> dragCells)
		{
			foreach (IntVec3 intVec in dragCells)
			{
				Vector3 position = intVec.ToVector3Shifted();
				position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, DesignatorUtility.DragHighlightCellMat, 0);
			}
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x000BB1B0 File Offset: 0x000B93B0
		public static void RenderHighlightOverSelectableThings(Designator designator, List<IntVec3> dragCells)
		{
			DesignatorUtility.selectedThings.Clear();
			foreach (IntVec3 c in dragCells)
			{
				List<Thing> thingList = c.GetThingList(designator.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (designator.CanDesignateThing(thingList[i]).Accepted && !DesignatorUtility.selectedThings.Contains(thingList[i]))
					{
						DesignatorUtility.selectedThings.Add(thingList[i]);
						Vector3 drawPos = thingList[i].DrawPos;
						drawPos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
						Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, DesignatorUtility.DragHighlightThingMat, 0);
					}
				}
			}
			DesignatorUtility.selectedThings.Clear();
		}

		// Token: 0x04001557 RID: 5463
		public static readonly Material DragHighlightCellMat = MaterialPool.MatFrom("UI/Overlays/DragHighlightCell", ShaderDatabase.MetaOverlay);

		// Token: 0x04001558 RID: 5464
		public static readonly Material DragHighlightThingMat = MaterialPool.MatFrom("UI/Overlays/DragHighlightThing", ShaderDatabase.MetaOverlay);

		// Token: 0x04001559 RID: 5465
		private static Dictionary<Type, Designator> StandaloneDesignators = new Dictionary<Type, Designator>();

		// Token: 0x0400155A RID: 5466
		private static HashSet<Thing> selectedThings = new HashSet<Thing>();
	}
}

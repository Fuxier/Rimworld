using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001A8 RID: 424
	public static class AnimalPenGUI
	{
		// Token: 0x06000BC9 RID: 3017 RVA: 0x00042500 File Offset: 0x00040700
		public static void DoAllowedAreaMessage(Rect rect, Pawn pawn)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
			TaggedString taggedString;
			TaggedString str;
			if (currentPenOf != null)
			{
				taggedString = "InPen".Translate() + ": " + currentPenOf.label;
				str = taggedString;
			}
			else
			{
				GUI.color = Color.gray;
				taggedString = "(" + "Unpenned".Translate() + ")";
				str = "UnpennedTooltip".Translate();
			}
			Widgets.Label(rect, taggedString);
			TooltipHandler.TipRegion(rect, str);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000425A8 File Offset: 0x000407A8
		public static void DrawPlacingMouseAttachments(IntVec3 mouseCell, Map map, PenFoodCalculator calc)
		{
			AnimalPenGUI.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.calc = calc;
			CS$<>8__locals1.sb = new StringBuilder();
			Vector2 location = Find.WorldGrid.LongLatOf(map.Tile);
			CS$<>8__locals1.summerQuadrum = CS$<>8__locals1.calc.GetSummerOrBestQuadrum();
			CS$<>8__locals1.summerLabel = CS$<>8__locals1.summerQuadrum.GetSeason(location);
			CS$<>8__locals1.sb.AppendLine(CS$<>8__locals1.calc.PenSizeDescription());
			CS$<>8__locals1.sb.AppendLine("PenExampleAnimals".Translate() + ":");
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Cow, ref CS$<>8__locals1);
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Goat, ref CS$<>8__locals1);
			AnimalPenGUI.<DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDefOf.Chicken, ref CS$<>8__locals1);
			Widgets.MouseAttachedLabel(CS$<>8__locals1.sb.ToString().TrimEnd(Array.Empty<char>()), 8f, 35f);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00042684 File Offset: 0x00040884
		[CompilerGenerated]
		internal static void <DrawPlacingMouseAttachments>g__AppendCapacityOf|3_0(ThingDef animalDef, ref AnimalPenGUI.<>c__DisplayClass3_0 A_1)
		{
			A_1.sb.Append("PenCapacityDesc".Translate(animalDef.Named("ANIMALDEF")).CapitalizeFirst());
			A_1.sb.Append(" (").Append(A_1.summerLabel.Label()).Append("): ");
			if (A_1.calc.Unenclosed)
			{
				A_1.sb.Append("?");
			}
			else
			{
				A_1.sb.Append(A_1.calc.CapacityOf(A_1.summerQuadrum, animalDef).ToString("F1"));
			}
			A_1.sb.AppendLine();
		}

		// Token: 0x02001D3D RID: 7485
		public class PenPainter : AnimalPenEnclosureCalculator
		{
			// Token: 0x0600B3B9 RID: 46009 RVA: 0x0040EC98 File Offset: 0x0040CE98
			protected override void VisitDirectlyConnectedRegion(Region r)
			{
				this.directEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x0600B3BA RID: 46010 RVA: 0x0040ECAB File Offset: 0x0040CEAB
			protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
			{
				this.indirectEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x0600B3BB RID: 46011 RVA: 0x0040ECBE File Offset: 0x0040CEBE
			protected override void VisitPassableDoorway(Region r)
			{
				this.openDoorEdgeCells.AddRange(r.Cells);
			}

			// Token: 0x0600B3BC RID: 46012 RVA: 0x0040ECD4 File Offset: 0x0040CED4
			public void Paint(IntVec3 position, Map map)
			{
				this.directEdgeCells.Clear();
				this.indirectEdgeCells.Clear();
				this.openDoorEdgeCells.Clear();
				if (base.VisitPen(position, map))
				{
					GenDraw.DrawFieldEdges(this.directEdgeCells, Color.green, null);
					GenDraw.DrawFieldEdges(this.indirectEdgeCells, Color.white, null);
					GenDraw.DrawFieldEdges(this.openDoorEdgeCells, Color.white, null);
					return;
				}
				GenDraw.DrawFieldEdges(this.openDoorEdgeCells, Color.red, null);
			}

			// Token: 0x04007389 RID: 29577
			private readonly List<IntVec3> directEdgeCells = new List<IntVec3>();

			// Token: 0x0400738A RID: 29578
			private readonly List<IntVec3> indirectEdgeCells = new List<IntVec3>();

			// Token: 0x0400738B RID: 29579
			private readonly List<IntVec3> openDoorEdgeCells = new List<IntVec3>();
		}

		// Token: 0x02001D3E RID: 7486
		public class PenBlueprintPainter
		{
			// Token: 0x0600B3BE RID: 46014 RVA: 0x0040ED9C File Offset: 0x0040CF9C
			public void Paint(IntVec3 position, Map map)
			{
				this.filler.VisitPen(position, map);
				if (this.filler.isEnclosed)
				{
					GenDraw.DrawFieldEdges(this.filler.cellsFound, Color.white, null);
				}
			}

			// Token: 0x0400738C RID: 29580
			private AnimalPenBlueprintEnclosureCalculator filler = new AnimalPenBlueprintEnclosureCalculator();
		}
	}
}

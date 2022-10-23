using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020004BF RID: 1215
	public class MouseoverReadout
	{
		// Token: 0x060024BF RID: 9407 RVA: 0x000E9FB0 File Offset: 0x000E81B0
		public void MouseoverReadoutOnGUI()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (Find.MainTabsRoot.OpenTab != null)
			{
				return;
			}
			GenUI.DrawTextWinterShadow(new Rect(256f, (float)(UI.screenHeight - 256), -256f, 256f));
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.8f);
			IntVec3 intVec = UI.MouseCell();
			if (!intVec.InBounds(Find.CurrentMap))
			{
				return;
			}
			float num = 0f;
			if (intVec.Fogged(Find.CurrentMap))
			{
				Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f), "Undiscovered".Translate());
				GUI.color = Color.white;
				return;
			}
			Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f), MouseoverUtility.GetGlowLabelByValue(Find.CurrentMap.glowGrid.GameGlowAt(intVec, false)));
			num += 19f;
			Rect rect = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
			TerrainDef terrain = intVec.GetTerrain(Find.CurrentMap);
			bool flag = intVec.IsPolluted(Find.CurrentMap);
			if (terrain != this.cachedTerrain || flag != this.cachedPolluted)
			{
				float fertility = intVec.GetFertility(Find.CurrentMap);
				string t = ((double)fertility > 0.0001) ? (" " + "FertShort".TranslateSimple() + " " + fertility.ToStringPercent()) : "";
				string t2 = flag ? "PollutedTerrain".Translate(terrain.label).CapitalizeFirst() : terrain.LabelCap;
				this.cachedTerrainString = t2 + ((terrain.passability != Traversability.Impassable) ? (" (" + "WalkSpeed".Translate(GenPath.SpeedPercentString((float)terrain.pathCost)) + t + ")") : null);
				this.cachedTerrain = terrain;
				this.cachedPolluted = flag;
			}
			Widgets.Label(rect, this.cachedTerrainString);
			num += 19f;
			Zone zone = intVec.GetZone(Find.CurrentMap);
			if (zone != null)
			{
				Rect rect2 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
				string label = zone.label;
				Widgets.Label(rect2, label);
				num += 19f;
			}
			float depth = Find.CurrentMap.snowGrid.GetDepth(intVec);
			if (depth > 0.03f)
			{
				Rect rect3 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
				SnowCategory snowCategory = SnowUtility.GetSnowCategory(depth);
				string label2 = "Snow".Translate() + "(" + SnowUtility.GetDescription(snowCategory) + ")" + " (" + "WalkSpeed".Translate(GenPath.SpeedPercentString((float)SnowUtility.MovementTicksAddOn(snowCategory))) + ")";
				Widgets.Label(rect3, label2);
				num += 19f;
			}
			List<Thing> thingList = intVec.GetThingList(Find.CurrentMap);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def.category != ThingCategory.Mote)
				{
					Rect rect4 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
					string labelMouseover = thing.LabelMouseover;
					Widgets.Label(rect4, labelMouseover);
					num += 19f;
				}
			}
			RoofDef roof = intVec.GetRoof(Find.CurrentMap);
			if (roof != null)
			{
				Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f), roof.LabelCap);
				num += 19f;
			}
			if (Find.CurrentMap.gasGrid.AnyGasAt(intVec))
			{
				this.DrawGas(GasType.BlindSmoke, Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.BlindSmoke), ref num);
				this.DrawGas(GasType.ToxGas, Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.ToxGas), ref num);
				this.DrawGas(GasType.RotStink, Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.RotStink), ref num);
			}
			GUI.color = Color.white;
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000EA488 File Offset: 0x000E8688
		private void DrawGas(GasType gasType, byte density, ref float curYOffset)
		{
			if (density > 0)
			{
				Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - curYOffset, 999f, 999f), gasType.GetLabel().CapitalizeFirst() + " " + ((float)density / 255f).ToStringPercent("F0"));
				curYOffset += 19f;
			}
		}

		// Token: 0x0400178D RID: 6029
		private TerrainDef cachedTerrain;

		// Token: 0x0400178E RID: 6030
		private bool cachedPolluted;

		// Token: 0x0400178F RID: 6031
		private string cachedTerrainString;

		// Token: 0x04001790 RID: 6032
		private const float YInterval = 19f;

		// Token: 0x04001791 RID: 6033
		private static readonly Vector2 BotLeft = new Vector2(15f, 65f);
	}
}

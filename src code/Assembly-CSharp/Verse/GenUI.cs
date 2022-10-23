using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D6 RID: 1238
	[StaticConstructorOnStartup]
	public static class GenUI
	{
		// Token: 0x06002550 RID: 9552 RVA: 0x000ECEB4 File Offset: 0x000EB0B4
		public static void SetLabelAlign(TextAnchor a)
		{
			Text.Anchor = a;
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000ECEBC File Offset: 0x000EB0BC
		public static void ResetLabelAlign()
		{
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000ECEC4 File Offset: 0x000EB0C4
		public static float BackgroundDarkAlphaForText()
		{
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			float num = GenCelestial.CurCelestialSunGlow(Find.CurrentMap);
			float num2 = (Find.CurrentMap.Biome == BiomeDefOf.IceSheet) ? 1f : Mathf.Clamp01(Find.CurrentMap.snowGrid.TotalDepth / 1000f);
			return num * num2 * 0.41f;
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000ECF24 File Offset: 0x000EB124
		public static void DrawTextWinterShadow(Rect rect)
		{
			float num = GenUI.BackgroundDarkAlphaForText();
			if (num > 0.001f)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(rect, GenUI.UnderShadowTex);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000ECF70 File Offset: 0x000EB170
		public static void DrawTextureWithMaterial(Rect rect, Texture texture, Material material, Rect texCoords = default(Rect))
		{
			if (texCoords == default(Rect))
			{
				if (material == null)
				{
					GUI.DrawTexture(rect, texture);
					return;
				}
				if (Event.current.type == EventType.Repaint)
				{
					Graphics.DrawTexture(rect, texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, GUI.color.a * 0.5f), material);
					return;
				}
			}
			else
			{
				if (material == null)
				{
					GUI.DrawTextureWithTexCoords(rect, texture, texCoords);
					return;
				}
				if (Event.current.type == EventType.Repaint)
				{
					Graphics.DrawTexture(rect, texture, texCoords, 0, 0, 0, 0, new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, GUI.color.a * 0.5f), material);
				}
			}
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000ED090 File Offset: 0x000EB290
		public static float IconDrawScale(ThingDef tDef)
		{
			float num = tDef.uiIconScale;
			if (tDef.uiIconPath.NullOrEmpty() && tDef.graphicData != null)
			{
				IntVec2 intVec = (!tDef.defaultPlacingRot.IsHorizontal) ? tDef.Size : tDef.Size.Rotated();
				num *= Mathf.Min(tDef.graphicData.drawSize.x / (float)intVec.x, tDef.graphicData.drawSize.y / (float)intVec.z);
			}
			return num;
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000ED118 File Offset: 0x000EB318
		public static void ErrorDialog(string message)
		{
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox(message, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			}
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000ED14C File Offset: 0x000EB34C
		public static void DrawFlash(float centerX, float centerY, float size, float alpha, Color color)
		{
			Rect position = new Rect(centerX - size / 2f, centerY - size / 2f, size, size);
			Color color2 = color;
			color2.a = alpha;
			GUI.color = color2;
			GUI.DrawTexture(position, GenUI.UIFlash);
			GUI.color = Color.white;
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000ED198 File Offset: 0x000EB398
		public static Vector2 GetSizeCached(this string s)
		{
			if (GenUI.labelWidthCache.Count > 2000 || (Time.frameCount % 40000 == 0 && GenUI.labelWidthCache.Count > 100))
			{
				GenUI.labelWidthCache.Clear();
			}
			s = s.StripTags();
			Vector2 vector;
			if (!GenUI.labelWidthCache.TryGetValue(s, out vector))
			{
				vector = Text.CalcSize(s);
				GenUI.labelWidthCache.Add(s, vector);
			}
			return vector;
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000ED206 File Offset: 0x000EB406
		public static float GetWidthCached(this string s)
		{
			return s.GetSizeCached().x;
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000ED213 File Offset: 0x000EB413
		public static float GetHeightCached(this string s)
		{
			return s.GetSizeCached().y;
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000ED220 File Offset: 0x000EB420
		public static void ClearLabelWidthCache()
		{
			GenUI.labelWidthCache.Clear();
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000ED22C File Offset: 0x000EB42C
		public static Rect Rounded(this Rect r)
		{
			return new Rect((float)((int)r.x), (float)((int)r.y), (float)((int)r.width), (float)((int)r.height));
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000ED257 File Offset: 0x000EB457
		public static Rect RoundedCeil(this Rect r)
		{
			return new Rect((float)Mathf.CeilToInt(r.x), (float)Mathf.CeilToInt(r.y), (float)Mathf.CeilToInt(r.width), (float)Mathf.CeilToInt(r.height));
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x000ED292 File Offset: 0x000EB492
		public static Vector2 Rounded(this Vector2 v)
		{
			return new Vector2((float)((int)v.x), (float)((int)v.y));
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000ED2AC File Offset: 0x000EB4AC
		public static float DistFromRect(Rect r, Vector2 p)
		{
			float num = Mathf.Abs(p.x - r.center.x) - r.width / 2f;
			if (num < 0f)
			{
				num = 0f;
			}
			float num2 = Mathf.Abs(p.y - r.center.y) - r.height / 2f;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000ED330 File Offset: 0x000EB530
		public static void DrawMouseAttachment(Texture iconTex, string text = "", float angle = 0f, Vector2 offset = default(Vector2), Rect? customRect = null, bool drawTextBackground = false, Color textBgColor = default(Color), Color? iconColor = null, Action<Rect> postDrawAction = null)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float num = mousePosition.y + 12f;
			if (drawTextBackground && text != "")
			{
				Rect value;
				if (customRect != null)
				{
					value = customRect.Value;
				}
				else
				{
					Vector2 vector = Text.CalcSize(text);
					float num2 = (iconTex != null) ? 42f : 0f;
					value = new Rect(mousePosition.x + 12f - 4f, num + num2, Text.CalcSize(text).x + 8f, vector.y);
				}
				Widgets.DrawBoxSolid(value, textBgColor);
			}
			if (iconTex != null)
			{
				Rect mouseRect;
				if (customRect != null)
				{
					mouseRect = customRect.Value;
				}
				else
				{
					mouseRect = new Rect(mousePosition.x + 8f, num + 8f, 32f, 32f);
				}
				Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
				{
					Rect rect = mouseRect.AtZero();
					rect.position += new Vector2(offset.x * rect.size.x, offset.y * rect.size.y);
					GUI.color = (iconColor ?? Color.white);
					Widgets.DrawTextureRotated(rect, iconTex, angle);
					GUI.color = Color.white;
					Action<Rect> postDrawAction2 = postDrawAction;
					if (postDrawAction2 == null)
					{
						return;
					}
					postDrawAction2(rect);
				}, false, false, 0f, null);
				num += mouseRect.height + 10f;
			}
			if (text != "")
			{
				Rect textRect = new Rect(mousePosition.x + 12f, num, 200f, 9999f);
				Find.WindowStack.ImmediateWindow(34003429, textRect, WindowLayer.Super, delegate
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					Widgets.Label(textRect.AtZero(), text);
					Text.Font = font;
				}, false, false, 0f, null);
			}
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000ED534 File Offset: 0x000EB734
		public static void DrawMouseAttachment(Texture2D icon)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect mouseRect = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
			{
				GUI.DrawTexture(mouseRect.AtZero(), icon);
			}, false, false, 0f, null);
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000ED5B0 File Offset: 0x000EB7B0
		public static void RenderMouseoverBracket()
		{
			Vector3 position = UI.MouseCell().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenUI.MouseoverBracketMaterial, 0);
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000ED5E4 File Offset: 0x000EB7E4
		public static void DrawStatusLevel(Need status, Rect rect)
		{
			Widgets.BeginGroup(rect);
			Widgets.Label(new Rect(0f, 2f, rect.width, 25f), status.LabelCap);
			Rect rect2 = new Rect(100f, 3f, GenUI.PieceBarSize.x, GenUI.PieceBarSize.y);
			Widgets.FillableBar(rect2, status.CurLevelPercentage);
			Widgets.FillableBarChangeArrows(rect2, status.GUIChangeArrow);
			Widgets.EndGroup();
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, status.GetTipString());
			}
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000ED689 File Offset: 0x000EB889
		public static IEnumerable<LocalTargetInfo> TargetsAtMouse(TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			return GenUI.TargetsAt(UI.MouseMapPosition(), clickParams, thingsOnly, source);
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000ED698 File Offset: 0x000EB898
		public static IEnumerable<LocalTargetInfo> TargetsAt(Vector3 clickPos, TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			List<Thing> clickableList = GenUI.ThingsUnderMouse(clickPos, 0.8f, clickParams, source);
			Thing caster = (source != null) ? source.Caster : null;
			int num;
			for (int i = 0; i < clickableList.Count; i = num + 1)
			{
				Pawn pawn = clickableList[i] as Pawn;
				if (pawn == null || !pawn.IsInvisible() || caster == null || caster.Faction == pawn.Faction)
				{
					yield return clickableList[i];
				}
				num = i;
			}
			if (!thingsOnly)
			{
				IntVec3 intVec = UI.MouseCell();
				if (intVec.InBounds(Find.CurrentMap) && clickParams.CanTarget(new TargetInfo(intVec, Find.CurrentMap, false), source))
				{
					yield return intVec;
				}
			}
			yield break;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000ED6C0 File Offset: 0x000EB8C0
		public static List<Thing> ThingsUnderMouse(Vector3 clickPos, float pawnWideClickRadius, TargetingParameters clickParams, ITargetingSource source = null)
		{
			IntVec3 intVec = IntVec3.FromVector3(clickPos);
			List<Thing> list = new List<Thing>();
			List<Pawn> allPawnsSpawned = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((pawn.DrawPos - clickPos).MagnitudeHorizontal() < 0.4f && clickParams.CanTarget(pawn, source))
				{
					list.Add(pawn);
					list.AddRange(ContainingSelectionUtility.SelectableContainedThings(pawn));
				}
			}
			list.Sort(new Comparison<Thing>(GenUI.CompareThingsByDistanceToMousePointer));
			GenUI.cellThings.Clear();
			foreach (Thing thing4 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
			{
				if (!list.Contains(thing4) && clickParams.CanTarget(thing4, source))
				{
					GenUI.cellThings.Add(thing4);
					GenUI.cellThings.AddRange(ContainingSelectionUtility.SelectableContainedThings(thing4));
				}
			}
			IntVec3[] adjacentCells = GenAdj.AdjacentCells;
			for (int j = 0; j < adjacentCells.Length; j++)
			{
				IntVec3 c = adjacentCells[j] + intVec;
				if (c.InBounds(Find.CurrentMap) && c.GetItemCount(Find.CurrentMap) > 1)
				{
					foreach (Thing thing2 in Find.CurrentMap.thingGrid.ThingsAt(c))
					{
						if (thing2.def.category == ThingCategory.Item && (thing2.TrueCenter() - UI.MouseMapPosition()).MagnitudeHorizontalSquared() <= 0.25f && !list.Contains(thing2) && clickParams.CanTarget(thing2, source))
						{
							GenUI.cellThings.Add(thing2);
						}
					}
				}
			}
			List<Thing> list2 = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.WithCustomRectForSelector);
			for (int k = 0; k < list2.Count; k++)
			{
				Thing thing3 = list2[k];
				if (thing3.CustomRectForSelector != null && thing3.CustomRectForSelector.Value.Contains(intVec) && !list.Contains(thing3) && clickParams.CanTarget(thing3, source))
				{
					GenUI.cellThings.Add(thing3);
				}
			}
			GenUI.cellThings.Sort(new Comparison<Thing>(GenUI.CompareThingsByDrawAltitudeOrDistToItem));
			list.AddRange(GenUI.cellThings);
			GenUI.cellThings.Clear();
			List<Pawn> allPawnsSpawned2 = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int l = 0; l < allPawnsSpawned2.Count; l++)
			{
				Pawn pawn2 = allPawnsSpawned2[l];
				if ((pawn2.DrawPos - clickPos).MagnitudeHorizontal() < pawnWideClickRadius && clickParams.CanTarget(pawn2, source))
				{
					GenUI.cellThings.Add(pawn2);
				}
			}
			GenUI.cellThings.Sort(new Comparison<Thing>(GenUI.CompareThingsByDistanceToMousePointer));
			for (int m = 0; m < GenUI.cellThings.Count; m++)
			{
				if (!list.Contains(GenUI.cellThings[m]))
				{
					list.Add(GenUI.cellThings[m]);
					list.AddRange(ContainingSelectionUtility.SelectableContainedThings(GenUI.cellThings[m]));
				}
			}
			list.RemoveAll((Thing thing) => !clickParams.CanTarget(thing, source));
			GenUI.cellThings.Clear();
			return list;
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000EDAB8 File Offset: 0x000EBCB8
		private static int CompareThingsByDistanceToMousePointer(Thing a, Thing b)
		{
			Vector3 b2 = UI.MouseMapPosition();
			float num = (a.DrawPosHeld.Value - b2).MagnitudeHorizontalSquared();
			float num2 = (b.DrawPosHeld.Value - b2).MagnitudeHorizontalSquared();
			if (num < num2)
			{
				return -1;
			}
			if (num == num2)
			{
				return b.Spawned.CompareTo(a.Spawned);
			}
			return 1;
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000EDB20 File Offset: 0x000EBD20
		private static int CompareThingsByDrawAltitudeOrDistToItem(Thing A, Thing B)
		{
			if (A.def.category == ThingCategory.Item && B.def.category == ThingCategory.Item)
			{
				return (A.TrueCenter() - UI.MouseMapPosition()).MagnitudeHorizontalSquared().CompareTo((B.TrueCenter() - UI.MouseMapPosition()).MagnitudeHorizontalSquared());
			}
			Thing spawnedParentOrMe = A.SpawnedParentOrMe;
			Thing spawnedParentOrMe2 = B.SpawnedParentOrMe;
			if (spawnedParentOrMe.def.Altitude != spawnedParentOrMe2.def.Altitude)
			{
				return spawnedParentOrMe2.def.Altitude.CompareTo(spawnedParentOrMe.def.Altitude);
			}
			return B.Spawned.CompareTo(A.Spawned);
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000EDBD5 File Offset: 0x000EBDD5
		public static int CurrentAdjustmentMultiplier()
		{
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent && KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				return 1000;
			}
			if (KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				return 100;
			}
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent)
			{
				return 10;
			}
			return 1;
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x000EDC14 File Offset: 0x000EBE14
		public static Rect GetInnerRect(this Rect rect)
		{
			return rect.ContractedBy(17f);
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000EDC21 File Offset: 0x000EBE21
		public static Rect ExpandedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x - margin, rect.y - margin, rect.width + margin * 2f, rect.height + margin * 2f);
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000EDC58 File Offset: 0x000EBE58
		public static Rect ExpandedBy(this Rect rect, float marginX, float marginY)
		{
			return new Rect(rect.x - marginX, rect.y - marginY, rect.width + marginX * 2f, rect.height + marginY * 2f);
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000EDC8F File Offset: 0x000EBE8F
		public static Rect ContractedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2f, rect.height - margin * 2f);
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000EDCC6 File Offset: 0x000EBEC6
		public static Rect ContractedBy(this Rect rect, float marginX, float marginY)
		{
			return new Rect(rect.x + marginX, rect.y + marginY, rect.width - marginX * 2f, rect.height - marginY * 2f);
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000EDD00 File Offset: 0x000EBF00
		public static Rect ScaledBy(this Rect rect, float scale)
		{
			rect.x -= rect.width * (scale - 1f) / 2f;
			rect.y -= rect.height * (scale - 1f) / 2f;
			rect.width *= scale;
			rect.height *= scale;
			return rect;
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000EDD72 File Offset: 0x000EBF72
		public static Rect CenteredOnXIn(this Rect rect, Rect otherRect)
		{
			return new Rect(otherRect.x + (otherRect.width - rect.width) / 2f, rect.y, rect.width, rect.height);
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000EDDAB File Offset: 0x000EBFAB
		public static Rect CenteredOnYIn(this Rect rect, Rect otherRect)
		{
			return new Rect(rect.x, otherRect.y + (otherRect.height - rect.height) / 2f, rect.width, rect.height);
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000EDDE4 File Offset: 0x000EBFE4
		public static Rect AtZero(this Rect rect)
		{
			return new Rect(0f, 0f, rect.width, rect.height);
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000EDE04 File Offset: 0x000EC004
		public static Rect Union(this Rect a, Rect b)
		{
			return new Rect
			{
				min = Vector2.Min(a.min, b.min),
				max = Vector2.Max(a.max, b.max)
			};
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000EDE4E File Offset: 0x000EC04E
		public static void AbsorbClicksInRect(Rect r)
		{
			if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000EDE79 File Offset: 0x000EC079
		public static Rect LeftHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000EDEA2 File Offset: 0x000EC0A2
		public static Rect LeftPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000EDEC7 File Offset: 0x000EC0C7
		public static Rect LeftPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x, rect.y, width, rect.height);
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000EDEE4 File Offset: 0x000EC0E4
		public static Rect RightHalf(this Rect rect)
		{
			return new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000EDF1B File Offset: 0x000EC11B
		public static Rect RightPart(this Rect rect, float pct)
		{
			return new Rect(rect.x + rect.width * (1f - pct), rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000EDF50 File Offset: 0x000EC150
		public static Rect RightPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x000EDF77 File Offset: 0x000EC177
		public static Rect TopHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height / 2f);
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000EDFA0 File Offset: 0x000EC1A0
		public static Rect TopPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height * pct);
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000EDFC5 File Offset: 0x000EC1C5
		public static Rect TopPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y, rect.width, height);
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x000EDFE2 File Offset: 0x000EC1E2
		public static Rect BottomHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000EE019 File Offset: 0x000EC219
		public static Rect BottomPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y + rect.height * (1f - pct), rect.width, rect.height * pct);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000EE04E File Offset: 0x000EC24E
		public static Rect BottomPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000EE078 File Offset: 0x000EC278
		public static bool SplitHorizontallyWithMargin(this Rect rect, out Rect top, out Rect bottom, out float overflow, float compressibleMargin = 0f, float? topHeight = null, float? bottomHeight = null)
		{
			if (topHeight != null == (bottomHeight != null))
			{
				throw new ArgumentException("Exactly one null height and one non-null height must be provided.");
			}
			overflow = Mathf.Max(0f, (topHeight ?? (bottomHeight ?? 0f)) - rect.height);
			float height = Mathf.Clamp(topHeight ?? (rect.height - bottomHeight.Value - compressibleMargin), 0f, rect.height);
			float num = Mathf.Clamp(bottomHeight ?? (rect.height - topHeight.Value - compressibleMargin), 0f, rect.height);
			top = new Rect(rect.x, rect.y, rect.width, height);
			bottom = new Rect(rect.x, rect.yMax - num, rect.width, num);
			return overflow == 0f;
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000EE1A4 File Offset: 0x000EC3A4
		public static bool SplitVerticallyWithMargin(this Rect rect, out Rect left, out Rect right, out float overflow, float compressibleMargin = 0f, float? leftWidth = null, float? rightWidth = null)
		{
			if (leftWidth != null == (rightWidth != null))
			{
				throw new ArgumentException("Exactly one null width and one non-null width must be provided.");
			}
			overflow = Mathf.Max(0f, (leftWidth ?? (rightWidth ?? 0f)) - rect.width);
			float width = Mathf.Clamp(leftWidth ?? (rect.width - rightWidth.Value - compressibleMargin), 0f, rect.width);
			float num = Mathf.Clamp(rightWidth ?? (rect.width - leftWidth.Value - compressibleMargin), 0f, rect.width);
			left = new Rect(rect.x, rect.y, width, rect.height);
			right = new Rect(rect.xMax - num, rect.y, num, rect.height);
			return overflow == 0f;
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000EE2D0 File Offset: 0x000EC4D0
		public static void SplitHorizontally(this Rect rect, float topHeight, out Rect top, out Rect bottom)
		{
			float num;
			rect.SplitHorizontallyWithMargin(out top, out bottom, out num, 0f, new float?(topHeight), null);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000EE2FC File Offset: 0x000EC4FC
		public static void SplitVertically(this Rect rect, float leftWidth, out Rect left, out Rect right)
		{
			float num;
			rect.SplitVerticallyWithMargin(out left, out right, out num, 0f, new float?(leftWidth), null);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000EE328 File Offset: 0x000EC528
		public static Color LerpColor(List<Pair<float, Color>> colors, float value)
		{
			if (colors.Count == 0)
			{
				return Color.white;
			}
			int i = 0;
			while (i < colors.Count)
			{
				if (value < colors[i].First)
				{
					if (i == 0)
					{
						return colors[i].Second;
					}
					return Color.Lerp(colors[i - 1].Second, colors[i].Second, Mathf.InverseLerp(colors[i - 1].First, colors[i].First, value));
				}
				else
				{
					i++;
				}
			}
			return colors.Last<Pair<float, Color>>().Second;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000EE3D4 File Offset: 0x000EC5D4
		public static Vector2 GetMouseAttachedWindowPos(float width, float height)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float y;
			if (mousePosition.y + 14f + height < (float)UI.screenHeight)
			{
				y = mousePosition.y + 14f;
			}
			else if (mousePosition.y - 5f - height >= 0f)
			{
				y = mousePosition.y - 5f - height;
			}
			else
			{
				y = (float)UI.screenHeight - (14f + height);
			}
			float x;
			if (mousePosition.x + 16f + width < (float)UI.screenWidth)
			{
				x = mousePosition.x + 16f;
			}
			else
			{
				x = mousePosition.x - 4f - width;
			}
			return new Vector2(x, y);
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000EE490 File Offset: 0x000EC690
		public static float GetCenteredButtonPos(int buttonIndex, int buttonsCount, float totalWidth, float buttonWidth, float pad = 10f)
		{
			float num = (float)buttonsCount * buttonWidth + (float)(buttonsCount - 1) * pad;
			return Mathf.Floor((totalWidth - num) / 2f + (float)buttonIndex * (buttonWidth + pad));
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000EE4C0 File Offset: 0x000EC6C0
		public static void DrawArrowPointingAt(Rect rect)
		{
			Vector2 vector = new Vector2((float)UI.screenWidth, (float)UI.screenHeight) / 2f;
			float angle = Mathf.Atan2(rect.center.x - vector.x, vector.y - rect.center.y) * 57.29578f;
			Vector2 vector2 = new Bounds(rect.center, rect.size).ClosestPoint(vector);
			Rect position = new Rect(vector2 + Vector2.left * (float)GenUI.ArrowTex.width * 0.5f, new Vector2((float)GenUI.ArrowTex.width, (float)GenUI.ArrowTex.height));
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			Vector2 center = GUIUtility.GUIToScreenPoint(vector2);
			GUI.matrix = matrix;
			UI.RotateAroundPivot(angle, center);
			GUI.DrawTexture(position, GenUI.ArrowTex);
			GUI.matrix = matrix;
			UnityGUIBugsFixer.Notify_GUIMatrixChanged();
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000EE5D0 File Offset: 0x000EC7D0
		public static void DrawArrowPointingAtWorldspace(Vector3 worldspace, Camera camera)
		{
			Vector3 vector = camera.WorldToScreenPoint(worldspace) / Prefs.UIScale;
			GenUI.DrawArrowPointingAt(new Rect(new Vector2(vector.x, (float)UI.screenHeight - vector.y) + new Vector2(-2f, 2f), new Vector2(4f, 4f)));
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000EE634 File Offset: 0x000EC834
		public static Rect DrawElementStack<T>(Rect rect, float rowHeight, List<T> elements, GenUI.StackElementDrawer<T> drawer, GenUI.StackElementWidthGetter<T> widthGetter, float rowMargin = 4f, float elementMargin = 5f, bool allowOrderOptimization = true)
		{
			GenUI.tmpRects.Clear();
			GenUI.tmpRects2.Clear();
			for (int i = 0; i < elements.Count; i++)
			{
				GenUI.tmpRects.Add(new GenUI.StackedElementRect(new Rect(0f, 0f, widthGetter(elements[i]), rowHeight), i));
			}
			int num = Mathf.FloorToInt(rect.height / rowHeight);
			List<GenUI.StackedElementRect> list = GenUI.tmpRects;
			float num3;
			float num2;
			if (allowOrderOptimization)
			{
				num2 = (num3 = 0f);
				while (num2 < (float)num)
				{
					GenUI.StackedElementRect item = default(GenUI.StackedElementRect);
					int num4 = -1;
					for (int j = 0; j < list.Count; j++)
					{
						GenUI.StackedElementRect stackedElementRect = list[j];
						if (num4 == -1 || (item.rect.width < stackedElementRect.rect.width && stackedElementRect.rect.width < rect.width - num3))
						{
							num4 = j;
							item = stackedElementRect;
						}
					}
					if (num4 == -1)
					{
						if (num3 == 0f)
						{
							break;
						}
						num3 = 0f;
						num2 += 1f;
					}
					else
					{
						num3 += item.rect.width + elementMargin;
						GenUI.tmpRects2.Add(item);
					}
					list.RemoveAt(num4);
					if (list.Count <= 0)
					{
						break;
					}
				}
				list = GenUI.tmpRects2;
			}
			num2 = (num3 = 0f);
			while (list.Count > 0)
			{
				GenUI.StackedElementRect stackedElementRect2 = list[0];
				if (num3 + stackedElementRect2.rect.width > rect.width)
				{
					num3 = 0f;
					num2 += rowHeight + rowMargin;
				}
				drawer(new Rect(rect.x + num3, rect.y + num2, stackedElementRect2.rect.width, stackedElementRect2.rect.height), elements[stackedElementRect2.elementIndex]);
				num3 += stackedElementRect2.rect.width + elementMargin;
				list.RemoveAt(0);
			}
			return new Rect(rect.x, rect.y, rect.width, num2 + rowHeight);
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000EE844 File Offset: 0x000ECA44
		public static Rect DrawElementStackVertical<T>(Rect rect, float rowHeight, List<T> elements, GenUI.StackElementDrawer<T> drawer, GenUI.StackElementWidthGetter<T> widthGetter, float elementMargin = 5f)
		{
			GenUI.tmpRects.Clear();
			for (int i = 0; i < elements.Count; i++)
			{
				GenUI.tmpRects.Add(new GenUI.StackedElementRect(new Rect(0f, 0f, widthGetter(elements[i]), rowHeight), i));
			}
			int elem = Mathf.FloorToInt(rect.height / rowHeight);
			GenUI.spacingCache.Reset(elem);
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			for (int j = 0; j < GenUI.tmpRects.Count; j++)
			{
				GenUI.StackedElementRect stackedElementRect = GenUI.tmpRects[j];
				if (num3 + stackedElementRect.rect.height > rect.height)
				{
					num3 = 0f;
					num = 0;
				}
				drawer(new Rect(rect.x + GenUI.spacingCache.GetSpaceFor(num), rect.y + num3, stackedElementRect.rect.width, stackedElementRect.rect.height), elements[stackedElementRect.elementIndex]);
				num3 += stackedElementRect.rect.height + elementMargin;
				GenUI.spacingCache.AddSpace(num, stackedElementRect.rect.width + elementMargin);
				num2 = Mathf.Max(num2, GenUI.spacingCache.GetSpaceFor(num));
				num++;
			}
			return new Rect(rect.x, rect.y, num2, num3 + rowHeight);
		}

		// Token: 0x040017EB RID: 6123
		public const float Pad = 10f;

		// Token: 0x040017EC RID: 6124
		public const float GapTiny = 4f;

		// Token: 0x040017ED RID: 6125
		public const float GapSmall = 10f;

		// Token: 0x040017EE RID: 6126
		public const float Gap = 17f;

		// Token: 0x040017EF RID: 6127
		public const float GapWide = 26f;

		// Token: 0x040017F0 RID: 6128
		public const float ListSpacing = 28f;

		// Token: 0x040017F1 RID: 6129
		public const float MouseAttachIconSize = 32f;

		// Token: 0x040017F2 RID: 6130
		public const float MouseAttachIconOffset = 8f;

		// Token: 0x040017F3 RID: 6131
		public const float ScrollBarWidth = 16f;

		// Token: 0x040017F4 RID: 6132
		public const float HorizontalSliderHeight = 10f;

		// Token: 0x040017F5 RID: 6133
		public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

		// Token: 0x040017F6 RID: 6134
		public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

		// Token: 0x040017F7 RID: 6135
		public static readonly Color SubtleMouseoverColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x040017F8 RID: 6136
		public static readonly Color FillableBar_Green = new Color(0.40392157f, 0.7019608f, 0.28627452f);

		// Token: 0x040017F9 RID: 6137
		public static readonly Color FillableBar_Empty = new Color(0.03f, 0.035f, 0.05f);

		// Token: 0x040017FA RID: 6138
		public static readonly Vector2 MaxWinSize = new Vector2(1010f, 754f);

		// Token: 0x040017FB RID: 6139
		public const float SmallIconSize = 24f;

		// Token: 0x040017FC RID: 6140
		public const int RootGUIDepth = 50;

		// Token: 0x040017FD RID: 6141
		private const float MouseIconSize = 32f;

		// Token: 0x040017FE RID: 6142
		private const float MouseIconOffset = 12f;

		// Token: 0x040017FF RID: 6143
		private static readonly Material MouseoverBracketMaterial = MaterialPool.MatFrom("UI/Overlays/MouseoverBracketTex", ShaderDatabase.MetaOverlay);

		// Token: 0x04001800 RID: 6144
		private static readonly Texture2D UnderShadowTex = ContentFinder<Texture2D>.Get("UI/Misc/ScreenCornerShadow", true);

		// Token: 0x04001801 RID: 6145
		private static readonly Texture2D UIFlash = ContentFinder<Texture2D>.Get("UI/Misc/Flash", true);

		// Token: 0x04001802 RID: 6146
		private static Dictionary<string, Vector2> labelWidthCache = new Dictionary<string, Vector2>();

		// Token: 0x04001803 RID: 6147
		private static readonly Vector2 PieceBarSize = new Vector2(100f, 17f);

		// Token: 0x04001804 RID: 6148
		public const float PawnDirectClickRadius = 0.4f;

		// Token: 0x04001805 RID: 6149
		private static List<Thing> cellThings = new List<Thing>(32);

		// Token: 0x04001806 RID: 6150
		private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/Arrow", true);

		// Token: 0x04001807 RID: 6151
		private static List<GenUI.StackedElementRect> tmpRects = new List<GenUI.StackedElementRect>();

		// Token: 0x04001808 RID: 6152
		private static List<GenUI.StackedElementRect> tmpRects2 = new List<GenUI.StackedElementRect>();

		// Token: 0x04001809 RID: 6153
		public const float ElementStackDefaultElementMargin = 5f;

		// Token: 0x0400180A RID: 6154
		public const float ElementStackDefaultRowMargin = 4f;

		// Token: 0x0400180B RID: 6155
		private static GenUI.SpacingCache spacingCache;

		// Token: 0x020020C4 RID: 8388
		private struct StackedElementRect
		{
			// Token: 0x0600C516 RID: 50454 RVA: 0x0043B342 File Offset: 0x00439542
			public StackedElementRect(Rect rect, int elementIndex)
			{
				this.rect = rect;
				this.elementIndex = elementIndex;
			}

			// Token: 0x0400821D RID: 33309
			public Rect rect;

			// Token: 0x0400821E RID: 33310
			public int elementIndex;
		}

		// Token: 0x020020C5 RID: 8389
		public class AnonymousStackElement
		{
			// Token: 0x0400821F RID: 33311
			public Action<Rect> drawer;

			// Token: 0x04008220 RID: 33312
			public float width;
		}

		// Token: 0x020020C6 RID: 8390
		private struct SpacingCache
		{
			// Token: 0x0600C518 RID: 50456 RVA: 0x0043B354 File Offset: 0x00439554
			public void Reset(int elem = 16)
			{
				if (this.spaces == null || this.maxElements != elem)
				{
					this.maxElements = elem;
					this.spaces = new float[this.maxElements];
					return;
				}
				for (int i = 0; i < this.maxElements; i++)
				{
					this.spaces[i] = 0f;
				}
			}

			// Token: 0x0600C519 RID: 50457 RVA: 0x0043B3A9 File Offset: 0x004395A9
			public float GetSpaceFor(int elem)
			{
				if (this.spaces == null || this.maxElements < 1)
				{
					this.Reset(16);
				}
				if (elem >= 0 && elem < this.maxElements)
				{
					return this.spaces[elem];
				}
				return 0f;
			}

			// Token: 0x0600C51A RID: 50458 RVA: 0x0043B3DF File Offset: 0x004395DF
			public void AddSpace(int elem, float space)
			{
				if (this.spaces == null || this.maxElements < 1)
				{
					this.Reset(16);
				}
				if (elem >= 0 && elem < this.maxElements)
				{
					this.spaces[elem] += space;
				}
			}

			// Token: 0x04008221 RID: 33313
			private int maxElements;

			// Token: 0x04008222 RID: 33314
			private float[] spaces;
		}

		// Token: 0x020020C7 RID: 8391
		// (Invoke) Token: 0x0600C51C RID: 50460
		public delegate void StackElementDrawer<T>(Rect rect, T element);

		// Token: 0x020020C8 RID: 8392
		// (Invoke) Token: 0x0600C520 RID: 50464
		public delegate float StackElementWidthGetter<T>(T element);
	}
}

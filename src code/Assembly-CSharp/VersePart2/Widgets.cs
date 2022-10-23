using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020004EB RID: 1259
	[StaticConstructorOnStartup]
	public static class Widgets
	{
		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x000F1583 File Offset: 0x000EF783
		public static bool Painting
		{
			get
			{
				return Widgets.dropdownPainting || Widgets.checkboxPainting;
			}
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000F1594 File Offset: 0x000EF794
		static Widgets()
		{
			Color color = new Color(1f, 1f, 1f, 0f);
			Widgets.LineTexAA = new Texture2D(1, 3, TextureFormat.ARGB32, false);
			Widgets.LineTexAA.name = "LineTexAA";
			Widgets.LineTexAA.SetPixel(0, 0, color);
			Widgets.LineTexAA.SetPixel(0, 1, Color.white);
			Widgets.LineTexAA.SetPixel(0, 2, color);
			Widgets.LineTexAA.Apply();
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000F1AB3 File Offset: 0x000EFCB3
		public static void BeginGroup(Rect rect)
		{
			GUI.BeginGroup(rect);
			UnityGUIBugsFixer.Notify_BeginGroup();
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000F1AC0 File Offset: 0x000EFCC0
		public static void EndGroup()
		{
			GUI.EndGroup();
			UnityGUIBugsFixer.Notify_EndGroup();
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000F1ACC File Offset: 0x000EFCCC
		public static void ClearLabelCache()
		{
			Widgets.LabelCache.Clear();
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000F1AD8 File Offset: 0x000EFCD8
		public static bool CanDrawIconFor(Def def)
		{
			BuildableDef buildableDef;
			if ((buildableDef = (def as BuildableDef)) != null)
			{
				return buildableDef.uiIcon != null;
			}
			FactionDef factionDef;
			return (factionDef = (def as FactionDef)) != null && factionDef.FactionIcon != null;
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000F1B14 File Offset: 0x000EFD14
		public static void DefIcon(Rect rect, Def def, ThingDef stuffDef = null, float scale = 1f, ThingStyleDef thingStyleDef = null, bool drawPlaceholder = false, Color? color = null, Material material = null, int? graphicIndexOverride = null)
		{
			BuildableDef buildableDef;
			if ((buildableDef = (def as BuildableDef)) != null)
			{
				rect.position += new Vector2(buildableDef.uiIconOffset.x * rect.size.x, buildableDef.uiIconOffset.y * rect.size.y);
			}
			ThingDef thingDef;
			if ((thingDef = (def as ThingDef)) != null && thingDef.IsFrame && thingDef.entityDefToBuild != null)
			{
				def = thingDef.entityDefToBuild;
			}
			ThingDef thingDef2;
			if ((thingDef2 = (def as ThingDef)) != null)
			{
				Widgets.ThingIcon(rect, thingDef2, stuffDef, thingStyleDef, scale, color, graphicIndexOverride);
				return;
			}
			PawnKindDef pawnKindDef;
			if ((pawnKindDef = (def as PawnKindDef)) != null)
			{
				Widgets.ThingIcon(rect, pawnKindDef.race, stuffDef, thingStyleDef, scale, color, graphicIndexOverride);
				return;
			}
			RecipeDef recipeDef;
			if ((recipeDef = (def as RecipeDef)) != null && recipeDef.UIIconThing != null)
			{
				Widgets.ThingIcon(rect, recipeDef.UIIconThing, null, thingStyleDef, scale, color, graphicIndexOverride);
				return;
			}
			TerrainDef terrainDef;
			if ((terrainDef = (def as TerrainDef)) != null && terrainDef.uiIcon != null)
			{
				GUI.color = terrainDef.uiIconColor;
				Widgets.DrawTextureFitted(rect, terrainDef.uiIcon, scale, Vector2.one, Widgets.CroppedTerrainTextureRect(terrainDef.uiIcon), 0f, material);
				GUI.color = Color.white;
				return;
			}
			FactionDef factionDef;
			if ((factionDef = (def as FactionDef)) != null)
			{
				if (!factionDef.colorSpectrum.NullOrEmpty<Color>())
				{
					GUI.color = factionDef.colorSpectrum.FirstOrDefault<Color>();
				}
				Widgets.DrawTextureFitted(rect, factionDef.FactionIcon, scale, material);
				GUI.color = Color.white;
				return;
			}
			StyleItemDef styleItemDef;
			if ((styleItemDef = (def as StyleItemDef)) != null)
			{
				Widgets.DrawTextureFitted(rect, styleItemDef.Icon, scale, material);
				return;
			}
			GeneDef geneDef;
			if ((geneDef = (def as GeneDef)) != null)
			{
				GUI.color = (color ?? geneDef.IconColor);
				Widgets.DrawTextureFitted(rect, geneDef.Icon, scale, material);
				GUI.color = Color.white;
				return;
			}
			XenotypeDef xenotypeDef;
			if ((xenotypeDef = (def as XenotypeDef)) != null)
			{
				GUI.color = (color ?? XenotypeDef.IconColor);
				Widgets.DrawTextureFitted(rect, xenotypeDef.Icon, scale, material);
				GUI.color = Color.white;
				return;
			}
			if (drawPlaceholder)
			{
				Widgets.DrawTextureFitted(rect, Widgets.PlaceholderIconTex, scale, material);
			}
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000F1D50 File Offset: 0x000EFF50
		public static void ThingIcon(Rect rect, Thing thing, float alpha = 1f, Rot4? rot = null, bool stackOfOne = false)
		{
			thing = thing.GetInnerIfMinified();
			float num;
			float resolvedIconAngle;
			Vector2 vector;
			Color color;
			Texture resolvedIcon = Widgets.GetIconFor(thing, new Vector2(rect.width, rect.height), rot, stackOfOne, out num, out resolvedIconAngle, out vector, out color);
			GUI.color = color;
			ThingStyleDef styleDef = thing.StyleDef;
			if (styleDef != null && styleDef.UIIcon != null)
			{
				rect.position += new Vector2(thing.def.uiIconOffset.x * rect.size.x, thing.def.uiIconOffset.y * rect.size.y);
			}
			else if (!thing.def.uiIconPath.NullOrEmpty())
			{
				rect.position += new Vector2(thing.def.uiIconOffset.x * rect.size.x, thing.def.uiIconOffset.y * rect.size.y);
			}
			else if (thing is Pawn || thing is Corpse)
			{
				Pawn pawn = thing as Pawn;
				if (pawn == null)
				{
					pawn = ((Corpse)thing).InnerPawn;
				}
				if (pawn.RaceProps.Humanlike)
				{
					resolvedIcon = PortraitsCache.Get(pawn, new Vector2(rect.width, rect.height), rot ?? Rot4.South, default(Vector3), 1f, true, true, true, true, null, null, false, null);
				}
			}
			if (alpha != 1f)
			{
				Color color2 = GUI.color;
				color2.a *= alpha;
				GUI.color = color2;
			}
			Widgets.ThingIconWorker(rect, thing.def, resolvedIcon, resolvedIconAngle, 1f);
			GUI.color = Color.white;
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000F1F40 File Offset: 0x000F0140
		public static void ThingIcon(Rect rect, ThingDef thingDef, ThingDef stuffDef = null, ThingStyleDef thingStyleDef = null, float scale = 1f, Color? color = null, int? graphicIndexOverride = null)
		{
			if (thingDef.uiIcon == null || thingDef.uiIcon == BaseContent.BadTex)
			{
				return;
			}
			Texture2D iconFor = Widgets.GetIconFor(thingDef, stuffDef, thingStyleDef, graphicIndexOverride);
			if (color != null)
			{
				GUI.color = color.Value;
			}
			else if (stuffDef != null)
			{
				GUI.color = thingDef.GetColorForStuff(stuffDef);
			}
			else
			{
				GUI.color = (thingDef.MadeFromStuff ? thingDef.GetColorForStuff(GenStuff.DefaultStuffFor(thingDef)) : thingDef.uiIconColor);
			}
			Widgets.ThingIconWorker(rect, thingDef, iconFor, thingDef.uiIconAngle, scale);
			GUI.color = Color.white;
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000F1FDC File Offset: 0x000F01DC
		public static Texture2D GetIconFor(ThingDef thingDef, ThingDef stuffDef = null, ThingStyleDef thingStyleDef = null, int? graphicIndexOverride = null)
		{
			if (thingDef.IsCorpse)
			{
				IngestibleProperties ingestible = thingDef.ingestible;
				if (((ingestible != null) ? ingestible.sourceDef : null) != null)
				{
					thingDef = thingDef.ingestible.sourceDef;
				}
			}
			Texture2D result = thingDef.GetUIIconForStuff(stuffDef);
			Graphic_Appearances graphic_Appearances;
			if (thingStyleDef != null && thingStyleDef.UIIcon != null)
			{
				if (graphicIndexOverride != null)
				{
					result = thingStyleDef.IconForIndex(graphicIndexOverride.Value);
				}
				else
				{
					result = thingStyleDef.UIIcon;
				}
			}
			else if ((graphic_Appearances = (thingDef.graphic as Graphic_Appearances)) != null)
			{
				result = (Texture2D)graphic_Appearances.SubGraphicFor(stuffDef ?? GenStuff.DefaultStuffFor(thingDef)).MatAt(thingDef.defaultPlacingRot, null).mainTexture;
			}
			return result;
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000F2088 File Offset: 0x000F0288
		public static Texture GetIconFor(Thing thing, Vector2 size, Rot4? rot, bool stackOfOne, out float scale, out float angle, out Vector2 iconProportions, out Color color)
		{
			thing = thing.GetInnerIfMinified();
			Corpse corpse;
			if ((corpse = (thing as Corpse)) != null)
			{
				thing = corpse.InnerPawn;
			}
			ThingStyleDef styleDef = thing.StyleDef;
			iconProportions = thing.DrawSize;
			color = thing.DrawColor;
			scale = GenUI.IconDrawScale(thing.def);
			if (rot != null && rot.Value.IsHorizontal)
			{
				iconProportions = new Vector2(iconProportions.y, iconProportions.x);
			}
			angle = 0f;
			Texture result;
			Pawn pawn;
			if (styleDef != null && styleDef.UIIcon != null)
			{
				result = styleDef.UIIcon;
				angle = thing.def.uiIconAngle;
			}
			else if (!thing.def.uiIconPath.NullOrEmpty())
			{
				result = thing.def.uiIcon;
				angle = thing.def.uiIconAngle;
			}
			else if ((pawn = (thing as Pawn)) != null)
			{
				if (!pawn.Drawer.renderer.graphics.AllResolved)
				{
					pawn.Drawer.renderer.graphics.ResolveAllGraphics();
				}
				if (!pawn.RaceProps.Humanlike)
				{
					Material material = pawn.Drawer.renderer.graphics.nakedGraphic.MatAt(Rot4.East, null);
					result = material.mainTexture;
					color = material.color;
				}
				else
				{
					Rect r = new Rect(0f, 0f, size.x, size.y).ScaledBy(1.8f);
					r.y += 3f;
					r = r.Rounded();
					result = PortraitsCache.Get(pawn, new Vector2(r.width, r.height), Rot4.South, default(Vector3), 1.8f, true, true, true, true, null, null, false, null);
				}
			}
			else if (stackOfOne)
			{
				Graphic_StackCount graphic_StackCount = thing.Graphic as Graphic_StackCount;
				if (graphic_StackCount != null)
				{
					result = graphic_StackCount.SubGraphicForStackCount(1, thing.def).MatSingleFor(thing).mainTexture;
				}
				else
				{
					result = thing.Graphic.ExtractInnerGraphicFor(thing, null).MatAt(thing.def.defaultPlacingRot, null).mainTexture;
				}
			}
			else
			{
				result = thing.Graphic.ExtractInnerGraphicFor(thing, null).MatAt(thing.def.defaultPlacingRot, null).mainTexture;
			}
			return result;
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000F231C File Offset: 0x000F051C
		private static void ThingIconWorker(Rect rect, ThingDef thingDef, Texture resolvedIcon, float resolvedIconAngle, float scale = 1f)
		{
			Vector2 texProportions = new Vector2((float)resolvedIcon.width, (float)resolvedIcon.height);
			Rect texCoords = Widgets.DefaultTexCoords;
			if (thingDef.graphicData != null)
			{
				texProportions = thingDef.graphicData.drawSize.RotatedBy(thingDef.defaultPlacingRot);
				if (thingDef.uiIconPath.NullOrEmpty() && thingDef.graphicData.linkFlags != LinkFlags.None)
				{
					texCoords = Widgets.LinkedTexCoords;
				}
			}
			Widgets.DrawTextureFitted(rect, resolvedIcon, GenUI.IconDrawScale(thingDef) * scale, texProportions, texCoords, resolvedIconAngle, null);
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000F2397 File Offset: 0x000F0597
		public static Rect CroppedTerrainTextureRect(Texture2D tex)
		{
			return new Rect(0f, 0f, 64f / (float)tex.width, 64f / (float)tex.height);
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000F23C2 File Offset: 0x000F05C2
		public static void DrawAltRect(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.AltTexture);
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000F23D0 File Offset: 0x000F05D0
		public static void ListSeparator(ref RectDivider divider, string label)
		{
			RectDivider rect = divider.NewRow(25f, VerticalJustification.Top);
			GUI.BeginGroup(rect);
			float num = 0f;
			Widgets.ListSeparator(ref num, rect.Rect.width, label);
			GUI.EndGroup();
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000F2418 File Offset: 0x000F0618
		public static void ListSeparator(ref float curY, float width, string label)
		{
			Color color = GUI.color;
			curY += 3f;
			GUI.color = Widgets.SeparatorLabelColor;
			Rect rect = new Rect(0f, curY, width, 30f);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect, label);
			curY += 20f;
			GUI.color = Widgets.SeparatorLineColor;
			Widgets.DrawLineHorizontal(0f, curY, width);
			curY += 2f;
			GUI.color = color;
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000F248C File Offset: 0x000F068C
		public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
		{
			float num = end.x - start.x;
			float num2 = end.y - start.y;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 < 0.01f)
			{
				return;
			}
			width *= 3f;
			float num4 = width * num2 / num3;
			float num5 = width * num / num3;
			float z = -Mathf.Atan2(-num2, num) * 57.29578f;
			Vector2 vector = start + new Vector2(0.5f * num4, -0.5f * num5);
			Matrix4x4 m = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, z), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			Rect position = new Rect(start.x, start.y - 0.5f * num5, num3, width);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GUI.DrawTexture(position, Widgets.LineTexAA, ScaleMode.StretchToFill, true, 0f, color, 0f, 0f);
			GL.PopMatrix();
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000F259B File Offset: 0x000F079B
		public static void DrawLineHorizontal(float x, float y, float length)
		{
			GUI.DrawTexture(new Rect(x, y, length, 1f), BaseContent.WhiteTex);
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000F25B4 File Offset: 0x000F07B4
		public static void DrawLineVertical(float x, float y, float length)
		{
			GUI.DrawTexture(new Rect(x, y, 1f, length), BaseContent.WhiteTex);
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x000F25CD File Offset: 0x000F07CD
		public static void DrawBoxSolid(Rect rect, Color color)
		{
			Color color2 = GUI.color;
			GUI.color = color;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = color2;
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000F25EA File Offset: 0x000F07EA
		public static void DrawBoxSolidWithOutline(Rect rect, Color solidColor, Color outlineColor, int outlineThickness = 1)
		{
			Widgets.DrawBoxSolid(rect, solidColor);
			Color color = GUI.color;
			GUI.color = outlineColor;
			Widgets.DrawBox(rect, outlineThickness, null);
			GUI.color = color;
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000F260C File Offset: 0x000F080C
		public static void DrawBox(Rect rect, int thickness = 1, Texture2D lineTexture = null)
		{
			Vector2 vector = new Vector2(rect.x, rect.y);
			Vector2 vector2 = new Vector2(rect.x + rect.width, rect.y + rect.height);
			if (vector.x > vector2.x)
			{
				float x = vector.x;
				vector.x = vector2.x;
				vector2.x = x;
			}
			if (vector.y > vector2.y)
			{
				float y = vector.y;
				vector.y = vector2.y;
				vector2.y = y;
			}
			Vector3 vector3 = vector2 - vector;
			GUI.DrawTexture(Widgets.AdjustRectToUIScaling(new Rect(vector.x, vector.y, (float)thickness, vector3.y)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(Widgets.AdjustRectToUIScaling(new Rect(vector2.x - (float)thickness, vector.y, (float)thickness, vector3.y)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(Widgets.AdjustRectToUIScaling(new Rect(vector.x + (float)thickness, vector.y, vector3.x - (float)(thickness * 2), (float)thickness)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(Widgets.AdjustRectToUIScaling(new Rect(vector.x + (float)thickness, vector2.y - (float)thickness, vector3.x - (float)(thickness * 2), (float)thickness)), lineTexture ?? BaseContent.WhiteTex);
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000F2780 File Offset: 0x000F0980
		public static void LabelCacheHeight(ref Rect rect, string label, bool renderLabel = true, bool forceInvalidation = false)
		{
			bool flag = Widgets.LabelCache.ContainsKey(label);
			if (forceInvalidation)
			{
				flag = false;
			}
			float height;
			if (flag)
			{
				height = Widgets.LabelCache[label];
			}
			else
			{
				height = Text.CalcHeight(label, rect.width);
			}
			rect.height = height;
			if (renderLabel)
			{
				Widgets.Label(rect, label);
			}
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000F27D8 File Offset: 0x000F09D8
		public static void Label(Rect rect, GUIContent content)
		{
			GUI.Label(rect, content, Text.CurFontStyle);
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000F27E8 File Offset: 0x000F09E8
		public static void Label(Rect rect, string label)
		{
			Rect position = rect;
			float num = Prefs.UIScale / 2f;
			if (Prefs.UIScale > 1f && Math.Abs(num - Mathf.Floor(num)) > 1E-45f)
			{
				position.xMin = Widgets.AdjustCoordToUIScalingFloor(rect.xMin);
				position.yMin = Widgets.AdjustCoordToUIScalingFloor(rect.yMin);
				position.xMax = Widgets.AdjustCoordToUIScalingCeil(rect.xMax + 1E-05f);
				position.yMax = Widgets.AdjustCoordToUIScalingCeil(rect.yMax + 1E-05f);
			}
			GUI.Label(position, label, Text.CurFontStyle);
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000F2887 File Offset: 0x000F0A87
		public static void Label(Rect rect, TaggedString label)
		{
			Widgets.Label(rect, label.Resolve());
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000F2898 File Offset: 0x000F0A98
		public static void Label(float x, ref float curY, float width, string text, TipSignal tip = default(TipSignal))
		{
			if (text.NullOrEmpty())
			{
				return;
			}
			float num = Text.CalcHeight(text, width);
			Rect rect = new Rect(x, curY, width, num);
			if (!tip.text.NullOrEmpty() || tip.textGetter != null)
			{
				float x2 = Text.CalcSize(text).x;
				Rect rect2 = new Rect(rect.x, rect.y, x2, num);
				Widgets.DrawHighlightIfMouseover(rect2);
				TooltipHandler.TipRegion(rect2, tip);
			}
			Widgets.Label(rect, text);
			curY += num;
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000F2914 File Offset: 0x000F0B14
		public static void LongLabel(float x, float width, string label, ref float curY, bool draw = true)
		{
			if (label.Length < 2500)
			{
				if (draw)
				{
					Widgets.Label(new Rect(x, curY, width, 1000f), label);
				}
				curY += Text.CalcHeight(label, width);
				return;
			}
			int num = 0;
			int num2 = -1;
			bool flag = false;
			for (int i = 0; i < label.Length; i++)
			{
				if (label[i] == '\n')
				{
					num++;
					if (num >= 50)
					{
						string text = label.Substring(num2 + 1, i - num2 - 1);
						num2 = i;
						num = 0;
						if (flag)
						{
							curY += Text.SpaceBetweenLines;
						}
						if (draw)
						{
							Widgets.Label(new Rect(x, curY, width, 10000f), text);
						}
						curY += Text.CalcHeight(text, width);
						flag = true;
					}
				}
			}
			if (num2 != label.Length - 1)
			{
				if (flag)
				{
					curY += Text.SpaceBetweenLines;
				}
				string text2 = label.Substring(num2 + 1);
				if (draw)
				{
					Widgets.Label(new Rect(x, curY, width, 10000f), text2);
				}
				curY += Text.CalcHeight(text2, width);
			}
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000F2A14 File Offset: 0x000F0C14
		public static void LabelScrollable(Rect rect, string label, ref Vector2 scrollbarPosition, bool dontConsumeScrollEventsIfNoScrollbar = false, bool takeScrollbarSpaceEvenIfNoScrollbar = true, bool longLabel = false)
		{
			bool flag = takeScrollbarSpaceEvenIfNoScrollbar || Text.CalcHeight(label, rect.width) > rect.height;
			bool flag2 = flag && (!dontConsumeScrollEventsIfNoScrollbar || Text.CalcHeight(label, rect.width - 16f) > rect.height);
			float num = rect.width;
			if (flag)
			{
				num -= 16f;
			}
			float num2;
			if (longLabel)
			{
				num2 = 0f;
				Widgets.LongLabel(0f, num, label, ref num2, false);
			}
			else
			{
				num2 = Text.CalcHeight(label, num);
			}
			Rect rect2 = new Rect(0f, 0f, num, Mathf.Max(num2 + 5f, rect.height));
			if (flag2)
			{
				Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			}
			else
			{
				Widgets.BeginGroup(rect);
			}
			if (longLabel)
			{
				float y = rect2.y;
				Widgets.LongLabel(rect2.x, rect2.width, label, ref y, true);
			}
			else
			{
				Widgets.Label(rect2, label);
			}
			if (flag2)
			{
				Widgets.EndScrollView();
				return;
			}
			Widgets.EndGroup();
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000F2B10 File Offset: 0x000F0D10
		public static void LabelWithIcon(Rect rect, string label, Texture2D labelIcon, float labelIconScale = 1f)
		{
			Rect outerRect = new Rect(rect.x, rect.y, (float)labelIcon.width, rect.height);
			rect.xMin += (float)labelIcon.width;
			Widgets.DrawTextureFitted(outerRect, labelIcon, labelIconScale);
			Widgets.Label(rect, label);
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x000F2B64 File Offset: 0x000F0D64
		public static void DefLabelWithIcon(Rect rect, Def def, float iconMargin = 2f, float textOffsetX = 6f)
		{
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, def.description);
			Widgets.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.height, rect.height);
			if (iconMargin != 0f)
			{
				rect2 = rect2.ContractedBy(iconMargin);
			}
			Widgets.DefIcon(rect2, def, null, 1f, null, true, null, null, null);
			Rect rect3 = new Rect(rect2.xMax + textOffsetX, 0f, rect.width, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect3, def.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			Widgets.EndGroup();
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x000F2C28 File Offset: 0x000F0E28
		public static bool LabelFit(Rect rect, string label)
		{
			bool result = false;
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			if (Text.CalcSize(label).x <= rect.width)
			{
				Widgets.Label(rect, label);
			}
			else
			{
				Text.Font = GameFont.Tiny;
				if (Text.CalcSize(label).x <= rect.width)
				{
					Widgets.Label(rect, label);
				}
				else
				{
					Widgets.Label(rect, label.Truncate(rect.width, null));
					result = true;
				}
				Text.Font = GameFont.Small;
			}
			Text.Font = font;
			return result;
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x000F2CA4 File Offset: 0x000F0EA4
		public static void HyperlinkWithIcon(Rect rect, Dialog_InfoCard.Hyperlink hyperlink, string text = null, float iconMargin = 2f, float textOffsetX = 6f, Color? color = null, bool truncateLabel = false, string textSuffix = null)
		{
			string text2 = text ?? hyperlink.Label.CapitalizeFirst();
			if (textSuffix != null)
			{
				text2 += textSuffix;
			}
			Widgets.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.height, rect.height);
			if (iconMargin != 0f)
			{
				rect2 = rect2.ContractedBy(iconMargin);
			}
			if (hyperlink.thing != null)
			{
				Widgets.ThingIcon(rect2, hyperlink.thing, 1f, null, false);
			}
			else
			{
				Widgets.DefIcon(rect2, hyperlink.def, null, 1f, null, true, null, null, null);
			}
			float num = rect2.xMax + textOffsetX;
			Rect rect3 = new Rect(rect2.xMax + textOffsetX, 0f, rect.width - num, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.ButtonText(rect3, truncateLabel ? text2.Truncate(rect3.width, null) : text2, false, false, color ?? Widgets.NormalOptionColor, false, null);
			if (Widgets.ButtonInvisible(rect3, true))
			{
				hyperlink.ActivateHyperlink();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			Widgets.EndGroup();
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000F2DF8 File Offset: 0x000F0FF8
		public static void DrawNumberOnMap(Vector2 screenPos, int number, Color textColor)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			string text = number.ToStringCached();
			float val = Text.CalcSize(text).x + 8f;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 15f, Math.Max(40f, val), 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			GUI.color = textColor;
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000F2E84 File Offset: 0x000F1084
		public static void Checkbox(Vector2 topLeft, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Widgets.Checkbox(topLeft.x, topLeft.y, ref checkOn, size, disabled, paintable, texChecked, texUnchecked);
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000F2EA0 File Offset: 0x000F10A0
		public static void Checkbox(float x, float y, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Rect rect = new Rect(x, y, size, size);
			Widgets.CheckboxDraw(x, y, checkOn, disabled, size, texChecked, texUnchecked);
			if (!disabled)
			{
				MouseoverSounds.DoRegion(rect);
				bool flag = false;
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect, false);
				if (draggableResult == Widgets.DraggableResult.Pressed)
				{
					checkOn = !checkOn;
					flag = true;
				}
				else if (draggableResult == Widgets.DraggableResult.Dragged && paintable)
				{
					checkOn = !checkOn;
					flag = true;
					Widgets.checkboxPainting = true;
					Widgets.checkboxPaintingState = checkOn;
				}
				if (paintable && Mouse.IsOver(rect) && Widgets.checkboxPainting && Input.GetMouseButton(0) && checkOn != Widgets.checkboxPaintingState)
				{
					checkOn = Widgets.checkboxPaintingState;
					flag = true;
				}
				if (flag)
				{
					if (checkOn)
					{
						SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
					}
				}
			}
			if (disabled)
			{
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000F2F78 File Offset: 0x000F1178
		public static void CheckboxLabeled(Rect rect, string label, ref bool checkOn, bool disabled = false, Texture2D texChecked = null, Texture2D texUnchecked = null, bool placeCheckboxNearText = false)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (placeCheckboxNearText)
			{
				rect.width = Mathf.Min(rect.width, Text.CalcSize(label).x + 24f + 10f);
			}
			Rect rect2 = rect;
			rect2.xMax -= 24f;
			Widgets.Label(rect2, label);
			if (!disabled && Widgets.ButtonInvisible(rect, true))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			Widgets.CheckboxDraw(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, checkOn, disabled, 24f, null, null);
			Text.Anchor = anchor;
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000F3050 File Offset: 0x000F1250
		public static bool CheckboxLabeledSelectable(Rect rect, string label, ref bool selected, ref bool checkOn, Texture2D labelIcon = null, float labelIconScale = 1f)
		{
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (labelIcon != null)
			{
				Rect outerRect = new Rect(rect.x, rect.y, (float)labelIcon.width, rect.height);
				rect.xMin += (float)labelIcon.width;
				Widgets.DrawTextureFitted(outerRect, labelIcon, labelIconScale);
			}
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			bool flag = selected;
			Rect butRect = rect;
			butRect.width -= 24f;
			if (!selected && Widgets.ButtonInvisible(butRect, true))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				selected = true;
			}
			Color color = GUI.color;
			GUI.color = Color.white;
			Widgets.CheckboxDraw(rect.xMax - 24f, rect.y, checkOn, false, 24f, null, null);
			GUI.color = color;
			if (Widgets.ButtonInvisible(new Rect(rect.xMax - 24f, rect.y, 24f, 24f), true))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			return selected && !flag;
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000F318B File Offset: 0x000F138B
		public static Texture2D GetCheckboxTexture(bool state)
		{
			if (state)
			{
				return Widgets.CheckboxOnTex;
			}
			return Widgets.CheckboxOffTex;
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000F319C File Offset: 0x000F139C
		public static void CheckboxDraw(float x, float y, bool active, bool disabled, float size = 24f, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Color color = GUI.color;
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Texture2D image;
			if (active)
			{
				image = ((texChecked != null) ? texChecked : Widgets.CheckboxOnTex);
			}
			else
			{
				image = ((texUnchecked != null) ? texUnchecked : Widgets.CheckboxOffTex);
			}
			GUI.DrawTexture(new Rect(x, y, size, size), image);
			if (disabled)
			{
				GUI.color = color;
			}
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000F3204 File Offset: 0x000F1404
		public static MultiCheckboxState CheckboxMulti(Rect rect, MultiCheckboxState state, bool paintable = false)
		{
			Texture2D tex;
			if (state == MultiCheckboxState.On)
			{
				tex = Widgets.CheckboxOnTex;
			}
			else if (state == MultiCheckboxState.Off)
			{
				tex = Widgets.CheckboxOffTex;
			}
			else
			{
				tex = Widgets.CheckboxPartialTex;
			}
			MouseoverSounds.DoRegion(rect);
			MultiCheckboxState multiCheckboxState = (state == MultiCheckboxState.Off) ? MultiCheckboxState.On : MultiCheckboxState.Off;
			bool flag = false;
			Widgets.DraggableResult draggableResult = Widgets.ButtonImageDraggable(rect, tex);
			if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
			{
				Widgets.checkboxPainting = true;
				Widgets.checkboxPaintingState = (multiCheckboxState == MultiCheckboxState.On);
				flag = true;
			}
			else if (draggableResult.AnyPressed())
			{
				flag = true;
			}
			else if (paintable && Widgets.checkboxPainting && Mouse.IsOver(rect))
			{
				multiCheckboxState = (Widgets.checkboxPaintingState ? MultiCheckboxState.On : MultiCheckboxState.Off);
				if (state != multiCheckboxState)
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (multiCheckboxState == MultiCheckboxState.On)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				return multiCheckboxState;
			}
			return state;
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000F32B2 File Offset: 0x000F14B2
		public static bool RadioButton(Vector2 topLeft, bool chosen)
		{
			return Widgets.RadioButton(topLeft.x, topLeft.y, chosen);
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000F32C6 File Offset: 0x000F14C6
		public static bool RadioButton(float x, float y, bool chosen)
		{
			Rect butRect = new Rect(x, y, 24f, 24f);
			Widgets.RadioButtonDraw(x, y, chosen);
			bool flag = Widgets.ButtonInvisible(butRect, true);
			if (flag && !chosen)
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			return flag;
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x000F32F8 File Offset: 0x000F14F8
		public static bool RadioButtonLabeled(Rect rect, string labelText, bool chosen)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, labelText);
			Text.Anchor = anchor;
			bool flag = Widgets.ButtonInvisible(rect, true);
			if (flag && !chosen)
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Widgets.RadioButtonDraw(rect.x + rect.width - 24f, rect.y + rect.height / 2f - 12f, chosen);
			return flag;
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x000F336C File Offset: 0x000F156C
		private static void RadioButtonDraw(float x, float y, bool chosen)
		{
			Color color = GUI.color;
			GUI.color = Color.white;
			Texture2D image;
			if (chosen)
			{
				image = Widgets.RadioButOnTex;
			}
			else
			{
				image = Widgets.RadioButOffTex;
			}
			GUI.DrawTexture(new Rect(x, y, 24f, 24f), image);
			GUI.color = color;
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x000F33B5 File Offset: 0x000F15B5
		public static bool ButtonText(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = true, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, overrideTextAnchor);
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x000F33C9 File Offset: 0x000F15C9
		public static bool ButtonText(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, textColor, active, false, overrideTextAnchor).AnyPressed();
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x000F33E0 File Offset: 0x000F15E0
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = false, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonTextDraggable(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, overrideTextAnchor);
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000F33F4 File Offset: 0x000F15F4
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, true, overrideTextAnchor);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000F340C File Offset: 0x000F160C
		private static Widgets.DraggableResult ButtonTextWorker(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active, bool draggable, TextAnchor? overrideTextAnchor = null)
		{
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (drawBackground)
			{
				Texture2D atlas = Widgets.ButtonBGAtlas;
				if (Mouse.IsOver(rect))
				{
					atlas = Widgets.ButtonBGAtlasMouseover;
					if (Input.GetMouseButton(0))
					{
						atlas = Widgets.ButtonBGAtlasClick;
					}
				}
				Widgets.DrawAtlas(rect, atlas);
			}
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (!drawBackground)
			{
				GUI.color = textColor;
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
			}
			if (overrideTextAnchor != null)
			{
				Text.Anchor = overrideTextAnchor.Value;
			}
			else if (drawBackground)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleLeft;
			}
			bool wordWrap = Text.WordWrap;
			if (rect.height < Text.LineHeight * 2f)
			{
				Text.WordWrap = false;
			}
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			Text.WordWrap = wordWrap;
			if (active && draggable)
			{
				return Widgets.ButtonInvisibleDraggable(rect, false);
			}
			if (!active)
			{
				return Widgets.DraggableResult.Idle;
			}
			if (!Widgets.ButtonInvisible(rect, false))
			{
				return Widgets.DraggableResult.Idle;
			}
			return Widgets.DraggableResult.Pressed;
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000F34F6 File Offset: 0x000F16F6
		public static void DrawRectFast(Rect position, Color color, GUIContent content = null)
		{
			Color backgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			GUI.Box(position, content ?? GUIContent.none, TexUI.FastFillStyle);
			GUI.backgroundColor = backgroundColor;
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x000F3520 File Offset: 0x000F1720
		public static bool CustomButtonText(ref Rect rect, string label, Color bgColor, Color textColor, Color borderColor, bool cacheHeight = false, int borderSize = 1, bool doMouseoverSound = true, bool active = true)
		{
			if (cacheHeight)
			{
				Widgets.LabelCacheHeight(ref rect, label, false, false);
			}
			Rect position = new Rect(rect);
			position.x += (float)borderSize;
			position.y += (float)borderSize;
			position.width -= (float)(borderSize * 2);
			position.height -= (float)(borderSize * 2);
			Widgets.DrawRectFast(rect, borderColor, null);
			Widgets.DrawRectFast(position, bgColor, null);
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			GUI.color = textColor;
			if (Mouse.IsOver(rect))
			{
				GUI.color = Widgets.MouseoverOptionColor;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			return active && Widgets.ButtonInvisible(rect, false);
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x000F360C File Offset: 0x000F180C
		public static bool ButtonTextSubtle(Rect rect, string label, float barPercent = 0f, float textLeftMargin = -1f, SoundDef mouseoverSound = null, Vector2 functionalSizeOffset = default(Vector2), Color? labelColor = null, bool highlight = false)
		{
			Rect rect2 = rect;
			rect2.width += functionalSizeOffset.x;
			rect2.height += functionalSizeOffset.y;
			bool flag = false;
			if (Mouse.IsOver(rect2))
			{
				flag = true;
				GUI.color = GenUI.MouseoverColor;
			}
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect2, mouseoverSound);
			}
			Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
			if (highlight)
			{
				GUI.color = Color.grey;
				Widgets.DrawBox(rect, 2, null);
			}
			GUI.color = Color.white;
			if (barPercent > 0.001f)
			{
				Widgets.FillableBar(rect.ContractedBy(1f), barPercent, Widgets.ButtonBarTex, null, false);
			}
			Rect rect3 = new Rect(rect);
			if (textLeftMargin < 0f)
			{
				textLeftMargin = rect.width * 0.15f;
			}
			rect3.x += textLeftMargin;
			if (flag)
			{
				rect3.x += 2f;
				rect3.y -= 2f;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Text.Font = GameFont.Small;
			GUI.color = (labelColor ?? Color.white);
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			return Widgets.ButtonInvisible(rect2, false);
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000F375F File Offset: 0x000F195F
		public static bool ButtonImage(Rect butRect, Texture2D tex, bool doMouseoverSound = true)
		{
			return Widgets.ButtonImage(butRect, tex, Color.white, doMouseoverSound);
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x000F376E File Offset: 0x000F196E
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, bool doMouseoverSound = true)
		{
			return Widgets.ButtonImage(butRect, tex, baseColor, GenUI.MouseoverColor, doMouseoverSound);
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x000F377E File Offset: 0x000F197E
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor, bool doMouseoverSound = true)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			bool result = Widgets.ButtonInvisible(butRect, doMouseoverSound);
			GUI.color = Color.white;
			return result;
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x000F37B5 File Offset: 0x000F19B5
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, Color.white);
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000F37C3 File Offset: 0x000F19C3
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x000F37D2 File Offset: 0x000F19D2
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			return Widgets.ButtonInvisibleDraggable(butRect, false);
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000F37FE File Offset: 0x000F19FE
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageFitted(butRect, tex, Color.white);
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000F380C File Offset: 0x000F1A0C
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageFitted(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x000F381B File Offset: 0x000F1A1B
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			Widgets.DrawTextureFitted(butRect, tex, 1f);
			GUI.color = baseColor;
			return Widgets.ButtonInvisible(butRect, true);
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x000F384C File Offset: 0x000F1A4C
		public static bool ButtonImageWithBG(Rect butRect, Texture2D image, Vector2? imageSize = null)
		{
			bool result = Widgets.ButtonText(butRect, "", true, true, true, null);
			Rect position;
			if (imageSize != null)
			{
				position = new Rect(Mathf.Floor(butRect.x + butRect.width / 2f - imageSize.Value.x / 2f), Mathf.Floor(butRect.y + butRect.height / 2f - imageSize.Value.y / 2f), imageSize.Value.x, imageSize.Value.y);
			}
			else
			{
				position = butRect;
			}
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x000F3900 File Offset: 0x000F1B00
		public static bool CloseButtonFor(Rect rectToClose)
		{
			return Widgets.ButtonImage(new Rect(rectToClose.x + rectToClose.width - 18f - 4f, rectToClose.y + 4f, 18f, 18f), TexButton.CloseXSmall, true);
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000F3950 File Offset: 0x000F1B50
		public static bool BackButtonFor(Rect rectToBack)
		{
			return Widgets.ButtonText(new Rect(rectToBack.x + rectToBack.width - 18f - 4f - 120f - 16f, rectToBack.y + 18f, 120f, 40f), "Back".Translate(), true, true, true, null);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000F39C0 File Offset: 0x000F1BC0
		public static bool ButtonInvisible(Rect butRect, bool doMouseoverSound = true)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			return GUI.Button(butRect, "", Widgets.EmptyStyle);
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000F39DC File Offset: 0x000F1BDC
		public static Widgets.DraggableResult ButtonInvisibleDraggable(Rect butRect, bool doMouseoverSound = false)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			int controlID = GUIUtility.GetControlID(FocusType.Passive, butRect);
			if (Input.GetMouseButtonDown(0) && Mouse.IsOver(butRect))
			{
				Widgets.buttonInvisibleDraggable_activeControl = controlID;
				Widgets.buttonInvisibleDraggable_mouseStart = Input.mousePosition;
				Widgets.buttonInvisibleDraggable_dragged = false;
			}
			if (Widgets.buttonInvisibleDraggable_activeControl == controlID)
			{
				if (Input.GetMouseButtonUp(0))
				{
					Widgets.buttonInvisibleDraggable_activeControl = 0;
					if (!Mouse.IsOver(butRect))
					{
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged)
					{
						return Widgets.DraggableResult.Pressed;
					}
					return Widgets.DraggableResult.DraggedThenPressed;
				}
				else
				{
					if (!Input.GetMouseButton(0))
					{
						Widgets.buttonInvisibleDraggable_activeControl = 0;
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged && (Widgets.buttonInvisibleDraggable_mouseStart - Input.mousePosition).sqrMagnitude > Widgets.DragStartDistanceSquared)
					{
						Widgets.buttonInvisibleDraggable_dragged = true;
						return Widgets.DraggableResult.Dragged;
					}
				}
			}
			return Widgets.DraggableResult.Idle;
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000F3A89 File Offset: 0x000F1C89
		public static string TextField(Rect rect, string text)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextField(rect, text, Text.CurTextFieldStyle);
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000F3AA4 File Offset: 0x000F1CA4
		public static string TextField(Rect rect, string text, int maxLength, Regex inputValidator = null)
		{
			string text2 = Widgets.TextField(rect, text);
			if (text2.Length <= maxLength && (inputValidator == null || inputValidator.IsMatch(text2)))
			{
				return text2;
			}
			return text;
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x000F3AD1 File Offset: 0x000F1CD1
		public static string TextArea(Rect rect, string text, bool readOnly = false)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextArea(rect, text, readOnly ? Text.CurTextAreaReadOnlyStyle : Text.CurTextAreaStyle);
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000F3AF4 File Offset: 0x000F1CF4
		public static string TextAreaScrollable(Rect rect, string text, ref Vector2 scrollbarPosition, bool readOnly = false)
		{
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, Mathf.Max(Text.CalcHeight(text, rect.width) + 10f, rect.height));
			Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			string result = Widgets.TextArea(rect2, text, readOnly);
			Widgets.EndScrollView();
			return result;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000F3B54 File Offset: 0x000F1D54
		public static string TextEntryLabeled(Rect rect, string label, string text)
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			if (rect.height <= 30f)
			{
				return Widgets.TextField(rect3, text);
			}
			return Widgets.TextArea(rect3, text, false);
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000F3BB0 File Offset: 0x000F1DB0
		public static string DelayedTextField(Rect rect, string text, ref string buffer, string previousFocusedControlName, string controlName = null)
		{
			controlName = (controlName ?? string.Format("TextField{0},{1}", rect.x, rect.y));
			bool flag = previousFocusedControlName == controlName;
			bool flag2 = GUI.GetNameOfFocusedControl() == controlName;
			string text2 = controlName + "_unfocused";
			GUI.SetNextControlName(text2);
			GUI.Label(rect, "");
			GUI.SetNextControlName(controlName);
			bool flag3 = false;
			if (flag2 && Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				Event.current.Use();
				flag3 = true;
			}
			bool flag4 = false;
			if (Event.current.type == EventType.MouseDown && !rect.Contains(Event.current.mousePosition))
			{
				flag4 = true;
			}
			if (!flag)
			{
				buffer = Widgets.TextField(rect, text);
				return buffer;
			}
			buffer = Widgets.TextField(rect, buffer);
			if (!flag2)
			{
				return buffer;
			}
			if (flag4 || flag3)
			{
				GUI.FocusControl(text2);
				return buffer;
			}
			return text;
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x000F3CB0 File Offset: 0x000F1EB0
		public static void TextFieldNumeric<T>(Rect rect, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			if (buffer == null)
			{
				buffer = val.ToString();
			}
			string text = "TextField" + rect.y.ToString("F0") + rect.x.ToString("F0");
			GUI.SetNextControlName(text);
			string text2 = Widgets.TextField(rect, buffer);
			if (GUI.GetNameOfFocusedControl() != text)
			{
				Widgets.ResolveParseNow<T>(buffer, ref val, ref buffer, min, max, true);
				return;
			}
			if (text2 != buffer && Widgets.IsPartiallyOrFullyTypedNumber<T>(ref val, text2, min, max))
			{
				buffer = text2;
				if (text2.IsFullyTypedNumber<T>())
				{
					Widgets.ResolveParseNow<T>(text2, ref val, ref buffer, min, max, false);
				}
			}
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x000F3D5C File Offset: 0x000F1F5C
		private static void ResolveParseNow<T>(string edited, ref T val, ref string buffer, float min, float max, bool force)
		{
			if (typeof(T) == typeof(int))
			{
				if (edited.NullOrEmpty())
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
				int num;
				if (int.TryParse(edited, out num))
				{
					val = (T)((object)Mathf.RoundToInt(Mathf.Clamp((float)num, min, max)));
					buffer = Widgets.ToStringTypedIn<T>(val);
					return;
				}
				if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
			}
			else if (typeof(T) == typeof(float))
			{
				float value;
				if (float.TryParse(edited, out value))
				{
					val = (T)((object)Mathf.Clamp(value, min, max));
					buffer = Widgets.ToStringTypedIn<T>(val);
					return;
				}
				if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
			}
			else
			{
				Log.Error("TextField<T> does not support " + typeof(T));
			}
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000F3E58 File Offset: 0x000F2058
		private static void ResetValue<T>(string edited, ref T val, ref string buffer, float min, float max)
		{
			val = default(T);
			if (min > 0f)
			{
				val = (T)((object)Mathf.RoundToInt(min));
			}
			if (max < 0f)
			{
				val = (T)((object)Mathf.RoundToInt(max));
			}
			buffer = Widgets.ToStringTypedIn<T>(val);
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000F3EB8 File Offset: 0x000F20B8
		private static string ToStringTypedIn<T>(T val)
		{
			if (typeof(T) == typeof(float))
			{
				return ((float)((object)val)).ToString("0.##########");
			}
			return val.ToString();
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000F3F08 File Offset: 0x000F2108
		private static bool IsPartiallyOrFullyTypedNumber<T>(ref T val, string s, float min, float max)
		{
			return s == "" || ((s[0] != '-' || min < 0f) && (s.Length <= 1 || s[s.Length - 1] != '-') && !(s == "00") && s.Length <= 12 && ((typeof(T) == typeof(float) && s.CharacterCount('.') <= 1 && s.ContainsOnlyCharacters("-.0123456789")) || s.IsFullyTypedNumber<T>()));
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000F3FB4 File Offset: 0x000F21B4
		private static bool IsFullyTypedNumber<T>(this string s)
		{
			if (s == "")
			{
				return false;
			}
			if (typeof(T) == typeof(float))
			{
				string[] array = s.Split(new char[]
				{
					'.'
				});
				if (array.Length > 2 || array.Length < 1)
				{
					return false;
				}
				if (!array[0].ContainsOnlyCharacters("-0123456789"))
				{
					return false;
				}
				if (array.Length == 2 && (array[1].Length == 0 || !array[1].ContainsOnlyCharacters("0123456789")))
				{
					return false;
				}
			}
			return !(typeof(T) == typeof(int)) || s.ContainsOnlyCharacters("-0123456789");
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000F4068 File Offset: 0x000F2268
		private static bool ContainsOnlyCharacters(this string s, string allowedChars)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!allowedChars.Contains(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000F4098 File Offset: 0x000F2298
		private static int CharacterCount(this string s, char c)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == c)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000F40C8 File Offset: 0x000F22C8
		public static void TextFieldNumericLabeled<T>(Rect rect, string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			Widgets.TextFieldNumeric<T>(rect3, ref val, ref buffer, min, max);
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000F4110 File Offset: 0x000F2310
		public static void TextFieldPercent(Rect rect, ref float val, ref string buffer, float min = 0f, float max = 1f)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 25f, rect.height);
			Widgets.Label(new Rect(rect2.xMax, rect.y, 25f, rect2.height), "%");
			float num = val * 100f;
			Widgets.TextFieldNumeric<float>(rect2, ref num, ref buffer, min * 100f, max * 100f);
			val = num / 100f;
			if (val > max)
			{
				val = max;
				buffer = val.ToString();
			}
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000F41AC File Offset: 0x000F23AC
		public static T ChangeType<T>(this object obj)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (T)((object)Convert.ChangeType(obj, typeof(T), invariantCulture));
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000F41D8 File Offset: 0x000F23D8
		public static void HorizontalSlider(Rect rect, ref float value, FloatRange range, string label = null, float roundTo = -1f)
		{
			float trueMin = range.TrueMin;
			float trueMax = range.TrueMax;
			value = Widgets.HorizontalSlider(rect, value, trueMin, trueMax, false, label, trueMin.ToString(), trueMax.ToString(), roundTo);
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000F4214 File Offset: 0x000F2414
		public static float HorizontalSlider(Rect rect, float value, float leftValue, float rightValue, bool middleAlignment = false, string label = null, string leftAlignedLabel = null, string rightAlignedLabel = null, float roundTo = -1f)
		{
			if (middleAlignment || !label.NullOrEmpty())
			{
				rect.y += Mathf.Round((rect.height - 10f) / 2f);
			}
			if (!label.NullOrEmpty())
			{
				rect.y += 5f;
			}
			float num = GUI.HorizontalSlider(rect, value, leftValue, rightValue);
			if (!label.NullOrEmpty() || !leftAlignedLabel.NullOrEmpty() || !rightAlignedLabel.NullOrEmpty())
			{
				TextAnchor anchor = Text.Anchor;
				GameFont font = Text.Font;
				Text.Font = GameFont.Tiny;
				float num2 = label.NullOrEmpty() ? 18f : Text.CalcSize(label).y;
				rect.y = rect.y - num2 + 3f;
				if (!leftAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect, leftAlignedLabel);
				}
				if (!rightAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect, rightAlignedLabel);
				}
				if (!label.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, label);
				}
				Text.Anchor = anchor;
				Text.Font = font;
			}
			if (roundTo > 0f)
			{
				num = (float)Mathf.RoundToInt(num / roundTo) * roundTo;
			}
			if (value != num)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			return num;
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000F4354 File Offset: 0x000F2554
		public static float FrequencyHorizontalSlider(Rect rect, float freq, float minFreq, float maxFreq, bool roundToInt = false)
		{
			float num;
			if (freq < 1f)
			{
				float x = 1f / freq;
				num = GenMath.LerpDouble(1f, 1f / minFreq, 0.5f, 1f, x);
			}
			else
			{
				num = GenMath.LerpDouble(maxFreq, 1f, 0f, 0.5f, freq);
			}
			string label;
			if (freq == 1f)
			{
				label = "EveryDay".Translate();
			}
			else if (freq < 1f)
			{
				label = "TimesPerDay".Translate((1f / freq).ToString("0.##"));
			}
			else
			{
				label = "EveryDays".Translate(freq.ToString("0.##"));
			}
			float num2 = Widgets.HorizontalSlider(rect, num, 0f, 1f, true, label, null, null, -1f);
			if (num != num2)
			{
				float num3;
				if (num2 < 0.5f)
				{
					num3 = GenMath.LerpDouble(0.5f, 0f, 1f, maxFreq, num2);
					if (roundToInt)
					{
						num3 = Mathf.Round(num3);
					}
				}
				else
				{
					float num4 = GenMath.LerpDouble(1f, 0.5f, 1f / minFreq, 1f, num2);
					if (roundToInt)
					{
						num4 = Mathf.Round(num4);
					}
					num3 = 1f / num4;
				}
				freq = num3;
			}
			return freq;
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000F44A0 File Offset: 0x000F26A0
		public static void IntEntry(Rect rect, ref int value, ref string editBuffer, int multiplier = 1)
		{
			int num = Mathf.Min(Widgets.IntEntryButtonWidth, (int)rect.width / 5);
			if (Widgets.ButtonText(new Rect(rect.xMin, rect.yMin, (float)num, rect.height), (-10 * multiplier).ToStringCached(), true, true, true, null))
			{
				value -= 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMin + (float)num, rect.yMin, (float)num, rect.height), (-1 * multiplier).ToStringCached(), true, true, true, null))
			{
				value -= multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)num, rect.yMin, (float)num, rect.height), "+" + (10 * multiplier).ToStringCached(), true, true, true, null))
			{
				value += 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)(num * 2), rect.yMin, (float)num, rect.height), "+" + multiplier.ToStringCached(), true, true, true, null))
			{
				value += multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			Widgets.TextFieldNumeric<int>(new Rect(rect.xMin + (float)(num * 2), rect.yMin, rect.width - (float)(num * 4), rect.height), ref value, ref editBuffer, 0f, 1E+09f);
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000F4680 File Offset: 0x000F2880
		public static void FloatRange(Rect rect, int id, ref FloatRange range, float min = 0f, float max = 1f, string labelKey = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo, float gap = 0f, GameFont sliderLabelFont = GameFont.Tiny, Color? sliderLabelColor = null)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = (sliderLabelColor ?? Widgets.RangeControlTextColor);
			string text = range.min.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute) + " - " + range.max.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute);
			if (labelKey != null)
			{
				text = labelKey.Translate(text);
			}
			GameFont font = Text.Font;
			Text.Font = sliderLabelFont;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			rect3.height = Mathf.Max(rect3.height, Text.CalcHeight(text, rect3.width));
			Widgets.Label(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			float num = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.min);
			float num2 = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.max);
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 < num4) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					float num5 = (Event.current.mousePosition.x - rect2.x) / rect2.width * (max - min) + min;
					num5 = Mathf.Clamp(num5, min, max);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num5 != range.min)
						{
							range.min = Mathf.Min(num5, max - gap);
							if (range.max < range.min + gap)
							{
								range.max = range.min + gap;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && num5 != range.max)
					{
						range.max = Mathf.Max(num5, min + gap);
						if (range.min > range.max - gap)
						{
							range.min = range.max - gap;
						}
						Widgets.CheckPlayDragSliderSound();
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000F4A50 File Offset: 0x000F2C50
		public static void IntRange(Rect rect, int id, ref IntRange range, int min = 0, int max = 100, string labelKey = null, int minWidth = 0)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string text = range.min.ToStringCached() + " - " + range.max.ToStringCached();
			if (labelKey != null)
			{
				text = labelKey.Translate(text);
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			float num = rect2.x + rect2.width * (float)(range.min - min) / (float)(max - min);
			float num2 = rect2.x + rect2.width * (float)(range.max - min) / (float)(max - min);
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 < num4) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					int num5 = Mathf.RoundToInt(Mathf.Clamp((Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(max - min) + (float)min, (float)min, (float)max));
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num5 != range.min)
						{
							range.min = num5;
							if (range.min > max - minWidth)
							{
								range.min = max - minWidth;
							}
							int num6 = Mathf.Max(min, range.min + minWidth);
							if (range.max < num6)
							{
								range.max = num6;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && num5 != range.max)
					{
						range.max = num5;
						if (range.max < min + minWidth)
						{
							range.max = min + minWidth;
						}
						int num7 = Mathf.Min(max, range.max - minWidth);
						if (range.min > num7)
						{
							range.min = num7;
						}
						Widgets.CheckPlayDragSliderSound();
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000F4E0C File Offset: 0x000F300C
		private static void CheckPlayDragSliderSound()
		{
			if (Time.realtimeSinceStartup > Widgets.lastDragSliderSoundTime + 0.075f)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Widgets.lastDragSliderSoundTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000F4E38 File Offset: 0x000F3038
		public static void QualityRange(Rect rect, int id, ref QualityRange range)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string label;
			if (range == RimWorld.QualityRange.All)
			{
				label = "AnyQuality".Translate();
			}
			else if (range.max == range.min)
			{
				label = "OnlyQuality".Translate(range.min.GetLabel());
			}
			else
			{
				label = range.min.GetLabel() + " - " + range.max.GetLabel();
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			int length = Enum.GetValues(typeof(QualityCategory)).Length;
			float num = rect2.x + rect2.width / (float)(length - 1) * (float)range.min;
			float num2 = rect2.x + rect2.width / (float)(length - 1) * (float)range.max;
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || id == Widgets.draggingId)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 < num4) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					int num5 = Mathf.RoundToInt((Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(length - 1));
					num5 = Mathf.Clamp(num5, 0, length - 1);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (range.min != (QualityCategory)num5)
						{
							range.min = (QualityCategory)num5;
							if (range.max < range.min)
							{
								range.max = range.min;
							}
							SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && range.max != (QualityCategory)num5)
					{
						range.max = (QualityCategory)num5;
						if (range.min > range.max)
						{
							range.min = range.max;
						}
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000F5210 File Offset: 0x000F3410
		public static void FloatRangeWithTypeIn(Rect rect, int id, ref FloatRange fRange, float sliderMin = 0f, float sliderMax = 1f, ToStringStyle valueStyle = ToStringStyle.FloatTwo, string labelKey = null)
		{
			Rect rect2 = new Rect(rect);
			rect2.width = rect.width / 4f;
			Rect rect3 = new Rect(rect);
			rect3.width = rect.width / 2f;
			rect3.x = rect.x + rect.width / 4f;
			rect3.height = rect.height / 2f;
			rect3.width -= rect.height;
			Rect butRect = new Rect(rect3);
			butRect.x = rect3.xMax;
			butRect.height = rect.height;
			butRect.width = rect.height;
			Rect rect4 = new Rect(rect);
			rect4.x = rect.x + rect.width * 0.75f;
			rect4.width = rect.width / 4f;
			rect3.y += 4f;
			rect3.height += 4f;
			Widgets.FloatRange(rect3, id, ref fRange, sliderMin, sliderMax, labelKey, valueStyle, 0f, GameFont.Tiny, null);
			if (Widgets.ButtonImage(butRect, TexButton.RangeMatch, true))
			{
				fRange.max = fRange.min;
			}
			float.TryParse(Widgets.TextField(rect2, fRange.min.ToString()), out fRange.min);
			float.TryParse(Widgets.TextField(rect4, fRange.max.ToString()), out fRange.max);
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000F539D File Offset: 0x000F359D
		public static Rect FillableBar(Rect rect, float fillPercent)
		{
			return Widgets.FillableBar(rect, fillPercent, Widgets.BarFullTexHor);
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000F53AC File Offset: 0x000F35AC
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex)
		{
			bool doBorder = rect.height > 15f && rect.width > 20f;
			return Widgets.FillableBar(rect, fillPercent, fillTex, Widgets.DefaultBarBgTex, doBorder);
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x000F53E8 File Offset: 0x000F35E8
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex, Texture2D bgTex, bool doBorder)
		{
			if (doBorder)
			{
				GUI.DrawTexture(rect, BaseContent.BlackTex);
				rect = rect.ContractedBy(3f);
			}
			if (bgTex != null)
			{
				GUI.DrawTexture(rect, bgTex);
			}
			Rect result = rect;
			rect.width *= fillPercent;
			GUI.DrawTexture(rect, fillTex);
			return result;
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000F5438 File Offset: 0x000F3638
		public static void FillableBarLabeled(Rect rect, float fillPercent, int labelWidth, string label)
		{
			if (fillPercent < 0f)
			{
				fillPercent = 0f;
			}
			if (fillPercent > 1f)
			{
				fillPercent = 1f;
			}
			Rect rect2 = rect;
			rect2.width = (float)labelWidth;
			Widgets.Label(rect2, label);
			Rect rect3 = rect;
			rect3.x += (float)labelWidth;
			rect3.width -= (float)labelWidth;
			Widgets.FillableBar(rect3, fillPercent);
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000F54A0 File Offset: 0x000F36A0
		public static void FillableBarChangeArrows(Rect barRect, float changeRate)
		{
			int changeRate2 = (int)(changeRate * Widgets.FillableBarChangeRateDisplayRatio);
			Widgets.FillableBarChangeArrows(barRect, changeRate2);
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000F54C0 File Offset: 0x000F36C0
		public static void FillableBarChangeArrows(Rect barRect, int changeRate)
		{
			if (changeRate == 0)
			{
				return;
			}
			if (changeRate > Widgets.MaxFillableBarChangeRate)
			{
				changeRate = Widgets.MaxFillableBarChangeRate;
			}
			if (changeRate < -Widgets.MaxFillableBarChangeRate)
			{
				changeRate = -Widgets.MaxFillableBarChangeRate;
			}
			float num = barRect.height;
			if (num > 16f)
			{
				num = 16f;
			}
			int num2 = Mathf.Abs(changeRate);
			float y = barRect.y + barRect.height / 2f - num / 2f;
			float num3;
			float num4;
			Texture2D image;
			if (changeRate > 0)
			{
				num3 = barRect.x + barRect.width + 2f;
				num4 = 8f;
				image = Widgets.FillArrowTexRight;
			}
			else
			{
				num3 = barRect.x - 8f - 2f;
				num4 = -8f;
				image = Widgets.FillArrowTexLeft;
			}
			for (int i = 0; i < num2; i++)
			{
				GUI.DrawTexture(new Rect(num3, y, 8f, num), image);
				num3 += num4;
			}
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000F55A1 File Offset: 0x000F37A1
		public static void DrawWindowBackground(Rect rect)
		{
			GUI.color = Widgets.WindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.WindowBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000F55D4 File Offset: 0x000F37D4
		public static void DrawWindowBackground(Rect rect, Color colorFactor)
		{
			Color color = GUI.color;
			GUI.color = Widgets.WindowBGFillColor * colorFactor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.WindowBGBorderColor * colorFactor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = color;
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000F5613 File Offset: 0x000F3813
		public static void DrawMenuSection(Rect rect)
		{
			GUI.color = Widgets.MenuSectionBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.MenuSectionBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000F5646 File Offset: 0x000F3846
		public static void DrawWindowBackgroundTutor(Rect rect)
		{
			GUI.color = Widgets.TutorWindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.TutorWindowBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000F5679 File Offset: 0x000F3879
		public static void DrawOptionUnselected(Rect rect)
		{
			GUI.color = Widgets.OptionUnselectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionUnselectedBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x000F56AC File Offset: 0x000F38AC
		public static void DrawOptionSelected(Rect rect)
		{
			GUI.color = Widgets.OptionSelectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionSelectedBGBorderColor;
			Widgets.DrawBox(rect.ExpandedBy(3f), 3, null);
			GUI.color = Color.white;
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x000F56E9 File Offset: 0x000F38E9
		public static void DrawOptionBackground(Rect rect, bool selected)
		{
			if (selected)
			{
				Widgets.DrawOptionSelected(rect);
			}
			else
			{
				Widgets.DrawOptionUnselected(rect);
			}
			Widgets.DrawHighlightIfMouseover(rect);
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x000F5704 File Offset: 0x000F3904
		public static void DrawShadowAround(Rect rect)
		{
			Rect rect2 = rect.ContractedBy(-9f);
			rect2.x += 2f;
			rect2.y += 2f;
			Widgets.DrawAtlas(rect2, Widgets.ShadowAtlas);
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x000F574E File Offset: 0x000F394E
		public static void DrawAtlas(Rect rect, Texture2D atlas)
		{
			Widgets.DrawAtlas(rect, atlas, true);
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000F5758 File Offset: 0x000F3958
		private static Rect AdjustRectToUIScaling(Rect rect)
		{
			Rect result = rect;
			result.xMin = Widgets.AdjustCoordToUIScalingFloor(rect.xMin);
			result.yMin = Widgets.AdjustCoordToUIScalingFloor(rect.yMin);
			result.xMax = Widgets.AdjustCoordToUIScalingCeil(rect.xMax);
			result.yMax = Widgets.AdjustCoordToUIScalingCeil(rect.yMax);
			return result;
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000F57B4 File Offset: 0x000F39B4
		public static float AdjustCoordToUIScalingFloor(float coord)
		{
			double num = (double)(Prefs.UIScale * coord);
			float num2 = (float)(num - Math.Floor(num)) / Prefs.UIScale;
			return coord - num2;
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000F57DC File Offset: 0x000F39DC
		public static float AdjustCoordToUIScalingCeil(float coord)
		{
			double num = (double)(Prefs.UIScale * coord);
			float num2 = (float)(num - Math.Ceiling(num)) / Prefs.UIScale;
			return coord - num2;
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000F5804 File Offset: 0x000F3A04
		public static void DrawAtlas(Rect rect, Texture2D atlas, bool drawTop)
		{
			rect.x = Mathf.Round(rect.x);
			rect.y = Mathf.Round(rect.y);
			rect.width = Mathf.Round(rect.width);
			rect.height = Mathf.Round(rect.height);
			rect = Widgets.AdjustRectToUIScaling(rect);
			float num = (float)atlas.width * 0.25f;
			num = Widgets.AdjustCoordToUIScalingCeil(GenMath.Min(num, rect.height / 2f, rect.width / 2f));
			Widgets.BeginGroup(rect);
			Rect drawRect;
			Rect uvRect;
			if (drawTop)
			{
				drawRect = new Rect(0f, 0f, num, num);
				uvRect = new Rect(0f, 0f, 0.25f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
				drawRect = new Rect(rect.width - num, 0f, num, num);
				uvRect = new Rect(0.75f, 0f, 0.25f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			}
			drawRect = new Rect(0f, rect.height - num, num, num);
			uvRect = new Rect(0f, 0.75f, 0.25f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(rect.width - num, rect.height - num, num, num);
			uvRect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(num, num, rect.width - num * 2f, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0.25f, 0.25f, 0.5f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			if (drawTop)
			{
				drawRect = new Rect(num, 0f, rect.width - num * 2f, num);
				uvRect = new Rect(0.25f, 0f, 0.5f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			}
			drawRect = new Rect(num, rect.height - num, rect.width - num * 2f, num);
			uvRect = new Rect(0.25f, 0.75f, 0.5f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(0f, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0f, 0.25f, 0.25f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(rect.width - num, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0.75f, 0.25f, 0.25f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			Widgets.EndGroup();
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000F5B47 File Offset: 0x000F3D47
		public static Rect ToUVRect(this Rect r, Vector2 texSize)
		{
			return new Rect(r.x / texSize.x, r.y / texSize.y, r.width / texSize.x, r.height / texSize.y);
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x000F5B86 File Offset: 0x000F3D86
		public static void DrawTexturePart(Rect drawRect, Rect uvRect, Texture2D tex)
		{
			uvRect.y = 1f - uvRect.y - uvRect.height;
			GUI.DrawTextureWithTexCoords(drawRect, tex, uvRect);
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000F5BAC File Offset: 0x000F3DAC
		public static void ScrollHorizontal(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, float ScrollWheelSpeed = 20f)
		{
			if (Event.current.type == EventType.ScrollWheel && Mouse.IsOver(outRect))
			{
				scrollPosition.x += Event.current.delta.y * ScrollWheelSpeed;
				float num = 0f;
				float num2 = viewRect.width - outRect.width + 16f;
				if (scrollPosition.x < num)
				{
					scrollPosition.x = num;
				}
				if (scrollPosition.x > num2)
				{
					scrollPosition.x = num2;
				}
				Event.current.Use();
			}
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x000F5C30 File Offset: 0x000F3E30
		public static void BeginScrollView(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, bool showScrollbars = true)
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Widgets.mouseOverScrollViewStack.Push(Widgets.mouseOverScrollViewStack.Peek() && outRect.Contains(Event.current.mousePosition));
			}
			else
			{
				Widgets.mouseOverScrollViewStack.Push(outRect.Contains(Event.current.mousePosition));
			}
			SteamDeck.HandleTouchScreenScrollViewScroll(outRect, ref scrollPosition);
			if (showScrollbars)
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect);
			}
			else
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect, GUIStyle.none, GUIStyle.none);
			}
			UnityGUIBugsFixer.Notify_BeginScrollView();
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x000F5CD2 File Offset: 0x000F3ED2
		public static void EndScrollView()
		{
			Widgets.mouseOverScrollViewStack.Pop();
			GUI.EndScrollView();
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000F5CE4 File Offset: 0x000F3EE4
		public static void EnsureMousePositionStackEmpty()
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Log.Error("Mouse position stack is not empty. There were more calls to BeginScrollView than EndScrollView. Fixing.");
				Widgets.mouseOverScrollViewStack.Clear();
			}
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x000F5D07 File Offset: 0x000F3F07
		public static void ColorSelectorIcon(Rect rect, Texture icon, Color color, bool drawColor = false)
		{
			if (icon != null)
			{
				GUI.color = color;
				GUI.DrawTexture(rect, icon);
				GUI.color = Color.white;
				return;
			}
			if (drawColor)
			{
				Widgets.DrawBoxSolid(rect, color);
			}
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x000F5D34 File Offset: 0x000F3F34
		public static bool ColorBox(Rect rect, ref Color color, Color boxColor, int colorSize = 22, int colorPadding = 2)
		{
			Widgets.DrawLightHighlight(rect);
			Widgets.DrawHighlightIfMouseover(rect);
			if (color.IndistinguishableFrom(boxColor))
			{
				Widgets.DrawBox(rect, 1, null);
			}
			Widgets.DrawBoxSolid(new Rect(rect.x + (float)colorPadding, rect.y + (float)colorPadding, (float)colorSize, (float)colorSize), boxColor);
			bool result = false;
			if (Widgets.ButtonInvisible(rect, true))
			{
				result = true;
				color = boxColor;
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			return result;
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x000F5DA8 File Offset: 0x000F3FA8
		public static bool ColorSelector(Rect rect, ref Color color, List<Color> colors, out float height, Texture icon = null, int colorSize = 22, int colorPadding = 2)
		{
			height = 0f;
			bool result = false;
			int num = colorSize + colorPadding * 2;
			float num2 = (icon != null) ? ((float)(colorSize * 4) + 10f) : 0f;
			int num3 = Mathf.FloorToInt((rect.width - num2 + (float)colorPadding) / (float)(num + colorPadding));
			int num4 = Mathf.CeilToInt((float)colors.Count / (float)num3);
			Widgets.BeginGroup(rect);
			Widgets.ColorSelectorIcon(new Rect(5f, 5f, (float)(colorSize * 4), (float)(colorSize * 4)), icon, color, false);
			for (int i = 0; i < colors.Count; i++)
			{
				int num5 = i / num3;
				int num6 = i % num3;
				float num7 = (icon != null) ? ((num2 - (float)(num * num4) - (float)colorPadding) / 2f) : 0f;
				Rect rect2 = new Rect(num2 + (float)(num6 * num) + (float)(num6 * colorPadding), num7 + (float)(num5 * num) + (float)(num5 * colorPadding), (float)num, (float)num);
				if (Widgets.ColorBox(rect2, ref color, colors[i], colorSize, colorPadding))
				{
					result = true;
				}
				height = Mathf.Max(height, rect2.yMax);
			}
			Widgets.EndGroup();
			return result;
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x000F5EDC File Offset: 0x000F40DC
		private static void DrawColorSelectionCircle(Rect hsvColorWheelRect, Vector2Int center, Color color)
		{
			int num = (int)Mathf.Round(hsvColorWheelRect.width * 0.125f);
			GUI.DrawTexture(new Rect((float)(center.x - num / 2), (float)(center.y - num / 2), (float)num, (float)num), Widgets.ColorSelectionCircle, ScaleMode.ScaleToFit, true, 1f, color, 0f, 0f);
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x000F5F3A File Offset: 0x000F413A
		private static bool ClickedInsideRect(Rect rect)
		{
			return Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition);
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x000F5F5C File Offset: 0x000F415C
		public static void HSVColorWheel(Rect rect, ref Color color, ref bool currentlyDragging, float? colorValueOverride = null, string controlName = null)
		{
			if (rect.width != rect.height)
			{
				throw new ArgumentException("HSV color wheel must be drawn in a square rect.");
			}
			float num;
			float d;
			float num2;
			Color.RGBToHSV(color, out num, out d, out num2);
			float num3 = colorValueOverride ?? num2;
			GUI.DrawTexture(rect, Widgets.HSVColorWheelTex, ScaleMode.ScaleToFit, true, 1f, Color.HSVToRGB(0f, 0f, num3), 0f, 0f);
			num = (num + 0.25f) * 2f * 3.1415927f;
			Vector2 a = new Vector2(Mathf.Cos(num), -Mathf.Sin(num)) * d * rect.width / 2f;
			Widgets.DrawColorSelectionCircle(rect, Vector2Int.RoundToInt(a + rect.center), (num3 > 0.5f) ? Color.black : Color.white);
			if (!currentlyDragging)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (Event.current.isMouse && Event.current.button == 0)
			{
				if (currentlyDragging && Event.current.type == EventType.MouseUp)
				{
					currentlyDragging = false;
					return;
				}
				if (Widgets.ClickedInsideRect(rect) | currentlyDragging)
				{
					GUI.FocusControl(controlName);
					currentlyDragging = true;
					Vector2 vector = (Event.current.mousePosition - rect.center) / (rect.size / 2f);
					float num4 = Mathf.Atan2(-vector.y, vector.x) / 6.2831855f;
					num4 += 1.75f;
					num4 %= 1f;
					float s = Mathf.Clamp01(vector.magnitude);
					color = Color.HSVToRGB(num4, s, num3);
					Event.current.Use();
				}
			}
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x000F6124 File Offset: 0x000F4324
		public static void ColorTemperatureBar(Rect rect, ref Color color, ref bool dragging, float? colorValueOverride = null)
		{
			float num = colorValueOverride ?? Mathf.Max(new float[]
			{
				color.r,
				color.g,
				color.b
			});
			float? num2 = color.ColorTemperature();
			string label = ((num2 != null) ? num2.GetValueOrDefault().ToString("0.K") : null) ?? "";
			RectDivider rect2 = new RectDivider(rect, 661493905, new Vector2?(new Vector2(17f, 0f)));
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				string text = "ColorTemperature".Translate().CapitalizeFirst();
				Widgets.Label(rect2.NewCol(Text.CalcSize(text).x, HorizontalJustification.Left), text);
				Widgets.Label(rect2.NewCol(Text.CalcSize("XXXXXK").x, HorizontalJustification.Left), label);
			}
			if (!dragging)
			{
				TooltipHandler.TipRegion(rect, "ColorTemperatureTooltip".Translate());
				MouseoverSounds.DoRegion(rect);
			}
			if (Event.current.button == 0)
			{
				if (dragging && Event.current.type == EventType.MouseUp)
				{
					dragging = false;
				}
				else if (Widgets.ClickedInsideRect(rect2) || (dragging && Event.current.type == EventType.MouseDrag))
				{
					dragging = true;
					Event.current.Use();
					float fraction = Mathf.Clamp01((Event.current.mousePosition.x - rect2.Rect.xMin) / rect2.Rect.width);
					num2 = new float?(GenMath.ExponentialWarpInterpolation(1000f, 40000f, fraction, new Vector2(0.5f, 6600f)));
					color = GenColor.FromColorTemperature(num2.Value);
					color *= num;
				}
			}
			rect2.NewRow(6f, VerticalJustification.Top);
			rect2.NewRow(6f, VerticalJustification.Bottom);
			GUI.DrawTexture(rect2, Widgets.ColorTemperatureExp, ScaleMode.StretchToFill, true, 1f, Color.HSVToRGB(0f, 0f, num), 0f, 0f);
			if (num2 != null)
			{
				float num3 = rect2.Rect.width * GenMath.InverseExponentialWarpInterpolation(1000f, 40000f, num2.Value, new Vector2(0.5f, 6600f));
				Rect position = new Rect(rect2.Rect.x + num3 - 6f, rect2.Rect.y - 6f, 12f, 12f);
				Rect position2 = new Rect(rect2.Rect.x + num3 - 6f, rect2.Rect.yMax - 6f, 12f, 12f);
				GUI.DrawTextureWithTexCoords(position, Widgets.SelectionArrow, new Rect(0f, 1f, 1f, -1f), true);
				GUI.DrawTextureWithTexCoords(position2, Widgets.SelectionArrow, new Rect(0f, 0f, 1f, 1f), true);
			}
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x000F64A0 File Offset: 0x000F46A0
		private static int ToIntegerRange(float fraction, int min, int max)
		{
			return Mathf.Clamp(Mathf.RoundToInt(fraction * (float)max), min, max);
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x000F64B4 File Offset: 0x000F46B4
		public static bool ColorTextfields(ref RectAggregator aggregator, ref Color color, ref string[] buffers, ref Color colorBuffer, string previousFocusedControlName, string controlName = null, Widgets.ColorComponents editable = Widgets.ColorComponents.All, Widgets.ColorComponents visible = Widgets.ColorComponents.All)
		{
			if (visible == Widgets.ColorComponents.None)
			{
				return false;
			}
			if ((~(visible != Widgets.ColorComponents.None) & editable) != Widgets.ColorComponents.None)
			{
				throw new ArgumentException(string.Format("Cannot have editable but invisible components {0}.", ~visible & editable));
			}
			controlName = (controlName ?? string.Format("ColorTextfields{0}{1}", aggregator.Rect.x, aggregator.Rect.y));
			bool flag = previousFocusedControlName != null && previousFocusedControlName.StartsWith(controlName);
			bool flag2 = GUI.GetNameOfFocusedControl().StartsWith(controlName);
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				float num = 30f;
				float num2 = 0f;
				for (int i = 0; i < Widgets.colorComponentLabels.Length; i++)
				{
					Widgets.tmpTranslatedColorComponentLabels[i] = Widgets.colorComponentLabels[i].Translate().CapitalizeFirst();
					num = Mathf.Max(num, Widgets.tmpTranslatedColorComponentLabels[i].GetHeightCached());
					num2 = Mathf.Max(num2, Widgets.tmpTranslatedColorComponentLabels[i].GetWidthCached());
				}
				float fraction;
				float fraction2;
				float fraction3;
				Color.RGBToHSV(colorBuffer, out fraction, out fraction2, out fraction3);
				Widgets.intColorComponents[0] = Widgets.ToIntegerRange(colorBuffer.r, 0, Widgets.maxColorComponentValues[0]);
				Widgets.intColorComponents[1] = Widgets.ToIntegerRange(colorBuffer.g, 0, Widgets.maxColorComponentValues[1]);
				Widgets.intColorComponents[2] = Widgets.ToIntegerRange(colorBuffer.b, 0, Widgets.maxColorComponentValues[2]);
				Widgets.intColorComponents[3] = Widgets.ToIntegerRange(fraction, 0, Widgets.maxColorComponentValues[3]);
				Widgets.intColorComponents[4] = Widgets.ToIntegerRange(fraction2, 0, Widgets.maxColorComponentValues[4]);
				Widgets.intColorComponents[5] = Widgets.ToIntegerRange(fraction3, 0, Widgets.maxColorComponentValues[5]);
				for (int j = 0; j <= 5; j++)
				{
					Widgets.ColorComponents colorComponents = (Widgets.ColorComponents)(1 << j);
					if ((visible & colorComponents) != Widgets.ColorComponents.None)
					{
						RectDivider rect = aggregator.NewRow(num, VerticalJustification.Bottom);
						Widgets.Label(rect.NewCol(num2, HorizontalJustification.Left), Widgets.tmpTranslatedColorComponentLabels[j]);
						if ((editable & colorComponents) == Widgets.ColorComponents.None)
						{
							Widgets.Label(rect, Widgets.intColorComponents[j].ToString());
						}
						else
						{
							string text = Widgets.intColorComponents[j].ToString();
							string text2 = Widgets.DelayedTextField(rect, text, ref buffers[j], previousFocusedControlName, string.Format("{0}_{1}", controlName, j));
							int num3;
							if (text != text2 && int.TryParse(text2, out num3))
							{
								Widgets.intColorComponents[j] = num3;
								if (j < 3)
								{
									colorBuffer = new ColorInt(Widgets.intColorComponents[0], Widgets.intColorComponents[1], Widgets.intColorComponents[2]).ToColor;
								}
								else
								{
									colorBuffer = Color.HSVToRGB((float)Widgets.intColorComponents[3] / 360f, (float)Widgets.intColorComponents[4] / 100f, (float)Widgets.intColorComponents[5] / 100f);
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				if (!flag2)
				{
					color = colorBuffer;
					return true;
				}
			}
			else
			{
				colorBuffer = color;
			}
			return false;
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x000F67E8 File Offset: 0x000F49E8
		public static void DrawHighlightSelected(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x000F67F5 File Offset: 0x000F49F5
		public static void DrawHighlightIfMouseover(Rect rect)
		{
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x000F6805 File Offset: 0x000F4A05
		public static void DrawHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightTex);
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000F6812 File Offset: 0x000F4A12
		public static void DrawLightHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.LightHighlight);
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000F681F File Offset: 0x000F4A1F
		public static void DrawStrongHighlight(Rect rect, Color? color = null)
		{
			Color color2 = GUI.color;
			GUI.color = color.GetValueOrDefault(Widgets.HighlightStrongBgColor);
			Widgets.DrawAtlas(rect, TexUI.RectHighlight);
			GUI.color = color2;
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x000F6848 File Offset: 0x000F4A48
		public static void DrawTextHighlight(Rect rect, float expandBy = 4f, Color? color = null)
		{
			rect.y -= expandBy;
			rect.height += expandBy * 2f;
			Color color2 = GUI.color;
			GUI.color = color.GetValueOrDefault(Widgets.HighlightTextBgColor);
			GUI.DrawTexture(rect, TexUI.RectTextHighlight);
			GUI.color = color2;
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000F689F File Offset: 0x000F4A9F
		public static void DrawTitleBG(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.TitleBGTex);
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000F68AC File Offset: 0x000F4AAC
		public static bool InfoCardButton(float x, float y, Thing thing)
		{
			IConstructible constructible = thing as IConstructible;
			if (constructible != null)
			{
				ThingDef thingDef = thing.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					return Widgets.InfoCardButton(x, y, thingDef, constructible.EntityToBuildStuff());
				}
				return Widgets.InfoCardButton(x, y, thing.def.entityDefToBuild);
			}
			else
			{
				if (Widgets.InfoCardButtonWorker(x, y))
				{
					Find.WindowStack.Add(new Dialog_InfoCard(thing, null));
					return true;
				}
				return false;
			}
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000F6916 File Offset: 0x000F4B16
		public static bool InfoCardButton(float x, float y, Def def)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, null));
				return true;
			}
			return false;
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000F6935 File Offset: 0x000F4B35
		public static bool InfoCardButton(Rect rect, Def def)
		{
			if (Widgets.InfoCardButtonWorker(rect))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, null));
				return true;
			}
			return false;
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x000F6953 File Offset: 0x000F4B53
		public static bool InfoCardButton(float x, float y, Def def, Precept_ThingStyle precept)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, precept));
				return true;
			}
			return false;
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x000F6972 File Offset: 0x000F4B72
		public static bool InfoCardButton(float x, float y, ThingDef thingDef, ThingDef stuffDef)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(thingDef, stuffDef, null));
				return true;
			}
			return false;
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x000F6992 File Offset: 0x000F4B92
		public static bool InfoCardButton(float x, float y, WorldObject worldObject)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(worldObject));
				return true;
			}
			return false;
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000F69B0 File Offset: 0x000F4BB0
		public static bool InfoCardButton(Rect rect, Hediff hediff)
		{
			if (Widgets.InfoCardButtonWorker(rect))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(hediff));
				return true;
			}
			return false;
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x000F69CD File Offset: 0x000F4BCD
		public static bool InfoCardButton(float x, float y, Faction faction)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(faction));
				return true;
			}
			return false;
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000F69EB File Offset: 0x000F4BEB
		public static bool InfoCardButtonCentered(Rect rect, Thing thing)
		{
			return Widgets.InfoCardButton(rect.center.x - 12f, rect.center.y - 12f, thing);
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000F6A17 File Offset: 0x000F4C17
		public static bool InfoCardButtonCentered(Rect rect, Faction faction)
		{
			return Widgets.InfoCardButton(rect.center.x - 12f, rect.center.y - 12f, faction);
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000F6A43 File Offset: 0x000F4C43
		private static bool InfoCardButtonWorker(float x, float y)
		{
			return Widgets.InfoCardButtonWorker(new Rect(x, y, 24f, 24f));
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000F6A5B File Offset: 0x000F4C5B
		private static bool InfoCardButtonWorker(Rect rect)
		{
			MouseoverSounds.DoRegion(rect);
			TooltipHandler.TipRegionByKey(rect, "DefInfoTip");
			bool result = Widgets.ButtonImage(rect, TexButton.Info, GUI.color, true);
			UIHighlighter.HighlightOpportunity(rect, "InfoCard");
			return result;
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000F6A8A File Offset: 0x000F4C8A
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale)
		{
			Widgets.DrawTextureFitted(outerRect, tex, scale, new Vector2((float)tex.width, (float)tex.height), new Rect(0f, 0f, 1f, 1f), 0f, null);
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000F6AC6 File Offset: 0x000F4CC6
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, Material mat)
		{
			Widgets.DrawTextureFitted(outerRect, tex, scale, new Vector2((float)tex.width, (float)tex.height), new Rect(0f, 0f, 1f, 1f), 0f, mat);
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000F6B04 File Offset: 0x000F4D04
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, Vector2 texProportions, Rect texCoords, float angle = 0f, Material mat = null)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			Rect rect = new Rect(0f, 0f, texProportions.x, texProportions.y);
			float num;
			if (rect.width / rect.height < outerRect.width / outerRect.height)
			{
				num = outerRect.height / rect.height;
			}
			else
			{
				num = outerRect.width / rect.width;
			}
			num *= scale;
			rect.width *= num;
			rect.height *= num;
			rect.x = outerRect.x + outerRect.width / 2f - rect.width / 2f;
			rect.y = outerRect.y + outerRect.height / 2f - rect.height / 2f;
			Matrix4x4 matrix = Matrix4x4.identity;
			if (angle != 0f)
			{
				matrix = GUI.matrix;
				UI.RotateAroundPivot(angle, rect.center);
			}
			GenUI.DrawTextureWithMaterial(rect, tex, mat, texCoords);
			if (angle != 0f)
			{
				GUI.matrix = matrix;
			}
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x000F6C34 File Offset: 0x000F4E34
		public static void DrawTextureRotated(Vector2 center, Texture tex, float angle, float scale = 1f)
		{
			float num = (float)tex.width * scale;
			float num2 = (float)tex.height * scale;
			Widgets.DrawTextureRotated(new Rect(center.x - num / 2f, center.y - num2 / 2f, num, num2), tex, angle);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x000F6C7F File Offset: 0x000F4E7F
		public static void DrawTextureRotated(Rect rect, Texture tex, float angle)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (angle == 0f)
			{
				GUI.DrawTexture(rect, tex);
				return;
			}
			Matrix4x4 matrix = GUI.matrix;
			UI.RotateAroundPivot(angle, rect.center);
			GUI.DrawTexture(rect, tex);
			GUI.matrix = matrix;
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x000F6CBD File Offset: 0x000F4EBD
		public static void NoneLabel(float y, float width, string customLabel = null)
		{
			Widgets.NoneLabel(ref y, width, customLabel);
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x000F6CC8 File Offset: 0x000F4EC8
		public static void NoneLabel(ref float curY, float width, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(new Rect(0f, curY, width, 30f), customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			curY += 25f;
			GUI.color = Color.white;
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000F6D2A File Offset: 0x000F4F2A
		public static void NoneLabelCenteredVertically(Rect rect, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000F6D68 File Offset: 0x000F4F68
		public static void DraggableBar(Rect barRect, Texture2D barTexture, Texture2D barHighlightTexture, Texture2D emptyBarTex, Texture2D dragBarTex, ref bool draggingBar, float barValue, ref float targetValue, List<float> bandPercentages = null, int increments = 20)
		{
			bool flag = Mouse.IsOver(barRect);
			Widgets.FillableBar(barRect, Mathf.Min(barValue, 1f), flag ? barHighlightTexture : barTexture, emptyBarTex, true);
			if (!bandPercentages.NullOrEmpty<float>())
			{
				for (int i = 1; i < bandPercentages.Count - 1; i++)
				{
					Widgets.DrawDraggableBarThreshold(barRect, bandPercentages[i], barValue);
				}
			}
			float num = Mathf.Clamp(Mathf.Round((Event.current.mousePosition.x - barRect.x) / barRect.width * (float)increments) / (float)increments, 0f, 1f);
			Event current = Event.current;
			if (current.type == EventType.MouseDown && current.button == 0 && flag)
			{
				targetValue = num;
				draggingBar = true;
				current.Use();
			}
			if (((current.type == EventType.MouseDrag && current.button == 0) & draggingBar) && flag)
			{
				if (Math.Abs(num - targetValue) > 1E-45f)
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				targetValue = num;
				current.Use();
			}
			if ((current.type == EventType.MouseUp && current.button == 0) & draggingBar)
			{
				draggingBar = false;
				current.Use();
			}
			Widgets.DrawDraggableBarTarget(barRect, draggingBar ? num : targetValue, dragBarTex);
			GUI.color = Color.white;
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000F6EB0 File Offset: 0x000F50B0
		private static void DrawDraggableBarThreshold(Rect rect, float percent, float curValue)
		{
			Rect position = new Rect
			{
				x = rect.x + 3f + (rect.width - 8f) * percent,
				y = rect.y + rect.height - 9f,
				width = 2f,
				height = 6f
			};
			if (curValue < percent)
			{
				GUI.DrawTexture(position, BaseContent.GreyTex);
				return;
			}
			GUI.DrawTexture(position, BaseContent.BlackTex);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x000F6F3C File Offset: 0x000F513C
		private static void DrawDraggableBarTarget(Rect rect, float percent, Texture2D targetTex)
		{
			float num = Mathf.Round((rect.width - 8f) * percent);
			GUI.DrawTexture(new Rect
			{
				x = rect.x + 3f + num,
				y = rect.y,
				width = 2f,
				height = rect.height
			}, targetTex);
			float num2 = Widgets.AdjustCoordToUIScalingFloor(rect.x + 2f + num);
			float xMax = Widgets.AdjustCoordToUIScalingCeil(num2 + 4f);
			Rect rect2 = new Rect
			{
				y = rect.y - 3f,
				height = 5f,
				xMin = num2,
				xMax = xMax
			};
			GUI.DrawTexture(rect2, targetTex);
			Rect position = rect2;
			position.y = rect.yMax - 2f;
			GUI.DrawTexture(position, targetTex);
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000F702C File Offset: 0x000F522C
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false)
		{
			Widgets.Dropdown<Target, Payload>(rect, target, Color.white, getPayload, menuGenerator, buttonLabel, buttonIcon, dragLabel, dragIcon, dropdownOpened, paintable);
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000F7054 File Offset: 0x000F5254
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Color iconColor, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false)
		{
			MouseoverSounds.DoRegion(rect);
			Widgets.DraggableResult draggableResult;
			if (buttonIcon != null)
			{
				Widgets.DrawHighlightIfMouseover(rect);
				GUI.color = iconColor;
				Widgets.DrawTextureFitted(rect, buttonIcon, 1f);
				GUI.color = Color.white;
				draggableResult = Widgets.ButtonInvisibleDraggable(rect, false);
			}
			else
			{
				draggableResult = Widgets.ButtonTextDraggable(rect, buttonLabel, true, false, true, null);
			}
			if (draggableResult == Widgets.DraggableResult.Pressed)
			{
				List<FloatMenuOption> options = (from opt in menuGenerator(target)
				select opt.option).ToList<FloatMenuOption>();
				Find.WindowStack.Add(new FloatMenu(options));
				if (dropdownOpened != null)
				{
					dropdownOpened();
					return;
				}
			}
			else
			{
				if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
				{
					Widgets.dropdownPainting = true;
					Widgets.dropdownPainting_Payload = getPayload(target);
					Widgets.dropdownPainting_Type = typeof(Payload);
					Widgets.dropdownPainting_Text = ((dragLabel != null) ? dragLabel : buttonLabel);
					Widgets.dropdownPainting_Icon = ((dragIcon != null) ? dragIcon : buttonIcon);
					return;
				}
				if (paintable && Widgets.dropdownPainting && Mouse.IsOver(rect) && Widgets.dropdownPainting_Type == typeof(Payload))
				{
					FloatMenuOption floatMenuOption = (from opt in menuGenerator(target)
					where object.Equals(opt.payload, Widgets.dropdownPainting_Payload)
					select opt.option).FirstOrDefault<FloatMenuOption>();
					if (floatMenuOption != null && !floatMenuOption.Disabled)
					{
						Payload x = getPayload(target);
						floatMenuOption.action();
						Payload y = getPayload(target);
						if (!EqualityComparer<Payload>.Default.Equals(x, y))
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
					}
				}
			}
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x000F722C File Offset: 0x000F542C
		public static void MouseAttachedLabel(string label, float xOffset = 0f, float yOffset = 0f)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect rect = new Rect(mousePosition.x + 8f + xOffset, mousePosition.y + 8f + yOffset, 32f, 32f);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(rect.xMax, rect.y, 9999f, 100f);
			Vector2 vector = Text.CalcSize(label);
			rect2.height = Mathf.Max(rect2.height, vector.y);
			GUI.DrawTexture(new Rect(rect2.x - vector.x * 0.1f, rect2.y, vector.x * 1.2f, vector.y), TexUI.GrayTextBG);
			Widgets.Label(rect2, label);
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x000F7304 File Offset: 0x000F5504
		public static void WidgetsOnGUI()
		{
			if (Event.current.rawType == EventType.MouseUp || Input.GetMouseButtonUp(0))
			{
				Widgets.checkboxPainting = false;
				Widgets.dropdownPainting = false;
			}
			if (Widgets.checkboxPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.checkboxPaintingState ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
			}
			if (Widgets.dropdownPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.dropdownPainting_Icon, Widgets.dropdownPainting_Text, 0f, default(Vector2), null, false, default(Color), null, null);
			}
		}

		// Token: 0x040018BE RID: 6334
		public static Stack<bool> mouseOverScrollViewStack = new Stack<bool>();

		// Token: 0x040018BF RID: 6335
		public static readonly GUIStyle EmptyStyle = new GUIStyle();

		// Token: 0x040018C0 RID: 6336
		[TweakValue("Input", 0f, 100f)]
		private static float DragStartDistanceSquared = 20f;

		// Token: 0x040018C1 RID: 6337
		public const int LeftMouseButton = 0;

		// Token: 0x040018C2 RID: 6338
		public static readonly Color InactiveColor = new Color(0.37f, 0.37f, 0.37f, 0.8f);

		// Token: 0x040018C3 RID: 6339
		public static readonly Color HighlightStrongBgColor = ColorLibrary.SkyBlue;

		// Token: 0x040018C4 RID: 6340
		public static readonly Color HighlightTextBgColor = Widgets.HighlightStrongBgColor.ToTransparent(0.25f);

		// Token: 0x040018C5 RID: 6341
		private static readonly Texture2D DefaultBarBgTex = BaseContent.BlackTex;

		// Token: 0x040018C6 RID: 6342
		public static readonly Texture2D BarFullTexHor = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		// Token: 0x040018C7 RID: 6343
		public static readonly Texture2D CheckboxOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);

		// Token: 0x040018C8 RID: 6344
		public static readonly Texture2D CheckboxOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOff", true);

		// Token: 0x040018C9 RID: 6345
		public static readonly Texture2D CheckboxPartialTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckPartial", true);

		// Token: 0x040018CA RID: 6346
		public const float CheckboxSize = 24f;

		// Token: 0x040018CB RID: 6347
		public const float RadioButtonSize = 24f;

		// Token: 0x040018CC RID: 6348
		public static readonly Texture2D RadioButOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOn", true);

		// Token: 0x040018CD RID: 6349
		public static readonly Texture2D HSVColorWheelTex = ContentFinder<Texture2D>.Get("UI/Widgets/HSVColorWheel", true);

		// Token: 0x040018CE RID: 6350
		public static readonly Texture2D ColorSelectionCircle = ContentFinder<Texture2D>.Get("UI/Overlays/TargetHighlight_Square", true);

		// Token: 0x040018CF RID: 6351
		public static readonly Texture2D ColorTemperatureExp = ContentFinder<Texture2D>.Get("UI/Widgets/ColorTemperatureExp", true);

		// Token: 0x040018D0 RID: 6352
		public static readonly Texture2D SelectionArrow = ContentFinder<Texture2D>.Get("Things/Mote/InteractionArrow", true);

		// Token: 0x040018D1 RID: 6353
		private static readonly Texture2D RadioButOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOff", true);

		// Token: 0x040018D2 RID: 6354
		private static readonly Texture2D FillArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowRight", true);

		// Token: 0x040018D3 RID: 6355
		private static readonly Texture2D FillArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowLeft", true);

		// Token: 0x040018D4 RID: 6356
		public static readonly Texture2D PlaceholderIconTex = ContentFinder<Texture2D>.Get("UI/Icons/MenuOptionNoIcon", true);

		// Token: 0x040018D5 RID: 6357
		private const int FillableBarBorderWidth = 3;

		// Token: 0x040018D6 RID: 6358
		private const int MaxFillChangeArrowHeight = 16;

		// Token: 0x040018D7 RID: 6359
		private const int FillChangeArrowWidth = 8;

		// Token: 0x040018D8 RID: 6360
		public const float CloseButtonSize = 18f;

		// Token: 0x040018D9 RID: 6361
		public const float CloseButtonMargin = 4f;

		// Token: 0x040018DA RID: 6362
		public const float BackButtonWidth = 120f;

		// Token: 0x040018DB RID: 6363
		public const float BackButtonHeight = 40f;

		// Token: 0x040018DC RID: 6364
		public const float BackButtonMargin = 16f;

		// Token: 0x040018DD RID: 6365
		private const float ColorHighlightCircleFraction = 0.125f;

		// Token: 0x040018DE RID: 6366
		private const float ColorTextfieldHeight = 30f;

		// Token: 0x040018DF RID: 6367
		private const float SelectionArrowSize = 12f;

		// Token: 0x040018E0 RID: 6368
		private static readonly Texture2D ShadowAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/DropShadow", true);

		// Token: 0x040018E1 RID: 6369
		public static readonly Texture2D ButtonBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBG", true);

		// Token: 0x040018E2 RID: 6370
		private static readonly Texture2D ButtonBGAtlasMouseover = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGMouseover", true);

		// Token: 0x040018E3 RID: 6371
		public static readonly Texture2D ButtonBGAtlasClick = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGClick", true);

		// Token: 0x040018E4 RID: 6372
		private static readonly Texture2D FloatRangeSliderTex = ContentFinder<Texture2D>.Get("UI/Widgets/RangeSlider", true);

		// Token: 0x040018E5 RID: 6373
		public static readonly Texture2D LightHighlight = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.04f));

		// Token: 0x040018E6 RID: 6374
		private static readonly Rect DefaultTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x040018E7 RID: 6375
		private static readonly Rect LinkedTexCoords = new Rect(0f, 0.5f, 0.25f, 0.25f);

		// Token: 0x040018E8 RID: 6376
		[TweakValue("Input", 0f, 100f)]
		private static int IntEntryButtonWidth = 40;

		// Token: 0x040018E9 RID: 6377
		private static Texture2D LineTexAA = null;

		// Token: 0x040018EA RID: 6378
		private static readonly Texture2D AltTexture = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x040018EB RID: 6379
		public static readonly Color NormalOptionColor = new Color(0.8f, 0.85f, 1f);

		// Token: 0x040018EC RID: 6380
		public static readonly Color MouseoverOptionColor = Color.yellow;

		// Token: 0x040018ED RID: 6381
		private static Dictionary<string, float> LabelCache = new Dictionary<string, float>();

		// Token: 0x040018EE RID: 6382
		private const float TileSize = 64f;

		// Token: 0x040018EF RID: 6383
		public static readonly Color SeparatorLabelColor = new Color(0.8f, 0.8f, 0.8f, 1f);

		// Token: 0x040018F0 RID: 6384
		public static readonly Color SeparatorLineColor = new Color(0.3f, 0.3f, 0.3f, 1f);

		// Token: 0x040018F1 RID: 6385
		private const float SeparatorLabelHeight = 20f;

		// Token: 0x040018F2 RID: 6386
		public const float ListSeparatorHeight = 25f;

		// Token: 0x040018F3 RID: 6387
		private static bool checkboxPainting = false;

		// Token: 0x040018F4 RID: 6388
		private static bool checkboxPaintingState = false;

		// Token: 0x040018F5 RID: 6389
		public static readonly Texture2D ButtonSubtleAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonSubtleAtlas", true);

		// Token: 0x040018F6 RID: 6390
		private static readonly Texture2D ButtonBarTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(78, 109, 129, 130).ToColor);

		// Token: 0x040018F7 RID: 6391
		public const float ButtonSubtleDefaultMarginPct = 0.15f;

		// Token: 0x040018F8 RID: 6392
		private static int buttonInvisibleDraggable_activeControl = 0;

		// Token: 0x040018F9 RID: 6393
		private static bool buttonInvisibleDraggable_dragged = false;

		// Token: 0x040018FA RID: 6394
		private static Vector3 buttonInvisibleDraggable_mouseStart = Vector3.zero;

		// Token: 0x040018FB RID: 6395
		public const float RangeControlIdealHeight = 31f;

		// Token: 0x040018FC RID: 6396
		public const float RangeControlCompactHeight = 28f;

		// Token: 0x040018FD RID: 6397
		private const float RangeSliderSize = 16f;

		// Token: 0x040018FE RID: 6398
		private static readonly Color RangeControlTextColor = new Color(0.6f, 0.6f, 0.6f);

		// Token: 0x040018FF RID: 6399
		private static int draggingId = 0;

		// Token: 0x04001900 RID: 6400
		private static Widgets.RangeEnd curDragEnd = Widgets.RangeEnd.None;

		// Token: 0x04001901 RID: 6401
		private static float lastDragSliderSoundTime = -1f;

		// Token: 0x04001902 RID: 6402
		private static float FillableBarChangeRateDisplayRatio = 100000000f;

		// Token: 0x04001903 RID: 6403
		public static int MaxFillableBarChangeRate = 3;

		// Token: 0x04001904 RID: 6404
		private static readonly Color WindowBGBorderColor = new ColorInt(97, 108, 122).ToColor;

		// Token: 0x04001905 RID: 6405
		public static readonly Color WindowBGFillColor = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x04001906 RID: 6406
		public static readonly Color MenuSectionBGFillColor = new ColorInt(42, 43, 44).ToColor;

		// Token: 0x04001907 RID: 6407
		private static readonly Color MenuSectionBGBorderColor = new ColorInt(135, 135, 135).ToColor;

		// Token: 0x04001908 RID: 6408
		private static readonly Color TutorWindowBGFillColor = new ColorInt(133, 85, 44).ToColor;

		// Token: 0x04001909 RID: 6409
		private static readonly Color TutorWindowBGBorderColor = new ColorInt(176, 139, 61).ToColor;

		// Token: 0x0400190A RID: 6410
		private static readonly Color OptionUnselectedBGFillColor = new Color(0.21f, 0.21f, 0.21f);

		// Token: 0x0400190B RID: 6411
		private static readonly Color OptionUnselectedBGBorderColor = Widgets.OptionUnselectedBGFillColor * 1.8f;

		// Token: 0x0400190C RID: 6412
		private static readonly Color OptionSelectedBGFillColor = new Color(0.32f, 0.28f, 0.21f);

		// Token: 0x0400190D RID: 6413
		private static readonly Color OptionSelectedBGBorderColor = Widgets.OptionSelectedBGFillColor * 1.8f;

		// Token: 0x0400190E RID: 6414
		private static int[] maxColorComponentValues = new int[]
		{
			255,
			255,
			255,
			360,
			100,
			100
		};

		// Token: 0x0400190F RID: 6415
		private static string[] colorComponentLabels = new string[]
		{
			"Red",
			"Green",
			"Blue",
			"Hue",
			"Saturation",
			"Value"
		};

		// Token: 0x04001910 RID: 6416
		private static string[] tmpTranslatedColorComponentLabels = new string[6];

		// Token: 0x04001911 RID: 6417
		private static int[] intColorComponents = new int[6];

		// Token: 0x04001912 RID: 6418
		public const float InfoCardButtonSize = 24f;

		// Token: 0x04001913 RID: 6419
		private static bool dropdownPainting = false;

		// Token: 0x04001914 RID: 6420
		private static object dropdownPainting_Payload = null;

		// Token: 0x04001915 RID: 6421
		private static Type dropdownPainting_Type = null;

		// Token: 0x04001916 RID: 6422
		private static string dropdownPainting_Text = "";

		// Token: 0x04001917 RID: 6423
		private static Texture2D dropdownPainting_Icon = null;

		// Token: 0x020020D4 RID: 8404
		public enum DraggableResult
		{
			// Token: 0x0400824F RID: 33359
			Idle,
			// Token: 0x04008250 RID: 33360
			Pressed,
			// Token: 0x04008251 RID: 33361
			Dragged,
			// Token: 0x04008252 RID: 33362
			DraggedThenPressed
		}

		// Token: 0x020020D5 RID: 8405
		private enum RangeEnd : byte
		{
			// Token: 0x04008254 RID: 33364
			None,
			// Token: 0x04008255 RID: 33365
			Min,
			// Token: 0x04008256 RID: 33366
			Max
		}

		// Token: 0x020020D6 RID: 8406
		[Flags]
		public enum ColorComponents
		{
			// Token: 0x04008258 RID: 33368
			Red = 1,
			// Token: 0x04008259 RID: 33369
			Green = 2,
			// Token: 0x0400825A RID: 33370
			Blue = 4,
			// Token: 0x0400825B RID: 33371
			Hue = 8,
			// Token: 0x0400825C RID: 33372
			Sat = 16,
			// Token: 0x0400825D RID: 33373
			Value = 32,
			// Token: 0x0400825E RID: 33374
			None = 0,
			// Token: 0x0400825F RID: 33375
			All = 63
		}

		// Token: 0x020020D7 RID: 8407
		public struct DropdownMenuElement<Payload>
		{
			// Token: 0x04008260 RID: 33376
			public FloatMenuOption option;

			// Token: 0x04008261 RID: 33377
			public Payload payload;
		}
	}
}

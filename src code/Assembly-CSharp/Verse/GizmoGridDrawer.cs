using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020004A1 RID: 1185
	public static class GizmoGridDrawer
	{
		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x000E4021 File Offset: 0x000E2221
		public static float HeightDrawnRecently
		{
			get
			{
				if (Time.frameCount > GizmoGridDrawer.heightDrawnFrame + 2)
				{
					return 0f;
				}
				return GizmoGridDrawer.heightDrawn;
			}
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x000E403C File Offset: 0x000E223C
		public static void DrawGizmoGrid(IEnumerable<Gizmo> gizmos, float startX, out Gizmo mouseoverGizmo, Func<Gizmo, bool> customActivatorFunc = null, Func<Gizmo, bool> highlightFunc = null, Func<Gizmo, bool> lowlightFunc = null)
		{
			if (Event.current.type == EventType.Layout)
			{
				mouseoverGizmo = null;
				return;
			}
			GizmoGridDrawer.tmpAllGizmos.Clear();
			GizmoGridDrawer.tmpAllGizmos.AddRange(gizmos);
			GizmoGridDrawer.tmpAllGizmos.SortStable(GizmoGridDrawer.SortByOrder);
			GizmoGridDrawer.gizmoGroups.Clear();
			for (int i = 0; i < GizmoGridDrawer.tmpAllGizmos.Count; i++)
			{
				Gizmo gizmo = GizmoGridDrawer.tmpAllGizmos[i];
				bool flag = false;
				for (int j = 0; j < GizmoGridDrawer.gizmoGroups.Count; j++)
				{
					if (GizmoGridDrawer.gizmoGroups[j][0].GroupsWith(gizmo))
					{
						flag = true;
						GizmoGridDrawer.gizmoGroups[j].Add(gizmo);
						GizmoGridDrawer.gizmoGroups[j][0].MergeWith(gizmo);
						break;
					}
				}
				if (!flag)
				{
					List<Gizmo> list = SimplePool<List<Gizmo>>.Get();
					list.Add(gizmo);
					GizmoGridDrawer.gizmoGroups.Add(list);
				}
			}
			GizmoGridDrawer.firstGizmos.Clear();
			GizmoGridDrawer.shrinkableCommands.Clear();
			float num = (float)(UI.screenWidth - 147);
			float num2 = (float)(UI.screenHeight - 35) - GizmoGridDrawer.GizmoSpacing.y - 75f;
			if (SteamDeck.IsSteamDeck && SteamDeck.KeyboardShowing && Find.MainTabsRoot.OpenTab == MainButtonDefOf.Architect && ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).QuickSearchWidgetFocused)
			{
				num2 -= 335f;
			}
			Vector2 vector = new Vector2(startX, num2);
			float maxWidth = num - startX;
			int num3 = 0;
			for (int k = 0; k < GizmoGridDrawer.gizmoGroups.Count; k++)
			{
				List<Gizmo> list2 = GizmoGridDrawer.gizmoGroups[k];
				Gizmo gizmo2 = null;
				for (int l = 0; l < list2.Count; l++)
				{
					if (!list2[l].disabled)
					{
						gizmo2 = list2[l];
						break;
					}
				}
				if (gizmo2 == null)
				{
					gizmo2 = list2.FirstOrDefault<Gizmo>();
				}
				else
				{
					Command_Toggle command_Toggle = gizmo2 as Command_Toggle;
					if (command_Toggle != null)
					{
						if (!command_Toggle.activateIfAmbiguous && !command_Toggle.isActive())
						{
							for (int m = 0; m < list2.Count; m++)
							{
								Command_Toggle command_Toggle2 = list2[m] as Command_Toggle;
								if (command_Toggle2 != null && !command_Toggle2.disabled && command_Toggle2.isActive())
								{
									gizmo2 = list2[m];
									break;
								}
							}
						}
						if (command_Toggle.activateIfAmbiguous && command_Toggle.isActive())
						{
							for (int n = 0; n < list2.Count; n++)
							{
								Command_Toggle command_Toggle3 = list2[n] as Command_Toggle;
								if (command_Toggle3 != null && !command_Toggle3.disabled && !command_Toggle3.isActive())
								{
									gizmo2 = list2[n];
									break;
								}
							}
						}
					}
				}
				if (gizmo2 != null)
				{
					Command_Ability command_Ability;
					if ((command_Ability = (gizmo2 as Command_Ability)) != null)
					{
						command_Ability.GroupAbilityCommands(list2);
					}
					Command command;
					if ((command = (gizmo2 as Command)) != null && command.shrinkable && command.Visible)
					{
						GizmoGridDrawer.shrinkableCommands.Add(command);
					}
					if (vector.x + gizmo2.GetWidth(maxWidth) > num)
					{
						vector.x = startX;
						vector.y -= 75f + GizmoGridDrawer.GizmoSpacing.y;
						num3++;
					}
					vector.x += gizmo2.GetWidth(maxWidth) + GizmoGridDrawer.GizmoSpacing.x;
					GizmoGridDrawer.firstGizmos.Add(gizmo2);
				}
			}
			if (num3 > 1 && GizmoGridDrawer.shrinkableCommands.Count > 1)
			{
				for (int num4 = 0; num4 < GizmoGridDrawer.shrinkableCommands.Count; num4++)
				{
					GizmoGridDrawer.firstGizmos.Remove(GizmoGridDrawer.shrinkableCommands[num4]);
				}
			}
			else
			{
				GizmoGridDrawer.shrinkableCommands.Clear();
			}
			GizmoGridDrawer.drawnHotKeys.Clear();
			GizmoGridDrawer.customActivator = customActivatorFunc;
			Text.Font = GameFont.Tiny;
			Vector2 vector2 = new Vector2(startX, num2);
			mouseoverGizmo = null;
			GizmoGridDrawer.<>c__DisplayClass12_0 CS$<>8__locals1;
			CS$<>8__locals1.interactedGiz = null;
			CS$<>8__locals1.interactedEvent = null;
			CS$<>8__locals1.floatMenuGiz = null;
			bool isFirst = true;
			for (int num5 = 0; num5 < GizmoGridDrawer.firstGizmos.Count; num5++)
			{
				Gizmo gizmo3 = GizmoGridDrawer.firstGizmos[num5];
				if (gizmo3.Visible)
				{
					if (vector2.x + gizmo3.GetWidth(maxWidth) > num)
					{
						vector2.x = startX;
						vector2.y -= 75f + GizmoGridDrawer.GizmoSpacing.y;
					}
					GizmoGridDrawer.heightDrawnFrame = Time.frameCount;
					GizmoGridDrawer.heightDrawn = (float)UI.screenHeight - vector2.y;
					GizmoRenderParms parms = new GizmoRenderParms
					{
						highLight = (highlightFunc != null && highlightFunc(gizmo3)),
						lowLight = (lowlightFunc != null && lowlightFunc(gizmo3)),
						isFirst = isFirst
					};
					GizmoResult result = gizmo3.GizmoOnGUI(vector2, maxWidth, parms);
					GizmoGridDrawer.<DrawGizmoGrid>g__ProcessGizmoState|12_0(gizmo3, result, ref mouseoverGizmo, ref CS$<>8__locals1);
					isFirst = false;
					GenUI.AbsorbClicksInRect(new Rect(vector2.x - 12f, vector2.y, gizmo3.GetWidth(maxWidth) + 12f, 75f + GizmoGridDrawer.GizmoSpacing.y));
					vector2.x += gizmo3.GetWidth(maxWidth) + GizmoGridDrawer.GizmoSpacing.x;
				}
			}
			float x = vector2.x;
			int num6 = 0;
			for (int num7 = 0; num7 < GizmoGridDrawer.shrinkableCommands.Count; num7++)
			{
				Command command2 = GizmoGridDrawer.shrinkableCommands[num7];
				float getShrunkSize = command2.GetShrunkSize;
				if (vector2.x + getShrunkSize > num)
				{
					num6++;
					if (num6 > 1)
					{
						x = startX;
					}
					vector2.x = x;
					vector2.y -= getShrunkSize + 3f;
				}
				Vector2 vector3 = vector2;
				vector3.y += getShrunkSize + 3f;
				GizmoGridDrawer.heightDrawnFrame = Time.frameCount;
				GizmoGridDrawer.heightDrawn = Mathf.Min(GizmoGridDrawer.heightDrawn, (float)UI.screenHeight - vector3.y);
				GizmoRenderParms parms2 = new GizmoRenderParms
				{
					highLight = (highlightFunc != null && highlightFunc(command2)),
					lowLight = (lowlightFunc != null && lowlightFunc(command2)),
					isFirst = isFirst
				};
				GizmoResult result2 = command2.GizmoOnGUIShrunk(vector3, getShrunkSize, parms2);
				GizmoGridDrawer.<DrawGizmoGrid>g__ProcessGizmoState|12_0(command2, result2, ref mouseoverGizmo, ref CS$<>8__locals1);
				isFirst = false;
				GenUI.AbsorbClicksInRect(new Rect(vector3.x - 3f, vector3.y, getShrunkSize + 3f, getShrunkSize + 3f));
				vector2.x += getShrunkSize + 3f;
			}
			if (CS$<>8__locals1.interactedGiz != null)
			{
				List<Gizmo> list3 = GizmoGridDrawer.<DrawGizmoGrid>g__FindMatchingGroup|12_1(CS$<>8__locals1.interactedGiz);
				for (int num8 = 0; num8 < list3.Count; num8++)
				{
					Gizmo gizmo4 = list3[num8];
					if (gizmo4 != CS$<>8__locals1.interactedGiz && !gizmo4.disabled && CS$<>8__locals1.interactedGiz.InheritInteractionsFrom(gizmo4))
					{
						gizmo4.ProcessInput(CS$<>8__locals1.interactedEvent);
					}
				}
				CS$<>8__locals1.interactedGiz.ProcessInput(CS$<>8__locals1.interactedEvent);
				Event.current.Use();
			}
			else if (CS$<>8__locals1.floatMenuGiz != null)
			{
				List<FloatMenuOption> list4 = new List<FloatMenuOption>();
				foreach (FloatMenuOption item in CS$<>8__locals1.floatMenuGiz.RightClickFloatMenuOptions)
				{
					list4.Add(item);
				}
				List<Gizmo> list5 = GizmoGridDrawer.<DrawGizmoGrid>g__FindMatchingGroup|12_1(CS$<>8__locals1.floatMenuGiz);
				for (int num9 = 0; num9 < list5.Count; num9++)
				{
					Gizmo gizmo5 = list5[num9];
					if (gizmo5 != CS$<>8__locals1.floatMenuGiz && !gizmo5.disabled && CS$<>8__locals1.floatMenuGiz.InheritFloatMenuInteractionsFrom(gizmo5))
					{
						foreach (FloatMenuOption floatMenuOption in gizmo5.RightClickFloatMenuOptions)
						{
							FloatMenuOption floatMenuOption2 = null;
							for (int num10 = 0; num10 < list4.Count; num10++)
							{
								if (list4[num10].Label == floatMenuOption.Label)
								{
									floatMenuOption2 = list4[num10];
									break;
								}
							}
							if (floatMenuOption2 == null)
							{
								list4.Add(floatMenuOption);
							}
							else if (!floatMenuOption.Disabled)
							{
								if (!floatMenuOption2.Disabled)
								{
									Action prevAction = floatMenuOption2.action;
									Action localOptionAction = floatMenuOption.action;
									floatMenuOption2.action = delegate()
									{
										prevAction();
										localOptionAction();
									};
								}
								else if (floatMenuOption2.Disabled)
								{
									list4[list4.IndexOf(floatMenuOption2)] = floatMenuOption;
								}
							}
						}
					}
				}
				Event.current.Use();
				if (list4.Any<FloatMenuOption>())
				{
					Find.WindowStack.Add(new FloatMenu(list4));
				}
			}
			for (int num11 = 0; num11 < GizmoGridDrawer.gizmoGroups.Count; num11++)
			{
				GizmoGridDrawer.gizmoGroups[num11].Clear();
				SimplePool<List<Gizmo>>.Return(GizmoGridDrawer.gizmoGroups[num11]);
			}
			GizmoGridDrawer.gizmoGroups.Clear();
			GizmoGridDrawer.firstGizmos.Clear();
			GizmoGridDrawer.tmpAllGizmos.Clear();
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000E4A0C File Offset: 0x000E2C0C
		[CompilerGenerated]
		internal static void <DrawGizmoGrid>g__ProcessGizmoState|12_0(Gizmo giz, GizmoResult result, ref Gizmo mouseoverGiz, ref GizmoGridDrawer.<>c__DisplayClass12_0 A_3)
		{
			if (result.State == GizmoState.Interacted || (result.State == GizmoState.OpenedFloatMenu && giz.RightClickFloatMenuOptions.FirstOrDefault<FloatMenuOption>() == null))
			{
				A_3.interactedEvent = result.InteractEvent;
				A_3.interactedGiz = giz;
			}
			else if (result.State == GizmoState.OpenedFloatMenu)
			{
				A_3.floatMenuGiz = giz;
			}
			if (result.State >= GizmoState.Mouseover)
			{
				mouseoverGiz = giz;
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000E4A70 File Offset: 0x000E2C70
		[CompilerGenerated]
		internal static List<Gizmo> <DrawGizmoGrid>g__FindMatchingGroup|12_1(Gizmo toMatch)
		{
			for (int i = 0; i < GizmoGridDrawer.gizmoGroups.Count; i++)
			{
				if (GizmoGridDrawer.gizmoGroups[i].Contains(toMatch))
				{
					return GizmoGridDrawer.gizmoGroups[i];
				}
			}
			return null;
		}

		// Token: 0x040016F8 RID: 5880
		public static HashSet<KeyCode> drawnHotKeys = new HashSet<KeyCode>();

		// Token: 0x040016F9 RID: 5881
		public static Func<Gizmo, bool> customActivator;

		// Token: 0x040016FA RID: 5882
		private static float heightDrawn;

		// Token: 0x040016FB RID: 5883
		private static int heightDrawnFrame;

		// Token: 0x040016FC RID: 5884
		public static readonly Vector2 GizmoSpacing = new Vector2(5f, 14f);

		// Token: 0x040016FD RID: 5885
		private static List<List<Gizmo>> gizmoGroups = new List<List<Gizmo>>();

		// Token: 0x040016FE RID: 5886
		private static List<Gizmo> firstGizmos = new List<Gizmo>();

		// Token: 0x040016FF RID: 5887
		private static List<Command> shrinkableCommands = new List<Command>();

		// Token: 0x04001700 RID: 5888
		private static List<Gizmo> tmpAllGizmos = new List<Gizmo>();

		// Token: 0x04001701 RID: 5889
		private static readonly Func<Gizmo, Gizmo, int> SortByOrder = (Gizmo lhs, Gizmo rhs) => lhs.Order.CompareTo(rhs.Order);
	}
}

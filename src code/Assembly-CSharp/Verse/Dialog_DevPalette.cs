using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000464 RID: 1124
	[StaticConstructorOnStartup]
	public class Dialog_DevPalette : Window
	{
		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x00002662 File Offset: 0x00000862
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x0001D158 File Offset: 0x0001B358
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002288 RID: 8840 RVA: 0x000DC564 File Offset: 0x000DA764
		private List<DebugActionNode> Nodes
		{
			get
			{
				if (Dialog_DevPalette.cachedNodes == null)
				{
					Dialog_DevPalette.cachedNodes = new List<DebugActionNode>();
					for (int i = 0; i < Prefs.DebugActionsPalette.Count; i++)
					{
						DebugActionNode node = Dialog_Debug.GetNode(Prefs.DebugActionsPalette[i]);
						if (node != null)
						{
							Dialog_DevPalette.cachedNodes.Add(node);
						}
					}
				}
				return Dialog_DevPalette.cachedNodes;
			}
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000DC5BC File Offset: 0x000DA7BC
		public Dialog_DevPalette()
		{
			this.draggable = true;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
			this.drawInScreenshotMode = false;
			this.windowPosition = Prefs.DevPalettePosition;
			this.onlyDrawInDevMode = true;
			this.lastLabelCacheFrame = RealTime.frameCount;
			this.EnsureAllNodesValid();
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x000DC63C File Offset: 0x000DA83C
		private void EnsureAllNodesValid()
		{
			Dialog_DevPalette.cachedNodes = null;
			for (int i = Prefs.DebugActionsPalette.Count - 1; i >= 0; i--)
			{
				string text = Prefs.DebugActionsPalette[i];
				if (Dialog_Debug.GetNode(text) == null)
				{
					Log.Warning("Could not find node from path '" + text + "'. Removing.");
					Prefs.DebugActionsPalette.RemoveAt(i);
					Prefs.Save();
				}
			}
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000DC69F File Offset: 0x000DA89F
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (RealTime.frameCount >= this.lastLabelCacheFrame + 30)
			{
				this.nameCache.Clear();
				this.lastLabelCacheFrame = RealTime.frameCount;
			}
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x000DC6D0 File Offset: 0x000DA8D0
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 24f), "Dev palette");
			inRect.yMin += 26f;
			if (Prefs.DebugActionsPalette.Count == 0)
			{
				GUI.color = ColoredText.SubtleGrayColor;
				Widgets.Label(inRect, "<i>To add commands here, open the debug actions menu and click the pin icons.</i>");
				GUI.color = Color.white;
			}
			else
			{
				if (Event.current.type == EventType.Repaint)
				{
					this.reorderableGroupID = ReorderableWidget.NewGroup(delegate(int from, int to)
					{
						string item = Prefs.DebugActionsPalette[from];
						Prefs.DebugActionsPalette.Insert(to, item);
						Prefs.DebugActionsPalette.RemoveAt((from < to) ? from : (from + 1));
						Dialog_DevPalette.cachedNodes = null;
						Prefs.Save();
					}, ReorderableDirection.Vertical, inRect, -1f, null, false);
				}
				GUI.BeginGroup(inRect);
				float num = 0f;
				Text.Font = GameFont.Tiny;
				for (int i = 0; i < this.Nodes.Count; i++)
				{
					DebugActionNode debugActionNode = this.Nodes[i];
					float num2 = 0f;
					Rect rect = new Rect(num2, num, 18f, 18f);
					if (ReorderableWidget.Reorderable(this.reorderableGroupID, rect.ExpandedBy(4f), false, true))
					{
						Widgets.DrawRectFast(rect, Widgets.WindowBGFillColor * new Color(1f, 1f, 1f, 0.5f), null);
					}
					Widgets.ButtonImage(rect.ContractedBy(1f), TexButton.DragHash, true);
					num2 += 18f;
					Rect rect2 = new Rect(num2, num, inRect.width - 36f, 18f);
					if (debugActionNode.ActiveNow)
					{
						if (debugActionNode.settingsField != null)
						{
							Rect rect3 = rect2;
							rect3.xMax -= rect3.height + 4f;
							Widgets.Label(rect3, "  " + this.PrettifyNodeName(debugActionNode));
							GUI.DrawTexture(new Rect(rect3.xMax, rect3.y, rect3.height, rect3.height), debugActionNode.On ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
							Widgets.DrawHighlightIfMouseover(rect2);
							if (Widgets.ButtonInvisible(rect2, true))
							{
								debugActionNode.Enter(null);
							}
						}
						else if (Widgets.ButtonText(rect2, "  " + this.PrettifyNodeName(debugActionNode), true, true, true, new TextAnchor?(TextAnchor.MiddleLeft)))
						{
							debugActionNode.Enter(Find.WindowStack.WindowOfType<Dialog_Debug>());
						}
					}
					else
					{
						Widgets.Label(rect2, "  " + this.PrettifyNodeName(debugActionNode));
					}
					num2 += rect2.width;
					Rect butRect = new Rect(num2, num, 18f, 18f);
					if (Widgets.ButtonImage(butRect, Widgets.CheckboxOffTex, true))
					{
						Prefs.DebugActionsPalette.RemoveAt(i);
						Dialog_DevPalette.cachedNodes = null;
						this.SetInitialSizeAndPosition();
					}
					num2 += butRect.width;
					num += 20f;
				}
				GUI.EndGroup();
			}
			if (!Mathf.Approximately(this.windowRect.x, this.windowPosition.x) || !Mathf.Approximately(this.windowRect.y, this.windowPosition.y))
			{
				this.windowPosition = new Vector2(this.windowRect.x, this.windowRect.y);
				Prefs.DevPalettePosition = this.windowPosition;
			}
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000DCA18 File Offset: 0x000DAC18
		public static void ToggleAction(string actionLabel)
		{
			if (Prefs.DebugActionsPalette.Contains(actionLabel))
			{
				Prefs.DebugActionsPalette.Remove(actionLabel);
			}
			else
			{
				Prefs.DebugActionsPalette.Add(actionLabel);
			}
			Prefs.Save();
			Dialog_DevPalette.cachedNodes = null;
			Dialog_DevPalette dialog_DevPalette = Find.WindowStack.WindowOfType<Dialog_DevPalette>();
			if (dialog_DevPalette == null)
			{
				return;
			}
			dialog_DevPalette.SetInitialSizeAndPosition();
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x000DCA6C File Offset: 0x000DAC6C
		protected override void SetInitialSizeAndPosition()
		{
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			Vector2 vector = new Vector2(Text.CalcSize("Dev palette").x + 48f + 10f, 28f);
			if (!this.Nodes.Any<DebugActionNode>())
			{
				vector.x = Mathf.Max(vector.x, 200f);
				vector.y += Text.CalcHeight("<i>To add commands here, open the debug actions menu and click the pin icons.</i>", vector.x) + this.Margin * 2f;
			}
			else
			{
				Text.Font = GameFont.Tiny;
				for (int i = 0; i < this.Nodes.Count; i++)
				{
					vector.x = Mathf.Max(vector.x, Text.CalcSize("  " + this.PrettifyNodeName(this.Nodes[i]) + "  ").x + 48f);
				}
				vector.y += (float)this.Nodes.Count * 18f + (float)((this.Nodes.Count + 1) * 2) + this.Margin;
			}
			this.windowPosition.x = Mathf.Clamp(this.windowPosition.x, 0f, (float)UI.screenWidth - vector.x);
			this.windowPosition.y = Mathf.Clamp(this.windowPosition.y, 0f, (float)UI.screenHeight - vector.y);
			this.windowRect = new Rect(this.windowPosition.x, this.windowPosition.y, vector.x, vector.y);
			this.windowRect = this.windowRect.Rounded();
			Text.Font = font;
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x000DCC30 File Offset: 0x000DAE30
		private string PrettifyNodeName(DebugActionNode node)
		{
			string path = node.Path;
			string text;
			if (this.nameCache.TryGetValue(path, out text))
			{
				return text;
			}
			DebugActionNode debugActionNode = node;
			text = debugActionNode.LabelNow.Replace("...", "");
			while (debugActionNode.parent != null && !debugActionNode.parent.IsRoot && (debugActionNode.parent.parent == null || !debugActionNode.parent.parent.IsRoot))
			{
				text = debugActionNode.parent.LabelNow.Replace("...", "") + "\\" + text;
				debugActionNode = debugActionNode.parent;
			}
			this.nameCache[path] = text;
			return text;
		}

		// Token: 0x040015EC RID: 5612
		private Vector2 windowPosition;

		// Token: 0x040015ED RID: 5613
		private static List<DebugActionNode> cachedNodes;

		// Token: 0x040015EE RID: 5614
		private int reorderableGroupID = -1;

		// Token: 0x040015EF RID: 5615
		private Dictionary<string, string> nameCache = new Dictionary<string, string>();

		// Token: 0x040015F0 RID: 5616
		private int lastLabelCacheFrame = -1;

		// Token: 0x040015F1 RID: 5617
		private const string Title = "Dev palette";

		// Token: 0x040015F2 RID: 5618
		private const float ButtonSize = 24f;

		// Token: 0x040015F3 RID: 5619
		private const float ButtonSize_Small = 18f;

		// Token: 0x040015F4 RID: 5620
		private const string NoActionDesc = "<i>To add commands here, open the debug actions menu and click the pin icons.</i>";
	}
}

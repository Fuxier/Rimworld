using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047B RID: 1147
	[StaticConstructorOnStartup]
	public class EditWindow_Log : EditWindow
	{
		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x000DDEAB File Offset: 0x000DC0AB
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060022BE RID: 8894 RVA: 0x00002662 File Offset: 0x00000862
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060022BF RID: 8895 RVA: 0x000DDECA File Offset: 0x000DC0CA
		// (set) Token: 0x060022C0 RID: 8896 RVA: 0x000DDED1 File Offset: 0x000DC0D1
		private static LogMessage SelectedMessage
		{
			get
			{
				return EditWindow_Log.selectedMessage;
			}
			set
			{
				if (EditWindow_Log.selectedMessage == value)
				{
					return;
				}
				EditWindow_Log.selectedMessage = value;
				if (UnityData.IsInMainThread && GUI.GetNameOfFocusedControl() == EditWindow_Log.MessageDetailsControlName)
				{
					UI.UnfocusCurrentControl();
				}
			}
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x000DDEFF File Offset: 0x000DC0FF
		public EditWindow_Log()
		{
			this.optionalTitle = "Debug log";
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x000DDF12 File Offset: 0x000DC112
		public static void TryAutoOpen()
		{
			if (EditWindow_Log.canAutoOpen)
			{
				EditWindow_Log.wantsToOpen = true;
			}
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x000DDF21 File Offset: 0x000DC121
		public static void ClearSelectedMessage()
		{
			EditWindow_Log.SelectedMessage = null;
			EditWindow_Log.detailsScrollPosition = Vector2.zero;
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000DDF33 File Offset: 0x000DC133
		public static void SelectLastMessage(bool expandDetailsPane = false)
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.SelectedMessage = Log.Messages.LastOrDefault<LogMessage>();
			EditWindow_Log.messagesScrollPosition.y = (float)Log.Messages.Count<LogMessage>() * 30f;
			if (expandDetailsPane)
			{
				EditWindow_Log.detailsPaneHeight = 9999f;
			}
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x000DDF71 File Offset: 0x000DC171
		public static void ClearAll()
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.messagesScrollPosition = Vector2.zero;
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x000DDF82 File Offset: 0x000DC182
		public override void PostClose()
		{
			base.PostClose();
			EditWindow_Log.wantsToOpen = false;
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x000DDF90 File Offset: 0x000DC190
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Clear", "Clear all log messages.", true, true, true, null))
			{
				Log.Clear();
				EditWindow_Log.ClearAll();
			}
			if (widgetRow.ButtonText("Trace big", "Set the stack trace to be large on screen.", true, true, true, null))
			{
				EditWindow_Log.detailsPaneHeight = 700f;
			}
			if (widgetRow.ButtonText("Trace medium", "Set the stack trace to be medium-sized on screen.", true, true, true, null))
			{
				EditWindow_Log.detailsPaneHeight = 300f;
			}
			if (widgetRow.ButtonText("Trace small", "Set the stack trace to be small on screen.", true, true, true, null))
			{
				EditWindow_Log.detailsPaneHeight = 100f;
			}
			if (EditWindow_Log.canAutoOpen)
			{
				if (widgetRow.ButtonText("Auto-open is ON", "", true, true, true, null))
				{
					EditWindow_Log.canAutoOpen = false;
				}
			}
			else if (widgetRow.ButtonText("Auto-open is OFF", "", true, true, true, null))
			{
				EditWindow_Log.canAutoOpen = true;
			}
			if (widgetRow.ButtonText("Copy to clipboard", "Copy all messages to the clipboard.", true, true, true, null))
			{
				this.CopyAllMessagesToClipboard();
			}
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect);
			rect.yMin += 26f;
			rect.yMax = inRect.height;
			if (EditWindow_Log.selectedMessage != null)
			{
				rect.yMax -= EditWindow_Log.detailsPaneHeight;
			}
			Rect detailsRect = new Rect(inRect);
			detailsRect.yMin = rect.yMax;
			this.DoMessagesListing(rect);
			this.DoMessageDetails(detailsRect, inRect);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				EditWindow_Log.ClearSelectedMessage();
			}
			EditWindow_Log.detailsPaneHeight = Mathf.Max(EditWindow_Log.detailsPaneHeight, 10f);
			EditWindow_Log.detailsPaneHeight = Mathf.Min(EditWindow_Log.detailsPaneHeight, inRect.height - 80f);
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000DE196 File Offset: 0x000DC396
		public static void Notify_MessageDequeued(LogMessage oldMessage)
		{
			if (EditWindow_Log.SelectedMessage == oldMessage)
			{
				EditWindow_Log.SelectedMessage = null;
			}
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000DE1A8 File Offset: 0x000DC3A8
		private void DoMessagesListing(Rect listingRect)
		{
			Rect viewRect = new Rect(0f, 0f, listingRect.width - 16f, this.listingViewHeight + 100f);
			Widgets.BeginScrollView(listingRect, ref EditWindow_Log.messagesScrollPosition, viewRect, true);
			float width = viewRect.width - 28f;
			Text.Font = GameFont.Tiny;
			float num = 0f;
			bool flag = false;
			foreach (LogMessage logMessage in Log.Messages)
			{
				string text = logMessage.text;
				if (text.Length > 1000)
				{
					text = text.Substring(0, 1000);
				}
				float num2 = Math.Min(Text.TinyFontSupported ? 30f : Text.LineHeight, Text.CalcHeight(text, width));
				GUI.color = new Color(1f, 1f, 1f, 0.7f);
				Widgets.Label(new Rect(4f, num, 28f, num2), logMessage.repeats.ToStringCached());
				Rect rect = new Rect(28f, num, width, num2);
				if (EditWindow_Log.selectedMessage == logMessage)
				{
					GUI.DrawTexture(rect, EditWindow_Log.SelectedMessageTex);
				}
				else if (flag)
				{
					GUI.DrawTexture(rect, EditWindow_Log.AltMessageTex);
				}
				if (Widgets.ButtonInvisible(rect, true))
				{
					EditWindow_Log.ClearSelectedMessage();
					EditWindow_Log.SelectedMessage = logMessage;
				}
				GUI.color = logMessage.Color;
				Widgets.Label(rect, text);
				num += num2;
				flag = !flag;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.listingViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.color = Color.white;
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000DE370 File Offset: 0x000DC570
		private void DoMessageDetails(Rect detailsRect, Rect outRect)
		{
			if (EditWindow_Log.selectedMessage == null)
			{
				return;
			}
			Rect rect = detailsRect;
			rect.height = 7f;
			Rect rect2 = detailsRect;
			rect2.yMin = rect.yMax;
			GUI.DrawTexture(rect, EditWindow_Log.StackTraceBorderTex);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
			{
				this.borderDragging = true;
				Event.current.Use();
			}
			if (this.borderDragging)
			{
				EditWindow_Log.detailsPaneHeight = outRect.height + Mathf.Round(3.5f) - Event.current.mousePosition.y;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				this.borderDragging = false;
			}
			GUI.DrawTexture(rect2, EditWindow_Log.StackTraceAreaTex);
			string text = EditWindow_Log.selectedMessage.text + "\n" + EditWindow_Log.selectedMessage.StackTrace;
			GUI.SetNextControlName(EditWindow_Log.MessageDetailsControlName);
			if (text.Length > 15000)
			{
				Widgets.LabelScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, false, true, true);
				return;
			}
			Widgets.TextAreaScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, true);
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000DE484 File Offset: 0x000DC684
		private void CopyAllMessagesToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LogMessage logMessage in Log.Messages)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine(logMessage.text);
				stringBuilder.Append(logMessage.StackTrace);
				if (stringBuilder[stringBuilder.Length - 1] != '\n')
				{
					stringBuilder.AppendLine();
				}
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x04001612 RID: 5650
		private static LogMessage selectedMessage = null;

		// Token: 0x04001613 RID: 5651
		private static Vector2 messagesScrollPosition;

		// Token: 0x04001614 RID: 5652
		private static Vector2 detailsScrollPosition;

		// Token: 0x04001615 RID: 5653
		private static float detailsPaneHeight = 100f;

		// Token: 0x04001616 RID: 5654
		private static bool canAutoOpen = true;

		// Token: 0x04001617 RID: 5655
		public static bool wantsToOpen = false;

		// Token: 0x04001618 RID: 5656
		private float listingViewHeight;

		// Token: 0x04001619 RID: 5657
		private bool borderDragging;

		// Token: 0x0400161A RID: 5658
		private const float CountWidth = 28f;

		// Token: 0x0400161B RID: 5659
		private const float Yinc = 25f;

		// Token: 0x0400161C RID: 5660
		private const float DetailsPaneBorderHeight = 7f;

		// Token: 0x0400161D RID: 5661
		private const float DetailsPaneMinHeight = 10f;

		// Token: 0x0400161E RID: 5662
		private const float ListingMinHeight = 80f;

		// Token: 0x0400161F RID: 5663
		private const float TopAreaHeight = 26f;

		// Token: 0x04001620 RID: 5664
		private const float MessageMaxHeight = 30f;

		// Token: 0x04001621 RID: 5665
		private static readonly Texture2D AltMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.17f, 0.17f, 0.17f, 0.85f));

		// Token: 0x04001622 RID: 5666
		private static readonly Texture2D SelectedMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.25f, 0.25f, 0.17f, 0.85f));

		// Token: 0x04001623 RID: 5667
		private static readonly Texture2D StackTraceAreaTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.5f));

		// Token: 0x04001624 RID: 5668
		private static readonly Texture2D StackTraceBorderTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.3f, 0.3f, 0.3f, 1f));

		// Token: 0x04001625 RID: 5669
		private static readonly string MessageDetailsControlName = "MessageDetailsTextArea";
	}
}

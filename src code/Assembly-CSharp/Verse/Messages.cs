using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004BD RID: 1213
	[StaticConstructorOnStartup]
	public static class Messages
	{
		// Token: 0x060024AF RID: 9391 RVA: 0x000E9BE0 File Offset: 0x000E7DE0
		public static void Update()
		{
			if (Current.ProgramState == ProgramState.Playing && Messages.mouseoverMessageIndex >= 0 && Messages.mouseoverMessageIndex < Messages.liveMessages.Count)
			{
				Messages.liveMessages[Messages.mouseoverMessageIndex].lookTargets.TryHighlight(true, true, false);
			}
			Messages.mouseoverMessageIndex = -1;
			Messages.liveMessages.RemoveAll((Message m) => m.Expired);
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000E9C5A File Offset: 0x000E7E5A
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, Quest quest, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, lookTargets))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def, lookTargets, quest), historical);
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000E9C7B File Offset: 0x000E7E7B
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, lookTargets))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def, lookTargets), historical);
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000E9C9A File Offset: 0x000E7E9A
		public static void Message(string text, MessageTypeDef def, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, TargetInfo.Invalid))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def), historical);
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000E9CC4 File Offset: 0x000E7EC4
		public static void Message(Message msg, bool historical = true)
		{
			if (!Messages.AcceptsMessage(msg.text, msg.lookTargets))
			{
				return;
			}
			if (historical && Find.Archive != null)
			{
				Find.Archive.Add(msg);
			}
			Messages.liveMessages.Add(msg);
			while (Messages.liveMessages.Count > 12)
			{
				Messages.liveMessages.RemoveAt(0);
			}
			if (msg.def.sound != null)
			{
				msg.def.sound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000E9D3F File Offset: 0x000E7F3F
		public static bool IsLive(Message msg)
		{
			return Messages.liveMessages.Contains(msg);
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000E9D4C File Offset: 0x000E7F4C
		public static void MessagesDoGUI()
		{
			Text.Font = GameFont.Small;
			int xOffset = (int)Messages.MessagesTopLeftStandard.x;
			int num = (int)Messages.MessagesTopLeftStandard.y;
			if (Current.Game != null && Find.ActiveLesson.ActiveLessonVisible)
			{
				num += (int)Find.ActiveLesson.Current.MessagesYOffset;
			}
			for (int i = Messages.liveMessages.Count - 1; i >= 0; i--)
			{
				Messages.liveMessages[i].Draw(xOffset, num);
				num += 26;
			}
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000E9DCC File Offset: 0x000E7FCC
		public static bool CollidesWithAnyMessage(Rect rect, out float messageAlpha)
		{
			bool result = false;
			float num = 0f;
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Message message = Messages.liveMessages[i];
				if (rect.Overlaps(message.lastDrawRect))
				{
					result = true;
					num = Mathf.Max(num, message.Alpha);
				}
			}
			messageAlpha = num;
			return result;
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000E9E24 File Offset: 0x000E8024
		public static void Clear()
		{
			Messages.liveMessages.Clear();
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000E9E30 File Offset: 0x000E8030
		public static void Notify_LoadedLevelChanged()
		{
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Messages.liveMessages[i].lookTargets = null;
			}
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x000E9E64 File Offset: 0x000E8064
		private static bool AcceptsMessage(string text, LookTargets lookTargets)
		{
			if (text.NullOrEmpty())
			{
				return false;
			}
			int i = 0;
			while (i < Messages.liveMessages.Count)
			{
				if (Messages.liveMessages[i].text == text && LookTargets.SameTargets(Messages.liveMessages[i].lookTargets, lookTargets))
				{
					if (Messages.liveMessages[i].startingFrame == RealTime.frameCount)
					{
						return false;
					}
					Messages.liveMessages[i].ResetTimer();
					Messages.liveMessages[i].Flash();
					Messages.liveMessages[i].def.sound.PlayOneShotOnCamera(null);
					return false;
				}
				else
				{
					i++;
				}
			}
			return true;
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000E9F1D File Offset: 0x000E811D
		public static void Notify_Mouseover(Message msg)
		{
			Messages.mouseoverMessageIndex = Messages.liveMessages.IndexOf(msg);
		}

		// Token: 0x04001787 RID: 6023
		private static List<Message> liveMessages = new List<Message>();

		// Token: 0x04001788 RID: 6024
		private static int mouseoverMessageIndex = -1;

		// Token: 0x04001789 RID: 6025
		public static readonly Vector2 MessagesTopLeftStandard = new Vector2(140f, 16f);

		// Token: 0x0400178A RID: 6026
		private const int MessageYInterval = 26;

		// Token: 0x0400178B RID: 6027
		private const int MaxLiveMessages = 12;
	}
}

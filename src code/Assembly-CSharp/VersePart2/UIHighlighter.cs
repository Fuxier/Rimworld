using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D0 RID: 1232
	[StaticConstructorOnStartup]
	public static class UIHighlighter
	{
		// Token: 0x0600252B RID: 9515 RVA: 0x000EC044 File Offset: 0x000EA244
		public static void HighlightTag(string tag)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (tag.NullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
			{
				if (UIHighlighter.liveTags[i].First == tag && UIHighlighter.liveTags[i].Second == Time.frameCount)
				{
					return;
				}
			}
			UIHighlighter.liveTags.Add(new Pair<string, int>(tag, Time.frameCount));
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000EC0C8 File Offset: 0x000EA2C8
		public static void HighlightOpportunity(Rect rect, string tag)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
			{
				Pair<string, int> pair = UIHighlighter.liveTags[i];
				if (tag == pair.First && Time.frameCount == pair.Second + 1)
				{
					Rect rect2 = rect.ContractedBy(-10f);
					GUI.color = new Color(1f, 1f, 1f, Pulser.PulseBrightness(1.2f, 0.7f));
					Widgets.DrawAtlas(rect2, UIHighlighter.TutorHighlightAtlas);
					GUI.color = Color.white;
				}
			}
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000EC16D File Offset: 0x000EA36D
		public static void UIHighlighterUpdate()
		{
			UIHighlighter.liveTags.RemoveAll((Pair<string, int> pair) => Time.frameCount > pair.Second + 1);
		}

		// Token: 0x040017CD RID: 6093
		private static List<Pair<string, int>> liveTags = new List<Pair<string, int>>();

		// Token: 0x040017CE RID: 6094
		private const float PulseFrequency = 1.2f;

		// Token: 0x040017CF RID: 6095
		private const float PulseAmplitude = 0.7f;

		// Token: 0x040017D0 RID: 6096
		private static readonly Texture2D TutorHighlightAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TutorHighlightAtlas", true);
	}
}

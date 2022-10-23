using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004A9 RID: 1193
	public abstract class Letter : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool CanShowInLetterStack
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060023E5 RID: 9189 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool CanDismissWithRightClick
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000E58C0 File Offset: 0x000E3AC0
		public bool ArchivedOnly
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060023E7 RID: 9191 RVA: 0x000E58D5 File Offset: 0x000E3AD5
		public IThingHolder ParentHolder
		{
			get
			{
				return Find.World;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060023E8 RID: 9192 RVA: 0x000E58DC File Offset: 0x000E3ADC
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return this.def.Icon;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060023E9 RID: 9193 RVA: 0x000E58E9 File Offset: 0x000E3AE9
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return this.def.color;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x000E58F6 File Offset: 0x000E3AF6
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x000E5903 File Offset: 0x000E3B03
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.GetMouseoverText();
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x000E590B File Offset: 0x000E3B0B
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.arrivalTick;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x000E58C0 File Offset: 0x000E3AC0
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x000E5913 File Offset: 0x000E3B13
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060023EF RID: 9199 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool ShouldAutomaticallyOpenLetter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x000E591B File Offset: 0x000E3B1B
		// (set) Token: 0x060023F1 RID: 9201 RVA: 0x000E5923 File Offset: 0x000E3B23
		public TaggedString Label
		{
			get
			{
				return this.label;
			}
			set
			{
				this.label = value.CapitalizeFirst();
			}
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000E5934 File Offset: 0x000E3B34
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Defs.Look<LetterDef>(ref this.def, "def");
			Scribe_Values.Look<TaggedString>(ref this.label, "label", default(TaggedString), false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.arrivalTick, "arrivalTick", 0, false);
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000E59B8 File Offset: 0x000E3BB8
		public virtual void DrawButtonAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			Rect rect = new Rect(num, topY, 38f, 30f);
			Rect rect2 = new Rect(rect);
			float num2 = Time.time - this.arrivalTime;
			Color color = this.def.color;
			if (num2 < 1f)
			{
				rect2.y -= (1f - num2) * 200f;
				color.a = num2 / 1f;
			}
			if (!Mouse.IsOver(rect) && this.def.bounce && num2 > 15f && num2 % 5f < 1f)
			{
				float num3 = (float)UI.screenWidth * 0.06f;
				float num4 = 2f * (num2 % 1f) - 1f;
				float num5 = num3 * (1f - num4 * num4);
				rect2.x -= num5;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (this.def.flashInterval > 0f)
				{
					float num6 = Time.time - (this.arrivalTime + 1f);
					if (num6 > 0f && num6 % this.def.flashInterval < 1f)
					{
						GenUI.DrawFlash(num, topY, (float)UI.screenWidth * 0.6f, Pulser.PulseBrightness(1f, 1f, num6) * 0.55f, this.def.flashColor);
					}
				}
				GUI.color = color;
				Widgets.DrawShadowAround(rect2);
				GUI.DrawTexture(rect2, this.def.Icon);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperRight;
				string text = this.PostProcessedLabel();
				Vector2 vector = Text.CalcSize(text);
				float x = vector.x;
				float y = vector.y;
				Vector2 vector2 = new Vector2(rect2.x + rect2.width / 2f, rect2.center.y - y / 2f + 4f);
				float num7 = vector2.x + x / 2f - (float)(UI.screenWidth - 2);
				if (num7 > 0f)
				{
					vector2.x -= num7;
				}
				GUI.DrawTexture(new Rect(vector2.x - x / 2f - 6f - 1f, vector2.y, x + 12f, 16f), TexUI.GrayTextBG);
				GUI.color = new Color(1f, 1f, 1f, 0.75f);
				Rect rect3 = new Rect(vector2.x - x / 2f, vector2.y - 3f, x, 999f);
				Text.WordWrap = false;
				Widgets.Label(rect3, text);
				Text.WordWrap = true;
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (this.CanDismissWithRightClick && Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.LetterStack.RemoveLetter(this);
				Event.current.Use();
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.OpenLetter();
				Event.current.Use();
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x000E5CF0 File Offset: 0x000E3EF0
		public virtual void CheckForMouseOverTextAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			if (Mouse.IsOver(new Rect(num, topY, 38f, 30f)))
			{
				Find.LetterStack.Notify_LetterMouseover(this);
				TaggedString mouseoverText = this.GetMouseoverText();
				if (!mouseoverText.Resolve().NullOrEmpty())
				{
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					float num2 = Text.CalcHeight(mouseoverText, 310f);
					num2 += 20f;
					float x = num - 330f - 10f;
					float y = Mathf.Max(topY - num2 / 2f, 0f);
					Rect infoRect = new Rect(x, y, 330f, num2);
					Find.WindowStack.ImmediateWindow(2768333, infoRect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Small;
						Rect rect = infoRect.AtZero().ContractedBy(10f);
						Widgets.BeginGroup(rect);
						Widgets.Label(new Rect(0f, 0f, rect.width, rect.height), mouseoverText.Resolve());
						Widgets.EndGroup();
					}, true, false, 1f, null);
				}
			}
		}

		// Token: 0x060023F5 RID: 9205
		protected abstract string GetMouseoverText();

		// Token: 0x060023F6 RID: 9206
		public abstract void OpenLetter();

		// Token: 0x060023F7 RID: 9207 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Received()
		{
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Removed()
		{
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000E58F6 File Offset: 0x000E3AF6
		protected virtual string PostProcessedLabel()
		{
			return this.label;
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000E5E01 File Offset: 0x000E4001
		void IArchivable.OpenArchived()
		{
			this.OpenLetter();
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x000E5E09 File Offset: 0x000E4009
		public string GetUniqueLoadID()
		{
			return "Letter_" + this.ID;
		}

		// Token: 0x04001728 RID: 5928
		public int ID;

		// Token: 0x04001729 RID: 5929
		public LetterDef def;

		// Token: 0x0400172A RID: 5930
		private TaggedString label;

		// Token: 0x0400172B RID: 5931
		public LookTargets lookTargets;

		// Token: 0x0400172C RID: 5932
		public Faction relatedFaction;

		// Token: 0x0400172D RID: 5933
		public int arrivalTick;

		// Token: 0x0400172E RID: 5934
		public float arrivalTime;

		// Token: 0x0400172F RID: 5935
		public string debugInfo;

		// Token: 0x04001730 RID: 5936
		public const float DrawWidth = 38f;

		// Token: 0x04001731 RID: 5937
		public const float DrawHeight = 30f;

		// Token: 0x04001732 RID: 5938
		private const float FallTime = 1f;

		// Token: 0x04001733 RID: 5939
		private const float FallDistance = 200f;
	}
}

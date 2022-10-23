using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004BC RID: 1212
	public class Message : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002498 RID: 9368 RVA: 0x000E988A File Offset: 0x000E7A8A
		protected float Age
		{
			get
			{
				return RealTime.LastRealTime - this.startingTime;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x000E9898 File Offset: 0x000E7A98
		protected float TimeLeft
		{
			get
			{
				return 13f - this.Age;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x0600249A RID: 9370 RVA: 0x000E98A6 File Offset: 0x000E7AA6
		public bool Expired
		{
			get
			{
				return this.TimeLeft <= 0f;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x000E98B8 File Offset: 0x000E7AB8
		public float Alpha
		{
			get
			{
				if (this.TimeLeft < 0.6f)
				{
					return this.TimeLeft / 0.6f;
				}
				return 1f;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x0600249C RID: 9372 RVA: 0x000E98DC File Offset: 0x000E7ADC
		private static bool ShouldDrawBackground
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return true;
				}
				WindowStack windowStack = Find.WindowStack;
				for (int i = 0; i < windowStack.Count; i++)
				{
					if (windowStack[i].CausesMessageBackground())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x000029B0 File Offset: 0x00000BB0
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600249E RID: 9374 RVA: 0x00020495 File Offset: 0x0001E695
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x000E991B File Offset: 0x000E7B1B
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060024A0 RID: 9376 RVA: 0x000E9928 File Offset: 0x000E7B28
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x000E9930 File Offset: 0x000E7B30
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.startingTick;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060024A2 RID: 9378 RVA: 0x000E9938 File Offset: 0x000E7B38
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Messages.IsLive(this);
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060024A3 RID: 9379 RVA: 0x000E9943 File Offset: 0x000E7B43
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000E994B File Offset: 0x000E7B4B
		public Message()
		{
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000E9968 File Offset: 0x000E7B68
		public Message(string text, MessageTypeDef def)
		{
			this.text = text;
			this.def = def;
			this.ResetTimer();
			if (Find.UniqueIDsManager != null)
			{
				this.ID = Find.UniqueIDsManager.GetNextMessageID();
				return;
			}
			this.ID = Rand.Int;
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000E99C7 File Offset: 0x000E7BC7
		public Message(string text, MessageTypeDef def, LookTargets lookTargets) : this(text, def)
		{
			this.lookTargets = lookTargets;
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x000E99D8 File Offset: 0x000E7BD8
		public Message(string text, MessageTypeDef def, LookTargets lookTargets, Quest quest) : this(text, def, lookTargets)
		{
			this.quest = quest;
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x000E99EC File Offset: 0x000E7BEC
		public void ExposeData()
		{
			Scribe_Defs.Look<MessageTypeDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<float>(ref this.startingTime, "startingTime", 0f, false);
			Scribe_Values.Look<int>(ref this.startingFrame, "startingFrame", 0, false);
			Scribe_Values.Look<int>(ref this.startingTick, "startingTick", 0, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000E9A90 File Offset: 0x000E7C90
		public Rect CalculateRect(float x, float y)
		{
			Text.Font = GameFont.Small;
			if (this.cachedSize.x < 0f)
			{
				this.cachedSize = Text.CalcSize(this.text);
			}
			this.lastDrawRect = new Rect(x, y, this.cachedSize.x, this.cachedSize.y);
			this.lastDrawRect = this.lastDrawRect.ContractedBy(-2f);
			return this.lastDrawRect;
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000E9B08 File Offset: 0x000E7D08
		public void Draw(int xOffset, int yOffset)
		{
			Rect rect = this.CalculateRect((float)xOffset, (float)yOffset);
			Find.WindowStack.ImmediateWindow(Gen.HashCombineInt(this.ID, 45574281), rect, WindowLayer.Super, delegate
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect = rect.AtZero();
				float alpha = this.Alpha;
				GUI.color = new Color(1f, 1f, 1f, alpha);
				if (Message.ShouldDrawBackground)
				{
					GUI.color = new Color(0.15f, 0.15f, 0.15f, 0.8f * alpha);
					GUI.DrawTexture(rect, BaseContent.WhiteTex);
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()) || (this.quest != null && !this.quest.hidden))
				{
					UIHighlighter.HighlightOpportunity(rect, "Messages");
					Widgets.DrawHighlightIfMouseover(rect);
				}
				if (this.flashTime + 0.6f > RealTime.LastRealTime)
				{
					float transparency = 1f - (RealTime.LastRealTime - this.flashTime) / 0.6f;
					Widgets.DrawTextHighlight(rect, 4f, new Color?(ColorLibrary.SkyBlue.ToTransparent(transparency)));
				}
				Widgets.Label(new Rect(2f, 0f, rect.width - 2f, rect.height), this.text);
				if (Current.ProgramState == ProgramState.Playing && Widgets.ButtonInvisible(rect, true))
				{
					if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()))
					{
						CameraJumper.TryJumpAndSelect(this.lookTargets.TryGetPrimaryTarget(), CameraJumper.MovementMode.Pan);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ClickingMessages, KnowledgeAmount.Total);
					}
					else if (this.quest != null && !this.quest.hidden)
					{
						if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Quests)
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
						else
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
						}
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				if (Mouse.IsOver(rect))
				{
					Messages.Notify_Mouseover(this);
				}
			}, false, false, 0f, null);
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000E9B68 File Offset: 0x000E7D68
		void IArchivable.OpenArchived()
		{
			Find.WindowStack.Add(new Dialog_MessageBox(this.text, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000E9B98 File Offset: 0x000E7D98
		public void ResetTimer()
		{
			this.startingFrame = RealTime.frameCount;
			this.startingTime = RealTime.LastRealTime;
			this.startingTick = GenTicks.TicksGame;
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000E9BBB File Offset: 0x000E7DBB
		public void Flash()
		{
			this.flashTime = RealTime.LastRealTime;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000E9BC8 File Offset: 0x000E7DC8
		public string GetUniqueLoadID()
		{
			return "Message_" + this.ID;
		}

		// Token: 0x04001779 RID: 6009
		public MessageTypeDef def;

		// Token: 0x0400177A RID: 6010
		private int ID;

		// Token: 0x0400177B RID: 6011
		public string text;

		// Token: 0x0400177C RID: 6012
		private float startingTime;

		// Token: 0x0400177D RID: 6013
		public int startingFrame;

		// Token: 0x0400177E RID: 6014
		public int startingTick;

		// Token: 0x0400177F RID: 6015
		private float flashTime;

		// Token: 0x04001780 RID: 6016
		public LookTargets lookTargets;

		// Token: 0x04001781 RID: 6017
		public Quest quest;

		// Token: 0x04001782 RID: 6018
		private Vector2 cachedSize = new Vector2(-1f, -1f);

		// Token: 0x04001783 RID: 6019
		public Rect lastDrawRect;

		// Token: 0x04001784 RID: 6020
		private const float DefaultMessageLifespan = 13f;

		// Token: 0x04001785 RID: 6021
		private const float FadeoutDuration = 0.6f;

		// Token: 0x04001786 RID: 6022
		private const float FlashDuration = 0.6f;
	}
}

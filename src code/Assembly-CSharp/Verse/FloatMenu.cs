using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000481 RID: 1153
	public class FloatMenu : Window
	{
		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060022E1 RID: 8929 RVA: 0x00004E2A File Offset: 0x0000302A
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060022E2 RID: 8930 RVA: 0x000DF29C File Offset: 0x000DD49C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalWindowHeight);
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x000DF2AF File Offset: 0x000DD4AF
		private float MaxWindowHeight
		{
			get
			{
				return (float)UI.screenHeight * 0.9f;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060022E4 RID: 8932 RVA: 0x000DF2BD File Offset: 0x000DD4BD
		private float TotalWindowHeight
		{
			get
			{
				return Mathf.Min(this.TotalViewHeight, this.MaxWindowHeight) + 1f;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060022E5 RID: 8933 RVA: 0x000DF2D8 File Offset: 0x000DD4D8
		private float MaxViewHeight
		{
			get
			{
				if (this.UsingScrollbar)
				{
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < this.options.Count; i++)
					{
						float requiredHeight = this.options[i].RequiredHeight;
						if (requiredHeight > num)
						{
							num = requiredHeight;
						}
						num2 += requiredHeight + -1f;
					}
					int columnCount = this.ColumnCount;
					num2 += (float)columnCount * num;
					return num2 / (float)columnCount;
				}
				return this.MaxWindowHeight;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x000DF350 File Offset: 0x000DD550
		private float TotalViewHeight
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				float maxViewHeight = this.MaxViewHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1f > maxViewHeight)
					{
						if (num2 > num)
						{
							num = num2;
						}
						num2 = requiredHeight;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return Mathf.Max(num, num2);
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x000DF3C4 File Offset: 0x000DD5C4
		private float TotalWidth
		{
			get
			{
				float num = (float)this.ColumnCount * this.ColumnWidth;
				if (this.UsingScrollbar)
				{
					num += 16f;
				}
				return num;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060022E8 RID: 8936 RVA: 0x000DF3F4 File Offset: 0x000DD5F4
		private float ColumnWidth
		{
			get
			{
				float num = 70f;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredWidth = this.options[i].RequiredWidth;
					if (requiredWidth >= 300f)
					{
						return 300f;
					}
					if (requiredWidth > num)
					{
						num = requiredWidth;
					}
				}
				return Mathf.Round(num);
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060022E9 RID: 8937 RVA: 0x000DF449 File Offset: 0x000DD649
		private int MaxColumns
		{
			get
			{
				return Mathf.FloorToInt(((float)UI.screenWidth - 16f) / this.ColumnWidth);
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060022EA RID: 8938 RVA: 0x000DF463 File Offset: 0x000DD663
		private bool UsingScrollbar
		{
			get
			{
				return this.ColumnCountIfNoScrollbar > this.MaxColumns;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060022EB RID: 8939 RVA: 0x000DF473 File Offset: 0x000DD673
		private int ColumnCount
		{
			get
			{
				return Mathf.Min(this.ColumnCountIfNoScrollbar, this.MaxColumns);
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x000DF488 File Offset: 0x000DD688
		private int ColumnCountIfNoScrollbar
		{
			get
			{
				if (this.options == null)
				{
					return 1;
				}
				Text.Font = GameFont.Small;
				int num = 1;
				float num2 = 0f;
				float maxWindowHeight = this.MaxWindowHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1f > maxWindowHeight)
					{
						num2 = requiredHeight;
						num++;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return num;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060022ED RID: 8941 RVA: 0x000DF4FF File Offset: 0x000DD6FF
		public FloatMenuSizeMode SizeMode
		{
			get
			{
				if (this.options.Count > 60)
				{
					return FloatMenuSizeMode.Tiny;
				}
				return FloatMenuSizeMode.Normal;
			}
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x000DF514 File Offset: 0x000DD714
		public FloatMenu(List<FloatMenuOption> options)
		{
			if (options.NullOrEmpty<FloatMenuOption>())
			{
				Log.Error("Created FloatMenu with no options. Closing.");
				this.Close(true);
			}
			this.options = (from op in options
			orderby op.Priority descending, op.orderInPriority descending
			select op).ToList<FloatMenuOption>();
			for (int i = 0; i < options.Count; i++)
			{
				options[i].SetSizeMode(this.SizeMode);
			}
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x000DF5F7 File Offset: 0x000DD7F7
		public FloatMenu(List<FloatMenuOption> options, string title, bool needSelection = false) : this(options)
		{
			this.title = title;
			this.needSelection = needSelection;
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x000DF610 File Offset: 0x000DD810
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + FloatMenu.InitialPositionShift;
			if (vector.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				vector.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (vector.y + this.InitialSize.y > (float)UI.screenHeight)
			{
				vector.y = (float)UI.screenHeight - this.InitialSize.y;
			}
			this.windowRect = new Rect(vector.x, vector.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x000DF6C0 File Offset: 0x000DD8C0
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (!this.title.NullOrEmpty())
			{
				Vector2 vector = new Vector2(this.windowRect.x, this.windowRect.y);
				Text.Font = GameFont.Small;
				float width = Mathf.Max(150f, 15f + Text.CalcSize(this.title).x);
				Rect titleRect = new Rect(vector.x + FloatMenu.TitleOffset.x, vector.y + FloatMenu.TitleOffset.y, width, 23f);
				Find.WindowStack.ImmediateWindow(6830963, titleRect, WindowLayer.Super, delegate
				{
					GUI.color = this.baseColor;
					Text.Font = GameFont.Small;
					Rect position = titleRect.AtZero();
					position.width = 150f;
					GUI.DrawTexture(position, TexUI.TextBGBlack);
					Rect rect = titleRect.AtZero();
					rect.x += 15f;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect, this.title);
					Text.Anchor = TextAnchor.UpperLeft;
				}, false, false, 0f, null);
			}
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x000DF794 File Offset: 0x000DD994
		public override void DoWindowContents(Rect rect)
		{
			if (this.needSelection && Find.Selector.SingleSelectedThing == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			this.UpdateBaseColor();
			bool usingScrollbar = this.UsingScrollbar;
			GUI.color = this.baseColor;
			Text.Font = GameFont.Small;
			Vector2 zero = Vector2.zero;
			float maxViewHeight = this.MaxViewHeight;
			float columnWidth = this.ColumnWidth;
			if (usingScrollbar)
			{
				rect.width -= 10f;
				Widgets.BeginScrollView(rect, ref this.scrollPosition, new Rect(0f, 0f, this.TotalWidth - 16f, this.TotalViewHeight), true);
			}
			for (int i = 0; i < this.options.Count; i++)
			{
				FloatMenuOption floatMenuOption = this.options[i];
				float requiredHeight = floatMenuOption.RequiredHeight;
				if (zero.y + requiredHeight + -1f > maxViewHeight)
				{
					zero.y = 0f;
					zero.x += columnWidth + -1f;
				}
				Rect rect2 = new Rect(zero.x, zero.y, columnWidth, requiredHeight);
				zero.y += requiredHeight + -1f;
				if (floatMenuOption.DoGUI(rect2, this.givesColonistOrders, this))
				{
					Find.WindowStack.TryRemove(this, true);
					break;
				}
			}
			if (usingScrollbar)
			{
				Widgets.EndScrollView();
			}
			if (Event.current.type == EventType.MouseDown)
			{
				Event.current.Use();
				this.Close(true);
			}
			GUI.color = Color.white;
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x000DF915 File Offset: 0x000DDB15
		public override void PostClose()
		{
			base.PostClose();
			if (this.onCloseCallback != null)
			{
				this.onCloseCallback();
			}
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x000DF930 File Offset: 0x000DDB30
		public void Cancel()
		{
			SoundDefOf.FloatMenu_Cancel.PlayOneShotOnCamera(null);
			Find.WindowStack.TryRemove(this, true);
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PreOptionChosen(FloatMenuOption opt)
		{
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x000DF94C File Offset: 0x000DDB4C
		private void UpdateBaseColor()
		{
			this.baseColor = Color.white;
			if (this.vanishIfMouseDistant)
			{
				Rect r = new Rect(0f, 0f, this.TotalWidth, this.TotalWindowHeight).ContractedBy(-5f);
				if (!r.Contains(Event.current.mousePosition))
				{
					float num = GenUI.DistFromRect(r, Event.current.mousePosition);
					this.baseColor = new Color(1f, 1f, 1f, 1f - num / 95f);
					if (num > 95f)
					{
						this.Close(false);
						this.Cancel();
						return;
					}
				}
			}
		}

		// Token: 0x04001638 RID: 5688
		public bool givesColonistOrders;

		// Token: 0x04001639 RID: 5689
		public bool vanishIfMouseDistant = true;

		// Token: 0x0400163A RID: 5690
		public Action onCloseCallback;

		// Token: 0x0400163B RID: 5691
		protected List<FloatMenuOption> options;

		// Token: 0x0400163C RID: 5692
		private string title;

		// Token: 0x0400163D RID: 5693
		private bool needSelection;

		// Token: 0x0400163E RID: 5694
		private Color baseColor = Color.white;

		// Token: 0x0400163F RID: 5695
		private Vector2 scrollPosition;

		// Token: 0x04001640 RID: 5696
		private static readonly Vector2 TitleOffset = new Vector2(30f, -25f);

		// Token: 0x04001641 RID: 5697
		private const float OptionSpacing = -1f;

		// Token: 0x04001642 RID: 5698
		private const float MaxScreenHeightPercent = 0.9f;

		// Token: 0x04001643 RID: 5699
		private const float MinimumColumnWidth = 70f;

		// Token: 0x04001644 RID: 5700
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);

		// Token: 0x04001645 RID: 5701
		public const float FadeStartMouseDist = 5f;

		// Token: 0x04001646 RID: 5702
		private const float FadeFinishMouseDist = 100f;

		// Token: 0x04001647 RID: 5703
		public const float FinishDistFromStartDist = 95f;
	}
}

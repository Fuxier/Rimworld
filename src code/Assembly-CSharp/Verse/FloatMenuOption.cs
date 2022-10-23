using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000486 RID: 1158
	[StaticConstructorOnStartup]
	public class FloatMenuOption
	{
		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x000E025D File Offset: 0x000DE45D
		private static float NormalVerticalMargin
		{
			get
			{
				return (float)(SteamDeck.IsSteamDeck ? 10 : 4);
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600230F RID: 8975 RVA: 0x000E026C File Offset: 0x000DE46C
		// (set) Token: 0x06002310 RID: 8976 RVA: 0x000E0274 File Offset: 0x000DE474
		public string Label
		{
			get
			{
				return this.labelInt;
			}
			set
			{
				if (value.NullOrEmpty())
				{
					value = "(missing label)";
				}
				this.labelInt = value.TrimEnd(Array.Empty<char>());
				this.SetSizeMode(this.sizeMode);
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002311 RID: 8977 RVA: 0x000E02A2 File Offset: 0x000DE4A2
		private float VerticalMargin
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return 1f;
				}
				return FloatMenuOption.NormalVerticalMargin;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x000E02B8 File Offset: 0x000DE4B8
		private float HorizontalMargin
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return 3f;
				}
				return 6f;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002313 RID: 8979 RVA: 0x000E02CE File Offset: 0x000DE4CE
		private float IconOffset
		{
			get
			{
				if (this.shownItem == null && !this.drawPlaceHolderIcon && !(this.itemIcon != null) && this.iconThing == null)
				{
					return 0f;
				}
				return 27f;
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x000E0301 File Offset: 0x000DE501
		private GameFont CurrentFont
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return GameFont.Tiny;
				}
				return GameFont.Small;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002315 RID: 8981 RVA: 0x000E030F File Offset: 0x000DE50F
		// (set) Token: 0x06002316 RID: 8982 RVA: 0x000E031A File Offset: 0x000DE51A
		public bool Disabled
		{
			get
			{
				return this.action == null;
			}
			set
			{
				if (value)
				{
					this.action = null;
				}
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002317 RID: 8983 RVA: 0x000E0326 File Offset: 0x000DE526
		public float RequiredHeight
		{
			get
			{
				return this.cachedRequiredHeight;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x000E032E File Offset: 0x000DE52E
		public float RequiredWidth
		{
			get
			{
				return this.cachedRequiredWidth;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002319 RID: 8985 RVA: 0x000E0336 File Offset: 0x000DE536
		// (set) Token: 0x0600231A RID: 8986 RVA: 0x000E0348 File Offset: 0x000DE548
		public MenuOptionPriority Priority
		{
			get
			{
				if (this.Disabled)
				{
					return MenuOptionPriority.DisabledOption;
				}
				return this.priorityInt;
			}
			set
			{
				if (this.Disabled)
				{
					Log.Error("Setting priority on disabled FloatMenuOption: " + this.Label);
				}
				this.priorityInt = value;
			}
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x000E0370 File Offset: 0x000DE570
		public FloatMenuOption(string label, Action action, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0)
		{
			this.Label = label;
			this.action = action;
			this.priorityInt = priority;
			this.revalidateClickTarget = revalidateClickTarget;
			this.mouseoverGuiAction = mouseoverGuiAction;
			this.extraPartWidth = extraPartWidth;
			this.extraPartOnGUI = extraPartOnGUI;
			this.revalidateWorldClickTarget = revalidateWorldClickTarget;
			this.playSelectionSound = playSelectionSound;
			this.orderInPriority = orderInPriority;
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x000E0408 File Offset: 0x000DE608
		public FloatMenuOption(string label, Action action, ThingDef shownItemForIcon, ThingStyleDef thingStyle = null, bool forceBasicStyle = false, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0, int? graphicIndexOverride = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.shownItem = shownItemForIcon;
			this.thingStyle = thingStyle;
			this.forceBasicStyle = forceBasicStyle;
			this.graphicIndexOverride = graphicIndexOverride;
			if (shownItemForIcon == null)
			{
				this.drawPlaceHolderIcon = true;
			}
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x000E0458 File Offset: 0x000DE658
		public FloatMenuOption(string label, Action action, Texture2D itemIcon, Color iconColor, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0, HorizontalJustification iconJustification = HorizontalJustification.Left, bool extraPartRightJustified = false) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.itemIcon = itemIcon;
			this.iconColor = iconColor;
			this.iconJustification = iconJustification;
			this.extraPartRightJustified = extraPartRightJustified;
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x000E049C File Offset: 0x000DE69C
		public FloatMenuOption(string label, Action action, Thing iconThing, Color iconColor, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.iconThing = iconThing;
			this.iconColor = iconColor;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x000E04D0 File Offset: 0x000DE6D0
		public static FloatMenuOption CheckboxLabeled(string label, Action checkboxStateChanged, bool currentState)
		{
			return new FloatMenuOption(label, checkboxStateChanged, Widgets.GetCheckboxTexture(currentState), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Right, false);
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x000E0500 File Offset: 0x000DE700
		public void SetSizeMode(FloatMenuSizeMode newSizeMode)
		{
			this.sizeMode = newSizeMode;
			GameFont font = Text.Font;
			Text.Font = this.CurrentFont;
			float width = 300f - (2f * this.HorizontalMargin + 4f + this.extraPartWidth + this.IconOffset);
			this.cachedRequiredHeight = 2f * this.VerticalMargin + Text.CalcHeight(this.Label, width);
			this.cachedRequiredWidth = this.HorizontalMargin + 4f + Text.CalcSize(this.Label).x + this.extraPartWidth + this.HorizontalMargin + this.IconOffset + 4f;
			Text.Font = font;
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x000E05B0 File Offset: 0x000DE7B0
		public void Chosen(bool colonistOrdering, FloatMenu floatMenu)
		{
			if (floatMenu != null)
			{
				floatMenu.PreOptionChosen(this);
			}
			if (!this.Disabled)
			{
				if (this.action != null)
				{
					if (colonistOrdering && this.playSelectionSound)
					{
						SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
					}
					this.action();
					return;
				}
			}
			else if (this.playSelectionSound)
			{
				SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x000E060C File Offset: 0x000DE80C
		public virtual bool DoGUI(Rect rect, bool colonistOrdering, FloatMenu floatMenu)
		{
			Rect rect2 = rect;
			float height = rect2.height;
			rect2.height = height - 1f;
			bool flag = !this.Disabled && Mouse.IsOver(rect2);
			bool flag2 = false;
			Text.Font = this.CurrentFont;
			if (this.tooltip != null)
			{
				TooltipHandler.TipRegion(rect, this.tooltip.Value);
			}
			Rect rect3 = rect;
			if (this.iconJustification == HorizontalJustification.Left)
			{
				rect3.xMin += 4f;
				rect3.xMax = rect.x + 27f;
				rect3.yMin += 4f;
				rect3.yMax = rect.y + 27f;
				if (flag)
				{
					rect3.x += 4f;
				}
			}
			Rect rect4 = rect;
			rect4.xMin += this.HorizontalMargin;
			rect4.xMax -= this.HorizontalMargin;
			rect4.xMax -= 4f;
			rect4.xMax -= this.extraPartWidth + this.IconOffset;
			if (this.iconJustification == HorizontalJustification.Left)
			{
				rect4.x += this.IconOffset;
			}
			if (flag)
			{
				rect4.x += 4f;
			}
			float num = Mathf.Min(Text.CalcSize(this.Label).x, rect4.width - 4f);
			float num2 = rect4.xMin + num;
			if (this.iconJustification == HorizontalJustification.Right)
			{
				rect3.x = num2 + 4f;
				rect3.width = 27f;
				rect3.yMin += 4f;
				rect3.yMax = rect.y + 27f;
				num2 += 27f;
			}
			Rect rect5 = default(Rect);
			if (this.extraPartWidth != 0f)
			{
				if (this.extraPartRightJustified)
				{
					num2 = rect.xMax - this.extraPartWidth;
				}
				rect5 = new Rect(num2, rect4.yMin, this.extraPartWidth, 30f);
				flag2 = Mouse.IsOver(rect5);
			}
			if (!this.Disabled)
			{
				MouseoverSounds.DoRegion(rect2);
			}
			Color color = GUI.color;
			if (this.Disabled)
			{
				GUI.color = FloatMenuOption.ColorBGDisabled * color;
			}
			else if (flag && !flag2)
			{
				GUI.color = FloatMenuOption.ColorBGActiveMouseover * color;
			}
			else
			{
				GUI.color = FloatMenuOption.ColorBGActive * color;
			}
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = ((!this.Disabled) ? FloatMenuOption.ColorTextActive : FloatMenuOption.ColorTextDisabled) * color;
			if (this.sizeMode == FloatMenuSizeMode.Tiny)
			{
				rect4.y += 1f;
			}
			Widgets.DrawAtlas(rect, TexUI.FloatMenuOptionBG);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect4, this.Label);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(this.iconColor.r, this.iconColor.g, this.iconColor.b, this.iconColor.a * GUI.color.a);
			if (this.shownItem != null || this.drawPlaceHolderIcon)
			{
				ThingStyleDef thingStyleDef;
				if ((thingStyleDef = this.thingStyle) == null)
				{
					if (this.shownItem == null || Find.World == null)
					{
						thingStyleDef = null;
					}
					else
					{
						FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
						if (ideos == null)
						{
							thingStyleDef = null;
						}
						else
						{
							Ideo primaryIdeo = ideos.PrimaryIdeo;
							thingStyleDef = ((primaryIdeo != null) ? primaryIdeo.GetStyleFor(this.shownItem) : null);
						}
					}
				}
				ThingStyleDef thingStyleDef2 = thingStyleDef;
				if (this.forceBasicStyle)
				{
					thingStyleDef2 = null;
				}
				Color value;
				if (this.forceThingColor != null)
				{
					value = this.forceThingColor.Value;
				}
				else if (this.shownItem != null)
				{
					value = (this.shownItem.MadeFromStuff ? this.shownItem.GetColorForStuff(GenStuff.DefaultStuffFor(this.shownItem)) : this.shownItem.uiIconColor);
				}
				else
				{
					value = Color.white;
				}
				value.a *= color.a;
				Widgets.DefIcon(rect3, this.shownItem, null, 1f, thingStyleDef2, this.drawPlaceHolderIcon, new Color?(value), null, this.graphicIndexOverride);
			}
			else if (this.itemIcon)
			{
				Widgets.DrawTextureFitted(rect3, this.itemIcon, 1f, new Vector2(1f, 1f), this.iconTexCoords, 0f, null);
			}
			else if (this.iconThing != null)
			{
				if (this.iconThing is Pawn)
				{
					rect3.xMax -= 4f;
					rect3.yMax -= 4f;
				}
				Widgets.ThingIcon(rect3, this.iconThing, 1f, null, false);
			}
			GUI.color = color;
			if (this.extraPartOnGUI != null)
			{
				bool flag3 = this.extraPartOnGUI(rect5);
				GUI.color = color;
				if (flag3)
				{
					return true;
				}
			}
			if (flag && this.mouseoverGuiAction != null)
			{
				this.mouseoverGuiAction(rect);
			}
			if (this.tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.tutorTag);
			}
			if (!Widgets.ButtonInvisible(rect2, true))
			{
				return false;
			}
			if (this.tutorTag != null && !TutorSystem.AllowAction(this.tutorTag))
			{
				return false;
			}
			this.Chosen(colonistOrdering, floatMenu);
			if (this.tutorTag != null)
			{
				TutorSystem.Notify_Event(this.tutorTag);
			}
			return true;
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x000E0B74 File Offset: 0x000DED74
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"FloatMenuOption(",
				this.Label,
				", ",
				this.Disabled ? "disabled" : "enabled",
				")"
			});
		}

		// Token: 0x04001663 RID: 5731
		private string labelInt;

		// Token: 0x04001664 RID: 5732
		public Action action;

		// Token: 0x04001665 RID: 5733
		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		// Token: 0x04001666 RID: 5734
		public int orderInPriority;

		// Token: 0x04001667 RID: 5735
		public bool autoTakeable;

		// Token: 0x04001668 RID: 5736
		public float autoTakeablePriority;

		// Token: 0x04001669 RID: 5737
		public Action<Rect> mouseoverGuiAction;

		// Token: 0x0400166A RID: 5738
		public Thing revalidateClickTarget;

		// Token: 0x0400166B RID: 5739
		public WorldObject revalidateWorldClickTarget;

		// Token: 0x0400166C RID: 5740
		public float extraPartWidth;

		// Token: 0x0400166D RID: 5741
		public Func<Rect, bool> extraPartOnGUI;

		// Token: 0x0400166E RID: 5742
		public string tutorTag;

		// Token: 0x0400166F RID: 5743
		public ThingStyleDef thingStyle;

		// Token: 0x04001670 RID: 5744
		public bool forceBasicStyle;

		// Token: 0x04001671 RID: 5745
		public TipSignal? tooltip;

		// Token: 0x04001672 RID: 5746
		public bool extraPartRightJustified;

		// Token: 0x04001673 RID: 5747
		public int? graphicIndexOverride;

		// Token: 0x04001674 RID: 5748
		private FloatMenuSizeMode sizeMode;

		// Token: 0x04001675 RID: 5749
		private float cachedRequiredHeight;

		// Token: 0x04001676 RID: 5750
		private float cachedRequiredWidth;

		// Token: 0x04001677 RID: 5751
		private bool drawPlaceHolderIcon;

		// Token: 0x04001678 RID: 5752
		private bool playSelectionSound = true;

		// Token: 0x04001679 RID: 5753
		private ThingDef shownItem;

		// Token: 0x0400167A RID: 5754
		private Thing iconThing;

		// Token: 0x0400167B RID: 5755
		private Texture2D itemIcon;

		// Token: 0x0400167C RID: 5756
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x0400167D RID: 5757
		private HorizontalJustification iconJustification;

		// Token: 0x0400167E RID: 5758
		public Color iconColor = Color.white;

		// Token: 0x0400167F RID: 5759
		public Color? forceThingColor;

		// Token: 0x04001680 RID: 5760
		public const float MaxWidth = 300f;

		// Token: 0x04001681 RID: 5761
		private const float TinyVerticalMargin = 1f;

		// Token: 0x04001682 RID: 5762
		private const float NormalHorizontalMargin = 6f;

		// Token: 0x04001683 RID: 5763
		private const float TinyHorizontalMargin = 3f;

		// Token: 0x04001684 RID: 5764
		private const float MouseOverLabelShift = 4f;

		// Token: 0x04001685 RID: 5765
		public static readonly Color ColorBGActive = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x04001686 RID: 5766
		public static readonly Color ColorBGActiveMouseover = new ColorInt(29, 45, 50).ToColor;

		// Token: 0x04001687 RID: 5767
		public static readonly Color ColorBGDisabled = new ColorInt(40, 40, 40).ToColor;

		// Token: 0x04001688 RID: 5768
		public static readonly Color ColorTextActive = Color.white;

		// Token: 0x04001689 RID: 5769
		public static readonly Color ColorTextDisabled = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x0400168A RID: 5770
		public const float ExtraPartHeight = 30f;

		// Token: 0x0400168B RID: 5771
		private const float ItemIconSize = 27f;

		// Token: 0x0400168C RID: 5772
		private const float ItemIconMargin = 4f;
	}
}

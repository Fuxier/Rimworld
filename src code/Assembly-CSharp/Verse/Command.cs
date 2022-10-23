using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000493 RID: 1171
	[StaticConstructorOnStartup]
	public abstract class Command : Gizmo
	{
		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x0600235F RID: 9055 RVA: 0x000E2A16 File Offset: 0x000E0C16
		public virtual string Label
		{
			get
			{
				return this.defaultLabel;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x000E2A1E File Offset: 0x000E0C1E
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002361 RID: 9057 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual string TopRightLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002362 RID: 9058 RVA: 0x000E2A2B File Offset: 0x000E0C2B
		public virtual string Desc
		{
			get
			{
				return this.defaultDesc;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002363 RID: 9059 RVA: 0x000E2A33 File Offset: 0x000E0C33
		public virtual string DescPostfix
		{
			get
			{
				return this.defaultDescPostfix;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x000E2A3B File Offset: 0x000E0C3B
		public virtual Color IconDrawColor
		{
			get
			{
				return this.defaultIconColor;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x000E2A43 File Offset: 0x000E0C43
		public virtual SoundDef CurActivateSound
		{
			get
			{
				return this.activateSound;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x00002662 File Offset: 0x00000862
		protected virtual bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002367 RID: 9063 RVA: 0x000E2A4B File Offset: 0x000E0C4B
		public virtual string HighlightTag
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002368 RID: 9064 RVA: 0x000E2A4B File Offset: 0x000E0C4B
		public virtual string TutorTagSelect
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002369 RID: 9065 RVA: 0x000E2A53 File Offset: 0x000E0C53
		public virtual Texture2D BGTexture
		{
			get
			{
				return Command.BGTex;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x000E2A5A File Offset: 0x000E0C5A
		public virtual Texture2D BGTextureShrunk
		{
			get
			{
				return Command.BGTexShrunk;
			}
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x000E2A61 File Offset: 0x000E0C61
		public override float GetWidth(float maxWidth)
		{
			return 75f;
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x0600236C RID: 9068 RVA: 0x000E2A68 File Offset: 0x000E0C68
		public float GetShrunkSize
		{
			get
			{
				return 36f;
			}
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x000E2A6F File Offset: 0x000E0C6F
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			return this.GizmoOnGUIInt(new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f), parms);
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x000E2A95 File Offset: 0x000E0C95
		public virtual GizmoResult GizmoOnGUIShrunk(Vector2 topLeft, float size, GizmoRenderParms parms)
		{
			parms.shrunk = true;
			return this.GizmoOnGUIInt(new Rect(topLeft.x, topLeft.y, size, size), parms);
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x000E2ABC File Offset: 0x000E0CBC
		protected virtual GizmoResult GizmoOnGUIInt(Rect butRect, GizmoRenderParms parms)
		{
			Text.Font = GameFont.Tiny;
			Color color = Color.white;
			bool flag = false;
			if (Mouse.IsOver(butRect))
			{
				flag = true;
				if (!this.disabled)
				{
					color = GenUI.MouseoverColor;
				}
			}
			MouseoverSounds.DoRegion(butRect, SoundDefOf.Mouseover_Command);
			if (parms.highLight)
			{
				Widgets.DrawStrongHighlight(butRect.ExpandedBy(12f), null);
			}
			Material material = (this.disabled || parms.lowLight) ? TexUI.GrayscaleGUI : null;
			GUI.color = (parms.lowLight ? Command.LowLightBgColor : color);
			GenUI.DrawTextureWithMaterial(butRect, parms.shrunk ? this.BGTextureShrunk : this.BGTexture, material, default(Rect));
			GUI.color = color;
			this.DrawIcon(butRect, material, parms);
			bool flag2 = false;
			GUI.color = Color.white;
			if (parms.lowLight)
			{
				GUI.color = Command.LowLightLabelColor;
			}
			Vector2 vector = parms.shrunk ? new Vector2(3f, 0f) : new Vector2(5f, 3f);
			Rect rect = new Rect(butRect.x + vector.x, butRect.y + vector.y, butRect.width - 10f, Text.LineHeight);
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				if (parms.isFirst)
				{
					GUI.DrawTexture(new Rect(rect.x, rect.y, 21f, 21f), TexUI.SteamDeck_ButtonA);
					if (KeyBindingDefOf.Accept.KeyDownEvent)
					{
						flag2 = true;
						Event.current.Use();
					}
				}
			}
			else
			{
				KeyCode keyCode = (this.hotKey == null) ? KeyCode.None : this.hotKey.MainKey;
				if (keyCode != KeyCode.None && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode))
				{
					Widgets.Label(rect, keyCode.ToStringReadable());
					GizmoGridDrawer.drawnHotKeys.Add(keyCode);
					if (this.hotKey.KeyDownEvent)
					{
						flag2 = true;
						Event.current.Use();
					}
				}
			}
			if (GizmoGridDrawer.customActivator != null && GizmoGridDrawer.customActivator(this))
			{
				flag2 = true;
			}
			if (Widgets.ButtonInvisible(butRect, true))
			{
				flag2 = true;
			}
			if (!parms.shrunk)
			{
				string topRightLabel = this.TopRightLabel;
				if (!topRightLabel.NullOrEmpty())
				{
					Vector2 vector2 = Text.CalcSize(topRightLabel);
					Rect position;
					Rect rect2 = position = new Rect(butRect.xMax - vector2.x - 2f, butRect.y + 3f, vector2.x, vector2.y);
					position.x -= 2f;
					position.width += 3f;
					Text.Anchor = TextAnchor.UpperRight;
					GUI.DrawTexture(position, TexUI.GrayTextBG);
					Widgets.Label(rect2, topRightLabel);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				string labelCap = this.LabelCap;
				if (!labelCap.NullOrEmpty())
				{
					float num = Text.CalcHeight(labelCap, butRect.width);
					Rect rect3 = new Rect(butRect.x, butRect.yMax - num + 12f, butRect.width, num);
					GUI.DrawTexture(rect3, TexUI.GrayTextBG);
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect3, labelCap);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				GUI.color = Color.white;
			}
			if (Mouse.IsOver(butRect) && this.DoTooltip)
			{
				TipSignal tip = this.Desc;
				if (this.disabled && !this.disabledReason.NullOrEmpty())
				{
					tip.text += ("\n\n" + "DisabledCommand".Translate() + ": " + this.disabledReason).Colorize(ColorLibrary.RedReadable);
				}
				tip.text += this.DescPostfix;
				TooltipHandler.TipRegion(butRect, tip);
			}
			if (!this.HighlightTag.NullOrEmpty() && (Find.WindowStack.FloatMenu == null || !Find.WindowStack.FloatMenu.windowRect.Overlaps(butRect)))
			{
				UIHighlighter.HighlightOpportunity(butRect, this.HighlightTag);
			}
			Text.Font = GameFont.Small;
			if (flag2)
			{
				if (this.disabled)
				{
					if (!this.disabledReason.NullOrEmpty())
					{
						Messages.Message(this.disabledReason, MessageTypeDefOf.RejectInput, false);
					}
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				GizmoResult result;
				if (Event.current.button == 1)
				{
					result = new GizmoResult(GizmoState.OpenedFloatMenu, Event.current);
				}
				else
				{
					if (!TutorSystem.AllowAction(this.TutorTagSelect))
					{
						return new GizmoResult(GizmoState.Mouseover, null);
					}
					result = new GizmoResult(GizmoState.Interacted, Event.current);
					TutorSystem.Notify_Event(this.TutorTagSelect);
				}
				return result;
			}
			else
			{
				if (flag)
				{
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				return new GizmoResult(GizmoState.Clear, null);
			}
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x000E2F5C File Offset: 0x000E115C
		public virtual void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			Texture badTex = this.icon;
			if (badTex == null)
			{
				badTex = BaseContent.BadTex;
			}
			rect.position += new Vector2(this.iconOffset.x * rect.size.x, this.iconOffset.y * rect.size.y);
			if (!this.disabled || parms.lowLight)
			{
				GUI.color = this.IconDrawColor;
			}
			else
			{
				GUI.color = this.IconDrawColor.SaturationChanged(0f);
			}
			if (parms.lowLight)
			{
				GUI.color = GUI.color.ToTransparent(0.6f);
			}
			Widgets.DrawTextureFitted(rect, badTex, this.iconDrawScale * 0.85f, this.iconProportions, this.iconTexCoords, this.iconAngle, buttonMat);
			GUI.color = Color.white;
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x000E3044 File Offset: 0x000E1244
		public override bool GroupsWith(Gizmo other)
		{
			if (!this.groupable)
			{
				return false;
			}
			Command command = other as Command;
			return command != null && command.groupable && ((this.hotKey == command.hotKey && this.Label == command.Label && this.icon == command.icon && this.groupKey == command.groupKey) || (this.groupKeyIgnoreContent != -1 && command.groupKeyIgnoreContent != -1 && this.groupKeyIgnoreContent == command.groupKeyIgnoreContent));
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000E30D8 File Offset: 0x000E12D8
		public override void ProcessInput(Event ev)
		{
			if (this.CurActivateSound != null)
			{
				this.CurActivateSound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x000E30EE File Offset: 0x000E12EE
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Command(label=",
				this.defaultLabel,
				", defaultDesc=",
				this.defaultDesc,
				")"
			});
		}

		// Token: 0x040016B5 RID: 5813
		public string defaultLabel;

		// Token: 0x040016B6 RID: 5814
		public string defaultDesc = "No description.";

		// Token: 0x040016B7 RID: 5815
		public string defaultDescPostfix;

		// Token: 0x040016B8 RID: 5816
		public Texture icon;

		// Token: 0x040016B9 RID: 5817
		public float iconAngle;

		// Token: 0x040016BA RID: 5818
		public Vector2 iconProportions = Vector2.one;

		// Token: 0x040016BB RID: 5819
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x040016BC RID: 5820
		public float iconDrawScale = 1f;

		// Token: 0x040016BD RID: 5821
		public Vector2 iconOffset;

		// Token: 0x040016BE RID: 5822
		public Color defaultIconColor = Color.white;

		// Token: 0x040016BF RID: 5823
		public KeyBindingDef hotKey;

		// Token: 0x040016C0 RID: 5824
		public SoundDef activateSound;

		// Token: 0x040016C1 RID: 5825
		public int groupKey = -1;

		// Token: 0x040016C2 RID: 5826
		public int groupKeyIgnoreContent = -1;

		// Token: 0x040016C3 RID: 5827
		public string tutorTag = "TutorTagNotSet";

		// Token: 0x040016C4 RID: 5828
		public bool shrinkable;

		// Token: 0x040016C5 RID: 5829
		public bool groupable = true;

		// Token: 0x040016C6 RID: 5830
		public static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x040016C7 RID: 5831
		public static readonly Texture2D BGTexShrunk = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x040016C8 RID: 5832
		public static readonly Color LowLightBgColor = new Color(0.8f, 0.8f, 0.7f, 0.5f);

		// Token: 0x040016C9 RID: 5833
		public static readonly Color LowLightIconColor = new Color(0.8f, 0.8f, 0.7f, 0.6f);

		// Token: 0x040016CA RID: 5834
		public static readonly Color LowLightLabelColor = new Color(0.8f, 0.8f, 0.7f, 0.5f);

		// Token: 0x040016CB RID: 5835
		public const float LowLightIconAlpha = 0.6f;

		// Token: 0x040016CC RID: 5836
		protected const float InnerIconDrawScale = 0.85f;
	}
}

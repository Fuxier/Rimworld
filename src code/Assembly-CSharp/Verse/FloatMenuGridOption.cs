using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000483 RID: 1155
	public class FloatMenuGridOption
	{
		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x000DFD5F File Offset: 0x000DDF5F
		public bool Disabled
		{
			get
			{
				return this.action == null;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x000DFD6A File Offset: 0x000DDF6A
		public MenuOptionPriority Priority
		{
			get
			{
				if (this.Disabled)
				{
					return MenuOptionPriority.DisabledOption;
				}
				return MenuOptionPriority.Default;
			}
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x000DFD78 File Offset: 0x000DDF78
		public FloatMenuGridOption(Texture2D texture, Action action, Color? color = null, TipSignal? tooltip = null)
		{
			this.texture = texture;
			this.action = action;
			this.color = (color ?? Color.white);
			this.tooltip = tooltip;
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x000DFDEC File Offset: 0x000DDFEC
		public bool OnGUI(Rect rect)
		{
			bool flag = !this.Disabled && Mouse.IsOver(rect);
			if (!this.Disabled)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (this.tooltip != null)
			{
				TooltipHandler.TipRegion(rect, this.tooltip.Value);
			}
			Color color = GUI.color;
			if (this.Disabled)
			{
				GUI.color = FloatMenuOption.ColorBGDisabled * color;
			}
			else if (flag)
			{
				GUI.color = FloatMenuOption.ColorBGActiveMouseover * color;
			}
			else
			{
				GUI.color = FloatMenuOption.ColorBGActive * color;
			}
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = ((!this.Disabled) ? FloatMenuOption.ColorTextActive : FloatMenuOption.ColorTextDisabled) * color;
			Widgets.DrawAtlas(rect, TexUI.FloatMenuOptionBG);
			GUI.color = new Color(this.color.r, this.color.g, this.color.b, this.color.a * color.a);
			Rect rect2 = rect.ContractedBy(2f);
			if (!flag)
			{
				rect2 = rect2.ContractedBy(2f);
			}
			Material mat = this.Disabled ? TexUI.GrayscaleGUI : null;
			Widgets.DrawTextureFitted(rect2, this.texture, 1f, new Vector2(1f, 1f), this.iconTexCoords, 0f, mat);
			GUI.color = color;
			Action<Rect> action = this.postDrawAction;
			if (action != null)
			{
				action(rect2);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.Chosen();
				return true;
			}
			return false;
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x000DFF70 File Offset: 0x000DE170
		public void Chosen()
		{
			if (this.Disabled)
			{
				SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
				return;
			}
			Action action = this.action;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0400164D RID: 5709
		public Texture2D texture;

		// Token: 0x0400164E RID: 5710
		public Color color = Color.white;

		// Token: 0x0400164F RID: 5711
		public Action action;

		// Token: 0x04001650 RID: 5712
		public TipSignal? tooltip;

		// Token: 0x04001651 RID: 5713
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001652 RID: 5714
		public Action<Rect> postDrawAction;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000482 RID: 1154
	public class FloatMenuGrid : Window
	{
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060022F8 RID: 8952 RVA: 0x000DFA20 File Offset: 0x000DDC20
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalHeight);
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060022F9 RID: 8953 RVA: 0x000DFA33 File Offset: 0x000DDC33
		public float TotalWidth
		{
			get
			{
				return (float)this.calculatedSquareSize * (FloatMenuGrid.OptionSize.x - 1f);
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x000DFA4D File Offset: 0x000DDC4D
		public float TotalHeight
		{
			get
			{
				return (float)this.calculatedSquareSize * (FloatMenuGrid.OptionSize.y - 1f);
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x00004E2A File Offset: 0x0000302A
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x000DFA68 File Offset: 0x000DDC68
		public FloatMenuGrid(List<FloatMenuGridOption> options)
		{
			this.options = (from op in options
			orderby op.Priority descending
			select op).ToList<FloatMenuGridOption>();
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			this.calculatedSquareSize = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(Mathf.Round(Mathf.Sqrt((float)options.Count)), 2f)));
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x000DFB10 File Offset: 0x000DDD10
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (mousePositionOnUIInverted.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				mousePositionOnUIInverted.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (mousePositionOnUIInverted.y > (float)UI.screenHeight)
			{
				mousePositionOnUIInverted.y = (float)UI.screenHeight;
			}
			this.windowRect = new Rect(mousePositionOnUIInverted.x, mousePositionOnUIInverted.y - this.InitialSize.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x000DFBAC File Offset: 0x000DDDAC
		public override void DoWindowContents(Rect rect)
		{
			this.UpdateBaseColor();
			GUI.color = this.baseColor;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this.options.Count; i++)
			{
				FloatMenuGridOption floatMenuGridOption = this.options[i];
				float num3 = (float)num * FloatMenuGrid.OptionSize.x;
				float num4 = (float)num2 * FloatMenuGrid.OptionSize.y;
				if (num3 > 0f)
				{
					num3 -= (float)num;
				}
				if (num4 > 0f)
				{
					num4 -= (float)num2;
				}
				if (floatMenuGridOption.OnGUI(new Rect(num3, num4, FloatMenuGrid.OptionSize.x, FloatMenuGrid.OptionSize.y)))
				{
					Find.WindowStack.TryRemove(this, true);
					break;
				}
				num++;
				if (num >= this.calculatedSquareSize)
				{
					num = 0;
					num2++;
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x000DFC80 File Offset: 0x000DDE80
		private void UpdateBaseColor()
		{
			this.baseColor = Color.white;
			Rect r = new Rect(0f, 0f, this.TotalWidth, this.TotalHeight).ExpandedBy(5f);
			if (!r.Contains(Event.current.mousePosition))
			{
				float num = GenUI.DistFromRect(r, Event.current.mousePosition);
				this.baseColor = new Color(1f, 1f, 1f, 1f - num / 95f);
				if (num > 95f)
				{
					this.Close(false);
					SoundDefOf.FloatMenu_Cancel.PlayOneShotOnCamera(null);
					Find.WindowStack.TryRemove(this, true);
					return;
				}
			}
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x000DFD31 File Offset: 0x000DDF31
		public override void PostClose()
		{
			base.PostClose();
			Action action = this.onCloseCallback;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04001648 RID: 5704
		private List<FloatMenuGridOption> options;

		// Token: 0x04001649 RID: 5705
		private int calculatedSquareSize;

		// Token: 0x0400164A RID: 5706
		private Color baseColor = Color.white;

		// Token: 0x0400164B RID: 5707
		public Action onCloseCallback;

		// Token: 0x0400164C RID: 5708
		private static readonly Vector2 OptionSize = new Vector2(34f, 34f);
	}
}

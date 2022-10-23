using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000413 RID: 1043
	public class CompColorable : ThingComp
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001E9F RID: 7839 RVA: 0x000B7278 File Offset: 0x000B5478
		// (set) Token: 0x06001EA0 RID: 7840 RVA: 0x000B7280 File Offset: 0x000B5480
		public Color? DesiredColor
		{
			get
			{
				return this.desiredColor;
			}
			set
			{
				this.desiredColor = value;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001EA1 RID: 7841 RVA: 0x000B7289 File Offset: 0x000B5489
		public Color Color
		{
			get
			{
				if (!this.active)
				{
					return this.parent.def.graphicData.color;
				}
				return this.color;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001EA2 RID: 7842 RVA: 0x000B72AF File Offset: 0x000B54AF
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000B72B8 File Offset: 0x000B54B8
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.SetColor(this.parent.def.colorGenerator.NewRandomizedColor());
			}
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x000B7320 File Offset: 0x000B5520
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && !this.active)
			{
				return;
			}
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
			Scribe_Values.Look<Color?>(ref this.desiredColor, "desiredColor", null, false);
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000B738A File Offset: 0x000B558A
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000B73A8 File Offset: 0x000B55A8
		public void Recolor()
		{
			if (this.desiredColor == null)
			{
				Log.Error("Tried recoloring apparel which does not have a desired color set!");
				return;
			}
			this.SetColor(this.DesiredColor.Value);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000B73E1 File Offset: 0x000B55E1
		public void Disable()
		{
			this.active = false;
			this.color = Color.white;
			this.desiredColor = null;
			this.parent.Notify_ColorChanged();
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000B740C File Offset: 0x000B560C
		public void SetColor(Color value)
		{
			if (value == this.color)
			{
				return;
			}
			this.active = true;
			this.color = value;
			this.desiredColor = null;
			this.parent.Notify_ColorChanged();
		}

		// Token: 0x040014E6 RID: 5350
		private Color? desiredColor;

		// Token: 0x040014E7 RID: 5351
		private Color color = Color.white;

		// Token: 0x040014E8 RID: 5352
		private bool active;
	}
}

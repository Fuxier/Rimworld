using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000107 RID: 263
	public class LetterDef : Def
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x000258E1 File Offset: 0x00023AE1
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null && !this.icon.NullOrEmpty())
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00025916 File Offset: 0x00023B16
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.arriveSound == null)
			{
				this.arriveSound = SoundDefOf.LetterArrive;
			}
		}

		// Token: 0x04000667 RID: 1639
		public Type letterClass = typeof(StandardLetter);

		// Token: 0x04000668 RID: 1640
		public Color color = Color.white;

		// Token: 0x04000669 RID: 1641
		public Color flashColor = Color.white;

		// Token: 0x0400066A RID: 1642
		public float flashInterval = 90f;

		// Token: 0x0400066B RID: 1643
		public bool bounce;

		// Token: 0x0400066C RID: 1644
		public SoundDef arriveSound;

		// Token: 0x0400066D RID: 1645
		[NoTranslate]
		public string icon = "UI/Letters/LetterUnopened";

		// Token: 0x0400066E RID: 1646
		public AutomaticPauseMode pauseMode = AutomaticPauseMode.AnyLetter;

		// Token: 0x0400066F RID: 1647
		public bool forcedSlowdown;

		// Token: 0x04000670 RID: 1648
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}

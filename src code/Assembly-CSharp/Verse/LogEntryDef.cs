using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000108 RID: 264
	public class LogEntryDef : Def
	{
		// Token: 0x0600071E RID: 1822 RVA: 0x0002598A File Offset: 0x00023B8A
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.iconMiss.NullOrEmpty())
				{
					this.iconMissTex = ContentFinder<Texture2D>.Get(this.iconMiss, true);
				}
				if (!this.iconDamaged.NullOrEmpty())
				{
					this.iconDamagedTex = ContentFinder<Texture2D>.Get(this.iconDamaged, true);
				}
				if (!this.iconDamagedFromInstigator.NullOrEmpty())
				{
					this.iconDamagedFromInstigatorTex = ContentFinder<Texture2D>.Get(this.iconDamagedFromInstigator, true);
				}
			});
		}

		// Token: 0x04000671 RID: 1649
		[NoTranslate]
		public string iconMiss;

		// Token: 0x04000672 RID: 1650
		[NoTranslate]
		public string iconDamaged;

		// Token: 0x04000673 RID: 1651
		[NoTranslate]
		public string iconDamagedFromInstigator;

		// Token: 0x04000674 RID: 1652
		[Unsaved(false)]
		public Texture2D iconMissTex;

		// Token: 0x04000675 RID: 1653
		[Unsaved(false)]
		public Texture2D iconDamagedTex;

		// Token: 0x04000676 RID: 1654
		[Unsaved(false)]
		public Texture2D iconDamagedFromInstigatorTex;
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000137 RID: 311
	public class SongDef : Def
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x00028620 File Offset: 0x00026820
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				string[] array = this.clipPath.Split(new char[]
				{
					'/',
					'\\'
				});
				this.defName = array[array.Length - 1];
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0002866E File Offset: 0x0002686E
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
			});
		}

		// Token: 0x0400080A RID: 2058
		[NoTranslate]
		public string clipPath;

		// Token: 0x0400080B RID: 2059
		public float volume = 1f;

		// Token: 0x0400080C RID: 2060
		public bool playOnMap = true;

		// Token: 0x0400080D RID: 2061
		public float commonality = 1f;

		// Token: 0x0400080E RID: 2062
		public bool tense;

		// Token: 0x0400080F RID: 2063
		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		// Token: 0x04000810 RID: 2064
		public List<Season> allowedSeasons;

		// Token: 0x04000811 RID: 2065
		public RoyalTitleDef minRoyalTitle;

		// Token: 0x04000812 RID: 2066
		[Unsaved(false)]
		public AudioClip clip;
	}
}

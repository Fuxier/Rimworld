using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000415 RID: 1045
	public class CompProperties_ColorableAnimated : CompProperties
	{
		// Token: 0x06001EAB RID: 7851 RVA: 0x000B74B6 File Offset: 0x000B56B6
		public CompProperties_ColorableAnimated()
		{
			this.compClass = typeof(CompColorable_Animated);
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x000B74E0 File Offset: 0x000B56E0
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.colors.Count == 0)
			{
				yield return "there should be at least one color specified in colors list";
			}
			yield break;
		}

		// Token: 0x040014E9 RID: 5353
		public int changeInterval = 1;

		// Token: 0x040014EA RID: 5354
		public bool startWithRandom;

		// Token: 0x040014EB RID: 5355
		public List<Color> colors = new List<Color>();
	}
}

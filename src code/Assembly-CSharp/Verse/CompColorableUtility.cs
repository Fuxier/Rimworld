using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000414 RID: 1044
	public static class CompColorableUtility
	{
		// Token: 0x06001EAA RID: 7850 RVA: 0x000B7458 File Offset: 0x000B5658
		public static void SetColor(this Thing t, Color newColor, bool reportFailure = true)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on non-ThingWithComps " + t);
				}
				return;
			}
			CompColorable comp = thingWithComps.GetComp<CompColorable>();
			if (comp == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on Thing without CompColorable " + t);
				}
				return;
			}
			if (!comp.Color.IndistinguishableFrom(newColor))
			{
				comp.SetColor(newColor);
			}
		}
	}
}

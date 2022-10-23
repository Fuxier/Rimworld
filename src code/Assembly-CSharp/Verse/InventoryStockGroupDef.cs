using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000102 RID: 258
	public class InventoryStockGroupDef : Def
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0002551E File Offset: 0x0002371E
		public ThingDef DefaultThingDef
		{
			get
			{
				return this.defaultThingDef ?? this.thingDefs.First<ThingDef>();
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00025535 File Offset: 0x00023735
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.defaultThingDef != null && !this.thingDefs.Contains(this.defaultThingDef))
			{
				yield return "Default thing def " + this.defaultThingDef.defName + " should be in thingDefs but not found.";
			}
			if (this.min > this.max)
			{
				yield return "Min should be less than max.";
			}
			if (this.min < 0 || this.max < 0)
			{
				yield return "Min/max should be greater than zero.";
			}
			if (this.thingDefs.NullOrEmpty<ThingDef>())
			{
				yield return "thingDefs cannot be null or empty.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400063B RID: 1595
		public List<ThingDef> thingDefs;

		// Token: 0x0400063C RID: 1596
		public int min;

		// Token: 0x0400063D RID: 1597
		public int max = 3;

		// Token: 0x0400063E RID: 1598
		public ThingDef defaultThingDef;
	}
}

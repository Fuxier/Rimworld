using System;

namespace Verse
{
	// Token: 0x0200047E RID: 1150
	public class TableDataGetter<T>
	{
		// Token: 0x060022DA RID: 8922 RVA: 0x000DEF49 File Offset: 0x000DD149
		public TableDataGetter(string label, Func<T, string> getter)
		{
			this.label = label;
			this.getter = getter;
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x000DEF60 File Offset: 0x000DD160
		public TableDataGetter(string label, Func<T, float> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("0.#"));
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x000DEF9C File Offset: 0x000DD19C
		public TableDataGetter(string label, Func<T, int> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString("F0"));
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000DEFD8 File Offset: 0x000DD1D8
		public TableDataGetter(string label, Func<T, ThingDef> getter)
		{
			this.label = label;
			this.getter = delegate(T t)
			{
				ThingDef thingDef = getter(t);
				if (thingDef == null)
				{
					return "";
				}
				return thingDef.defName;
			};
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000DF014 File Offset: 0x000DD214
		public TableDataGetter(string label, Func<T, object> getter)
		{
			this.label = label;
			this.getter = ((T t) => getter(t).ToString());
		}

		// Token: 0x04001632 RID: 5682
		public string label;

		// Token: 0x04001633 RID: 5683
		public Func<T, string> getter;
	}
}

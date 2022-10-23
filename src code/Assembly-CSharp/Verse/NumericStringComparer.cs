using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000586 RID: 1414
	public class NumericStringComparer : IComparer<string>
	{
		// Token: 0x06002B2B RID: 11051 RVA: 0x001140C0 File Offset: 0x001122C0
		public int Compare(string x, string y)
		{
			x = (x ?? "");
			y = (y ?? "");
			if (x.Contains("~"))
			{
				string[] array = x.Split(new char[]
				{
					'~'
				});
				if (array.Length == 2)
				{
					x = array[0];
				}
			}
			if (y.Contains("~"))
			{
				string[] array2 = y.Split(new char[]
				{
					'~'
				});
				if (array2.Length == 2)
				{
					y = array2[0];
				}
			}
			if ((x.EndsWith("%") && y.EndsWith("%")) || (x.EndsWith("C") && y.EndsWith("C")))
			{
				x = x.Substring(0, x.Length - 1);
				y = y.Substring(0, y.Length - 1);
			}
			if (x.Length >= 2 && x[0] == '$')
			{
				x = x.Substring(1, x.Length - 1);
			}
			if (y.Length >= 2 && y[0] == '$')
			{
				y = y.Substring(1, y.Length - 1);
			}
			float num;
			float value;
			if (float.TryParse(x, out num) && float.TryParse(y, out value))
			{
				return num.CompareTo(value);
			}
			return x.CompareTo(y);
		}
	}
}

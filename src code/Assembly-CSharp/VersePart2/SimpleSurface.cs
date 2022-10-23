using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000530 RID: 1328
	public class SimpleSurface : IEnumerable<SurfaceColumn>, IEnumerable
	{
		// Token: 0x060028B6 RID: 10422 RVA: 0x001066F0 File Offset: 0x001048F0
		public float Evaluate(float x, float y)
		{
			if (this.columns.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve2D with no columns.");
				return 0f;
			}
			if (x <= this.columns[0].x)
			{
				return this.columns[0].y.Evaluate(y);
			}
			if (x >= this.columns[this.columns.Count - 1].x)
			{
				return this.columns[this.columns.Count - 1].y.Evaluate(y);
			}
			SurfaceColumn surfaceColumn = this.columns[0];
			SurfaceColumn surfaceColumn2 = this.columns[this.columns.Count - 1];
			int i = 0;
			while (i < this.columns.Count)
			{
				if (x <= this.columns[i].x)
				{
					surfaceColumn2 = this.columns[i];
					if (i > 0)
					{
						surfaceColumn = this.columns[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float t = (x - surfaceColumn.x) / (surfaceColumn2.x - surfaceColumn.x);
			return Mathf.Lerp(surfaceColumn.y.Evaluate(y), surfaceColumn2.y.Evaluate(y), t);
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x00106831 File Offset: 0x00104A31
		public void Add(SurfaceColumn newColumn)
		{
			this.columns.Add(newColumn);
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x0010683F File Offset: 0x00104A3F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x00106847 File Offset: 0x00104A47
		public IEnumerator<SurfaceColumn> GetEnumerator()
		{
			foreach (SurfaceColumn surfaceColumn in this.columns)
			{
				yield return surfaceColumn;
			}
			List<SurfaceColumn>.Enumerator enumerator = default(List<SurfaceColumn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x00106856 File Offset: 0x00104A56
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.columns.Count - 1; i++)
			{
				if (this.columns[i + 1].x < this.columns[i].x)
				{
					yield return prefix + ": columns are out of order";
					break;
				}
			}
			yield break;
		}

		// Token: 0x04001AAD RID: 6829
		private List<SurfaceColumn> columns = new List<SurfaceColumn>();
	}
}

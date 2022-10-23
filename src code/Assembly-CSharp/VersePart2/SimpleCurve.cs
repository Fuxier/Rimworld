using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200002B RID: 43
	public class SimpleCurve : IEnumerable<CurvePoint>, IEnumerable
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00009B83 File Offset: 0x00007D83
		public int PointsCount
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00009B90 File Offset: 0x00007D90
		public List<CurvePoint> Points
		{
			get
			{
				return this.points;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00009B98 File Offset: 0x00007D98
		public bool HasView
		{
			get
			{
				return this.view != null;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00009BA3 File Offset: 0x00007DA3
		public SimpleCurveView View
		{
			get
			{
				if (this.view == null)
				{
					this.view = new SimpleCurveView();
					this.view.SetViewRectAround(this);
				}
				return this.view;
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009BCA File Offset: 0x00007DCA
		public SimpleCurve(IEnumerable<CurvePoint> points)
		{
			this.SetPoints(points);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009BE4 File Offset: 0x00007DE4
		public SimpleCurve()
		{
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00009BF7 File Offset: 0x00007DF7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00009BFF File Offset: 0x00007DFF
		public IEnumerator<CurvePoint> GetEnumerator()
		{
			foreach (CurvePoint curvePoint in this.points)
			{
				yield return curvePoint;
			}
			List<CurvePoint>.Enumerator enumerator = default(List<CurvePoint>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x17000072 RID: 114
		public CurvePoint this[int i]
		{
			get
			{
				return this.points[i];
			}
			set
			{
				this.points[i] = value;
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009C2C File Offset: 0x00007E2C
		public void SetPoints(IEnumerable<CurvePoint> newPoints)
		{
			this.points.Clear();
			foreach (CurvePoint item in newPoints)
			{
				this.points.Add(item);
			}
			this.SortPoints();
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009C8C File Offset: 0x00007E8C
		public void Add(float x, float y, bool sort = true)
		{
			CurvePoint newPoint = new CurvePoint(x, y);
			this.Add(newPoint, sort);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00009CAA File Offset: 0x00007EAA
		public void Add(CurvePoint newPoint, bool sort = true)
		{
			this.points.Add(newPoint);
			if (sort)
			{
				this.SortPoints();
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00009CC1 File Offset: 0x00007EC1
		public void SortPoints()
		{
			this.points.Sort(SimpleCurve.CurvePointsComparer);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00009CD4 File Offset: 0x00007ED4
		public float ClampToCurve(float value)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Clamping a value to an empty SimpleCurve.");
				return value;
			}
			return Mathf.Clamp(value, this.points[0].y, this.points[this.points.Count - 1].y);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00009D34 File Offset: 0x00007F34
		public void RemovePointNear(CurvePoint point)
		{
			for (int i = 0; i < this.points.Count; i++)
			{
				if ((this.points[i].Loc - point.Loc).sqrMagnitude < 0.001f)
				{
					this.points.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00009D94 File Offset: 0x00007F94
		public float Evaluate(float x)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.");
				return 0f;
			}
			if (x <= this.points[0].x)
			{
				return this.points[0].y;
			}
			if (x >= this.points[this.points.Count - 1].x)
			{
				return this.points[this.points.Count - 1].y;
			}
			CurvePoint curvePoint = this.points[0];
			CurvePoint curvePoint2 = this.points[this.points.Count - 1];
			int i = 0;
			while (i < this.points.Count)
			{
				if (x <= this.points[i].x)
				{
					curvePoint2 = this.points[i];
					if (i > 0)
					{
						curvePoint = this.points[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float t = (x - curvePoint.x) / (curvePoint2.x - curvePoint.x);
			return Mathf.Lerp(curvePoint.y, curvePoint2.y, t);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009EDC File Offset: 0x000080DC
		public float EvaluateInverted(float y)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.");
				return 0f;
			}
			if (this.points.Count == 1)
			{
				return this.points[0].x;
			}
			int i = 0;
			while (i < this.points.Count - 1)
			{
				if ((y >= this.points[i].y && y <= this.points[i + 1].y) || (y <= this.points[i].y && y >= this.points[i + 1].y))
				{
					if (y == this.points[i].y)
					{
						return this.points[i].x;
					}
					if (y == this.points[i + 1].y)
					{
						return this.points[i + 1].x;
					}
					return GenMath.LerpDouble(this.points[i].y, this.points[i + 1].y, this.points[i].x, this.points[i + 1].x, y);
				}
				else
				{
					i++;
				}
			}
			if (y < this.points[0].y)
			{
				float result = 0f;
				float num = 0f;
				for (int j = 0; j < this.points.Count; j++)
				{
					if (j == 0 || this.points[j].y < num)
					{
						num = this.points[j].y;
						result = this.points[j].x;
					}
				}
				return result;
			}
			float result2 = 0f;
			float num2 = 0f;
			for (int k = 0; k < this.points.Count; k++)
			{
				if (k == 0 || this.points[k].y > num2)
				{
					num2 = this.points[k].y;
					result2 = this.points[k].x;
				}
			}
			return result2;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000A160 File Offset: 0x00008360
		public float PeriodProbabilityFromCumulative(float startX, float span)
		{
			if (this.points.Count < 2)
			{
				return 0f;
			}
			if (this.points[0].y != 0f)
			{
				Log.Warning("PeriodProbabilityFromCumulative should only run on curves whose first point is 0.");
			}
			float num = this.Evaluate(startX + span) - this.Evaluate(startX);
			if (num < 0f)
			{
				Log.Error("PeriodicProbability got negative probability from " + this + ": slope should never be negative.");
				num = 0f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000A1EA File Offset: 0x000083EA
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.points.Count - 1; i++)
			{
				if (this.points[i + 1].x < this.points[i].x)
				{
					yield return prefix + ": points are out of order";
					break;
				}
			}
			yield break;
		}

		// Token: 0x0400007E RID: 126
		private List<CurvePoint> points = new List<CurvePoint>();

		// Token: 0x0400007F RID: 127
		[Unsaved(false)]
		private SimpleCurveView view;

		// Token: 0x04000080 RID: 128
		private static Comparison<CurvePoint> CurvePointsComparer = delegate(CurvePoint a, CurvePoint b)
		{
			if (a.x < b.x)
			{
				return -1;
			}
			if (b.x < a.x)
			{
				return 1;
			}
			return 0;
		};
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200002C RID: 44
	public class SimpleCurveView
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000A218 File Offset: 0x00008418
		public IEnumerable<float> DebugInputValues
		{
			get
			{
				if (this.debugInputValues == null)
				{
					yield break;
				}
				foreach (float num in this.debugInputValues.Values)
				{
					yield return num;
				}
				Dictionary<object, float>.ValueCollection.Enumerator enumerator = default(Dictionary<object, float>.ValueCollection.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000A228 File Offset: 0x00008428
		public void SetDebugInput(object key, float value)
		{
			this.debugInputValues[key] = value;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000A237 File Offset: 0x00008437
		public void ClearDebugInputFrom(object key)
		{
			if (this.debugInputValues.ContainsKey(key))
			{
				this.debugInputValues.Remove(key);
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000A254 File Offset: 0x00008454
		public void SetViewRectAround(SimpleCurve curve)
		{
			if (curve.PointsCount == 0)
			{
				this.rect = SimpleCurveView.identityRect;
				return;
			}
			this.rect.xMin = (from pt in curve.Points
			select pt.Loc.x).Min();
			this.rect.xMax = (from pt in curve.Points
			select pt.Loc.x).Max();
			this.rect.yMin = (from pt in curve.Points
			select pt.Loc.y).Min();
			this.rect.yMax = (from pt in curve.Points
			select pt.Loc.y).Max();
			if (Mathf.Approximately(this.rect.width, 0f))
			{
				this.rect.width = this.rect.xMin * 2f;
			}
			if (Mathf.Approximately(this.rect.height, 0f))
			{
				this.rect.height = this.rect.yMin * 2f;
			}
			if (Mathf.Approximately(this.rect.width, 0f))
			{
				this.rect.width = 1f;
			}
			if (Mathf.Approximately(this.rect.height, 0f))
			{
				this.rect.height = 1f;
			}
			float width = this.rect.width;
			float height = this.rect.height;
			this.rect.xMin = this.rect.xMin - width * 0.1f;
			this.rect.xMax = this.rect.xMax + width * 0.1f;
			this.rect.yMin = this.rect.yMin - height * 0.1f;
			this.rect.yMax = this.rect.yMax + height * 0.1f;
		}

		// Token: 0x04000081 RID: 129
		public Rect rect;

		// Token: 0x04000082 RID: 130
		private Dictionary<object, float> debugInputValues = new Dictionary<object, float>();

		// Token: 0x04000083 RID: 131
		private const float ResetZoomBuffer = 0.1f;

		// Token: 0x04000084 RID: 132
		private static Rect identityRect = new Rect(0f, 0f, 1f, 1f);
	}
}

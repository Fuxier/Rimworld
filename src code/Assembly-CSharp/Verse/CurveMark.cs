using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200052B RID: 1323
	public struct CurveMark
	{
		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x0600287A RID: 10362 RVA: 0x00105357 File Offset: 0x00103557
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x0600287B RID: 10363 RVA: 0x0010535F File Offset: 0x0010355F
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x0600287C RID: 10364 RVA: 0x00105367 File Offset: 0x00103567
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x0010536F File Offset: 0x0010356F
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x04001A88 RID: 6792
		private float x;

		// Token: 0x04001A89 RID: 6793
		private string message;

		// Token: 0x04001A8A RID: 6794
		private Color color;
	}
}

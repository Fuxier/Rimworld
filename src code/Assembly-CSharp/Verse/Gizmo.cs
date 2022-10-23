using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004A0 RID: 1184
	public abstract class Gizmo
	{
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x060023A2 RID: 9122 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x060023A3 RID: 9123 RVA: 0x0008DB4C File Offset: 0x0008BD4C
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x060023A4 RID: 9124 RVA: 0x000E3FE0 File Offset: 0x000E21E0
		// (set) Token: 0x060023A5 RID: 9125 RVA: 0x000E3FE8 File Offset: 0x000E21E8
		public virtual float Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		// Token: 0x060023A6 RID: 9126
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms);

		// Token: 0x060023A7 RID: 9127 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x060023A8 RID: 9128
		public abstract float GetWidth(float maxWidth);

		// Token: 0x060023A9 RID: 9129 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x000E3FF1 File Offset: 0x000E21F1
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x000E3FF9 File Offset: 0x000E21F9
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x000E4002 File Offset: 0x000E2202
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}

		// Token: 0x040016F3 RID: 5875
		public bool disabled;

		// Token: 0x040016F4 RID: 5876
		public string disabledReason;

		// Token: 0x040016F5 RID: 5877
		public bool alsoClickIfOtherInGroupClicked = true;

		// Token: 0x040016F6 RID: 5878
		private float order;

		// Token: 0x040016F7 RID: 5879
		public const float Height = 75f;
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C7 RID: 967
	public class MechShield : ThingWithComps
	{
		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001BA6 RID: 7078 RVA: 0x000AA154 File Offset: 0x000A8354
		public override Vector3 DrawPos
		{
			get
			{
				if (this.target == null)
				{
					return base.DrawPos;
				}
				return this.target.DrawPos;
			}
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x000AA170 File Offset: 0x000A8370
		public void SetTarget(Thing target)
		{
			this.target = target;
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x000AA179 File Offset: 0x000A8379
		public bool IsTargeting(Thing target)
		{
			return this.target == target;
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x000AA184 File Offset: 0x000A8384
		public override void Draw()
		{
			base.Comps_PostDraw();
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x000AA18C File Offset: 0x000A838C
		public override void Tick()
		{
			base.Tick();
			if (this.target != null)
			{
				base.Position = this.target.Position;
			}
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x000AA1AD File Offset: 0x000A83AD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x040013FF RID: 5119
		private Thing target;
	}
}

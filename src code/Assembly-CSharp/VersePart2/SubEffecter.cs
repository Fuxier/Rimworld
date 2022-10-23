using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200054B RID: 1355
	public class SubEffecter
	{
		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002980 RID: 10624 RVA: 0x0010959C File Offset: 0x0010779C
		public Color EffectiveColor
		{
			get
			{
				Color? color = this.colorOverride;
				if (color == null)
				{
					return this.def.color;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x001095CC File Offset: 0x001077CC
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void SubCleanup()
		{
		}

		// Token: 0x04001B6E RID: 7022
		public Effecter parent;

		// Token: 0x04001B6F RID: 7023
		public SubEffecterDef def;

		// Token: 0x04001B70 RID: 7024
		public Color? colorOverride;
	}
}

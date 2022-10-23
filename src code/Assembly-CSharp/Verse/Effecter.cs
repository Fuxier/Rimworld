using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200054A RID: 1354
	public class Effecter
	{
		// Token: 0x0600297C RID: 10620 RVA: 0x0010948C File Offset: 0x0010768C
		public Effecter(EffecterDef def)
		{
			this.def = def;
			for (int i = 0; i < def.children.Count; i++)
			{
				this.children.Add(def.children[i].Spawn(this));
			}
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x001094F8 File Offset: 0x001076F8
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x00109530 File Offset: 0x00107730
		public void Trigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B, overrideSpawnTick);
			}
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x00109568 File Offset: 0x00107768
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}

		// Token: 0x04001B69 RID: 7017
		public EffecterDef def;

		// Token: 0x04001B6A RID: 7018
		public List<SubEffecter> children = new List<SubEffecter>();

		// Token: 0x04001B6B RID: 7019
		public int ticksLeft = -1;

		// Token: 0x04001B6C RID: 7020
		public Vector3 offset;

		// Token: 0x04001B6D RID: 7021
		public float scale = 1f;
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F0 RID: 240
	public class EffecterDef : Def
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x00024B19 File Offset: 0x00022D19
		public Effecter Spawn()
		{
			return new Effecter(this);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00024B24 File Offset: 0x00022D24
		public Effecter Spawn(IntVec3 target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo targetInfo = new TargetInfo(target, map, false);
			effecter.scale = scale;
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00024B54 File Offset: 0x00022D54
		public Effecter Spawn(IntVec3 targetA, IntVec3 targetB, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo a = new TargetInfo(targetA, map, false);
			effecter.scale = scale;
			effecter.Trigger(a, new TargetInfo(targetB, map, false), -1);
			return effecter;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00024B8C File Offset: 0x00022D8C
		public Effecter Spawn(IntVec3 target, Map map, Vector3 offset, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo targetInfo = new TargetInfo(target, map, false);
			effecter.scale = scale;
			effecter.offset = offset;
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00024BC4 File Offset: 0x00022DC4
		public Effecter Spawn(Thing target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = target.TrueCenter() - target.Position.ToVector3Shifted();
			effecter.scale = scale;
			TargetInfo targetInfo = new TargetInfo(target.Position, map, false);
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00024C18 File Offset: 0x00022E18
		public Effecter SpawnAttached(Thing target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = target.TrueCenter() - target.Position.ToVector3Shifted();
			effecter.scale = scale;
			effecter.Trigger(target, target, -1);
			return effecter;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00024C64 File Offset: 0x00022E64
		public Effecter Spawn(Thing target, Map map, Vector3 offset)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = offset;
			TargetInfo targetInfo = new TargetInfo(target.Position, map, false);
			effecter.Trigger(targetInfo, targetInfo, -1);
			return effecter;
		}

		// Token: 0x04000570 RID: 1392
		public List<SubEffecterDef> children;

		// Token: 0x04000571 RID: 1393
		public float positionRadius;

		// Token: 0x04000572 RID: 1394
		public FloatRange offsetTowardsTarget;
	}
}

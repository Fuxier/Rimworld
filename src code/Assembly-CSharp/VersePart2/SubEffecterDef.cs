using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F2 RID: 242
	public class SubEffecterDef
	{
		// Token: 0x060006DB RID: 1755 RVA: 0x00024C96 File Offset: 0x00022E96
		public SubEffecter Spawn(Effecter parent)
		{
			return (SubEffecter)Activator.CreateInstance(this.subEffecterClass, new object[]
			{
				this,
				parent
			});
		}

		// Token: 0x0400057A RID: 1402
		public Type subEffecterClass;

		// Token: 0x0400057B RID: 1403
		public IntRange burstCount = new IntRange(1, 1);

		// Token: 0x0400057C RID: 1404
		public int ticksBetweenMotes = 40;

		// Token: 0x0400057D RID: 1405
		public int maxMoteCount = int.MaxValue;

		// Token: 0x0400057E RID: 1406
		public int initialDelayTicks;

		// Token: 0x0400057F RID: 1407
		public float chancePerTick = 0.1f;

		// Token: 0x04000580 RID: 1408
		public MoteSpawnLocType spawnLocType = MoteSpawnLocType.BetweenPositions;

		// Token: 0x04000581 RID: 1409
		public float positionLerpFactor = 0.5f;

		// Token: 0x04000582 RID: 1410
		public Vector3 positionOffset = Vector3.zero;

		// Token: 0x04000583 RID: 1411
		public float positionRadius;

		// Token: 0x04000584 RID: 1412
		public float positionRadiusMin;

		// Token: 0x04000585 RID: 1413
		public List<Vector3> perRotationOffsets;

		// Token: 0x04000586 RID: 1414
		public Vector3? positionDimensions;

		// Token: 0x04000587 RID: 1415
		public bool attachToSpawnThing;

		// Token: 0x04000588 RID: 1416
		public float avoidLastPositionRadius;

		// Token: 0x04000589 RID: 1417
		public ThingDef moteDef;

		// Token: 0x0400058A RID: 1418
		public FleckDef fleckDef;

		// Token: 0x0400058B RID: 1419
		public Color color = Color.white;

		// Token: 0x0400058C RID: 1420
		public FloatRange angle = new FloatRange(0f, 360f);

		// Token: 0x0400058D RID: 1421
		public bool absoluteAngle;

		// Token: 0x0400058E RID: 1422
		public bool useTargetAInitialRotation;

		// Token: 0x0400058F RID: 1423
		public bool useTargetBInitialRotation;

		// Token: 0x04000590 RID: 1424
		public bool fleckUsesAngleForVelocity;

		// Token: 0x04000591 RID: 1425
		public bool rotateTowardsTargetCenter;

		// Token: 0x04000592 RID: 1426
		public FloatRange speed = new FloatRange(0f, 0f);

		// Token: 0x04000593 RID: 1427
		public FloatRange rotation = new FloatRange(0f, 360f);

		// Token: 0x04000594 RID: 1428
		public FloatRange rotationRate = new FloatRange(0f, 0f);

		// Token: 0x04000595 RID: 1429
		public FloatRange scale = new FloatRange(1f, 1f);

		// Token: 0x04000596 RID: 1430
		public FloatRange airTime = new FloatRange(999999f, 999999f);

		// Token: 0x04000597 RID: 1431
		public SoundDef soundDef;

		// Token: 0x04000598 RID: 1432
		public IntRange intermittentSoundInterval = new IntRange(300, 600);

		// Token: 0x04000599 RID: 1433
		public int ticksBeforeSustainerStart;
	}
}

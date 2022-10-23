using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000E4 RID: 228
	public class SkyfallerProperties
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0002362D File Offset: 0x0002182D
		public bool MakesShrapnel
		{
			get
			{
				return this.metalShrapnelCountRange.max > 0 || this.rubbleShrapnelCountRange.max > 0;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0002364D File Offset: 0x0002184D
		public bool CausesExplosion
		{
			get
			{
				return this.explosionDamage != null && this.explosionRadius > 0f;
			}
		}

		// Token: 0x040004B7 RID: 1207
		public bool hitRoof = true;

		// Token: 0x040004B8 RID: 1208
		public IntRange ticksToImpactRange = new IntRange(120, 200);

		// Token: 0x040004B9 RID: 1209
		public IntRange ticksToDiscardInReverse = IntRange.zero;

		// Token: 0x040004BA RID: 1210
		public bool reversed;

		// Token: 0x040004BB RID: 1211
		public float explosionRadius = 3f;

		// Token: 0x040004BC RID: 1212
		public DamageDef explosionDamage;

		// Token: 0x040004BD RID: 1213
		public bool damageSpawnedThings;

		// Token: 0x040004BE RID: 1214
		public float explosionDamageFactor = 1f;

		// Token: 0x040004BF RID: 1215
		public IntRange metalShrapnelCountRange = IntRange.zero;

		// Token: 0x040004C0 RID: 1216
		public IntRange rubbleShrapnelCountRange = IntRange.zero;

		// Token: 0x040004C1 RID: 1217
		public float shrapnelDistanceFactor = 1f;

		// Token: 0x040004C2 RID: 1218
		public SkyfallerMovementType movementType;

		// Token: 0x040004C3 RID: 1219
		public float speed = 1f;

		// Token: 0x040004C4 RID: 1220
		public string shadow = "Things/Skyfaller/SkyfallerShadowCircle";

		// Token: 0x040004C5 RID: 1221
		public Vector2 shadowSize = Vector2.one;

		// Token: 0x040004C6 RID: 1222
		public float cameraShake;

		// Token: 0x040004C7 RID: 1223
		public SoundDef impactSound;

		// Token: 0x040004C8 RID: 1224
		public bool rotateGraphicTowardsDirection;

		// Token: 0x040004C9 RID: 1225
		public SoundDef anticipationSound;

		// Token: 0x040004CA RID: 1226
		public SoundDef floatingSound;

		// Token: 0x040004CB RID: 1227
		public int anticipationSoundTicks = 100;

		// Token: 0x040004CC RID: 1228
		public int motesPerCell = 3;

		// Token: 0x040004CD RID: 1229
		public float moteSpawnTime = float.MinValue;

		// Token: 0x040004CE RID: 1230
		public SimpleCurve xPositionCurve;

		// Token: 0x040004CF RID: 1231
		public SimpleCurve zPositionCurve;

		// Token: 0x040004D0 RID: 1232
		public SimpleCurve angleCurve;

		// Token: 0x040004D1 RID: 1233
		public SimpleCurve rotationCurve;

		// Token: 0x040004D2 RID: 1234
		public SimpleCurve speedCurve;

		// Token: 0x040004D3 RID: 1235
		public int fadeInTicks;

		// Token: 0x040004D4 RID: 1236
		public int fadeOutTicks;
	}
}

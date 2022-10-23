using System;

namespace Verse
{
	// Token: 0x0200059E RID: 1438
	public static class ShootTuning
	{
		// Token: 0x04001CB8 RID: 7352
		public const float DistTouch = 3f;

		// Token: 0x04001CB9 RID: 7353
		public const float DistShort = 12f;

		// Token: 0x04001CBA RID: 7354
		public const float DistMedium = 25f;

		// Token: 0x04001CBB RID: 7355
		public const float DistLong = 40f;

		// Token: 0x04001CBC RID: 7356
		public const float MeleeRange = 1.42f;

		// Token: 0x04001CBD RID: 7357
		public const float HitChanceFactorFromEquipmentMin = 0.01f;

		// Token: 0x04001CBE RID: 7358
		public const float MinAccuracyFactorFromShooterAndDistance = 0.0201f;

		// Token: 0x04001CBF RID: 7359
		public const float LayingDownHitChanceFactorMinDistance = 4.5f;

		// Token: 0x04001CC0 RID: 7360
		public const float HitChanceFactorIfLayingDown = 0.2f;

		// Token: 0x04001CC1 RID: 7361
		public const float ExecutionMaxDistance = 3.9f;

		// Token: 0x04001CC2 RID: 7362
		public const float ExecutionAccuracyFactor = 7.5f;

		// Token: 0x04001CC3 RID: 7363
		public const float TargetSizeFactorFromFillPercentFactor = 2.5f;

		// Token: 0x04001CC4 RID: 7364
		public const float TargetSizeFactorMin = 0.5f;

		// Token: 0x04001CC5 RID: 7365
		public const float TargetSizeFactorMax = 2f;

		// Token: 0x04001CC6 RID: 7366
		public const float MinAimOnChance_StandardTarget = 0.0201f;

		// Token: 0x04001CC7 RID: 7367
		public static readonly SimpleSurface MissDistanceFromAimOnChanceCurves = new SimpleSurface
		{
			new SurfaceColumn(0.02f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 10f),
					true
				}
			}),
			new SurfaceColumn(0.04f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 8f),
					true
				}
			}),
			new SurfaceColumn(0.07f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 6f),
					true
				}
			}),
			new SurfaceColumn(0.11f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 4f),
					true
				}
			}),
			new SurfaceColumn(0.22f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 2f),
					true
				}
			}),
			new SurfaceColumn(1f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				}
			})
		};

		// Token: 0x04001CC8 RID: 7368
		public const float CanInterceptPawnsChanceOnWildOrForcedMissRadius = 0.5f;

		// Token: 0x04001CC9 RID: 7369
		public const float InterceptDistMin = 5f;

		// Token: 0x04001CCA RID: 7370
		public const float InterceptDistMax = 12f;

		// Token: 0x04001CCB RID: 7371
		public const float Intercept_Pawn_HitChancePerBodySize = 0.4f;

		// Token: 0x04001CCC RID: 7372
		public const float Intercept_Pawn_HitChanceFactor_LayingDown = 0.1f;

		// Token: 0x04001CCD RID: 7373
		[Obsolete]
		public const float Intercept_Pawn_HitChanceFactor_NonWildNonEnemy = 0.4f;

		// Token: 0x04001CCE RID: 7374
		public const float Intercept_Object_HitChancePerFillPercent = 0.15f;

		// Token: 0x04001CCF RID: 7375
		public const float Intercept_Object_AdjToTarget_HitChancePerFillPercent = 1f;

		// Token: 0x04001CD0 RID: 7376
		public const float Intercept_OpenDoor_HitChance = 0.05f;

		// Token: 0x04001CD1 RID: 7377
		public const float ImpactCell_Pawn_HitChancePerBodySize = 0.5f;

		// Token: 0x04001CD2 RID: 7378
		public const float ImpactCell_Object_HitChancePerFillPercent = 1.5f;

		// Token: 0x04001CD3 RID: 7379
		public const float BodySizeClampMin = 0.1f;

		// Token: 0x04001CD4 RID: 7380
		public const float BodySizeClampMax = 2f;
	}
}

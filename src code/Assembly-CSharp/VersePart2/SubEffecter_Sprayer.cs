using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200054F RID: 1359
	public abstract class SubEffecter_Sprayer : SubEffecter
	{
		// Token: 0x0600298C RID: 10636 RVA: 0x0001A6A9 File Offset: 0x000188A9
		public SubEffecter_Sprayer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x001096C0 File Offset: 0x001078C0
		protected void MakeMote(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
			Vector3 vector = Vector3.zero;
			switch (this.def.spawnLocType)
			{
			case MoteSpawnLocType.OnSource:
				vector = A.CenterVector3;
				break;
			case MoteSpawnLocType.BetweenPositions:
			{
				Vector3 vector2 = A.HasThing ? A.Thing.DrawPos : A.Cell.ToVector3Shifted();
				Vector3 vector3 = B.HasThing ? B.Thing.DrawPos : B.Cell.ToVector3Shifted();
				if (A.HasThing && !A.Thing.Spawned)
				{
					vector = vector3;
				}
				else if (B.HasThing && !B.Thing.Spawned)
				{
					vector = vector2;
				}
				else
				{
					vector = vector2 * this.def.positionLerpFactor + vector3 * (1f - this.def.positionLerpFactor);
				}
				break;
			}
			case MoteSpawnLocType.BetweenTouchingCells:
				vector = A.Cell.ToVector3Shifted() + (B.Cell - A.Cell).ToVector3().normalized * 0.5f;
				break;
			case MoteSpawnLocType.RandomCellOnTarget:
			{
				CellRect cellRect;
				if (B.HasThing)
				{
					cellRect = B.Thing.OccupiedRect();
				}
				else
				{
					cellRect = CellRect.CenteredOn(B.Cell, 0);
				}
				vector = cellRect.RandomCell.ToVector3Shifted();
				break;
			}
			case MoteSpawnLocType.RandomDrawPosOnTarget:
				if (B.Thing.DrawSize != Vector2.one && B.Thing.DrawSize != Vector2.zero)
				{
					Vector2 vector4 = B.Thing.DrawSize.RotatedBy(B.Thing.Rotation);
					Vector3 b = new Vector3(vector4.x * Rand.Value, 0f, vector4.y * Rand.Value);
					vector = B.CenterVector3 + b - new Vector3(vector4.x / 2f, 0f, vector4.y / 2f);
				}
				else
				{
					Vector3 b2 = new Vector3(Rand.Value, 0f, Rand.Value);
					vector = B.CenterVector3 + b2 - new Vector3(0.5f, 0f, 0.5f);
				}
				break;
			case MoteSpawnLocType.OnTarget:
				vector = B.CenterVector3;
				break;
			}
			if (this.parent != null)
			{
				Rand.PushState(this.parent.GetHashCode());
				if (A.CenterVector3 != B.CenterVector3)
				{
					vector += (B.CenterVector3 - A.CenterVector3).normalized * this.parent.def.offsetTowardsTarget.RandomInRange;
				}
				Vector3 a = Gen.RandomHorizontalVector(this.parent.def.positionRadius);
				Rand.PopState();
				if (this.def.positionDimensions != null)
				{
					a += Gen.Random2DVector(this.def.positionDimensions.Value);
				}
				vector += a + this.parent.offset;
			}
			Map map = A.Map ?? B.Map;
			float num;
			if (this.def.absoluteAngle)
			{
				num = 0f;
			}
			else if (this.def.useTargetAInitialRotation && A.HasThing)
			{
				num = A.Thing.Rotation.AsAngle;
			}
			else if (this.def.useTargetBInitialRotation && B.HasThing)
			{
				num = B.Thing.Rotation.AsAngle;
			}
			else
			{
				num = (B.Cell - A.Cell).AngleFlat;
			}
			float num2 = (this.parent != null) ? this.parent.scale : 1f;
			if (map != null)
			{
				int randomInRange = this.def.burstCount.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					Vector3 vector5 = this.def.positionOffset;
					if (this.def.useTargetAInitialRotation && A.HasThing)
					{
						vector5 = vector5.RotatedBy(A.Thing.Rotation);
					}
					else if (this.def.useTargetBInitialRotation && B.HasThing)
					{
						vector5 = vector5.RotatedBy(B.Thing.Rotation);
					}
					if (!this.def.perRotationOffsets.NullOrEmpty<Vector3>())
					{
						vector5 += this.def.perRotationOffsets[((this.def.spawnLocType == MoteSpawnLocType.OnSource) ? A.Thing.Rotation : B.Thing.Rotation).AsInt];
					}
					for (int j = 0; j < 5; j++)
					{
						vector5 = vector5 * num2 + Rand.InsideAnnulusVector3(this.def.positionRadiusMin, this.def.positionRadius) * num2;
						if (this.def.avoidLastPositionRadius < 1E-45f || this.lastOffset == null || (vector5 - this.lastOffset.Value).MagnitudeHorizontal() > this.def.avoidLastPositionRadius)
						{
							break;
						}
					}
					this.lastOffset = new Vector3?(vector5);
					Vector3 vector6 = vector + vector5;
					if (this.def.rotateTowardsTargetCenter && B.HasThing)
					{
						num = (vector6 - B.CenterVector3).AngleFlat();
					}
					if (this.def.moteDef != null && vector.ShouldSpawnMotesAt(map, this.def.moteDef.drawOffscreen))
					{
						this.mote = (Mote)ThingMaker.MakeThing(this.def.moteDef, null);
						GenSpawn.Spawn(this.mote, vector.ToIntVec3(), map, WipeMode.Vanish);
						this.mote.Scale = this.def.scale.RandomInRange * num2;
						this.mote.exactPosition = vector6;
						this.mote.rotationRate = this.def.rotationRate.RandomInRange;
						this.mote.exactRotation = this.def.rotation.RandomInRange + num;
						this.mote.instanceColor = base.EffectiveColor;
						if (overrideSpawnTick != -1)
						{
							this.mote.ForceSpawnTick(overrideSpawnTick);
						}
						MoteThrown moteThrown = this.mote as MoteThrown;
						if (moteThrown != null)
						{
							moteThrown.airTimeLeft = this.def.airTime.RandomInRange;
							moteThrown.SetVelocity(this.def.angle.RandomInRange + num, this.def.speed.RandomInRange);
						}
						if (this.def.attachToSpawnThing)
						{
							MoteAttached moteAttached = this.mote as MoteAttached;
							if (moteAttached != null)
							{
								if (this.def.spawnLocType == MoteSpawnLocType.OnSource && A.HasThing)
								{
									moteAttached.Attach(A, vector5);
								}
								else if (this.def.spawnLocType == MoteSpawnLocType.OnTarget && B.HasThing)
								{
									moteAttached.Attach(B, vector5);
								}
							}
						}
						this.mote.Maintain();
					}
					else if (this.def.fleckDef != null && vector6.ShouldSpawnMotesAt(map, this.def.fleckDef.drawOffscreen))
					{
						float velocityAngle = this.def.fleckUsesAngleForVelocity ? (this.def.angle.RandomInRange + num) : 0f;
						map.flecks.CreateFleck(new FleckCreationData
						{
							def = this.def.fleckDef,
							scale = this.def.scale.RandomInRange * num2,
							spawnPosition = vector6,
							rotationRate = this.def.rotationRate.RandomInRange,
							rotation = this.def.rotation.RandomInRange + num,
							instanceColor = new Color?(base.EffectiveColor),
							velocitySpeed = this.def.speed.RandomInRange,
							velocityAngle = velocityAngle,
							ageTicksOverride = overrideSpawnTick
						});
					}
				}
			}
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x00109F48 File Offset: 0x00108148
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			base.SubEffectTick(A, B);
			if (this.mote != null && this.mote.def.mote.needsMaintenance)
			{
				this.mote.Maintain();
			}
		}

		// Token: 0x04001B73 RID: 7027
		private Mote mote;

		// Token: 0x04001B74 RID: 7028
		private Vector3? lastOffset;
	}
}

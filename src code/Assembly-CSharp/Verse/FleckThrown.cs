using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020001D1 RID: 465
	public struct FleckThrown : IFleck
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x0004898F File Offset: 0x00046B8F
		public bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x0004899E File Offset: 0x00046B9E
		public bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x000489B7 File Offset: 0x00046BB7
		// (set) Token: 0x06000CFD RID: 3325 RVA: 0x000489BF File Offset: 0x00046BBF
		public Vector3 Velocity
		{
			get
			{
				return this.velocity;
			}
			set
			{
				this.velocity = value;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x000489C8 File Offset: 0x00046BC8
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x000489D5 File Offset: 0x00046BD5
		public float MoveAngle
		{
			get
			{
				return this.velocity.AngleFlat();
			}
			set
			{
				this.SetVelocity(value, this.Speed);
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x000489E4 File Offset: 0x00046BE4
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x000489F4 File Offset: 0x00046BF4
		public float Speed
		{
			get
			{
				return this.velocity.MagnitudeHorizontal();
			}
			set
			{
				if (value == 0f)
				{
					this.velocity = Vector3.zero;
					return;
				}
				if (this.velocity == Vector3.zero)
				{
					this.velocity = new Vector3(value, 0f, 0f);
					return;
				}
				this.velocity = this.velocity.normalized * value;
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00048A58 File Offset: 0x00046C58
		public void Setup(FleckCreationData creationData)
		{
			this.baseData = default(FleckStatic);
			this.baseData.Setup(creationData);
			this.airTimeLeft = (creationData.airTimeLeft ?? 999999f);
			this.attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);
			this.link = creationData.link;
			if (this.link.Linked)
			{
				this.attacheeLastPosition = this.link.LastDrawPos;
			}
			this.baseData.exactPosition = this.baseData.exactPosition + creationData.def.attachedDrawOffset;
			this.rotationRate = creationData.rotationRate;
			this.SetVelocity(creationData.velocityAngle, creationData.velocitySpeed);
			if (creationData.velocity != null)
			{
				this.velocity += creationData.velocity.Value;
			}
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00048B54 File Offset: 0x00046D54
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.baseData.TimeInterval(deltaTime, map))
			{
				return true;
			}
			if (!this.Flying && !this.Skidding)
			{
				return false;
			}
			Vector3 vector = this.NextExactPosition(deltaTime);
			IntVec3 intVec = new IntVec3(vector);
			if (intVec != new IntVec3(this.baseData.exactPosition))
			{
				if (!intVec.InBounds(map))
				{
					return true;
				}
				if (this.baseData.def.collide && intVec.Filled(map))
				{
					this.WallHit();
					return false;
				}
			}
			this.baseData.exactPosition = vector;
			if (this.baseData.def.rotateTowardsMoveDirection && this.velocity != default(Vector3))
			{
				this.baseData.exactRotation = this.velocity.AngleFlat() + this.baseData.def.rotateTowardsMoveDirectionExtraAngle;
			}
			else
			{
				this.baseData.exactRotation = this.baseData.exactRotation + this.rotationRate * deltaTime;
			}
			this.velocity += this.baseData.def.acceleration * deltaTime;
			if (this.baseData.def.speedPerTime != FloatRange.Zero)
			{
				this.Speed = Mathf.Max(this.Speed + this.baseData.def.speedPerTime.RandomInRange * deltaTime, 0f);
			}
			if (this.airTimeLeft > 0f)
			{
				this.airTimeLeft -= deltaTime;
				if (this.airTimeLeft < 0f)
				{
					this.airTimeLeft = 0f;
				}
				if (this.airTimeLeft <= 0f && !this.baseData.def.landSound.NullOrUndefined())
				{
					this.baseData.def.landSound.PlayOneShot(new TargetInfo(new IntVec3(this.baseData.exactPosition), map, false));
				}
			}
			if (this.Skidding)
			{
				this.Speed *= this.baseData.skidSpeedMultiplierPerTick;
				this.rotationRate *= this.baseData.skidSpeedMultiplierPerTick;
				if (this.Speed < 0.02f)
				{
					this.Speed = 0f;
				}
			}
			return false;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00048D98 File Offset: 0x00046F98
		private Vector3 NextExactPosition(float deltaTime)
		{
			Vector3 vector = this.baseData.exactPosition + this.velocity * deltaTime;
			if (this.link.Linked)
			{
				bool flag = this.link.detachAfterTicks == -1 || this.baseData.ageTicks < this.link.detachAfterTicks;
				if (!this.link.Target.ThingDestroyed && flag)
				{
					this.link.UpdateDrawPos();
				}
				Vector3 b = this.baseData.def.attachedDrawOffset;
				if (this.baseData.def.attachedToHead)
				{
					Pawn pawn = this.link.Target.Thing as Pawn;
					if (pawn != null && pawn.story != null)
					{
						b = pawn.Drawer.renderer.BaseHeadOffsetAt((pawn.GetPosture() == PawnPosture.Standing) ? Rot4.North : pawn.Drawer.renderer.LayingFacing()).RotatedBy(pawn.Drawer.renderer.BodyAngle());
					}
				}
				Vector3 b2 = this.link.LastDrawPos - this.attacheeLastPosition;
				vector += b2;
				vector += b;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				this.attacheeLastPosition = this.link.LastDrawPos;
			}
			return vector;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00048EFD File Offset: 0x000470FD
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00048F20 File Offset: 0x00047120
		public void Draw(DrawBatch batch)
		{
			this.baseData.Draw(batch);
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00048F2E File Offset: 0x0004712E
		private void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x04000BE3 RID: 3043
		public FleckStatic baseData;

		// Token: 0x04000BE4 RID: 3044
		public float airTimeLeft;

		// Token: 0x04000BE5 RID: 3045
		public Vector3 velocity;

		// Token: 0x04000BE6 RID: 3046
		public float rotationRate;

		// Token: 0x04000BE7 RID: 3047
		public FleckAttachLink link;

		// Token: 0x04000BE8 RID: 3048
		private Vector3 attacheeLastPosition;

		// Token: 0x04000BE9 RID: 3049
		public const float MinSpeed = 0.02f;
	}
}

using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003F9 RID: 1017
	public class MoteThrown : Mote
	{
		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x000AF822 File Offset: 0x000ADA22
		protected bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x000AF831 File Offset: 0x000ADA31
		protected bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x000AF84A File Offset: 0x000ADA4A
		// (set) Token: 0x06001CF1 RID: 7409 RVA: 0x000AF852 File Offset: 0x000ADA52
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

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x000AF85B File Offset: 0x000ADA5B
		// (set) Token: 0x06001CF3 RID: 7411 RVA: 0x000AF868 File Offset: 0x000ADA68
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

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x000AF877 File Offset: 0x000ADA77
		// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x000AF884 File Offset: 0x000ADA84
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

		// Token: 0x06001CF6 RID: 7414 RVA: 0x000AF8E8 File Offset: 0x000ADAE8
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			if (!this.Flying && !this.Skidding)
			{
				return;
			}
			Vector3 vector = this.NextExactPosition(deltaTime);
			IntVec3 intVec = new IntVec3(vector);
			if (intVec != base.Position)
			{
				if (!intVec.InBounds(base.Map))
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				if (this.def.mote.collide && intVec.Filled(base.Map))
				{
					this.WallHit();
					return;
				}
			}
			base.Position = intVec;
			this.exactPosition = vector;
			if (this.def.mote.rotateTowardsMoveDirection && this.velocity != default(Vector3))
			{
				this.exactRotation = this.velocity.AngleFlat();
			}
			else
			{
				this.exactRotation += this.rotationRate * deltaTime;
			}
			this.velocity += this.def.mote.acceleration * deltaTime;
			if (this.def.mote.speedPerTime != 0f)
			{
				this.Speed = Mathf.Max(this.Speed + this.def.mote.speedPerTime * deltaTime, 0f);
			}
			if (this.airTimeLeft > 0f)
			{
				this.airTimeLeft -= deltaTime;
				if (this.airTimeLeft < 0f)
				{
					this.airTimeLeft = 0f;
				}
				if (this.airTimeLeft <= 0f && !this.def.mote.landSound.NullOrUndefined())
				{
					this.def.mote.landSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
			}
			if (this.Skidding)
			{
				this.Speed *= this.skidSpeedMultiplierPerTick;
				this.rotationRate *= this.skidSpeedMultiplierPerTick;
				if (this.Speed < 0.02f)
				{
					this.Speed = 0f;
				}
			}
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x000AFAFF File Offset: 0x000ADCFF
		protected virtual Vector3 NextExactPosition(float deltaTime)
		{
			return this.exactPosition + this.velocity * deltaTime;
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x000AFB18 File Offset: 0x000ADD18
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x000AFB3B File Offset: 0x000ADD3B
		protected virtual void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x04001483 RID: 5251
		public float airTimeLeft = 999999f;

		// Token: 0x04001484 RID: 5252
		protected Vector3 velocity = Vector3.zero;
	}
}

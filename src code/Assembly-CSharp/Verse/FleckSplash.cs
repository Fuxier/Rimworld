using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001CD RID: 461
	public struct FleckSplash : IFleck
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x000481FF File Offset: 0x000463FF
		public bool EndOfLife
		{
			get
			{
				return this.ageSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x0004821C File Offset: 0x0004641C
		public float Alpha
		{
			get
			{
				Mathf.Clamp01(this.ageSecs * 10f);
				float num = 1f;
				float num2 = Mathf.Clamp01(1f - this.ageSecs / (this.targetSize / this.velocity));
				return num * num2 * this.CalculatedIntensity();
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0004826C File Offset: 0x0004646C
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.EndOfLife)
			{
				return true;
			}
			this.ageSecs += deltaTime;
			if (this.def.growthRate != 0f)
			{
				this.exactScale = new Vector3(this.exactScale.x + this.def.growthRate * deltaTime, this.exactScale.y, this.exactScale.z + this.def.growthRate * deltaTime);
				this.exactScale.x = Mathf.Max(this.exactScale.x, 0.0001f);
				this.exactScale.z = Mathf.Max(this.exactScale.z, 0.0001f);
			}
			float d = this.ageSecs * this.velocity;
			this.exactScale = Vector3.one * d;
			this.position += map.waterInfo.GetWaterMovement(this.position) * deltaTime;
			return false;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00048378 File Offset: 0x00046578
		public void Draw(DrawBatch batch)
		{
			this.position.y = this.def.altitudeLayer.AltitudeFor(this.def.altitudeLayerIncOffset);
			int num = this.setupTick + this.spawnPosition.GetHashCode();
			((Graphic_Fleck)this.def.GetGraphicData(num).Graphic).DrawFleck(new FleckDrawData
			{
				alpha = this.Alpha,
				color = Color.white,
				drawLayer = 0,
				pos = this.position,
				rotation = 0f,
				scale = this.exactScale,
				ageSecs = this.ageSecs,
				calculatedShockwaveSpan = this.CalculatedShockwaveSpan(),
				id = (float)num
			}, batch);
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00048454 File Offset: 0x00046654
		public void Setup(FleckCreationData creationData)
		{
			this.def = creationData.def;
			this.position = creationData.spawnPosition;
			this.spawnPosition = creationData.spawnPosition;
			this.velocity = creationData.velocitySpeed;
			this.targetSize = creationData.targetSize;
			this.setupTick = Find.TickManager.TicksGame;
			if (creationData.ageTicksOverride != -1)
			{
				this.ForceSpawnTick(creationData.ageTicksOverride);
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x000484C2 File Offset: 0x000466C2
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x000484D5 File Offset: 0x000466D5
		public float CalculatedShockwaveSpan()
		{
			return Mathf.Min(Mathf.Sqrt(this.targetSize) * 0.8f, this.exactScale.x) / this.exactScale.x;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00048504 File Offset: 0x00046704
		public void ForceSpawnTick(int tick)
		{
			this.setupTick = tick;
			this.ageSecs = (Find.TickManager.TicksGame - tick).TicksToSeconds();
		}

		// Token: 0x04000BC9 RID: 3017
		public const float VelocityFootstep = 1.5f;

		// Token: 0x04000BCA RID: 3018
		public const float SizeFootstep = 2f;

		// Token: 0x04000BCB RID: 3019
		public const float VelocityGunfire = 4f;

		// Token: 0x04000BCC RID: 3020
		public const float SizeGunfire = 1f;

		// Token: 0x04000BCD RID: 3021
		public const float VelocityExplosion = 20f;

		// Token: 0x04000BCE RID: 3022
		public const float SizeExplosion = 6f;

		// Token: 0x04000BCF RID: 3023
		public FleckDef def;

		// Token: 0x04000BD0 RID: 3024
		private float ageSecs;

		// Token: 0x04000BD1 RID: 3025
		private int setupTick;

		// Token: 0x04000BD2 RID: 3026
		private float targetSize;

		// Token: 0x04000BD3 RID: 3027
		private float velocity;

		// Token: 0x04000BD4 RID: 3028
		private Vector3 position;

		// Token: 0x04000BD5 RID: 3029
		private Vector3 spawnPosition;

		// Token: 0x04000BD6 RID: 3030
		private Vector3 exactScale;
	}
}

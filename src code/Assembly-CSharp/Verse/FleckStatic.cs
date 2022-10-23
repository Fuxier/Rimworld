using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001CF RID: 463
	public struct FleckStatic : IFleck
	{
		// Token: 0x17000270 RID: 624
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x0004852C File Offset: 0x0004672C
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00048540 File Offset: 0x00046740
		public float SolidTime
		{
			get
			{
				if (this.solidTimeOverride >= 0f)
				{
					return this.solidTimeOverride;
				}
				return this.def.solidTime;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x00048561 File Offset: 0x00046761
		public Vector3 DrawPos
		{
			get
			{
				return this.exactPosition + this.def.unattachedDrawOffset;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x00048579 File Offset: 0x00046779
		public bool EndOfLife
		{
			get
			{
				return this.ageSecs >= this.def.Lifespan;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x00048594 File Offset: 0x00046794
		public float Alpha
		{
			get
			{
				float num = this.ageSecs;
				if (num <= this.def.fadeInTime)
				{
					if (this.def.fadeInTime > 0f)
					{
						return num / this.def.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (num <= this.def.fadeInTime + this.SolidTime)
					{
						return 1f;
					}
					if (this.def.fadeOutTime > 0f)
					{
						return 1f - Mathf.InverseLerp(this.def.fadeInTime + this.SolidTime, this.def.fadeInTime + this.SolidTime + this.def.fadeOutTime, num);
					}
					return 1f;
				}
			}
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0004864C File Offset: 0x0004684C
		public void Setup(FleckCreationData creationData)
		{
			this.def = creationData.def;
			this.exactScale = Vector3.one;
			this.instanceColor = (creationData.instanceColor ?? Color.white);
			this.solidTimeOverride = (creationData.solidTimeOverride ?? -1f);
			this.skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);
			this.ageSecs = 0f;
			if (creationData.exactScale != null)
			{
				this.exactScale = creationData.exactScale.Value;
			}
			else
			{
				this.Scale = creationData.scale;
			}
			this.exactPosition = creationData.spawnPosition;
			this.spawnPosition = creationData.spawnPosition;
			this.exactRotation = creationData.rotation;
			this.setupTick = Find.TickManager.TicksGame;
			if (creationData.ageTicksOverride != -1)
			{
				this.ForceSpawnTick(creationData.ageTicksOverride);
			}
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00048750 File Offset: 0x00046950
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.EndOfLife)
			{
				return true;
			}
			this.ageSecs += deltaTime;
			this.ageTicks++;
			if (this.def.growthRate != 0f)
			{
				float num = Mathf.Sign(this.exactScale.x);
				float num2 = Mathf.Sign(this.exactScale.z);
				this.exactScale = new Vector3(this.exactScale.x + num * (this.def.growthRate * deltaTime), this.exactScale.y, this.exactScale.z + num2 * (this.def.growthRate * deltaTime));
				this.exactScale.x = ((num > 0f) ? Mathf.Max(this.exactScale.x, 0.0001f) : Mathf.Min(this.exactScale.x, -0.0001f));
				this.exactScale.z = ((num2 > 0f) ? Mathf.Max(this.exactScale.z, 0.0001f) : Mathf.Min(this.exactScale.z, -0.0001f));
			}
			return false;
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00048885 File Offset: 0x00046A85
		public void Draw(DrawBatch batch)
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor(this.def.altitudeLayerIncOffset), batch);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x000488AC File Offset: 0x00046AAC
		public void Draw(float altitude, DrawBatch batch)
		{
			this.exactPosition.y = altitude;
			int num = this.setupTick + this.spawnPosition.GetHashCode();
			((Graphic_Fleck)this.def.GetGraphicData(num).Graphic).DrawFleck(new FleckDrawData
			{
				alpha = this.Alpha,
				color = this.instanceColor,
				drawLayer = 0,
				pos = this.DrawPos,
				rotation = this.exactRotation,
				scale = this.exactScale,
				ageSecs = this.ageSecs,
				id = (float)num
			}, batch);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00048962 File Offset: 0x00046B62
		public void ForceSpawnTick(int tick)
		{
			this.ageTicks = Find.TickManager.TicksGame - tick;
			this.ageSecs = this.ageTicks.TicksToSeconds();
		}

		// Token: 0x04000BD7 RID: 3031
		public FleckDef def;

		// Token: 0x04000BD8 RID: 3032
		public Map map;

		// Token: 0x04000BD9 RID: 3033
		public Vector3 exactPosition;

		// Token: 0x04000BDA RID: 3034
		public float exactRotation;

		// Token: 0x04000BDB RID: 3035
		public Vector3 exactScale;

		// Token: 0x04000BDC RID: 3036
		public Color instanceColor;

		// Token: 0x04000BDD RID: 3037
		public float solidTimeOverride;

		// Token: 0x04000BDE RID: 3038
		public float ageSecs;

		// Token: 0x04000BDF RID: 3039
		public int ageTicks;

		// Token: 0x04000BE0 RID: 3040
		public int setupTick;

		// Token: 0x04000BE1 RID: 3041
		public Vector3 spawnPosition;

		// Token: 0x04000BE2 RID: 3042
		public float skidSpeedMultiplierPerTick;
	}
}

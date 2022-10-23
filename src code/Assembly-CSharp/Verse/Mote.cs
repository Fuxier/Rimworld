using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F3 RID: 1011
	public abstract class Mote : Thing
	{
		// Token: 0x170005DA RID: 1498
		// (set) Token: 0x06001CBC RID: 7356 RVA: 0x000AE8ED File Offset: 0x000ACAED
		public float Scale
		{
			set
			{
				this.exactScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x000AE901 File Offset: 0x000ACB01
		public float AgeSecs
		{
			get
			{
				if (this.def.mote.realTime)
				{
					return Time.realtimeSinceStartup - this.spawnRealTime;
				}
				return (float)(Find.TickManager.TicksGame - this.spawnTick - this.pausedTicks) / 60f;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001CBE RID: 7358 RVA: 0x000AE941 File Offset: 0x000ACB41
		public float AgeSecsPausable
		{
			get
			{
				return (float)this.currentAnimationTick / 60f;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001CBF RID: 7359 RVA: 0x000AE950 File Offset: 0x000ACB50
		protected float SolidTime
		{
			get
			{
				if (this.solidTimeOverride >= 0f)
				{
					return this.solidTimeOverride;
				}
				return this.def.mote.solidTime;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x000AE978 File Offset: 0x000ACB78
		public override Vector3 DrawPos
		{
			get
			{
				float z = 0f;
				if (this.def.mote.archDuration > 0f && this.AgeSecs < this.def.mote.archDuration + this.def.mote.archStartOffset)
				{
					z = (Mathf.Cos(Mathf.Clamp01((this.AgeSecs + this.def.mote.archStartOffset) / this.def.mote.archDuration) * 3.1415927f * 2f - 3.1415927f) + 1f) / 2f * this.def.mote.archHeight;
				}
				return this.exactPosition + this.def.mote.unattachedDrawOffset + new Vector3(0f, 0f, z);
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x000AEA60 File Offset: 0x000ACC60
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x000AEA80 File Offset: 0x000ACC80
		public virtual float Alpha
		{
			get
			{
				float ageSecs = this.AgeSecs;
				if (this.def.mote.fadeOutUnmaintained && Find.TickManager.TicksGame - this.lastMaintainTick > 0)
				{
					if (this.def.mote.fadeOutTime > 0f)
					{
						float num = (Find.TickManager.TicksGame - this.lastMaintainTick).TicksToSeconds();
						return 1f - num / this.def.mote.fadeOutTime;
					}
					return 1f;
				}
				else if (ageSecs <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						return ageSecs / this.def.mote.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (ageSecs <= this.def.mote.fadeInTime + this.SolidTime)
					{
						return 1f;
					}
					if (this.def.mote.fadeOutTime > 0f)
					{
						return 1f - Mathf.InverseLerp(this.def.mote.fadeInTime + this.SolidTime, this.def.mote.fadeInTime + this.SolidTime + this.def.mote.fadeOutTime, ageSecs);
					}
					return 1f;
				}
			}
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000AEBD4 File Offset: 0x000ACDD4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor() + this.yOffset;
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000AEC41 File Offset: 0x000ACE41
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000AEC68 File Offset: 0x000ACE68
		public override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.016666668f);
			}
			if (!this.animationPaused)
			{
				this.currentAnimationTick++;
			}
			if (this.paused)
			{
				this.pausedTicks++;
			}
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000AECBE File Offset: 0x000ACEBE
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000AECE0 File Offset: 0x000ACEE0
		protected virtual void TimeInterval(float deltaTime)
		{
			if (this.EndOfLife && !base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.def.mote.needsMaintenance && Find.TickManager.TicksGame - 1 > this.lastMaintainTick)
			{
				int num = this.def.mote.fadeOutTime.SecondsToTicks();
				if (!this.def.mote.fadeOutUnmaintained || Find.TickManager.TicksGame - this.lastMaintainTick > num)
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			if (this.def.mote.growthRate != 0f)
			{
				this.exactScale = new Vector3(this.exactScale.x + this.def.mote.growthRate * deltaTime, this.exactScale.y, this.exactScale.z + this.def.mote.growthRate * deltaTime);
				this.exactScale.x = Mathf.Max(this.exactScale.x, 0.0001f);
				this.exactScale.z = Mathf.Max(this.exactScale.z, 0.0001f);
			}
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000AEE1B File Offset: 0x000AD01B
		public override void Draw()
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x000AEE33 File Offset: 0x000AD033
		public void Draw(float altitude)
		{
			if (!this.paused)
			{
				this.exactPosition.y = altitude + this.yOffset;
				base.Draw();
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000AEE56 File Offset: 0x000AD056
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x000AEE68 File Offset: 0x000AD068
		public void Attach(TargetInfo a, Vector3 offset)
		{
			this.link1 = new MoteAttachLink(a, offset);
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x000AEE77 File Offset: 0x000AD077
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a, Vector3.zero);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x000AEE8A File Offset: 0x000AD08A
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000AEE9D File Offset: 0x000AD09D
		public void ForceSpawnTick(int tick)
		{
			this.spawnTick = tick;
		}

		// Token: 0x04001461 RID: 5217
		public Vector3 exactPosition;

		// Token: 0x04001462 RID: 5218
		public float exactRotation;

		// Token: 0x04001463 RID: 5219
		public Vector3 exactScale = new Vector3(1f, 1f, 1f);

		// Token: 0x04001464 RID: 5220
		public float rotationRate;

		// Token: 0x04001465 RID: 5221
		public float yOffset;

		// Token: 0x04001466 RID: 5222
		public Color instanceColor = Color.white;

		// Token: 0x04001467 RID: 5223
		private int lastMaintainTick;

		// Token: 0x04001468 RID: 5224
		private int currentAnimationTick;

		// Token: 0x04001469 RID: 5225
		public float solidTimeOverride = -1f;

		// Token: 0x0400146A RID: 5226
		public int pausedTicks;

		// Token: 0x0400146B RID: 5227
		public bool paused;

		// Token: 0x0400146C RID: 5228
		public int spawnTick;

		// Token: 0x0400146D RID: 5229
		public bool animationPaused;

		// Token: 0x0400146E RID: 5230
		public int detachAfterTicks = -1;

		// Token: 0x0400146F RID: 5231
		public float spawnRealTime;

		// Token: 0x04001470 RID: 5232
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		// Token: 0x04001471 RID: 5233
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		// Token: 0x04001472 RID: 5234
		public int offsetRandom = Rand.Range(0, 99999);

		// Token: 0x04001473 RID: 5235
		protected const float MinSpeed = 0.02f;
	}
}

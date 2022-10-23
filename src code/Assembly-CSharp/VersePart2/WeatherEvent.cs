using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200026A RID: 618
	public abstract class WeatherEvent
	{
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x0600119E RID: 4510
		public abstract bool Expired { get; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x0006714E File Offset: 0x0006534E
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060011A0 RID: 4512 RVA: 0x0003120D File Offset: 0x0002F40D
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x0006715D File Offset: 0x0006535D
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060011A2 RID: 4514 RVA: 0x00067164 File Offset: 0x00065364
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0006717A File Offset: 0x0006537A
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		// Token: 0x060011A4 RID: 4516
		public abstract void FireEvent();

		// Token: 0x060011A5 RID: 4517
		public abstract void WeatherEventTick();

		// Token: 0x060011A6 RID: 4518 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void WeatherEventDraw()
		{
		}

		// Token: 0x04000F0A RID: 3850
		protected Map map;
	}
}

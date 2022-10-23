using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000150 RID: 336
	public class WeatherDef : Def
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0002B281 File Offset: 0x00029481
		public WeatherWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = new WeatherWorker(this);
				}
				return this.workerInt;
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0002B29D File Offset: 0x0002949D
		public override void PostLoad()
		{
			base.PostLoad();
			this.workerInt = new WeatherWorker(this);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0002B2B1 File Offset: 0x000294B1
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.skyColorsDay.saturation == 0f || this.skyColorsDusk.saturation == 0f || this.skyColorsNightMid.saturation == 0f || this.skyColorsNightEdge.saturation == 0f)
			{
				yield return "a sky color has saturation of 0";
			}
			yield break;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0002B2C1 File Offset: 0x000294C1
		public static WeatherDef Named(string defName)
		{
			return DefDatabase<WeatherDef>.GetNamed(defName, true);
		}

		// Token: 0x04000944 RID: 2372
		public IntRange durationRange = new IntRange(16000, 160000);

		// Token: 0x04000945 RID: 2373
		public bool repeatable;

		// Token: 0x04000946 RID: 2374
		public bool isBad;

		// Token: 0x04000947 RID: 2375
		public Favorability favorability = Favorability.Neutral;

		// Token: 0x04000948 RID: 2376
		public FloatRange temperatureRange = new FloatRange(-999f, 999f);

		// Token: 0x04000949 RID: 2377
		public SimpleCurve commonalityRainfallFactor;

		// Token: 0x0400094A RID: 2378
		public float rainRate;

		// Token: 0x0400094B RID: 2379
		public float snowRate;

		// Token: 0x0400094C RID: 2380
		public float windSpeedFactor = 1f;

		// Token: 0x0400094D RID: 2381
		public float windSpeedOffset;

		// Token: 0x0400094E RID: 2382
		public float moveSpeedMultiplier = 1f;

		// Token: 0x0400094F RID: 2383
		public float accuracyMultiplier = 1f;

		// Token: 0x04000950 RID: 2384
		public float perceivePriority;

		// Token: 0x04000951 RID: 2385
		public ThoughtDef exposedThought;

		// Token: 0x04000952 RID: 2386
		public List<SoundDef> ambientSounds = new List<SoundDef>();

		// Token: 0x04000953 RID: 2387
		public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

		// Token: 0x04000954 RID: 2388
		public List<Type> overlayClasses = new List<Type>();

		// Token: 0x04000955 RID: 2389
		public SkyColorSet skyColorsNightMid;

		// Token: 0x04000956 RID: 2390
		public SkyColorSet skyColorsNightEdge;

		// Token: 0x04000957 RID: 2391
		public SkyColorSet skyColorsDay;

		// Token: 0x04000958 RID: 2392
		public SkyColorSet skyColorsDusk;

		// Token: 0x04000959 RID: 2393
		[Unsaved(false)]
		private WeatherWorker workerInt;
	}
}

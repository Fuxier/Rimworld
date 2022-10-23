using System;

namespace Verse
{
	// Token: 0x0200026C RID: 620
	public class WeatherEventMaker
	{
		// Token: 0x060011AC RID: 4524 RVA: 0x00067248 File Offset: 0x00065448
		public void WeatherEventMakerTick(Map map, float strength)
		{
			if (Rand.Value < 1f / this.averageInterval * strength)
			{
				WeatherEvent newEvent = (WeatherEvent)Activator.CreateInstance(this.eventClass, new object[]
				{
					map
				});
				map.weatherManager.eventHandler.AddEvent(newEvent);
			}
		}

		// Token: 0x04000F0C RID: 3852
		public float averageInterval = 100f;

		// Token: 0x04000F0D RID: 3853
		public Type eventClass;
	}
}

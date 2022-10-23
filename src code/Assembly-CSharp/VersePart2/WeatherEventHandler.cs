using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200026B RID: 619
	public class WeatherEventHandler
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060011A7 RID: 4519 RVA: 0x00067189 File Offset: 0x00065389
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00067191 File Offset: 0x00065391
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x000671A8 File Offset: 0x000653A8
		public void WeatherEventHandlerTick()
		{
			for (int i = this.liveEvents.Count - 1; i >= 0; i--)
			{
				this.liveEvents[i].WeatherEventTick();
				if (this.liveEvents[i].Expired)
				{
					this.liveEvents.RemoveAt(i);
				}
			}
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00067200 File Offset: 0x00065400
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}

		// Token: 0x04000F0B RID: 3851
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();
	}
}

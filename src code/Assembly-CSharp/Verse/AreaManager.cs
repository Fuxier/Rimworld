using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020001B0 RID: 432
	public class AreaManager : IExposable
	{
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00043EBA File Offset: 0x000420BA
		public List<Area> AllAreas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x00043EC2 File Offset: 0x000420C2
		public Area_Home Home
		{
			get
			{
				return this.Get<Area_Home>();
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x00043ECA File Offset: 0x000420CA
		public Area_BuildRoof BuildRoof
		{
			get
			{
				return this.Get<Area_BuildRoof>();
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00043ED2 File Offset: 0x000420D2
		public Area_NoRoof NoRoof
		{
			get
			{
				return this.Get<Area_NoRoof>();
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x00043EDA File Offset: 0x000420DA
		public Area_SnowClear SnowClear
		{
			get
			{
				return this.Get<Area_SnowClear>();
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00043EE2 File Offset: 0x000420E2
		public Area_PollutionClear PollutionClear
		{
			get
			{
				return this.Get<Area_PollutionClear>();
			}
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00043EEA File Offset: 0x000420EA
		public AreaManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00043F04 File Offset: 0x00042104
		public void AddStartingAreas()
		{
			this.areas.Add(new Area_Home(this));
			this.areas.Add(new Area_BuildRoof(this));
			this.areas.Add(new Area_NoRoof(this));
			this.areas.Add(new Area_SnowClear(this));
			if (ModsConfig.BiotechActive)
			{
				this.areas.Add(new Area_PollutionClear(this));
			}
			Area_Allowed area_Allowed;
			this.TryMakeNewAllowed(out area_Allowed);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00043F78 File Offset: 0x00042178
		public void ExposeData()
		{
			Scribe_Collections.Look<Area>(ref this.areas, "areas", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateAllAreasLinks();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && ModsConfig.BiotechActive && this.PollutionClear == null)
			{
				Area_PollutionClear area_PollutionClear = new Area_PollutionClear(this);
				area_PollutionClear.areaManager = this;
				this.areas.Add(area_PollutionClear);
			}
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00043FDC File Offset: 0x000421DC
		public void AreaManagerUpdate()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].AreaUpdate();
			}
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00044010 File Offset: 0x00042210
		internal void Remove(Area area)
		{
			if (!area.Mutable)
			{
				Log.Error("Tried to delete non-Deletable area " + area);
				return;
			}
			this.areas.Remove(area);
			this.NotifyEveryoneAreaRemoved(area);
			if (Designator_AreaAllowed.SelectedArea == area)
			{
				Designator_AreaAllowed.ClearSelectedArea();
			}
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0004404C File Offset: 0x0004224C
		public Area GetLabeled(string s)
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				if (this.areas[i].Label == s)
				{
					return this.areas[i];
				}
			}
			return null;
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00044098 File Offset: 0x00042298
		public T Get<T>() where T : Area
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				T t = this.areas[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x000440E5 File Offset: 0x000422E5
		private void SortAreas()
		{
			this.areas.InsertionSort((Area a, Area b) => b.ListPriority.CompareTo(a.ListPriority));
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00044114 File Offset: 0x00042314
		private void UpdateAllAreasLinks()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].areaManager = this;
			}
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0004414C File Offset: 0x0004234C
		private void NotifyEveryoneAreaRemoved(Area area)
		{
			foreach (Pawn pawn in PawnsFinder.All_AliveOrDead)
			{
				if (pawn.playerSettings != null)
				{
					pawn.playerSettings.Notify_AreaRemoved(area);
				}
			}
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000441AC File Offset: 0x000423AC
		public void Notify_MapRemoved()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.NotifyEveryoneAreaRemoved(this.areas[i]);
			}
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x000441E1 File Offset: 0x000423E1
		public bool CanMakeNewAllowed()
		{
			return (from a in this.areas
			where a is Area_Allowed
			select a).Count<Area>() < 10;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00044216 File Offset: 0x00042416
		public bool TryMakeNewAllowed(out Area_Allowed area)
		{
			if (!this.CanMakeNewAllowed())
			{
				area = null;
				return false;
			}
			area = new Area_Allowed(this, null);
			this.areas.Add(area);
			this.SortAreas();
			return true;
		}

		// Token: 0x04000B2D RID: 2861
		public Map map;

		// Token: 0x04000B2E RID: 2862
		private List<Area> areas = new List<Area>();

		// Token: 0x04000B2F RID: 2863
		public const int MaxAllowedAreas = 10;
	}
}

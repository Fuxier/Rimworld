using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000529 RID: 1321
	public class LookTargets : IExposable
	{
		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06002850 RID: 10320 RVA: 0x000029B0 File Offset: 0x00000BB0
		public static LookTargets Invalid
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06002851 RID: 10321 RVA: 0x00104D2C File Offset: 0x00102F2C
		public bool IsValid
		{
			get
			{
				return this.PrimaryTarget.IsValid;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002852 RID: 10322 RVA: 0x00104D47 File Offset: 0x00102F47
		public bool Any
		{
			get
			{
				return this.targets.Count != 0;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002853 RID: 10323 RVA: 0x00104D58 File Offset: 0x00102F58
		public GlobalTargetInfo PrimaryTarget
		{
			get
			{
				for (int i = 0; i < this.targets.Count; i++)
				{
					if (this.targets[i].IsValid)
					{
						return this.targets[i];
					}
				}
				if (this.targets.Count != 0)
				{
					return this.targets[0];
				}
				return GlobalTargetInfo.Invalid;
			}
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x00104DBD File Offset: 0x00102FBD
		public void ExposeData()
		{
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.targets, "targets", LookMode.GlobalTargetInfo, Array.Empty<object>());
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x00104DD5 File Offset: 0x00102FD5
		public LookTargets()
		{
			this.targets = new List<GlobalTargetInfo>();
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x00104DE8 File Offset: 0x00102FE8
		public LookTargets(Thing t)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(t);
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x00104E0C File Offset: 0x0010300C
		public LookTargets(WorldObject o)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(o);
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x00104E30 File Offset: 0x00103030
		public LookTargets(IntVec3 c, Map map)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(c, map, false));
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x00104E56 File Offset: 0x00103056
		public LookTargets(int tile)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.targets.Add(new GlobalTargetInfo(tile));
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x00104E7A File Offset: 0x0010307A
		public LookTargets(IEnumerable<GlobalTargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				this.targets.AddRange(targets);
			}
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x00104E9C File Offset: 0x0010309C
		public LookTargets(params GlobalTargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x00104EE0 File Offset: 0x001030E0
		public LookTargets(IEnumerable<TargetInfo> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				IList<TargetInfo> list = targets as IList<TargetInfo>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						this.targets.Add(list[i]);
					}
					return;
				}
				foreach (TargetInfo target in targets)
				{
					this.targets.Add(target);
				}
			}
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x00104F7C File Offset: 0x0010317C
		public LookTargets(params TargetInfo[] targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			if (targets != null)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					this.targets.Add(targets[i]);
				}
			}
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x00104FC2 File Offset: 0x001031C2
		public LookTargets(IEnumerable<Thing> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Thing>(targets);
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x00104FDC File Offset: 0x001031DC
		public LookTargets(IEnumerable<ThingWithComps> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<ThingWithComps>(targets);
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x00104FF6 File Offset: 0x001031F6
		public LookTargets(IEnumerable<Pawn> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Pawn>(targets);
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x00105010 File Offset: 0x00103210
		public LookTargets(IEnumerable<Building> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Building>(targets);
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x0010502A File Offset: 0x0010322A
		public LookTargets(IEnumerable<Plant> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendThingTargets<Plant>(targets);
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x00105044 File Offset: 0x00103244
		public LookTargets(IEnumerable<WorldObject> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<WorldObject>(targets);
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x0010505E File Offset: 0x0010325E
		public LookTargets(IEnumerable<Caravan> targets)
		{
			this.targets = new List<GlobalTargetInfo>();
			this.AppendWorldObjectTargets<Caravan>(targets);
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x00105078 File Offset: 0x00103278
		public static implicit operator LookTargets(Thing t)
		{
			return new LookTargets(t);
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x00105080 File Offset: 0x00103280
		public static implicit operator LookTargets(WorldObject o)
		{
			return new LookTargets(o);
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x00105088 File Offset: 0x00103288
		public static implicit operator LookTargets(TargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x001050AB File Offset: 0x001032AB
		public static implicit operator LookTargets(List<TargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x001050B3 File Offset: 0x001032B3
		public static implicit operator LookTargets(GlobalTargetInfo target)
		{
			return new LookTargets
			{
				targets = new List<GlobalTargetInfo>(),
				targets = 
				{
					target
				}
			};
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x001050D1 File Offset: 0x001032D1
		public static implicit operator LookTargets(List<GlobalTargetInfo> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x001050D9 File Offset: 0x001032D9
		public static implicit operator LookTargets(List<Thing> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x001050E1 File Offset: 0x001032E1
		public static implicit operator LookTargets(List<ThingWithComps> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x001050E9 File Offset: 0x001032E9
		public static implicit operator LookTargets(List<Pawn> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x001050F1 File Offset: 0x001032F1
		public static implicit operator LookTargets(List<Building> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x001050F9 File Offset: 0x001032F9
		public static implicit operator LookTargets(List<Plant> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x00105101 File Offset: 0x00103301
		public static implicit operator LookTargets(List<WorldObject> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x00105109 File Offset: 0x00103309
		public static implicit operator LookTargets(List<Caravan> targets)
		{
			return new LookTargets(targets);
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x00105114 File Offset: 0x00103314
		public static bool SameTargets(LookTargets a, LookTargets b)
		{
			if (a == null)
			{
				return b == null || !b.Any;
			}
			if (b == null)
			{
				return a == null || !a.Any;
			}
			if (a.targets.Count != b.targets.Count)
			{
				return false;
			}
			for (int i = 0; i < a.targets.Count; i++)
			{
				if (a.targets[i] != b.targets[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x00105198 File Offset: 0x00103398
		public void Highlight(bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			for (int i = 0; i < this.targets.Count; i++)
			{
				TargetHighlighter.Highlight(this.targets[i], arrow, colonistBar, circleOverlay);
			}
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x001051D0 File Offset: 0x001033D0
		private void AppendThingTargets<T>(IEnumerable<T> things) where T : Thing
		{
			if (things == null)
			{
				return;
			}
			IList<T> list = things as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.targets.Add(list[i]);
				}
				return;
			}
			foreach (T t in things)
			{
				this.targets.Add(t);
			}
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x00105264 File Offset: 0x00103464
		private void AppendWorldObjectTargets<T>(IEnumerable<T> worldObjects) where T : WorldObject
		{
			if (worldObjects == null)
			{
				return;
			}
			IList<T> list = worldObjects as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.targets.Add(list[i]);
				}
				return;
			}
			foreach (T t in worldObjects)
			{
				this.targets.Add(t);
			}
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x001052F8 File Offset: 0x001034F8
		public void Notify_MapRemoved(Map map)
		{
			this.targets.RemoveAll((GlobalTargetInfo t) => t.Map == map);
		}

		// Token: 0x04001A87 RID: 6791
		public List<GlobalTargetInfo> targets;
	}
}

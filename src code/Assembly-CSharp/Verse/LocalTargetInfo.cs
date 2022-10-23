using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000528 RID: 1320
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06002839 RID: 10297 RVA: 0x00104A0E File Offset: 0x00102C0E
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x0600283A RID: 10298 RVA: 0x00104A25 File Offset: 0x00102C25
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x0600283B RID: 10299 RVA: 0x00104A30 File Offset: 0x00102C30
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x0600283C RID: 10300 RVA: 0x00104A38 File Offset: 0x00102C38
		public Pawn Pawn
		{
			get
			{
				return this.Thing as Pawn;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x00104A45 File Offset: 0x00102C45
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600283E RID: 10302 RVA: 0x00104A5C File Offset: 0x00102C5C
		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600283F RID: 10303 RVA: 0x00104A68 File Offset: 0x00102C68
		public string Label
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.LabelShort;
				}
				return "Location".Translate();
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x00104A8D File Offset: 0x00102C8D
		public IntVec3 Cell
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.PositionHeld;
				}
				return this.cellInt;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06002841 RID: 10305 RVA: 0x00104AAC File Offset: 0x00102CAC
		public Vector3 CenterVector3
		{
			get
			{
				if (this.thingInt != null)
				{
					if (this.thingInt.Spawned)
					{
						return this.thingInt.DrawPos;
					}
					if (this.thingInt.SpawnedOrAnyParentSpawned)
					{
						return this.thingInt.PositionHeld.ToVector3Shifted();
					}
					return this.thingInt.Position.ToVector3Shifted();
				}
				else
				{
					if (this.cellInt.IsValid)
					{
						return this.cellInt.ToVector3Shifted();
					}
					return default(Vector3);
				}
			}
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x00104B31 File Offset: 0x00102D31
		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x00104B45 File Offset: 0x00102D45
		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x00104B55 File Offset: 0x00102D55
		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x00104B5D File Offset: 0x00102D5D
		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x00104B65 File Offset: 0x00102D65
		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to IntVec3 but it had Thing " + targ.thingInt, 6324165);
			}
			return targ.Cell;
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x00104B90 File Offset: 0x00102D90
		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				Log.ErrorOnce("Casted LocalTargetInfo to Thing but it had cell " + targ.cellInt, 631672);
			}
			return targ.thingInt;
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x00104BC5 File Offset: 0x00102DC5
		public TargetInfo ToTargetInfo(Map map)
		{
			if (!this.IsValid)
			{
				return TargetInfo.Invalid;
			}
			if (this.Thing != null)
			{
				return new TargetInfo(this.Thing);
			}
			return new TargetInfo(this.Cell, map, false);
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x00104BF6 File Offset: 0x00102DF6
		public GlobalTargetInfo ToGlobalTargetInfo(Map map)
		{
			if (!this.IsValid)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (this.Thing != null)
			{
				return new GlobalTargetInfo(this.Thing);
			}
			return new GlobalTargetInfo(this.Cell, map, false);
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x00104C28 File Offset: 0x00102E28
		public static bool operator ==(LocalTargetInfo a, LocalTargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || a.cellInt == b.cellInt;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x00104C87 File Offset: 0x00102E87
		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x00104C93 File Offset: 0x00102E93
		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x00104CAB File Offset: 0x00102EAB
		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x00104CB9 File Offset: 0x00102EB9
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return this.cellInt.GetHashCode();
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x00104CE0 File Offset: 0x00102EE0
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return this.Thing.GetUniqueLoadID();
			}
			if (this.Cell.IsValid)
			{
				return this.Cell.ToString();
			}
			return "null";
		}

		// Token: 0x04001A85 RID: 6789
		private Thing thingInt;

		// Token: 0x04001A86 RID: 6790
		private IntVec3 cellInt;
	}
}

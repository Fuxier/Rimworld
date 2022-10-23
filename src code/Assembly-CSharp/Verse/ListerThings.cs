using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001F2 RID: 498
	public sealed class ListerThings
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0004E6B8 File Offset: 0x0004C8B8
		public List<Thing> AllThings
		{
			get
			{
				return this.listsByGroup[2];
			}
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0004E6C4 File Offset: 0x0004C8C4
		public ListerThings(ListerThingsUse use)
		{
			this.use = use;
			this.listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
			this.stateHashByGroup = new int[ThingListGroupHelper.AllGroups.Length];
			this.listsByGroup[2] = new List<Thing>();
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x0004E71F File Offset: 0x0004C91F
		public List<Thing> ThingsInGroup(ThingRequestGroup group)
		{
			return this.ThingsMatching(ThingRequest.ForGroup(group));
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0004E730 File Offset: 0x0004C930
		public int StateHashOfGroup(ThingRequestGroup group)
		{
			if (this.use == ListerThingsUse.Region && !group.StoreInRegion())
			{
				Log.ErrorOnce("Tried to get state hash of group " + group + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968738832);
				return -1;
			}
			return Gen.HashCombineInt(85693994, this.stateHashByGroup[(int)group]);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0004E781 File Offset: 0x0004C981
		public List<Thing> ThingsOfDef(ThingDef def)
		{
			return this.ThingsMatching(ThingRequest.ForDef(def));
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0004E790 File Offset: 0x0004C990
		public List<Thing> ThingsMatching(ThingRequest req)
		{
			if (req.singleDef != null)
			{
				List<Thing> result;
				if (!this.listsByDef.TryGetValue(req.singleDef, out result))
				{
					return ListerThings.EmptyList;
				}
				return result;
			}
			else
			{
				if (req.group == ThingRequestGroup.Undefined)
				{
					throw new InvalidOperationException("Invalid ThingRequest " + req);
				}
				if (this.use == ListerThingsUse.Region && !req.group.StoreInRegion())
				{
					Log.ErrorOnce("Tried to get things in group " + req.group + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968735132);
					return ListerThings.EmptyList;
				}
				return this.listsByGroup[(int)req.group] ?? ListerThings.EmptyList;
			}
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0004E836 File Offset: 0x0004CA36
		public bool Contains(Thing t)
		{
			return this.AllThings.Contains(t);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0004E844 File Offset: 0x0004CA44
		public void Add(Thing t)
		{
			if (!ListerThings.EverListable(t.def, this.use))
			{
				return;
			}
			List<Thing> list;
			if (!this.listsByDef.TryGetValue(t.def, out list))
			{
				list = new List<Thing>();
				this.listsByDef.Add(t.def, list);
			}
			list.Add(t);
			foreach (ThingRequestGroup thingRequestGroup in ThingListGroupHelper.AllGroups)
			{
				if ((this.use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion()) && thingRequestGroup.Includes(t.def))
				{
					List<Thing> list2 = this.listsByGroup[(int)thingRequestGroup];
					if (list2 == null)
					{
						list2 = new List<Thing>();
						this.listsByGroup[(int)thingRequestGroup] = list2;
						this.stateHashByGroup[(int)thingRequestGroup] = 0;
					}
					list2.Add(t);
					this.stateHashByGroup[(int)thingRequestGroup]++;
				}
			}
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0004E914 File Offset: 0x0004CB14
		public void Remove(Thing t)
		{
			if (!ListerThings.EverListable(t.def, this.use))
			{
				return;
			}
			this.listsByDef[t.def].Remove(t);
			ThingRequestGroup[] allGroups = ThingListGroupHelper.AllGroups;
			for (int i = 0; i < allGroups.Length; i++)
			{
				ThingRequestGroup thingRequestGroup = allGroups[i];
				if ((this.use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion()) && thingRequestGroup.Includes(t.def))
				{
					this.listsByGroup[i].Remove(t);
					this.stateHashByGroup[(int)thingRequestGroup]++;
				}
			}
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0004E9A4 File Offset: 0x0004CBA4
		public static bool EverListable(ThingDef def, ListerThingsUse use)
		{
			return (def.category != ThingCategory.Mote || (def.drawGUIOverlay && use != ListerThingsUse.Region)) && (def.category != ThingCategory.Projectile || use != ListerThingsUse.Region);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0004E9D0 File Offset: 0x0004CBD0
		public void Clear()
		{
			this.listsByDef.Clear();
			for (int i = 0; i < this.listsByGroup.Length; i++)
			{
				if (this.listsByGroup[i] != null)
				{
					this.listsByGroup[i].Clear();
				}
				this.stateHashByGroup[i] = 0;
			}
		}

		// Token: 0x04000C7D RID: 3197
		private Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		// Token: 0x04000C7E RID: 3198
		private List<Thing>[] listsByGroup;

		// Token: 0x04000C7F RID: 3199
		private int[] stateHashByGroup;

		// Token: 0x04000C80 RID: 3200
		public ListerThingsUse use;

		// Token: 0x04000C81 RID: 3201
		private static readonly List<Thing> EmptyList = new List<Thing>();
	}
}

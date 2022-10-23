using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000441 RID: 1089
	public static class DebugOutputsBossgroups
	{
		// Token: 0x060020F0 RID: 8432 RVA: 0x000C77D8 File Offset: 0x000C59D8
		[DebugOutput("Bossgroups", true)]
		public static void Bossgroups()
		{
			IEnumerable<BossgroupDef> dataSources = from d in DefDatabase<BossgroupDef>.AllDefs
			orderby d.defName
			select d;
			TableDataGetter<BossgroupDef>[] array = new TableDataGetter<BossgroupDef>[7];
			array[0] = new TableDataGetter<BossgroupDef>("defName", (BossgroupDef d) => d.defName);
			array[1] = new TableDataGetter<BossgroupDef>("boss", (BossgroupDef d) => d.boss.kindDef.label);
			array[2] = new TableDataGetter<BossgroupDef>("available after", (BossgroupDef d) => d.boss.appearAfterTicks.ToStringTicksToPeriod(true, false, true, true, false));
			array[3] = new TableDataGetter<BossgroupDef>("available in", delegate(BossgroupDef d)
			{
				if (d.boss.appearAfterTicks <= Find.TickManager.TicksGame)
				{
					return "now";
				}
				return (d.boss.appearAfterTicks - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false);
			});
			array[4] = new TableDataGetter<BossgroupDef>("defeated in combat", (BossgroupDef d) => Find.BossgroupManager.IsDefeated(d.boss));
			array[5] = new TableDataGetter<BossgroupDef>("reserved by bossgroup", (BossgroupDef d) => Find.BossgroupManager.ReservedByBossgroup(d.boss.kindDef));
			array[6] = new TableDataGetter<BossgroupDef>("reward", delegate(BossgroupDef d)
			{
				ThingDef rewardDef = d.rewardDef;
				if (rewardDef == null)
				{
					return null;
				}
				return rewardDef.label;
			});
			DebugTables.MakeTablesDialog<BossgroupDef>(dataSources, array);
		}
	}
}

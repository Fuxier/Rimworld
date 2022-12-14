using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000449 RID: 1097
	public static class DebugOutputsJoy
	{
		// Token: 0x0600218E RID: 8590 RVA: 0x000D0854 File Offset: 0x000CEA54
		[DebugOutput]
		public static void JoyGivers()
		{
			IEnumerable<JoyGiverDef> allDefs = DefDatabase<JoyGiverDef>.AllDefs;
			TableDataGetter<JoyGiverDef>[] array = new TableDataGetter<JoyGiverDef>[11];
			array[0] = new TableDataGetter<JoyGiverDef>("defName", (JoyGiverDef d) => d.defName);
			array[1] = new TableDataGetter<JoyGiverDef>("joyKind", delegate(JoyGiverDef d)
			{
				if (d.joyKind != null)
				{
					return d.joyKind.defName;
				}
				return "null";
			});
			array[2] = new TableDataGetter<JoyGiverDef>("baseChance", (JoyGiverDef d) => d.baseChance.ToString());
			array[3] = new TableDataGetter<JoyGiverDef>("canDoWhileInBed", (JoyGiverDef d) => d.canDoWhileInBed.ToStringCheckBlank());
			array[4] = new TableDataGetter<JoyGiverDef>("desireSit", (JoyGiverDef d) => d.desireSit.ToStringCheckBlank());
			array[5] = new TableDataGetter<JoyGiverDef>("unroofedOnly", (JoyGiverDef d) => d.unroofedOnly.ToStringCheckBlank());
			array[6] = new TableDataGetter<JoyGiverDef>("jobDef", delegate(JoyGiverDef d)
			{
				if (d.jobDef != null)
				{
					return d.jobDef.defName;
				}
				return "null";
			});
			array[7] = new TableDataGetter<JoyGiverDef>("pctPawnsEverDo", (JoyGiverDef d) => d.pctPawnsEverDo.ToStringPercent());
			array[8] = new TableDataGetter<JoyGiverDef>("requiredCapacities", delegate(JoyGiverDef d)
			{
				if (d.requiredCapacities != null)
				{
					return (from c in d.requiredCapacities
					select c.defName).ToCommaList(false, false);
				}
				return "";
			});
			array[9] = new TableDataGetter<JoyGiverDef>("thingDefs", delegate(JoyGiverDef d)
			{
				if (d.thingDefs != null)
				{
					return (from c in d.thingDefs
					select c.defName).ToCommaList(false, false);
				}
				return "";
			});
			array[10] = new TableDataGetter<JoyGiverDef>("JoyGainFactors", delegate(JoyGiverDef d)
			{
				if (d.thingDefs != null)
				{
					return (from c in d.thingDefs
					select c.GetStatValueAbstract(StatDefOf.JoyGainFactor, null).ToString("F2")).ToCommaList(false, false);
				}
				return "";
			});
			DebugTables.MakeTablesDialog<JoyGiverDef>(allDefs, array);
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000D0A58 File Offset: 0x000CEC58
		[DebugOutput]
		public static void JoyKinds()
		{
			IEnumerable<JoyKindDef> allDefs = DefDatabase<JoyKindDef>.AllDefs;
			TableDataGetter<JoyKindDef>[] array = new TableDataGetter<JoyKindDef>[2];
			array[0] = new TableDataGetter<JoyKindDef>("defName", (JoyKindDef d) => d.defName);
			array[1] = new TableDataGetter<JoyKindDef>("titleRequiredAny", delegate(JoyKindDef d)
			{
				if (d.titleRequiredAny != null)
				{
					return string.Join(",", (from t in d.titleRequiredAny
					select t.defName).ToArray<string>());
				}
				return "NULL";
			});
			DebugTables.MakeTablesDialog<JoyKindDef>(allDefs, array);
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000D0AD0 File Offset: 0x000CECD0
		[DebugOutput]
		public static void JoyJobs()
		{
			IEnumerable<JobDef> dataSources = from j in DefDatabase<JobDef>.AllDefs
			where j.joyKind != null
			select j;
			TableDataGetter<JobDef>[] array = new TableDataGetter<JobDef>[7];
			array[0] = new TableDataGetter<JobDef>("defName", (JobDef d) => d.defName);
			array[1] = new TableDataGetter<JobDef>("joyKind", (JobDef d) => d.joyKind.defName);
			array[2] = new TableDataGetter<JobDef>("joyDuration", (JobDef d) => d.joyDuration.ToString());
			array[3] = new TableDataGetter<JobDef>("joyGainRate", (JobDef d) => d.joyGainRate.ToString());
			array[4] = new TableDataGetter<JobDef>("joyMaxParticipants", (JobDef d) => d.joyMaxParticipants.ToString());
			array[5] = new TableDataGetter<JobDef>("joySkill", delegate(JobDef d)
			{
				if (d.joySkill == null)
				{
					return "";
				}
				return d.joySkill.defName;
			});
			array[6] = new TableDataGetter<JobDef>("joyXpPerTick", (JobDef d) => d.joyXpPerTick.ToString());
			DebugTables.MakeTablesDialog<JobDef>(dataSources, array);
		}
	}
}

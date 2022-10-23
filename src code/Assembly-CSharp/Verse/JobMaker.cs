using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000084 RID: 132
	public static class JobMaker
	{
		// Token: 0x060004C8 RID: 1224 RVA: 0x0001ABD7 File Offset: 0x00018DD7
		public static Job MakeJob()
		{
			Job job = SimplePool<Job>.Get();
			job.loadID = Find.UniqueIDsManager.GetNextJobID();
			return job;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001ABEE File Offset: 0x00018DEE
		public static Job MakeJob(JobDef def)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			return job;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001ABFC File Offset: 0x00018DFC
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			return job;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001AC11 File Offset: 0x00018E11
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.targetB = targetB;
			return job;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001AC2D File Offset: 0x00018E2D
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.targetB = targetB;
			job.targetC = targetC;
			return job;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001AC50 File Offset: 0x00018E50
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.expiryInterval = expiryInterval;
			job.checkOverrideOnExpire = checkOverrideOnExpiry;
			return job;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001AC73 File Offset: 0x00018E73
		public static Job MakeJob(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.expiryInterval = expiryInterval;
			job.checkOverrideOnExpire = checkOverrideOnExpiry;
			return job;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001AC8F File Offset: 0x00018E8F
		public static void ReturnToPool(Job job)
		{
			if (job == null)
			{
				return;
			}
			if (SimplePool<Job>.FreeItemsCount >= 1000)
			{
				return;
			}
			job.Clear();
			SimplePool<Job>.Return(job);
		}

		// Token: 0x04000229 RID: 553
		private const int MaxJobPoolSize = 1000;
	}
}

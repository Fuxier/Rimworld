using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005AE RID: 1454
	public class Verb_LaunchProjectileStatic : Verb_LaunchProjectile
	{
		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002C8D RID: 11405 RVA: 0x00002662 File Offset: 0x00000862
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002C8E RID: 11406 RVA: 0x0011AF82 File Offset: 0x00119182
		public override Texture2D UIIcon
		{
			get
			{
				return TexCommand.Attack;
			}
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x0011AF89 File Offset: 0x00119189
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, true) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x0011AFB0 File Offset: 0x001191B0
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStatic, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}

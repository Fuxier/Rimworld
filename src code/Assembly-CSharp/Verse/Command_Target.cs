using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200049A RID: 1178
	public class Command_Target : Command
	{
		// Token: 0x06002390 RID: 9104 RVA: 0x000E3CA3 File Offset: 0x000E1EA3
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target);
			}, null, null, null);
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x0000249D File Offset: 0x0000069D
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}

		// Token: 0x040016DE RID: 5854
		public Action<LocalTargetInfo> action;

		// Token: 0x040016DF RID: 5855
		public TargetingParameters targetingParams;
	}
}

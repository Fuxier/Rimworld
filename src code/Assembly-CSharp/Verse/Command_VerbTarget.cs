using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200049C RID: 1180
	public class Command_VerbTarget : Command
	{
		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06002399 RID: 9113 RVA: 0x000E3DEA File Offset: 0x000E1FEA
		public override Color IconDrawColor
		{
			get
			{
				if (this.verb.EquipmentSource != null)
				{
					return this.verb.EquipmentSource.DrawColor;
				}
				return base.IconDrawColor;
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x000E3E10 File Offset: 0x000E2010
		public override void GizmoUpdateOnMouseover()
		{
			if (!this.drawRadius)
			{
				return;
			}
			this.verb.verbProps.DrawRadiusRing(this.verb.caster.Position);
			if (!this.groupedVerbs.NullOrEmpty<Verb>())
			{
				foreach (Verb verb in this.groupedVerbs)
				{
					verb.verbProps.DrawRadiusRing(verb.caster.Position);
				}
			}
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x000E3EA8 File Offset: 0x000E20A8
		public override void MergeWith(Gizmo other)
		{
			base.MergeWith(other);
			Command_VerbTarget command_VerbTarget = other as Command_VerbTarget;
			if (command_VerbTarget == null)
			{
				Log.ErrorOnce("Tried to merge Command_VerbTarget with unexpected type", 73406263);
				return;
			}
			if (this.groupedVerbs == null)
			{
				this.groupedVerbs = new List<Verb>();
			}
			this.groupedVerbs.Add(command_VerbTarget.verb);
			if (command_VerbTarget.groupedVerbs != null)
			{
				this.groupedVerbs.AddRange(command_VerbTarget.groupedVerbs);
			}
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x000E3F14 File Offset: 0x000E2114
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Targeter targeter = Find.Targeter;
			if (this.verb.CasterIsPawn && targeter.targetingSource != null && targeter.targetingSource.GetVerb.verbProps == this.verb.verbProps)
			{
				Pawn casterPawn = this.verb.CasterPawn;
				if (!targeter.IsPawnTargeting(casterPawn))
				{
					targeter.targetingSourceAdditionalPawns.Add(casterPawn);
					return;
				}
			}
			else
			{
				Find.Targeter.BeginTargeting(this.verb, null, false, null, null);
			}
		}

		// Token: 0x040016E5 RID: 5861
		public Verb verb;

		// Token: 0x040016E6 RID: 5862
		private List<Verb> groupedVerbs;

		// Token: 0x040016E7 RID: 5863
		public bool drawRadius = true;
	}
}

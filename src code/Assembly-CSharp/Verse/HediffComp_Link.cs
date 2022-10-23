using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000305 RID: 773
	public class HediffComp_Link : HediffComp
	{
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x0007FBF6 File Offset: 0x0007DDF6
		public HediffCompProperties_Link Props
		{
			get
			{
				return (HediffCompProperties_Link)this.props;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600153B RID: 5435 RVA: 0x0007FC03 File Offset: 0x0007DE03
		public Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.other;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x0007FC10 File Offset: 0x0007DE10
		public override bool CompShouldRemove
		{
			get
			{
				if (base.CompShouldRemove)
				{
					return true;
				}
				if (this.other == null || !this.parent.pawn.Spawned || !this.other.Spawned)
				{
					return true;
				}
				if (this.Props.maxDistance > 0f && !this.parent.pawn.Position.InHorDistOf(this.other.Position, this.Props.maxDistance))
				{
					return true;
				}
				if (this.Props.requireLinkOnOtherPawn)
				{
					Pawn pawn;
					if ((pawn = (this.other as Pawn)) != null)
					{
						foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
						{
							HediffWithComps hediffWithComps = hediff as HediffWithComps;
							if (hediffWithComps != null && hediffWithComps.comps.FirstOrDefault(delegate(HediffComp c)
							{
								HediffComp_Link hediffComp_Link = c as HediffComp_Link;
								return hediffComp_Link != null && hediffComp_Link.other == this.parent.pawn && hediffComp_Link.parent.def == this.parent.def;
							}) != null)
							{
								return false;
							}
						}
						return true;
					}
					Log.Error("HediffComp_Link requires link on other pawn, but other thing is not a pawn!");
					return true;
				}
				return false;
			}
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0007FD34 File Offset: 0x0007DF34
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.drawConnection)
			{
				ThingDef moteDef = this.Props.customMote ?? ThingDefOf.Mote_PsychicLinkLine;
				if (this.mote == null)
				{
					this.mote = MoteMaker.MakeInteractionOverlay(moteDef, this.parent.pawn, this.other);
				}
				this.mote.Maintain();
			}
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0007FD9F File Offset: 0x0007DF9F
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Thing>(ref this.other, "other", false);
			Scribe_Values.Look<bool>(ref this.drawConnection, "drawConnection", false, false);
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600153F RID: 5439 RVA: 0x0007FDCA File Offset: 0x0007DFCA
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showName || this.other == null)
				{
					return null;
				}
				return this.other.LabelShort;
			}
		}

		// Token: 0x0400111A RID: 4378
		public Thing other;

		// Token: 0x0400111B RID: 4379
		private MoteDualAttached mote;

		// Token: 0x0400111C RID: 4380
		public bool drawConnection;
	}
}

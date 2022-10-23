using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003C8 RID: 968
	public abstract class PawnTrigger : Thing
	{
		// Token: 0x06001BAD RID: 7085 RVA: 0x000AA1CE File Offset: 0x000A83CE
		protected bool TriggeredBy(Thing thing)
		{
			return thing.def.category == ThingCategory.Pawn && thing.def.race.intelligence == Intelligence.Humanlike && thing.Faction == Faction.OfPlayer;
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x000AA200 File Offset: 0x000A8400
		public void ActivatedBy(Pawn p)
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag, p.Named("SUBJECT")));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x000AA231 File Offset: 0x000A8431
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04001400 RID: 5120
		public string signalTag;
	}
}

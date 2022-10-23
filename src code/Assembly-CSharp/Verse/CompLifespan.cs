using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200041A RID: 1050
	public class CompLifespan : ThingComp
	{
		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x000B7910 File Offset: 0x000B5B10
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000B791D File Offset: 0x000B5B1D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000B7937 File Offset: 0x000B5B37
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.Expire();
			}
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000B7960 File Offset: 0x000B5B60
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.Expire();
			}
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x000B7990 File Offset: 0x000B5B90
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			string result = "";
			int num = this.Props.lifespanTicks - this.age;
			if (num > 0)
			{
				result = "LifespanExpiry".Translate() + " " + num.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor);
				if (!text.NullOrEmpty())
				{
					result = "\n" + text;
				}
			}
			return result;
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x000B7A0C File Offset: 0x000B5C0C
		protected void Expire()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.Props.expireEffect != null)
			{
				this.Props.expireEffect.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			}
			if (this.Props.plantDefToSpawn != null && this.Props.plantDefToSpawn.CanNowPlantAt(this.parent.Position, this.parent.Map, false))
			{
				GenSpawn.Spawn(this.Props.plantDefToSpawn, this.parent.Position, this.parent.Map, WipeMode.Vanish);
			}
			this.parent.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x040014F2 RID: 5362
		public int age = -1;
	}
}

using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002E8 RID: 744
	public class HediffComp_GiveAbility : HediffComp
	{
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0007E6B5 File Offset: 0x0007C8B5
		private HediffCompProperties_GiveAbility Props
		{
			get
			{
				return (HediffCompProperties_GiveAbility)this.props;
			}
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0007E6C4 File Offset: 0x0007C8C4
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.Props.abilityDef != null)
			{
				this.parent.pawn.abilities.GainAbility(this.Props.abilityDef);
			}
			if (!this.Props.abilityDefs.NullOrEmpty<AbilityDef>())
			{
				for (int i = 0; i < this.Props.abilityDefs.Count; i++)
				{
					this.parent.pawn.abilities.GainAbility(this.Props.abilityDefs[i]);
				}
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0007E754 File Offset: 0x0007C954
		public override void CompPostPostRemoved()
		{
			if (this.Props.abilityDef != null)
			{
				this.parent.pawn.abilities.RemoveAbility(this.Props.abilityDef);
			}
			if (!this.Props.abilityDefs.NullOrEmpty<AbilityDef>())
			{
				for (int i = 0; i < this.Props.abilityDefs.Count; i++)
				{
					this.parent.pawn.abilities.RemoveAbility(this.Props.abilityDefs[i]);
				}
			}
		}
	}
}

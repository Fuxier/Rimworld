using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000104 RID: 260
	public class JobDef : Def
	{
		// Token: 0x0600070D RID: 1805 RVA: 0x00025554 File Offset: 0x00023754
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.joySkill != null && this.joyXpPerTick == 0f)
			{
				yield return "funSkill is not null but funXpPerTick is zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000643 RID: 1603
		public Type driverClass;

		// Token: 0x04000644 RID: 1604
		[MustTranslate]
		public string reportString = "Doing something.";

		// Token: 0x04000645 RID: 1605
		public bool playerInterruptible = true;

		// Token: 0x04000646 RID: 1606
		public bool forceCompleteBeforeNextJob;

		// Token: 0x04000647 RID: 1607
		public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;

		// Token: 0x04000648 RID: 1608
		public bool alwaysShowWeapon;

		// Token: 0x04000649 RID: 1609
		public bool neverShowWeapon;

		// Token: 0x0400064A RID: 1610
		public bool suspendable = true;

		// Token: 0x0400064B RID: 1611
		public bool casualInterruptible = true;

		// Token: 0x0400064C RID: 1612
		public bool allowOpportunisticPrefix;

		// Token: 0x0400064D RID: 1613
		public bool collideWithPawns;

		// Token: 0x0400064E RID: 1614
		public bool isIdle;

		// Token: 0x0400064F RID: 1615
		public TaleDef taleOnCompletion;

		// Token: 0x04000650 RID: 1616
		public bool neverFleeFromEnemies;

		// Token: 0x04000651 RID: 1617
		public bool sleepCanInterrupt = true;

		// Token: 0x04000652 RID: 1618
		public bool makeTargetPrisoner;

		// Token: 0x04000653 RID: 1619
		public int waitAfterArriving;

		// Token: 0x04000654 RID: 1620
		public bool carryThingAfterJob;

		// Token: 0x04000655 RID: 1621
		public bool dropThingBeforeJob = true;

		// Token: 0x04000656 RID: 1622
		public int joyDuration = 4000;

		// Token: 0x04000657 RID: 1623
		public int joyMaxParticipants = 1;

		// Token: 0x04000658 RID: 1624
		public float joyGainRate = 1f;

		// Token: 0x04000659 RID: 1625
		public SkillDef joySkill;

		// Token: 0x0400065A RID: 1626
		public float joyXpPerTick;

		// Token: 0x0400065B RID: 1627
		public JoyKindDef joyKind;

		// Token: 0x0400065C RID: 1628
		public Rot4 faceDir = Rot4.Invalid;

		// Token: 0x0400065D RID: 1629
		public int learningDuration = 20000;

		// Token: 0x0400065E RID: 1630
		public ReservationLayerDef containerReservationLayer;
	}
}

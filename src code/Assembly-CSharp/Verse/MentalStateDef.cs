using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000112 RID: 274
	public class MentalStateDef : Def
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00025CA2 File Offset: 0x00023EA2
		public MentalStateWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalStateWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x00025CE2 File Offset: 0x00023EE2
		public bool IsAggro
		{
			get
			{
				return this.category == MentalStateCategory.Aggro;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00025CF0 File Offset: 0x00023EF0
		public bool IsExtreme
		{
			get
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].intensity == MentalBreakIntensity.Extreme && allDefsListForReading[i].mentalState == this)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00025D35 File Offset: 0x00023F35
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.beginLetterDef == null)
			{
				this.beginLetterDef = LetterDefOf.NegativeEvent;
			}
		}

		// Token: 0x0400069F RID: 1695
		public Type stateClass = typeof(MentalState);

		// Token: 0x040006A0 RID: 1696
		public Type workerClass = typeof(MentalStateWorker);

		// Token: 0x040006A1 RID: 1697
		public MentalStateCategory category;

		// Token: 0x040006A2 RID: 1698
		public bool prisonersCanDo = true;

		// Token: 0x040006A3 RID: 1699
		public bool slavesCanDo = true;

		// Token: 0x040006A4 RID: 1700
		public bool inCaravanCanDo;

		// Token: 0x040006A5 RID: 1701
		public bool colonistsOnly;

		// Token: 0x040006A6 RID: 1702
		public bool slavesOnly;

		// Token: 0x040006A7 RID: 1703
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x040006A8 RID: 1704
		public bool downedCanDo;

		// Token: 0x040006A9 RID: 1705
		public bool unspawnedNotInCaravanCanDo;

		// Token: 0x040006AA RID: 1706
		public bool blockNormalThoughts;

		// Token: 0x040006AB RID: 1707
		public bool stopsJobs = true;

		// Token: 0x040006AC RID: 1708
		public List<InteractionDef> blockInteractionInitiationExcept;

		// Token: 0x040006AD RID: 1709
		public List<InteractionDef> blockInteractionRecipientExcept;

		// Token: 0x040006AE RID: 1710
		public bool blockRandomInteraction;

		// Token: 0x040006AF RID: 1711
		public EffecterDef stateEffecter;

		// Token: 0x040006B0 RID: 1712
		public TaleDef tale;

		// Token: 0x040006B1 RID: 1713
		public bool allowBeatfire;

		// Token: 0x040006B2 RID: 1714
		public DrugCategory drugCategory = DrugCategory.Any;

		// Token: 0x040006B3 RID: 1715
		public bool ignoreDrugPolicy;

		// Token: 0x040006B4 RID: 1716
		public float recoveryMtbDays = 1f;

		// Token: 0x040006B5 RID: 1717
		public int minTicksBeforeRecovery = 500;

		// Token: 0x040006B6 RID: 1718
		public int maxTicksBeforeRecovery = 99999999;

		// Token: 0x040006B7 RID: 1719
		public bool recoverFromSleep;

		// Token: 0x040006B8 RID: 1720
		public bool recoverFromDowned = true;

		// Token: 0x040006B9 RID: 1721
		public bool recoverFromCollapsingExhausted = true;

		// Token: 0x040006BA RID: 1722
		public ThoughtDef moodRecoveryThought;

		// Token: 0x040006BB RID: 1723
		public bool allowGuilty = true;

		// Token: 0x040006BC RID: 1724
		[MustTranslate]
		public string beginLetter;

		// Token: 0x040006BD RID: 1725
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x040006BE RID: 1726
		public LetterDef beginLetterDef;

		// Token: 0x040006BF RID: 1727
		public Color nameColor = Color.green;

		// Token: 0x040006C0 RID: 1728
		[MustTranslate]
		public string recoveryMessage;

		// Token: 0x040006C1 RID: 1729
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x040006C2 RID: 1730
		public bool escapingPrisonersIgnore;

		// Token: 0x040006C3 RID: 1731
		public bool blocksDefendAndExpandHive;

		// Token: 0x040006C4 RID: 1732
		private MentalStateWorker workerInt;
	}
}

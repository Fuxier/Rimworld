using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B3 RID: 179
	public class CostListForDifficulty
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0001FDA2 File Offset: 0x0001DFA2
		public bool Applies
		{
			get
			{
				if (Find.Storyteller == null)
				{
					return false;
				}
				if (this.cachedDifficulty != Find.Storyteller.difficulty)
				{
					this.RecacheApplies();
				}
				return this.cachedApplies;
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001FDCC File Offset: 0x0001DFCC
		public void RecacheApplies()
		{
			this.cachedDifficulty = Find.Storyteller.difficulty;
			if (this.difficultyVar.NullOrEmpty())
			{
				this.cachedApplies = false;
				return;
			}
			FieldInfo field = typeof(Difficulty).GetField(this.difficultyVar, BindingFlags.Instance | BindingFlags.Public);
			this.cachedApplies = (bool)field.GetValue(this.cachedDifficulty);
			if (this.invert)
			{
				this.cachedApplies = !this.cachedApplies;
			}
		}

		// Token: 0x040002EC RID: 748
		public string difficultyVar;

		// Token: 0x040002ED RID: 749
		public List<ThingDefCountClass> costList;

		// Token: 0x040002EE RID: 750
		public int costStuffCount;

		// Token: 0x040002EF RID: 751
		public bool invert;

		// Token: 0x040002F0 RID: 752
		private bool cachedApplies;

		// Token: 0x040002F1 RID: 753
		private Difficulty cachedDifficulty;
	}
}

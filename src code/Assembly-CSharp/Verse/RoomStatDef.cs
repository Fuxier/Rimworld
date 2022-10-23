using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000130 RID: 304
	public class RoomStatDef : Def
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x000282AE File Offset: 0x000264AE
		public RoomStatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomStatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000282E0 File Offset: 0x000264E0
		public RoomStatScoreStage GetScoreStage(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				return null;
			}
			return this.scoreStages[this.GetScoreStageIndex(score)];
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00028304 File Offset: 0x00026504
		public int GetScoreStageIndex(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				throw new InvalidOperationException("No score stages available.");
			}
			int result = 0;
			int num = 0;
			while (num < this.scoreStages.Count && score >= this.scoreStages[num].minScore)
			{
				result = num;
				num++;
			}
			return result;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00028358 File Offset: 0x00026558
		public string ScoreToString(float score)
		{
			if (this.displayRounded)
			{
				return Mathf.RoundToInt(score).ToString();
			}
			return score.ToString("F2");
		}

		// Token: 0x040007EC RID: 2028
		public Type workerClass;

		// Token: 0x040007ED RID: 2029
		public float updatePriority;

		// Token: 0x040007EE RID: 2030
		public bool displayRounded;

		// Token: 0x040007EF RID: 2031
		public bool isHidden;

		// Token: 0x040007F0 RID: 2032
		public float roomlessScore;

		// Token: 0x040007F1 RID: 2033
		public List<RoomStatScoreStage> scoreStages;

		// Token: 0x040007F2 RID: 2034
		public RoomStatDef inputStat;

		// Token: 0x040007F3 RID: 2035
		public SimpleCurve curve;

		// Token: 0x040007F4 RID: 2036
		[Unsaved(false)]
		private RoomStatWorker workerInt;
	}
}

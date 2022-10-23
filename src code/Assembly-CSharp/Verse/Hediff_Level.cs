using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000348 RID: 840
	public class Hediff_Level : HediffWithComps
	{
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x000843BC File Offset: 0x000825BC
		public override string Label
		{
			get
			{
				if (!this.def.levelIsQuantity)
				{
					return this.def.label + " (" + "LevelNum".Translate(this.level).ToString() + ")";
				}
				return this.def.label + " x" + this.level;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x00084434 File Offset: 0x00082634
		public override bool ShouldRemove
		{
			get
			{
				return this.level == 0;
			}
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x0008443F File Offset: 0x0008263F
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part == null)
			{
				Log.Error(this.def.defName + " has null Part. It should be set before PostAdd.");
			}
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x0008446A File Offset: 0x0008266A
		public override void Tick()
		{
			base.Tick();
			this.Severity = (float)this.level;
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x0008447F File Offset: 0x0008267F
		public virtual void ChangeLevel(int levelOffset)
		{
			this.level = (int)Mathf.Clamp((float)(this.level + levelOffset), this.def.minSeverity, this.def.maxSeverity);
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x000844AC File Offset: 0x000826AC
		public virtual void SetLevelTo(int targetLevel)
		{
			if (targetLevel != this.level)
			{
				this.ChangeLevel(targetLevel - this.level);
			}
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x000844C8 File Offset: 0x000826C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.level, "level", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error(base.GetType().Name + " has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}

		// Token: 0x04001195 RID: 4501
		public int level = 1;
	}
}

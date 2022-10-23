using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000336 RID: 822
	public abstract class HediffComp_SeverityModifierBase : HediffComp
	{
		// Token: 0x060015EE RID: 5614
		public abstract float SeverityChangePerDay();

		// Token: 0x060015EF RID: 5615 RVA: 0x000820F4 File Offset: 0x000802F4
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(200))
			{
				float num = this.SeverityChangePerDay();
				num *= 0.0033333334f;
				severityAdjustment += num;
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00082130 File Offset: 0x00080330
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			if (!base.Pawn.Dead)
			{
				stringBuilder.AppendLine("severity/day: " + this.SeverityChangePerDay().ToString("F3"));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0400117C RID: 4476
		protected const int SeverityUpdateInterval = 200;
	}
}

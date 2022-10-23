using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200052E RID: 1326
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06002890 RID: 10384 RVA: 0x00106525 File Offset: 0x00104725
		// (set) Token: 0x06002891 RID: 10385 RVA: 0x0010652D File Offset: 0x0010472D
		public bool DrawBackground { get; set; }

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06002892 RID: 10386 RVA: 0x00106536 File Offset: 0x00104736
		// (set) Token: 0x06002893 RID: 10387 RVA: 0x0010653E File Offset: 0x0010473E
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06002894 RID: 10388 RVA: 0x00106547 File Offset: 0x00104747
		// (set) Token: 0x06002895 RID: 10389 RVA: 0x0010654F File Offset: 0x0010474F
		public bool DrawMeasures { get; set; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06002896 RID: 10390 RVA: 0x00106558 File Offset: 0x00104758
		// (set) Token: 0x06002897 RID: 10391 RVA: 0x00106560 File Offset: 0x00104760
		public bool DrawPoints { get; set; }

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002898 RID: 10392 RVA: 0x00106569 File Offset: 0x00104769
		// (set) Token: 0x06002899 RID: 10393 RVA: 0x00106571 File Offset: 0x00104771
		public bool DrawLegend { get; set; }

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x0010657A File Offset: 0x0010477A
		// (set) Token: 0x0600289B RID: 10395 RVA: 0x00106582 File Offset: 0x00104782
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600289C RID: 10396 RVA: 0x0010658B File Offset: 0x0010478B
		// (set) Token: 0x0600289D RID: 10397 RVA: 0x00106593 File Offset: 0x00104793
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x0010659C File Offset: 0x0010479C
		// (set) Token: 0x0600289F RID: 10399 RVA: 0x001065A4 File Offset: 0x001047A4
		public bool UseFixedSection { get; set; }

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x001065AD File Offset: 0x001047AD
		// (set) Token: 0x060028A1 RID: 10401 RVA: 0x001065B5 File Offset: 0x001047B5
		public bool UseFixedScale { get; set; }

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x001065BE File Offset: 0x001047BE
		// (set) Token: 0x060028A3 RID: 10403 RVA: 0x001065C6 File Offset: 0x001047C6
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060028A4 RID: 10404 RVA: 0x001065CF File Offset: 0x001047CF
		// (set) Token: 0x060028A5 RID: 10405 RVA: 0x001065D7 File Offset: 0x001047D7
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060028A6 RID: 10406 RVA: 0x001065E0 File Offset: 0x001047E0
		// (set) Token: 0x060028A7 RID: 10407 RVA: 0x001065E8 File Offset: 0x001047E8
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x001065F1 File Offset: 0x001047F1
		// (set) Token: 0x060028A9 RID: 10409 RVA: 0x001065F9 File Offset: 0x001047F9
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x060028AA RID: 10410 RVA: 0x00106602 File Offset: 0x00104802
		// (set) Token: 0x060028AB RID: 10411 RVA: 0x0010660A File Offset: 0x0010480A
		public bool XIntegersOnly { get; set; }

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x00106613 File Offset: 0x00104813
		// (set) Token: 0x060028AD RID: 10413 RVA: 0x0010661B File Offset: 0x0010481B
		public bool YIntegersOnly { get; set; }

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x060028AE RID: 10414 RVA: 0x00106624 File Offset: 0x00104824
		// (set) Token: 0x060028AF RID: 10415 RVA: 0x0010662C File Offset: 0x0010482C
		public string LabelX { get; set; }

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x060028B0 RID: 10416 RVA: 0x00106635 File Offset: 0x00104835
		// (set) Token: 0x060028B1 RID: 10417 RVA: 0x0010663D File Offset: 0x0010483D
		public FloatRange FixedSection { get; set; }

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060028B2 RID: 10418 RVA: 0x00106646 File Offset: 0x00104846
		// (set) Token: 0x060028B3 RID: 10419 RVA: 0x0010664E File Offset: 0x0010484E
		public Vector2 FixedScale { get; set; }

		// Token: 0x060028B4 RID: 10420 RVA: 0x00106658 File Offset: 0x00104858
		public SimpleCurveDrawerStyle()
		{
			this.DrawBackground = false;
			this.DrawBackgroundLines = true;
			this.DrawMeasures = false;
			this.DrawPoints = true;
			this.DrawLegend = false;
			this.DrawCurveMousePoint = false;
			this.OnlyPositiveValues = false;
			this.UseFixedSection = false;
			this.UseFixedScale = false;
			this.UseAntiAliasedLines = false;
			this.PointsRemoveOptimization = false;
			this.MeasureLabelsXCount = 5;
			this.MeasureLabelsYCount = 5;
			this.XIntegersOnly = false;
			this.YIntegersOnly = false;
			this.LabelX = "x";
		}
	}
}

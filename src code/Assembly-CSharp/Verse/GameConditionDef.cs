using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000F3 RID: 243
	public class GameConditionDef : Def
	{
		// Token: 0x060006DD RID: 1757 RVA: 0x00024DB1 File Offset: 0x00022FB1
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00024DD2 File Offset: 0x00022FD2
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00024DDB File Offset: 0x00022FDB
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.conditionClass == null)
			{
				yield return "conditionClass is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400059A RID: 1434
		public Type conditionClass = typeof(GameCondition);

		// Token: 0x0400059B RID: 1435
		private List<GameConditionDef> exclusiveConditions;

		// Token: 0x0400059C RID: 1436
		[MustTranslate]
		public string startMessage;

		// Token: 0x0400059D RID: 1437
		[MustTranslate]
		public string endMessage;

		// Token: 0x0400059E RID: 1438
		[MustTranslate]
		public string letterText;

		// Token: 0x0400059F RID: 1439
		public List<ThingDef> letterHyperlinks;

		// Token: 0x040005A0 RID: 1440
		public LetterDef letterDef;

		// Token: 0x040005A1 RID: 1441
		public bool canBePermanent;

		// Token: 0x040005A2 RID: 1442
		[MustTranslate]
		public string descriptionFuture;

		// Token: 0x040005A3 RID: 1443
		[NoTranslate]
		public string jumpToSourceKey = "ClickToJumpToSource";

		// Token: 0x040005A4 RID: 1444
		public List<GameConditionDef> silencedByConditions;

		// Token: 0x040005A5 RID: 1445
		public bool natural = true;

		// Token: 0x040005A6 RID: 1446
		public PsychicDroneLevel defaultDroneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x040005A7 RID: 1447
		public bool preventRain;

		// Token: 0x040005A8 RID: 1448
		public WeatherDef weatherDef;

		// Token: 0x040005A9 RID: 1449
		public float temperatureOffset = -10f;

		// Token: 0x040005AA RID: 1450
		public float minNearbyPollution;

		// Token: 0x040005AB RID: 1451
		public SimpleCurve mtbOverNearbyPollutionCurve;
	}
}

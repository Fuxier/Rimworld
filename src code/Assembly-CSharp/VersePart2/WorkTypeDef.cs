using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000152 RID: 338
	public class WorkTypeDef : Def
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0002B352 File Offset: 0x00029552
		public bool VisibleCurrently
		{
			get
			{
				if (this.cachedFrameVisibleCurrently == -1 || this.cachedFrameVisibleCurrently < Time.frameCount - 30)
				{
					this.cachedVisibleCurrently = this.VisibleNow(null, null);
					this.cachedFrameVisibleCurrently = Time.frameCount;
				}
				return this.cachedVisibleCurrently;
			}
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0002B38C File Offset: 0x0002958C
		public bool VisibleNow(Pawn ignorePawn = null, Pawn alsoCheckPawn = null)
		{
			if (!this.visible)
			{
				return false;
			}
			if (this.visibleOnlyWithChildrenInColony)
			{
				bool flag = false;
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction)
				{
					if (pawn.RaceProps.Humanlike && pawn != ignorePawn && pawn.DevelopmentalStage.Juvenile())
					{
						flag = true;
						break;
					}
				}
				if (alsoCheckPawn != null && alsoCheckPawn.DevelopmentalStage.Juvenile())
				{
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0002B428 File Offset: 0x00029628
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.naturalPriority < 0 || this.naturalPriority > 10000)
			{
				yield return "naturalPriority is " + this.naturalPriority + ", but it must be between 0 and 10000";
			}
			yield break;
			yield break;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0002B438 File Offset: 0x00029638
		public override void ResolveReferences()
		{
			foreach (WorkGiverDef item in from d in DefDatabase<WorkGiverDef>.AllDefs
			where d.workType == this
			orderby d.priorityInType descending
			select d)
			{
				this.workGiversByPriority.Add(item);
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002B4C0 File Offset: 0x000296C0
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(this.defName.GetHashCode(), this.gerundLabel);
		}

		// Token: 0x04000970 RID: 2416
		public WorkTags workTags;

		// Token: 0x04000971 RID: 2417
		[MustTranslate]
		public string labelShort;

		// Token: 0x04000972 RID: 2418
		[MustTranslate]
		public string pawnLabel;

		// Token: 0x04000973 RID: 2419
		[MustTranslate]
		public string gerundLabel;

		// Token: 0x04000974 RID: 2420
		[MustTranslate]
		public string verb;

		// Token: 0x04000975 RID: 2421
		public bool visible = true;

		// Token: 0x04000976 RID: 2422
		public bool visibleOnlyWithChildrenInColony;

		// Token: 0x04000977 RID: 2423
		public int naturalPriority;

		// Token: 0x04000978 RID: 2424
		public bool alwaysStartActive;

		// Token: 0x04000979 RID: 2425
		public bool requireCapableColonist;

		// Token: 0x0400097A RID: 2426
		public List<SkillDef> relevantSkills = new List<SkillDef>();

		// Token: 0x0400097B RID: 2427
		public bool disabledForSlaves;

		// Token: 0x0400097C RID: 2428
		[Unsaved(false)]
		public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();

		// Token: 0x0400097D RID: 2429
		[Unsaved(false)]
		private bool cachedVisibleCurrently;

		// Token: 0x0400097E RID: 2430
		[Unsaved(false)]
		private int cachedFrameVisibleCurrently = -1;
	}
}

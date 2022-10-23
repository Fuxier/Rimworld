using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200035E RID: 862
	public class HediffSet : IExposable
	{
		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060016EE RID: 5870 RVA: 0x000868AE File Offset: 0x00084AAE
		public float PainTotal
		{
			get
			{
				if (this.cachedPain < 0f)
				{
					this.cachedPain = this.CalculatePain();
				}
				return this.cachedPain;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060016EF RID: 5871 RVA: 0x000868CF File Offset: 0x00084ACF
		public float BleedRateTotal
		{
			get
			{
				if (this.cachedBleedRate < 0f)
				{
					this.cachedBleedRate = this.CalculateBleedRate();
				}
				return this.cachedBleedRate;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x000868F0 File Offset: 0x00084AF0
		public bool HasHead
		{
			get
			{
				if (this.cachedHasHead == null)
				{
					this.cachedHasHead = new bool?(this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x.def == BodyPartDefOf.Head));
				}
				return this.cachedHasHead.Value;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x0008694E File Offset: 0x00084B4E
		public float HungerRateFactor
		{
			get
			{
				return this.GetHungerRateFactor(null);
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060016F2 RID: 5874 RVA: 0x00086958 File Offset: 0x00084B58
		public float RestFallFactor
		{
			get
			{
				float num = 1f;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.restFallFactor;
					}
				}
				for (int j = 0; j < this.hediffs.Count; j++)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.restFallFactorOffset;
					}
				}
				return Mathf.Max(num, 0f);
			}
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x000869E4 File Offset: 0x00084BE4
		public HediffSet(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00086A4C File Offset: 0x00084C4C
		public void ExposeData()
		{
			Scribe_Collections.Look<Hediff>(ref this.hediffs, "hediffs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.hediffs.RemoveAll((Hediff x) => x == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had some null hediffs.");
				}
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					this.hediffs[i].pawn = this.pawn;
				}
				this.hediffs.RemoveAll(delegate(Hediff hediff)
				{
					if (((hediff != null) ? hediff.def : null) != null)
					{
						return false;
					}
					Log.Error(hediff.ToStringSafe<Hediff>() + " on " + this.pawn.ToStringSafe<Pawn>() + " had a null def.");
					return true;
				});
				this.DirtyCache();
			}
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00086B18 File Offset: 0x00084D18
		public void AddDirect(Hediff hediff, DamageInfo? dinfo = null, DamageWorker.DamageResult damageResult = null)
		{
			if (hediff.def == null)
			{
				Log.Error("Tried to add health diff with null def. Canceling.");
				return;
			}
			if (hediff.Part != null && !this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff.Part))
			{
				Log.Error("Tried to add health diff to missing part " + hediff.Part);
				return;
			}
			hediff.ageTicks = 0;
			hediff.pawn = this.pawn;
			bool flag = false;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].TryMergeWith(hediff))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				this.hediffs.Add(hediff);
				hediff.PostAdd(dinfo);
				if (this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			bool flag2 = hediff is Hediff_MissingPart;
			if (!(hediff is Hediff_MissingPart) && hediff.Part != null && hediff.Part != this.pawn.RaceProps.body.corePart && this.GetPartHealth(hediff.Part) == 0f && hediff.Part != this.pawn.RaceProps.body.corePart)
			{
				bool flag3 = this.HasDirectlyAddedPartFor(hediff.Part);
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
				hediff_MissingPart.IsFresh = !flag3;
				hediff_MissingPart.lastInjury = hediff.def;
				this.pawn.health.AddHediff(hediff_MissingPart, hediff.Part, dinfo, null);
				if (damageResult != null)
				{
					damageResult.AddHediff(hediff_MissingPart);
				}
				if (flag3)
				{
					if (dinfo != null)
					{
						hediff_MissingPart.lastInjury = HealthUtility.GetHediffDefFromDamage(dinfo.Value.Def, this.pawn, hediff.Part);
					}
					else
					{
						hediff_MissingPart.lastInjury = null;
					}
				}
				flag2 = true;
			}
			this.DirtyCache();
			if (flag2 && this.pawn.apparel != null)
			{
				this.pawn.apparel.Notify_LostBodyPart();
			}
			if (hediff.def.causesNeed != null && !this.pawn.Dead)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00086D6C File Offset: 0x00084F6C
		public void DirtyCache()
		{
			this.CacheMissingPartsCommonAncestors();
			this.pawn.Drawer.renderer.WoundOverlays.ClearCache();
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			this.cachedPain = -1f;
			this.cachedBleedRate = -1f;
			this.cachedHasHead = null;
			this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			this.pawn.health.summaryHealth.Notify_HealthChanged();
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00086DFC File Offset: 0x00084FFC
		public void Notify_PawnDied()
		{
			for (int i = this.hediffs.Count - 1; i >= 0; i--)
			{
				this.hediffs[i].Notify_PawnDied();
			}
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00086E34 File Offset: 0x00085034
		public float GetHungerRateFactor(HediffDef ignore = null)
		{
			float num = 1f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def != ignore)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.hungerRateFactor;
					}
				}
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j].def != ignore)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.hungerRateFactorOffset;
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00086EE8 File Offset: 0x000850E8
		public int GetHediffCount(HediffDef def)
		{
			int num = 0;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00086F28 File Offset: 0x00085128
		public void GetHediffs<T>(ref List<T> resultHediffs, Predicate<T> filter = null) where T : Hediff
		{
			resultHediffs.Clear();
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null && (filter == null || filter(t)))
				{
					resultHediffs.Add(t);
				}
			}
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00086F88 File Offset: 0x00085188
		public T GetFirstHediff<T>() where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T result;
				if ((result = (this.hediffs[i] as T)) != null)
				{
					return result;
				}
			}
			return default(T);
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x00086FD8 File Offset: 0x000851D8
		public Hediff GetFirstHediffOfDef(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return this.hediffs[i];
				}
			}
			return null;
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x00087034 File Offset: 0x00085234
		public T GetFirstHediffMatchingPart<T>(BodyPartRecord part) where T : Hediff
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				T t = this.hediffs[i] as T;
				if (t != null && t.Part == part)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00087090 File Offset: 0x00085290
		public bool PartIsMissing(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_MissingPart)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x000870E0 File Offset: 0x000852E0
		public float GetPartHealth(BodyPartRecord part)
		{
			if (part == null)
			{
				return 0f;
			}
			float num = part.def.GetMaxHealth(this.pawn);
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i] is Hediff_MissingPart && this.hediffs[i].Part == part)
				{
					return 0f;
				}
				if (this.hediffs[i].Part == part)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null)
					{
						num -= hediff_Injury.Severity;
					}
				}
			}
			num = Mathf.Max(num, 0f);
			if (!part.def.destroyableByDamage)
			{
				num = Mathf.Max(num, 1f);
			}
			return (float)Mathf.RoundToInt(num);
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x000871AC File Offset: 0x000853AC
		public BodyPartRecord GetBrain()
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				if (bodyPartRecord.def.tags.Contains(BodyPartTagDefOf.ConsciousnessSource))
				{
					return bodyPartRecord;
				}
			}
			return null;
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00087214 File Offset: 0x00085414
		public bool HasHediff(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00087264 File Offset: 0x00085464
		public bool HasHediff(HediffDef def, BodyPartRecord bodyPart, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && this.hediffs[i].Part == bodyPart && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x000872C8 File Offset: 0x000854C8
		public List<Verb> GetHediffsVerbs()
		{
			this.tmpHediffVerbs.Clear();
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				HediffComp_VerbGiver hediffComp_VerbGiver = this.hediffs[i].TryGetComp<HediffComp_VerbGiver>();
				if (hediffComp_VerbGiver != null)
				{
					List<Verb> allVerbs = hediffComp_VerbGiver.VerbTracker.AllVerbs;
					for (int j = 0; j < allVerbs.Count; j++)
					{
						this.tmpHediffVerbs.Add(allVerbs[j]);
					}
				}
			}
			return this.tmpHediffVerbs;
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00087340 File Offset: 0x00085540
		public IEnumerable<Hediff> GetHediffsTendable()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num + 1)
			{
				if (this.hediffs[i].TendableNow(false))
				{
					yield return this.hediffs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x00087350 File Offset: 0x00085550
		public bool HasTendableHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && this.hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x000873A8 File Offset: 0x000855A8
		public bool HasHediffBlocksSleeping()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.blocksSleeping)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x000873E8 File Offset: 0x000855E8
		public bool HasHediffPreventsPregnancy()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.preventsPregnancy)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00087428 File Offset: 0x00085628
		public bool HasPregnancyHediff()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def.pregnant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x00087466 File Offset: 0x00085666
		public IEnumerable<HediffComp> GetAllComps()
		{
			foreach (Hediff hediff in this.hediffs)
			{
				HediffWithComps hediffWithComps = hediff as HediffWithComps;
				if (hediffWithComps != null)
				{
					foreach (HediffComp hediffComp in hediffWithComps.comps)
					{
						yield return hediffComp;
					}
					List<HediffComp>.Enumerator enumerator2 = default(List<HediffComp>.Enumerator);
				}
			}
			List<Hediff>.Enumerator enumerator = default(List<Hediff>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x00087476 File Offset: 0x00085676
		public IEnumerable<Hediff_Injury> GetInjuriesTendable()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.TendableNow(false))
				{
					yield return hediff_Injury;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00087488 File Offset: 0x00085688
		public bool HasTendableInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x000874CC File Offset: 0x000856CC
		public bool HasNonPermanentInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x00087510 File Offset: 0x00085710
		public bool HasNaturallyHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealNaturally())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x00087554 File Offset: 0x00085754
		public bool HasTendedAndHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealFromTending() && hediff_Injury.Severity > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x000875A4 File Offset: 0x000857A4
		public bool HasTemperatureInjury(TemperatureInjuryStage minStage)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((this.hediffs[i].def == HediffDefOf.Hypothermia || this.hediffs[i].def == HediffDefOf.Heatstroke) && this.hediffs[i].CurStageIndex >= (int)minStage)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00087610 File Offset: 0x00085810
		public List<BodyPartRecord> GetInjuredParts()
		{
			HediffSet.tmpInjuredParts.Clear();
			using (List<Hediff>.Enumerator enumerator = this.hediffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Hediff_Injury hediff_Injury;
					if ((hediff_Injury = (enumerator.Current as Hediff_Injury)) != null && !HediffSet.tmpInjuredParts.Contains(hediff_Injury.Part))
					{
						HediffSet.tmpInjuredParts.Add(hediff_Injury.Part);
					}
				}
			}
			return HediffSet.tmpInjuredParts;
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00087698 File Offset: 0x00085898
		public List<BodyPartRecord> GetNaturallyHealingInjuredParts()
		{
			HediffSet.tmpNaturallyHealingInjuredParts.Clear();
			foreach (BodyPartRecord bodyPartRecord in this.GetInjuredParts())
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null && this.hediffs[i].Part == bodyPartRecord && hediff_Injury.CanHealNaturally())
					{
						HediffSet.tmpNaturallyHealingInjuredParts.Add(bodyPartRecord);
						break;
					}
				}
			}
			return HediffSet.tmpNaturallyHealingInjuredParts;
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x00087748 File Offset: 0x00085948
		public List<Hediff_MissingPart> GetMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.CacheMissingPartsCommonAncestors();
			}
			return this.cachedMissingPartsCommonAncestors;
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x0008775E File Offset: 0x0008595E
		public IEnumerable<BodyPartRecord> GetNotMissingParts(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartTagDef tag = null, BodyPartRecord partParent = null)
		{
			List<BodyPartRecord> allPartsList = this.pawn.def.race.body.AllParts;
			int num;
			for (int i = 0; i < allPartsList.Count; i = num + 1)
			{
				BodyPartRecord bodyPartRecord = allPartsList[i];
				if (!this.PartIsMissing(bodyPartRecord) && (height == BodyPartHeight.Undefined || bodyPartRecord.height == height) && (depth == BodyPartDepth.Undefined || bodyPartRecord.depth == depth) && (tag == null || bodyPartRecord.def.tags.Contains(tag)) && (partParent == null || bodyPartRecord.parent == partParent))
				{
					yield return bodyPartRecord;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x0008778C File Offset: 0x0008598C
		public BodyPartRecord GetRandomNotMissingPart(DamageDef damDef, BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartRecord partParent = null)
		{
			IEnumerable<BodyPartRecord> notMissingParts;
			if (this.GetNotMissingParts(height, depth, null, partParent).Any((BodyPartRecord p) => p.coverageAbs > 0f))
			{
				notMissingParts = this.GetNotMissingParts(height, depth, null, partParent);
			}
			else
			{
				if (!this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent).Any((BodyPartRecord p) => p.coverageAbs > 0f))
				{
					return null;
				}
				notMissingParts = this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent);
			}
			BodyPartRecord result;
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs * x.def.GetHitChanceFactorFor(damDef), out result))
			{
				return result;
			}
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0008786C File Offset: 0x00085A6C
		public float GetCoverageOfNotMissingNaturalParts(BodyPartRecord part)
		{
			if (this.PartIsMissing(part))
			{
				return 0f;
			}
			if (this.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				return 0f;
			}
			this.coverageRejectedPartsSet.Clear();
			List<Hediff_MissingPart> missingPartsCommonAncestors = this.GetMissingPartsCommonAncestors();
			for (int i = 0; i < missingPartsCommonAncestors.Count; i++)
			{
				this.coverageRejectedPartsSet.Add(missingPartsCommonAncestors[i].Part);
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j] is Hediff_AddedPart)
				{
					this.coverageRejectedPartsSet.Add(this.hediffs[j].Part);
				}
			}
			float num = 0f;
			this.coveragePartsStack.Clear();
			this.coveragePartsStack.Push(part);
			while (this.coveragePartsStack.Any<BodyPartRecord>())
			{
				BodyPartRecord bodyPartRecord = this.coveragePartsStack.Pop();
				num += bodyPartRecord.coverageAbs;
				for (int k = 0; k < bodyPartRecord.parts.Count; k++)
				{
					if (!this.coverageRejectedPartsSet.Contains(bodyPartRecord.parts[k]))
					{
						this.coveragePartsStack.Push(bodyPartRecord.parts[k]);
					}
				}
			}
			this.coveragePartsStack.Clear();
			this.coverageRejectedPartsSet.Clear();
			return num;
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x000879C4 File Offset: 0x00085BC4
		private void CacheMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.cachedMissingPartsCommonAncestors = new List<Hediff_MissingPart>();
			}
			else
			{
				this.cachedMissingPartsCommonAncestors.Clear();
			}
			this.missingPartsCommonAncestorsQueue.Clear();
			this.missingPartsCommonAncestorsQueue.Enqueue(this.pawn.def.race.body.corePart);
			while (this.missingPartsCommonAncestorsQueue.Count != 0)
			{
				BodyPartRecord bodyPartRecord = this.missingPartsCommonAncestorsQueue.Dequeue();
				if (!this.PartOrAnyAncestorHasDirectlyAddedParts(bodyPartRecord))
				{
					Hediff_MissingPart firstHediffMatchingPart = this.GetFirstHediffMatchingPart<Hediff_MissingPart>(bodyPartRecord);
					if (firstHediffMatchingPart != null)
					{
						this.cachedMissingPartsCommonAncestors.Add(firstHediffMatchingPart);
					}
					else
					{
						for (int i = 0; i < bodyPartRecord.parts.Count; i++)
						{
							this.missingPartsCommonAncestorsQueue.Enqueue(bodyPartRecord.parts[i]);
						}
					}
				}
			}
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x00087A8C File Offset: 0x00085C8C
		public bool HasDirectlyAddedPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_AddedPart)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x00087AD9 File Offset: 0x00085CD9
		public bool PartOrAnyAncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return this.HasDirectlyAddedPartFor(part) || (part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent));
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x00087AFF File Offset: 0x00085CFF
		public bool AncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent);
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x00087B1A File Offset: 0x00085D1A
		public IEnumerable<Hediff> GetTendableNonInjuryNonMissingPartHediffs()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num + 1)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow(false))
				{
					yield return this.hediffs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x00087B2C File Offset: 0x00085D2C
		public bool HasTendableNonInjuryNonMissingPartHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && !(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x00087BA8 File Offset: 0x00085DA8
		public bool HasImmunizableNotImmuneHediff()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].Visible && this.hediffs[i].def.PossibleToDevelopImmunityNaturally() && !this.hediffs[i].FullyImmune())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x00087C34 File Offset: 0x00085E34
		public bool AnyHediffMakesSickThought
		{
			get
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					if (this.hediffs[i].def.makesSickThought && this.hediffs[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x00087C85 File Offset: 0x00085E85
		public bool InLabor(bool includePostpartumExhaustion = true)
		{
			return ModsConfig.BiotechActive && (this.HasHediff(HediffDefOf.PregnancyLabor, false) || this.HasHediff(HediffDefOf.PregnancyLaborPushing, false) || (includePostpartumExhaustion && this.HasHediff(HediffDefOf.PostpartumExhaustion, false)));
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x00087CC4 File Offset: 0x00085EC4
		private float CalculateBleedRate()
		{
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.health.Dead)
			{
				return 0f;
			}
			if (this.pawn.Deathresting)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff hediff = this.hediffs[i];
				HediffStage curStage = hediff.CurStage;
				if (curStage != null)
				{
					num *= curStage.totalBleedFactor;
				}
				num2 += hediff.BleedRate;
			}
			return num2 * num / this.pawn.HealthScale;
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x00087D6C File Offset: 0x00085F6C
		private float CalculatePain()
		{
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.Dead)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				num += this.hediffs[i].PainOffset;
			}
			if (this.pawn.genes != null)
			{
				num += this.pawn.genes.PainOffset;
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				num *= this.hediffs[j].PainFactor;
			}
			if (this.pawn.genes != null)
			{
				num *= this.pawn.genes.PainFactor;
			}
			return Mathf.Clamp(num, 0f, 1f);
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x00087E48 File Offset: 0x00086048
		public void Clear()
		{
			this.hediffs.Clear();
			this.DirtyCache();
		}

		// Token: 0x040011BD RID: 4541
		public Pawn pawn;

		// Token: 0x040011BE RID: 4542
		public List<Hediff> hediffs = new List<Hediff>();

		// Token: 0x040011BF RID: 4543
		private List<Hediff_MissingPart> cachedMissingPartsCommonAncestors;

		// Token: 0x040011C0 RID: 4544
		private float cachedPain = -1f;

		// Token: 0x040011C1 RID: 4545
		private float cachedBleedRate = -1f;

		// Token: 0x040011C2 RID: 4546
		private bool? cachedHasHead;

		// Token: 0x040011C3 RID: 4547
		private List<Verb> tmpHediffVerbs = new List<Verb>();

		// Token: 0x040011C4 RID: 4548
		private static List<BodyPartRecord> tmpInjuredParts = new List<BodyPartRecord>();

		// Token: 0x040011C5 RID: 4549
		private static List<BodyPartRecord> tmpNaturallyHealingInjuredParts = new List<BodyPartRecord>();

		// Token: 0x040011C6 RID: 4550
		private Stack<BodyPartRecord> coveragePartsStack = new Stack<BodyPartRecord>();

		// Token: 0x040011C7 RID: 4551
		private HashSet<BodyPartRecord> coverageRejectedPartsSet = new HashSet<BodyPartRecord>();

		// Token: 0x040011C8 RID: 4552
		private Queue<BodyPartRecord> missingPartsCommonAncestorsQueue = new Queue<BodyPartRecord>();
	}
}

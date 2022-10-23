using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F9 RID: 249
	public class HediffDef : Def
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00024F5A File Offset: 0x0002315A
		public bool IsAddiction
		{
			get
			{
				return typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00024F74 File Offset: 0x00023174
		public bool AlwaysAllowMothball
		{
			get
			{
				if (!this.alwaysAllowMothballCached)
				{
					this.alwaysAllowMothball = true;
					if (this.comps != null && this.comps.Count > 0)
					{
						this.alwaysAllowMothball = false;
					}
					if (this.stages != null)
					{
						for (int i = 0; i < this.stages.Count; i++)
						{
							HediffStage hediffStage = this.stages[i];
							if (hediffStage.deathMtbDays > 0f || (hediffStage.hediffGivers != null && hediffStage.hediffGivers.Count > 0))
							{
								this.alwaysAllowMothball = false;
							}
						}
					}
					this.alwaysAllowMothballCached = true;
				}
				return this.alwaysAllowMothball;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00025012 File Offset: 0x00023212
		public Hediff ConcreteExample
		{
			get
			{
				if (this.concreteExampleInt == null)
				{
					this.concreteExampleInt = HediffMaker.Debug_MakeConcreteExampleHediff(this);
				}
				return this.concreteExampleInt;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00025030 File Offset: 0x00023230
		public string Description
		{
			get
			{
				if (this.descriptionCached == null)
				{
					if (!this.descriptionShort.NullOrEmpty())
					{
						this.descriptionCached = this.descriptionShort;
					}
					else
					{
						this.descriptionCached = this.description;
					}
					this.descriptionCached = Regex.Replace(this.descriptionCached, "\\r\\n?|\\n", " ");
				}
				return this.descriptionCached;
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00025090 File Offset: 0x00023290
		public bool HasComp(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000250D8 File Offset: 0x000232D8
		public HediffCompProperties CompPropsFor(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return this.comps[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0002512C File Offset: 0x0002332C
		public T CompProps<T>() where T : HediffCompProperties
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00025184 File Offset: 0x00023384
		public bool PossibleToDevelopImmunityNaturally()
		{
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.CompProps<HediffCompProperties_Immunizable>();
			return hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0f || hediffCompProperties_Immunizable.immunityPerDaySick > 0f);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x000251B8 File Offset: 0x000233B8
		public string PrettyTextForPart(BodyPartRecord bodyPart)
		{
			if (this.labelNounPretty.NullOrEmpty())
			{
				return null;
			}
			return string.Format(this.labelNounPretty, this.label, bodyPart.Label);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x000251E0 File Offset: 0x000233E0
		public override void ResolveReferences()
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].ResolveReferences(this);
				}
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00025220 File Offset: 0x00023420
		public int StageAtSeverity(float severity)
		{
			if (this.stages == null)
			{
				return 0;
			}
			for (int i = this.stages.Count - 1; i >= 0; i--)
			{
				if (severity >= this.stages[i].minSeverity)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00025266 File Offset: 0x00023466
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.hediffClass == null)
			{
				yield return "hediffClass is null";
			}
			if (!this.comps.NullOrEmpty<HediffCompProperties>() && !typeof(HediffWithComps).IsAssignableFrom(this.hediffClass))
			{
				yield return "has comps but hediffClass is not HediffWithComps or subclass thereof";
			}
			if (this.minSeverity > this.initialSeverity)
			{
				yield return "minSeverity is greater than initialSeverity";
			}
			if (this.maxSeverity < this.initialSeverity)
			{
				yield return "maxSeverity is lower than initialSeverity";
			}
			if (!this.tendable && this.HasComp(typeof(HediffComp_TendDuration)))
			{
				yield return "has HediffComp_TendDuration but tendable = false";
			}
			if (string.IsNullOrEmpty(this.description))
			{
				yield return "Hediff with defName " + this.defName + " has no description!";
			}
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (string arg in this.comps[i].ConfigErrors(this))
					{
						yield return this.comps[i] + ": " + arg;
					}
					enumerator = null;
					num = i;
				}
			}
			if (this.stages != null)
			{
				int num;
				if (!typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass))
				{
					for (int i = 0; i < this.stages.Count; i = num + 1)
					{
						if (i >= 1 && this.stages[i].minSeverity <= this.stages[i - 1].minSeverity)
						{
							yield return "stages are not in order of minSeverity";
						}
						num = i;
					}
				}
				for (int i = 0; i < this.stages.Count; i = num + 1)
				{
					if (this.stages[i].makeImmuneTo != null)
					{
						if (!this.stages[i].makeImmuneTo.Any((HediffDef im) => im.HasComp(typeof(HediffComp_Immunizable))))
						{
							yield return "makes immune to hediff which doesn't have comp immunizable";
						}
					}
					if (this.stages[i].hediffGivers != null)
					{
						for (int j = 0; j < this.stages[i].hediffGivers.Count; j = num + 1)
						{
							foreach (string text2 in this.stages[i].hediffGivers[j].ConfigErrors())
							{
								yield return text2;
							}
							enumerator = null;
							num = j;
						}
					}
					if (this.stages[i].minSeverity > this.maxSeverity)
					{
						yield return "minSeverity of stage " + i + " is greater than maxSeverity.";
					}
					num = i;
				}
			}
			if (this.hediffGivers != null)
			{
				int num;
				for (int i = 0; i < this.hediffGivers.Count; i = num + 1)
				{
					foreach (string text3 in this.hediffGivers[i].ConfigErrors())
					{
						yield return text3;
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00025276 File Offset: 0x00023476
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.stages != null && this.stages.Count == 1)
			{
				foreach (StatDrawEntry statDrawEntry in this.stages[0].SpecialDisplayStats())
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00025286 File Offset: 0x00023486
		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
		}

		// Token: 0x040005BF RID: 1471
		public Type hediffClass = typeof(Hediff);

		// Token: 0x040005C0 RID: 1472
		public List<HediffCompProperties> comps;

		// Token: 0x040005C1 RID: 1473
		[MustTranslate]
		public string descriptionShort;

		// Token: 0x040005C2 RID: 1474
		public float initialSeverity = 0.5f;

		// Token: 0x040005C3 RID: 1475
		public float lethalSeverity = -1f;

		// Token: 0x040005C4 RID: 1476
		public List<HediffStage> stages;

		// Token: 0x040005C5 RID: 1477
		public bool tendable;

		// Token: 0x040005C6 RID: 1478
		public bool isBad = true;

		// Token: 0x040005C7 RID: 1479
		public ThingDef spawnThingOnRemoved;

		// Token: 0x040005C8 RID: 1480
		public float chanceToCauseNoPain;

		// Token: 0x040005C9 RID: 1481
		public bool makesSickThought;

		// Token: 0x040005CA RID: 1482
		public bool makesAlert = true;

		// Token: 0x040005CB RID: 1483
		public NeedDef causesNeed;

		// Token: 0x040005CC RID: 1484
		public List<NeedDef> disablesNeeds;

		// Token: 0x040005CD RID: 1485
		public float minSeverity;

		// Token: 0x040005CE RID: 1486
		public float maxSeverity = float.MaxValue;

		// Token: 0x040005CF RID: 1487
		public bool scenarioCanAdd;

		// Token: 0x040005D0 RID: 1488
		public List<HediffGiver> hediffGivers;

		// Token: 0x040005D1 RID: 1489
		public bool cureAllAtOnceIfCuredByItem;

		// Token: 0x040005D2 RID: 1490
		public TaleDef taleOnVisible;

		// Token: 0x040005D3 RID: 1491
		public bool recordDownedTale = true;

		// Token: 0x040005D4 RID: 1492
		public bool everCurableByItem = true;

		// Token: 0x040005D5 RID: 1493
		public List<string> tags;

		// Token: 0x040005D6 RID: 1494
		public bool priceImpact;

		// Token: 0x040005D7 RID: 1495
		public float priceOffset;

		// Token: 0x040005D8 RID: 1496
		public bool chronic;

		// Token: 0x040005D9 RID: 1497
		public bool keepOnBodyPartRestoration;

		// Token: 0x040005DA RID: 1498
		public bool countsAsAddedPartOrImplant;

		// Token: 0x040005DB RID: 1499
		public bool blocksSocialInteraction;

		// Token: 0x040005DC RID: 1500
		public bool blocksSleeping;

		// Token: 0x040005DD RID: 1501
		[MustTranslate]
		public string overrideTooltip;

		// Token: 0x040005DE RID: 1502
		[MustTranslate]
		public string extraTooltip;

		// Token: 0x040005DF RID: 1503
		public bool levelIsQuantity;

		// Token: 0x040005E0 RID: 1504
		public bool removeOnDeathrestStart;

		// Token: 0x040005E1 RID: 1505
		public bool preventsPregnancy;

		// Token: 0x040005E2 RID: 1506
		public bool preventsLungRot;

		// Token: 0x040005E3 RID: 1507
		public bool pregnant;

		// Token: 0x040005E4 RID: 1508
		public bool allowMothballIfLowPriorityWorldPawn;

		// Token: 0x040005E5 RID: 1509
		public List<string> removeWithTags;

		// Token: 0x040005E6 RID: 1510
		public SimpleCurve removeOnRedressChanceByDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			}
		};

		// Token: 0x040005E7 RID: 1511
		public bool removeOnQuestLodgers;

		// Token: 0x040005E8 RID: 1512
		public List<PawnKindDef> removeOnRedressIfNotOfKind;

		// Token: 0x040005E9 RID: 1513
		public bool displayWound;

		// Token: 0x040005EA RID: 1514
		public float? woundAnchorRange;

		// Token: 0x040005EB RID: 1515
		public Color defaultLabelColor = Color.white;

		// Token: 0x040005EC RID: 1516
		public GraphicData eyeGraphicSouth;

		// Token: 0x040005ED RID: 1517
		public GraphicData eyeGraphicEast;

		// Token: 0x040005EE RID: 1518
		public float eyeGraphicScale = 1f;

		// Token: 0x040005EF RID: 1519
		public InjuryProps injuryProps;

		// Token: 0x040005F0 RID: 1520
		public AddedBodyPartProps addedPartProps;

		// Token: 0x040005F1 RID: 1521
		[MustTranslate]
		public string labelNoun;

		// Token: 0x040005F2 RID: 1522
		[MustTranslate]
		public string battleStateLabel;

		// Token: 0x040005F3 RID: 1523
		[MustTranslate]
		public string labelNounPretty;

		// Token: 0x040005F4 RID: 1524
		[MustTranslate]
		public string targetPrefix;

		// Token: 0x040005F5 RID: 1525
		private bool alwaysAllowMothballCached;

		// Token: 0x040005F6 RID: 1526
		private bool alwaysAllowMothball;

		// Token: 0x040005F7 RID: 1527
		private string descriptionCached;

		// Token: 0x040005F8 RID: 1528
		private Hediff concreteExampleInt;
	}
}

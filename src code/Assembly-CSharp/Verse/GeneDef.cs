using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000064 RID: 100
	public class GeneDef : Def
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00018348 File Offset: 0x00016548
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					if (this.iconPath.NullOrEmpty())
					{
						this.cachedIcon = BaseContent.BadTex;
					}
					else
					{
						this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00018398 File Offset: 0x00016598
		public Color IconColor
		{
			get
			{
				if (this.iconColor != null)
				{
					return this.iconColor.Value;
				}
				if (this.skinColorBase != null)
				{
					return this.skinColorBase.Value;
				}
				if (this.skinColorOverride != null)
				{
					return this.skinColorOverride.Value;
				}
				if (this.hairColorOverride != null)
				{
					return this.hairColorOverride.Value;
				}
				return Color.white;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x0001840E File Offset: 0x0001660E
		public string LabelShortAdj
		{
			get
			{
				if (!this.labelShortAdj.NullOrEmpty())
				{
					return this.labelShortAdj;
				}
				return this.label;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0001842A File Offset: 0x0001662A
		public string DescriptionFull
		{
			get
			{
				if (this.cachedDescription == null)
				{
					this.cachedDescription = this.GetDescriptionFull();
				}
				return this.cachedDescription;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00018446 File Offset: 0x00016646
		public bool HasGraphic
		{
			get
			{
				return this.graphicData != null && (this.graphicData.graphicPath != null || this.graphicData.graphicPathFemale != null || !this.graphicData.graphicPaths.NullOrEmpty<string>());
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00018484 File Offset: 0x00016684
		public bool RandomChosen
		{
			get
			{
				return this.randomChosen || (this.biostatArc <= 0 && this.biostatCpx == 0 && this.biostatMet == 0 && (this.hairColorOverride != null || this.skinColorBase != null || this.skinColorOverride != null || this.bodyType != null || !this.forcedHeadTypes.NullOrEmpty<HeadTypeDef>() || this.hairTagFilter != null || this.beardTagFilter != null || this.HasGraphic));
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00018510 File Offset: 0x00016710
		public bool CanDrawNow(Rot4 rotation, GeneDrawLayer drawLayer)
		{
			if (this.graphicData == null || this.graphicData.layer == GeneDrawLayer.None)
			{
				return false;
			}
			if (rotation == Rot4.North)
			{
				if (!this.graphicData.visibleNorth)
				{
					return false;
				}
				if (this.graphicData.drawNorthAfterHair)
				{
					return drawLayer != GeneDrawLayer.PostHair;
				}
			}
			return drawLayer == this.graphicData.layer;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00018573 File Offset: 0x00016773
		public override void ResolveReferences()
		{
			if (this.displayCategory == null)
			{
				this.displayCategory = GeneCategoryDefOf.Miscellaneous;
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00018588 File Offset: 0x00016788
		public int AptitudeFor(SkillDef skill)
		{
			int num = 0;
			if (this.aptitudes.NullOrEmpty<Aptitude>())
			{
				return num;
			}
			for (int i = 0; i < this.aptitudes.Count; i++)
			{
				if (this.aptitudes[i].skill == skill)
				{
					num += this.aptitudes[i].level;
				}
			}
			return num;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000185E8 File Offset: 0x000167E8
		private string GetDescriptionFull()
		{
			GeneDef.<>c__DisplayClass94_0 CS$<>8__locals1 = new GeneDef.<>c__DisplayClass94_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.sb = new StringBuilder();
			if (!this.description.NullOrEmpty())
			{
				CS$<>8__locals1.sb.Append(this.description).AppendLine().AppendLine();
			}
			bool flag = false;
			if (this.prerequisite != null)
			{
				CS$<>8__locals1.sb.AppendLine("Requires".Translate() + ": " + this.prerequisite.LabelCap);
				flag = true;
			}
			if (this.minAgeActive > 0f)
			{
				CS$<>8__locals1.sb.AppendLine("TakesEffectAfterAge".Translate() + ": " + this.minAgeActive);
				flag = true;
			}
			if (flag)
			{
				CS$<>8__locals1.sb.AppendLine();
			}
			bool flag2 = false;
			if (this.biostatCpx != 0)
			{
				CS$<>8__locals1.sb.AppendLineTagged("Complexity".Translate().Colorize(GeneUtility.GCXColor) + ": " + this.biostatCpx.ToStringWithSign());
				flag2 = true;
			}
			if (this.biostatMet != 0)
			{
				CS$<>8__locals1.sb.AppendLineTagged("Metabolism".Translate().Colorize(GeneUtility.METColor) + ": " + this.biostatMet.ToStringWithSign());
				flag2 = true;
			}
			if (this.biostatArc != 0)
			{
				CS$<>8__locals1.sb.AppendLineTagged("ArchitesRequired".Translate().Colorize(GeneUtility.ARCColor) + ": " + this.biostatArc.ToStringWithSign());
				flag2 = true;
			}
			if (flag2)
			{
				CS$<>8__locals1.sb.AppendLine();
			}
			if (this.forcedTraits != null)
			{
				CS$<>8__locals1.sb.AppendLineTagged(("ForcedTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.Append((from x in this.forcedTraits
				select x.def.DataAtDegree(x.degree).label).ToLineList("  - ", true)).AppendLine().AppendLine();
			}
			if (this.suppressedTraits != null)
			{
				CS$<>8__locals1.sb.AppendLineTagged(("SuppressedTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.Append((from x in this.suppressedTraits
				select x.def.DataAtDegree(x.degree).label).ToLineList("  - ", true)).AppendLine().AppendLine();
			}
			if (this.aptitudes != null)
			{
				CS$<>8__locals1.sb.AppendLineTagged(("Aptitudes".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.Append((from x in this.aptitudes
				select x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList("  - ", true)).AppendLine().AppendLine();
			}
			CS$<>8__locals1.effectsTitleWritten = false;
			if (this.passionMod != null)
			{
				PassionMod.PassionModType modType = this.passionMod.modType;
				if (modType != PassionMod.PassionModType.AddOneLevel)
				{
					if (modType == PassionMod.PassionModType.DropAll)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("PassionModDrop".Translate(this.passionMod.skill));
					}
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("PassionModAdd".Translate(this.passionMod.skill));
				}
			}
			if (!this.statFactors.NullOrEmpty<StatModifier>())
			{
				for (int i = 0; i < this.statFactors.Count; i++)
				{
					StatModifier statModifier = this.statFactors[i];
					if (statModifier.stat.CanShowWithLoadedMods())
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier.stat.LabelCap + " " + statModifier.ToStringAsFactor);
					}
				}
			}
			if (!this.conditionalStatAffecters.NullOrEmpty<ConditionalStatAffecter>())
			{
				for (int j = 0; j < this.conditionalStatAffecters.Count; j++)
				{
					if (!this.conditionalStatAffecters[j].statFactors.NullOrEmpty<StatModifier>())
					{
						for (int k = 0; k < this.conditionalStatAffecters[j].statFactors.Count; k++)
						{
							StatModifier statModifier2 = this.conditionalStatAffecters[j].statFactors[k];
							if (statModifier2.stat.CanShowWithLoadedMods())
							{
								CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier2.stat.LabelCap + " " + statModifier2.ToStringAsFactor + " (" + this.conditionalStatAffecters[j].Label + ")");
							}
						}
					}
				}
			}
			if (!this.statOffsets.NullOrEmpty<StatModifier>())
			{
				for (int l = 0; l < this.statOffsets.Count; l++)
				{
					StatModifier statModifier3 = this.statOffsets[l];
					if (statModifier3.stat.CanShowWithLoadedMods())
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier3.stat.LabelCap + " " + statModifier3.ValueToStringAsOffset);
					}
				}
			}
			if (!this.conditionalStatAffecters.NullOrEmpty<ConditionalStatAffecter>())
			{
				for (int m = 0; m < this.conditionalStatAffecters.Count; m++)
				{
					if (!this.conditionalStatAffecters[m].statOffsets.NullOrEmpty<StatModifier>())
					{
						for (int n = 0; n < this.conditionalStatAffecters[m].statOffsets.Count; n++)
						{
							StatModifier statModifier4 = this.conditionalStatAffecters[m].statOffsets[n];
							if (statModifier4.stat.CanShowWithLoadedMods())
							{
								CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier4.stat.LabelCap + " " + statModifier4.ValueToStringAsOffset + " (" + this.conditionalStatAffecters[m].Label.UncapitalizeFirst() + ")");
							}
						}
					}
				}
			}
			if (!this.capMods.NullOrEmpty<PawnCapacityModifier>())
			{
				for (int num = 0; num < this.capMods.Count; num++)
				{
					PawnCapacityModifier pawnCapacityModifier = this.capMods[num];
					if (pawnCapacityModifier.offset != 0f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(pawnCapacityModifier.capacity.GetLabelFor(true, true).CapitalizeFirst() + " " + (pawnCapacityModifier.offset * 100f).ToString("+#;-#") + "%");
					}
					if (pawnCapacityModifier.postFactor != 1f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(pawnCapacityModifier.capacity.GetLabelFor(true, true).CapitalizeFirst() + " x" + pawnCapacityModifier.postFactor.ToStringPercent());
					}
					if (pawnCapacityModifier.setMax != 999f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(pawnCapacityModifier.capacity.GetLabelFor(true, true).CapitalizeFirst() + " " + "max".Translate().CapitalizeFirst() + ": " + pawnCapacityModifier.setMax.ToStringPercent());
					}
				}
			}
			if (!this.customEffectDescriptions.NullOrEmpty<string>())
			{
				foreach (string str in this.customEffectDescriptions)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(str.ResolveTags());
				}
			}
			if (!this.damageFactors.NullOrEmpty<DamageFactor>())
			{
				for (int num2 = 0; num2 < this.damageFactors.Count; num2++)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("DamageType".Translate(this.damageFactors[num2].damageDef.label).CapitalizeFirst() + " x" + this.damageFactors[num2].factor.ToStringPercent());
				}
			}
			if (this.resourceLossPerDay != 0f && !this.resourceLabel.NullOrEmpty())
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("ResourceLossPerDay".Translate(this.resourceLabel.Named("RESOURCE"), (-Mathf.RoundToInt(this.resourceLossPerDay * 100f)).ToStringWithSign().Named("OFFSET")).CapitalizeFirst());
			}
			if (this.painFactor != 1f)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("Pain".Translate() + " x" + this.painFactor.ToStringPercent());
			}
			if (this.painOffset != 0f)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("Pain".Translate() + " " + (this.painOffset * 100f).ToString("+###0;-###0") + "%");
			}
			if (this.addictionChanceFactor != 1f && this.chemical != null)
			{
				if (this.addictionChanceFactor <= 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AddictionImmune".Translate(this.chemical).CapitalizeFirst());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AddictionChanceFactor".Translate(this.chemical).CapitalizeFirst() + " x" + this.addictionChanceFactor.ToStringPercent());
				}
			}
			if (this.causesNeed != null)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("CausesNeed".Translate() + ": " + this.causesNeed.LabelCap);
			}
			if (!this.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				if (this.disablesNeeds.Count == 1)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("DisablesNeed".Translate() + ": " + this.disablesNeeds[0].LabelCap);
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("DisablesNeeds".Translate() + ": " + (from x in this.disablesNeeds
					select x.label).ToCommaList(false, false).CapitalizeFirst());
				}
			}
			if (this.missingGeneRomanceChanceFactor != 1f)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("MissingGeneRomanceChance".Translate(this.label.Named("GENE")) + " x" + this.missingGeneRomanceChanceFactor.ToStringPercent());
			}
			if (this.ignoreDarkness)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("UnaffectedByDarkness".Translate());
			}
			if (this.foodPoisoningChanceFactor != 1f)
			{
				if (this.foodPoisoningChanceFactor <= 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("FoodPoisoningImmune".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("Stat_Hediff_FoodPoisoningChanceFactor_Name".Translate() + " x" + this.foodPoisoningChanceFactor.ToStringPercent());
				}
			}
			if (this.socialFightChanceFactor != 1f)
			{
				if (this.socialFightChanceFactor <= 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("WillNeverSocialFight".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("SocialFightChanceFactor".Translate() + " x" + this.socialFightChanceFactor.ToStringPercent());
				}
			}
			if (this.aggroMentalBreakSelectionChanceFactor != 1f)
			{
				if (this.aggroMentalBreakSelectionChanceFactor >= 999f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AlwaysAggroMentalBreak".Translate());
				}
				else if (this.aggroMentalBreakSelectionChanceFactor <= 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("NeverAggroMentalBreak".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AggroMentalBreakSelectionChanceFactor".Translate() + " x" + this.aggroMentalBreakSelectionChanceFactor.ToStringPercent());
				}
			}
			if (this.prisonBreakMTBFactor != 1f)
			{
				if (this.prisonBreakMTBFactor < 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("WillNeverPrisonBreak".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("PrisonBreakIntervalFactor".Translate() + " x" + this.prisonBreakMTBFactor.ToStringPercent());
				}
			}
			bool flag3 = CS$<>8__locals1.effectsTitleWritten;
			if (!this.makeImmuneTo.NullOrEmpty<HediffDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("ImmuneTo".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine((from x in this.makeImmuneTo
				select x.label).ToLineList("  - ", true));
				flag3 = true;
			}
			if (!this.hediffGiversCannotGive.NullOrEmpty<HediffDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("ImmuneTo".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine((from x in this.hediffGiversCannotGive
				select x.label).ToLineList("  - ", true));
				flag3 = true;
			}
			if (this.biologicalAgeTickFactorFromAgeCurve != null)
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("AgeFactors".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine((from p in this.biologicalAgeTickFactorFromAgeCurve
				select "PeriodYears".Translate(p.x).ToString() + ": x" + p.y.ToStringPercent()).ToLineList("  - ", true));
				flag3 = true;
			}
			if (this.disabledWorkTags != WorkTags.None)
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				IEnumerable<WorkTypeDef> source = from x in DefDatabase<WorkTypeDef>.AllDefsListForReading
				where (CS$<>8__locals1.<>4__this.disabledWorkTags & x.workTags) > WorkTags.None
				select x;
				CS$<>8__locals1.sb.AppendLineTagged(("DisabledWorkLabel".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine("  - " + (from x in source
				select x.labelShort).ToCommaList(false, false).CapitalizeFirst());
				this.disabledWorkTags.LabelTranslated();
				if (this.disabledWorkTags.ExactlyOneWorkTagSet())
				{
					CS$<>8__locals1.sb.AppendLine("  - " + this.disabledWorkTags.LabelTranslated().CapitalizeFirst());
				}
				flag3 = true;
			}
			if (!this.abilities.NullOrEmpty<AbilityDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				if (this.abilities.Count == 1)
				{
					CS$<>8__locals1.sb.AppendLineTagged(("GivesAbility".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				}
				else
				{
					CS$<>8__locals1.sb.AppendLineTagged(("GivesAbilities".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				}
				CS$<>8__locals1.sb.AppendLine((from x in this.abilities
				select x.label).ToLineList("  - ", true));
				flag3 = true;
			}
			IEnumerable<ThoughtDef> enumerable = DefDatabase<ThoughtDef>.AllDefs.Where(delegate(ThoughtDef x)
			{
				if ((x.requiredGenes.NotNullAndContains(CS$<>8__locals1.<>4__this) || x.nullifyingGenes.NotNullAndContains(CS$<>8__locals1.<>4__this)) && x.stages != null)
				{
					return x.stages.Any((ThoughtStage y) => y.baseMoodEffect != 0f);
				}
				return false;
			});
			if (enumerable.Any<ThoughtDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("Mood".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				foreach (ThoughtDef thoughtDef in enumerable)
				{
					ThoughtStage thoughtStage = thoughtDef.stages.FirstOrDefault((ThoughtStage x) => x.baseMoodEffect != 0f);
					if (thoughtStage != null)
					{
						string text = thoughtStage.LabelCap + ": " + thoughtStage.baseMoodEffect.ToStringWithSign("0.##");
						if (thoughtDef.requiredGenes.NotNullAndContains(this))
						{
							CS$<>8__locals1.sb.AppendLine("  - " + text);
						}
						else if (thoughtDef.nullifyingGenes.NotNullAndContains(this))
						{
							CS$<>8__locals1.sb.AppendLine("  - " + "Removes".Translate() + ": " + text);
						}
					}
				}
			}
			return CS$<>8__locals1.sb.ToString().TrimEndNewlines();
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00019830 File Offset: 0x00017A30
		public bool ConflictsWith(GeneDef other)
		{
			if (this.exclusionTags != null && other.exclusionTags != null)
			{
				for (int i = 0; i < this.exclusionTags.Count; i++)
				{
					if (other.exclusionTags.Contains(this.exclusionTags[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001987F File Offset: 0x00017A7F
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.painFactor != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Pain".Translate(), "x" + this.painFactor.ToStringPercent(), "Stat_Hediff_Pain_Desc".Translate(), 4050, null, null, false);
			}
			if (this.painOffset != 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Pain".Translate(), (this.painOffset * 100f).ToString("+###0;-###0") + "%", "Stat_Hediff_Pain_Desc".Translate(), 4050, null, null, false);
			}
			if (this.missingGeneRomanceChanceFactor != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "MissingGeneRomanceChance".Translate(this.LabelCap.Named("GENE")), "x" + this.missingGeneRomanceChanceFactor.ToStringPercent(), "StatsReport_MissingGeneRomanceChance".Translate(), 4050, null, null, false);
			}
			if (this.forcedTraits != null)
			{
				string text = (from x in this.forcedTraits
				select x.def.DataAtDegree(x.degree).label).ToLineList(null, true);
				yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "ForcedTraits".Translate(), text, "ForcedTraitsDesc".Translate() + "\n\n" + text, 4080, null, null, false);
			}
			if (this.aptitudes != null)
			{
				string text2 = (from x in this.aptitudes
				select x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList(null, true);
				yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Aptitudes".Translate().CapitalizeFirst(), text2, "AptitudesDesc".Translate() + "\n\n" + text2, 4090, null, null, false);
			}
			if (this.statOffsets != null)
			{
				int num;
				for (int i = 0; i < this.statOffsets.Count; i = num + 1)
				{
					StatModifier statModifier = this.statOffsets[i];
					if (statModifier.stat.CanShowWithLoadedMods())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifier.stat.LabelCap, statModifier.ValueToStringAsOffset, statModifier.stat.description, 4070, null, null, false);
					}
					num = i;
				}
			}
			if (this.statFactors != null)
			{
				int num;
				for (int i = 0; i < this.statFactors.Count; i = num + 1)
				{
					StatModifier statModifier2 = this.statFactors[i];
					if (statModifier2.stat.CanShowWithLoadedMods())
					{
						yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifier2.stat.LabelCap, statModifier2.ToStringAsFactor, statModifier2.stat.description, 4070, null, null, false);
					}
					num = i;
				}
			}
			if (this.capMods != null)
			{
				int num;
				for (int i = 0; i < this.capMods.Count; i = num + 1)
				{
					PawnCapacityModifier capMod = this.capMods[i];
					if (capMod.offset != 0f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, capMod.capacity.GetLabelFor(true, true).CapitalizeFirst(), (capMod.offset * 100f).ToString("+#;-#") + "%", capMod.capacity.description, 4060, null, null, false);
					}
					if (capMod.postFactor != 1f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, capMod.capacity.GetLabelFor(true, true).CapitalizeFirst(), "x" + capMod.postFactor.ToStringPercent(), capMod.capacity.description, 4060, null, null, false);
					}
					if (capMod.SetMaxDefined && req.Pawn != null)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.CapacityEffects, capMod.capacity.GetLabelFor(true, true).CapitalizeFirst(), "max".Translate().CapitalizeFirst() + " " + capMod.EvaluateSetMax(req.Pawn).ToStringPercent(), capMod.capacity.description, 4060, null, null, false);
					}
					capMod = null;
					num = i;
				}
			}
			if (this.biostatCpx != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "Complexity".Translate(), this.biostatCpx.ToString(), "ComplexityDesc".Translate(), 998, null, null, false);
			}
			if (this.biostatMet != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "Metabolism".Translate(), this.biostatMet.ToString(), "MetabolismDesc".Translate(), 997, null, null, false);
			}
			if (this.biostatArc != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Genetics, "ArchitesRequired".Translate(), this.biostatArc.ToString(), "ArchitesRequiredDesc".Translate(), 995, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00019896 File Offset: 0x00017A96
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.geneClass == null)
			{
				yield return "geneClass is null";
			}
			if (!typeof(Gene).IsAssignableFrom(this.geneClass))
			{
				yield return "geneClass is not Gene or subclass thereof";
			}
			if (this.mentalBreakMtbDays > 0f && this.mentalBreakDef == null)
			{
				yield return "mentalBreakMtbDays is >0 with null mentalBreakDef";
			}
			if (this.graphicData != null)
			{
				foreach (string str in this.graphicData.ConfigErrors())
				{
					yield return "graphicData - " + str;
				}
				IEnumerator<string> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04000178 RID: 376
		public Type geneClass = typeof(Gene);

		// Token: 0x04000179 RID: 377
		[MustTranslate]
		public string labelShortAdj;

		// Token: 0x0400017A RID: 378
		[MustTranslate]
		public List<string> customEffectDescriptions;

		// Token: 0x0400017B RID: 379
		[NoTranslate]
		public string iconPath;

		// Token: 0x0400017C RID: 380
		private Color? iconColor;

		// Token: 0x0400017D RID: 381
		public GeneCategoryDef displayCategory;

		// Token: 0x0400017E RID: 382
		public float displayOrderInCategory;

		// Token: 0x0400017F RID: 383
		public GeneGraphicData graphicData;

		// Token: 0x04000180 RID: 384
		public bool neverGrayHair;

		// Token: 0x04000181 RID: 385
		public SoundDef soundCall;

		// Token: 0x04000182 RID: 386
		public SoundDef soundDeath;

		// Token: 0x04000183 RID: 387
		public SoundDef soundWounded;

		// Token: 0x04000184 RID: 388
		public Type resourceGizmoType = typeof(GeneGizmo_Resource);

		// Token: 0x04000185 RID: 389
		public float resourceLossPerDay;

		// Token: 0x04000186 RID: 390
		[MustTranslate]
		public string resourceLabel;

		// Token: 0x04000187 RID: 391
		[MustTranslate]
		public string resourceDescription;

		// Token: 0x04000188 RID: 392
		public List<float> resourceGizmoThresholds;

		// Token: 0x04000189 RID: 393
		public bool showGizmoOnWorldView;

		// Token: 0x0400018A RID: 394
		public List<AbilityDef> abilities;

		// Token: 0x0400018B RID: 395
		public List<GeneticTraitData> forcedTraits;

		// Token: 0x0400018C RID: 396
		public List<GeneticTraitData> suppressedTraits;

		// Token: 0x0400018D RID: 397
		public List<NeedDef> disablesNeeds;

		// Token: 0x0400018E RID: 398
		public NeedDef causesNeed;

		// Token: 0x0400018F RID: 399
		public WorkTags disabledWorkTags;

		// Token: 0x04000190 RID: 400
		public bool ignoreDarkness;

		// Token: 0x04000191 RID: 401
		public EndogeneCategory endogeneCategory;

		// Token: 0x04000192 RID: 402
		public bool dislikesSunlight;

		// Token: 0x04000193 RID: 403
		public float minAgeActive;

		// Token: 0x04000194 RID: 404
		public float lovinMTBFactor = 1f;

		// Token: 0x04000195 RID: 405
		public bool immuneToToxGasExposure;

		// Token: 0x04000196 RID: 406
		public bool randomChosen;

		// Token: 0x04000197 RID: 407
		public List<Aptitude> aptitudes;

		// Token: 0x04000198 RID: 408
		public PassionMod passionMod;

		// Token: 0x04000199 RID: 409
		public List<StatModifier> statOffsets;

		// Token: 0x0400019A RID: 410
		public List<StatModifier> statFactors;

		// Token: 0x0400019B RID: 411
		public List<ConditionalStatAffecter> conditionalStatAffecters;

		// Token: 0x0400019C RID: 412
		public float painOffset;

		// Token: 0x0400019D RID: 413
		public float painFactor = 1f;

		// Token: 0x0400019E RID: 414
		public float foodPoisoningChanceFactor = 1f;

		// Token: 0x0400019F RID: 415
		public List<DamageFactor> damageFactors;

		// Token: 0x040001A0 RID: 416
		public SimpleCurve biologicalAgeTickFactorFromAgeCurve;

		// Token: 0x040001A1 RID: 417
		public List<HediffDef> makeImmuneTo;

		// Token: 0x040001A2 RID: 418
		public List<HediffDef> hediffGiversCannotGive;

		// Token: 0x040001A3 RID: 419
		public ChemicalDef chemical;

		// Token: 0x040001A4 RID: 420
		public float addictionChanceFactor = 1f;

		// Token: 0x040001A5 RID: 421
		public bool sterilize;

		// Token: 0x040001A6 RID: 422
		public List<PawnCapacityModifier> capMods;

		// Token: 0x040001A7 RID: 423
		public bool preventPermanentWounds;

		// Token: 0x040001A8 RID: 424
		public bool dontMindRawFood;

		// Token: 0x040001A9 RID: 425
		public Color? hairColorOverride;

		// Token: 0x040001AA RID: 426
		public Color? skinColorBase;

		// Token: 0x040001AB RID: 427
		public Color? skinColorOverride;

		// Token: 0x040001AC RID: 428
		public float randomBrightnessFactor;

		// Token: 0x040001AD RID: 429
		public TagFilter hairTagFilter;

		// Token: 0x040001AE RID: 430
		public TagFilter beardTagFilter;

		// Token: 0x040001AF RID: 431
		public GeneticBodyType? bodyType;

		// Token: 0x040001B0 RID: 432
		public List<HeadTypeDef> forcedHeadTypes;

		// Token: 0x040001B1 RID: 433
		public float minMelanin = -1f;

		// Token: 0x040001B2 RID: 434
		public HairDef forcedHair;

		// Token: 0x040001B3 RID: 435
		public bool womenCanHaveBeards;

		// Token: 0x040001B4 RID: 436
		public float socialFightChanceFactor = 1f;

		// Token: 0x040001B5 RID: 437
		public float aggroMentalBreakSelectionChanceFactor = 1f;

		// Token: 0x040001B6 RID: 438
		public float mentalBreakMtbDays;

		// Token: 0x040001B7 RID: 439
		public MentalBreakDef mentalBreakDef;

		// Token: 0x040001B8 RID: 440
		public float missingGeneRomanceChanceFactor = 1f;

		// Token: 0x040001B9 RID: 441
		public float prisonBreakMTBFactor = 1f;

		// Token: 0x040001BA RID: 442
		public int biostatCpx = 1;

		// Token: 0x040001BB RID: 443
		public int biostatMet;

		// Token: 0x040001BC RID: 444
		public int biostatArc;

		// Token: 0x040001BD RID: 445
		public List<string> exclusionTags;

		// Token: 0x040001BE RID: 446
		public GeneDef prerequisite;

		// Token: 0x040001BF RID: 447
		public float selectionWeight = 1f;

		// Token: 0x040001C0 RID: 448
		public float selectionWeightFactorDarkSkin = 1f;

		// Token: 0x040001C1 RID: 449
		public bool canGenerateInGeneSet = true;

		// Token: 0x040001C2 RID: 450
		public GeneSymbolPack symbolPack;

		// Token: 0x040001C3 RID: 451
		public float marketValueFactor = 1f;

		// Token: 0x040001C4 RID: 452
		public bool removeOnRedress;

		// Token: 0x040001C5 RID: 453
		[Unsaved(false)]
		private string cachedDescription;

		// Token: 0x040001C6 RID: 454
		[Unsaved(false)]
		private Texture2D cachedIcon;
	}
}

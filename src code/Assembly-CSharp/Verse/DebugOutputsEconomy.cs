using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000443 RID: 1091
	public static class DebugOutputsEconomy
	{
		// Token: 0x06002100 RID: 8448 RVA: 0x000C8608 File Offset: 0x000C6808
		[DebugOutput("Economy", false)]
		public static void ApparelByStuff()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Stuffless", delegate()
			{
				DebugOutputsEconomy.DoTableInternalApparel(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			foreach (ThingDef localStuff2 in from td in DefDatabase<ThingDef>.AllDefs
			where td.IsStuff
			select td)
			{
				ThingDef localStuff = localStuff2;
				list.Add(new FloatMenuOption(localStuff.defName, delegate()
				{
					DebugOutputsEconomy.DoTableInternalApparel(localStuff);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000C8700 File Offset: 0x000C6900
		[DebugOutput("Economy", false)]
		public static void ApparelArmor()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("label", (ThingDef x) => x.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("stuff", (ThingDef x) => x.MadeFromStuff.ToStringCheckBlank()));
			list.Add(new TableDataGetter<ThingDef>("mass", (ThingDef x) => x.BaseMass));
			list.Add(new TableDataGetter<ThingDef>("mrkt\nvalue", (ThingDef x) => x.BaseMarketValue.ToString("F0")));
			list.Add(new TableDataGetter<ThingDef>("hp", (ThingDef x) => x.BaseMaxHitPoints));
			list.Add(new TableDataGetter<ThingDef>("flama\nbility", (ThingDef x) => x.BaseFlammability));
			list.Add(new TableDataGetter<ThingDef>("recipe\nmin\nskill", delegate(ThingDef x)
			{
				if (x.recipeMaker == null || x.recipeMaker.skillRequirements.NullOrEmpty<SkillRequirement>())
				{
					return "";
				}
				return x.recipeMaker.skillRequirements[0].skill.defName + " " + x.recipeMaker.skillRequirements[0].minLevel;
			}));
			list.Add(new TableDataGetter<ThingDef>("equip\ndelay", (ThingDef x) => x.GetStatValueAbstract(StatDefOf.EquipDelay, null)));
			list.Add(new TableDataGetter<ThingDef>("none", delegate(ThingDef x)
			{
				if (x.MadeFromStuff)
				{
					return "";
				}
				return string.Concat(new string[]
				{
					x.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null).ToStringPercent(),
					" / ",
					x.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null).ToStringPercent(),
					" / ",
					x.GetStatValueAbstract(StatDefOf.ArmorRating_Heat, null).ToStringPercent()
				});
			}));
			list.Add(new TableDataGetter<ThingDef>("verbs", (ThingDef x) => string.Join(",", from v in x.Verbs
			select v.label)));
			foreach (ThingDef stuffLocal2 in new List<ThingDef>
			{
				ThingDefOf.Steel,
				ThingDefOf.Plasteel,
				ThingDefOf.Cloth,
				ThingDef.Named("Leather_Patch"),
				ThingDefOf.Leather_Plain,
				ThingDef.Named("Leather_Heavy"),
				ThingDef.Named("Leather_Thrumbo"),
				ThingDef.Named("Synthread"),
				ThingDef.Named("Hyperweave"),
				ThingDef.Named("DevilstrandCloth"),
				ThingDef.Named("WoolSheep"),
				ThingDef.Named("WoolMegasloth"),
				ThingDefOf.BlocksGranite,
				ThingDefOf.Silver,
				ThingDefOf.Gold
			})
			{
				ThingDef stuffLocal = stuffLocal2;
				if (DefDatabase<ThingDef>.AllDefs.Any((ThingDef x) => x.IsApparel && stuffLocal.stuffProps.CanMake(x)))
				{
					list.Add(new TableDataGetter<ThingDef>(stuffLocal.label.Shorten(), delegate(ThingDef x)
					{
						if (!stuffLocal.stuffProps.CanMake(x))
						{
							return "";
						}
						return string.Concat(new string[]
						{
							x.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuffLocal).ToStringPercent(),
							" / ",
							x.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuffLocal).ToStringPercent(),
							" / ",
							x.GetStatValueAbstract(StatDefOf.ArmorRating_Heat, stuffLocal).ToStringPercent()
						});
					}));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsApparel
			orderby x.BaseMarketValue
			select x, list.ToArray());
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000C8A98 File Offset: 0x000C6C98
		[DebugOutput("Economy", false)]
		public static void ApparelInsulation()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("label", (ThingDef x) => x.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("none", delegate(ThingDef x)
			{
				if (x.MadeFromStuff)
				{
					return "";
				}
				return x.GetStatValueAbstract(StatDefOf.Insulation_Heat, null).ToStringTemperature("F1") + " / " + x.GetStatValueAbstract(StatDefOf.Insulation_Cold, null).ToStringTemperature("F1");
			}));
			foreach (ThingDef stuffLocal2 in from x in DefDatabase<ThingDef>.AllDefs
			where x.IsStuff
			orderby x.BaseMarketValue
			select x)
			{
				ThingDef stuffLocal = stuffLocal2;
				if (DefDatabase<ThingDef>.AllDefs.Any((ThingDef x) => x.IsApparel && stuffLocal.stuffProps.CanMake(x)))
				{
					list.Add(new TableDataGetter<ThingDef>(stuffLocal.label.Shorten(), delegate(ThingDef x)
					{
						if (!stuffLocal.stuffProps.CanMake(x))
						{
							return "";
						}
						return x.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuffLocal).ToString("F1") + ", " + x.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuffLocal).ToString("F1");
					}));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsApparel
			orderby x.BaseMarketValue
			select x, list.ToArray());
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000C8C30 File Offset: 0x000C6E30
		[DebugOutput("Economy", false)]
		public static void ApparelCountsForNudity()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef x) => x.defName));
			list.Add(new TableDataGetter<ThingDef>("label", (ThingDef x) => x.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("countsAsClothingForNudity", (ThingDef x) => x.apparel.countsAsClothingForNudity));
			DebugTables.MakeTablesDialog<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsApparel
			orderby x.BaseMarketValue
			select x, list.ToArray());
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000C8D28 File Offset: 0x000C6F28
		private static void DoTableInternalApparel(ThingDef stuff)
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsApparel && (stuff == null || (d.MadeFromStuff && stuff.stuffProps.CanMake(d)))
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[15];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("body\nparts", (ThingDef d) => GenText.ToSpaceList(from bp in d.apparel.bodyPartGroups
			select bp.defName));
			array[2] = new TableDataGetter<ThingDef>("layers", (ThingDef d) => GenText.ToSpaceList(from l in d.apparel.layers
			select l.ToString()));
			array[3] = new TableDataGetter<ThingDef>("tags", (ThingDef d) => GenText.ToSpaceList(from t in d.apparel.tags
			select t.ToString()));
			array[4] = new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0"));
			array[5] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, stuff).ToString("F0"));
			array[6] = new TableDataGetter<ThingDef>("insul.\ncold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff).ToString("F1"));
			array[7] = new TableDataGetter<ThingDef>("insul.\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff).ToString("F1"));
			array[8] = new TableDataGetter<ThingDef>("armor\nblunt", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuff).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("armor\nsharp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuff).ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("StuffEffectMult.\narmor", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierArmor, stuff).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("StuffEffectMult.\ncold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierInsulation_Cold, stuff).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("StuffEffectMult.\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierInsulation_Heat, stuff).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("equip\ntime", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.EquipDelay, stuff).ToString("F1"));
			array[14] = new TableDataGetter<ThingDef>("ingredients", (ThingDef d) => DebugOutputsEconomy.CostListString(d, false, false));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000C8F40 File Offset: 0x000C7140
		[DebugOutput("Economy", false)]
		public static void RecipeSkills()
		{
			IEnumerable<RecipeDef> allDefs = DefDatabase<RecipeDef>.AllDefs;
			TableDataGetter<RecipeDef>[] array = new TableDataGetter<RecipeDef>[5];
			array[0] = new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName);
			array[1] = new TableDataGetter<RecipeDef>("workSkill", delegate(RecipeDef d)
			{
				if (d.workSkill != null)
				{
					return d.workSkill.defName;
				}
				return "";
			});
			array[2] = new TableDataGetter<RecipeDef>("workSpeedStat", delegate(RecipeDef d)
			{
				if (d.workSpeedStat != null)
				{
					return d.workSpeedStat.defName;
				}
				return "";
			});
			array[3] = new TableDataGetter<RecipeDef>("workSpeedStat's skillNeedFactors", delegate(RecipeDef d)
			{
				if (d.workSpeedStat == null)
				{
					return "";
				}
				if (!d.workSpeedStat.skillNeedFactors.NullOrEmpty<SkillNeed>())
				{
					return (from fac in d.workSpeedStat.skillNeedFactors
					select fac.skill.defName).ToCommaList(false, false);
				}
				return "";
			});
			array[4] = new TableDataGetter<RecipeDef>("workSkillLearnFactor", (RecipeDef d) => d.workSkillLearnFactor);
			DebugTables.MakeTablesDialog<RecipeDef>(allDefs, array);
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000C903C File Offset: 0x000C723C
		[DebugOutput("Economy", false)]
		public static void Drugs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsWithinCategory(ThingCategoryDefOf.Medicine) || d.IsWithinCategory(ThingCategoryDefOf.Drugs)
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[30];
			array[0] = new TableDataGetter<ThingDef>("name", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
			array[2] = new TableDataGetter<ThingDef>("ingredients", (ThingDef d) => DebugOutputsEconomy.CostListString(d, true, true));
			array[3] = new TableDataGetter<ThingDef>("work\namount", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.WorkToProduceBest(d) <= 0f)
				{
					return "-";
				}
				return DebugOutputsEconomy.WorkToProduceBest(d).ToString("F0");
			});
			array[4] = new TableDataGetter<ThingDef>("real\ningredient cost", (ThingDef d) => DebugOutputsEconomy.<Drugs>g__RealIngredientCost|6_0(d).ToString("F1"));
			array[5] = new TableDataGetter<ThingDef>("real\nsell price", (ThingDef d) => DebugOutputsEconomy.<Drugs>g__RealSellPrice|6_1(d).ToStringMoney(null));
			array[6] = new TableDataGetter<ThingDef>("real\nprofit\nper item", (ThingDef d) => (DebugOutputsEconomy.<Drugs>g__RealSellPrice|6_1(d) - DebugOutputsEconomy.<Drugs>g__RealIngredientCost|6_0(d)).ToStringMoney(null));
			array[7] = new TableDataGetter<ThingDef>("real\nprofit\nper day's work", (ThingDef d) => ((DebugOutputsEconomy.<Drugs>g__RealSellPrice|6_1(d) - DebugOutputsEconomy.<Drugs>g__RealIngredientCost|6_0(d)) / DebugOutputsEconomy.WorkToProduceBest(d) * 30000f).ToStringMoney(null));
			array[8] = new TableDataGetter<ThingDef>("real\nbuy price", (ThingDef d) => DebugOutputsEconomy.<Drugs>g__RealBuyPrice|6_2(d).ToStringMoney(null));
			array[9] = new TableDataGetter<ThingDef>("for\npleasure", (ThingDef d) => d.IsPleasureDrug.ToStringCheckBlank());
			array[10] = new TableDataGetter<ThingDef>("non\nmedical", (ThingDef d) => d.IsNonMedicalDrug.ToStringCheckBlank());
			array[11] = new TableDataGetter<ThingDef>("joy", delegate(ThingDef d)
			{
				if (!d.IsPleasureDrug)
				{
					return "-";
				}
				return d.ingestible.joy.ToString();
			});
			array[12] = new TableDataGetter<ThingDef>("high\ngain", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetDrugHighGiver(d) == null)
				{
					return "-";
				}
				if (DrugStatsUtility.GetDrugHighGiver(d).severity <= 0f)
				{
					return "-";
				}
				return DrugStatsUtility.GetDrugHighGiver(d).severity.ToString();
			});
			array[13] = new TableDataGetter<ThingDef>("high\noffset\nper day", delegate(ThingDef d)
			{
				IngestionOutcomeDoer_GiveHediff drugHighGiver = DrugStatsUtility.GetDrugHighGiver(d);
				if (((drugHighGiver != null) ? drugHighGiver.hediffDef : null) == null)
				{
					return "-";
				}
				return DrugStatsUtility.GetHighOffsetPerDay(d).ToString();
			});
			array[14] = new TableDataGetter<ThingDef>("high\ndays\nper dose", delegate(ThingDef d)
			{
				IngestionOutcomeDoer_GiveHediff drugHighGiver = DrugStatsUtility.GetDrugHighGiver(d);
				if (((drugHighGiver != null) ? drugHighGiver.hediffDef : null) == null)
				{
					return "-";
				}
				return (DrugStatsUtility.GetDrugHighGiver(d).severity / -DrugStatsUtility.GetHighOffsetPerDay(d)).ToString("F2");
			});
			array[15] = new TableDataGetter<ThingDef>("tolerance\ngain", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetToleranceGain(d) <= 0f)
				{
					return "-";
				}
				return DrugStatsUtility.GetToleranceGain(d).ToStringPercent();
			});
			array[16] = new TableDataGetter<ThingDef>("tolerance\noffset\nper day", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetTolerance(d) == null)
				{
					return "-";
				}
				return DrugStatsUtility.GetToleranceOffsetPerDay(d).ToStringPercent();
			});
			array[17] = new TableDataGetter<ThingDef>("tolerance\ndays\nper dose", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetTolerance(d) == null)
				{
					return "-";
				}
				return (DrugStatsUtility.GetToleranceGain(d) / -DrugStatsUtility.GetToleranceOffsetPerDay(d)).ToString("F2");
			});
			array[18] = new TableDataGetter<ThingDef>("addiction\nmin tolerance", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__Addictive|6_4(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__MinToleranceToAddict|6_11(d).ToString();
			});
			array[19] = new TableDataGetter<ThingDef>("addiction\nnew chance", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__Addictive|6_4(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__NewAddictionChance|6_6(d).ToStringPercent();
			});
			array[20] = new TableDataGetter<ThingDef>("addiction\nnew severity", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__Addictive|6_4(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__NewAddictionSeverity|6_7(d).ToString();
			});
			array[21] = new TableDataGetter<ThingDef>("addiction\nold severity gain", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__Addictive|6_4(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__OldAddictionSeverityOffset|6_8(d).ToString();
			});
			array[22] = new TableDataGetter<ThingDef>("addiction\noffset\nper day", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<Drugs>g__Addiction|6_5(d) == null)
				{
					return "-";
				}
				return DrugStatsUtility.GetAddictionOffsetPerDay(d).ToString();
			});
			array[23] = new TableDataGetter<ThingDef>("addiction\nrecover\nmin days", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<Drugs>g__Addiction|6_5(d) == null)
				{
					return "-";
				}
				return (DebugOutputsEconomy.<Drugs>g__NewAddictionSeverity|6_7(d) / -DrugStatsUtility.GetAddictionOffsetPerDay(d)).ToString("F2");
			});
			array[24] = new TableDataGetter<ThingDef>("need fall\nper day", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetNeed(d) == null)
				{
					return "-";
				}
				return DrugStatsUtility.GetNeed(d).fallPerDay.ToString("F2");
			});
			array[25] = new TableDataGetter<ThingDef>("need cost\nper day", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetNeed(d) == null)
				{
					return "-";
				}
				return DrugStatsUtility.GetAddictionNeedCostPerDay(d).ToStringMoney(null);
			});
			array[26] = new TableDataGetter<ThingDef>("overdose\nseverity gain", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__IsDrug|6_3(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__OverdoseSeverity|6_9(d).ToString();
			});
			array[27] = new TableDataGetter<ThingDef>("overdose\nrandom-emerg\nchance", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__IsDrug|6_3(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__LargeOverdoseChance|6_10(d).ToStringPercent();
			});
			array[28] = new TableDataGetter<ThingDef>("combat\ndrug", (ThingDef d) => (DebugOutputsEconomy.<Drugs>g__IsDrug|6_3(d) && d.GetCompProperties<CompProperties_Drug>().isCombatEnhancingDrug).ToStringCheckBlank());
			array[29] = new TableDataGetter<ThingDef>("safe dose\ninterval", (ThingDef d) => DrugStatsUtility.GetSafeDoseIntervalReadout(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000C95BC File Offset: 0x000C77BC
		[DebugOutput("Economy", false)]
		public static void Wool()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh && d.GetCompProperties<CompProperties_Shearable>() != null
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("animal", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("woolDef", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolDef.defName);
			array[2] = new TableDataGetter<ThingDef>("woolAmount", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolAmount.ToString());
			array[3] = new TableDataGetter<ThingDef>("woolValue", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolDef.BaseMarketValue.ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("shear interval", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().shearIntervalDays.ToString("F1"));
			array[5] = new TableDataGetter<ThingDef>("value yearly", delegate(ThingDef d)
			{
				CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
				return (compProperties.woolDef.BaseMarketValue * (float)compProperties.woolAmount * (60f / (float)compProperties.shearIntervalDays)).ToString("F0");
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x000C9705 File Offset: 0x000C7905
		private static float AdultAgeDays(ThingDef d)
		{
			return d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge * 60f;
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x000C9734 File Offset: 0x000C7934
		private static float SlaughterValue(ThingDef d)
		{
			float num = 0f;
			if (d.race.meatDef != null)
			{
				num = AnimalProductionUtility.AdultMeatAmount(d) * d.race.meatDef.BaseMarketValue;
			}
			float num2 = 0f;
			if (d.race.leatherDef != null)
			{
				num2 = AnimalProductionUtility.AdultLeatherAmount(d) * d.race.leatherDef.BaseMarketValue;
			}
			return num + num2;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x000C979C File Offset: 0x000C799C
		private static float SlaughterValuePerGrowthYear(ThingDef d)
		{
			float num = DebugOutputsEconomy.AdultAgeDays(d) / 60f;
			return DebugOutputsEconomy.SlaughterValue(d) / num;
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x000C97BE File Offset: 0x000C79BE
		private static float TotalMarketValueOutputPerYear(ThingDef d)
		{
			return 0f + AnimalProductionUtility.MilkMarketValuePerYear(d) + AnimalProductionUtility.WoolMarketValuePerYear(d) + AnimalProductionUtility.EggMarketValuePerYear(d);
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x000C97DC File Offset: 0x000C79DC
		[DebugOutput("Economy", false)]
		public static void AnimalEconomy()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh && !d.race.Humanlike
			orderby d.devNote
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[46];
			array[0] = new TableDataGetter<ThingDef>("devNote", (ThingDef d) => d.devNote);
			array[1] = new TableDataGetter<ThingDef>("", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("trainability", delegate(ThingDef d)
			{
				if (d.race.trainability != null)
				{
					return d.race.trainability.defName;
				}
				return "";
			});
			array[3] = new TableDataGetter<ThingDef>("hunger\nrate\nadult", (ThingDef d) => d.race.baseHungerRate.ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("eaten\nnutriton\nyearly", (ThingDef d) => DebugOutputsEconomy.<AnimalEconomy>g__EatenNutritionPerYear|12_7(d).ToString("F1"));
			array[5] = new TableDataGetter<ThingDef>("gestation\ndays raw", (ThingDef d) => AnimalProductionUtility.GestationDaysLitter(d).ToString("F1"));
			array[6] = new TableDataGetter<ThingDef>("litter size\naverage", (ThingDef d) => DebugOutputsEconomy.LitterSizeAverage(d).ToString("F1"));
			array[7] = new TableDataGetter<ThingDef>("gestation\ndays each", (ThingDef d) => AnimalProductionUtility.GestationDaysEach(d).ToString("F1"));
			array[8] = new TableDataGetter<ThingDef>("herbivore", delegate(ThingDef d)
			{
				if (!AnimalProductionUtility.Herbivore(d))
				{
					return "";
				}
				return "He";
			});
			array[9] = new TableDataGetter<ThingDef>("grass to\nmaintain", delegate(ThingDef d)
			{
				if (AnimalProductionUtility.Herbivore(d))
				{
					return AnimalProductionUtility.GrassToMaintain(d).ToString("F0");
				}
				return "";
			});
			array[10] = new TableDataGetter<ThingDef>("value output\nper nutrition", (ThingDef d) => DebugOutputsEconomy.<AnimalEconomy>g__TotalMarketValuePerNutritionEaten|12_9(d).ToStringMoney(null));
			array[11] = new TableDataGetter<ThingDef>("body\nsize", (ThingDef d) => d.race.baseBodySize.ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("filth", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.FilthRate, null));
			array[13] = new TableDataGetter<ThingDef>("adult age\ndays", (ThingDef d) => DebugOutputsEconomy.AdultAgeDays(d).ToString("F1"));
			array[14] = new TableDataGetter<ThingDef>("nutrition to\nadulthood", (ThingDef d) => AnimalProductionUtility.NutritionToAdulthood(d).ToString("F2"));
			array[15] = new TableDataGetter<ThingDef>("adult meat\namount", (ThingDef d) => AnimalProductionUtility.AdultMeatAmount(d).ToString("F0"));
			array[16] = new TableDataGetter<ThingDef>("adult meat\nnutrition", (ThingDef d) => DebugOutputsEconomy.<AnimalEconomy>g__AdultMeatNutrition|12_0(d).ToString("F2"));
			array[17] = new TableDataGetter<ThingDef>("adult meat\nnutrition per\ninput nutrition", (ThingDef d) => DebugOutputsEconomy.<AnimalEconomy>g__AdultMeatNutritionPerInput|12_3(d).ToString("F3"));
			array[18] = new TableDataGetter<ThingDef>("slaughter value", (ThingDef d) => DebugOutputsEconomy.SlaughterValue(d).ToStringMoney(null));
			array[19] = new TableDataGetter<ThingDef>("slaughter value\n/input nutrition", (ThingDef d) => DebugOutputsEconomy.<AnimalEconomy>g__SlaughterValuePerInputNutrition|12_8(d).ToStringMoney(null));
			array[20] = new TableDataGetter<ThingDef>("slaughter value\n/growth year", (ThingDef d) => DebugOutputsEconomy.SlaughterValuePerGrowthYear(d).ToStringMoney(null));
			array[21] = new TableDataGetter<ThingDef>("eggs\nyearly", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsEggLayer|12_4(d))
				{
					return AnimalProductionUtility.EggsPerYear(d).ToString("F1");
				}
				return "";
			});
			array[22] = new TableDataGetter<ThingDef>("egg\nvalue", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsEggLayer|12_4(d))
				{
					return AnimalProductionUtility.EggMarketValue(d).ToStringMoney(null);
				}
				return "";
			});
			array[23] = new TableDataGetter<ThingDef>("egg\nvalue\nyearly", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsEggLayer|12_4(d))
				{
					return AnimalProductionUtility.EggMarketValuePerYear(d).ToStringMoney(null);
				}
				return "";
			});
			array[24] = new TableDataGetter<ThingDef>("egg\nnutrition", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsEggLayer|12_4(d))
				{
					return AnimalProductionUtility.EggNutrition(d).ToString("F1");
				}
				return "";
			});
			array[25] = new TableDataGetter<ThingDef>("egg\nnutrition\nyearly", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsEggLayer|12_4(d))
				{
					return AnimalProductionUtility.EggNutritionPerYear(d).ToString("F1");
				}
				return "";
			});
			array[26] = new TableDataGetter<ThingDef>("milk\nyearly", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsMilkable|12_5(d))
				{
					return AnimalProductionUtility.MilkPerYear(d).ToString("F1");
				}
				return "";
			});
			array[27] = new TableDataGetter<ThingDef>("milk\nvalue", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsMilkable|12_5(d))
				{
					return AnimalProductionUtility.MilkMarketValue(d).ToStringMoney(null);
				}
				return "";
			});
			array[28] = new TableDataGetter<ThingDef>("milk\nvalue\nyearly", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsMilkable|12_5(d))
				{
					return AnimalProductionUtility.MilkMarketValuePerYear(d).ToStringMoney(null);
				}
				return "";
			});
			array[29] = new TableDataGetter<ThingDef>("milk nutrition\nyearly", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsMilkable|12_5(d))
				{
					return AnimalProductionUtility.MilkNutritionPerYear(d).ToString("F1");
				}
				return "";
			});
			array[30] = new TableDataGetter<ThingDef>("wool\nyearly", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsShearable|12_6(d))
				{
					return AnimalProductionUtility.WoolPerYear(d).ToString("F0");
				}
				return "";
			});
			array[31] = new TableDataGetter<ThingDef>("wool\nvalue", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsShearable|12_6(d))
				{
					return AnimalProductionUtility.WoolMarketValue(d).ToStringMoney(null);
				}
				return "";
			});
			array[32] = new TableDataGetter<ThingDef>("wool value\nyearly", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__IsShearable|12_6(d))
				{
					return AnimalProductionUtility.WoolMarketValuePerYear(d).ToStringMoney(null);
				}
				return "";
			});
			array[33] = new TableDataGetter<ThingDef>("temp\nmin", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null).ToString("F0"));
			array[34] = new TableDataGetter<ThingDef>("temp\nmax", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null).ToString("F0"));
			array[35] = new TableDataGetter<ThingDef>("temp\nwidth", (ThingDef d) => (d.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null) - d.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null)).ToString("F0"));
			array[36] = new TableDataGetter<ThingDef>("move\nspeed", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MoveSpeed, null).ToString());
			array[37] = new TableDataGetter<ThingDef>("wildness", (ThingDef d) => d.race.wildness.ToStringPercent());
			array[38] = new TableDataGetter<ThingDef>("roam\nMTB days", delegate(ThingDef d)
			{
				if (d.race.roamMtbDays != null)
				{
					return d.race.roamMtbDays.Value.ToString("F1");
				}
				return "";
			});
			array[39] = new TableDataGetter<ThingDef>("petness", delegate(ThingDef d)
			{
				if (d.race.petness > 0f)
				{
					return d.race.petness.ToStringPercent();
				}
				return "";
			});
			array[40] = new TableDataGetter<ThingDef>("nuzzle\nMTB hours", delegate(ThingDef d)
			{
				if (d.race.nuzzleMtbHours >= 0f)
				{
					return d.race.nuzzleMtbHours.ToString("F0");
				}
				return "";
			});
			array[41] = new TableDataGetter<ThingDef>("baby\nsize", (ThingDef d) => (d.race.lifeStageAges[0].def.bodySizeFactor * d.race.baseBodySize).ToString("F2"));
			array[42] = new TableDataGetter<ThingDef>("nutrition to\ngestate", (ThingDef d) => AnimalProductionUtility.NutritionToGestate(d).ToString("F2"));
			array[43] = new TableDataGetter<ThingDef>("baby meat\nnutrition", (ThingDef d) => DebugOutputsEconomy.<AnimalEconomy>g__BabyMeatNutrition|12_1(d).ToString("F2"));
			array[44] = new TableDataGetter<ThingDef>("baby meat\nnutrition per\ninput nutrition", (ThingDef d) => DebugOutputsEconomy.<AnimalEconomy>g__BabyMeatNutritionPerInputNutrition|12_2(d).ToString("F2"));
			array[45] = new TableDataGetter<ThingDef>("should\neat babies", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalEconomy>g__BabyMeatNutritionPerInputNutrition|12_2(d) <= DebugOutputsEconomy.<AnimalEconomy>g__AdultMeatNutritionPerInput|12_3(d))
				{
					return "";
				}
				return "B";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000CA050 File Offset: 0x000C8250
		[DebugOutput("Economy", false)]
		public static void AnimalBreeding()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh
			orderby AnimalProductionUtility.GestationDaysEach(d) descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("gestation\ndays litter", (ThingDef d) => AnimalProductionUtility.GestationDaysEach(d).ToString("F1"));
			array[2] = new TableDataGetter<ThingDef>("offspring\ncount range", (ThingDef d) => AnimalProductionUtility.OffspringRange(d).ToString());
			array[3] = new TableDataGetter<ThingDef>("gestation\ndays group", (ThingDef d) => AnimalProductionUtility.GestationDaysLitter(d).ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("growth per 30d", delegate(ThingDef d)
			{
				float f = 1f + (d.HasComp(typeof(CompEggLayer)) ? d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average : ((d.race.litterSizeCurve != null) ? Rand.ByCurveAverage(d.race.litterSizeCurve) : 1f));
				float num = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge * 60f + (d.HasComp(typeof(CompEggLayer)) ? d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays : d.race.gestationPeriodDays);
				float p = 30f / num;
				return Mathf.Pow(f, p).ToString("F2");
			});
			array[5] = new TableDataGetter<ThingDef>("growth per 60d", delegate(ThingDef d)
			{
				float f = 1f + (d.HasComp(typeof(CompEggLayer)) ? d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average : ((d.race.litterSizeCurve != null) ? Rand.ByCurveAverage(d.race.litterSizeCurve) : 1f));
				float num = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge * 60f + (d.HasComp(typeof(CompEggLayer)) ? d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays : d.race.gestationPeriodDays);
				float p = 60f / num;
				return Mathf.Pow(f, p).ToString("F2");
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000CA1C0 File Offset: 0x000C83C0
		private static float LitterSizeAverage(ThingDef d)
		{
			if (d.HasComp(typeof(CompEggLayer)))
			{
				return d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average;
			}
			if (d.race.litterSizeCurve == null)
			{
				return 1f;
			}
			return Rand.ByCurveAverage(d.race.litterSizeCurve);
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000CA214 File Offset: 0x000C8414
		[DebugOutput("Economy", false)]
		public static void BuildingSkills()
		{
			IEnumerable<BuildableDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>())
			where d.BuildableByPlayer
			select d;
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[3];
			array[0] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[1] = new TableDataGetter<BuildableDef>("construction skill prerequisite", (BuildableDef d) => d.constructionSkillPrerequisite);
			array[2] = new TableDataGetter<BuildableDef>("artistic skill prerequisite", (BuildableDef d) => d.artisticSkillPrerequisite);
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000CA2F0 File Offset: 0x000C84F0
		[DebugOutput("Economy", false)]
		public static void Crops()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant && d.plant.Harvestable && d.plant.Sowable
			orderby d.plant.IsTree
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[17];
			array[0] = new TableDataGetter<ThingDef>("plant", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("product", (ThingDef d) => d.plant.harvestedThingDef.defName);
			array[2] = new TableDataGetter<ThingDef>("grow\ntime", (ThingDef d) => d.plant.growDays.ToString("F1"));
			array[3] = new TableDataGetter<ThingDef>("work\nsow", (ThingDef d) => d.plant.sowWork.ToString("F0"));
			array[4] = new TableDataGetter<ThingDef>("work\nharvest", (ThingDef d) => d.plant.harvestWork.ToString("F0"));
			array[5] = new TableDataGetter<ThingDef>("work\ntotal", (ThingDef d) => (d.plant.sowWork + d.plant.harvestWork).ToString("F0"));
			array[6] = new TableDataGetter<ThingDef>("harvest\nyield", (ThingDef d) => d.plant.harvestYield.ToString("F1"));
			array[7] = new TableDataGetter<ThingDef>("work-cost\nper cycle", (ThingDef d) => DebugOutputsEconomy.<Crops>g__workCost|16_0(d).ToString("F2"));
			array[8] = new TableDataGetter<ThingDef>("work-cost\nper harvestCount", (ThingDef d) => (DebugOutputsEconomy.<Crops>g__workCost|16_0(d) / d.plant.harvestYield).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("value\neach", (ThingDef d) => d.plant.harvestedThingDef.BaseMarketValue.ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("harvest Value\nTotal", (ThingDef d) => (d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("profit\nper growDay", (ThingDef d) => ((d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue - DebugOutputsEconomy.<Crops>g__workCost|16_0(d)) / d.plant.growDays).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("nutrition\nper growDay", delegate(ThingDef d)
			{
				if (d.plant.harvestedThingDef.ingestible == null)
				{
					return "";
				}
				return (d.plant.harvestYield * d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.plant.growDays).ToString("F2");
			});
			array[13] = new TableDataGetter<ThingDef>("nutrition", delegate(ThingDef d)
			{
				if (d.plant.harvestedThingDef.ingestible == null)
				{
					return "";
				}
				return d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2");
			});
			array[14] = new TableDataGetter<ThingDef>("fert\nmin", (ThingDef d) => d.plant.fertilityMin.ToStringPercent());
			array[15] = new TableDataGetter<ThingDef>("fert\nsensitivity", (ThingDef d) => d.plant.fertilitySensitivity.ToStringPercent());
			array[16] = new TableDataGetter<ThingDef>("yield per\nharvest work", (ThingDef d) => (d.plant.harvestYield / d.plant.harvestWork).ToString("F3"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000CA64C File Offset: 0x000C884C
		[DebugOutput("Economy", false)]
		public static void ItemAndBuildingAcquisition()
		{
			Func<ThingDef, string> calculatedMarketValue = delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.Producible(d))
				{
					return "not producible";
				}
				if (!d.StatBaseDefined(StatDefOf.MarketValue))
				{
					return "used";
				}
				string text = StatWorker_MarketValue.CalculatedBaseMarketValue(d, null).ToString("F1");
				if (StatWorker_MarketValue.CalculableRecipe(d) != null)
				{
					return text + " (recipe)";
				}
				return text;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.01f) || (d.category == ThingCategory.Building && (d.BuildableByPlayer || d.Minifiable))
			orderby d.BaseMarketValue
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[16];
			array[0] = new TableDataGetter<ThingDef>("cat.", (ThingDef d) => d.category.ToString().Substring(0, 1).CapitalizeFirst());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("mobile", (ThingDef d) => (d.category == ThingCategory.Item || d.Minifiable).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("base\nmarket value", (ThingDef d) => d.BaseMarketValue.ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("calculated\nmarket value", (ThingDef d) => calculatedMarketValue(d));
			array[5] = new TableDataGetter<ThingDef>("cost to make", (ThingDef d) => DebugOutputsEconomy.CostToMakeString(d, false));
			array[6] = new TableDataGetter<ThingDef>("work to produce", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.WorkToProduceBest(d) <= 0f)
				{
					return "-";
				}
				return DebugOutputsEconomy.WorkToProduceBest(d).ToString("F1");
			});
			array[7] = new TableDataGetter<ThingDef>("profit", (ThingDef d) => (d.BaseMarketValue - DebugOutputsEconomy.CostToMake(d, false)).ToString("F1"));
			array[8] = new TableDataGetter<ThingDef>("profit\nrate", delegate(ThingDef d)
			{
				if (d.recipeMaker == null)
				{
					return "-";
				}
				return ((d.BaseMarketValue - DebugOutputsEconomy.CostToMake(d, false)) / DebugOutputsEconomy.WorkToProduceBest(d) * 10000f).ToString("F0");
			});
			array[9] = new TableDataGetter<ThingDef>("market value\ndefined", (ThingDef d) => d.statBases.Any((StatModifier st) => st.stat == StatDefOf.MarketValue).ToStringCheckBlank());
			array[10] = new TableDataGetter<ThingDef>("producible", (ThingDef d) => DebugOutputsEconomy.Producible(d).ToStringCheckBlank());
			array[11] = new TableDataGetter<ThingDef>("thing set\nmaker tags", delegate(ThingDef d)
			{
				if (!d.thingSetMakerTags.NullOrEmpty<string>())
				{
					return d.thingSetMakerTags.ToCommaList(false, false);
				}
				return "";
			});
			array[12] = new TableDataGetter<ThingDef>("made\nfrom\nstuff", (ThingDef d) => d.MadeFromStuff.ToStringCheckBlank());
			array[13] = new TableDataGetter<ThingDef>("cost list", (ThingDef d) => DebugOutputsEconomy.CostListString(d, false, false));
			array[14] = new TableDataGetter<ThingDef>("recipes", (ThingDef d) => DebugOutputsEconomy.<ItemAndBuildingAcquisition>g__recipes|17_0(d));
			array[15] = new TableDataGetter<ThingDef>("work amount\nsources", (ThingDef d) => DebugOutputsEconomy.<ItemAndBuildingAcquisition>g__workAmountSources|17_1(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000CA994 File Offset: 0x000C8B94
		[DebugOutput("Economy", false)]
		public static void ItemAccessibility()
		{
			IEnumerable<ThingDef> dataSources = from x in ThingSetMakerUtility.allGeneratableItems
			orderby x.defName
			select x;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("1", delegate(ThingDef d)
			{
				if (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1, Find.CurrentMap))
				{
					return "";
				}
				return "✓";
			});
			array[2] = new TableDataGetter<ThingDef>("10", delegate(ThingDef d)
			{
				if (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10, Find.CurrentMap))
				{
					return "";
				}
				return "✓";
			});
			array[3] = new TableDataGetter<ThingDef>("100", delegate(ThingDef d)
			{
				if (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 100, Find.CurrentMap))
				{
					return "";
				}
				return "✓";
			});
			array[4] = new TableDataGetter<ThingDef>("1000", delegate(ThingDef d)
			{
				if (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1000, Find.CurrentMap))
				{
					return "";
				}
				return "✓";
			});
			array[5] = new TableDataGetter<ThingDef>("10000", delegate(ThingDef d)
			{
				if (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10000, Find.CurrentMap))
				{
					return "";
				}
				return "✓";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000CAAE0 File Offset: 0x000C8CE0
		[DebugOutput("Economy", false)]
		public static void ThingSetMakerTags()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			list.Add(new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToString("F1")));
			List<TableDataGetter<ThingDef>> list2 = list;
			using (IEnumerator<string> enumerator = (from d in DefDatabase<ThingDef>.AllDefs
			where d.thingSetMakerTags != null
			select d).SelectMany((ThingDef d) => d.thingSetMakerTags).Distinct<string>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string uniqueTag = enumerator.Current;
					list2.Add(new TableDataGetter<ThingDef>(uniqueTag, (ThingDef d) => (d.thingSetMakerTags != null && d.thingSetMakerTags.Contains(uniqueTag)).ToStringCheckBlank()));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.01f) || (d.category == ThingCategory.Building && d.Minifiable)
			orderby d.BaseMarketValue
			select d, list2.ToArray());
			string text = "";
			string[] array = new string[]
			{
				"RewardStandardHighFreq",
				"RewardStandardMidFreq",
				"RewardStandardLowFreq"
			};
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.thingSetMakerTags != null)
				{
					int num = 0;
					for (int i = 0; i < array.Length; i++)
					{
						if (thingDef.thingSetMakerTags.Contains(array[i]))
						{
							num++;
						}
					}
					if (num > 1)
					{
						text = string.Concat(new object[]
						{
							text,
							thingDef.defName,
							": ",
							num,
							" reward tags\n"
						});
					}
				}
			}
			if (text.Length > 0)
			{
				Log.Warning(text);
			}
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000CAD3C File Offset: 0x000C8F3C
		[DebugOutput("Economy", false)]
		public static void ThingSmeltProducts()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				Thing thing = ThingMaker.MakeThing(thingDef, GenStuff.DefaultStuffFor(thingDef));
				if (thing.SmeltProducts(1f).Any<Thing>())
				{
					stringBuilder.Append(thing.LabelCap + ": ");
					foreach (Thing thing2 in thing.SmeltProducts(1f))
					{
						stringBuilder.Append(" " + thing2.Label);
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000CAE28 File Offset: 0x000C9028
		[DebugOutput("Economy", false)]
		public static void Recipes()
		{
			IEnumerable<RecipeDef> dataSources = from d in DefDatabase<RecipeDef>.AllDefs
			where !d.products.NullOrEmpty<ThingDefCountClass>() && !d.ingredients.NullOrEmpty<IngredientCount>()
			select d;
			TableDataGetter<RecipeDef>[] array = new TableDataGetter<RecipeDef>[12];
			array[0] = new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName);
			array[1] = new TableDataGetter<RecipeDef>("work /w carry", (RecipeDef d) => DebugOutputsEconomy.TrueWorkWithCarryTime(d).ToString("F0"));
			array[2] = new TableDataGetter<RecipeDef>("work seconds", (RecipeDef d) => (DebugOutputsEconomy.TrueWorkWithCarryTime(d) / 60f).ToString("F0"));
			array[3] = new TableDataGetter<RecipeDef>("cheapest products value", (RecipeDef d) => DebugOutputsEconomy.CheapestProductsValue(d).ToString("F1"));
			array[4] = new TableDataGetter<RecipeDef>("cheapest ingredients value", (RecipeDef d) => DebugOutputsEconomy.CheapestIngredientValue(d).ToString("F1"));
			array[5] = new TableDataGetter<RecipeDef>("work value", (RecipeDef d) => DebugOutputsEconomy.WorkValueEstimate(d).ToString("F1"));
			array[6] = new TableDataGetter<RecipeDef>("profit raw", (RecipeDef d) => (DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.CheapestIngredientValue(d)).ToString("F1"));
			array[7] = new TableDataGetter<RecipeDef>("profit with work", (RecipeDef d) => (DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.WorkValueEstimate(d) - DebugOutputsEconomy.CheapestIngredientValue(d)).ToString("F1"));
			array[8] = new TableDataGetter<RecipeDef>("profit per work day", (RecipeDef d) => ((DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.CheapestIngredientValue(d)) * 60000f / DebugOutputsEconomy.TrueWorkWithCarryTime(d)).ToString("F0"));
			array[9] = new TableDataGetter<RecipeDef>("min skill", delegate(RecipeDef d)
			{
				if (!d.skillRequirements.NullOrEmpty<SkillRequirement>())
				{
					return d.skillRequirements[0].Summary;
				}
				return "";
			});
			array[10] = new TableDataGetter<RecipeDef>("cheapest stuff", delegate(RecipeDef d)
			{
				if (DebugOutputsEconomy.CheapestNonDerpStuff(d) == null)
				{
					return "";
				}
				return DebugOutputsEconomy.CheapestNonDerpStuff(d).defName;
			});
			array[11] = new TableDataGetter<RecipeDef>("cheapest ingredients", (RecipeDef d) => (from pa in DebugOutputsEconomy.CheapestIngredients(d)
			select pa.First.defName + " x" + pa.Second).ToCommaList(false, false));
			DebugTables.MakeTablesDialog<RecipeDef>(dataSources, array);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x000CB080 File Offset: 0x000C9280
		[DebugOutput("Economy", false)]
		public static void Floors()
		{
			IEnumerable<TerrainDef> dataSources = (from d in DefDatabase<TerrainDef>.AllDefs
			where d.designationCategory == DesignationCategoryDefOf.Floors || d == TerrainDefOf.Soil
			select d).Concat(TerrainDefGenerator_Stone.ImpliedTerrainDefs());
			TableDataGetter<TerrainDef>[] array = new TableDataGetter<TerrainDef>[5];
			array[0] = new TableDataGetter<TerrainDef>("defName", (TerrainDef d) => d.defName);
			array[1] = new TableDataGetter<TerrainDef>("stuff cost", delegate(TerrainDef d)
			{
				if (d.CostList.NullOrEmpty<ThingDefCountClass>())
				{
					return "";
				}
				return d.CostList.First<ThingDefCountClass>().Label;
			});
			array[2] = new TableDataGetter<TerrainDef>("work to build", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.WorkToBuild, null));
			array[3] = new TableDataGetter<TerrainDef>("beauty", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null));
			array[4] = new TableDataGetter<TerrainDef>("cleanliness", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Cleanliness, null));
			DebugTables.MakeTablesDialog<TerrainDef>(dataSources, array);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x000CB1A8 File Offset: 0x000C93A8
		private static bool Producible(BuildableDef b)
		{
			ThingDef d = b as ThingDef;
			TerrainDef terrainDef = b as TerrainDef;
			if (d != null)
			{
				Predicate<ThingDefCountClass> <>9__1;
				if (DefDatabase<RecipeDef>.AllDefs.Any(delegate(RecipeDef r)
				{
					List<ThingDefCountClass> products = r.products;
					Predicate<ThingDefCountClass> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((ThingDefCountClass pr) => pr.thingDef == d));
					}
					return products.Any(predicate);
				}))
				{
					return true;
				}
				if (d.category == ThingCategory.Building && d.BuildableByPlayer)
				{
					return true;
				}
			}
			else if (terrainDef != null)
			{
				return terrainDef.BuildableByPlayer;
			}
			return false;
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000CB218 File Offset: 0x000C9418
		public static string CostListString(BuildableDef d, bool divideByVolume, bool starIfOnlyBuyable)
		{
			if (!DebugOutputsEconomy.Producible(d))
			{
				return "";
			}
			List<string> list = new List<string>();
			if (d.CostList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in d.CostList)
				{
					float num = (float)thingDefCountClass.count;
					if (divideByVolume)
					{
						num /= thingDefCountClass.thingDef.VolumePerUnit;
					}
					string text = thingDefCountClass.thingDef + " x" + num;
					if (starIfOnlyBuyable && DebugOutputsEconomy.RequiresBuying(thingDefCountClass.thingDef))
					{
						text += "*";
					}
					list.Add(text);
				}
			}
			if (d.MadeFromStuff)
			{
				list.Add("stuff x" + d.CostStuffCount);
			}
			return list.ToCommaList(false, false);
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000CB308 File Offset: 0x000C9508
		private static float TrueWorkWithCarryTime(RecipeDef d)
		{
			ThingDef stuffDef = DebugOutputsEconomy.CheapestNonDerpStuff(d);
			return (float)d.ingredients.Count * 90f + d.WorkAmountTotal(stuffDef) + 90f;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000CB33C File Offset: 0x000C953C
		private static float CheapestIngredientValue(RecipeDef d)
		{
			float num = 0f;
			foreach (Pair<ThingDef, float> pair in DebugOutputsEconomy.CheapestIngredients(d))
			{
				num += pair.First.BaseMarketValue * pair.Second;
			}
			return num;
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000CB3A0 File Offset: 0x000C95A0
		private static IEnumerable<Pair<ThingDef, float>> CheapestIngredients(RecipeDef d)
		{
			foreach (IngredientCount ingredientCount in d.ingredients)
			{
				ThingDef thingDef = (from td in ingredientCount.filter.AllowedThingDefs
				where td != ThingDefOf.Meat_Human
				select td).MinBy((ThingDef td) => td.BaseMarketValue / td.VolumePerUnit);
				yield return new Pair<ThingDef, float>(thingDef, ingredientCount.GetBaseCount() / d.IngredientValueGetter.ValuePerUnitOf(thingDef));
			}
			List<IngredientCount>.Enumerator enumerator = default(List<IngredientCount>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000CB3B0 File Offset: 0x000C95B0
		private static float WorkValueEstimate(RecipeDef d)
		{
			return DebugOutputsEconomy.TrueWorkWithCarryTime(d) * 0.01f;
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x000CB3C0 File Offset: 0x000C95C0
		private static ThingDef CheapestNonDerpStuff(RecipeDef d)
		{
			ThingDef productDef = d.products[0].thingDef;
			if (!productDef.MadeFromStuff)
			{
				return null;
			}
			return (from td in d.ingredients.First<IngredientCount>().filter.AllowedThingDefs
			where !productDef.IsWeapon || !PawnWeaponGenerator.IsDerpWeapon(productDef, td)
			select td).MinBy((ThingDef td) => td.BaseMarketValue / td.VolumePerUnit);
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x000CB444 File Offset: 0x000C9644
		private static float CheapestProductsValue(RecipeDef d)
		{
			float num = 0f;
			foreach (ThingDefCountClass thingDefCountClass in d.products)
			{
				num += thingDefCountClass.thingDef.GetStatValueAbstract(StatDefOf.MarketValue, DebugOutputsEconomy.CheapestNonDerpStuff(d)) * (float)thingDefCountClass.count;
			}
			return num;
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x000CB4B8 File Offset: 0x000C96B8
		private static string CostToMakeString(ThingDef d, bool real = false)
		{
			if (d.recipeMaker == null)
			{
				return "-";
			}
			return DebugOutputsEconomy.CostToMake(d, real).ToString("F1");
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000CB4E8 File Offset: 0x000C96E8
		private static float CostToMake(ThingDef d, bool real = false)
		{
			if (d.recipeMaker == null)
			{
				return d.BaseMarketValue;
			}
			float num = 0f;
			if (d.CostList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in d.CostList)
				{
					float num2 = 1f;
					if (real)
					{
						num2 = (DebugOutputsEconomy.RequiresBuying(thingDefCountClass.thingDef) ? 1.4f : 0.6f);
					}
					num += (float)thingDefCountClass.count * DebugOutputsEconomy.CostToMake(thingDefCountClass.thingDef, true) * num2;
				}
			}
			if (d.CostStuffCount > 0)
			{
				ThingDef thingDef = GenStuff.DefaultStuffFor(d);
				num += (float)d.CostStuffCount * thingDef.BaseMarketValue;
			}
			return num;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000CB5B4 File Offset: 0x000C97B4
		private static bool RequiresBuying(ThingDef def)
		{
			if (def.CostList != null)
			{
				using (List<ThingDefCountClass>.Enumerator enumerator = def.CostList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (DebugOutputsEconomy.RequiresBuying(enumerator.Current.thingDef))
						{
							return true;
						}
					}
				}
				return false;
			}
			return !DefDatabase<ThingDef>.AllDefs.Any((ThingDef d) => d.plant != null && d.plant.harvestedThingDef == def && d.plant.Sowable);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000CB64C File Offset: 0x000C984C
		public static float WorkToProduceBest(BuildableDef d)
		{
			float num = float.MaxValue;
			if (d.StatBaseDefined(StatDefOf.WorkToMake))
			{
				num = d.GetStatValueAbstract(StatDefOf.WorkToMake, null);
			}
			if (d.StatBaseDefined(StatDefOf.WorkToBuild) && d.GetStatValueAbstract(StatDefOf.WorkToBuild, null) < num)
			{
				num = d.GetStatValueAbstract(StatDefOf.WorkToBuild, null);
			}
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
			{
				if (recipeDef.workAmount > 0f && !recipeDef.products.NullOrEmpty<ThingDefCountClass>())
				{
					for (int i = 0; i < recipeDef.products.Count; i++)
					{
						if (recipeDef.products[i].thingDef == d && recipeDef.workAmount < num)
						{
							num = recipeDef.workAmount;
						}
					}
				}
			}
			if (num != 3.4028235E+38f)
			{
				return num;
			}
			return -1f;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000CB740 File Offset: 0x000C9940
		[DebugOutput("Economy", false)]
		public static void HediffsPriceImpact()
		{
			IEnumerable<HediffDef> allDefs = DefDatabase<HediffDef>.AllDefs;
			List<TableDataGetter<HediffDef>> list = new List<TableDataGetter<HediffDef>>();
			list.Add(new TableDataGetter<HediffDef>("defName", (HediffDef h) => h.defName));
			list.Add(new TableDataGetter<HediffDef>("price impact", (HediffDef h) => h.priceImpact.ToStringCheckBlank()));
			list.Add(new TableDataGetter<HediffDef>("price offset", delegate(HediffDef h)
			{
				if (h.priceOffset != 0f)
				{
					return h.priceOffset.ToStringMoney(null);
				}
				if (h.spawnThingOnRemoved != null)
				{
					return h.spawnThingOnRemoved.BaseMarketValue.ToStringMoney(null);
				}
				return "";
			}));
			DebugTables.MakeTablesDialog<HediffDef>(allDefs, list.ToArray());
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000CB7F0 File Offset: 0x000C99F0
		[DebugOutput("Economy", false)]
		public static void RoamingVsEconomy()
		{
			float cowMarketValuePerRoam = DebugOutputsEconomy.<RoamingVsEconomy>g__MarketValuePerRoam|36_1(ThingDefOf.Cow);
			float cowSlaughterValuePerRoam = DebugOutputsEconomy.<RoamingVsEconomy>g__SlaughterValuePerRoam|36_2(ThingDefOf.Cow);
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.Roamer
			orderby d.devNote
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[13];
			array[0] = new TableDataGetter<ThingDef>("devNote", (ThingDef d) => d.devNote);
			array[1] = new TableDataGetter<ThingDef>("", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("wildness", (ThingDef d) => d.race.wildness.ToStringPercent());
			array[3] = new TableDataGetter<ThingDef>("roam\nMTB days", (ThingDef d) => d.race.roamMtbDays.Value.ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("roams\navg per year", (ThingDef d) => DebugOutputsEconomy.<RoamingVsEconomy>g__RoamsPerYear|36_0(d).ToString("F1"));
			array[5] = new TableDataGetter<ThingDef>("trainability", delegate(ThingDef d)
			{
				if (d.race.trainability != null)
				{
					return d.race.trainability.defName;
				}
				return "";
			});
			array[6] = new TableDataGetter<ThingDef>("grass to\nmaintain", delegate(ThingDef d)
			{
				if (AnimalProductionUtility.Herbivore(d))
				{
					return AnimalProductionUtility.GrassToMaintain(d).ToString("F0");
				}
				return "";
			});
			array[7] = new TableDataGetter<ThingDef>("yearly market value", (ThingDef d) => DebugOutputsEconomy.TotalMarketValueOutputPerYear(d).ToStringMoney(null));
			array[8] = new TableDataGetter<ThingDef>("yearly market value\ndollars per roam", (ThingDef d) => DebugOutputsEconomy.<RoamingVsEconomy>g__MarketValuePerRoam|36_1(d).ToStringMoney(null));
			array[9] = new TableDataGetter<ThingDef>("yearly market value\ndollars per roam\ncow normalized", (ThingDef d) => (DebugOutputsEconomy.<RoamingVsEconomy>g__MarketValuePerRoam|36_1(d) / cowMarketValuePerRoam).ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("yearly slaughter value", (ThingDef d) => DebugOutputsEconomy.SlaughterValuePerGrowthYear(d).ToStringMoney(null));
			array[11] = new TableDataGetter<ThingDef>("yearly slaughter value\ndollars per roam", (ThingDef d) => DebugOutputsEconomy.<RoamingVsEconomy>g__SlaughterValuePerRoam|36_2(d).ToStringMoney(null));
			array[12] = new TableDataGetter<ThingDef>("yearly slaughter value\ndollars per roam\ncow normalized", (ThingDef d) => (DebugOutputsEconomy.<RoamingVsEconomy>g__SlaughterValuePerRoam|36_2(d) / cowSlaughterValuePerRoam).ToString("F2"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000CBA98 File Offset: 0x000C9C98
		[DebugOutput("Economy", false)]
		public static void ArchonexusAllowedItems()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.ArchonexusMaxAllowedCount != 0
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[14];
			array[0] = new TableDataGetter<ThingDef>("label", (ThingDef d) => d.LabelCap);
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("allowed count", (ThingDef d) => d.ArchonexusMaxAllowedCount);
			array[3] = new TableDataGetter<ThingDef>("category", (ThingDef d) => string.Join<ThingCategoryDef>(",", d.thingCategories));
			array[4] = new TableDataGetter<ThingDef>("sort prio", (ThingDef d) => TransferableUIUtility.DefaultArchonexusItemListOrderPriority(d));
			array[5] = new TableDataGetter<ThingDef>("max value", (ThingDef d) => d.BaseMarketValue * (float)d.ArchonexusMaxAllowedCount);
			array[6] = new TableDataGetter<ThingDef>("x1 value", (ThingDef d) => d.BaseMarketValue);
			array[7] = new TableDataGetter<ThingDef>("x1 weight", (ThingDef d) => d.BaseMass.ToString("0.###"));
			array[8] = new TableDataGetter<ThingDef>("max stack", (ThingDef d) => d.stackLimit);
			array[9] = new TableDataGetter<ThingDef>("weight limit", (ThingDef d) => (int)(5f / d.BaseMass));
			array[10] = new TableDataGetter<ThingDef>("value limit", (ThingDef d) => (int)(2000f / d.BaseMarketValue));
			array[11] = new TableDataGetter<ThingDef>("bringable", (ThingDef d) => MoveColonyUtility.IsBringableItem(ThingMaker.MakeThing(d, GenStuff.RandomStuffFor(d))));
			array[12] = new TableDataGetter<ThingDef>("show distinct", (ThingDef d) => MoveColonyUtility.IsDistinctArchonexusItem(d));
			array[13] = new TableDataGetter<ThingDef>("path", (ThingDef d) => d.fileName);
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000CBD48 File Offset: 0x000C9F48
		[DebugOutput("Economy", false)]
		public static void RewardTags()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			List<TableDataGetter<ThingDef>> list2 = list;
			using (IEnumerator<string> enumerator = (from x in (from d in DefDatabase<ThingDef>.AllDefs
			where d.thingSetMakerTags != null
			select d).SelectMany((ThingDef d) => d.thingSetMakerTags)
			where x.Contains("RewardStandard")
			select x).Distinct<string>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string uniqueTag = enumerator.Current;
					list2.Add(new TableDataGetter<ThingDef>(uniqueTag, (ThingDef d) => (d.thingSetMakerTags != null && d.thingSetMakerTags.Contains(uniqueTag)).ToStringCheckBlank()));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs.Where(delegate(ThingDef d)
			{
				if (d.thingSetMakerTags != null)
				{
					return d.thingSetMakerTags.Any((string x) => x.Contains("RewardStandard"));
				}
				return false;
			})
			orderby d.BaseMarketValue
			select d, list2.ToArray());
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000CBEB4 File Offset: 0x000CA0B4
		[CompilerGenerated]
		internal static float <Drugs>g__RealIngredientCost|6_0(ThingDef d)
		{
			return DebugOutputsEconomy.CostToMake(d, true);
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000CBEBD File Offset: 0x000CA0BD
		[CompilerGenerated]
		internal static float <Drugs>g__RealSellPrice|6_1(ThingDef d)
		{
			return d.BaseMarketValue * 0.6f;
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000CBECB File Offset: 0x000CA0CB
		[CompilerGenerated]
		internal static float <Drugs>g__RealBuyPrice|6_2(ThingDef d)
		{
			return d.BaseMarketValue * 1.4f;
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000CBED9 File Offset: 0x000CA0D9
		[CompilerGenerated]
		internal static bool <Drugs>g__IsDrug|6_3(ThingDef d)
		{
			return d.HasComp(typeof(CompDrug));
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000CBEEB File Offset: 0x000CA0EB
		[CompilerGenerated]
		internal static bool <Drugs>g__Addictive|6_4(ThingDef d)
		{
			return DebugOutputsEconomy.<Drugs>g__IsDrug|6_3(d) && DrugStatsUtility.GetDrugComp(d).Addictive;
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x000CBF02 File Offset: 0x000CA102
		[CompilerGenerated]
		internal static HediffDef <Drugs>g__Addiction|6_5(ThingDef d)
		{
			if (!DebugOutputsEconomy.<Drugs>g__Addictive|6_4(d))
			{
				return null;
			}
			return DrugStatsUtility.GetChemical(d).addictionHediff;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x000CBF19 File Offset: 0x000CA119
		[CompilerGenerated]
		internal static float <Drugs>g__NewAddictionChance|6_6(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|6_3(d))
			{
				return DrugStatsUtility.GetDrugComp(d).addictiveness;
			}
			return -1f;
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000CBF34 File Offset: 0x000CA134
		[CompilerGenerated]
		internal static float <Drugs>g__NewAddictionSeverity|6_7(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|6_3(d))
			{
				return DrugStatsUtility.GetChemical(d).addictionHediff.initialSeverity;
			}
			return -1f;
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x000CBF54 File Offset: 0x000CA154
		[CompilerGenerated]
		internal static float <Drugs>g__OldAddictionSeverityOffset|6_8(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|6_3(d))
			{
				return DrugStatsUtility.GetDrugComp(d).existingAddictionSeverityOffset;
			}
			return -1f;
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x000CBF6F File Offset: 0x000CA16F
		[CompilerGenerated]
		internal static FloatRange <Drugs>g__OverdoseSeverity|6_9(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|6_3(d))
			{
				return DrugStatsUtility.GetDrugComp(d).overdoseSeverityOffset;
			}
			return FloatRange.Zero;
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x000CBF8A File Offset: 0x000CA18A
		[CompilerGenerated]
		internal static float <Drugs>g__LargeOverdoseChance|6_10(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|6_3(d))
			{
				return DrugStatsUtility.GetDrugComp(d).largeOverdoseChance;
			}
			return -1f;
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x000CBFA5 File Offset: 0x000CA1A5
		[CompilerGenerated]
		internal static float <Drugs>g__MinToleranceToAddict|6_11(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|6_3(d))
			{
				return DrugStatsUtility.GetDrugComp(d).minToleranceToAddict;
			}
			return -1f;
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x000CBFC0 File Offset: 0x000CA1C0
		[CompilerGenerated]
		internal static float <AnimalEconomy>g__AdultMeatNutrition|12_0(ThingDef d)
		{
			return AnimalProductionUtility.AdultMeatAmount(d) * 0.05f;
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000CBFCE File Offset: 0x000CA1CE
		[CompilerGenerated]
		internal static float <AnimalEconomy>g__BabyMeatNutrition|12_1(ThingDef d)
		{
			return DebugOutputsEconomy.<AnimalEconomy>g__AdultMeatNutrition|12_0(d) * d.race.lifeStageAges[0].def.bodySizeFactor;
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000CBFF2 File Offset: 0x000CA1F2
		[CompilerGenerated]
		internal static float <AnimalEconomy>g__BabyMeatNutritionPerInputNutrition|12_2(ThingDef d)
		{
			return DebugOutputsEconomy.<AnimalEconomy>g__BabyMeatNutrition|12_1(d) / AnimalProductionUtility.NutritionToGestate(d);
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000CC001 File Offset: 0x000CA201
		[CompilerGenerated]
		internal static float <AnimalEconomy>g__AdultMeatNutritionPerInput|12_3(ThingDef d)
		{
			return DebugOutputsEconomy.<AnimalEconomy>g__AdultMeatNutrition|12_0(d) / AnimalProductionUtility.NutritionToAdulthood(d);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000CC010 File Offset: 0x000CA210
		[CompilerGenerated]
		internal static bool <AnimalEconomy>g__IsEggLayer|12_4(ThingDef d)
		{
			return d.HasComp(typeof(CompEggLayer));
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000CC022 File Offset: 0x000CA222
		[CompilerGenerated]
		internal static bool <AnimalEconomy>g__IsMilkable|12_5(ThingDef d)
		{
			return d.HasComp(typeof(CompMilkable));
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000CC034 File Offset: 0x000CA234
		[CompilerGenerated]
		internal static bool <AnimalEconomy>g__IsShearable|12_6(ThingDef d)
		{
			return d.HasComp(typeof(CompShearable));
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000CC046 File Offset: 0x000CA246
		[CompilerGenerated]
		internal static float <AnimalEconomy>g__EatenNutritionPerYear|12_7(ThingDef d)
		{
			return 2.6666667E-05f * d.race.baseHungerRate * 3600000f;
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x000CC05F File Offset: 0x000CA25F
		[CompilerGenerated]
		internal static float <AnimalEconomy>g__SlaughterValuePerInputNutrition|12_8(ThingDef d)
		{
			return DebugOutputsEconomy.SlaughterValue(d) / AnimalProductionUtility.NutritionToAdulthood(d);
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x000CC06E File Offset: 0x000CA26E
		[CompilerGenerated]
		internal static float <AnimalEconomy>g__TotalMarketValuePerNutritionEaten|12_9(ThingDef d)
		{
			return DebugOutputsEconomy.TotalMarketValueOutputPerYear(d) / DebugOutputsEconomy.<AnimalEconomy>g__EatenNutritionPerYear|12_7(d);
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x000CC07D File Offset: 0x000CA27D
		[CompilerGenerated]
		internal static float <Crops>g__workCost|16_0(ThingDef d)
		{
			return 1.1f + d.plant.growDays * 1f + (d.plant.sowWork + d.plant.harvestWork) * 0.00612f;
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x000CC0B4 File Offset: 0x000CA2B4
		[CompilerGenerated]
		internal static string <ItemAndBuildingAcquisition>g__recipes|17_0(ThingDef d)
		{
			List<string> list = new List<string>();
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
			{
				if (!recipeDef.products.NullOrEmpty<ThingDefCountClass>())
				{
					for (int i = 0; i < recipeDef.products.Count; i++)
					{
						if (recipeDef.products[i].thingDef == d)
						{
							list.Add(recipeDef.defName);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return "";
			}
			return list.ToCommaList(false, false);
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x000CC158 File Offset: 0x000CA358
		[CompilerGenerated]
		internal static string <ItemAndBuildingAcquisition>g__workAmountSources|17_1(ThingDef d)
		{
			List<string> list = new List<string>();
			if (d.StatBaseDefined(StatDefOf.WorkToMake))
			{
				list.Add("WorkToMake(" + d.GetStatValueAbstract(StatDefOf.WorkToMake, null) + ")");
			}
			if (d.StatBaseDefined(StatDefOf.WorkToBuild))
			{
				list.Add("WorkToBuild(" + d.GetStatValueAbstract(StatDefOf.WorkToBuild, null) + ")");
			}
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
			{
				if (recipeDef.workAmount > 0f && !recipeDef.products.NullOrEmpty<ThingDefCountClass>())
				{
					for (int i = 0; i < recipeDef.products.Count; i++)
					{
						if (recipeDef.products[i].thingDef == d)
						{
							list.Add(string.Concat(new object[]
							{
								"RecipeDef-",
								recipeDef.defName,
								"(",
								recipeDef.workAmount,
								")"
							}));
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return "";
			}
			return list.ToCommaList(false, false);
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000CC2AC File Offset: 0x000CA4AC
		[CompilerGenerated]
		internal static float <RoamingVsEconomy>g__RoamsPerYear|36_0(ThingDef d)
		{
			return 60f / d.race.roamMtbDays.Value;
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000CC2C4 File Offset: 0x000CA4C4
		[CompilerGenerated]
		internal static float <RoamingVsEconomy>g__MarketValuePerRoam|36_1(ThingDef d)
		{
			return DebugOutputsEconomy.TotalMarketValueOutputPerYear(d) / DebugOutputsEconomy.<RoamingVsEconomy>g__RoamsPerYear|36_0(d);
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000CC2D3 File Offset: 0x000CA4D3
		[CompilerGenerated]
		internal static float <RoamingVsEconomy>g__SlaughterValuePerRoam|36_2(ThingDef d)
		{
			return DebugOutputsEconomy.SlaughterValuePerGrowthYear(d) / DebugOutputsEconomy.<RoamingVsEconomy>g__RoamsPerYear|36_0(d);
		}
	}
}

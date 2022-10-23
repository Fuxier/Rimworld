using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000133 RID: 307
	public class RulePackDef : Def
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x000283AC File Offset: 0x000265AC
		public List<Rule> RulesPlusIncludes
		{
			get
			{
				if (this.cachedRules == null)
				{
					this.cachedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedRules.AddRange(this.rulePack.Rules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedRules.AddRange(this.include[i].RulesPlusIncludes);
						}
					}
				}
				return this.cachedRules;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0002842C File Offset: 0x0002662C
		public List<Rule> UntranslatedRulesPlusIncludes
		{
			get
			{
				if (this.cachedUntranslatedRules == null)
				{
					this.cachedUntranslatedRules = new List<Rule>();
					if (this.rulePack != null)
					{
						this.cachedUntranslatedRules.AddRange(this.rulePack.UntranslatedRules);
					}
					if (this.include != null)
					{
						for (int i = 0; i < this.include.Count; i++)
						{
							this.cachedUntranslatedRules.AddRange(this.include[i].UntranslatedRulesPlusIncludes);
						}
					}
				}
				return this.cachedUntranslatedRules;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x000284AA File Offset: 0x000266AA
		public List<Rule> RulesImmediate
		{
			get
			{
				if (this.rulePack == null)
				{
					return null;
				}
				return this.rulePack.Rules;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x000284C1 File Offset: 0x000266C1
		public List<Rule> UntranslatedRulesImmediate
		{
			get
			{
				if (this.rulePack == null)
				{
					return null;
				}
				return this.rulePack.UntranslatedRules;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x000284D8 File Offset: 0x000266D8
		public string FirstRuleKeyword
		{
			get
			{
				List<Rule> rulesPlusIncludes = this.RulesPlusIncludes;
				if (!rulesPlusIncludes.Any<Rule>())
				{
					return "none";
				}
				return rulesPlusIncludes[0].keyword;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00028508 File Offset: 0x00026708
		public string FirstUntranslatedRuleKeyword
		{
			get
			{
				List<Rule> untranslatedRulesPlusIncludes = this.UntranslatedRulesPlusIncludes;
				if (!untranslatedRulesPlusIncludes.Any<Rule>())
				{
					return "none";
				}
				return untranslatedRulesPlusIncludes[0].keyword;
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00028536 File Offset: 0x00026736
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.include != null)
			{
				int num;
				for (int i = 0; i < this.include.Count; i = num + 1)
				{
					if (this.include[i].include != null && this.include[i].include.Contains(this))
					{
						yield return "includes other RulePackDef which includes it: " + this.include[i].defName;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00028546 File Offset: 0x00026746
		public static RulePackDef Named(string defName)
		{
			return DefDatabase<RulePackDef>.GetNamed(defName, true);
		}

		// Token: 0x040007F9 RID: 2041
		public List<RulePackDef> include;

		// Token: 0x040007FA RID: 2042
		private RulePack rulePack;

		// Token: 0x040007FB RID: 2043
		public bool directTestable;

		// Token: 0x040007FC RID: 2044
		[Unsaved(false)]
		private List<Rule> cachedRules;

		// Token: 0x040007FD RID: 2045
		[Unsaved(false)]
		private List<Rule> cachedUntranslatedRules;
	}
}

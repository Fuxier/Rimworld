using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000532 RID: 1330
	public sealed class IngredientCount
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060028D4 RID: 10452 RVA: 0x00106C27 File Offset: 0x00104E27
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060028D5 RID: 10453 RVA: 0x00106C37 File Offset: 0x00104E37
		public ThingDef FixedIngredient
		{
			get
			{
				if (!this.IsFixedIngredient)
				{
					Log.Error("Called for SingleIngredient on an IngredientCount that is not IsSingleIngredient: " + this);
				}
				return this.filter.AnyAllowedDef;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060028D6 RID: 10454 RVA: 0x00106C5C File Offset: 0x00104E5C
		public string Summary
		{
			get
			{
				return this.count + "x " + this.filter.Summary;
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x00106C7E File Offset: 0x00104E7E
		public string SummaryFor(RecipeDef recipe)
		{
			return this.CountFor(recipe) + "x " + this.filter.Summary;
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x00106CA4 File Offset: 0x00104EA4
		public float CountFor(RecipeDef recipe)
		{
			float num = this.GetBaseCount();
			ThingDef thingDef = this.filter.AllowedThingDefs.FirstOrDefault((ThingDef x) => recipe.fixedIngredientFilter.Allows(x) && !x.smallVolume) ?? this.filter.AllowedThingDefs.FirstOrDefault((ThingDef x) => recipe.fixedIngredientFilter.Allows(x));
			if (thingDef != null)
			{
				float num2 = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
				if (Math.Abs(num2) > 1E-45f)
				{
					num /= num2;
				}
			}
			return num;
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x00106D2C File Offset: 0x00104F2C
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe, Bill bill = null)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(((bill == null) ? this.count : recipe.Worker.GetIngredientCount(this, bill)) / num);
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x00106D65 File Offset: 0x00104F65
		public float GetBaseCount()
		{
			return this.count;
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x00106D6D File Offset: 0x00104F6D
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x00106D76 File Offset: 0x00104F76
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x00106D83 File Offset: 0x00104F83
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}

		// Token: 0x04001AB1 RID: 6833
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04001AB2 RID: 6834
		private float count = 1f;
	}
}

using System;
using RimWorld;
using Verse;

// Token: 0x02000010 RID: 16
public class Thought_FoodEaten : Thought_Memory
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600005B RID: 91 RVA: 0x00004C50 File Offset: 0x00002E50
	public override string Description
	{
		get
		{
			return base.Description + "\n\n" + this.foodThoughtDescription;
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00004C68 File Offset: 0x00002E68
	public void SetFood(Thing food)
	{
		CompIngredients compIngredients = food.TryGetComp<CompIngredients>();
		this.foodThoughtDescription = "ThoughtFoodEatenFood".Translate() + ": " + food.def.LabelCap;
		if (compIngredients != null && compIngredients.ingredients.Count > 0)
		{
			bool flag;
			this.foodThoughtDescription += " (" + "ThoughtFoodEatenIngredients".Translate() + ": " + compIngredients.GetIngredientsString(false, out flag) + ")";
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00004D08 File Offset: 0x00002F08
	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look<string>(ref this.foodThoughtDescription, "foodThoughtDescription", null, false);
	}

	// Token: 0x0400001F RID: 31
	private string foodThoughtDescription;
}

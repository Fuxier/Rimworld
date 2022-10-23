using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200040B RID: 1035
	public class UnfinishedThing : ThingWithComps
	{
		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001E6A RID: 7786 RVA: 0x000B6785 File Offset: 0x000B4985
		// (set) Token: 0x06001E6B RID: 7787 RVA: 0x000B678D File Offset: 0x000B498D
		public Pawn Creator
		{
			get
			{
				return this.creatorInt;
			}
			set
			{
				if (value == null)
				{
					Log.Error("Cannot set creator to null.");
					return;
				}
				this.creatorInt = value;
				this.creatorName = value.LabelShort;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x000B67B0 File Offset: 0x000B49B0
		public RecipeDef Recipe
		{
			get
			{
				return this.recipeInt;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x000B67B8 File Offset: 0x000B49B8
		// (set) Token: 0x06001E6E RID: 7790 RVA: 0x000B67EC File Offset: 0x000B49EC
		public Bill_ProductionWithUft BoundBill
		{
			get
			{
				if (this.boundBillInt != null && (this.boundBillInt.DeletedOrDereferenced || this.boundBillInt.BoundUft != this))
				{
					this.boundBillInt = null;
				}
				return this.boundBillInt;
			}
			set
			{
				if (value == this.boundBillInt)
				{
					return;
				}
				Bill_ProductionWithUft bill_ProductionWithUft = this.boundBillInt;
				this.boundBillInt = value;
				if (bill_ProductionWithUft != null && bill_ProductionWithUft.BoundUft == this)
				{
					bill_ProductionWithUft.SetBoundUft(null, false);
				}
				if (value != null)
				{
					this.recipeInt = value.recipe;
					if (value.BoundUft != this)
					{
						value.SetBoundUft(this, false);
					}
				}
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x000B6848 File Offset: 0x000B4A48
		public Thing BoundWorkTable
		{
			get
			{
				if (this.BoundBill == null)
				{
					return null;
				}
				Thing thing = this.BoundBill.billStack.billGiver as Thing;
				if (thing.Destroyed)
				{
					return null;
				}
				return thing;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x000B6880 File Offset: 0x000B4A80
		public override string LabelNoCount
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				if (base.Stuff == null)
				{
					return "UnfinishedItem".Translate(this.Recipe.products[0].thingDef.label);
				}
				return "UnfinishedItemWithStuff".Translate(base.Stuff.LabelAsStuff, this.Recipe.products[0].thingDef.label);
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x000B6913 File Offset: 0x000B4B13
		public override string DescriptionDetailed
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				return this.Recipe.ProducedThingDef.DescriptionDetailed;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x000B6934 File Offset: 0x000B4B34
		public override string DescriptionFlavor
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				return this.Recipe.ProducedThingDef.description;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001E73 RID: 7795 RVA: 0x000B6955 File Offset: 0x000B4B55
		public bool Initialized
		{
			get
			{
				return this.workLeft > -5000f;
			}
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x000B6964 File Offset: 0x000B4B64
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && this.boundBillInt != null && this.boundBillInt.DeletedOrDereferenced)
			{
				this.boundBillInt = null;
			}
			Scribe_References.Look<Pawn>(ref this.creatorInt, "creator", false);
			Scribe_Values.Look<string>(ref this.creatorName, "creatorName", null, false);
			Scribe_References.Look<Bill_ProductionWithUft>(ref this.boundBillInt, "bill", false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipeInt, "recipe");
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.ingredients, "ingredients", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x000B6A0C File Offset: 0x000B4C0C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode == DestroyMode.Cancel)
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					int num = GenMath.RoundRandom((float)this.ingredients[i].stackCount * 0.75f);
					if (num > 0)
					{
						this.ingredients[i].stackCount = num;
						GenPlace.TryPlaceThing(this.ingredients[i], base.Position, base.Map, ThingPlaceMode.Near, null, null, default(Rot4));
					}
				}
				this.ingredients.Clear();
			}
			base.Destroy(mode);
			this.BoundBill = null;
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x000B6AAA File Offset: 0x000B4CAA
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Action
			{
				defaultLabel = "CommandCancelConstructionLabel".Translate(),
				defaultDesc = "CommandCancelConstructionDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true),
				hotKey = KeyBindingDefOf.Designator_Cancel,
				action = delegate()
				{
					this.Destroy(DestroyMode.Cancel);
				}
			};
			if (this.Initialized && DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Complete",
					action = delegate()
					{
						this.workLeft = 0f;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000B6ABC File Offset: 0x000B4CBC
		public Bill_ProductionWithUft BillOnTableForMe(Thing workTable)
		{
			if (this.Recipe.AllRecipeUsers.Contains(workTable.def))
			{
				IBillGiver billGiver = (IBillGiver)workTable;
				for (int i = 0; i < billGiver.BillStack.Count; i++)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = billGiver.BillStack[i] as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null && bill_ProductionWithUft.ShouldDoNow() && bill_ProductionWithUft != null && bill_ProductionWithUft.recipe == this.Recipe)
					{
						return bill_ProductionWithUft;
					}
				}
			}
			return null;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000B6B2F File Offset: 0x000B4D2F
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.BoundWorkTable != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.BoundWorkTable.TrueCenter());
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000B6B58 File Offset: 0x000B4D58
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "Author".Translate() + ": " + this.creatorName;
			text += "\n" + "WorkLeft".Translate() + ": " + this.workLeft.ToStringWorkAmount();
			Bill_ProductionWithUft boundBill = this.BoundBill;
			bool flag;
			if (boundBill == null)
			{
				flag = (null != null);
			}
			else
			{
				ThingStyleDef style = boundBill.style;
				flag = (((style != null) ? style.Category : null) != null);
			}
			if (flag && base.StyleSourcePrecept == null)
			{
				text += "\n" + "Style".Translate() + ": " + this.BoundBill.style.Category.LabelCap;
			}
			return text;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000B6C52 File Offset: 0x000B4E52
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			Bill_ProductionWithUft boundBill = this.BoundBill;
			bool flag;
			if (boundBill == null)
			{
				flag = (null != null);
			}
			else
			{
				ThingStyleDef style = boundBill.style;
				flag = (((style != null) ? style.Category : null) != null);
			}
			if (flag && base.StyleSourcePrecept == null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_StyleLabel".Translate(), this.BoundBill.style.Category.LabelCap, "Stat_Thing_StyleDesc".Translate(), 1108, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x040014D7 RID: 5335
		private Pawn creatorInt;

		// Token: 0x040014D8 RID: 5336
		private string creatorName = "ErrorCreatorName";

		// Token: 0x040014D9 RID: 5337
		private RecipeDef recipeInt;

		// Token: 0x040014DA RID: 5338
		public List<Thing> ingredients = new List<Thing>();

		// Token: 0x040014DB RID: 5339
		private Bill_ProductionWithUft boundBillInt;

		// Token: 0x040014DC RID: 5340
		public float workLeft = -10000f;

		// Token: 0x040014DD RID: 5341
		private const float CancelIngredientRecoveryFraction = 0.75f;
	}
}

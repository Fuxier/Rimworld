using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200042B RID: 1067
	public class ReverseDesignatorDatabase
	{
		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001F82 RID: 8066 RVA: 0x000BB4F6 File Offset: 0x000B96F6
		public List<Designator> AllDesignators
		{
			get
			{
				if (this.desList == null)
				{
					this.InitDesignators();
				}
				return this.desList;
			}
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x000BB50C File Offset: 0x000B970C
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x000BB518 File Offset: 0x000B9718
		public T Get<T>() where T : Designator
		{
			if (this.desList == null)
			{
				this.InitDesignators();
			}
			for (int i = 0; i < this.desList.Count; i++)
			{
				T t = this.desList[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x000BB574 File Offset: 0x000B9774
		private void InitDesignators()
		{
			this.desList = new List<Designator>();
			this.desList.Add(new Designator_Cancel());
			this.desList.Add(new Designator_Claim());
			this.desList.Add(new Designator_Deconstruct());
			this.desList.Add(new Designator_Uninstall());
			this.desList.Add(new Designator_Haul());
			this.desList.Add(new Designator_Hunt());
			this.desList.Add(new Designator_Slaughter());
			this.desList.Add(new Designator_Tame());
			this.desList.Add(new Designator_PlantsCut());
			this.desList.Add(new Designator_PlantsHarvest());
			this.desList.Add(new Designator_PlantsHarvestWood());
			this.desList.Add(new Designator_Mine());
			this.desList.Add(new Designator_Strip());
			this.desList.Add(new Designator_Open());
			this.desList.Add(new Designator_SmoothSurface());
			this.desList.Add(new Designator_ReleaseAnimalToWild());
			this.desList.Add(new Designator_ExtractTree());
			this.desList.Add(new Designator_Study());
			this.desList.Add(new Designator_RemovePaint());
			if (ModsConfig.BiotechActive)
			{
				this.desList.Add(new Designator_MechControlGroup());
				this.desList.Add(new Designator_Adopt());
			}
			if (ModsConfig.IdeologyActive)
			{
				this.desList.Add(new Designator_ExtractSkull());
			}
			this.desList.RemoveAll((Designator des) => !Current.Game.Rules.DesignatorAllowed(des));
		}

		// Token: 0x0400155D RID: 5469
		private List<Designator> desList;
	}
}

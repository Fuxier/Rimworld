using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000417 RID: 1047
	public class CompEquippable : ThingComp, IVerbOwner
	{
		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001EB2 RID: 7858 RVA: 0x000B75F4 File Offset: 0x000B57F4
		private Pawn Holder
		{
			get
			{
				return this.PrimaryVerb.CasterPawn;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x000B7601 File Offset: 0x000B5801
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001EB4 RID: 7860 RVA: 0x000B760E File Offset: 0x000B580E
		public Verb PrimaryVerb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x000B761B File Offset: 0x000B581B
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x000B7623 File Offset: 0x000B5823
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000B7635 File Offset: 0x000B5835
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x000029B0 File Offset: 0x00000BB0
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000B7647 File Offset: 0x000B5847
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Weapon;
			}
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x000B764E File Offset: 0x000B584E
		public CompEquippable()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x000B7662 File Offset: 0x000B5862
		public IEnumerable<Command> GetVerbsCommands()
		{
			return this.verbTracker.GetVerbsCommands(KeyCode.None);
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x000B7670 File Offset: 0x000B5870
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Holder != null && this.Holder.equipment != null && this.Holder.equipment.Primary == this.parent)
			{
				this.Holder.equipment.Notify_PrimaryDestroyed();
			}
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x000B76C2 File Offset: 0x000B58C2
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x000B76E4 File Offset: 0x000B58E4
		public override void CompTick()
		{
			base.CompTick();
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x000B76F8 File Offset: 0x000B58F8
		public override void Notify_Unequipped(Pawn p)
		{
			List<Verb> allVerbs = this.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				allVerbs[i].Notify_EquipmentLost();
			}
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000B7729 File Offset: 0x000B5929
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "CompEquippable_" + this.parent.ThingID;
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000B7740 File Offset: 0x000B5940
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			Apparel apparel = this.parent as Apparel;
			if (apparel != null)
			{
				return p.apparel.WornApparel.Contains(apparel);
			}
			return p.equipment.AllEquipmentListForReading.Contains(this.parent);
		}

		// Token: 0x040014ED RID: 5357
		public VerbTracker verbTracker;
	}
}

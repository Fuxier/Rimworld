using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000497 RID: 1175
	[StaticConstructorOnStartup]
	public class Command_SetBedOwnerType : Command
	{
		// Token: 0x06002381 RID: 9089 RVA: 0x000E3410 File Offset: 0x000E1610
		public Command_SetBedOwnerType(Building_Bed bed)
		{
			this.bed = bed;
			switch (bed.ForOwnerType)
			{
			case BedOwnerType.Colonist:
				this.defaultLabel = "CommandBedSetForColonistsLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForColonistsTex;
				break;
			case BedOwnerType.Prisoner:
				this.defaultLabel = "CommandBedSetForPrisonersLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForPrisonersTex;
				break;
			case BedOwnerType.Slave:
				this.defaultLabel = "CommandBedSetForSlavesLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForSlavesTex;
				break;
			default:
				Log.Error(string.Format("Unknown owner type selected for bed: {0}", bed.ForOwnerType));
				break;
			}
			this.defaultDesc = "CommandBedSetForOwnerTypeDesc".Translate();
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x000E34DC File Offset: 0x000E16DC
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("CommandBedSetForColonistsLabel".Translate(), delegate()
			{
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Colonist);
			}, Command_SetBedOwnerType.ForColonistsTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			list.Add(new FloatMenuOption("CommandBedSetForPrisonersLabel".Translate(), delegate()
			{
				if (!Building_Bed.RoomCanBePrisonCell(this.bed.GetRoom(RegionType.Set_All)) && !this.bed.ForPrisoners)
				{
					Messages.Message("CommandBedSetForPrisonersFailOutdoors".Translate(), this.bed, MessageTypeDefOf.RejectInput, false);
					return;
				}
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Prisoner);
			}, Command_SetBedOwnerType.ForPrisonersTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			list.Add(new FloatMenuOption("CommandBedSetForSlavesLabel".Translate(), delegate()
			{
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Slave);
			}, Command_SetBedOwnerType.ForSlavesTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x040016D6 RID: 5846
		private Building_Bed bed;

		// Token: 0x040016D7 RID: 5847
		private static readonly Texture2D ForColonistsTex = ContentFinder<Texture2D>.Get("UI/Commands/ForColonists", true);

		// Token: 0x040016D8 RID: 5848
		private static readonly Texture2D ForSlavesTex = ContentFinder<Texture2D>.Get("UI/Commands/ForSlaves", true);

		// Token: 0x040016D9 RID: 5849
		private static readonly Texture2D ForPrisonersTex = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true);
	}
}

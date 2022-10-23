using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000251 RID: 593
	public class RoofCollapseBufferResolver
	{
		// Token: 0x060010FB RID: 4347 RVA: 0x0006308A File Offset: 0x0006128A
		public RoofCollapseBufferResolver(Map map)
		{
			this.map = map;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x000630B0 File Offset: 0x000612B0
		public void CollapseRoofsMarkedToCollapse()
		{
			RoofCollapseBuffer roofCollapseBuffer = this.map.roofCollapseBuffer;
			if (roofCollapseBuffer.CellsMarkedToCollapse.Any<IntVec3>())
			{
				this.tmpCrushedThings.Clear();
				RoofCollapserImmediate.DropRoofInCells(roofCollapseBuffer.CellsMarkedToCollapse, this.map, this.tmpCrushedThings);
				if (this.tmpCrushedThings.Any<Thing>())
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("RoofCollapsed".Translate());
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("TheseThingsCrushed".Translate());
					this.tmpCrushedNames.Clear();
					for (int i = 0; i < this.tmpCrushedThings.Count; i++)
					{
						Thing thing = this.tmpCrushedThings[i];
						Corpse corpse;
						if ((corpse = (thing as Corpse)) == null || !corpse.Bugged)
						{
							string item = thing.LabelShortCap;
							if (thing.def.category == ThingCategory.Pawn)
							{
								item = thing.LabelCap;
							}
							if (!this.tmpCrushedNames.Contains(item))
							{
								this.tmpCrushedNames.Add(item);
							}
						}
					}
					foreach (string str in this.tmpCrushedNames)
					{
						stringBuilder.AppendLine("    -" + str);
					}
					Find.LetterStack.ReceiveLetter("LetterLabelRoofCollapsed".Translate(), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.NegativeEvent, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), null, null, null, null);
				}
				else
				{
					Messages.Message("RoofCollapsed".Translate(), new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), MessageTypeDefOf.SilentInput, true);
				}
				this.tmpCrushedThings.Clear();
				roofCollapseBuffer.Clear();
			}
		}

		// Token: 0x04000EB5 RID: 3765
		private Map map;

		// Token: 0x04000EB6 RID: 3766
		private List<Thing> tmpCrushedThings = new List<Thing>();

		// Token: 0x04000EB7 RID: 3767
		private HashSet<string> tmpCrushedNames = new HashSet<string>();
	}
}

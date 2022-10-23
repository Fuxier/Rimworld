using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DF RID: 991
	public class Graphic_LinkedAsymmetric : Graphic_Linked
	{
		// Token: 0x06001C3C RID: 7228 RVA: 0x000AC790 File Offset: 0x000AA990
		public Graphic_LinkedAsymmetric(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001C3D RID: 7229 RVA: 0x00015E8B File Offset: 0x0001408B
		public override LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Asymmetric;
			}
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000AC799 File Offset: 0x000AA999
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_LinkedAsymmetric(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000AC7BC File Offset: 0x000AA9BC
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Graphic_LinkedAsymmetric.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.thing = thing;
			CS$<>8__locals1.layer = layer;
			CS$<>8__locals1.extraRotation = extraRotation;
			base.Print(CS$<>8__locals1.layer, CS$<>8__locals1.thing, CS$<>8__locals1.extraRotation);
			if (CS$<>8__locals1.thing.def.graphicData.asymmetricLink == null || !CS$<>8__locals1.thing.def.graphicData.asymmetricLink.linkToDoors)
			{
				return;
			}
			CS$<>8__locals1.cell = CS$<>8__locals1.thing.Position;
			CS$<>8__locals1.map = CS$<>8__locals1.thing.Map;
			if (CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderEast != null)
			{
				Graphic_LinkedAsymmetric.<Print>g__DrawBorder|4_0(IntVec3.East, CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderEast, ref CS$<>8__locals1);
			}
			if (CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderWest != null)
			{
				Graphic_LinkedAsymmetric.<Print>g__DrawBorder|4_0(IntVec3.West, CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderWest, ref CS$<>8__locals1);
			}
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000AC8D8 File Offset: 0x000AAAD8
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			if (base.ShouldLinkWith(c, parent))
			{
				return true;
			}
			if (parent.def.graphicData.asymmetricLink != null)
			{
				if ((parent.Map.linkGrid.LinkFlagsAt(c) & parent.def.graphicData.asymmetricLink.linkFlags) != LinkFlags.None)
				{
					return true;
				}
				if (parent.def.graphicData.asymmetricLink.linkToDoors && c.GetDoor(parent.Map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x000AC958 File Offset: 0x000AAB58
		[CompilerGenerated]
		internal static void <Print>g__DrawBorder|4_0(IntVec3 dir, AsymmetricLinkData.BorderData border, ref Graphic_LinkedAsymmetric.<>c__DisplayClass4_0 A_2)
		{
			IntVec3 c = A_2.cell + dir;
			if (c.InBounds(A_2.map) && c.GetDoor(A_2.map) != null)
			{
				Vector3 center = A_2.thing.DrawPos + border.offset + Altitudes.AltIncVect;
				Printer_Plane.PrintPlane(A_2.layer, center, border.size, border.Mat, A_2.extraRotation, false, null, null, 0.01f, 0f);
			}
		}
	}
}

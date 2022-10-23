using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DE RID: 990
	public class Graphic_Linked : Graphic
	{
		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001C33 RID: 7219 RVA: 0x00002662 File Offset: 0x00000862
		public virtual LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Basic;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x000AC613 File Offset: 0x000AA813
		public override Material MatSingle
		{
			get
			{
				return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingle, LinkDirections.None);
			}
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000AB849 File Offset: 0x000A9A49
		public Graphic_Linked()
		{
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000AC626 File Offset: 0x000AA826
		public Graphic_Linked(Graphic subGraphic)
		{
			this.subGraphic = subGraphic;
			this.data = subGraphic.data;
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000AC641 File Offset: 0x000AA841
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_Linked(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000AC664 File Offset: 0x000AA864
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Material mat = this.LinkedDrawMatFrom(thing, thing.Position);
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), mat, extraRotation, false, null, null, 0.01f, 0f);
			if (base.ShadowGraphic != null && thing != null)
			{
				base.ShadowGraphic.Print(layer, thing, 0f);
			}
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x000AC6C6 File Offset: 0x000AA8C6
		public override Material MatSingleFor(Thing thing)
		{
			return this.LinkedDrawMatFrom(thing, thing.Position);
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000AC6D8 File Offset: 0x000AA8D8
		protected virtual Material LinkedDrawMatFrom(Thing parent, IntVec3 cell)
		{
			int num = 0;
			int num2 = 1;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (this.ShouldLinkWith(c, parent))
				{
					num += num2;
				}
				num2 *= 2;
			}
			LinkDirections linkSet = (LinkDirections)num;
			return MaterialAtlasPool.SubMaterialFromAtlas(this.subGraphic.MatSingleFor(parent), linkSet);
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x000AC730 File Offset: 0x000AA930
		public virtual bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (!c.InBounds(parent.Map))
			{
				return (parent.def.graphicData.linkFlags & LinkFlags.MapEdge) > LinkFlags.None;
			}
			return (parent.Map.linkGrid.LinkFlagsAt(c) & parent.def.graphicData.linkFlags) > LinkFlags.None;
		}

		// Token: 0x04001438 RID: 5176
		protected Graphic subGraphic;
	}
}

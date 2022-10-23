using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E9 RID: 1001
	public class Graphic_RandomRotated : Graphic
	{
		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x000ADD9F File Offset: 0x000ABF9F
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001C83 RID: 7299 RVA: 0x000ADDAC File Offset: 0x000ABFAC
		public Graphic SubGraphic
		{
			get
			{
				return this.subGraphic;
			}
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x000ADDB4 File Offset: 0x000ABFB4
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
			this.drawSize = subGraphic.drawSize;
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x000ADDD8 File Offset: 0x000ABFD8
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			float num = 0f;
			float? rotInRack = this.GetRotInRack(thing, thingDef, loc.ToIntVec3());
			if (rotInRack != null)
			{
				num = rotInRack.Value;
			}
			else if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			Material material = this.subGraphic.MatSingleFor(thing);
			Graphics.DrawMesh(mesh, loc, Quaternion.AngleAxis(num, Vector3.up), material, 0, null, 0);
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x000ADE65 File Offset: 0x000AC065
		public override string ToString()
		{
			return "RandomRotated(subGraphic=" + this.subGraphic.ToString() + ")";
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x000ADE81 File Offset: 0x000AC081
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data,
				drawSize = this.drawSize
			};
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000ADEB4 File Offset: 0x000AC0B4
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			float num = 0f;
			float? rotInRack = this.GetRotInRack(thing, thing.def, thing.Position);
			if (rotInRack != null)
			{
				num = rotInRack.Value;
			}
			else if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			this.subGraphic.Print(layer, thing, num);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000ADF26 File Offset: 0x000AC126
		public override void TryInsertIntoAtlas(TextureAtlasGroup group)
		{
			this.subGraphic.TryInsertIntoAtlas(group);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x000ADF34 File Offset: 0x000AC134
		private float? GetRotInRack(Thing thing, ThingDef thingDef, IntVec3 loc)
		{
			if (thing == null || !thingDef.IsWeapon || !thing.Spawned || !loc.InBounds(thing.Map) || loc.GetEdifice(thing.Map) == null || loc.GetItemCount(thing.Map) < 2)
			{
				return null;
			}
			if (thingDef.rotateInShelves)
			{
				return new float?(-90f);
			}
			return new float?(0f);
		}

		// Token: 0x0400144E RID: 5198
		private Graphic subGraphic;

		// Token: 0x0400144F RID: 5199
		private float maxAngle;
	}
}

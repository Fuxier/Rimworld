using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EC RID: 1004
	public class Graphic_StackCount : Graphic_Collection
	{
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x000AE2E9 File Offset: 0x000AC4E9
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[this.subGraphics.Length - 1].MatSingle;
			}
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000AE301 File Offset: 0x000AC501
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_StackCount>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x000AC5D0 File Offset: 0x000AA7D0
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x000AE31E File Offset: 0x000AC51E
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x000AE336 File Offset: 0x000AC536
		public virtual Graphic SubGraphicFor(Thing thing)
		{
			return this.SubGraphicForStackCount(thing.stackCount, thing.def);
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000AE34C File Offset: 0x000AC54C
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic graphic;
			if (thing != null)
			{
				graphic = this.SubGraphicFor(thing);
			}
			else
			{
				graphic = this.subGraphics[0];
			}
			graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x000AE380 File Offset: 0x000AC580
		public Graphic SubGraphicForStackCount(int stackCount, ThingDef def)
		{
			switch (this.subGraphics.Length)
			{
			case 1:
				return this.subGraphics[0];
			case 2:
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				return this.subGraphics[1];
			case 3:
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				if (stackCount == def.stackLimit)
				{
					return this.subGraphics[2];
				}
				return this.subGraphics[1];
			default:
			{
				if (stackCount == 1)
				{
					return this.subGraphics[0];
				}
				if (stackCount == def.stackLimit)
				{
					return this.subGraphics[this.subGraphics.Length - 1];
				}
				int num = Mathf.Min(1 + Mathf.RoundToInt((float)stackCount / (float)def.stackLimit * ((float)this.subGraphics.Length - 3f) + 1E-05f), this.subGraphics.Length - 2);
				return this.subGraphics[num];
			}
			}
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x000AE45A File Offset: 0x000AC65A
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"StackCount(path=",
				this.path,
				", count=",
				this.subGraphics.Length,
				")"
			});
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D3 RID: 979
	public class Graphic_Appearances : Graphic
	{
		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001C04 RID: 7172 RVA: 0x000AB589 File Offset: 0x000A9789
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index].MatSingle;
			}
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000AB5A4 File Offset: 0x000A97A4
		private ThingDef StuffOfThing(Thing thing)
		{
			IConstructible constructible;
			if ((constructible = (thing as IConstructible)) != null)
			{
				return constructible.EntityToBuildStuff();
			}
			return thing.Stuff;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000AB5C8 File Offset: 0x000A97C8
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.SubGraphicFor(thing).MatAt(rot, thing);
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000AB5D8 File Offset: 0x000A97D8
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.drawSize = req.drawSize;
			List<StuffAppearanceDef> allDefsListForReading = DefDatabase<StuffAppearanceDef>.AllDefsListForReading;
			this.subGraphics = new Graphic[allDefsListForReading.Count];
			for (int i = 0; i < this.subGraphics.Length; i++)
			{
				StuffAppearanceDef stuffAppearance = allDefsListForReading[i];
				string text = req.path;
				if (!stuffAppearance.pathPrefix.NullOrEmpty())
				{
					text = stuffAppearance.pathPrefix + "/" + text.Split(new char[]
					{
						'/'
					}).Last<string>();
				}
				Texture2D texture2D = (from x in ContentFinder<Texture2D>.GetAllInFolder(text)
				where x.name.EndsWith(stuffAppearance.defName)
				select x).FirstOrDefault<Texture2D>();
				if (texture2D != null)
				{
					this.subGraphics[i] = GraphicDatabase.Get<Graphic_Single>(text + "/" + texture2D.name, req.shader, this.drawSize, this.color);
				}
			}
			for (int j = 0; j < this.subGraphics.Length; j++)
			{
				if (this.subGraphics[j] == null)
				{
					this.subGraphics[j] = this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index];
				}
			}
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x000AB732 File Offset: 0x000A9932
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Appearances.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Appearances>(this.path, newShader, this.drawSize, newColor, Color.white, this.data, null);
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x000AB76F File Offset: 0x000A996F
		public override Material MatSingleFor(Thing thing)
		{
			return this.SubGraphicFor(thing).MatSingleFor(thing);
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x000AB77E File Offset: 0x000A997E
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.SubGraphicFor(thing).DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x000AB794 File Offset: 0x000A9994
		public Graphic SubGraphicFor(Thing thing)
		{
			StuffAppearanceDef smooth = StuffAppearanceDefOf.Smooth;
			if (thing != null)
			{
				return this.SubGraphicFor(this.StuffOfThing(thing));
			}
			return this.subGraphics[(int)smooth.index];
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x000AB7C8 File Offset: 0x000A99C8
		public Graphic SubGraphicFor(ThingDef stuff)
		{
			StuffAppearanceDef app = StuffAppearanceDefOf.Smooth;
			if (stuff != null && stuff.stuffProps.appearance != null)
			{
				app = stuff.stuffProps.appearance;
			}
			return this.SubGraphicFor(app);
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x000AB7FE File Offset: 0x000A99FE
		public Graphic SubGraphicFor(StuffAppearanceDef app)
		{
			return this.subGraphics[(int)app.index];
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000AB80D File Offset: 0x000A9A0D
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Appearance(path=",
				this.path,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}

		// Token: 0x04001421 RID: 5153
		protected Graphic[] subGraphics;
	}
}

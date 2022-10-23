using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D5 RID: 981
	public abstract class Graphic_Collection : Graphic
	{
		// Token: 0x06001C15 RID: 7189 RVA: 0x000ABA60 File Offset: 0x000A9C60
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			Graphic[] array = this.subGraphics;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].TryInsertIntoAtlas(groupKey);
			}
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x000ABA8C File Offset: 0x000A9C8C
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			if (req.path.NullOrEmpty())
			{
				throw new ArgumentNullException("folderPath");
			}
			if (req.shader == null)
			{
				throw new ArgumentNullException("shader");
			}
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			List<ValueTuple<Texture2D, string>> list = (from x in ContentFinder<Texture2D>.GetAllInFolder(req.path)
			where !x.name.EndsWith(Graphic_Single.MaskSuffix)
			orderby x.name
			select new ValueTuple<Texture2D, string>(x, x.name.Split(new char[]
			{
				'_'
			})[0])).ToList<ValueTuple<Texture2D, string>>();
			if (list.NullOrEmpty<ValueTuple<Texture2D, string>>())
			{
				Log.Error("Collection cannot init: No textures found at path " + req.path);
				this.subGraphics = new Graphic[]
				{
					BaseContent.BadGraphic
				};
				return;
			}
			List<Graphic> list2 = new List<Graphic>();
			foreach (IGrouping<string, ValueTuple<Texture2D, string>> grouping in from s in list
			group s by s.Item2)
			{
				List<ValueTuple<Texture2D, string>> list3 = grouping.ToList<ValueTuple<Texture2D, string>>();
				string path = req.path + "/" + grouping.Key;
				bool flag = false;
				for (int i = list3.Count - 1; i >= 0; i--)
				{
					if (list3[i].Item1.name.Contains("_east") || list3[i].Item1.name.Contains("_north") || list3[i].Item1.name.Contains("_west") || list3[i].Item1.name.Contains("_south"))
					{
						list3.RemoveAt(i);
						flag = true;
					}
				}
				if (list3.Count > 0)
				{
					foreach (ValueTuple<Texture2D, string> valueTuple in list3)
					{
						list2.Add(GraphicDatabase.Get(typeof(Graphic_Single), req.path + "/" + valueTuple.Item1.name, req.shader, this.drawSize, this.color, this.colorTwo, this.data, req.shaderParameters, null));
					}
				}
				if (flag)
				{
					list2.Add(GraphicDatabase.Get(typeof(Graphic_Multi), path, req.shader, this.drawSize, this.color, this.colorTwo, this.data, req.shaderParameters, null));
				}
			}
			this.subGraphics = list2.ToArray();
		}

		// Token: 0x04001426 RID: 5158
		protected Graphic[] subGraphics;
	}
}

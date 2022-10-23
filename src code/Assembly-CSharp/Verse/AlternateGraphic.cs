using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200011F RID: 287
	public class AlternateGraphic
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x00026602 File Offset: 0x00024802
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0002660C File Offset: 0x0002480C
		public Graphic GetGraphic(Graphic other)
		{
			if (this.graphicData == null)
			{
				this.graphicData = new GraphicData();
			}
			this.graphicData.CopyFrom(other.data);
			if (!this.texPath.NullOrEmpty())
			{
				this.graphicData.texPath = this.texPath;
			}
			this.graphicData.color = (this.color ?? other.color);
			this.graphicData.colorTwo = (this.colorTwo ?? other.colorTwo);
			return this.graphicData.Graphic;
		}

		// Token: 0x04000769 RID: 1897
		private float weight = 0.5f;

		// Token: 0x0400076A RID: 1898
		private string texPath;

		// Token: 0x0400076B RID: 1899
		private Color? color;

		// Token: 0x0400076C RID: 1900
		private Color? colorTwo;

		// Token: 0x0400076D RID: 1901
		private GraphicData graphicData;
	}
}

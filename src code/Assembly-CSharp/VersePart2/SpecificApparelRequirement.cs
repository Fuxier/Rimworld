using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200011D RID: 285
	public class SpecificApparelRequirement
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00026535 File Offset: 0x00024735
		public BodyPartGroupDef BodyPartGroup
		{
			get
			{
				return this.bodyPartGroup;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x0002653D File Offset: 0x0002473D
		public ApparelLayerDef ApparelLayer
		{
			get
			{
				return this.apparelLayer;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x00026545 File Offset: 0x00024745
		public ThingDef ApparelDef
		{
			get
			{
				return this.apparelDef;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x0002654D File Offset: 0x0002474D
		public string RequiredTag
		{
			get
			{
				return this.requiredTag;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00026555 File Offset: 0x00024755
		public List<SpecificApparelRequirement.TagChance> AlternateTagChoices
		{
			get
			{
				return this.alternateTagChoices;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x0002655D File Offset: 0x0002475D
		public ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00026565 File Offset: 0x00024765
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0002656D File Offset: 0x0002476D
		public ColorGenerator ColorGenerator
		{
			get
			{
				return this.ColorGenerator;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00026575 File Offset: 0x00024775
		public bool Locked
		{
			get
			{
				return this.locked;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x0002657D File Offset: 0x0002477D
		public bool Biocode
		{
			get
			{
				return this.biocode;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00026585 File Offset: 0x00024785
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.styleDef;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x0002658D File Offset: 0x0002478D
		public QualityCategory? Quality
		{
			get
			{
				return this.quality;
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00026598 File Offset: 0x00024798
		public Color GetColor()
		{
			if (this.color != default(Color))
			{
				return this.color;
			}
			if (this.colorGenerator != null)
			{
				return this.colorGenerator.NewRandomizedColor();
			}
			return default(Color);
		}

		// Token: 0x0400075B RID: 1883
		private BodyPartGroupDef bodyPartGroup;

		// Token: 0x0400075C RID: 1884
		private ApparelLayerDef apparelLayer;

		// Token: 0x0400075D RID: 1885
		private ThingDef apparelDef;

		// Token: 0x0400075E RID: 1886
		private string requiredTag;

		// Token: 0x0400075F RID: 1887
		private List<SpecificApparelRequirement.TagChance> alternateTagChoices;

		// Token: 0x04000760 RID: 1888
		private ThingDef stuff;

		// Token: 0x04000761 RID: 1889
		private ThingStyleDef styleDef;

		// Token: 0x04000762 RID: 1890
		private Color color;

		// Token: 0x04000763 RID: 1891
		private ColorGenerator colorGenerator;

		// Token: 0x04000764 RID: 1892
		private bool locked;

		// Token: 0x04000765 RID: 1893
		private bool biocode;

		// Token: 0x04000766 RID: 1894
		private QualityCategory? quality;

		// Token: 0x02001CE0 RID: 7392
		public struct TagChance
		{
			// Token: 0x040071F3 RID: 29171
			public string tag;

			// Token: 0x040071F4 RID: 29172
			public float chance;
		}
	}
}

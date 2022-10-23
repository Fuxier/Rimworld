using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000045 RID: 69
	[StaticConstructorOnStartup]
	public static class BaseContent
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x000143E8 File Offset: 0x000125E8
		public static bool NullOrBad(this Material mat)
		{
			return mat == null || mat == BaseContent.BadMat;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00014400 File Offset: 0x00012600
		public static bool NullOrBad(this Texture2D tex)
		{
			return tex == null || tex == BaseContent.BadTex;
		}

		// Token: 0x040000E5 RID: 229
		public static readonly string BadTexPath = "UI/Misc/BadTexture";

		// Token: 0x040000E6 RID: 230
		public static readonly string PlaceholderImagePath = "PlaceholderImage";

		// Token: 0x040000E7 RID: 231
		public static readonly string PlaceholderGearImagePath = "PlaceholderImage_Gear";

		// Token: 0x040000E8 RID: 232
		public static readonly Material BadMat = MaterialPool.MatFrom(BaseContent.BadTexPath, ShaderDatabase.Cutout);

		// Token: 0x040000E9 RID: 233
		public static readonly Texture2D BadTex = ContentFinder<Texture2D>.Get(BaseContent.BadTexPath, true);

		// Token: 0x040000EA RID: 234
		public static readonly Graphic BadGraphic = GraphicDatabase.Get<Graphic_Single>(BaseContent.BadTexPath);

		// Token: 0x040000EB RID: 235
		public static readonly Texture2D BlackTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x040000EC RID: 236
		public static readonly Texture2D GreyTex = SolidColorMaterials.NewSolidColorTexture(Color.grey);

		// Token: 0x040000ED RID: 237
		public static readonly Texture2D WhiteTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x040000EE RID: 238
		public static readonly Texture2D ClearTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x040000EF RID: 239
		public static readonly Texture2D YellowTex = SolidColorMaterials.NewSolidColorTexture(Color.yellow);

		// Token: 0x040000F0 RID: 240
		public static readonly Material BlackMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.black, false);

		// Token: 0x040000F1 RID: 241
		public static readonly Material WhiteMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.white, false);

		// Token: 0x040000F2 RID: 242
		public static readonly Material ClearMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.clear, false);
	}
}

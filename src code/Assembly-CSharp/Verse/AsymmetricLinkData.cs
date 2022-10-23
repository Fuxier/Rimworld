using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CA RID: 202
	public class AsymmetricLinkData
	{
		// Token: 0x040003C1 RID: 961
		public LinkFlags linkFlags;

		// Token: 0x040003C2 RID: 962
		public bool linkToDoors;

		// Token: 0x040003C3 RID: 963
		public AsymmetricLinkData.BorderData drawDoorBorderEast;

		// Token: 0x040003C4 RID: 964
		public AsymmetricLinkData.BorderData drawDoorBorderWest;

		// Token: 0x02001CBF RID: 7359
		public class BorderData
		{
			// Token: 0x17001DB7 RID: 7607
			// (get) Token: 0x0600B0C4 RID: 45252 RVA: 0x00401786 File Offset: 0x003FF986
			public Material Mat
			{
				get
				{
					if (this.colorMat == null)
					{
						this.colorMat = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
					}
					return this.colorMat;
				}
			}

			// Token: 0x0400714D RID: 29005
			public Color color = Color.black;

			// Token: 0x0400714E RID: 29006
			public Vector2 size;

			// Token: 0x0400714F RID: 29007
			public Vector3 offset;

			// Token: 0x04007150 RID: 29008
			private Material colorMat;
		}
	}
}

using System;

namespace Verse
{
	// Token: 0x020003ED RID: 1005
	public class Graphic_Terrain : Graphic_Single
	{
		// Token: 0x06001CA6 RID: 7334 RVA: 0x000AE498 File Offset: 0x000AC698
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x000AE4A4 File Offset: 0x000AC6A4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Terrain(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				")"
			});
		}
	}
}

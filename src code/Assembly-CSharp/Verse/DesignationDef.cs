using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000EE RID: 238
	public class DesignationDef : Def
	{
		// Token: 0x060006CD RID: 1741 RVA: 0x00024A9A File Offset: 0x00022C9A
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}

		// Token: 0x04000567 RID: 1383
		[NoTranslate]
		public string texturePath;

		// Token: 0x04000568 RID: 1384
		public TargetType targetType;

		// Token: 0x04000569 RID: 1385
		public bool removeIfBuildingDespawned;

		// Token: 0x0400056A RID: 1386
		public bool designateCancelable = true;

		// Token: 0x0400056B RID: 1387
		public bool shouldBatchDraw = true;

		// Token: 0x0400056C RID: 1388
		[Unsaved(false)]
		public Material iconMat;
	}
}

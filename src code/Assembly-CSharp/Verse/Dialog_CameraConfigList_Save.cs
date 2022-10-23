using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000098 RID: 152
	public class Dialog_CameraConfigList_Save : Dialog_CameraConfigList
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00002662 File Offset: 0x00000862
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001D8A8 File Offset: 0x0001BAA8
		public Dialog_CameraConfigList_Save(CameraMapConfig config)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.config = config;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001D8CC File Offset: 0x0001BACC
		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths.AbsFilePathForCameraConfig(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveCameraConfig(this.config, absPath);
			}, "SavingLongEvent", false, null, true);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x0400027A RID: 634
		private CameraMapConfig config;
	}
}

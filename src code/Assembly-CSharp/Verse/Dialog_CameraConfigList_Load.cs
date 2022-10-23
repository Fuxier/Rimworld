using System;

namespace Verse
{
	// Token: 0x02000099 RID: 153
	public class Dialog_CameraConfigList_Load : Dialog_CameraConfigList
	{
		// Token: 0x06000546 RID: 1350 RVA: 0x0001D938 File Offset: 0x0001BB38
		public Dialog_CameraConfigList_Load(Action<CameraMapConfig> cfgReturner)
		{
			this.configReturner = cfgReturner;
			this.interactButLabel = "Load config";
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001D954 File Offset: 0x0001BB54
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsFilePathForCameraConfig(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.CameraConfig, delegate
			{
				CameraMapConfig obj;
				if (GameDataSaveLoader.TryLoadCameraConfig(filePath, out obj))
				{
					this.configReturner(obj);
				}
				this.Close(true);
			}, false);
		}

		// Token: 0x0400027B RID: 635
		private Action<CameraMapConfig> configReturner;
	}
}

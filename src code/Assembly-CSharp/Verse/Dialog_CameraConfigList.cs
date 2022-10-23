using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x02000097 RID: 151
	public abstract class Dialog_CameraConfigList : Dialog_FileList
	{
		// Token: 0x06000541 RID: 1345 RVA: 0x0001D804 File Offset: 0x0001BA04
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCameraConfigFiles)
			{
				try
				{
					SaveFileInfo saveFileInfo = new SaveFileInfo(fileInfo);
					saveFileInfo.LoadData();
					this.files.Add(saveFileInfo);
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString());
				}
			}
		}
	}
}

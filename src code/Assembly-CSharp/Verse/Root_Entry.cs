using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x02000178 RID: 376
	public class Root_Entry : Root
	{
		// Token: 0x06000A33 RID: 2611 RVA: 0x000317EC File Offset: 0x0002F9EC
		public override void Start()
		{
			base.Start();
			try
			{
				Current.Game = null;
				this.musicManagerEntry = new MusicManagerEntry();
				FileInfo fileInfo = Root.checkedAutostartSaveFile ? null : SaveGameFilesUtility.GetAutostartSaveFile();
				Root.checkedAutostartSaveFile = true;
				if (fileInfo != null)
				{
					GameDataSaveLoader.LoadGame(fileInfo);
				}
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg);
			}
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00031854 File Offset: 0x0002FA54
		public override void Update()
		{
			base.Update();
			if (LongEventHandler.ShouldWaitForEvent || this.destroyed)
			{
				return;
			}
			try
			{
				this.musicManagerEntry.MusicManagerEntryUpdate();
				if (Find.World != null)
				{
					Find.World.WorldUpdate();
				}
				if (Current.Game != null)
				{
					Current.Game.UpdateEntry();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in Update(): " + arg);
			}
		}

		// Token: 0x04000A46 RID: 2630
		public MusicManagerEntry musicManagerEntry;
	}
}

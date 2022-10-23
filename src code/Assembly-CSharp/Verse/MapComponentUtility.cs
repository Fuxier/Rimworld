using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001FB RID: 507
	public static class MapComponentUtility
	{
		// Token: 0x06000ECB RID: 3787 RVA: 0x00051C4C File Offset: 0x0004FE4C
		public static void MapComponentUpdate(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00051C9C File Offset: 0x0004FE9C
		public static void MapComponentTick(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00051CEC File Offset: 0x0004FEEC
		public static void MapComponentOnGUI(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentOnGUI();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00051D3C File Offset: 0x0004FF3C
		public static void FinalizeInit(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].FinalizeInit();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00051D8C File Offset: 0x0004FF8C
		public static void MapGenerated(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapGenerated();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00051DDC File Offset: 0x0004FFDC
		public static void MapRemoved(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapRemoved();
				}
				catch (Exception arg)
				{
					Log.Error("Could not notify map component: " + arg);
				}
			}
		}
	}
}

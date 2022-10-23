using System;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Verse
{
	// Token: 0x02000176 RID: 374
	public static class Current
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x000312C3 File Offset: 0x0002F4C3
		public static Root Root
		{
			get
			{
				return Current.rootInt;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x000312CA File Offset: 0x0002F4CA
		public static Root_Entry Root_Entry
		{
			get
			{
				return Current.rootEntryInt;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x000312D1 File Offset: 0x0002F4D1
		public static Root_Play Root_Play
		{
			get
			{
				return Current.rootPlayInt;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x000312D8 File Offset: 0x0002F4D8
		public static Camera Camera
		{
			get
			{
				return Current.cameraInt;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x000312DF File Offset: 0x0002F4DF
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.cameraDriverInt;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x000312E6 File Offset: 0x0002F4E6
		public static ColorCorrectionCurves ColorCorrectionCurves
		{
			get
			{
				return Current.colorCorrectionCurvesInt;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x000312ED File Offset: 0x0002F4ED
		public static SubcameraDriver SubcameraDriver
		{
			get
			{
				return Current.subcameraDriverInt;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x000312F4 File Offset: 0x0002F4F4
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x000312FB File Offset: 0x0002F4FB
		public static Game Game
		{
			get
			{
				return Current.gameInt;
			}
			set
			{
				Current.gameInt = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x00031303 File Offset: 0x0002F503
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x0003130A File Offset: 0x0002F50A
		public static World CreatingWorld
		{
			get
			{
				return Current.creatingWorldInt;
			}
			set
			{
				Current.creatingWorldInt = value;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x00031312 File Offset: 0x0002F512
		// (set) Token: 0x06000A29 RID: 2601 RVA: 0x00031319 File Offset: 0x0002F519
		public static ProgramState ProgramState
		{
			get
			{
				return Current.programStateInt;
			}
			set
			{
				Current.programStateInt = value;
			}
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00031324 File Offset: 0x0002F524
		public static void Notify_LoadedSceneChanged()
		{
			Current.cameraInt = GameObject.Find("Camera").GetComponent<Camera>();
			if (GenScene.InEntryScene)
			{
				Current.ProgramState = ProgramState.Entry;
				Current.rootEntryInt = GameObject.Find("GameRoot").GetComponent<Root_Entry>();
				Current.rootPlayInt = null;
				Current.rootInt = Current.rootEntryInt;
				Current.cameraDriverInt = null;
				Current.colorCorrectionCurvesInt = null;
				return;
			}
			if (GenScene.InPlayScene)
			{
				Current.ProgramState = ProgramState.MapInitializing;
				Current.rootEntryInt = null;
				Current.rootPlayInt = GameObject.Find("GameRoot").GetComponent<Root_Play>();
				Current.rootInt = Current.rootPlayInt;
				Current.cameraDriverInt = Current.cameraInt.GetComponent<CameraDriver>();
				Current.colorCorrectionCurvesInt = Current.cameraInt.GetComponent<ColorCorrectionCurves>();
				Current.subcameraDriverInt = GameObject.Find("Subcameras").GetComponent<SubcameraDriver>();
			}
		}

		// Token: 0x04000A36 RID: 2614
		private static ProgramState programStateInt;

		// Token: 0x04000A37 RID: 2615
		private static Root rootInt;

		// Token: 0x04000A38 RID: 2616
		private static Root_Entry rootEntryInt;

		// Token: 0x04000A39 RID: 2617
		private static Root_Play rootPlayInt;

		// Token: 0x04000A3A RID: 2618
		private static Camera cameraInt;

		// Token: 0x04000A3B RID: 2619
		private static CameraDriver cameraDriverInt;

		// Token: 0x04000A3C RID: 2620
		private static ColorCorrectionCurves colorCorrectionCurvesInt;

		// Token: 0x04000A3D RID: 2621
		private static SubcameraDriver subcameraDriverInt;

		// Token: 0x04000A3E RID: 2622
		private static Game gameInt;

		// Token: 0x04000A3F RID: 2623
		private static World creatingWorldInt;
	}
}

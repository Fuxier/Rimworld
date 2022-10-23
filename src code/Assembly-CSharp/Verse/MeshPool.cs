using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000396 RID: 918
	[StaticConstructorOnStartup]
	public static class MeshPool
	{
		// Token: 0x06001A4F RID: 6735 RVA: 0x0009EB7C File Offset: 0x0009CD7C
		static MeshPool()
		{
			for (int i = 0; i < 361; i++)
			{
				MeshPool.pies[i] = MeshMakerCircles.MakePieMesh(i);
			}
			MeshPool.wholeMapPlane = MeshMakerPlanes.NewWholeMapPlane();
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x0009ED0C File Offset: 0x0009CF0C
		public static Mesh GridPlane(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planes.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, false, false, false);
				MeshPool.planes.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x0009ED40 File Offset: 0x0009CF40
		public static Mesh GridPlaneFlip(Vector2 size)
		{
			Mesh mesh;
			if (!MeshPool.planesFlip.TryGetValue(size, out mesh))
			{
				mesh = MeshMakerPlanes.NewPlaneMesh(size, true, false, false);
				MeshPool.planesFlip.Add(size, mesh);
			}
			return mesh;
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0009ED73 File Offset: 0x0009CF73
		private static Vector2 RoundedToHundredths(this Vector2 v)
		{
			return new Vector2((float)((int)(v.x * 100f)) / 100f, (float)((int)(v.y * 100f)) / 100f);
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0009EDA4 File Offset: 0x0009CFA4
		[DebugOutput("System", false)]
		public static void MeshPoolStats()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MeshPool stats:");
			stringBuilder.AppendLine("Planes: " + MeshPool.planes.Count);
			stringBuilder.AppendLine("PlanesFlip: " + MeshPool.planesFlip.Count);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x0009EE0C File Offset: 0x0009D00C
		public static GraphicMeshSet GetMeshSetForWidth(float width)
		{
			return MeshPool.GetMeshSetForWidth(width, width);
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0009EE18 File Offset: 0x0009D018
		public static GraphicMeshSet GetMeshSetForWidth(float width, float height)
		{
			Vector2 key = new Vector2(width, height);
			if (!MeshPool.humanlikeMeshSet_Custom.ContainsKey(key))
			{
				MeshPool.humanlikeMeshSet_Custom[key] = new GraphicMeshSet(width);
			}
			return MeshPool.humanlikeMeshSet_Custom[key];
		}

		// Token: 0x04001324 RID: 4900
		private const int MaxGridMeshSize = 15;

		// Token: 0x04001325 RID: 4901
		public const float HumanlikeBodyWidth = 1.5f;

		// Token: 0x04001326 RID: 4902
		public const float HumanlikeHeadAverageWidth = 1.5f;

		// Token: 0x04001327 RID: 4903
		public const float HumanlikeHeadNarrowWidth = 1.3f;

		// Token: 0x04001328 RID: 4904
		public const float HumanlikeHeadWideWidth = 1.7f;

		// Token: 0x04001329 RID: 4905
		public static readonly GraphicMeshSet humanlikeBodySet = new GraphicMeshSet(1.5f);

		// Token: 0x0400132A RID: 4906
		public static readonly GraphicMeshSet humanlikeHeadSet = new GraphicMeshSet(1.5f);

		// Token: 0x0400132B RID: 4907
		public static readonly GraphicMeshSet humanlikeSwaddledBaby = new GraphicMeshSet(1.3f);

		// Token: 0x0400132C RID: 4908
		public static readonly GraphicMeshSet humanlikeBodySet_Male = new GraphicMeshSet(1.3f);

		// Token: 0x0400132D RID: 4909
		public static readonly GraphicMeshSet humanlikeBodySet_Female = new GraphicMeshSet(1.3f, 1.4f);

		// Token: 0x0400132E RID: 4910
		public static readonly GraphicMeshSet humanlikeBodySet_Hulk = new GraphicMeshSet(1.5f, 1.65f);

		// Token: 0x0400132F RID: 4911
		public static readonly GraphicMeshSet humanlikeBodySet_Fat = new GraphicMeshSet(1.6f, 1.4f);

		// Token: 0x04001330 RID: 4912
		public static readonly GraphicMeshSet humanlikeBodySet_Thin = new GraphicMeshSet(1.2f, 1.4f);

		// Token: 0x04001331 RID: 4913
		private static readonly Dictionary<Vector2, GraphicMeshSet> humanlikeMeshSet_Custom = new Dictionary<Vector2, GraphicMeshSet>();

		// Token: 0x04001332 RID: 4914
		public static readonly Mesh plane025 = MeshMakerPlanes.NewPlaneMesh(0.25f);

		// Token: 0x04001333 RID: 4915
		public static readonly Mesh plane03 = MeshMakerPlanes.NewPlaneMesh(0.3f);

		// Token: 0x04001334 RID: 4916
		public static readonly Mesh plane05 = MeshMakerPlanes.NewPlaneMesh(0.5f);

		// Token: 0x04001335 RID: 4917
		public static readonly Mesh plane08 = MeshMakerPlanes.NewPlaneMesh(0.8f);

		// Token: 0x04001336 RID: 4918
		public static readonly Mesh plane10 = MeshMakerPlanes.NewPlaneMesh(1f);

		// Token: 0x04001337 RID: 4919
		public static readonly Mesh plane10Back = MeshMakerPlanes.NewPlaneMesh(1f, false, true);

		// Token: 0x04001338 RID: 4920
		public static readonly Mesh plane10Flip = MeshMakerPlanes.NewPlaneMesh(1f, true);

		// Token: 0x04001339 RID: 4921
		public static readonly Mesh plane14 = MeshMakerPlanes.NewPlaneMesh(1.4f);

		// Token: 0x0400133A RID: 4922
		public static readonly Mesh plane20 = MeshMakerPlanes.NewPlaneMesh(2f);

		// Token: 0x0400133B RID: 4923
		public static readonly Mesh wholeMapPlane;

		// Token: 0x0400133C RID: 4924
		private static Dictionary<Vector2, Mesh> planes = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x0400133D RID: 4925
		private static Dictionary<Vector2, Mesh> planesFlip = new Dictionary<Vector2, Mesh>(FastVector2Comparer.Instance);

		// Token: 0x0400133E RID: 4926
		public static readonly Mesh circle = MeshMakerCircles.MakeCircleMesh(1f);

		// Token: 0x0400133F RID: 4927
		public static readonly Mesh[] pies = new Mesh[361];
	}
}

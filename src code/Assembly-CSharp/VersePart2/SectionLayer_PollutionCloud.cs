using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200020F RID: 527
	[StaticConstructorOnStartup]
	public class SectionLayer_PollutionCloud : SectionLayer_Gas
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x00057021 File Offset: 0x00055221
		protected override FloatRange VertexScaleOffsetRange
		{
			get
			{
				return new FloatRange(7f, 11f);
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x00057032 File Offset: 0x00055232
		protected override FloatRange VertexPositionOffsetRange
		{
			get
			{
				return new FloatRange(-1f, 1f);
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x00057044 File Offset: 0x00055244
		public override Material Mat
		{
			get
			{
				if (SectionLayer_PollutionCloud.matCached == null)
				{
					SectionLayer_PollutionCloud.matCached = MaterialPool.MatFrom(SectionLayer_PollutionCloud.PollutionTex.Texture, ShaderDatabase.PollutionCloud, Color.white, 3000);
					SectionLayer_PollutionCloud.matCached.SetTexture("_FadeTex", SectionLayer_PollutionCloud.PollutionFadeTex.Texture);
					SectionLayer_PollutionCloud.matCached.SetVector("_FadeTexScrollSpeed", SectionLayer_PollutionCloud.FadeTexScrollSpeed);
					SectionLayer_PollutionCloud.matCached.SetVector("_FadeTexScale", SectionLayer_PollutionCloud.FadeTexScale);
				}
				return SectionLayer_PollutionCloud.matCached;
			}
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x000570D7 File Offset: 0x000552D7
		public SectionLayer_PollutionCloud(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Pollution;
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x00057114 File Offset: 0x00055314
		public override bool Visible
		{
			get
			{
				return base.Visible && ModsConfig.BiotechActive;
			}
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00057128 File Offset: 0x00055328
		public override Color ColorAt(IntVec3 cell)
		{
			float depth = base.Map.snowGrid.GetDepth(cell);
			TerrainDef terrainDef = base.Map.terrainGrid.TerrainAt(cell);
			if (depth >= 0.4f)
			{
				return SectionLayer_PollutionCloud.SnowPollutionCloudColor;
			}
			return terrainDef.pollutionCloudColor;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0005716C File Offset: 0x0005536C
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			LayerSubMesh subMesh = base.GetSubMesh(this.Mat);
			float altitude = AltitudeLayer.Gas.AltitudeFor();
			int num = this.section.botLeft.x;
			foreach (IntVec3 c in this.AffectedCells(this.section.CellRect))
			{
				int count = subMesh.verts.Count;
				base.AddCell(c, num, count, subMesh, altitude);
				num++;
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.All);
			}
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00057224 File Offset: 0x00055424
		private IEnumerable<IntVec3> AffectedCells(CellRect rect)
		{
			this.cellsTmp.Clear();
			int num = 0;
			foreach (IntVec3 cell in rect.Cells)
			{
				if (base.Map.pollutionGrid.IsPolluted(cell))
				{
					num++;
				}
			}
			if ((float)num / (float)rect.Area < this.MinPercentage)
			{
				yield break;
			}
			float numCellsToCover = SectionLayer_PollutionCloud.NumCellsPerSectionPollutionLevel.Evaluate((float)num);
			int i = 0;
			while ((float)i < numCellsToCover)
			{
				for (int j = 0; j < this.AttemptsPerCell; j++)
				{
					IntVec3 randomCell = rect.RandomCell;
					if (base.Map.pollutionGrid.IsPolluted(randomCell) && !base.Map.terrainGrid.TerrainAt(randomCell).IsWater)
					{
						bool flag = !this.cellsTmp.Contains(randomCell);
						if (flag)
						{
							foreach (IntVec3 intVec in this.cellsTmp)
							{
								if (!intVec.InHorDistOf(randomCell, this.MinCellDistance))
								{
									flag = false;
									break;
								}
							}
						}
						if (flag)
						{
							this.cellsTmp.Add(randomCell);
							yield return randomCell;
							break;
						}
					}
				}
				int num2 = i;
				i = num2 + 1;
			}
			yield break;
		}

		// Token: 0x04000D98 RID: 3480
		private static Material matCached;

		// Token: 0x04000D99 RID: 3481
		private float MinCellDistance = 5f;

		// Token: 0x04000D9A RID: 3482
		private int AttemptsPerCell = 15;

		// Token: 0x04000D9B RID: 3483
		private float MinPercentage = 0.35f;

		// Token: 0x04000D9C RID: 3484
		private static readonly SimpleCurve NumCellsPerSectionPollutionLevel = new SimpleCurve
		{
			{
				new CurvePoint(10f, 3f),
				true
			},
			{
				new CurvePoint(20f, 6f),
				true
			}
		};

		// Token: 0x04000D9D RID: 3485
		private static readonly Vector2 FadeTexScrollSpeed = new Vector2(0.035f, 0.0125f);

		// Token: 0x04000D9E RID: 3486
		private static readonly Vector2 FadeTexScale = new Vector2(0.15f, 0.15f);

		// Token: 0x04000D9F RID: 3487
		private static readonly CachedTexture PollutionTex = new CachedTexture("Other/PollutionCloud");

		// Token: 0x04000DA0 RID: 3488
		private static readonly CachedTexture PollutionFadeTex = new CachedTexture("Other/PollutionCloudFade");

		// Token: 0x04000DA1 RID: 3489
		private static readonly Color SnowPollutionCloudColor = new Color(1f, 1f, 1f, 0.66f);

		// Token: 0x04000DA2 RID: 3490
		private const float SnowPollutionColorThreshold = 0.4f;

		// Token: 0x04000DA3 RID: 3491
		private List<IntVec3> cellsTmp = new List<IntVec3>();
	}
}

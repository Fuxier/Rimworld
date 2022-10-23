using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001FD RID: 509
	public class WaterInfo : MapComponent
	{
		// Token: 0x06000ED3 RID: 3795 RVA: 0x00051E66 File Offset: 0x00050066
		public WaterInfo(Map map) : base(map)
		{
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00051E7A File Offset: 0x0005007A
		public override void MapRemoved()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				UnityEngine.Object.Destroy(this.riverOffsetTexture);
			});
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00051E90 File Offset: 0x00050090
		public void SetTextures()
		{
			Camera subcamera = Current.SubcameraDriver.GetSubcamera(SubcameraDefOf.WaterDepth);
			Shader.SetGlobalTexture(ShaderPropertyIDs.WaterOutputTex, subcamera.targetTexture);
			if (this.riverOffsetTexture == null && this.riverOffsetMap != null && this.riverOffsetMap.Length != 0)
			{
				this.riverOffsetTexture = new Texture2D(this.map.Size.x + 4, this.map.Size.z + 4, TextureFormat.RGFloat, false);
				this.riverOffsetTexture.LoadRawTextureData(this.riverOffsetMap);
				this.riverOffsetTexture.wrapMode = TextureWrapMode.Clamp;
				this.riverOffsetTexture.Apply();
			}
			Shader.SetGlobalTexture(ShaderPropertyIDs.WaterOffsetTex, this.riverOffsetTexture);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00051F48 File Offset: 0x00050148
		public Vector3 GetWaterMovement(Vector3 position)
		{
			if (this.riverOffsetMap == null)
			{
				return Vector3.zero;
			}
			if (this.riverFlowMap == null)
			{
				this.GenerateRiverFlowMap();
			}
			IntVec3 intVec = new IntVec3(Mathf.FloorToInt(position.x), 0, Mathf.FloorToInt(position.z));
			IntVec3 c = new IntVec3(Mathf.FloorToInt(position.x) + 1, 0, Mathf.FloorToInt(position.z) + 1);
			if (!this.riverFlowMapBounds.Contains(intVec) || !this.riverFlowMapBounds.Contains(c))
			{
				return Vector3.zero;
			}
			int num = this.riverFlowMapBounds.IndexOf(intVec);
			int num2 = num + 1;
			int num3 = num + this.riverFlowMapBounds.Width;
			int num4 = num3 + 1;
			Vector3 a = Vector3.Lerp(new Vector3(this.riverFlowMap[num * 2], 0f, this.riverFlowMap[num * 2 + 1]), new Vector3(this.riverFlowMap[num2 * 2], 0f, this.riverFlowMap[num2 * 2 + 1]), position.x - Mathf.Floor(position.x));
			Vector3 b = Vector3.Lerp(new Vector3(this.riverFlowMap[num3 * 2], 0f, this.riverFlowMap[num3 * 2 + 1]), new Vector3(this.riverFlowMap[num4 * 2], 0f, this.riverFlowMap[num4 * 2 + 1]), position.x - Mathf.Floor(position.x));
			return Vector3.Lerp(a, b, position.z - (float)Mathf.FloorToInt(position.z));
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x000520C8 File Offset: 0x000502C8
		public void GenerateRiverFlowMap()
		{
			if (this.riverOffsetMap == null)
			{
				return;
			}
			this.riverFlowMapBounds = new CellRect(-2, -2, this.map.Size.x + 4, this.map.Size.z + 4);
			this.riverFlowMap = new float[this.riverFlowMapBounds.Area * 2];
			float[] array = new float[this.riverFlowMapBounds.Area * 2];
			Buffer.BlockCopy(this.riverOffsetMap, 0, array, 0, array.Length * 4);
			for (int i = this.riverFlowMapBounds.minZ; i <= this.riverFlowMapBounds.maxZ; i++)
			{
				int newZ = (i == this.riverFlowMapBounds.minZ) ? i : (i - 1);
				int newZ2 = (i == this.riverFlowMapBounds.maxZ) ? i : (i + 1);
				float num = (float)((i == this.riverFlowMapBounds.minZ || i == this.riverFlowMapBounds.maxZ) ? 1 : 2);
				for (int j = this.riverFlowMapBounds.minX; j <= this.riverFlowMapBounds.maxX; j++)
				{
					int newX = (j == this.riverFlowMapBounds.minX) ? j : (j - 1);
					int newX2 = (j == this.riverFlowMapBounds.maxX) ? j : (j + 1);
					float num2 = (float)((j == this.riverFlowMapBounds.minX || j == this.riverFlowMapBounds.maxX) ? 1 : 2);
					float x = (array[this.riverFlowMapBounds.IndexOf(new IntVec3(newX2, 0, i)) * 2 + 1] - array[this.riverFlowMapBounds.IndexOf(new IntVec3(newX, 0, i)) * 2 + 1]) / num2;
					float z = (array[this.riverFlowMapBounds.IndexOf(new IntVec3(j, 0, newZ2)) * 2 + 1] - array[this.riverFlowMapBounds.IndexOf(new IntVec3(j, 0, newZ)) * 2 + 1]) / num;
					Vector3 vector = new Vector3(x, 0f, z);
					if (vector.magnitude > 0.0001f)
					{
						vector = vector.normalized / vector.magnitude;
						int num3 = this.riverFlowMapBounds.IndexOf(new IntVec3(j, 0, i)) * 2;
						this.riverFlowMap[num3] = vector.x;
						this.riverFlowMap[num3 + 1] = vector.z;
					}
				}
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00052324 File Offset: 0x00050524
		public override void ExposeData()
		{
			base.ExposeData();
			DataExposeUtility.ByteArray(ref this.riverOffsetMap, "riverOffsetMap");
			this.GenerateRiverFlowMap();
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00052344 File Offset: 0x00050544
		public void DebugDrawRiver()
		{
			for (int i = 0; i < this.riverDebugData.Count; i += 2)
			{
				GenDraw.DrawLineBetween(this.riverDebugData[i], this.riverDebugData[i + 1], SimpleColor.Magenta, 0.2f);
			}
		}

		// Token: 0x04000D49 RID: 3401
		public byte[] riverOffsetMap;

		// Token: 0x04000D4A RID: 3402
		public Texture2D riverOffsetTexture;

		// Token: 0x04000D4B RID: 3403
		public List<Vector3> riverDebugData = new List<Vector3>();

		// Token: 0x04000D4C RID: 3404
		public float[] riverFlowMap;

		// Token: 0x04000D4D RID: 3405
		public CellRect riverFlowMapBounds;

		// Token: 0x04000D4E RID: 3406
		public const int RiverOffsetMapBorder = 2;
	}
}

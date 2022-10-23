using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000269 RID: 617
	public abstract class SkyOverlay
	{
		// Token: 0x17000354 RID: 852
		// (set) Token: 0x06001198 RID: 4504 RVA: 0x00066F02 File Offset: 0x00065102
		public Color OverlayColor
		{
			set
			{
				if (this.worldOverlayMat != null)
				{
					this.worldOverlayMat.color = value;
				}
				if (this.screenOverlayMat != null)
				{
					this.screenOverlayMat.color = value;
				}
			}
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00066F38 File Offset: 0x00065138
		public SkyOverlay()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.OverlayColor = Color.clear;
			});
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00066F54 File Offset: 0x00065154
		public virtual void TickOverlay(Map map)
		{
			if (this.worldOverlayMat != null)
			{
				this.worldOverlayMat.SetTextureOffset("_MainTex", (float)(Find.TickManager.TicksGame % 3600000) * this.worldPanDir1 * -1f * this.worldOverlayPanSpeed1 * this.worldOverlayMat.GetTextureScale("_MainTex").x);
				if (this.worldOverlayMat.HasProperty("_MainTex2"))
				{
					this.worldOverlayMat.SetTextureOffset("_MainTex2", (float)(Find.TickManager.TicksGame % 3600000) * this.worldPanDir2 * -1f * this.worldOverlayPanSpeed2 * this.worldOverlayMat.GetTextureScale("_MainTex2").x);
				}
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0006703C File Offset: 0x0006523C
		public void DrawOverlay(Map map)
		{
			if (this.worldOverlayMat != null)
			{
				Vector3 position = map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather);
				Graphics.DrawMesh(MeshPool.wholeMapPlane, position, Quaternion.identity, this.worldOverlayMat, 0);
			}
			if (this.screenOverlayMat != null)
			{
				float num = Find.Camera.orthographicSize * 2f;
				Vector3 s = new Vector3(num * Find.Camera.aspect, 1f, num);
				Vector3 position2 = Find.Camera.transform.position;
				position2.y = AltitudeLayer.Weather.AltitudeFor() + 0.04054054f;
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(position2, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.screenOverlayMat, 0);
			}
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00067106 File Offset: 0x00065306
		public override string ToString()
		{
			if (this.worldOverlayMat != null)
			{
				return this.worldOverlayMat.name;
			}
			if (this.screenOverlayMat != null)
			{
				return this.screenOverlayMat.name;
			}
			return "NoOverlayOverlay";
		}

		// Token: 0x04000F04 RID: 3844
		public Material worldOverlayMat;

		// Token: 0x04000F05 RID: 3845
		public Material screenOverlayMat;

		// Token: 0x04000F06 RID: 3846
		protected float worldOverlayPanSpeed1;

		// Token: 0x04000F07 RID: 3847
		protected float worldOverlayPanSpeed2;

		// Token: 0x04000F08 RID: 3848
		protected Vector2 worldPanDir1;

		// Token: 0x04000F09 RID: 3849
		protected Vector2 worldPanDir2;
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200008F RID: 143
	public abstract class CameraMapConfig : IExposable
	{
		// Token: 0x0600052D RID: 1325 RVA: 0x0001CD14 File Offset: 0x0001AF14
		public virtual void ConfigFixedUpdate_60(ref Vector3 rootPos, ref Vector3 velocity)
		{
			if (this.followSelected)
			{
				List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
				if (selectedPawns.Count > 0)
				{
					Vector3 vector = Vector3.zero;
					int num = 0;
					foreach (Pawn pawn in selectedPawns)
					{
						if (pawn.MapHeld == Find.CurrentMap)
						{
							vector += pawn.TrueCenter();
							num++;
						}
					}
					if (num > 0)
					{
						vector /= (float)num;
						vector.y = rootPos.y;
						rootPos = Vector3.MoveTowards(rootPos, vector, 0.02f * Mathf.Max(Find.TickManager.TickRateMultiplier, 1f) * this.moveSpeedScale);
					}
				}
			}
			if (this.autoPanSpeed > 0f && (Find.TickManager.CurTimeSpeed != TimeSpeed.Paused || this.autoPanWhilePaused))
			{
				velocity.x = Mathf.Cos(this.autoPanAngle) * this.autoPanSpeed;
				velocity.z = Mathf.Sin(this.autoPanAngle) * this.autoPanSpeed;
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001CE44 File Offset: 0x0001B044
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.moveSpeedScale, "moveSpeedScale", 0f, false);
			Scribe_Values.Look<float>(ref this.zoomSpeed, "zoomSpeed", 0f, false);
			Scribe_Values.Look<FloatRange>(ref this.sizeRange, "sizeRange", default(FloatRange), false);
			Scribe_Values.Look<float>(ref this.zoomPreserveFactor, "zoomPreserveFactor", 0f, false);
			Scribe_Values.Look<bool>(ref this.smoothZoom, "smoothZoom", false, false);
			Scribe_Values.Look<bool>(ref this.followSelected, "followSelected", false, false);
			Scribe_Values.Look<float>(ref this.autoPanTargetAngle, "autoPanTargetAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.autoPanSpeed, "autoPanSpeed", 0f, false);
			Scribe_Values.Look<string>(ref this.fileName, "fileName", null, false);
			Scribe_Values.Look<bool>(ref this.autoPanWhilePaused, "autoPanWhilePaused", false, false);
		}

		// Token: 0x04000266 RID: 614
		public float dollyRateKeys = 50f;

		// Token: 0x04000267 RID: 615
		public float dollyRateScreenEdge = 35f;

		// Token: 0x04000268 RID: 616
		public float camSpeedDecayFactor = 0.85f;

		// Token: 0x04000269 RID: 617
		public float moveSpeedScale = 2f;

		// Token: 0x0400026A RID: 618
		public float zoomSpeed = 2.6f;

		// Token: 0x0400026B RID: 619
		public FloatRange sizeRange = new FloatRange(SteamDeck.IsSteamDeck ? 7.2f : 11f, 60f);

		// Token: 0x0400026C RID: 620
		public float zoomPreserveFactor;

		// Token: 0x0400026D RID: 621
		public bool smoothZoom;

		// Token: 0x0400026E RID: 622
		public bool followSelected;

		// Token: 0x0400026F RID: 623
		public string fileName;

		// Token: 0x04000270 RID: 624
		public bool autoPanWhilePaused;

		// Token: 0x04000271 RID: 625
		public float autoPanTargetAngle;

		// Token: 0x04000272 RID: 626
		public float autoPanAngle;

		// Token: 0x04000273 RID: 627
		public float autoPanSpeed;
	}
}

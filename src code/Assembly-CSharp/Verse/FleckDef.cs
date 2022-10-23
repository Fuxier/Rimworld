using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C6 RID: 454
	public class FleckDef : Def
	{
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x000478C3 File Offset: 0x00045AC3
		public float Lifespan
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x000478D9 File Offset: 0x00045AD9
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.fleckSystemClass == null)
			{
				yield return "FleckDef without system class type set!";
			}
			else if (!typeof(FleckSystem).IsAssignableFrom(this.fleckSystemClass))
			{
				yield return "FleckDef has system class type assigned which is not assignable to FleckSystemBase!";
			}
			if (this.graphicData == null && this.randomGraphics.NullOrEmpty<GraphicData>())
			{
				yield return "Fleck graphic data and random graphics are null!";
			}
			else if (this.graphicData != null && !typeof(Graphic_Fleck).IsAssignableFrom(this.graphicData.graphicClass))
			{
				yield return "Fleck graphic class is not derived from Graphic_Fleck!";
			}
			else if (!this.randomGraphics.NullOrEmpty<GraphicData>())
			{
				if (this.randomGraphics.Any((GraphicData g) => !typeof(Graphic_Fleck).IsAssignableFrom(g.graphicClass)))
				{
					yield return "random fleck graphic class is not derived from Graphic_Fleck!";
				}
			}
			yield break;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x000478EC File Offset: 0x00045AEC
		public GraphicData GetGraphicData(int id)
		{
			if (this.graphicData != null)
			{
				return this.graphicData;
			}
			Rand.PushState(id);
			GraphicData result;
			try
			{
				result = this.randomGraphics.RandomElement<GraphicData>();
			}
			finally
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x04000B96 RID: 2966
		public Type fleckSystemClass;

		// Token: 0x04000B97 RID: 2967
		public AltitudeLayer altitudeLayer;

		// Token: 0x04000B98 RID: 2968
		public float altitudeLayerIncOffset;

		// Token: 0x04000B99 RID: 2969
		public bool drawGUIOverlay;

		// Token: 0x04000B9A RID: 2970
		public GraphicData graphicData;

		// Token: 0x04000B9B RID: 2971
		public List<GraphicData> randomGraphics;

		// Token: 0x04000B9C RID: 2972
		public bool realTime;

		// Token: 0x04000B9D RID: 2973
		public bool attachedToHead;

		// Token: 0x04000B9E RID: 2974
		public float fadeInTime;

		// Token: 0x04000B9F RID: 2975
		public float solidTime = 1f;

		// Token: 0x04000BA0 RID: 2976
		public float fadeOutTime;

		// Token: 0x04000BA1 RID: 2977
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x04000BA2 RID: 2978
		public FloatRange speedPerTime = FloatRange.Zero;

		// Token: 0x04000BA3 RID: 2979
		public float growthRate;

		// Token: 0x04000BA4 RID: 2980
		public bool collide;

		// Token: 0x04000BA5 RID: 2981
		public SoundDef landSound;

		// Token: 0x04000BA6 RID: 2982
		public Vector3 unattachedDrawOffset;

		// Token: 0x04000BA7 RID: 2983
		public Vector3 attachedDrawOffset;

		// Token: 0x04000BA8 RID: 2984
		public bool rotateTowardsMoveDirection;

		// Token: 0x04000BA9 RID: 2985
		public float rotateTowardsMoveDirectionExtraAngle;

		// Token: 0x04000BAA RID: 2986
		public bool drawOffscreen;
	}
}

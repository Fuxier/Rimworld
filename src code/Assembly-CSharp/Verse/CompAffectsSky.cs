using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000410 RID: 1040
	public class CompAffectsSky : ThingComp
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000B6F15 File Offset: 0x000B5115
		public CompProperties_AffectsSky Props
		{
			get
			{
				return (CompProperties_AffectsSky)this.props;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x000B6F24 File Offset: 0x000B5124
		public virtual float LerpFactor
		{
			get
			{
				if (this.HasAutoAnimation)
				{
					int ticksGame = Find.TickManager.TicksGame;
					float num;
					if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration)
					{
						num = (float)(ticksGame - this.autoAnimationStartTick) / (float)this.fadeInDuration;
					}
					else if (ticksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration)
					{
						num = 1f;
					}
					else
					{
						num = 1f - (float)(ticksGame - this.autoAnimationStartTick - this.fadeInDuration - this.holdDuration) / (float)this.fadeOutDuration;
					}
					return Mathf.Clamp01(num * this.autoAnimationTarget);
				}
				return 0f;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x000B6FC3 File Offset: 0x000B51C3
		public bool HasAutoAnimation
		{
			get
			{
				return Find.TickManager.TicksGame < this.autoAnimationStartTick + this.fadeInDuration + this.holdDuration + this.fadeOutDuration;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x000B6FEC File Offset: 0x000B51EC
		public virtual SkyTarget SkyTarget
		{
			get
			{
				return new SkyTarget(this.Props.glow, this.Props.skyColors, this.Props.lightsourceShineSize, this.Props.lightsourceShineIntensity);
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x000B7020 File Offset: 0x000B5220
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x000B7038 File Offset: 0x000B5238
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.autoAnimationStartTick, "autoAnimationStartTick", 0, false);
			Scribe_Values.Look<int>(ref this.fadeInDuration, "fadeInDuration", 0, false);
			Scribe_Values.Look<int>(ref this.holdDuration, "holdDuration", 0, false);
			Scribe_Values.Look<int>(ref this.fadeOutDuration, "fadeOutDuration", 0, false);
			Scribe_Values.Look<float>(ref this.autoAnimationTarget, "autoAnimationTarget", 0f, false);
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x000B70A9 File Offset: 0x000B52A9
		public void StartFadeInHoldFadeOut(int fadeInDuration, int holdDuration, int fadeOutDuration, float target = 1f)
		{
			this.autoAnimationStartTick = Find.TickManager.TicksGame;
			this.fadeInDuration = fadeInDuration;
			this.holdDuration = holdDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.autoAnimationTarget = target;
		}

		// Token: 0x040014E0 RID: 5344
		private int autoAnimationStartTick;

		// Token: 0x040014E1 RID: 5345
		private int fadeInDuration;

		// Token: 0x040014E2 RID: 5346
		private int holdDuration;

		// Token: 0x040014E3 RID: 5347
		private int fadeOutDuration;

		// Token: 0x040014E4 RID: 5348
		private float autoAnimationTarget;
	}
}

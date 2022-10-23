using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000096 RID: 150
	[StaticConstructorOnStartup]
	public class Dialog_CameraConfig : Window
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001D13B File Offset: 0x0001B33B
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(260f, 300f);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001D14C File Offset: 0x0001B34C
		private CameraMapConfig Config
		{
			get
			{
				return Find.CameraDriver.config;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001D158 File Offset: 0x0001B358
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001D160 File Offset: 0x0001B360
		public Dialog_CameraConfig()
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.draggable = true;
			this.layer = WindowLayer.Super;
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.preventCameraMotion = false;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.drawInScreenshotMode = false;
			this.Reset();
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001D1C0 File Offset: 0x0001B3C0
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(4f, 0f, rect.width, 30f), "Camera config");
			Rect rect2 = new Rect(4f, 36f, rect.width - 8f, 30f);
			Widgets.HorizontalSlider(rect2, ref this.Config.moveSpeedScale, Dialog_CameraConfig.MoveScaleFactorRange, "Pan speed " + this.Config.moveSpeedScale, 0.005f);
			rect2.y += 36f;
			Widgets.HorizontalSlider(rect2, ref this.Config.zoomSpeed, Dialog_CameraConfig.ZoomScaleFactorRange, "Zoom speed " + this.Config.zoomSpeed, 0.1f);
			rect2.y += 36f;
			Widgets.FloatRange(rect2, this.GetHashCode(), ref this.Config.sizeRange, 0f, 100f, "ZoomRange", ToStringStyle.FloatOne, 1f, GameFont.Tiny, null);
			rect2.y += 36f;
			bool flag = this.Config.zoomPreserveFactor > 0f;
			Widgets.CheckboxLabeled(rect2, "Continuous zoom", ref flag, false, null, null, false);
			this.Config.zoomPreserveFactor = (flag ? 1f : 0f);
			rect2.y += 30f;
			Widgets.CheckboxLabeled(rect2, "Smooth zoom", ref this.Config.smoothZoom, false, null, null, false);
			rect2.y += 30f;
			Widgets.CheckboxLabeled(rect2, "Follow selected pawns", ref this.Config.followSelected, false, null, null, false);
			rect2.y += 30f;
			Widgets.CheckboxLabeled(rect2, "Auto pan while paused", ref this.Config.autoPanWhilePaused, false, null, null, false);
			GUI.BeginGroup(new Rect(4f, rect2.yMax, rect.width - 8f, 9999f));
			Rect rect3 = new Rect((rect.width - 8f) / 2f - 15f, 0f, 30f, 30f);
			Widgets.DrawTextureRotated(rect3.center, Dialog_CameraConfig.ArrowTex, -this.Config.autoPanTargetAngle * 57.29578f, 0.4f);
			Rect rect4 = new Rect(0f, rect3.yMax + 3f, rect.width - 8f, 30f);
			float num = this.Config.autoPanTargetAngle;
			num = Widgets.HorizontalSlider(rect4, num, 0f, 6.2831855f, false, "Auto pan angle " + (num * 57.29578f).ToString("F0") + "°", "0°", "360°", 0.01f);
			if (num != this.Config.autoPanTargetAngle)
			{
				this.Config.autoPanTargetAngle = (this.Config.autoPanAngle = num);
			}
			float yMax = rect4.yMax;
			Rect rect5 = new Rect(0f, yMax + 6f, rect.width - 8f, 30f);
			float num2 = this.Config.autoPanSpeed;
			num2 = Widgets.HorizontalSlider(rect5, num2, 0f, 5f, false, "Auto pan speed " + this.Config.autoPanSpeed, "0", "10", 0.05f);
			if (num2 != this.Config.autoPanSpeed)
			{
				this.Config.autoPanSpeed = num2;
			}
			yMax = rect5.yMax;
			GUI.EndGroup();
			Rect rect6 = new Rect(0f, rect2.yMax + yMax + 10f, rect.width, 30f);
			Rect rect7 = rect6;
			rect7.xMax = rect6.width / 3f;
			if (Widgets.ButtonText(rect7, "Reset", true, true, true, null))
			{
				this.Reset();
			}
			rect7.x += rect6.width / 3f;
			if (Widgets.ButtonText(rect7, "Save", true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_CameraConfigList_Save(this.Config));
			}
			rect7.x += rect6.width / 3f;
			if (Widgets.ButtonText(rect7, "Load", true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_CameraConfigList_Load(delegate(CameraMapConfig c)
				{
					this.Config.moveSpeedScale = c.moveSpeedScale;
					this.Config.zoomSpeed = c.zoomSpeed;
					this.Config.sizeRange = c.sizeRange;
					this.Config.zoomPreserveFactor = c.zoomPreserveFactor;
					this.Config.smoothZoom = c.smoothZoom;
					this.Config.followSelected = c.followSelected;
					this.Config.autoPanTargetAngle = (this.Config.autoPanAngle = c.autoPanTargetAngle);
					this.Config.autoPanSpeed = c.autoPanSpeed;
					this.Config.fileName = c.fileName;
					this.Config.autoPanWhilePaused = c.autoPanWhilePaused;
				}));
			}
			if (Event.current.type == EventType.Layout)
			{
				this.windowRect.height = rect6.yMax + this.Margin * 2f;
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001D6B6 File Offset: 0x0001B8B6
		private void Reset()
		{
			Find.CameraDriver.config = new CameraMapConfig_Normal();
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001D6C8 File Offset: 0x0001B8C8
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 initialSize = this.InitialSize;
			this.windowRect = new Rect(5f, 5f, initialSize.x, initialSize.y).Rounded();
		}

		// Token: 0x04000276 RID: 630
		private static readonly FloatRange MoveScaleFactorRange = new FloatRange(0f, 3f);

		// Token: 0x04000277 RID: 631
		private static readonly FloatRange ZoomScaleFactorRange = new FloatRange(0.1f, 10f);

		// Token: 0x04000278 RID: 632
		private const float SliderHeight = 30f;

		// Token: 0x04000279 RID: 633
		private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/TutorArrowRight", true);
	}
}

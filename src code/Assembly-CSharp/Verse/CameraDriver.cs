using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200008E RID: 142
	public class CameraDriver : MonoBehaviour
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001BDD4 File Offset: 0x00019FD4
		private static float ScrollWheelZoomRate
		{
			get
			{
				if (!SteamDeck.IsSteamDeck)
				{
					return 0.35f;
				}
				return 0.55f;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0001BDE8 File Offset: 0x00019FE8
		private Camera MyCamera
		{
			get
			{
				if (this.cachedCamera == null)
				{
					this.cachedCamera = base.GetComponent<Camera>();
				}
				return this.cachedCamera;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001BE0A File Offset: 0x0001A00A
		private float ScreenDollyEdgeWidthBottom
		{
			get
			{
				if (Screen.fullScreen || ResolutionUtility.BorderlessFullscreen)
				{
					return 6f;
				}
				return 20f;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0001BE28 File Offset: 0x0001A028
		public CameraZoomRange CurrentZoom
		{
			get
			{
				if (this.rootSize < this.config.sizeRange.min + 1f)
				{
					return CameraZoomRange.Closest;
				}
				if (this.rootSize < this.config.sizeRange.max * 0.23f)
				{
					return CameraZoomRange.Close;
				}
				if (this.rootSize < this.config.sizeRange.max * 0.7f)
				{
					return CameraZoomRange.Middle;
				}
				if (this.rootSize < this.config.sizeRange.max * 0.95f)
				{
					return CameraZoomRange.Far;
				}
				return CameraZoomRange.Furthest;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0001BEB6 File Offset: 0x0001A0B6
		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001BEC8 File Offset: 0x0001A0C8
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || WorldRendererUtility.WorldRenderedNow;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0001BEE0 File Offset: 0x0001A0E0
		public IntVec3 MapPosition
		{
			get
			{
				IntVec3 result = this.CurrentRealPosition.ToIntVec3();
				result.y = 0;
				return result;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0001BF04 File Offset: 0x0001A104
		public CellRect CurrentViewRect
		{
			get
			{
				if (Time.frameCount != CameraDriver.lastViewRectGetFrame)
				{
					CameraDriver.lastViewRect = default(CellRect);
					float num = (float)UI.screenWidth / (float)UI.screenHeight;
					Vector3 currentRealPosition = this.CurrentRealPosition;
					CameraDriver.lastViewRect.minX = Mathf.FloorToInt(currentRealPosition.x - this.rootSize * num - 1f);
					CameraDriver.lastViewRect.maxX = Mathf.CeilToInt(currentRealPosition.x + this.rootSize * num);
					CameraDriver.lastViewRect.minZ = Mathf.FloorToInt(currentRealPosition.z - this.rootSize - 1f);
					CameraDriver.lastViewRect.maxZ = Mathf.CeilToInt(currentRealPosition.z + this.rootSize);
					CameraDriver.lastViewRectGetFrame = Time.frameCount;
				}
				return CameraDriver.lastViewRect;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001BFD0 File Offset: 0x0001A1D0
		public static float HitchReduceFactor
		{
			get
			{
				float result = 1f;
				if (Time.deltaTime > 0.1f)
				{
					result = 0.1f / Time.deltaTime;
				}
				return result;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001BFFC File Offset: 0x0001A1FC
		public float CellSizePixels
		{
			get
			{
				return (float)UI.screenHeight / (this.rootSize * 2f);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001C011 File Offset: 0x0001A211
		public float ZoomRootSize
		{
			get
			{
				return this.rootSize;
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001C019 File Offset: 0x0001A219
		public void Awake()
		{
			this.ResetSize();
			this.reverbDummy = GameObject.Find("ReverbZoneDummy");
			this.ApplyPositionToGameObject();
			this.MyCamera.farClipPlane = 71.5f;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001C047 File Offset: 0x0001A247
		public void OnPreRender()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			Map currentMap = Find.CurrentMap;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001C057 File Offset: 0x0001A257
		public void OnPreCull()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				Find.CurrentMap.weatherManager.DrawAllWeather();
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001C080 File Offset: 0x0001A280
		public void CameraDriverOnGUI()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (Input.GetMouseButtonUp(0) && Input.GetMouseButton(2))
			{
				this.releasedLeftWhileHoldingMiddle = true;
			}
			else if (Event.current.rawType == EventType.MouseDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
			{
				this.releasedLeftWhileHoldingMiddle = false;
			}
			this.mouseCoveredByUI = false;
			if (Find.WindowStack.GetWindowAt(UI.MousePositionOnUIInverted) != null)
			{
				this.mouseCoveredByUI = true;
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				if ((!UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild && Event.current.type == EventType.MouseDrag && Event.current.button == 2) || (UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild && Input.GetMouseButton(2) && (!SteamDeck.IsSteamDeck || !Find.Selector.AnyPawnSelected)))
				{
					Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
					if (Event.current.type == EventType.MouseDrag)
					{
						Event.current.Use();
					}
					if (currentEventDelta != Vector2.zero)
					{
						currentEventDelta.x *= -1f;
						this.desiredDollyRaw += currentEventDelta / UI.CurUICellSize() * Prefs.MapDragSensitivity;
						this.panner.JumpOnNextUpdate();
					}
				}
				float num = 0f;
				if (Event.current.type == EventType.ScrollWheel)
				{
					num -= Event.current.delta.y * CameraDriver.ScrollWheelZoomRate;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.TinyInteraction);
				}
				if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
				{
					num += 4f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
				}
				if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
				{
					num -= 4f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
				}
				this.desiredSize -= num * this.config.zoomSpeed * this.rootSize / 35f;
				this.desiredSize = Mathf.Clamp(this.desiredSize, this.config.sizeRange.min, this.config.sizeRange.max);
				this.desiredDolly = Vector2.zero;
				if (KeyBindingDefOf.MapDolly_Left.IsDown)
				{
					this.desiredDolly.x = -this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Right.IsDown)
				{
					this.desiredDolly.x = this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Up.IsDown)
				{
					this.desiredDolly.y = this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Down.IsDown)
				{
					this.desiredDolly.y = -this.config.dollyRateKeys;
				}
				if (this.desiredDolly != Vector2.zero)
				{
					this.panner.JumpOnNextUpdate();
				}
				this.config.ConfigOnGUI();
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001C338 File Offset: 0x0001A538
		public void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				if (Current.SubcameraDriver != null)
				{
					Current.SubcameraDriver.UpdatePositions(this.MyCamera);
				}
				return;
			}
			if (Find.CurrentMap == null)
			{
				return;
			}
			Vector2 vector = this.CalculateCurInputDollyVect();
			if (vector != Vector2.zero)
			{
				float d = (this.rootSize - this.config.sizeRange.min) / (this.config.sizeRange.max - this.config.sizeRange.min) * 0.7f + 0.3f;
				this.velocity = new Vector3(vector.x, 0f, vector.y) * d;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraDolly, KnowledgeAmount.FrameInteraction);
			}
			if ((!Input.GetMouseButton(2) || (SteamDeck.IsSteamDeck && this.releasedLeftWhileHoldingMiddle)) && this.dragTimeStamps.Any<CameraDriver.DragTimeStamp>())
			{
				Vector2 extraVelocityFromReleasingDragButton = CameraDriver.GetExtraVelocityFromReleasingDragButton(this.dragTimeStamps, 0.75f);
				this.velocity += new Vector3(extraVelocityFromReleasingDragButton.x, 0f, extraVelocityFromReleasingDragButton.y);
				this.dragTimeStamps.Clear();
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				float d2 = Time.deltaTime * CameraDriver.HitchReduceFactor;
				this.rootPos += this.velocity * d2 * this.config.moveSpeedScale;
				this.rootPos += new Vector3(this.desiredDollyRaw.x, 0f, this.desiredDollyRaw.y);
				this.dragTimeStamps.Add(new CameraDriver.DragTimeStamp
				{
					posDelta = this.desiredDollyRaw,
					time = Time.time
				});
				this.rootPos.x = Mathf.Clamp(this.rootPos.x, 2f, (float)Find.CurrentMap.Size.x + -2f);
				this.rootPos.z = Mathf.Clamp(this.rootPos.z, 2f, (float)Find.CurrentMap.Size.z + -2f);
			}
			this.desiredDollyRaw = Vector2.zero;
			int num = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
			for (int i = 0; i < num; i++)
			{
				if (this.velocity != Vector3.zero)
				{
					this.velocity *= this.config.camSpeedDecayFactor;
					if (this.velocity.magnitude < 0.1f)
					{
						this.velocity = Vector3.zero;
					}
				}
				if (this.config.smoothZoom)
				{
					float num2 = Mathf.Lerp(this.rootSize, this.desiredSize, 0.05f);
					this.desiredSize += (num2 - this.rootSize) * this.config.zoomPreserveFactor;
					this.rootSize = num2;
				}
				else
				{
					float num3 = (this.desiredSize - this.rootSize) * 0.4f;
					this.desiredSize += this.config.zoomPreserveFactor * num3;
					this.rootSize += num3;
				}
				this.config.ConfigFixedUpdate_60(ref this.rootPos, ref this.velocity);
			}
			CameraPanner.Interpolant? interpolant = this.panner.Update();
			CameraPanner.Interpolant valueOrDefault = interpolant.GetValueOrDefault();
			if (interpolant != null)
			{
				this.rootPos = valueOrDefault.Position;
				this.rootSize = valueOrDefault.Size;
			}
			this.shaker.Update();
			this.ApplyPositionToGameObject();
			Current.SubcameraDriver.UpdatePositions(this.MyCamera);
			if (Find.CurrentMap != null)
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				rememberedCameraPos.rootPos = this.rootPos;
				rememberedCameraPos.rootSize = this.rootSize;
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001C71C File Offset: 0x0001A91C
		private void ApplyPositionToGameObject()
		{
			this.rootPos.y = 15f + (this.rootSize - this.config.sizeRange.min) / (this.config.sizeRange.max - this.config.sizeRange.min) * 50f;
			this.MyCamera.orthographicSize = this.rootSize;
			this.MyCamera.transform.position = this.rootPos + this.shaker.ShakeOffset;
			Vector3 position = base.transform.position;
			position.y = 65f;
			this.reverbDummy.transform.position = position;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001C7DC File Offset: 0x0001A9DC
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredDolly;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen || ResolutionUtility.BorderlessFullscreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 vector2 = mousePositionOnUI;
				vector2.y = (float)UI.screenHeight - vector2.y;
				Rect rect = new Rect(0f, 0f, 200f, 200f);
				Rect rect2 = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect3 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect4 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
				if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Inspect && mainTabWindow_Inspect.RecentHeight > rect3.height)
				{
					rect3.yMin = (float)UI.screenHeight - mainTabWindow_Inspect.RecentHeight;
				}
				if (!rect.Contains(vector2) && !rect3.Contains(vector2) && !rect2.Contains(vector2) && !rect4.Contains(vector2))
				{
					Vector2 b = new Vector2(0f, 0f);
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						b.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						b.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						b.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							b.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += b;
				}
			}
			if (!flag)
			{
				this.mouseTouchingScreenBottomEdgeStartTime = -1f;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				vector *= 2.4f;
			}
			return vector;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001CA80 File Offset: 0x0001AC80
		public void Expose()
		{
			if (Scribe.EnterNode("cameraMap"))
			{
				try
				{
					Scribe_Values.Look<Vector3>(ref this.rootPos, "camRootPos", default(Vector3), false);
					Scribe_Values.Look<float>(ref this.desiredSize, "desiredSize", 0f, false);
					this.rootSize = this.desiredSize;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001CAF0 File Offset: 0x0001ACF0
		public void ResetSize()
		{
			this.desiredSize = 24f;
			this.rootSize = this.desiredSize;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001CB09 File Offset: 0x0001AD09
		public void JumpToCurrentMapLoc(IntVec3 cell)
		{
			this.JumpToCurrentMapLoc(cell.ToVector3Shifted());
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001CB18 File Offset: 0x0001AD18
		public void JumpToCurrentMapLoc(Vector3 loc)
		{
			this.rootPos = new Vector3(loc.x, this.rootPos.y, loc.z);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001CB3C File Offset: 0x0001AD3C
		public void PanToMapLoc(IntVec3 cell)
		{
			Vector3 vector = cell.ToVector3Shifted();
			float x = Vector3.Distance(vector, this.CurrentRealPosition);
			float duration = GenMath.LerpDoubleClamped(0f, 70f, 0f, 0.25f, x);
			this.panner.PanTo(new CameraPanner.Interpolant(this.rootPos, this.rootSize), new CameraPanner.Interpolant(new Vector3(vector.x, this.rootPos.y, vector.z), this.rootSize), duration, null);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001CBC0 File Offset: 0x0001ADC0
		public void SetRootPosAndSize(Vector3 rootPos, float rootSize)
		{
			this.rootPos = rootPos;
			this.rootSize = rootSize;
			this.desiredDolly = Vector2.zero;
			this.desiredDollyRaw = Vector2.zero;
			this.desiredSize = rootSize;
			this.dragTimeStamps.Clear();
			LongEventHandler.ExecuteWhenFinished(new Action(this.ApplyPositionToGameObject));
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001CC14 File Offset: 0x0001AE14
		public static Vector2 GetExtraVelocityFromReleasingDragButton(List<CameraDriver.DragTimeStamp> dragTimeStamps, float velocityFromMouseDragInitialFactor)
		{
			float num = 0f;
			Vector2 vector = Vector2.zero;
			for (int i = 0; i < dragTimeStamps.Count; i++)
			{
				if (dragTimeStamps[i].time < Time.time - 0.05f)
				{
					num = 0.05f;
				}
				else
				{
					num = Mathf.Max(num, Time.time - dragTimeStamps[i].time);
					vector += dragTimeStamps[i].posDelta;
				}
			}
			if (vector != Vector2.zero && num > 0f)
			{
				return vector / num * velocityFromMouseDragInitialFactor;
			}
			return Vector2.zero;
		}

		// Token: 0x04000245 RID: 581
		public CameraShaker shaker = new CameraShaker();

		// Token: 0x04000246 RID: 582
		private CameraPanner panner;

		// Token: 0x04000247 RID: 583
		private Camera cachedCamera;

		// Token: 0x04000248 RID: 584
		private GameObject reverbDummy;

		// Token: 0x04000249 RID: 585
		public CameraMapConfig config = new CameraMapConfig_Normal();

		// Token: 0x0400024A RID: 586
		private Vector3 velocity;

		// Token: 0x0400024B RID: 587
		private Vector3 rootPos;

		// Token: 0x0400024C RID: 588
		private float rootSize;

		// Token: 0x0400024D RID: 589
		private float desiredSize;

		// Token: 0x0400024E RID: 590
		private Vector2 desiredDolly = Vector2.zero;

		// Token: 0x0400024F RID: 591
		private Vector2 desiredDollyRaw = Vector2.zero;

		// Token: 0x04000250 RID: 592
		private List<CameraDriver.DragTimeStamp> dragTimeStamps = new List<CameraDriver.DragTimeStamp>();

		// Token: 0x04000251 RID: 593
		private bool releasedLeftWhileHoldingMiddle;

		// Token: 0x04000252 RID: 594
		private bool mouseCoveredByUI;

		// Token: 0x04000253 RID: 595
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x04000254 RID: 596
		private float fixedTimeStepBuffer;

		// Token: 0x04000255 RID: 597
		private static int lastViewRectGetFrame = -1;

		// Token: 0x04000256 RID: 598
		private static CellRect lastViewRect;

		// Token: 0x04000257 RID: 599
		public const float MaxDeltaTime = 0.1f;

		// Token: 0x04000258 RID: 600
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x04000259 RID: 601
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x0400025A RID: 602
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x0400025B RID: 603
		private const float DragTimeStampExpireSeconds = 0.05f;

		// Token: 0x0400025C RID: 604
		private const float VelocityFromMouseDragInitialFactor = 0.75f;

		// Token: 0x0400025D RID: 605
		private const float MapEdgeClampMarginCells = -2f;

		// Token: 0x0400025E RID: 606
		public const float StartingSize = 24f;

		// Token: 0x0400025F RID: 607
		private const float ZoomTightness = 0.4f;

		// Token: 0x04000260 RID: 608
		private const float ZoomScaleFromAltDenominator = 35f;

		// Token: 0x04000261 RID: 609
		private const float PageKeyZoomRate = 4f;

		// Token: 0x04000262 RID: 610
		public const float MinAltitude = 15f;

		// Token: 0x04000263 RID: 611
		private const float MaxAltitude = 65f;

		// Token: 0x04000264 RID: 612
		private const float ReverbDummyAltitude = 65f;

		// Token: 0x04000265 RID: 613
		public const float FullDurationPanDistance = 70f;

		// Token: 0x02001CA6 RID: 7334
		public struct DragTimeStamp
		{
			// Token: 0x040070D9 RID: 28889
			public Vector2 posDelta;

			// Token: 0x040070DA RID: 28890
			public float time;
		}
	}
}

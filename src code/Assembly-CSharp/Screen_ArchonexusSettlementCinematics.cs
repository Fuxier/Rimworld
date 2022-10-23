using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

// Token: 0x02000012 RID: 18
public class Screen_ArchonexusSettlementCinematics : Window
{
	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000061 RID: 97 RVA: 0x00004E17 File Offset: 0x00003017
	public override Vector2 InitialSize
	{
		get
		{
			return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000062 RID: 98 RVA: 0x00004E2A File Offset: 0x0000302A
	protected override float Margin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000063 RID: 99 RVA: 0x00004E31 File Offset: 0x00003031
	private float FadeToBlackEndTime
	{
		get
		{
			return this.ScreenStartTime + 2f + 0.2f;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000064 RID: 100 RVA: 0x00004E45 File Offset: 0x00003045
	private float MessageDisplayEndTime
	{
		get
		{
			return this.FadeToBlackEndTime + 7f;
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00004E53 File Offset: 0x00003053
	public Screen_ArchonexusSettlementCinematics(Action cameraJumpAction, Action nextStepAction)
	{
		this.doWindowBackground = false;
		this.doCloseButton = false;
		this.doCloseX = false;
		this.forcePause = true;
		this.cameraJumpAction = cameraJumpAction;
		this.nextStepAction = nextStepAction;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00004E88 File Offset: 0x00003088
	public override void PreOpen()
	{
		base.PreOpen();
		Find.MusicManagerPlay.ForceFadeoutAndSilenceFor(11.2f, 1.5f);
		SoundDefOf.ArchonexusNewColonyAccept.PlayOneShotOnCamera(null);
		ScreenFader.StartFade(Color.black, 2f);
		this.ScreenStartTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00004ED4 File Offset: 0x000030D4
	public override void PostClose()
	{
		base.PostOpen();
		ScreenFader.SetColor(Color.black);
		this.nextStepAction();
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00004EF4 File Offset: 0x000030F4
	public override void DoWindowContents(Rect inRect)
	{
		if (!this.IsFinishedFadingIn())
		{
			return;
		}
		if (!this.FadeInLatch)
		{
			this.FadeInLatch = true;
			this.cameraJumpAction();
			ScreenFader.SetColor(Color.clear);
		}
		if (this.IsFinishedDisplayMessage())
		{
			this.Close(false);
			return;
		}
		Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
		GUI.DrawTexture(rect, BaseContent.BlackTex);
		Rect rect2 = new Rect(rect);
		rect2.xMin = rect.center.x - 400f;
		rect2.width = 800f;
		rect2.yMin = rect.center.y;
		GameFont font = Text.Font;
		TextAnchor anchor = Text.Anchor;
		Color color = GUI.color;
		Text.Font = GameFont.Medium;
		GUI.color = Color.white;
		Text.Anchor = TextAnchor.MiddleCenter;
		Widgets.Label(new Rect(inRect), "SoldColonyDescription".Translate());
		Text.Font = font;
		GUI.color = color;
		Text.Anchor = anchor;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00004FF1 File Offset: 0x000031F1
	public bool IsFinishedFadingIn()
	{
		return Time.realtimeSinceStartup > this.FadeToBlackEndTime;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00005000 File Offset: 0x00003200
	public bool IsFinishedDisplayMessage()
	{
		return Time.realtimeSinceStartup > this.MessageDisplayEndTime;
	}

	// Token: 0x04000020 RID: 32
	private Action cameraJumpAction;

	// Token: 0x04000021 RID: 33
	private Action nextStepAction;

	// Token: 0x04000022 RID: 34
	private bool FadeInLatch;

	// Token: 0x04000023 RID: 35
	private float ScreenStartTime;

	// Token: 0x04000024 RID: 36
	public const float FadeSecs = 2f;

	// Token: 0x04000025 RID: 37
	private const float MessageDisplaySecs = 7f;

	// Token: 0x04000026 RID: 38
	private const float FadeBuffer = 0.2f;

	// Token: 0x04000027 RID: 39
	private const int MessageWidth = 800;
}

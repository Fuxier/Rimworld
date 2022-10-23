using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020001D7 RID: 471
	public class CompGlower : ThingComp
	{
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x00049758 File Offset: 0x00047958
		public CompProperties_Glower Props
		{
			get
			{
				return (CompProperties_Glower)this.props;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00049768 File Offset: 0x00047968
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x00049798 File Offset: 0x00047998
		public virtual ColorInt GlowColor
		{
			get
			{
				ColorInt? colorInt = this.glowColorOverride;
				if (colorInt == null)
				{
					return this.Props.glowColor;
				}
				return colorInt.GetValueOrDefault();
			}
			set
			{
				this.SetGlowColorInternal(new ColorInt?(value));
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x000497A8 File Offset: 0x000479A8
		protected virtual bool ShouldBeLitNow
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				if (!FlickUtility.WantsToBeOn(this.parent))
				{
					return false;
				}
				CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
				if (compPowerTrader != null && !compPowerTrader.PowerOn)
				{
					return false;
				}
				CompRefuelable compRefuelable = this.parent.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && !compRefuelable.HasFuel)
				{
					return false;
				}
				CompSendSignalOnCountdown compSendSignalOnCountdown = this.parent.TryGetComp<CompSendSignalOnCountdown>();
				if (compSendSignalOnCountdown != null && compSendSignalOnCountdown.ticksLeft <= 0)
				{
					return false;
				}
				CompSendSignalOnMotion compSendSignalOnMotion = this.parent.TryGetComp<CompSendSignalOnMotion>();
				if (compSendSignalOnMotion != null && compSendSignalOnMotion.Sent)
				{
					return false;
				}
				CompLoudspeaker compLoudspeaker = this.parent.TryGetComp<CompLoudspeaker>();
				if (compLoudspeaker != null && !compLoudspeaker.Active)
				{
					return false;
				}
				CompHackable compHackable = this.parent.TryGetComp<CompHackable>();
				if (compHackable != null && compHackable.IsHacked && !compHackable.Props.glowIfHacked)
				{
					return false;
				}
				CompRitualSignalSender compRitualSignalSender = this.parent.TryGetComp<CompRitualSignalSender>();
				Building_Crate building_Crate;
				return (compRitualSignalSender == null || compRitualSignalSender.ritualTarget) && ((building_Crate = (this.parent as Building_Crate)) == null || building_Crate.HasAnyContents);
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x000498B6 File Offset: 0x00047AB6
		public bool Glows
		{
			get
			{
				return this.glowOnInt;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x000498BE File Offset: 0x00047ABE
		public bool HasGlowColorOverride
		{
			get
			{
				return this.glowColorOverride != null;
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x000498CC File Offset: 0x00047ACC
		public void UpdateLit(Map map)
		{
			bool shouldBeLitNow = this.ShouldBeLitNow;
			if (this.glowOnInt == shouldBeLitNow)
			{
				return;
			}
			this.glowOnInt = shouldBeLitNow;
			if (!this.glowOnInt)
			{
				map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
				map.glowGrid.DeRegisterGlower(this);
				return;
			}
			map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
			map.glowGrid.RegisterGlower(this);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00049940 File Offset: 0x00047B40
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.ShouldBeLitNow)
			{
				this.UpdateLit(this.parent.Map);
				this.parent.Map.glowGrid.RegisterGlower(this);
				return;
			}
			this.UpdateLit(this.parent.Map);
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00049990 File Offset: 0x00047B90
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff" || signal == "FlickedOn" || signal == "FlickedOff" || signal == "Refueled" || signal == "RanOutOfFuel" || signal == "ScheduledOn" || signal == "ScheduledOff" || signal == "MechClusterDefeated" || signal == "Hackend" || signal == "RitualTargetChanged" || signal == "CrateContentsChanged")
			{
				this.UpdateLit(this.parent.Map);
			}
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00049A50 File Offset: 0x00047C50
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
			Scribe_Values.Look<ColorInt?>(ref this.glowColorOverride, "glowColorOverride", null, false);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00049A89 File Offset: 0x00047C89
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.UpdateLit(map);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00049A9C File Offset: 0x00047C9C
		private List<CompGlower> ExtraSelectedGlowers(CompGlower.GlowerColorChangeType type)
		{
			CompGlower.tmpExtraGlowers.Clear();
			foreach (object obj in Find.Selector.SelectedObjectsListForReading)
			{
				ThingWithComps thingWithComps;
				if (obj != this && (thingWithComps = (obj as ThingWithComps)) != null)
				{
					foreach (CompGlower compGlower in thingWithComps.GetComps<CompGlower>())
					{
						if (type == CompGlower.GlowerColorChangeType.AllGlowers || (type == CompGlower.GlowerColorChangeType.ColorPickerEnabled && compGlower.Props.colorPickerEnabled) || (type == CompGlower.GlowerColorChangeType.DarklightToggle && compGlower.Props.darklightToggle))
						{
							CompGlower.tmpExtraGlowers.Add(compGlower);
						}
					}
				}
			}
			return CompGlower.tmpExtraGlowers;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00049B74 File Offset: 0x00047D74
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			bool doCopyPasteGizmos = false;
			CompGlower.GlowerColorChangeType type = CompGlower.GlowerColorChangeType.None;
			if (this.Props.colorPickerEnabled)
			{
				type = CompGlower.GlowerColorChangeType.ColorPickerEnabled;
			}
			else if (DebugSettings.editableGlowerColors)
			{
				type = CompGlower.GlowerColorChangeType.AllGlowers;
			}
			List<CompGlower> extraGlowers = this.ExtraSelectedGlowers(type);
			Color32 projectToColor = this.GlowColor.ProjectToColor32;
			projectToColor.a = byte.MaxValue;
			Color32? color = new Color32?(projectToColor);
			using (List<CompGlower>.Enumerator enumerator2 = extraGlowers.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.GlowColor != this.GlowColor)
					{
						color = null;
					}
				}
			}
			Command_ColorIcon command_ColorIcon = new Command_ColorIcon
			{
				defaultLabel = "GlowerChangeColor".Translate(),
				defaultDesc = "GlowerChangeColorDescription".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Commands/ChangeColor", true),
				color = color,
				action = delegate()
				{
					Widgets.ColorComponents visibleTextfields = DebugSettings.editableGlowerColors ? CompGlower.visibleDebugColorTextfields : CompGlower.visibleColorTextfields;
					Widgets.ColorComponents editableTextfields = DebugSettings.editableGlowerColors ? CompGlower.editableDebugColorTextfields : CompGlower.editableColorTextfields;
					Dialog_GlowerColorPicker window = new Dialog_GlowerColorPicker(this, extraGlowers, visibleTextfields, editableTextfields);
					Find.WindowStack.Add(window);
				}
			};
			if (this.Props.colorPickerEnabled)
			{
				bool flag = DebugSettings.editableGlowerColors || ResearchProjectDefOf.ColoredLights.IsFinished;
				doCopyPasteGizmos = flag;
				command_ColorIcon.disabled = !flag;
				command_ColorIcon.disabledReason = "GlowerChangeColorNeedsResearch".Translate(ResearchProjectDefOf.ColoredLights.label);
				yield return command_ColorIcon;
			}
			else if (DebugSettings.editableGlowerColors)
			{
				doCopyPasteGizmos = true;
				yield return command_ColorIcon;
			}
			if (doCopyPasteGizmos)
			{
				Command_ColorIcon command_ColorIcon2 = new Command_ColorIcon();
				command_ColorIcon2.icon = ContentFinder<Texture2D>.Get("UI/Commands/CopyColor", true);
				command_ColorIcon2.defaultLabel = "CommandCopyColorLabel".Translate();
				command_ColorIcon2.defaultDesc = "CommandCopyColorDesc".Translate();
				Color32 projectToColor2 = this.GlowColor.ProjectToColor32;
				projectToColor2.a = byte.MaxValue;
				command_ColorIcon2.color = new Color32?(projectToColor2);
				command_ColorIcon2.action = delegate()
				{
					CompGlower.ColorClipboard = new ColorInt?(this.GlowColor);
					Messages.Message("ColorCopiedSuccessfully".Translate(), MessageTypeDefOf.PositiveEvent, false);
				};
				command_ColorIcon2.hotKey = KeyBindingDefOf.Misc4;
				yield return command_ColorIcon2;
				bool flag2 = true;
				float hue = 0f;
				float sat = 0f;
				if (CompGlower.ColorClipboard != null)
				{
					float num;
					Color.RGBToHSV(CompGlower.ColorClipboard.Value.ProjectToColor32, out hue, out sat, out num);
					flag2 = false;
				}
				Command_ColorIcon command_ColorIcon3 = new Command_ColorIcon();
				command_ColorIcon3.icon = ContentFinder<Texture2D>.Get("UI/Commands/PasteColor", true);
				command_ColorIcon3.defaultLabel = "CommandPasteColorLabel".Translate();
				command_ColorIcon3.defaultDesc = "CommandPasteColorDesc".Translate();
				if (!flag2)
				{
					command_ColorIcon3.color = new Color32?(Color.HSVToRGB(hue, sat, 1f));
				}
				command_ColorIcon3.disabled = flag2;
				command_ColorIcon3.disabledReason = "ClipboardInvalidColor".Translate();
				command_ColorIcon3.action = delegate()
				{
					try
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						ColorInt glowColor = this.GlowColor;
						glowColor.SetHueSaturation(hue, sat);
						this.GlowColor = glowColor;
						foreach (CompGlower compGlower in extraGlowers)
						{
							glowColor = compGlower.GlowColor;
							glowColor.SetHueSaturation(hue, sat);
							compGlower.GlowColor = glowColor;
						}
						Messages.Message("ColorPastedSuccessfully".Translate(), MessageTypeDefOf.PositiveEvent, false);
					}
					catch (Exception)
					{
						Messages.Message("ClipboardInvalidColor".Translate(), MessageTypeDefOf.RejectInput, false);
					}
				};
				command_ColorIcon3.hotKey = KeyBindingDefOf.Misc5;
				yield return command_ColorIcon3;
			}
			if (ModsConfig.IdeologyActive && this.Props.darklightToggle)
			{
				bool darklight = DarklightUtility.IsDarklight(this.GlowColor.ToColor);
				yield return new Command_Action
				{
					icon = ContentFinder<Texture2D>.Get(darklight ? "UI/Commands/SetNormalLight" : "UI/Commands/SetDarklight", true),
					defaultLabel = (darklight ? "ToggleDarklightOff" : "ToggleDarklightOn").Translate(),
					defaultDesc = (darklight ? "ToggleDarklightOffDesc" : "ToggleDarklightOnDesc").Translate(),
					action = delegate()
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						if (darklight)
						{
							this.SetGlowColorInternal(null);
						}
						else
						{
							this.GlowColor = new ColorInt(DarklightUtility.DefaultDarklight);
						}
						foreach (CompGlower compGlower in this.ExtraSelectedGlowers(CompGlower.GlowerColorChangeType.DarklightToggle))
						{
							if (darklight)
							{
								compGlower.SetGlowColorInternal(null);
							}
							else
							{
								compGlower.GlowColor = new ColorInt(DarklightUtility.DefaultDarklight);
							}
						}
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00049B84 File Offset: 0x00047D84
		protected virtual void SetGlowColorInternal(ColorInt? color)
		{
			if (this.ShouldBeLitNow)
			{
				this.parent.MapHeld.glowGrid.DeRegisterGlower(this);
			}
			this.glowColorOverride = color;
			if (this.ShouldBeLitNow)
			{
				this.parent.MapHeld.glowGrid.RegisterGlower(this);
			}
		}

		// Token: 0x04000BF3 RID: 3059
		public static Widgets.ColorComponents visibleColorTextfields = Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

		// Token: 0x04000BF4 RID: 3060
		public static Widgets.ColorComponents editableColorTextfields = Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

		// Token: 0x04000BF5 RID: 3061
		public static Widgets.ColorComponents visibleDebugColorTextfields = Widgets.ColorComponents.All;

		// Token: 0x04000BF6 RID: 3062
		public static Widgets.ColorComponents editableDebugColorTextfields = Widgets.ColorComponents.Red | Widgets.ColorComponents.Green | Widgets.ColorComponents.Blue | Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

		// Token: 0x04000BF7 RID: 3063
		public static ColorInt? ColorClipboard = null;

		// Token: 0x04000BF8 RID: 3064
		private bool glowOnInt;

		// Token: 0x04000BF9 RID: 3065
		private ColorInt? glowColorOverride;

		// Token: 0x04000BFA RID: 3066
		private static List<CompGlower> tmpExtraGlowers = new List<CompGlower>(64);

		// Token: 0x02001D5C RID: 7516
		private enum GlowerColorChangeType
		{
			// Token: 0x040073EF RID: 29679
			None,
			// Token: 0x040073F0 RID: 29680
			ColorPickerEnabled,
			// Token: 0x040073F1 RID: 29681
			DarklightToggle,
			// Token: 0x040073F2 RID: 29682
			AllGlowers
		}
	}
}

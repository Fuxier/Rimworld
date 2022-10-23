using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000427 RID: 1063
	public abstract class Designator : Command
	{
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001F4B RID: 8011 RVA: 0x000BABFC File Offset: 0x000B8DFC
		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001F4C RID: 8012 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual int DraggableDimensions
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001F4D RID: 8013 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001F4E RID: 8014 RVA: 0x000BAC03 File Offset: 0x000B8E03
		public virtual bool DragDrawOutline
		{
			get
			{
				return this.DraggableDimensions == 2;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x0000249D File Offset: 0x0000069D
		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x000029B0 File Offset: 0x00000BB0
		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x00004E2A File Offset: 0x0000302A
		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x000BAC0E File Offset: 0x000B8E0E
		public override string TutorTagSelect
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorTagSelect == null)
				{
					this.cachedTutorTagSelect = "SelectDesignator-" + this.tutorTag;
				}
				return this.cachedTutorTagSelect;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001F53 RID: 8019 RVA: 0x000BAC3E File Offset: 0x000B8E3E
		public string TutorTagDesignate
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorTagDesignate == null)
				{
					this.cachedTutorTagDesignate = "Designate-" + this.tutorTag;
				}
				return this.cachedTutorTagDesignate;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x000BAC6E File Offset: 0x000B8E6E
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x000BAC9C File Offset: 0x000B8E9C
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				foreach (FloatMenuOption floatMenuOption in base.RightClickFloatMenuOptions)
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				if (this.hasDesignateAllFloatMenuOption)
				{
					int num = 0;
					List<Thing> things = this.Map.listerThings.AllThings;
					for (int i = 0; i < things.Count; i++)
					{
						Thing t = things[i];
						if (!t.Fogged() && this.CanDesignateThing(t).Accepted)
						{
							num++;
						}
					}
					if (num > 0)
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "CountToDesignate".Translate(num) + ")", delegate()
						{
							for (int j = 0; j < things.Count; j++)
							{
								Thing t2 = things[j];
								if (!t2.Fogged() && this.CanDesignateThing(t2).Accepted)
								{
									this.DesignateThing(things[j]);
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
				}
				DesignationDef designationDef = this.Designation;
				if (this.Designation != null)
				{
					int num2 = 0;
					foreach (Designation designation in this.Map.designationManager.designationsByDef[designationDef])
					{
						if (this.RemoveAllDesignationsAffects(designation.target))
						{
							num2++;
						}
					}
					if (num2 > 0)
					{
						yield return new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + num2 + ")", delegate()
						{
							List<Designation> list = this.Map.designationManager.designationsByDef[designationDef];
							for (int j = list.Count - 1; j >= 0; j--)
							{
								if (this.RemoveAllDesignationsAffects(list[j].target))
								{
									this.Map.designationManager.RemoveDesignation(list[j]);
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x000BACAC File Offset: 0x000B8EAC
		public Designator()
		{
			this.activateSound = SoundDefOf.Tick_Tiny;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x000BACDF File Offset: 0x000B8EDF
		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x000BACFD File Offset: 0x000B8EFD
		public override void ProcessInput(Event ev)
		{
			if (!this.CheckCanInteract())
			{
				return;
			}
			base.ProcessInput(ev);
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x000BAD1C File Offset: 0x000B8F1C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			if (DebugViewSettings.showArchitectMenuOrder)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Tiny;
				Widgets.Label(new Rect(topLeft.x, topLeft.y + 5f, this.GetWidth(maxWidth), 15f), this.Order.ToString());
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			return result;
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x000BAD88 File Offset: 0x000B8F88
		public Command_Action CreateReverseDesignationGizmo(Thing t)
		{
			AcceptanceReport acceptanceReport = this.CanDesignateThing(t);
			if (acceptanceReport.Accepted || (this.showReverseDesignatorDisabledReason && !acceptanceReport.Reason.NullOrEmpty()))
			{
				float iconAngle;
				Vector2 iconOffset;
				return new Command_Action
				{
					defaultLabel = this.LabelCapReverseDesignating(t),
					icon = this.IconReverseDesignating(t, out iconAngle, out iconOffset),
					iconAngle = iconAngle,
					iconOffset = iconOffset,
					defaultDesc = (acceptanceReport.Reason.NullOrEmpty() ? this.DescReverseDesignating(t) : acceptanceReport.Reason),
					Order = ((this is Designator_Uninstall) ? -11f : -20f),
					disabled = !acceptanceReport.Accepted,
					action = delegate()
					{
						if (!TutorSystem.AllowAction(this.TutorTagDesignate))
						{
							return;
						}
						this.DesignateThing(t);
						this.Finalize(true);
					},
					hotKey = this.hotKey,
					groupKeyIgnoreContent = this.groupKeyIgnoreContent,
					groupKey = this.groupKey
				};
			}
			return null;
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x000BAEA1 File Offset: 0x000B90A1
		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0003120D File Offset: 0x0002F40D
		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F5D RID: 8029
		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		// Token: 0x06001F5E RID: 8030 RVA: 0x000BAEA8 File Offset: 0x000B90A8
		public virtual void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(this.TutorTagDesignate, cells)))
			{
				return;
			}
			bool somethingSucceeded = false;
			bool flag = false;
			foreach (IntVec3 intVec in cells)
			{
				if (this.CanDesignateCell(intVec).Accepted)
				{
					this.DesignateSingleCell(intVec);
					somethingSucceeded = true;
					if (!flag)
					{
						flag = this.ShowWarningForCell(intVec);
					}
				}
			}
			this.Finalize(somethingSucceeded);
			if (TutorSystem.TutorialMode)
			{
				TutorSystem.Notify_Event(new EventPack(this.TutorTagDesignate, cells));
			}
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0003120D File Offset: 0x0002F40D
		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x000BAF50 File Offset: 0x000B9150
		public void Finalize(bool somethingSucceeded)
		{
			if (somethingSucceeded)
			{
				this.FinalizeDesignationSucceeded();
				return;
			}
			this.FinalizeDesignationFailed();
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000BAF62 File Offset: 0x000B9162
		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x000BAF78 File Offset: 0x000B9178
		protected virtual void FinalizeDesignationFailed()
		{
			if (this.soundFailed != null)
			{
				this.soundFailed.PlayOneShotOnCamera(null);
			}
			if (Find.DesignatorManager.Dragger.FailureReason != null)
			{
				Messages.Message(Find.DesignatorManager.Dragger.FailureReason, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x000BAFC4 File Offset: 0x000B91C4
		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000BAFCC File Offset: 0x000B91CC
		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000BAFD4 File Offset: 0x000B91D4
		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return (Texture2D)this.icon;
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x00002662 File Offset: 0x00000862
		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x000BAFF8 File Offset: 0x000B91F8
		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.icon, "", this.iconAngle, this.iconOffset, null, false, default(Color), null, null);
			}
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void SelectedUpdate()
		{
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void SelectedProcessInput(Event ev)
		{
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Rotate(RotationDirection rotDir)
		{
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool CanRemainSelected()
		{
			return true;
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Selected()
		{
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void Deselected()
		{
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x000BB046 File Offset: 0x000B9246
		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}

		// Token: 0x0400154B RID: 5451
		protected bool useMouseIcon;

		// Token: 0x0400154C RID: 5452
		public bool isOrder;

		// Token: 0x0400154D RID: 5453
		public SoundDef soundDragSustain;

		// Token: 0x0400154E RID: 5454
		public SoundDef soundDragChanged;

		// Token: 0x0400154F RID: 5455
		public SoundDef soundSucceeded;

		// Token: 0x04001550 RID: 5456
		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		// Token: 0x04001551 RID: 5457
		protected bool hasDesignateAllFloatMenuOption;

		// Token: 0x04001552 RID: 5458
		protected string designateAllLabel;

		// Token: 0x04001553 RID: 5459
		protected bool showReverseDesignatorDisabledReason;

		// Token: 0x04001554 RID: 5460
		private string cachedTutorTagSelect;

		// Token: 0x04001555 RID: 5461
		private string cachedTutorTagDesignate;

		// Token: 0x04001556 RID: 5462
		protected string cachedHighlightTag;
	}
}

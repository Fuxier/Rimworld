using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200042A RID: 1066
	public class DesignatorManager
	{
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001F79 RID: 8057 RVA: 0x000BB2DA File Offset: 0x000B94DA
		public Designator SelectedDesignator
		{
			get
			{
				return this.selectedDesignator;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001F7A RID: 8058 RVA: 0x000BB2E2 File Offset: 0x000B94E2
		public DesignationDragger Dragger
		{
			get
			{
				return this.dragger;
			}
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x000BB2EA File Offset: 0x000B94EA
		public void Select(Designator des)
		{
			this.Deselect();
			this.selectedDesignator = des;
			this.selectedDesignator.Selected();
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x000BB304 File Offset: 0x000B9504
		public void Deselect()
		{
			if (this.selectedDesignator != null)
			{
				this.selectedDesignator.Deselected();
				this.selectedDesignator = null;
				this.dragger.EndDrag();
			}
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x000BB32B File Offset: 0x000B952B
		private bool CheckSelectedDesignatorValid()
		{
			if (this.selectedDesignator == null)
			{
				return false;
			}
			if (!this.selectedDesignator.CanRemainSelected())
			{
				this.Deselect();
				return false;
			}
			return true;
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x000BB350 File Offset: 0x000B9550
		public void ProcessInputEvents()
		{
			if (!this.CheckSelectedDesignatorValid())
			{
				return;
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
			{
				if (this.selectedDesignator.DraggableDimensions == 0)
				{
					Designator designator = this.selectedDesignator;
					AcceptanceReport acceptanceReport = this.selectedDesignator.CanDesignateCell(UI.MouseCell());
					if (acceptanceReport.Accepted)
					{
						designator.DesignateSingleCell(UI.MouseCell());
						designator.Finalize(true);
					}
					else
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.SilentInput, false);
						this.selectedDesignator.Finalize(false);
					}
				}
				else
				{
					this.dragger.StartDrag();
				}
				Event.current.Use();
			}
			if ((Event.current.type == EventType.MouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.Deselect();
				this.dragger.EndDrag();
				Event.current.Use();
				TutorSystem.Notify_Event("ClearDesignatorSelection");
			}
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && this.dragger.Dragging)
			{
				this.selectedDesignator.DesignateMultiCell(this.dragger.DragCells);
				this.dragger.EndDrag();
				Event.current.Use();
			}
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x000BB4A3 File Offset: 0x000B96A3
		public void DesignationManagerOnGUI()
		{
			this.dragger.DraggerOnGUI();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.DrawMouseAttachments();
			}
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x000BB4C3 File Offset: 0x000B96C3
		public void DesignatorManagerUpdate()
		{
			this.dragger.DraggerUpdate();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.SelectedUpdate();
			}
		}

		// Token: 0x0400155B RID: 5467
		private Designator selectedDesignator;

		// Token: 0x0400155C RID: 5468
		private DesignationDragger dragger = new DesignationDragger();
	}
}

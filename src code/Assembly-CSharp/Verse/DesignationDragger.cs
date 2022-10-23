using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000426 RID: 1062
	[StaticConstructorOnStartup]
	public class DesignationDragger
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x000BA4BC File Offset: 0x000B86BC
		public bool Dragging
		{
			get
			{
				return this.dragging;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001F3F RID: 7999 RVA: 0x000BA4C4 File Offset: 0x000B86C4
		private Designator SelDes
		{
			get
			{
				return Find.DesignatorManager.SelectedDesignator;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001F40 RID: 8000 RVA: 0x000BA4D0 File Offset: 0x000B86D0
		public List<IntVec3> DragCells
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.dragCells;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x000BA4DE File Offset: 0x000B86DE
		public string FailureReason
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.failureReasonInt;
			}
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000BA4EC File Offset: 0x000B86EC
		public void StartDrag()
		{
			this.dragging = true;
			this.startDragCell = UI.MouseCell();
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000BA500 File Offset: 0x000B8700
		public void EndDrag()
		{
			this.dragging = false;
			this.lastDragRealTime = -99999f;
			this.lastFrameDragCellsDrawn = 0;
			if (this.sustainer != null)
			{
				this.sustainer.End();
				this.sustainer = null;
			}
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x000BA538 File Offset: 0x000B8738
		public void DraggerUpdate()
		{
			if (this.dragging)
			{
				this.tmpHighlightCells.Clear();
				this.numSelectedCells = 0;
				CellRect cellRect = this.DragRect();
				CellRect cellRect2 = cellRect.ClipInsideRect(Find.CameraDriver.CurrentViewRect.ExpandedBy(3)).ClipInsideMap(this.SelDes.Map);
				foreach (IntVec3 intVec in cellRect)
				{
					if (this.SelDes.CanDesignateCell(intVec))
					{
						if (cellRect2.Contains(intVec))
						{
							this.tmpHighlightCells.Add(intVec);
						}
						this.numSelectedCells++;
					}
				}
				this.SelDes.RenderHighlight(this.tmpHighlightCells);
				if (this.numSelectedCells != this.lastFrameDragCellsDrawn)
				{
					if (this.SelDes.soundDragChanged != null)
					{
						SoundInfo info = SoundInfo.OnCamera(MaintenanceType.None);
						info.SetParameter("TimeSinceDrag", Time.realtimeSinceStartup - this.lastDragRealTime);
						this.SelDes.soundDragChanged.PlayOneShot(info);
					}
					this.lastDragRealTime = Time.realtimeSinceStartup;
					this.lastFrameDragCellsDrawn = this.numSelectedCells;
				}
				if (this.sustainer == null || this.sustainer.Ended)
				{
					if (this.SelDes.soundDragSustain != null)
					{
						this.sustainer = this.SelDes.soundDragSustain.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
						return;
					}
				}
				else
				{
					this.sustainer.externalParams["TimeSinceDrag"] = Time.realtimeSinceStartup - this.lastDragRealTime;
					this.sustainer.Maintain();
				}
			}
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x000BA6F0 File Offset: 0x000B88F0
		public void DraggerOnGUI()
		{
			if (this.dragging && this.SelDes != null)
			{
				IntVec3 intVec = this.startDragCell - UI.MouseCell();
				intVec.x = Mathf.Abs(intVec.x) + 1;
				intVec.z = Mathf.Abs(intVec.z) + 1;
				if (this.SelDes.DragDrawOutline && (intVec.x > 1 || intVec.z > 1))
				{
					IntVec3 intVec2 = UI.MouseCell();
					Vector3 v = new Vector3((float)Mathf.Min(this.startDragCell.x, intVec2.x), 0f, (float)Mathf.Min(this.startDragCell.z, intVec2.z));
					Vector3 v2 = new Vector3((float)(Mathf.Max(this.startDragCell.x, intVec2.x) + 1), 0f, (float)(Mathf.Max(this.startDragCell.z, intVec2.z) + 1));
					Vector2 vector = v.MapToUIPosition();
					Vector2 vector2 = v2.MapToUIPosition();
					Widgets.DrawBox(Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y), 1, DesignationDragger.OutlineTex);
				}
				if (this.SelDes.DragDrawMeasurements)
				{
					if (intVec.x >= 3)
					{
						Vector2 screenPos = (this.startDragCell.ToUIPosition() + UI.MouseCell().ToUIPosition()) / 2f;
						screenPos.y = this.startDragCell.ToUIPosition().y;
						Widgets.DrawNumberOnMap(screenPos, intVec.x, Color.white);
					}
					if (intVec.z >= 3)
					{
						Vector2 screenPos2 = (this.startDragCell.ToUIPosition() + UI.MouseCell().ToUIPosition()) / 2f;
						screenPos2.x = this.startDragCell.ToUIPosition().x;
						Widgets.DrawNumberOnMap(screenPos2, intVec.z, Color.white);
					}
				}
				if (intVec.x >= 5 && intVec.z >= 5 && this.numSelectedCells > 0)
				{
					Widgets.DrawNumberOnMap((this.startDragCell.ToUIPosition() + UI.MouseCell().ToUIPosition()) / 2f, this.numSelectedCells, Color.white);
				}
			}
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x000BA940 File Offset: 0x000B8B40
		public CellRect DragRect()
		{
			IntVec3 intVec = this.startDragCell;
			IntVec3 intVec2 = UI.MouseCell();
			if (this.SelDes.DraggableDimensions == 1)
			{
				bool flag = true;
				if (Mathf.Abs(intVec.x - intVec2.x) < Mathf.Abs(intVec.z - intVec2.z))
				{
					flag = false;
				}
				if (flag)
				{
					int z = intVec.z;
					if (intVec.x > intVec2.x)
					{
						IntVec3 intVec3 = intVec;
						intVec = intVec2;
						intVec2 = intVec3;
					}
					return CellRect.FromLimits(intVec.x, z, intVec2.x, z);
				}
				int x = intVec.x;
				if (intVec.z > intVec2.z)
				{
					IntVec3 intVec4 = intVec;
					intVec = intVec2;
					intVec2 = intVec4;
				}
				return CellRect.FromLimits(x, intVec.z, x, intVec2.z);
			}
			else
			{
				if (this.SelDes.DraggableDimensions == 2)
				{
					IntVec3 intVec5 = intVec;
					IntVec3 intVec6 = intVec2;
					if (intVec6.x < intVec5.x)
					{
						int x2 = intVec5.x;
						intVec5 = new IntVec3(intVec6.x, intVec5.y, intVec5.z);
						intVec6 = new IntVec3(x2, intVec6.y, intVec6.z);
					}
					if (intVec6.z < intVec5.z)
					{
						int z2 = intVec5.z;
						intVec5 = new IntVec3(intVec5.x, intVec5.y, intVec6.z);
						intVec6 = new IntVec3(intVec6.x, intVec6.y, z2);
					}
					return CellRect.FromLimits(intVec5.x, intVec5.z, intVec6.x, intVec6.z);
				}
				return CellRect.Empty;
			}
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x000BAACC File Offset: 0x000B8CCC
		private void UpdateDragCellsIfNeeded()
		{
			if (Time.frameCount == this.lastUpdateFrame)
			{
				return;
			}
			this.lastUpdateFrame = Time.frameCount;
			this.dragCells.Clear();
			this.failureReasonInt = null;
			if (this.SelDes.DraggableDimensions > 0)
			{
				CellRect cellRect = this.DragRect();
				for (int i = cellRect.minX; i <= cellRect.maxX; i++)
				{
					for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
					{
						this.TryAddDragCell(new IntVec3(i, this.startDragCell.y, j));
					}
				}
			}
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x000BAB60 File Offset: 0x000B8D60
		private void TryAddDragCell(IntVec3 c)
		{
			AcceptanceReport acceptanceReport = this.SelDes.CanDesignateCell(c);
			if (acceptanceReport.Accepted)
			{
				this.dragCells.Add(c);
				return;
			}
			if (!acceptanceReport.Reason.NullOrEmpty())
			{
				this.failureReasonInt = acceptanceReport.Reason;
			}
		}

		// Token: 0x0400153F RID: 5439
		private bool dragging;

		// Token: 0x04001540 RID: 5440
		private IntVec3 startDragCell;

		// Token: 0x04001541 RID: 5441
		private int lastFrameDragCellsDrawn;

		// Token: 0x04001542 RID: 5442
		private Sustainer sustainer;

		// Token: 0x04001543 RID: 5443
		private float lastDragRealTime = -1000f;

		// Token: 0x04001544 RID: 5444
		private List<IntVec3> dragCells = new List<IntVec3>();

		// Token: 0x04001545 RID: 5445
		private string failureReasonInt;

		// Token: 0x04001546 RID: 5446
		private int lastUpdateFrame = -1;

		// Token: 0x04001547 RID: 5447
		private static readonly Texture2D OutlineTex = SolidColorMaterials.NewSolidColorTexture(new Color32(109, 139, 79, 100));

		// Token: 0x04001548 RID: 5448
		private const string TimeSinceDragParam = "TimeSinceDrag";

		// Token: 0x04001549 RID: 5449
		protected List<IntVec3> tmpHighlightCells = new List<IntVec3>();

		// Token: 0x0400154A RID: 5450
		private int numSelectedCells;
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200010B RID: 267
	public class WorkModeDrawer
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00002662 File Offset: 0x00000862
		protected virtual bool DrawIconAtTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00025A10 File Offset: 0x00023C10
		public virtual void DrawControlGroupMouseOverExtra(MechanitorControlGroup group)
		{
			GlobalTargetInfo targetForLine = this.GetTargetForLine(group);
			List<Pawn> mechsForReading = group.MechsForReading;
			Map currentMap = Find.CurrentMap;
			if (targetForLine.IsValid && targetForLine.Map == currentMap)
			{
				Vector3 vector = targetForLine.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MoteOverhead);
				for (int i = 0; i < mechsForReading.Count; i++)
				{
					if (mechsForReading[i].Map == currentMap)
					{
						GenDraw.DrawLineBetween(vector, mechsForReading[i].DrawPos, SimpleColor.White, 0.1f);
						GenDraw.DrawCircleOutline(mechsForReading[i].DrawPos, 0.5f);
					}
				}
				if (this.DrawIconAtTarget)
				{
					if (this.iconMat == null)
					{
						this.iconMat = MaterialPool.MatFrom(this.def.uiIcon);
					}
					Matrix4x4 matrix = Matrix4x4.TRS(vector, Quaternion.identity, WorkModeDrawer.IconScale);
					Graphics.DrawMesh(MeshPool.plane14, matrix, this.iconMat, 0);
				}
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00025B04 File Offset: 0x00023D04
		public virtual GlobalTargetInfo GetTargetForLine(MechanitorControlGroup group)
		{
			return group.Target;
		}

		// Token: 0x0400067F RID: 1663
		private const float MouseoverLineWidth = 0.1f;

		// Token: 0x04000680 RID: 1664
		private const float CircleOutlineRadius = 0.5f;

		// Token: 0x04000681 RID: 1665
		private static readonly Vector3 IconScale = Vector3.one * 0.5f;

		// Token: 0x04000682 RID: 1666
		public MechWorkModeDef def;

		// Token: 0x04000683 RID: 1667
		private Material iconMat;
	}
}

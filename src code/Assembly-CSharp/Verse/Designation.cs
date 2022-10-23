using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C1 RID: 449
	public class Designation : IExposable
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x00045EE8 File Offset: 0x000440E8
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x00045EF5 File Offset: 0x000440F5
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00045F00 File Offset: 0x00044100
		public Material IconMat
		{
			get
			{
				if (this.cachedMaterial == null)
				{
					if (this.colorDef != null)
					{
						this.cachedMaterial = new Material(this.def.iconMat);
						this.cachedMaterial.color = this.colorDef.color;
					}
					else
					{
						this.cachedMaterial = this.def.iconMat;
					}
				}
				return this.cachedMaterial;
			}
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00003724 File Offset: 0x00001924
		public Designation()
		{
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00045F68 File Offset: 0x00044168
		public Designation(LocalTargetInfo target, DesignationDef def, ColorDef colorDef = null)
		{
			this.target = target;
			this.def = def;
			this.colorDef = colorDef;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00045F88 File Offset: 0x00044188
		public void ExposeData()
		{
			Scribe_Defs.Look<DesignationDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.target, "target");
			Scribe_Defs.Look<ColorDef>(ref this.colorDef, "colorDef");
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == DesignationDefOf.Haul && !this.target.HasThing)
			{
				Log.Error("Haul designation has no target! Deleting.");
				this.Delete();
			}
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00045FF7 File Offset: 0x000441F7
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00046021 File Offset: 0x00044221
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00046058 File Offset: 0x00044258
		public Vector3 DrawLoc()
		{
			if (this.target.HasThing)
			{
				Vector3 drawPos = this.target.Thing.DrawPos;
				drawPos.y = this.DesignationDrawAltitude;
				return drawPos;
			}
			return this.target.Cell.ToVector3ShiftedWithAltitude(this.DesignationDrawAltitude);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x000460AB File Offset: 0x000442AB
		public virtual void DesignationDraw()
		{
			if (this.target.HasThing && !this.target.Thing.Spawned)
			{
				return;
			}
			Graphics.DrawMesh(MeshPool.plane10, this.DrawLoc(), Quaternion.identity, this.IconMat, 0);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x000460E9 File Offset: 0x000442E9
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x000460FC File Offset: 0x000442FC
		public override string ToString()
		{
			return string.Format(string.Concat(new object[]
			{
				"(",
				this.def.defName,
				" target=",
				this.target,
				")"
			}), Array.Empty<object>());
		}

		// Token: 0x04000B76 RID: 2934
		public DesignationManager designationManager;

		// Token: 0x04000B77 RID: 2935
		public DesignationDef def;

		// Token: 0x04000B78 RID: 2936
		public LocalTargetInfo target;

		// Token: 0x04000B79 RID: 2937
		public ColorDef colorDef;

		// Token: 0x04000B7A RID: 2938
		[Unsaved(false)]
		private Material cachedMaterial;

		// Token: 0x04000B7B RID: 2939
		public const float ClaimedDesignationDrawAltitude = 15f;
	}
}

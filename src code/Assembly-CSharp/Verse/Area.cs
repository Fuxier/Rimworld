using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001AF RID: 431
	public abstract class Area : IExposable, ILoadReferenceable, ICellBoolGiver
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x00043C94 File Offset: 0x00041E94
		public Map Map
		{
			get
			{
				return this.areaManager.map;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x00043CA1 File Offset: 0x00041EA1
		public int TrueCount
		{
			get
			{
				return this.innerGrid.TrueCount;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000C0D RID: 3085
		public abstract string Label { get; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000C0E RID: 3086
		public abstract Color Color { get; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000C0F RID: 3087
		public abstract int ListPriority { get; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x00043CAE File Offset: 0x00041EAE
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.Color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x17000255 RID: 597
		public bool this[int index]
		{
			get
			{
				return this.innerGrid[index];
			}
			set
			{
				this.Set(this.Map.cellIndices.IndexToCell(index), value);
			}
		}

		// Token: 0x17000256 RID: 598
		public bool this[IntVec3 c]
		{
			get
			{
				return this.innerGrid[this.Map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x00043D28 File Offset: 0x00041F28
		private CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new CellBoolDrawer(this, this.Map.Size.x, this.Map.Size.z, 3650, 0.33f);
				}
				return this.drawer;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x00043D79 File Offset: 0x00041F79
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				return this.innerGrid.ActiveCells;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool Mutable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00043D86 File Offset: 0x00041F86
		public Area()
		{
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00043D95 File Offset: 0x00041F95
		public Area(AreaManager areaManager)
		{
			this.areaManager = areaManager;
			this.innerGrid = new BoolGrid(areaManager.map);
			this.ID = Find.UniqueIDsManager.GetNextAreaID();
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00043DCC File Offset: 0x00041FCC
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Deep.Look<BoolGrid>(ref this.innerGrid, "innerGrid", Array.Empty<object>());
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00043CD5 File Offset: 0x00041ED5
		public bool GetCellBool(int index)
		{
			return this.innerGrid[index];
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00020495 File Offset: 0x0001E695
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool AssignableAsAllowed()
		{
			return false;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0003120D File Offset: 0x0002F40D
		public virtual void SetLabel(string label)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00043DF8 File Offset: 0x00041FF8
		protected virtual void Set(IntVec3 c, bool val)
		{
			int index = this.Map.cellIndices.CellToIndex(c);
			if (this.innerGrid[index] == val)
			{
				return;
			}
			this.innerGrid[index] = val;
			this.MarkDirty(c);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00043E3C File Offset: 0x0004203C
		private void MarkDirty(IntVec3 c)
		{
			this.Drawer.SetDirty();
			Region region = c.GetRegion(this.Map, RegionType.Set_All);
			if (region != null)
			{
				region.Notify_AreaChanged(this);
			}
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00043E6D File Offset: 0x0004206D
		public void Delete()
		{
			this.areaManager.Remove(this);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00043E7B File Offset: 0x0004207B
		public void MarkForDraw()
		{
			if (this.Map == Find.CurrentMap)
			{
				this.Drawer.MarkForDraw();
			}
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00043E95 File Offset: 0x00042095
		public void AreaUpdate()
		{
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00043EA2 File Offset: 0x000420A2
		public void Invert()
		{
			this.innerGrid.Invert();
			this.Drawer.SetDirty();
		}

		// Token: 0x06000C25 RID: 3109
		public abstract string GetUniqueLoadID();

		// Token: 0x04000B28 RID: 2856
		public AreaManager areaManager;

		// Token: 0x04000B29 RID: 2857
		public int ID = -1;

		// Token: 0x04000B2A RID: 2858
		private BoolGrid innerGrid;

		// Token: 0x04000B2B RID: 2859
		private CellBoolDrawer drawer;

		// Token: 0x04000B2C RID: 2860
		private Texture2D colorTextureInt;
	}
}

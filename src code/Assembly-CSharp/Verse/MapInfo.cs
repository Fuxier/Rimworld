using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200021D RID: 541
	public sealed class MapInfo : IExposable
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x000595C3 File Offset: 0x000577C3
		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x000595D0 File Offset: 0x000577D0
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000F6D RID: 3949 RVA: 0x000595F5 File Offset: 0x000577F5
		// (set) Token: 0x06000F6E RID: 3950 RVA: 0x000595FD File Offset: 0x000577FD
		public IntVec3 Size
		{
			get
			{
				return this.sizeInt;
			}
			set
			{
				this.sizeInt = value;
			}
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00059608 File Offset: 0x00057808
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		// Token: 0x04000DBB RID: 3515
		private IntVec3 sizeInt;

		// Token: 0x04000DBC RID: 3516
		public MapParent parent;
	}
}

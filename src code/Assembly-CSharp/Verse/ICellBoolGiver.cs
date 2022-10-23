using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B2 RID: 434
	public interface ICellBoolGiver
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000C3C RID: 3132
		Color Color { get; }

		// Token: 0x06000C3D RID: 3133
		bool GetCellBool(int index);

		// Token: 0x06000C3E RID: 3134
		Color GetCellExtraColor(int index);
	}
}

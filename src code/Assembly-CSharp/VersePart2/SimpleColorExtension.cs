using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000563 RID: 1379
	public static class SimpleColorExtension
	{
		// Token: 0x06002A38 RID: 10808 RVA: 0x0010D81C File Offset: 0x0010BA1C
		public static Color ToUnityColor(this SimpleColor color)
		{
			switch (color)
			{
			case SimpleColor.White:
				return Color.white;
			case SimpleColor.Red:
				return Color.red;
			case SimpleColor.Green:
				return Color.green;
			case SimpleColor.Blue:
				return Color.blue;
			case SimpleColor.Magenta:
				return Color.magenta;
			case SimpleColor.Yellow:
				return Color.yellow;
			case SimpleColor.Cyan:
				return Color.cyan;
			case SimpleColor.Orange:
				return ColorLibrary.Orange;
			default:
				throw new ArgumentException();
			}
		}
	}
}

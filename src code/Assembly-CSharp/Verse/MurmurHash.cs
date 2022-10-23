using System;

namespace Verse
{
	// Token: 0x02000054 RID: 84
	public static class MurmurHash
	{
		// Token: 0x06000409 RID: 1033 RVA: 0x00016498 File Offset: 0x00014698
		public static int GetInt(uint seed, uint input)
		{
			uint num = input * 3432918353U;
			num = (num << 15 | num >> 17);
			num *= 461845907U;
			uint num2 = seed ^ num;
			num2 = (num2 << 13 | num2 >> 19);
			num2 = num2 * 5U + 3864292196U;
			num2 ^= 2834544218U;
			num2 ^= num2 >> 16;
			num2 *= 2246822507U;
			num2 ^= num2 >> 13;
			num2 *= 3266489909U;
			return (int)(num2 ^ num2 >> 16);
		}

		// Token: 0x0400012E RID: 302
		private const uint Const1 = 3432918353U;

		// Token: 0x0400012F RID: 303
		private const uint Const2 = 461845907U;

		// Token: 0x04000130 RID: 304
		private const uint Const3 = 3864292196U;

		// Token: 0x04000131 RID: 305
		private const uint Const4Mix = 2246822507U;

		// Token: 0x04000132 RID: 306
		private const uint Const5Mix = 3266489909U;

		// Token: 0x04000133 RID: 307
		private const uint Const6StreamPosition = 2834544218U;
	}
}

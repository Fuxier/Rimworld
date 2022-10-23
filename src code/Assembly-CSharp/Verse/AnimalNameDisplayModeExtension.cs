using System;

namespace Verse
{
	// Token: 0x02000591 RID: 1425
	public static class AnimalNameDisplayModeExtension
	{
		// Token: 0x06002BA0 RID: 11168 RVA: 0x00114EE8 File Offset: 0x001130E8
		public static string ToStringHuman(this AnimalNameDisplayMode mode)
		{
			switch (mode)
			{
			case AnimalNameDisplayMode.None:
				return "None".Translate();
			case AnimalNameDisplayMode.TameNamed:
				return "AnimalNameDisplayMode_TameNamed".Translate();
			case AnimalNameDisplayMode.TameAll:
				return "AnimalNameDisplayMode_TameAll".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x00114F40 File Offset: 0x00113140
		public static bool ShouldDisplayAnimalName(this AnimalNameDisplayMode mode, Pawn animal)
		{
			switch (mode)
			{
			case AnimalNameDisplayMode.None:
				return false;
			case AnimalNameDisplayMode.TameNamed:
				return animal.Name != null && !animal.Name.Numerical;
			case AnimalNameDisplayMode.TameAll:
				return animal.Name != null;
			default:
				throw new NotImplementedException(Prefs.AnimalNameMode.ToStringSafe<AnimalNameDisplayMode>());
			}
		}
	}
}

using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000576 RID: 1398
	[StaticConstructorOnStartup]
	public static class GenderUtility
	{
		// Token: 0x06002AEE RID: 10990 RVA: 0x00112B12 File Offset: 0x00110D12
		public static string GetGenderLabel(this Pawn pawn)
		{
			return pawn.gender.GetLabel(pawn.RaceProps.Animal);
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x00112B2C File Offset: 0x00110D2C
		public static string GetLabel(this Gender gender, bool animal = false)
		{
			switch (gender)
			{
			case Gender.None:
				return "NoneLower".Translate();
			case Gender.Male:
				return animal ? "MaleAnimal".Translate() : "Male".Translate();
			case Gender.Female:
				return animal ? "FemaleAnimal".Translate() : "Female".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x00112BA0 File Offset: 0x00110DA0
		public static string GetPronoun(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "Proit".Translate();
			case Gender.Male:
				return "Prohe".Translate();
			case Gender.Female:
				return "Proshe".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x00112BF8 File Offset: 0x00110DF8
		public static string GetPossessive(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "Proits".Translate();
			case Gender.Male:
				return "Prohis".Translate();
			case Gender.Female:
				return "Proher".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x00112C50 File Offset: 0x00110E50
		public static string GetObjective(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "ProitObj".Translate();
			case Gender.Male:
				return "ProhimObj".Translate();
			case Gender.Female:
				return "ProherObj".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x00112CA6 File Offset: 0x00110EA6
		public static Texture2D GetIcon(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return GenderUtility.GenderlessIcon;
			case Gender.Male:
				return GenderUtility.MaleIcon;
			case Gender.Female:
				return GenderUtility.FemaleIcon;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x00112CD3 File Offset: 0x00110ED3
		public static Gender Opposite(this Gender gender)
		{
			if (gender == Gender.Male)
			{
				return Gender.Female;
			}
			if (gender == Gender.Female)
			{
				return Gender.Male;
			}
			return Gender.None;
		}

		// Token: 0x04001C0E RID: 7182
		private static readonly Texture2D GenderlessIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Genderless", true);

		// Token: 0x04001C0F RID: 7183
		private static readonly Texture2D MaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Male", true);

		// Token: 0x04001C10 RID: 7184
		private static readonly Texture2D FemaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Female", true);
	}
}

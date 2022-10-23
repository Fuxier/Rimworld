using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029C RID: 668
	public static class PawnNameColorUtility
	{
		// Token: 0x06001321 RID: 4897 RVA: 0x00072748 File Offset: 0x00070948
		static PawnNameColorUtility()
		{
			for (int i = 0; i < 10; i++)
			{
				PawnNameColorUtility.ColorsNeutral.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseNeutral, i));
				PawnNameColorUtility.ColorsHostile.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseHostile, i));
				PawnNameColorUtility.ColorsPrisoner.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBasePrisoner, i));
			}
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00072984 File Offset: 0x00070B84
		private static Color RandomShiftOf(Color color, int i)
		{
			return new Color(Mathf.Clamp01(color.r * PawnNameColorUtility.ColorShifts[i].r), Mathf.Clamp01(color.g * PawnNameColorUtility.ColorShifts[i].g), Mathf.Clamp01(color.b * PawnNameColorUtility.ColorShifts[i].b), color.a);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x000729F0 File Offset: 0x00070BF0
		public static Color PawnNameColorOf(Pawn pawn)
		{
			if (pawn.MentalStateDef != null)
			{
				return pawn.MentalStateDef.nameColor;
			}
			int index;
			if (pawn.Faction == null)
			{
				index = 0;
			}
			else
			{
				index = pawn.Faction.randomKey % 10;
			}
			if (pawn.IsPrisoner)
			{
				return PawnNameColorUtility.ColorsPrisoner[index];
			}
			if (pawn.IsSlave && SlaveRebellionUtility.IsRebelling(pawn))
			{
				return PawnNameColorUtility.ColorBaseHostile;
			}
			if (pawn.IsSlave)
			{
				return PawnNameColorUtility.ColorSlave;
			}
			if (pawn.IsWildMan())
			{
				return PawnNameColorUtility.ColorWildMan;
			}
			if (pawn.Faction == null)
			{
				return PawnNameColorUtility.ColorsNeutral[index];
			}
			if (pawn.IsColonyMechRequiringMechanitor())
			{
				return PawnNameColorUtility.ColorUncontrolledPlayerMech;
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return PawnNameColorUtility.ColorColony;
			}
			if (pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				return PawnNameColorUtility.ColorsHostile[index];
			}
			return PawnNameColorUtility.ColorsNeutral[index];
		}

		// Token: 0x04000FC1 RID: 4033
		private static readonly List<Color> ColorsNeutral = new List<Color>();

		// Token: 0x04000FC2 RID: 4034
		private static readonly List<Color> ColorsHostile = new List<Color>();

		// Token: 0x04000FC3 RID: 4035
		private static readonly List<Color> ColorsPrisoner = new List<Color>();

		// Token: 0x04000FC4 RID: 4036
		private static readonly Color ColorBaseNeutral = new Color(0.4f, 0.85f, 0.9f);

		// Token: 0x04000FC5 RID: 4037
		private static readonly Color ColorBaseHostile = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x04000FC6 RID: 4038
		private static readonly Color ColorBasePrisoner = new Color(1f, 0.85f, 0.5f);

		// Token: 0x04000FC7 RID: 4039
		private static readonly Color ColorSlave = new Color32(222, 192, 22, byte.MaxValue);

		// Token: 0x04000FC8 RID: 4040
		private static readonly Color ColorColony = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04000FC9 RID: 4041
		private static readonly Color ColorWildMan = new Color(1f, 0.8f, 1f);

		// Token: 0x04000FCA RID: 4042
		private static readonly Color ColorUncontrolledPlayerMech = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x04000FCB RID: 4043
		private const int ColorShiftCount = 10;

		// Token: 0x04000FCC RID: 4044
		private static readonly List<Color> ColorShifts = new List<Color>
		{
			new Color(1f, 1f, 1f),
			new Color(0.8f, 1f, 1f),
			new Color(0.8f, 0.8f, 1f),
			new Color(0.8f, 0.8f, 0.8f),
			new Color(1.2f, 1f, 1f),
			new Color(0.8f, 1.2f, 1f),
			new Color(0.8f, 1.2f, 1.2f),
			new Color(1.2f, 1.2f, 1.2f),
			new Color(1f, 1.2f, 1f),
			new Color(1.2f, 1f, 0.8f)
		};
	}
}

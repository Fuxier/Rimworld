using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001F8 RID: 504
	public static class NameUseChecker
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000E97 RID: 3735 RVA: 0x00050358 File Offset: 0x0004E558
		public static IEnumerable<Name> AllPawnsNamesEverUsed
		{
			get
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
				{
					if (pawn.Name != null)
					{
						yield return pawn.Name;
					}
				}
				List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00050364 File Offset: 0x0004E564
		public static bool NameWordIsUsed(string singleName)
		{
			foreach (Name name in NameUseChecker.AllPawnsNamesEverUsed)
			{
				NameTriple nameTriple = name as NameTriple;
				if (nameTriple != null && (singleName == nameTriple.First || singleName == nameTriple.Nick || singleName == nameTriple.Last))
				{
					return true;
				}
				NameSingle nameSingle = name as NameSingle;
				if (nameSingle != null && nameSingle.Name == singleName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00050404 File Offset: 0x0004E604
		public static bool NameSingleIsUsed(string candidate)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				NameSingle nameSingle = pawn.Name as NameSingle;
				if (nameSingle != null && nameSingle.Name == candidate)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00050474 File Offset: 0x0004E674
		public static bool XenotypeNameIsUsed(string candidate)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn.genes != null && pawn.genes.UniqueXenotype)
				{
					string xenotypeName = pawn.genes.xenotypeName;
					if (candidate == xenotypeName)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}

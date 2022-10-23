using System;

namespace Verse
{
	// Token: 0x02000338 RID: 824
	public static class HediffMaker
	{
		// Token: 0x060015F9 RID: 5625 RVA: 0x00082300 File Offset: 0x00080500
		public static Hediff MakeHediff(HediffDef def, Pawn pawn, BodyPartRecord partRecord = null)
		{
			if (pawn == null)
			{
				Log.Error("Cannot make hediff " + def + " for null pawn.");
				return null;
			}
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.pawn = pawn;
			hediff.Part = partRecord;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00082362 File Offset: 0x00080562
		public static Hediff Debug_MakeConcreteExampleHediff(HediffDef def)
		{
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}
	}
}

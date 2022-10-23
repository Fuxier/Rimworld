using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000510 RID: 1296
	public class BackCompatibilityConverter_1_3 : BackCompatibilityConverter
	{
		// Token: 0x060027A1 RID: 10145 RVA: 0x001014C7 File Offset: 0x000FF6C7
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 || (majorVer == 1 && minorVer <= 3);
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x001014DC File Offset: 0x000FF6DC
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(JobDef) && defName == "TriggerFirefoamPopper")
			{
				return "TriggerObject";
			}
			if (defType == typeof(TerrainDef) && defName == "CarpetDark")
			{
				return "CarpetGreyDark";
			}
			return null;
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000029B0 File Offset: 0x00000BB0
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x00101534 File Offset: 0x000FF734
		public override void PostExposeData(object obj)
		{
			Pawn pawn;
			if ((pawn = (obj as Pawn)) != null && pawn.RaceProps.Humanlike && pawn.genes == null)
			{
				pawn.genes = new Pawn_GeneTracker(pawn);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B1 RID: 433
	public static class AreaUtility
	{
		// Token: 0x06000C39 RID: 3129 RVA: 0x00044244 File Offset: 0x00042444
		public static void MakeAllowedAreaListFloatMenu(Action<Area> selAction, bool addNullAreaOption, bool addManageOption, Map map)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (addNullAreaOption)
			{
				list.Add(new FloatMenuOption("NoAreaAllowed".Translate(), delegate()
				{
					selAction(null);
				}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0));
			}
			foreach (Area localArea2 in from a in map.areaManager.AllAreas
			where a.AssignableAsAllowed()
			select a)
			{
				Area localArea = localArea2;
				FloatMenuOption item = new FloatMenuOption(localArea.Label, delegate()
				{
					selAction(localArea);
				}, MenuOptionPriority.Default, delegate(Rect _)
				{
					localArea.MarkForDraw();
				}, null, 0f, null, null, true, 0);
				list.Add(item);
			}
			if (addManageOption)
			{
				list.Add(new FloatMenuOption("ManageAreas".Translate(), delegate()
				{
					Find.WindowStack.Add(new Dialog_ManageAreas(map));
				}, MenuOptionPriority.Low, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x000443A4 File Offset: 0x000425A4
		public static string AreaAllowedLabel(Pawn pawn)
		{
			if (pawn.playerSettings != null)
			{
				return AreaUtility.AreaAllowedLabel_Area(pawn.playerSettings.EffectiveAreaRestriction);
			}
			return AreaUtility.AreaAllowedLabel_Area(null);
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x000443C5 File Offset: 0x000425C5
		public static string AreaAllowedLabel_Area(Area area)
		{
			if (area != null)
			{
				return area.Label;
			}
			return "NoAreaAllowed".Translate();
		}
	}
}

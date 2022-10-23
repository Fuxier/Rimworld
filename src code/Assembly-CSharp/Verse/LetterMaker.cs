using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020004A4 RID: 1188
	public static class LetterMaker
	{
		// Token: 0x060023B9 RID: 9145 RVA: 0x000E4C9E File Offset: 0x000E2E9E
		public static Letter MakeLetter(LetterDef def)
		{
			Letter letter = (Letter)Activator.CreateInstance(def.letterClass);
			letter.def = def;
			letter.ID = Find.UniqueIDsManager.GetNextLetterID();
			return letter;
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000E4CC8 File Offset: 0x000E2EC8
		public static ChoiceLetter MakeLetter(TaggedString label, TaggedString text, LetterDef def, Faction relatedFaction = null, Quest quest = null)
		{
			if (!typeof(ChoiceLetter).IsAssignableFrom(def.letterClass))
			{
				Log.Error(def + " is not a choice letter.");
				return null;
			}
			ChoiceLetter choiceLetter = (ChoiceLetter)LetterMaker.MakeLetter(def);
			choiceLetter.Label = label;
			choiceLetter.Text = text;
			choiceLetter.relatedFaction = relatedFaction;
			choiceLetter.quest = quest;
			return choiceLetter;
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x000E4D26 File Offset: 0x000E2F26
		public static ChoiceLetter MakeLetter(TaggedString label, TaggedString text, LetterDef def, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null)
		{
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, def, null, null);
			choiceLetter.lookTargets = lookTargets;
			choiceLetter.relatedFaction = relatedFaction;
			choiceLetter.quest = quest;
			choiceLetter.hyperlinkThingDefs = hyperlinkThingDefs;
			return choiceLetter;
		}
	}
}

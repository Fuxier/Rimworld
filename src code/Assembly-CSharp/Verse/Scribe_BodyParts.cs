using System;

namespace Verse
{
	// Token: 0x020003B8 RID: 952
	public static class Scribe_BodyParts
	{
		// Token: 0x06001B1A RID: 6938 RVA: 0x000A5B8C File Offset: 0x000A3D8C
		public static void Look(ref BodyPartRecord part, string label, BodyPartRecord defaultValue = null)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (part == defaultValue || !Scribe.EnterNode(label))
				{
					return;
				}
				try
				{
					if (part == null)
					{
						Scribe.saver.WriteAttribute("IsNull", "True");
						return;
					}
					string defName = part.body.defName;
					Scribe_Values.Look<string>(ref defName, "body", null, false);
					int index = part.Index;
					Scribe_Values.Look<int>(ref index, "index", 0, true);
					return;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				part = ScribeExtractor.BodyPartFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
			}
		}
	}
}

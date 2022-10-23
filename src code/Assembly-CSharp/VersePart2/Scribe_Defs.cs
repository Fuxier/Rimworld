using System;

namespace Verse
{
	// Token: 0x020003BB RID: 955
	public static class Scribe_Defs
	{
		// Token: 0x06001B25 RID: 6949 RVA: 0x000A6C88 File Offset: 0x000A4E88
		public static void Look<T>(ref T value, string label) where T : Def, new()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				string text;
				if (value == null)
				{
					text = "null";
				}
				else
				{
					text = value.defName;
				}
				Scribe_Values.Look<string>(ref text, label, "null", false);
				return;
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.DefFromNode<T>(Scribe.loader.curXmlParent[label]);
			}
		}
	}
}

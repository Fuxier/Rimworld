using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004A6 RID: 1190
	public class BundleLetter : Letter
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060023CB RID: 9163 RVA: 0x0000249D File Offset: 0x0000069D
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x000E51A4 File Offset: 0x000E33A4
		public void SetLetters(List<Letter> letters)
		{
			if (GenCollection.ListsEqual<Letter>(letters, this.bundledLetters))
			{
				return;
			}
			this.bundledLetters.Clear();
			this.bundledLetters.AddRange(letters);
			this.floatMenuOptions.Clear();
			using (List<Letter>.Enumerator enumerator = letters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Letter letter = enumerator.Current;
					FloatMenuOption item = new FloatMenuOption(letter.Label, delegate()
					{
						letter.OpenLetter();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					this.floatMenuOptions.Add(item);
				}
			}
			base.Label = this.bundledLetters.Count + " " + "MoreLower".Translate() + "...";
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000E529C File Offset: 0x000E349C
		public override void OpenLetter()
		{
			if (Event.current.button == 0)
			{
				Find.WindowStack.Add(new FloatMenu(this.floatMenuOptions));
			}
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000E52C0 File Offset: 0x000E34C0
		protected override string GetMouseoverText()
		{
			return "MoreLetters".Translate(this.bundledLetters.Count) + ":\n\n" + (from l in this.bundledLetters
			select l.Label.ToString()).ToLineList(" - ", false);
		}

		// Token: 0x04001720 RID: 5920
		private List<Letter> bundledLetters = new List<Letter>();

		// Token: 0x04001721 RID: 5921
		private List<FloatMenuOption> floatMenuOptions = new List<FloatMenuOption>();
	}
}

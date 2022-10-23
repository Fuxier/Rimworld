using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse
{
	// Token: 0x020000A7 RID: 167
	public class Def : Editable
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x0001F468 File Offset: 0x0001D668
		public virtual TaggedString LabelCap
		{
			get
			{
				if (this.label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001F4E3 File Offset: 0x0001D6E3
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.modContentPack != null && !this.modContentPack.IsCoreMod)
			{
				TaggedString t = this.modContentPack.IsOfficialMod ? "Stat_Source_OfficialExpansionReport".Translate() : "Stat_Source_ModReport".Translate();
				yield return new StatDrawEntry(StatCategoryDefOf.Source, "Stat_Source_Label".Translate(), this.modContentPack.Name, t + ": " + this.modContentPack.Name, 90000, null, null, false);
			}
			yield break;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001F4F3 File Offset: 0x0001D6F3
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.defName == "UnnamedDef")
			{
				yield return base.GetType() + " lacks defName. Label=" + this.label;
			}
			if (this.defName == "null")
			{
				yield return "defName cannot be the string 'null'.";
			}
			if (!Def.AllowedDefnamesRegex.IsMatch(this.defName))
			{
				yield return "defName " + this.defName + " should only contain letters, numbers, underscores, or dashes.";
			}
			if (this.modExtensions != null)
			{
				int num;
				for (int i = 0; i < this.modExtensions.Count; i = num)
				{
					foreach (string text in this.modExtensions[i].ConfigErrors())
					{
						yield return text;
					}
					IEnumerator<string> enumerator = null;
					num = i + 1;
				}
			}
			if (this.description != null)
			{
				if (this.description == "")
				{
					yield return "empty description";
				}
				if (char.IsWhiteSpace(this.description[0]))
				{
					yield return "description has leading whitespace";
				}
				if (char.IsWhiteSpace(this.description[this.description.Length - 1]))
				{
					yield return "description has trailing whitespace";
				}
			}
			if (this.descriptionHyperlinks != null && this.descriptionHyperlinks.Count > 0)
			{
				if (this.descriptionHyperlinks.RemoveAll((DefHyperlink x) => x.def == null) != 0)
				{
					Log.Warning("Some descriptionHyperlinks in " + this.defName + " had null def.");
				}
				Def.<>c__DisplayClass21_0 CS$<>8__locals1 = new Def.<>c__DisplayClass21_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.i = this.descriptionHyperlinks.Count - 1;
				while (CS$<>8__locals1.i > 0)
				{
					if (this.descriptionHyperlinks.FirstIndexOf((DefHyperlink h) => h.def == CS$<>8__locals1.<>4__this.descriptionHyperlinks[CS$<>8__locals1.i].def) < CS$<>8__locals1.i)
					{
						yield return string.Concat(new string[]
						{
							"Hyperlink to ",
							this.descriptionHyperlinks[CS$<>8__locals1.i].def.defName,
							" more than once on ",
							this.defName,
							" description"
						});
					}
					int num = CS$<>8__locals1.i;
					CS$<>8__locals1.i = num - 1;
				}
				CS$<>8__locals1 = null;
			}
			if (this.label != null && !this.ignoreIllegalLabelCharacterConfigError && Def.DisallowedLabelCharsRegex.IsMatch(this.label))
			{
				yield return "label contains illegal character(s): \"[]{}\". This can cause issues during grammar resolution. If this was intended, you can use the \"ignoreIllegalLabelCharacterConfigError\" flag.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001F503 File Offset: 0x0001D703
		public virtual void ClearCachedData()
		{
			this.cachedLabelCap = null;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001F511 File Offset: 0x0001D711
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001F519 File Offset: 0x0001D719
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001F528 File Offset: 0x0001D728
		public T GetModExtension<T>() where T : DefModExtension
		{
			if (this.modExtensions == null)
			{
				return default(T);
			}
			for (int i = 0; i < this.modExtensions.Count; i++)
			{
				if (this.modExtensions[i] is T)
				{
					return this.modExtensions[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001F590 File Offset: 0x0001D790
		public bool HasModExtension<T>() where T : DefModExtension
		{
			return this.GetModExtension<T>() != null;
		}

		// Token: 0x0400029C RID: 668
		[Description("The name of this Def. It is used as an identifier by the game code.")]
		[NoTranslate]
		public string defName = "UnnamedDef";

		// Token: 0x0400029D RID: 669
		[Description("A human-readable label used to identify this in game.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string label;

		// Token: 0x0400029E RID: 670
		[Description("A human-readable description given when the Def is inspected by players.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string description;

		// Token: 0x0400029F RID: 671
		[XmlInheritanceAllowDuplicateNodes]
		public List<DefHyperlink> descriptionHyperlinks;

		// Token: 0x040002A0 RID: 672
		[Description("Disables config error checking. Intended for mod use. (Be careful!)")]
		[DefaultValue(false)]
		[MustTranslate]
		public bool ignoreConfigErrors;

		// Token: 0x040002A1 RID: 673
		public bool ignoreIllegalLabelCharacterConfigError;

		// Token: 0x040002A2 RID: 674
		[Description("Mod-specific data. Not used by core game code.")]
		[DefaultValue(null)]
		public List<DefModExtension> modExtensions;

		// Token: 0x040002A3 RID: 675
		[Unsaved(false)]
		public ushort shortHash;

		// Token: 0x040002A4 RID: 676
		[Unsaved(false)]
		public ushort index = ushort.MaxValue;

		// Token: 0x040002A5 RID: 677
		[Unsaved(false)]
		public ModContentPack modContentPack;

		// Token: 0x040002A6 RID: 678
		[Unsaved(false)]
		public string fileName;

		// Token: 0x040002A7 RID: 679
		[Unsaved(false)]
		protected TaggedString cachedLabelCap = null;

		// Token: 0x040002A8 RID: 680
		[Unsaved(false)]
		public bool generated;

		// Token: 0x040002A9 RID: 681
		[Unsaved(false)]
		public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

		// Token: 0x040002AA RID: 682
		public const string DefaultDefName = "UnnamedDef";

		// Token: 0x040002AB RID: 683
		private static Regex AllowedDefnamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");

		// Token: 0x040002AC RID: 684
		private static Regex DisallowedLabelCharsRegex = new Regex("\\[|\\]|\\{|\\}");
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000A9 RID: 169
	public class BodyPartRecord
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x0001F5CA File Offset: 0x0001D7CA
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001F5D5 File Offset: 0x0001D7D5
		public string Label
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel;
				}
				return this.def.label;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0001F5F6 File Offset: 0x0001D7F6
		public string LabelCap
		{
			get
			{
				if (this.customLabel.NullOrEmpty())
				{
					return this.def.LabelCap;
				}
				if (this.cachedCustomLabelCap == null)
				{
					this.cachedCustomLabelCap = this.customLabel.CapitalizeFirst();
				}
				return this.cachedCustomLabelCap;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0001F635 File Offset: 0x0001D835
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0001F642 File Offset: 0x0001D842
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0001F64F File Offset: 0x0001D84F
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001F660 File Offset: 0x0001D860
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"BodyPartRecord(",
				(this.def != null) ? this.def.defName : "NULL_DEF",
				" parts.Count=",
				this.parts.Count,
				")"
			});
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001F6C0 File Offset: 0x0001D8C0
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001F6D0 File Offset: 0x0001D8D0
		public bool IsInGroup(BodyPartGroupDef group)
		{
			for (int i = 0; i < this.groups.Count; i++)
			{
				if (this.groups[i] == group)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001F705 File Offset: 0x0001D905
		public IEnumerable<BodyPartRecord> GetChildParts(BodyPartTagDef tag)
		{
			if (this.def.tags.Contains(tag))
			{
				yield return this;
			}
			int num;
			for (int i = 0; i < this.parts.Count; i = num)
			{
				foreach (BodyPartRecord bodyPartRecord in this.parts[i].GetChildParts(tag))
				{
					yield return bodyPartRecord;
				}
				IEnumerator<BodyPartRecord> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001F71C File Offset: 0x0001D91C
		public IEnumerable<BodyPartRecord> GetPartAndAllChildParts()
		{
			yield return this;
			int num;
			for (int i = 0; i < this.parts.Count; i = num)
			{
				foreach (BodyPartRecord bodyPartRecord in this.parts[i].GetPartAndAllChildParts())
				{
					yield return bodyPartRecord;
				}
				IEnumerator<BodyPartRecord> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001F72C File Offset: 0x0001D92C
		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			int num;
			for (int i = 0; i < this.parts.Count; i = num)
			{
				yield return this.parts[i];
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001F73C File Offset: 0x0001D93C
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001F74A File Offset: 0x0001D94A
		public IEnumerable<BodyPartRecord> GetConnectedParts(BodyPartTagDef tag)
		{
			BodyPartRecord bodyPartRecord = this;
			while (bodyPartRecord.parent != null && bodyPartRecord.parent.def.tags.Contains(tag))
			{
				bodyPartRecord = bodyPartRecord.parent;
			}
			foreach (BodyPartRecord bodyPartRecord2 in bodyPartRecord.GetChildParts(tag))
			{
				yield return bodyPartRecord2;
			}
			IEnumerator<BodyPartRecord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040002AE RID: 686
		public BodyDef body;

		// Token: 0x040002AF RID: 687
		[TranslationHandle]
		public BodyPartDef def;

		// Token: 0x040002B0 RID: 688
		[MustTranslate]
		public string customLabel;

		// Token: 0x040002B1 RID: 689
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;

		// Token: 0x040002B2 RID: 690
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		// Token: 0x040002B3 RID: 691
		public BodyPartHeight height;

		// Token: 0x040002B4 RID: 692
		public BodyPartDepth depth;

		// Token: 0x040002B5 RID: 693
		public float coverage = 1f;

		// Token: 0x040002B6 RID: 694
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		// Token: 0x040002B7 RID: 695
		[NoTranslate]
		public string woundAnchorTag;

		// Token: 0x040002B8 RID: 696
		[Unsaved(false)]
		public BodyPartRecord parent;

		// Token: 0x040002B9 RID: 697
		[Unsaved(false)]
		public float coverageAbsWithChildren;

		// Token: 0x040002BA RID: 698
		[Unsaved(false)]
		public float coverageAbs;

		// Token: 0x040002BB RID: 699
		[Unsaved(false)]
		private string cachedCustomLabelCap;
	}
}

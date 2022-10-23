using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000AA RID: 170
	public class BodyDef : Def
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001F78A File Offset: 0x0001D98A
		public List<BodyPartRecord> AllParts
		{
			get
			{
				return this.cachedAllParts;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0001F792 File Offset: 0x0001D992
		public List<BodyPartRecord> AllPartsVulnerableToFrostbite
		{
			get
			{
				return this.cachedPartsVulnerableToFrostbite;
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001F79C File Offset: 0x0001D99C
		public List<BodyPartRecord> GetPartsWithTag(BodyPartTagDef tag)
		{
			if (!this.cachedPartsByTag.ContainsKey(tag))
			{
				this.cachedPartsByTag[tag] = new List<BodyPartRecord>();
				for (int i = 0; i < this.AllParts.Count; i++)
				{
					BodyPartRecord bodyPartRecord = this.AllParts[i];
					if (bodyPartRecord.def.tags.Contains(tag))
					{
						this.cachedPartsByTag[tag].Add(bodyPartRecord);
					}
				}
			}
			return this.cachedPartsByTag[tag];
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001F81C File Offset: 0x0001DA1C
		public List<BodyPartRecord> GetPartsWithDef(BodyPartDef def)
		{
			if (!this.cachedPartsByDef.ContainsKey(def))
			{
				this.cachedPartsByDef[def] = new List<BodyPartRecord>();
				for (int i = 0; i < this.AllParts.Count; i++)
				{
					BodyPartRecord bodyPartRecord = this.AllParts[i];
					if (bodyPartRecord.def == def)
					{
						this.cachedPartsByDef[def].Add(bodyPartRecord);
					}
				}
			}
			return this.cachedPartsByDef[def];
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001F894 File Offset: 0x0001DA94
		public bool HasPartWithTag(BodyPartTagDef tag)
		{
			for (int i = 0; i < this.AllParts.Count; i++)
			{
				if (this.AllParts[i].def.tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001F8D8 File Offset: 0x0001DAD8
		public BodyPartRecord GetPartAtIndex(int index)
		{
			if (index < 0 || index >= this.cachedAllParts.Count)
			{
				return null;
			}
			return this.cachedAllParts[index];
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001F8FC File Offset: 0x0001DAFC
		public int GetIndexOfPart(BodyPartRecord rec)
		{
			for (int i = 0; i < this.cachedAllParts.Count; i++)
			{
				if (this.cachedAllParts[i] == rec)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001F931 File Offset: 0x0001DB31
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.cachedPartsVulnerableToFrostbite.NullOrEmpty<BodyPartRecord>())
			{
				yield return "no parts vulnerable to frostbite";
			}
			foreach (BodyPartRecord bodyPartRecord in this.AllParts)
			{
				if (bodyPartRecord.def.conceptual && bodyPartRecord.coverageAbs != 0f)
				{
					yield return string.Format("part {0} is tagged conceptual, but has nonzero coverage", bodyPartRecord);
				}
				else if (Prefs.DevMode && !bodyPartRecord.def.conceptual)
				{
					float num = 0f;
					foreach (BodyPartRecord bodyPartRecord2 in bodyPartRecord.parts)
					{
						num += bodyPartRecord2.coverage;
					}
					if (num >= 1f)
					{
						Log.Warning(string.Concat(new string[]
						{
							"BodyDef ",
							this.defName,
							" has BodyPartRecord of ",
							bodyPartRecord.def.defName,
							" whose children have more or equal coverage than 100% (",
							(num * 100f).ToString("0.00"),
							"%)"
						}));
					}
				}
			}
			List<BodyPartRecord>.Enumerator enumerator2 = default(List<BodyPartRecord>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001F944 File Offset: 0x0001DB44
		public override void ResolveReferences()
		{
			if (this.corePart != null)
			{
				this.CacheDataRecursive(this.corePart);
			}
			this.cachedPartsVulnerableToFrostbite = new List<BodyPartRecord>();
			List<BodyPartRecord> allParts = this.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				if (allParts[i].def.frostbiteVulnerability > 0f)
				{
					this.cachedPartsVulnerableToFrostbite.Add(allParts[i]);
				}
			}
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001F9B4 File Offset: 0x0001DBB4
		private void CacheDataRecursive(BodyPartRecord node)
		{
			if (node.def == null)
			{
				Log.Error("BodyPartRecord with null def. body=" + this);
				return;
			}
			node.body = this;
			for (int i = 0; i < node.parts.Count; i++)
			{
				node.parts[i].parent = node;
			}
			if (node.parent != null)
			{
				node.coverageAbsWithChildren = node.parent.coverageAbsWithChildren * node.coverage;
			}
			else
			{
				node.coverageAbsWithChildren = 1f;
			}
			float num = 1f;
			for (int j = 0; j < node.parts.Count; j++)
			{
				num -= node.parts[j].coverage;
			}
			if (Mathf.Abs(num) < 1E-05f)
			{
				num = 0f;
			}
			if (num <= 0f)
			{
				num = 0f;
			}
			node.coverageAbs = node.coverageAbsWithChildren * num;
			if (node.height == BodyPartHeight.Undefined)
			{
				node.height = BodyPartHeight.Middle;
			}
			if (node.depth == BodyPartDepth.Undefined)
			{
				node.depth = BodyPartDepth.Outside;
			}
			for (int k = 0; k < node.parts.Count; k++)
			{
				if (node.parts[k].height == BodyPartHeight.Undefined)
				{
					node.parts[k].height = node.height;
				}
				if (node.parts[k].depth == BodyPartDepth.Undefined)
				{
					node.parts[k].depth = node.depth;
				}
			}
			this.cachedAllParts.Add(node);
			for (int l = 0; l < node.parts.Count; l++)
			{
				this.CacheDataRecursive(node.parts[l]);
			}
		}

		// Token: 0x040002BC RID: 700
		public BodyPartRecord corePart;

		// Token: 0x040002BD RID: 701
		[Unsaved(false)]
		private List<BodyPartRecord> cachedAllParts = new List<BodyPartRecord>();

		// Token: 0x040002BE RID: 702
		[Unsaved(false)]
		private List<BodyPartRecord> cachedPartsVulnerableToFrostbite;

		// Token: 0x040002BF RID: 703
		[Unsaved(false)]
		public Dictionary<BodyPartTagDef, List<BodyPartRecord>> cachedPartsByTag = new Dictionary<BodyPartTagDef, List<BodyPartRecord>>();

		// Token: 0x040002C0 RID: 704
		[Unsaved(false)]
		public Dictionary<BodyPartDef, List<BodyPartRecord>> cachedPartsByDef = new Dictionary<BodyPartDef, List<BodyPartRecord>>();
	}
}

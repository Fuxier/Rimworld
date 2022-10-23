using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200033A RID: 826
	public class HediffWithComps : Hediff
	{
		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x000824E4 File Offset: 0x000806E4
		public override string LabelBase
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				for (;;)
				{
					int num2 = num;
					List<HediffComp> list = this.comps;
					int? num3 = (list != null) ? new int?(list.Count) : null;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					string compLabelPrefix = this.comps[num].CompLabelPrefix;
					if (!compLabelPrefix.NullOrEmpty())
					{
						stringBuilder.Append(compLabelPrefix);
						stringBuilder.Append(" ");
					}
					num++;
				}
				stringBuilder.Append(base.LabelBase);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001603 RID: 5635 RVA: 0x00082578 File Offset: 0x00080778
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				int num = 0;
				for (;;)
				{
					int num2 = num;
					List<HediffComp> list = this.comps;
					int? num3 = (list != null) ? new int?(list.Count) : null;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					string compLabelInBracketsExtra = this.comps[num].CompLabelInBracketsExtra;
					if (!compLabelInBracketsExtra.NullOrEmpty())
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(compLabelInBracketsExtra);
					}
					num++;
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001604 RID: 5636 RVA: 0x00082614 File Offset: 0x00080814
		public override bool ShouldRemove
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompShouldRemove)
						{
							return true;
						}
					}
				}
				return base.ShouldRemove;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001605 RID: 5637 RVA: 0x0008265C File Offset: 0x0008085C
		public override bool Visible
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompDisallowVisible())
						{
							return false;
						}
					}
				}
				return base.Visible;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x000826A4 File Offset: 0x000808A4
		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.TipStringExtra);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string compTipStringExtra = this.comps[i].CompTipStringExtra;
						if (!compTipStringExtra.NullOrEmpty())
						{
							stringBuilder.AppendLine(compTipStringExtra);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x0008270C File Offset: 0x0008090C
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(base.Description);
				int num = 0;
				for (;;)
				{
					int num2 = num;
					List<HediffComp> list = this.comps;
					int? num3 = (list != null) ? new int?(list.Count) : null;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					string compDescriptionExtra = this.comps[num].CompDescriptionExtra;
					if (!compDescriptionExtra.NullOrEmpty())
					{
						stringBuilder.Append(" ");
						stringBuilder.Append(compDescriptionExtra);
					}
					num++;
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x00082798 File Offset: 0x00080998
		public override TextureAndColor StateIcon
		{
			get
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					TextureAndColor compStateIcon = this.comps[i].CompStateIcon;
					if (compStateIcon.HasValue)
					{
						return compStateIcon;
					}
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x000827DD File Offset: 0x000809DD
		public override IEnumerable<Gizmo> GetGizmos()
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				IEnumerable<Gizmo> enumerable = this.comps[i].CompGetGizmos();
				if (enumerable != null)
				{
					foreach (Gizmo gizmo in enumerable)
					{
						yield return gizmo;
					}
					IEnumerator<Gizmo> enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x000827F0 File Offset: 0x000809F0
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostPostAdd(dinfo);
				}
			}
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00082834 File Offset: 0x00080A34
		public override void PostRemoved()
		{
			base.PostRemoved();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostPostRemoved();
				}
			}
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x00082878 File Offset: 0x00080A78
		public override void PostTick()
		{
			base.PostTick();
			if (this.comps != null)
			{
				float num = 0f;
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostTick(ref num);
				}
				if (num != 0f)
				{
					this.Severity += num;
				}
			}
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x000828D8 File Offset: 0x00080AD8
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompExposeData();
				}
			}
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00082928 File Offset: 0x00080B28
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTended(quality, maxQuality, batchPosition);
			}
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x00082960 File Offset: 0x00080B60
		public override bool TryMergeWith(Hediff other)
		{
			if (base.TryMergeWith(other))
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostMerged(other);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x000829A4 File Offset: 0x00080BA4
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			for (int i = this.comps.Count - 1; i >= 0; i--)
			{
				this.comps[i].Notify_PawnDied();
			}
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x000829E0 File Offset: 0x00080BE0
		public override void Notify_PawnKilled()
		{
			base.Notify_PawnKilled();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnKilled();
			}
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00082A1C File Offset: 0x00080C1C
		public override void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
		{
			base.Notify_KilledPawn(victim, dinfo);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_KilledPawn(victim, dinfo);
			}
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00082A5C File Offset: 0x00080C5C
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			}
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00082A9C File Offset: 0x00080C9C
		public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompModifyChemicalEffect(chem, ref effect);
			}
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00082AD4 File Offset: 0x00080CD4
		public override void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
			base.Notify_PawnUsedVerb(verb, target);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnUsedVerb(verb, target);
			}
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00082B14 File Offset: 0x00080D14
		public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing src = null)
		{
			base.Notify_EntropyGained(baseAmount, finalAmount, src);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_EntropyGained(baseAmount, finalAmount, src);
			}
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00082B54 File Offset: 0x00080D54
		public override void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
			base.Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
			}
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x00082B94 File Offset: 0x00080D94
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			for (int i = this.comps.Count - 1; i >= 0; i--)
			{
				try
				{
					this.comps[i].CompPostMake();
				}
				catch (Exception arg)
				{
					Log.Error("Error in HediffComp.CompPostMake(): " + arg);
					this.comps.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x00082C08 File Offset: 0x00080E08
		private void InitializeComps()
		{
			if (this.def.comps != null)
			{
				this.comps = new List<HediffComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					HediffComp hediffComp = null;
					try
					{
						hediffComp = (HediffComp)Activator.CreateInstance(this.def.comps[i].compClass);
						hediffComp.props = this.def.comps[i];
						hediffComp.parent = this;
						this.comps.Add(hediffComp);
					}
					catch (Exception arg)
					{
						Log.Error("Could not instantiate or initialize a HediffComp: " + arg);
						this.comps.Remove(hediffComp);
					}
				}
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x00082CD0 File Offset: 0x00080ED0
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.DebugString());
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					string str;
					if (this.comps[i].ToString().Contains('_'))
					{
						str = this.comps[i].ToString().Split(new char[]
						{
							'_'
						})[1];
					}
					else
					{
						str = this.comps[i].ToString();
					}
					stringBuilder.AppendLine("--" + str);
					string text = this.comps[i].CompDebugString();
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text.TrimEnd(Array.Empty<char>()).Indented("    "));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400117E RID: 4478
		public List<HediffComp> comps = new List<HediffComp>();
	}
}

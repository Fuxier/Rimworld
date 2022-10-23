using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200035F RID: 863
	public class ImmunityHandler : IExposable
	{
		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x00087EA9 File Offset: 0x000860A9
		public List<ImmunityRecord> ImmunityListForReading
		{
			get
			{
				return this.immunityList;
			}
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x00087EB1 File Offset: 0x000860B1
		public ImmunityHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x00087ECB File Offset: 0x000860CB
		public void ExposeData()
		{
			Scribe_Collections.Look<ImmunityRecord>(ref this.immunityList, "imList", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x00087EE4 File Offset: 0x000860E4
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, BodyPartRecord part = null)
		{
			HediffDef hediffDef = null;
			return this.DiseaseContractChanceFactor(diseaseDef, out hediffDef, part);
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x00087F00 File Offset: 0x00086100
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, out HediffDef immunityCause, BodyPartRecord part = null)
		{
			immunityCause = null;
			if (!this.pawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo(diseaseDef, out hediff))
			{
				immunityCause = hediff.def;
				return 0f;
			}
			if (this.AnyGeneMakesFullyImmuneTo(diseaseDef))
			{
				return 0f;
			}
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def == diseaseDef && hediffs[i].Part == part)
				{
					return 0f;
				}
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				if (this.immunityList[j].hediffDef == diseaseDef)
				{
					immunityCause = this.immunityList[j].source;
					return Mathf.Lerp(1f, 0f, this.immunityList[j].immunity / 0.6f);
				}
			}
			return 1f;
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x00088004 File Offset: 0x00086204
		public float GetImmunity(HediffDef def, bool naturalImmunityOnly = false)
		{
			float num = 0f;
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				ImmunityRecord immunityRecord = this.immunityList[i];
				if (immunityRecord.hediffDef == def)
				{
					num = immunityRecord.immunity;
					break;
				}
			}
			Hediff hediff;
			if (!naturalImmunityOnly && (this.AnyHediffMakesFullyImmuneTo(def, out hediff) || this.AnyGeneMakesFullyImmuneTo(def)) && num < 0.65000004f)
			{
				num = 0.65000004f;
			}
			return num;
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00088074 File Offset: 0x00086274
		internal void ImmunityHandlerTick()
		{
			List<ImmunityHandler.ImmunityInfo> list = this.NeededImmunitiesNow();
			for (int i = 0; i < list.Count; i++)
			{
				this.TryAddImmunityRecord(list[i].immunity, list[i].source);
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				ImmunityRecord immunityRecord = this.immunityList[j];
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(immunityRecord.hediffDef, false);
				immunityRecord.ImmunityTick(this.pawn, firstHediffOfDef != null, firstHediffOfDef);
			}
			for (int k = this.immunityList.Count - 1; k >= 0; k--)
			{
				if (this.immunityList[k].immunity <= 0f)
				{
					bool flag = false;
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].immunity == this.immunityList[k].hediffDef)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.immunityList.RemoveAt(k);
					}
				}
			}
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x00088190 File Offset: 0x00086390
		private List<ImmunityHandler.ImmunityInfo> NeededImmunitiesNow()
		{
			ImmunityHandler.tmpNeededImmunitiesNow.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff hediff = hediffs[i];
				if (hediff.def.PossibleToDevelopImmunityNaturally())
				{
					ImmunityHandler.tmpNeededImmunitiesNow.Add(new ImmunityHandler.ImmunityInfo
					{
						immunity = hediff.def,
						source = hediff.def
					});
				}
			}
			return ImmunityHandler.tmpNeededImmunitiesNow;
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x00088218 File Offset: 0x00086418
		private bool AnyHediffMakesFullyImmuneTo(HediffDef def, out Hediff sourceHediff)
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.makeImmuneTo != null)
				{
					for (int j = 0; j < curStage.makeImmuneTo.Count; j++)
					{
						if (curStage.makeImmuneTo[j] == def)
						{
							sourceHediff = hediffs[i];
							return true;
						}
					}
				}
			}
			sourceHediff = null;
			return false;
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x00088298 File Offset: 0x00086498
		public bool AnyGeneMakesFullyImmuneTo(HediffDef def)
		{
			if (!ModsConfig.BiotechActive || this.pawn.genes == null)
			{
				return false;
			}
			for (int i = 0; i < this.pawn.genes.GenesListForReading.Count; i++)
			{
				Gene gene = this.pawn.genes.GenesListForReading[i];
				if (gene.def.makeImmuneTo != null)
				{
					for (int j = 0; j < gene.def.makeImmuneTo.Count; j++)
					{
						if (gene.def.makeImmuneTo[j] == def)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x00088334 File Offset: 0x00086534
		private void TryAddImmunityRecord(HediffDef def, HediffDef source)
		{
			if (def.CompProps<HediffCompProperties_Immunizable>() == null)
			{
				return;
			}
			if (this.ImmunityRecordExists(def))
			{
				return;
			}
			ImmunityRecord immunityRecord = new ImmunityRecord();
			immunityRecord.hediffDef = def;
			immunityRecord.source = source;
			this.immunityList.Add(immunityRecord);
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x00088374 File Offset: 0x00086574
		public ImmunityRecord GetImmunityRecord(HediffDef def)
		{
			ImmunityRecord immunityRecord = null;
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				if (this.immunityList[i].hediffDef == def)
				{
					immunityRecord = this.immunityList[i];
					break;
				}
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo(def, out hediff) && (immunityRecord == null || immunityRecord.immunity < 0.65000004f))
			{
				immunityRecord = new ImmunityRecord
				{
					immunity = 0.65000004f,
					hediffDef = def,
					source = hediff.def
				};
			}
			return immunityRecord;
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x000883FC File Offset: 0x000865FC
		public bool ImmunityRecordExists(HediffDef def)
		{
			return this.GetImmunityRecord(def) != null;
		}

		// Token: 0x040011C9 RID: 4553
		public Pawn pawn;

		// Token: 0x040011CA RID: 4554
		private List<ImmunityRecord> immunityList = new List<ImmunityRecord>();

		// Token: 0x040011CB RID: 4555
		private const float ForcedImmunityLevel = 0.65000004f;

		// Token: 0x040011CC RID: 4556
		private static List<ImmunityHandler.ImmunityInfo> tmpNeededImmunitiesNow = new List<ImmunityHandler.ImmunityInfo>();

		// Token: 0x02001E23 RID: 7715
		public struct ImmunityInfo
		{
			// Token: 0x040076EF RID: 30447
			public HediffDef immunity;

			// Token: 0x040076F0 RID: 30448
			public HediffDef source;
		}
	}
}

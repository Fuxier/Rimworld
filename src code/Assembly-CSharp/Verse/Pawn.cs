using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000367 RID: 871
	public class Pawn : ThingWithComps, IStrippable, IBillGiver, IVerbOwner, ITrader, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher, IThingHolder
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001773 RID: 6003 RVA: 0x000897EB File Offset: 0x000879EB
		// (set) Token: 0x06001774 RID: 6004 RVA: 0x000897F3 File Offset: 0x000879F3
		public Name Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001775 RID: 6005 RVA: 0x000897FC File Offset: 0x000879FC
		public RaceProperties RaceProps
		{
			get
			{
				return this.def.race;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001776 RID: 6006 RVA: 0x00089809 File Offset: 0x00087A09
		public Job CurJob
		{
			get
			{
				if (this.jobs == null)
				{
					return null;
				}
				return this.jobs.curJob;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001777 RID: 6007 RVA: 0x00089820 File Offset: 0x00087A20
		public JobDef CurJobDef
		{
			get
			{
				if (this.CurJob == null)
				{
					return null;
				}
				return this.CurJob.def;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001778 RID: 6008 RVA: 0x00089837 File Offset: 0x00087A37
		public bool Downed
		{
			get
			{
				return this.health.Downed;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x00089844 File Offset: 0x00087A44
		public bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x0600177A RID: 6010 RVA: 0x00089851 File Offset: 0x00087A51
		public string KindLabel
		{
			get
			{
				return GenLabel.BestKindLabel(this, false, false, false, -1);
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600177B RID: 6011 RVA: 0x0008985D File Offset: 0x00087A5D
		public bool InMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState;
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x00089879 File Offset: 0x00087A79
		public MentalState MentalState
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.mentalStateHandler.CurState;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x00089895 File Offset: 0x00087A95
		public MentalStateDef MentalStateDef
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.mentalStateHandler.CurStateDef;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600177E RID: 6014 RVA: 0x000898B1 File Offset: 0x00087AB1
		public bool InAggroMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState && this.mindState.mentalStateHandler.CurStateDef.IsAggro;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x000898E6 File Offset: 0x00087AE6
		public bool Inspired
		{
			get
			{
				if (this.Dead)
				{
					return false;
				}
				Pawn_MindState pawn_MindState = this.mindState;
				return ((pawn_MindState != null) ? pawn_MindState.inspirationHandler : null) != null && this.mindState.inspirationHandler.Inspired;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001780 RID: 6016 RVA: 0x00089918 File Offset: 0x00087B18
		public Inspiration Inspiration
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.inspirationHandler.CurState;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x00089934 File Offset: 0x00087B34
		public InspirationDef InspirationDef
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.inspirationHandler.CurStateDef;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001782 RID: 6018 RVA: 0x00089950 File Offset: 0x00087B50
		public override Vector3 DrawPos
		{
			get
			{
				return this.Drawer.DrawPos;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x0008995D File Offset: 0x00087B5D
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x00089965 File Offset: 0x00087B65
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.def.Verbs;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001785 RID: 6021 RVA: 0x00089972 File Offset: 0x00087B72
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001786 RID: 6022 RVA: 0x0008997F File Offset: 0x00087B7F
		public bool ShouldAvoidFences
		{
			get
			{
				return this.def.race.FenceBlocked || this.roping.AnyRopeesFenceBlocked;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001787 RID: 6023 RVA: 0x000899A0 File Offset: 0x00087BA0
		public bool IsColonist
		{
			get
			{
				return base.Faction != null && base.Faction.IsPlayer && this.RaceProps.Humanlike && (!this.IsSlave || this.guest.SlaveIsSecure);
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x000899DB File Offset: 0x00087BDB
		public bool IsFreeColonist
		{
			get
			{
				return this.IsColonist && this.HostFaction == null;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x000899F0 File Offset: 0x00087BF0
		public bool IsFreeNonSlaveColonist
		{
			get
			{
				return this.IsFreeColonist && !this.IsSlave;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x00089A05 File Offset: 0x00087C05
		public Faction HostFaction
		{
			get
			{
				if (this.guest == null)
				{
					return null;
				}
				return this.guest.HostFaction;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x00089A1C File Offset: 0x00087C1C
		public Faction SlaveFaction
		{
			get
			{
				Pawn_GuestTracker pawn_GuestTracker = this.guest;
				if (pawn_GuestTracker == null)
				{
					return null;
				}
				return pawn_GuestTracker.SlaveFaction;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x00089A2F File Offset: 0x00087C2F
		public Ideo Ideo
		{
			get
			{
				if (this.ideo == null)
				{
					return null;
				}
				return this.ideo.Ideo;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x00089A46 File Offset: 0x00087C46
		public bool Drafted
		{
			get
			{
				return this.drafter != null && this.drafter.Drafted;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x0600178E RID: 6030 RVA: 0x00089A5D File Offset: 0x00087C5D
		public bool IsPrisoner
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x00089A74 File Offset: 0x00087C74
		public bool IsPrisonerOfColony
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner && this.guest.HostFaction.IsPlayer;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x00089A9D File Offset: 0x00087C9D
		public bool IsSlave
		{
			get
			{
				return this.guest != null && this.guest.IsSlave;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x00089AB4 File Offset: 0x00087CB4
		public bool IsSlaveOfColony
		{
			get
			{
				return this.IsSlave && base.Faction.IsPlayer;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001792 RID: 6034 RVA: 0x00089ACC File Offset: 0x00087CCC
		public DevelopmentalStage DevelopmentalStage
		{
			get
			{
				Pawn_AgeTracker pawn_AgeTracker = this.ageTracker;
				DevelopmentalStage? developmentalStage;
				if (pawn_AgeTracker == null)
				{
					developmentalStage = null;
				}
				else
				{
					LifeStageDef curLifeStage = pawn_AgeTracker.CurLifeStage;
					developmentalStage = ((curLifeStage != null) ? new DevelopmentalStage?(curLifeStage.developmentalStage) : null);
				}
				DevelopmentalStage? developmentalStage2 = developmentalStage;
				if (developmentalStage2 == null)
				{
					return DevelopmentalStage.Adult;
				}
				return developmentalStage2.GetValueOrDefault();
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001793 RID: 6035 RVA: 0x00089B20 File Offset: 0x00087D20
		public GuestStatus? GuestStatus
		{
			get
			{
				if (this.guest != null && (this.HostFaction != null || this.guest.GuestStatus != RimWorld.GuestStatus.Guest))
				{
					return new GuestStatus?(this.guest.GuestStatus);
				}
				return null;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001794 RID: 6036 RVA: 0x00089B64 File Offset: 0x00087D64
		public bool IsColonistPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonist && this.MentalStateDef == null && (this.HostFaction == null || this.IsSlave);
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001795 RID: 6037 RVA: 0x00089B90 File Offset: 0x00087D90
		public bool IsColonyMech
		{
			get
			{
				return ModsConfig.BiotechActive && this.RaceProps.IsMechanoid && base.Faction == Faction.OfPlayer && this.MentalStateDef == null && (this.HostFaction == null || this.IsSlave);
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x00089BD0 File Offset: 0x00087DD0
		public bool IsColonyMechPlayerControlled
		{
			get
			{
				if (base.Spawned && this.IsColonyMech)
				{
					CompOverseerSubject compOverseerSubject = this.TryGetComp<CompOverseerSubject>();
					return compOverseerSubject != null && compOverseerSubject.State == OverseerSubjectState.Overseen;
				}
				return false;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001797 RID: 6039 RVA: 0x00089C04 File Offset: 0x00087E04
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001798 RID: 6040 RVA: 0x00089C14 File Offset: 0x00087E14
		public bool InContainerEnclosed
		{
			get
			{
				return base.ParentHolder.IsEnclosingContainer();
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x00089C21 File Offset: 0x00087E21
		public Corpse Corpse
		{
			get
			{
				return base.ParentHolder as Corpse;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x00089C30 File Offset: 0x00087E30
		public Pawn CarriedBy
		{
			get
			{
				if (base.ParentHolder == null)
				{
					return null;
				}
				Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker != null)
				{
					return pawn_CarryTracker.pawn;
				}
				return null;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x0600179B RID: 6043 RVA: 0x00089C60 File Offset: 0x00087E60
		public override string LabelNoCount
		{
			get
			{
				if (this.Name == null)
				{
					return this.KindLabel;
				}
				if (this.story == null || this.story.TitleShortCap.NullOrEmpty())
				{
					return this.Name.ToStringShort;
				}
				return this.Name.ToStringShort + (", " + this.story.TitleShortCap).Colorize(ColoredText.SubtleGrayColor);
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x00089CD1 File Offset: 0x00087ED1
		public override string LabelShort
		{
			get
			{
				if (this.Name != null)
				{
					return this.Name.ToStringShort;
				}
				return this.LabelNoCount;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x0600179D RID: 6045 RVA: 0x00089CF0 File Offset: 0x00087EF0
		public TaggedString LabelNoCountColored
		{
			get
			{
				if (this.Name == null)
				{
					return this.KindLabel;
				}
				if (this.story == null || this.story.TitleShortCap.NullOrEmpty())
				{
					return this.Name.ToStringShort.Colorize(ColoredText.NameColor);
				}
				return this.Name.ToStringShort.Colorize(ColoredText.NameColor) + (", " + this.story.TitleShortCap).Colorize(ColoredText.SubtleGrayColor);
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x0600179E RID: 6046 RVA: 0x00089D84 File Offset: 0x00087F84
		public TaggedString NameShortColored
		{
			get
			{
				if (this.Name != null)
				{
					return this.Name.ToStringShort.Colorize(ColoredText.NameColor);
				}
				return this.KindLabel;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x0600179F RID: 6047 RVA: 0x00089DB4 File Offset: 0x00087FB4
		public TaggedString NameFullColored
		{
			get
			{
				if (this.Name != null)
				{
					return this.Name.ToStringFull.Colorize(ColoredText.NameColor);
				}
				return this.KindLabel;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060017A0 RID: 6048 RVA: 0x00089DE4 File Offset: 0x00087FE4
		public TaggedString LegalStatus
		{
			get
			{
				if (this.IsSlave)
				{
					return "Slave".Translate();
				}
				if (base.Faction != null)
				{
					return new TaggedString(base.Faction.def.pawnSingular);
				}
				return "Colonist".Translate();
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x00089E21 File Offset: 0x00088021
		public override string DescriptionDetailed
		{
			get
			{
				return this.DescriptionFlavor;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060017A2 RID: 6050 RVA: 0x00089E2C File Offset: 0x0008802C
		public override string DescriptionFlavor
		{
			get
			{
				if (this.IsBaseliner())
				{
					return this.def.description;
				}
				return "StatsReport_NonBaselinerDescription".Translate(this.genes.XenotypeLabel) + "\n\n" + this.genes.Xenotype.description;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060017A3 RID: 6051 RVA: 0x00089E8B File Offset: 0x0008808B
		public override IEnumerable<DefHyperlink> DescriptionHyperlinks
		{
			get
			{
				foreach (DefHyperlink defHyperlink in base.DescriptionHyperlinks)
				{
					yield return defHyperlink;
				}
				IEnumerator<DefHyperlink> enumerator = null;
				if (!this.IsBaseliner())
				{
					yield return new DefHyperlink(this.genes.Xenotype);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060017A4 RID: 6052 RVA: 0x00089E9B File Offset: 0x0008809B
		public Pawn_DrawTracker Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new Pawn_DrawTracker(this);
				}
				return this.drawer;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060017A5 RID: 6053 RVA: 0x00089EB8 File Offset: 0x000880B8
		public Faction HomeFaction
		{
			get
			{
				if (base.Faction == null || !base.Faction.IsPlayer)
				{
					return base.Faction;
				}
				if (this.IsSlave && this.SlaveFaction != null)
				{
					return this.SlaveFaction;
				}
				if (this.HasExtraMiniFaction(null))
				{
					return this.GetExtraMiniFaction(null);
				}
				return this.GetExtraHomeFaction(null) ?? base.Faction;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060017A6 RID: 6054 RVA: 0x00089F1A File Offset: 0x0008811A
		public bool Deathresting
		{
			get
			{
				return ModsConfig.BiotechActive && this.health.hediffSet.HasHediff(HediffDefOf.Deathrest, false);
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060017A7 RID: 6055 RVA: 0x00089F3B File Offset: 0x0008813B
		public override bool Suspended
		{
			get
			{
				return base.Suspended || Find.WorldPawns.GetSituation(this) == WorldPawnSituation.ReservedByQuest;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x00089F59 File Offset: 0x00088159
		public BillStack BillStack
		{
			get
			{
				return this.health.surgeryBills;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x00089F68 File Offset: 0x00088168
		public override IntVec3 InteractionCell
		{
			get
			{
				Building_Bed building_Bed = this.CurrentBed();
				IntVec3? intVec = (building_Bed != null) ? building_Bed.FindPreferredInteractionCell(base.Position, null) : null;
				if (intVec == null)
				{
					return base.InteractionCell;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x00089FAE File Offset: 0x000881AE
		public TraderKindDef TraderKind
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.traderKind;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x00089FC5 File Offset: 0x000881C5
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060017AC RID: 6060 RVA: 0x00089FD2 File Offset: 0x000881D2
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x00089FDF File Offset: 0x000881DF
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x00089FEC File Offset: 0x000881EC
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x00004E2A File Offset: 0x0000302A
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x0008A003 File Offset: 0x00088203
		public float BodySize
		{
			get
			{
				return this.ageTracker.CurLifeStage.bodySizeFactor * this.RaceProps.baseBodySize;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x0008A021 File Offset: 0x00088221
		public float HealthScale
		{
			get
			{
				return this.ageTracker.CurLifeStage.healthScaleFactor * this.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060017B2 RID: 6066 RVA: 0x0008A03F File Offset: 0x0008823F
		public IEnumerable<Thing> EquippedWornOrInventoryThings
		{
			get
			{
				IEnumerable<Thing> innerContainer = this.inventory.innerContainer;
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				IEnumerable<Thing> first = innerContainer.ConcatIfNotNull((pawn_ApparelTracker != null) ? pawn_ApparelTracker.WornApparel : null);
				Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
				return first.ConcatIfNotNull((pawn_EquipmentTracker != null) ? pawn_EquipmentTracker.AllEquipmentListForReading : null);
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060017B3 RID: 6067 RVA: 0x0008A07A File Offset: 0x0008827A
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060017B4 RID: 6068 RVA: 0x00025F42 File Offset: 0x00024142
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060017B5 RID: 6069 RVA: 0x0008A080 File Offset: 0x00088280
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				if (!base.Spawned)
				{
					return LocalTargetInfo.Invalid;
				}
				Stance curStance = this.stances.curStance;
				if (curStance is Stance_Warmup || curStance is Stance_Cooldown)
				{
					return ((Stance_Busy)curStance).focusTarg;
				}
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060017B6 RID: 6070 RVA: 0x0008A07A File Offset: 0x0008827A
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060017B7 RID: 6071 RVA: 0x0008A0C8 File Offset: 0x000882C8
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.mindState.lastAttackedTarget;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060017B8 RID: 6072 RVA: 0x0008A0D5 File Offset: 0x000882D5
		public int LastAttackTargetTick
		{
			get
			{
				return this.mindState.lastAttackTargetTick;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060017B9 RID: 6073 RVA: 0x0008A0E4 File Offset: 0x000882E4
		public Verb CurrentEffectiveVerb
		{
			get
			{
				Building_Turret building_Turret = this.MannedThing() as Building_Turret;
				if (building_Turret != null)
				{
					return building_Turret.AttackVerb;
				}
				return this.TryGetAttackVerb(null, !this.IsColonist, false);
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060017BA RID: 6074 RVA: 0x0008A118 File Offset: 0x00088318
		private bool ForceNoDeathNotification
		{
			get
			{
				return this.forceNoDeathNotification || this.kindDef.forceNoDeathNotification;
			}
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x0008A12F File Offset: 0x0008832F
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return base.GetUniqueLoadID();
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x0008A137 File Offset: 0x00088337
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this;
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x0008A07A File Offset: 0x0008827A
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060017BE RID: 6078 RVA: 0x0008A13D File Offset: 0x0008833D
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Bodypart;
			}
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0008A144 File Offset: 0x00088344
		public int GetRootTile()
		{
			return base.Tile;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000029B0 File Offset: 0x00000BB0
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x0008A14C File Offset: 0x0008834C
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.inventory != null)
			{
				outChildren.Add(this.inventory);
			}
			if (this.carryTracker != null)
			{
				outChildren.Add(this.carryTracker);
			}
			if (this.equipment != null)
			{
				outChildren.Add(this.equipment);
			}
			if (this.apparel != null)
			{
				outChildren.Add(this.apparel);
			}
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0008A1B5 File Offset: 0x000883B5
		public string GetKindLabelPlural(int count = -1)
		{
			return GenLabel.BestKindLabel(this, false, false, true, count);
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0008A1C1 File Offset: 0x000883C1
		public static void ResetStaticData()
		{
			Pawn.NotSurgeryReadyTrans = "NotSurgeryReady".Translate();
			Pawn.CannotReachTrans = "CannotReach".Translate();
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0008A1EC File Offset: 0x000883EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
			Scribe_Values.Look<int>(ref this.becameWorldPawnTickAbs, "becameWorldPawnTickAbs", -1, false);
			Scribe_Values.Look<bool>(ref this.teleporting, "teleporting", false, false);
			Scribe_Values.Look<int>(ref this.showNamePromptOnTick, "showNamePromptOnTick", -1, false);
			Scribe_Values.Look<int>(ref this.babyNamingDeadline, "babyNamingDeadline", -1, false);
			Scribe_Deep.Look<Name>(ref this.nameInt, "name", Array.Empty<object>());
			Scribe_Deep.Look<Pawn_MindState>(ref this.mindState, "mindState", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_JobTracker>(ref this.jobs, "jobs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StanceTracker>(ref this.stances, "stances", new object[]
			{
				this
			});
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NativeVerbs>(ref this.natives, "natives", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MeleeVerbs>(ref this.meleeVerbs, "meleeVerbs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RotationTracker>(ref this.rotationTracker, "rotationTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ApparelTracker>(ref this.apparel, "apparel", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StoryTracker>(ref this.story, "story", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_EquipmentTracker>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DraftController>(ref this.drafter, "drafter", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AgeTracker>(ref this.ageTracker, "ageTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_HealthTracker>(ref this.health, "healthTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RecordsTracker>(ref this.records, "records", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryTracker>(ref this.inventory, "inventory", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FilthTracker>(ref this.filth, "filth", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RopeTracker>(ref this.roping, "roping", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuestTracker>(ref this.guest, "guest", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuiltTracker>(ref this.guilt, "guilt", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RoyaltyTracker>(ref this.royalty, "royalty", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RelationsTracker>(ref this.relations, "social", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PsychicEntropyTracker>(ref this.psychicEntropy, "psychicEntropy", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Ownership>(ref this.ownership, "ownership", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InteractionsTracker>(ref this.interactions, "interactions", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SkillTracker>(ref this.skills, "skills", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AbilityTracker>(ref this.abilities, "abilities", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_IdeoTracker>(ref this.ideo, "ideo", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_WorkSettings>(ref this.workSettings, "workSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_OutfitTracker>(ref this.outfits, "outfits", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DrugPolicyTracker>(ref this.drugs, "drugs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FoodRestrictionTracker>(ref this.foodRestriction, "foodRestriction", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TimetableTracker>(ref this.timetable, "timetable", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PlayerSettings>(ref this.playerSettings, "playerSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TrainingTracker>(ref this.training, "training", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StyleTracker>(ref this.style, "style", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StyleObserverTracker>(ref this.styleObserver, "styleObserver", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ConnectionsTracker>(ref this.connections, "connections", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryStockTracker>(ref this.inventoryStock, "inventoryStock", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SurroundingsTracker>(ref this.surroundings, "treeSightings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Thinker>(ref this.thinker, "thinker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MechanitorTracker>(ref this.mechanitor, "mechanitor", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GeneTracker>(ref this.genes, "genes", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_LearningTracker>(ref this.learning, "learning", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0008A74C File Offset: 0x0008894C
		public override string ToString()
		{
			if (this.story != null)
			{
				return this.LabelShort;
			}
			if (this.thingIDNumber > 0)
			{
				return base.ThingID;
			}
			if (this.kindDef != null)
			{
				return this.KindLabel + "_" + base.ThingID;
			}
			if (this.def != null)
			{
				return base.ThingID;
			}
			return base.GetType().ToString();
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x0008A7B4 File Offset: 0x000889B4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Dead)
			{
				Log.Warning("Tried to spawn Dead Pawn " + this.ToStringSafe<Pawn>() + ". Replacing with corpse.");
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				GenSpawn.Spawn(corpse, base.Position, map, WipeMode.Vanish);
				return;
			}
			if (this.def == null || this.kindDef == null)
			{
				Log.Warning("Tried to spawn pawn without def " + this.ToStringSafe<Pawn>() + ".");
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (Find.WorldPawns.Contains(this))
			{
				Find.WorldPawns.RemovePawn(this);
			}
			PawnComponentsUtility.AddComponentsForSpawn(this);
			if (!PawnUtility.InValidState(this))
			{
				Log.Error("Pawn " + this.ToStringSafe<Pawn>() + " spawned in invalid state. Destroying...");
				try
				{
					this.DeSpawn(DestroyMode.Vanish);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to despawn ",
						this.ToStringSafe<Pawn>(),
						" because of the previous error but couldn't: ",
						ex
					}));
				}
				Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Discard);
				return;
			}
			this.Drawer.Notify_Spawned();
			this.rotationTracker.Notify_Spawned();
			if (!respawningAfterLoad)
			{
				this.pather.ResetToCurrentPosition();
			}
			base.Map.mapPawns.RegisterPawn(this);
			base.Map.autoSlaughterManager.Notify_PawnSpawned();
			if (this.RaceProps.IsFlesh)
			{
				this.relations.everSeenByPlayer = true;
			}
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
			if (this.needs != null && this.needs.mood != null && this.needs.mood.recentMemory != null)
			{
				this.needs.mood.recentMemory.Notify_Spawned(respawningAfterLoad);
			}
			if (this.equipment != null)
			{
				this.equipment.Notify_PawnSpawned();
			}
			if (this.mechanitor != null)
			{
				this.mechanitor.Notify_PawnSpawned(respawningAfterLoad);
			}
			if (base.Faction == Faction.OfPlayer)
			{
				Ideo ideo = this.Ideo;
				if (ideo != null)
				{
					ideo.RecacheColonistBelieverCount();
				}
			}
			if (!respawningAfterLoad)
			{
				Find.GameEnder.CheckOrUpdateGameOver();
				if (base.Faction == Faction.OfPlayer)
				{
					Find.StoryWatcher.statsRecord.UpdateGreatestPopulation();
					Find.World.StoryState.RecordPopulationIncrease();
				}
				PawnDiedOrDownedThoughtsUtility.RemoveDiedThoughts(this);
				if (this.IsQuestLodger())
				{
					for (int i = this.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
					{
						if (this.health.hediffSet.hediffs[i].def.removeOnQuestLodgers)
						{
							this.health.RemoveHediff(this.health.hediffSet.hediffs[i]);
						}
					}
				}
			}
			if (this.RaceProps.soundAmbience != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.sustainerAmbient = this.RaceProps.soundAmbience.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				});
			}
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x0008AA8C File Offset: 0x00088C8C
		public override void PostMapInit()
		{
			base.PostMapInit();
			this.pather.TryResumePathingAfterLoading();
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0008AA9F File Offset: 0x00088C9F
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Drawer.DrawAt(drawLoc);
			Pawn_MechanitorTracker pawn_MechanitorTracker = this.mechanitor;
			if (pawn_MechanitorTracker == null)
			{
				return;
			}
			pawn_MechanitorTracker.DrawCommandRadius();
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0008AAC0 File Offset: 0x00088CC0
		public override void DrawGUIOverlay()
		{
			this.Drawer.ui.DrawPawnGUIOverlay();
			for (int i = 0; i < base.AllComps.Count; i++)
			{
				base.AllComps[i].DrawGUIOverlay();
			}
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0008AB04 File Offset: 0x00088D04
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsColonistPlayerControlled || this.IsColonyMechPlayerControlled)
			{
				if (this.pather.curPath != null)
				{
					this.pather.curPath.DrawPath(this);
				}
				this.jobs.DrawLinesBetweenTargets();
			}
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0008AB50 File Offset: 0x00088D50
		public override void TickRare()
		{
			base.TickRare();
			if (!this.Suspended)
			{
				if (this.apparel != null)
				{
					this.apparel.ApparelTrackerTickRare();
				}
				this.inventory.InventoryTrackerTickRare();
			}
			if (this.training != null)
			{
				this.training.TrainingTrackerTickRare();
			}
			if (base.Spawned && this.RaceProps.IsFlesh)
			{
				GenTemperature.PushHeat(this, 0.3f * this.BodySize * 4.1666665f * (this.def.race.Humanlike ? 1f : 0.6f));
			}
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0008ABE8 File Offset: 0x00088DE8
		public override void Tick()
		{
			if (DebugSettings.noAnimals && base.Spawned && this.RaceProps.Animal)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.TickRare();
			}
			bool suspended;
			using (new ProfilerBlock("Suspended"))
			{
				suspended = this.Suspended;
			}
			if (!suspended)
			{
				if (base.Spawned)
				{
					this.pather.PatherTick();
				}
				if (base.Spawned)
				{
					this.stances.StanceTrackerTick();
					this.verbTracker.VerbsTick();
				}
				if (base.Spawned)
				{
					this.roping.RopingTick();
					this.natives.NativeVerbsTick();
				}
				if (!this.IsWorldPawn())
				{
					Pawn_JobTracker pawn_JobTracker = this.jobs;
					if (pawn_JobTracker != null)
					{
						pawn_JobTracker.JobTrackerTick();
					}
				}
				this.health.HealthTick();
				if (!this.Dead)
				{
					this.mindState.MindStateTick();
					this.carryTracker.CarryHandsTick();
					if (this.showNamePromptOnTick != -1 && this.showNamePromptOnTick == Find.TickManager.TicksGame)
					{
						Find.WindowStack.Add(this.NamePawnDialog(null));
					}
				}
			}
			if (!this.Dead)
			{
				this.needs.NeedsTrackerTick();
			}
			if (!suspended)
			{
				if (this.equipment != null)
				{
					this.equipment.EquipmentTrackerTick();
				}
				if (this.apparel != null)
				{
					this.apparel.ApparelTrackerTick();
				}
				if (this.interactions != null && base.Spawned)
				{
					this.interactions.InteractionsTrackerTick();
				}
				if (this.caller != null)
				{
					this.caller.CallTrackerTick();
				}
				if (this.skills != null)
				{
					this.skills.SkillsTick();
				}
				if (this.abilities != null)
				{
					this.abilities.AbilitiesTick();
				}
				if (this.inventory != null)
				{
					this.inventory.InventoryTrackerTick();
				}
				if (this.drafter != null)
				{
					this.drafter.DraftControllerTick();
				}
				if (this.relations != null)
				{
					this.relations.RelationsTrackerTick();
				}
				if (ModsConfig.RoyaltyActive && this.psychicEntropy != null)
				{
					this.psychicEntropy.PsychicEntropyTrackerTick();
				}
				if (this.RaceProps.Humanlike)
				{
					this.guest.GuestTrackerTick();
				}
				if (this.ideo != null)
				{
					this.ideo.IdeoTrackerTick();
				}
				if (this.genes != null)
				{
					this.genes.GeneTrackerTick();
				}
				if (this.royalty != null && ModsConfig.RoyaltyActive)
				{
					this.royalty.RoyaltyTrackerTick();
				}
				if (this.style != null && ModsConfig.IdeologyActive)
				{
					this.style.StyleTrackerTick();
				}
				if (this.styleObserver != null && ModsConfig.IdeologyActive)
				{
					this.styleObserver.StyleObserverTick();
				}
				if (this.surroundings != null && ModsConfig.IdeologyActive)
				{
					this.surroundings.SurroundingsTrackerTick();
				}
				if (ModsConfig.BiotechActive && this.learning != null)
				{
					this.learning.LearningTick();
				}
				if (ModsConfig.BiotechActive)
				{
					PollutionUtility.PawnPollutionTick(this);
					GasUtility.PawnGasEffectsTick(this);
				}
				this.ageTracker.AgeTick();
				this.records.RecordsTick();
			}
			Pawn_GuiltTracker pawn_GuiltTracker = this.guilt;
			if (pawn_GuiltTracker != null)
			{
				pawn_GuiltTracker.GuiltTrackerTick();
			}
			Sustainer sustainer = this.sustainerAmbient;
			if (sustainer != null)
			{
				sustainer.Maintain();
			}
			Pawn_DrawTracker pawn_DrawTracker = this.drawer;
			if (pawn_DrawTracker == null)
			{
				return;
			}
			pawn_DrawTracker.renderer.EffectersTick(suspended);
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x0008AF34 File Offset: 0x00089134
		public void ProcessPostTickVisuals(int ticksPassed, CellRect viewRect)
		{
			if (!this.Suspended && base.Spawned)
			{
				if (Current.ProgramState != ProgramState.Playing || viewRect.Contains(base.Position))
				{
					this.Drawer.ProcessPostTickVisuals(ticksPassed);
				}
				this.rotationTracker.ProcessPostTickVisuals(ticksPassed);
			}
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x0008AF80 File Offset: 0x00089180
		public void TickMothballed(int interval)
		{
			if (!this.Suspended)
			{
				this.ageTracker.AgeTickMothballed(interval);
				this.records.RecordsTickMothballed(interval);
			}
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x0008AFA4 File Offset: 0x000891A4
		public void Notify_Teleported(bool endCurrentJob = true, bool resetTweenedPos = true)
		{
			if (resetTweenedPos)
			{
				this.Drawer.tweener.ResetTweenedPosToRoot();
			}
			this.pather.Notify_Teleported_Int();
			if (endCurrentJob && this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x0008AFF8 File Offset: 0x000891F8
		public void Notify_PassedToWorld()
		{
			if (((base.Faction == null && this.RaceProps.Humanlike) || (base.Faction != null && base.Faction.IsPlayer) || base.Faction == Faction.OfAncients || base.Faction == Faction.OfAncientsHostile) && !this.Dead && Find.WorldPawns.GetSituation(this) == WorldPawnSituation.Free)
			{
				bool tryMedievalOrBetter = base.Faction != null && base.Faction.def.techLevel >= TechLevel.Medieval;
				Faction faction;
				if (this.HasExtraHomeFaction(null) && !this.GetExtraHomeFaction(null).IsPlayer)
				{
					if (base.Faction != this.GetExtraHomeFaction(null))
					{
						this.SetFaction(this.GetExtraHomeFaction(null), null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, false, TechLevel.Undefined, false))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, true, TechLevel.Undefined, false))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
			}
			this.becameWorldPawnTickAbs = GenTicks.TicksAbs;
			if (!this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this))
			{
				this.ClearMind(false, false, true);
			}
			if (this.relations != null)
			{
				this.relations.Notify_PassedToWorld();
			}
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x0008B154 File Offset: 0x00089354
		public void Notify_AddBedThoughts()
		{
			foreach (ThingComp thingComp in base.AllComps)
			{
				thingComp.Notify_AddBedThoughts(this);
			}
			Ideo ideo = this.Ideo;
			if (ideo == null)
			{
				return;
			}
			ideo.Notify_AddBedThoughts(this);
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x0008B1B8 File Offset: 0x000893B8
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			if (ModsConfig.BiotechActive && this.genes != null)
			{
				float num = this.genes.FactorForDamage(dinfo);
				if (num != 1f)
				{
					dinfo.SetAmount(dinfo.Amount * num);
				}
			}
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			this.health.PreApplyDamage(dinfo, out absorbed);
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x0008B21C File Offset: 0x0008941C
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (dinfo.Def.ExternalViolenceFor(this))
			{
				this.records.AddTo(RecordDefOf.DamageTaken, totalDamageDealt);
			}
			if (dinfo.Def.makesBlood && !dinfo.InstantPermanentInjury && totalDamageDealt > 0f && Rand.Chance(0.5f))
			{
				this.health.DropBloodFilth();
			}
			this.health.PostApplyDamage(dinfo, totalDamageDealt);
			if (!this.Dead)
			{
				this.mindState.Notify_DamageTaken(dinfo);
			}
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x0008B2A8 File Offset: 0x000894A8
		public override Thing SplitOff(int count)
		{
			if (count <= 0 || count >= this.stackCount)
			{
				return base.SplitOff(count);
			}
			throw new NotImplementedException("Split off on Pawns is not supported (unless we're taking a full stack).");
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060017D5 RID: 6101 RVA: 0x0008B2C9 File Offset: 0x000894C9
		public int TicksPerMoveCardinal
		{
			get
			{
				return this.TicksPerMove(false);
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060017D6 RID: 6102 RVA: 0x0008B2D2 File Offset: 0x000894D2
		public int TicksPerMoveDiagonal
		{
			get
			{
				return this.TicksPerMove(true);
			}
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0008B2DC File Offset: 0x000894DC
		private int TicksPerMove(bool diagonal)
		{
			float num = this.GetStatValue(StatDefOf.MoveSpeed, true, -1);
			if (RestraintsUtility.InRestraints(this))
			{
				num *= 0.35f;
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null && this.carryTracker.CarriedThing.def.category == ThingCategory.Pawn)
			{
				num *= 0.6f;
			}
			float num2 = num / 60f;
			float num3;
			if (num2 == 0f)
			{
				num3 = 450f;
			}
			else
			{
				num3 = 1f / num2;
				if (base.Spawned && !base.Map.roofGrid.Roofed(base.Position))
				{
					num3 /= base.Map.weatherManager.CurMoveSpeedMultiplier;
				}
				if (diagonal)
				{
					num3 *= 1.41421f;
				}
			}
			return Mathf.Clamp(Mathf.RoundToInt(num3), 1, 450);
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x0008B3AC File Offset: 0x000895AC
		private void DoKillSideEffects(DamageInfo? dinfo, Hediff exactCulprit, bool spawned)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.Storyteller.Notify_PawnEvent(this, AdaptationEvent.Died, null);
			}
			if (this.IsColonist)
			{
				Find.StoryWatcher.statsRecord.Notify_ColonistKilled();
			}
			if (spawned && dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this))
			{
				LifeStageUtility.PlayNearestLifestageSound(this, (LifeStageAge ls) => ls.soundDeath, (GeneDef g) => g.soundDeath, 1f);
			}
			if (dinfo != null && dinfo.Value.Instigator != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null)
				{
					RecordsUtility.Notify_PawnKilled(this, pawn);
					if (pawn.equipment != null)
					{
						pawn.equipment.Notify_KilledPawn();
					}
					if (this.RaceProps.Humanlike)
					{
						Need_KillThirst need_KillThirst = pawn.needs.TryGetNeed<Need_KillThirst>();
						if (need_KillThirst != null)
						{
							need_KillThirst.Notify_KilledPawn(dinfo);
						}
					}
					if (pawn.health.hediffSet != null)
					{
						for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
						{
							pawn.health.hediffSet.hediffs[i].Notify_KilledPawn(pawn, dinfo);
						}
					}
					if (HistoryEventUtility.IsKillingInnocentAnimal(pawn, this))
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer), this.Named(HistoryEventArgsNames.Victim)), true);
					}
				}
			}
			TaleUtility.Notify_PawnDied(this, dinfo);
			if (spawned)
			{
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this, this.RaceProps.DeathActionWorker.DeathRules, (dinfo != null) ? (dinfo.Value.Instigator as Pawn) : null, exactCulprit, (dinfo != null) ? dinfo.Value.HitPart : null));
			}
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0008B5B8 File Offset: 0x000897B8
		private void PreDeathPawnModifications(DamageInfo? dinfo, Map map)
		{
			this.health.surgeryBills.Clear();
			if (this.apparel != null)
			{
				this.apparel.Notify_PawnKilled(dinfo);
			}
			if (this.relations != null)
			{
				this.relations.Notify_PawnKilled(dinfo, map);
			}
			if (this.connections != null)
			{
				this.connections.Notify_PawnKilled();
			}
			this.meleeVerbs.Notify_PawnKilled();
			for (int i = 0; i < this.health.hediffSet.hediffs.Count; i++)
			{
				this.health.hediffSet.hediffs[i].Notify_PawnKilled();
			}
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x0008B658 File Offset: 0x00089858
		private void DropBeforeDying(DamageInfo? dinfo, ref Map map, ref bool spawned)
		{
			Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
			Thing thing;
			if (pawn_CarryTracker != null && this.holdingOwner.TryDrop(this, pawn_CarryTracker.pawn.Position, pawn_CarryTracker.pawn.Map, ThingPlaceMode.Near, out thing, null, null, true))
			{
				map = pawn_CarryTracker.pawn.Map;
				spawned = true;
			}
			PawnDiedOrDownedThoughtsUtility.RemoveLostThoughts(this);
			PawnDiedOrDownedThoughtsUtility.RemoveResuedRelativeThought(this);
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this, dinfo, PawnDiedOrDownedThoughtsKind.Died);
			if (this.RaceProps.Animal)
			{
				PawnDiedOrDownedThoughtsUtility.GiveVeneratedAnimalDiedThoughts(this, map);
			}
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x0008B6D8 File Offset: 0x000898D8
		public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			int num = 0;
			try
			{
				num = 1;
				IntVec3 positionHeld = base.PositionHeld;
				Map map = base.Map;
				Map mapHeld = base.MapHeld;
				bool spawned = base.Spawned;
				bool spawnedOrAnyParentSpawned = base.SpawnedOrAnyParentSpawned;
				bool flag = this.IsWorldPawn();
				Pawn_GuiltTracker pawn_GuiltTracker = this.guilt;
				bool? flag2 = (pawn_GuiltTracker != null) ? new bool?(pawn_GuiltTracker.IsGuilty) : null;
				Caravan caravan = this.GetCaravan();
				Building_Grave assignedGrave = null;
				if (this.ownership != null)
				{
					assignedGrave = this.ownership.AssignedGrave;
				}
				Building_Bed currentBed = this.CurrentBed();
				ThingOwner thingOwner = null;
				bool inContainerEnclosed = this.InContainerEnclosed;
				if (inContainerEnclosed)
				{
					thingOwner = this.holdingOwner;
					thingOwner.Remove(this);
				}
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				if (Current.ProgramState == ProgramState.Playing && map != null)
				{
					flag3 = (map.designationManager.DesignationOn(this, DesignationDefOf.Hunt) != null);
					flag4 = this.ShouldBeSlaughtered();
					using (List<Lord>.Enumerator enumerator = map.lordManager.lords.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							LordJob_Ritual lordJob_Ritual;
							if ((lordJob_Ritual = (enumerator.Current.LordJob as LordJob_Ritual)) != null && lordJob_Ritual.pawnsDeathIgnored.Contains(this))
							{
								flag5 = true;
								break;
							}
						}
					}
				}
				bool flag6 = PawnUtility.ShouldSendNotificationAbout(this) && ((!flag4 && !flag5) || dinfo == null || dinfo.Value.Def != DamageDefOf.ExecutionCut) && !this.ForceNoDeathNotification;
				float num2 = 0f;
				Thing attachment = this.GetAttachment(ThingDefOf.Fire);
				if (attachment != null)
				{
					num2 = ((Fire)attachment).CurrentSize();
				}
				num = 2;
				this.DoKillSideEffects(dinfo, exactCulprit, spawned);
				num = 3;
				this.PreDeathPawnModifications(dinfo, map);
				num = 4;
				this.DropBeforeDying(dinfo, ref map, ref spawned);
				num = 5;
				this.health.SetDead();
				if (this.health.deflectionEffecter != null)
				{
					this.health.deflectionEffecter.Cleanup();
					this.health.deflectionEffecter = null;
				}
				if (this.health.woundedEffecter != null)
				{
					this.health.woundedEffecter.Cleanup();
					this.health.woundedEffecter = null;
				}
				if (caravan != null)
				{
					caravan.Notify_MemberDied(this);
				}
				Lord lord = this.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this, PawnLostCondition.Killed, dinfo);
				}
				if (spawned)
				{
					this.DropAndForbidEverything(false, false);
				}
				if (spawned)
				{
					GenLeaving.DoLeavingsFor(this, map, DestroyMode.KillFinalize, null);
				}
				bool flag7 = base.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (this.royalty != null)
				{
					this.royalty.Notify_PawnKilled();
				}
				Corpse corpse = null;
				if (!PawnGenerator.IsPawnBeingGeneratedAndNotAllowsDead(this))
				{
					if (inContainerEnclosed)
					{
						corpse = this.MakeCorpse(assignedGrave, currentBed);
						if (!thingOwner.TryAdd(corpse, true))
						{
							corpse.Destroy(DestroyMode.Vanish);
							corpse = null;
						}
					}
					else if (spawnedOrAnyParentSpawned)
					{
						if (this.holdingOwner != null)
						{
							this.holdingOwner.Remove(this);
						}
						corpse = this.MakeCorpse(assignedGrave, currentBed);
						if (GenPlace.TryPlaceThing(corpse, positionHeld, mapHeld, ThingPlaceMode.Direct, null, null, default(Rot4)))
						{
							corpse.Rotation = base.Rotation;
							if (HuntJobUtility.WasKilledByHunter(this, dinfo))
							{
								((Pawn)dinfo.Value.Instigator).Reserve(corpse, ((Pawn)dinfo.Value.Instigator).CurJob, 1, -1, null, true);
							}
							else if (!flag3 && !flag4)
							{
								corpse.SetForbiddenIfOutsideHomeArea();
							}
							if (num2 > 0f)
							{
								FireUtility.TryStartFireIn(corpse.Position, corpse.Map, num2);
							}
						}
						else
						{
							corpse.Destroy(DestroyMode.Vanish);
							corpse = null;
						}
					}
					else if (caravan != null && caravan.Spawned)
					{
						corpse = this.MakeCorpse(assignedGrave, currentBed);
						caravan.AddPawnOrItem(corpse, true);
					}
					else if (this.holdingOwner != null || this.IsWorldPawn())
					{
						Corpse.PostCorpseDestroy(this);
					}
					else
					{
						corpse = this.MakeCorpse(assignedGrave, currentBed);
					}
				}
				if (corpse != null)
				{
					Hediff firstHediffOfDef = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
					Hediff firstHediffOfDef2 = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Scaria, false);
					CompRottable comp;
					if ((comp = corpse.GetComp<CompRottable>()) != null && ((firstHediffOfDef != null && Rand.Value < firstHediffOfDef.Severity) || (firstHediffOfDef2 != null && Rand.Chance(Find.Storyteller.difficulty.scariaRotChance))))
					{
						comp.RotImmediately();
					}
				}
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.KillFinalize);
				}
				PawnComponentsUtility.RemoveComponentsOnKilled(this);
				this.health.hediffSet.DirtyCache();
				PortraitsCache.SetDirty(this);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this);
				if (flag7 && corpse != null)
				{
					Find.Selector.Select(corpse, false, false);
				}
				num = 6;
				this.health.hediffSet.Notify_PawnDied();
				Faction homeFaction = this.HomeFaction;
				if (homeFaction != null)
				{
					Faction faction = homeFaction;
					DamageInfo? dinfo2 = dinfo;
					bool wasWorldPawn = flag;
					bool? flag8 = flag2;
					bool flag9 = true;
					faction.Notify_MemberDied(this, dinfo2, wasWorldPawn, flag8.GetValueOrDefault() == flag9 & flag8 != null, mapHeld);
				}
				if (corpse != null)
				{
					if (this.RaceProps.DeathActionWorker != null && spawned)
					{
						this.RaceProps.DeathActionWorker.PawnDied(corpse);
					}
					if (Find.Scenario != null)
					{
						Find.Scenario.Notify_PawnDied(corpse);
					}
				}
				if (base.Faction != null && base.Faction.IsPlayer)
				{
					BillUtility.Notify_ColonistUnavailable(this);
				}
				if (spawnedOrAnyParentSpawned)
				{
					GenHostility.Notify_PawnLostForTutor(this, mapHeld);
				}
				if (base.Faction != null && base.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
				Pawn_PsychicEntropyTracker pawn_PsychicEntropyTracker = this.psychicEntropy;
				if (pawn_PsychicEntropyTracker != null)
				{
					pawn_PsychicEntropyTracker.Notify_PawnDied();
				}
				try
				{
					Ideo ideo = this.Ideo;
					if (ideo != null)
					{
						ideo.Notify_MemberDied(this);
					}
					Ideo ideo2 = this.Ideo;
					if (ideo2 != null)
					{
						ideo2.Notify_MemberLost(this, map);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error while notifying ideo of pawn death: " + arg);
				}
				if (flag6)
				{
					this.health.NotifyPlayerOfKilled(dinfo, exactCulprit, caravan);
				}
				Find.QuestManager.Notify_PawnKilled(this, dinfo);
				Find.FactionManager.Notify_PawnKilled(this);
				Find.IdeoManager.Notify_PawnKilled(this);
				if (ModsConfig.BiotechActive && MechanitorUtility.IsMechanitor(this))
				{
					Find.History.Notify_MechanitorDied();
				}
				Find.BossgroupManager.Notify_PawnKilled(this);
			}
			catch (Exception arg2)
			{
				Log.Error(string.Format("Error while killing {0} during phase {1}: {2}", this.ToStringSafe<Pawn>(), num, arg2));
			}
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0008BD40 File Offset: 0x00089F40
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.Vanish && mode != DestroyMode.KillFinalize)
			{
				Log.Error(string.Concat(new object[]
				{
					"Destroyed pawn ",
					this,
					" with unsupported mode ",
					mode,
					"."
				}));
			}
			base.Destroy(mode);
			Find.WorldPawns.Notify_PawnDestroyed(this);
			if (this.ownership != null)
			{
				Building_Grave assignedGrave = this.ownership.AssignedGrave;
				this.ownership.UnclaimAll();
				if (mode == DestroyMode.KillFinalize && assignedGrave != null)
				{
					assignedGrave.CompAssignableToPawn.TryAssignPawn(this);
				}
			}
			this.ClearMind(false, true, true);
			Lord lord = this.GetLord();
			if (lord != null)
			{
				PawnLostCondition cond = (mode == DestroyMode.KillFinalize) ? PawnLostCondition.Killed : PawnLostCondition.Vanished;
				lord.Notify_PawnLost(this, cond, null);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.GameEnder.CheckOrUpdateGameOver();
				Find.TaleManager.Notify_PawnDestroyed(this);
			}
			foreach (Pawn pawn in from p in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where p.playerSettings != null && p.playerSettings.Master == this
			select p)
			{
				pawn.playerSettings.Master = null;
			}
			if (this.equipment != null)
			{
				this.equipment.Notify_PawnDied();
			}
			if (mode != DestroyMode.KillFinalize)
			{
				if (this.equipment != null)
				{
					this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				}
				this.inventory.DestroyAll(DestroyMode.Vanish);
				if (this.apparel != null)
				{
					this.apparel.DestroyAll(DestroyMode.Vanish);
				}
			}
			WorldPawns worldPawns = Find.WorldPawns;
			if (!worldPawns.IsBeingDiscarded(this) && !worldPawns.Contains(this))
			{
				worldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			}
			if (base.Faction.IsPlayerSafe())
			{
				Ideo ideo = this.Ideo;
				if (ideo != null)
				{
					ideo.RecacheColonistBelieverCount();
				}
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker == null)
			{
				return;
			}
			pawn_RelationsTracker.Notify_PawnDestroyed(mode);
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0008BF0C File Offset: 0x0008A10C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			if (this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.StopAll(false, true);
			}
			base.DeSpawn(mode);
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null)
			{
				pawn_RopeTracker.Notify_DeSpawned();
			}
			this.mindState.droppedWeapon = null;
			if (this.needs != null && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (this.meleeVerbs != null)
			{
				this.meleeVerbs.Notify_PawnDespawned();
			}
			Pawn_MechanitorTracker pawn_MechanitorTracker = this.mechanitor;
			if (pawn_MechanitorTracker != null)
			{
				pawn_MechanitorTracker.Notify_DeSpawned(mode);
			}
			this.ClearAllReservations(false);
			if (map != null)
			{
				map.mapPawns.DeRegisterPawn(this);
				map.autoSlaughterManager.Notify_PawnDespawned();
			}
			PawnComponentsUtility.RemoveComponentsOnDespawned(this);
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.End();
				this.sustainerAmbient = null;
			}
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0008C010 File Offset: 0x0008A210
		public override void Discard(bool silentlyRemoveReferences = false)
		{
			if (Find.WorldPawns.Contains(this))
			{
				Log.Warning("Tried to discard a world pawn " + this + ".");
				return;
			}
			base.Discard(silentlyRemoveReferences);
			if (this.relations != null)
			{
				this.relations.ClearAllRelations();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.PlayLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.BattleLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.TaleManager.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.QuestManager.Notify_PawnDiscarded(this);
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				if (pawn.needs != null && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.memories.Notify_PawnDiscarded(this);
				}
			}
			Corpse.PostCorpseDestroy(this);
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0008C108 File Offset: 0x0008A308
		public Corpse MakeCorpse(Building_Grave assignedGrave, Building_Bed currentBed)
		{
			return this.MakeCorpse(assignedGrave, currentBed != null, (currentBed != null) ? currentBed.Rotation.AsAngle : 0f);
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0008C138 File Offset: 0x0008A338
		public Corpse MakeCorpse(Building_Grave assignedGrave, bool inBed, float bedRotation)
		{
			if (this.holdingOwner != null)
			{
				Log.Warning("We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=" + base.ParentHolder);
				return null;
			}
			Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
			corpse.InnerPawn = this;
			if (assignedGrave != null)
			{
				corpse.InnerPawn.ownership.ClaimGrave(assignedGrave);
			}
			if (inBed)
			{
				corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(bedRotation + 180f);
			}
			return corpse;
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0008C1BC File Offset: 0x0008A3BC
		public void ExitMap(bool allowedToJoinOrCreateCaravan, Rot4 exitDir)
		{
			if (this.IsWorldPawn())
			{
				Log.Warning("Called ExitMap() on world pawn " + this);
				return;
			}
			Ideo ideo = this.Ideo;
			if (ideo != null)
			{
				ideo.Notify_MemberLost(this, base.Map);
			}
			if (allowedToJoinOrCreateCaravan && CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this))
			{
				CaravanExitMapUtility.ExitMapAndJoinOrCreateCaravan(this, exitDir);
				return;
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.ExitedMap, null);
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
			{
				Pawn pawn = this.carryTracker.CarriedThing as Pawn;
				if (pawn != null)
				{
					if (base.Faction != null && base.Faction != pawn.Faction)
					{
						base.Faction.kidnapped.Kidnap(pawn, this);
					}
					else
					{
						if (!this.teleporting)
						{
							this.carryTracker.innerContainer.Remove(pawn);
						}
						pawn.ExitMap(false, exitDir);
					}
				}
				else
				{
					this.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
				}
				if (!this.teleporting || pawn == null)
				{
					this.carryTracker.innerContainer.Clear();
				}
			}
			bool flag = !this.IsCaravanMember() && !this.teleporting && !PawnUtility.IsTravelingInTransportPodWorldObject(this) && (!this.IsPrisoner || base.ParentHolder == null || base.ParentHolder is CompShuttle || (this.guest != null && this.guest.Released));
			if (flag)
			{
				foreach (Thing thing in this.EquippedWornOrInventoryThings)
				{
					Precept_ThingStyle styleSourcePrecept = thing.GetStyleSourcePrecept();
					if (styleSourcePrecept != null)
					{
						styleSourcePrecept.Notify_ThingLost(thing, false);
					}
				}
			}
			if (base.Faction != null)
			{
				base.Faction.Notify_MemberExitedMap(this, flag);
			}
			if (base.Faction == Faction.OfPlayer && this.IsSlave && this.SlaveFaction != null && this.SlaveFaction != Faction.OfPlayer && this.guest.Released)
			{
				this.SlaveFaction.Notify_MemberExitedMap(this, flag);
			}
			if (this.ownership != null && flag)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				bool isPrisonerOfColony = this.IsPrisonerOfColony;
				if (flag)
				{
					this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
				}
				if (isPrisonerOfColony)
				{
					this.guest.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
					if (!this.guest.Released && flag)
					{
						GuestUtility.Notify_PrisonerEscaped(this);
					}
				}
				this.guest.Released = false;
			}
			base.DeSpawnOrDeselect(DestroyMode.Vanish);
			this.inventory.UnloadEverything = false;
			if (flag)
			{
				this.ClearMind(false, false, true);
			}
			if (this.relations != null)
			{
				this.relations.Notify_ExitedMap();
			}
			Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			QuestUtility.SendQuestTargetSignals(this.questTags, "LeftMap", this.Named("SUBJECT"));
			Find.FactionManager.Notify_PawnLeftMap(this);
			Find.IdeoManager.Notify_PawnLeftMap(this);
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0008C4B4 File Offset: 0x0008A6B4
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			if (base.SpawnedOrAnyParentSpawned)
			{
				this.DropAndForbidEverything(false, false);
			}
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			if (action == TradeAction.PlayerSells)
			{
				Faction faction = this.GetExtraHomeFaction(null) ?? this.GetExtraHostFaction(null);
				if (faction != null && faction != Faction.OfPlayer)
				{
					Faction.OfPlayer.TryAffectGoodwillWith(faction, Faction.OfPlayer.GoodwillToMakeHostile(faction), true, true, HistoryEventDefOf.MemberSold, new GlobalTargetInfo?(this));
				}
			}
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (action == TradeAction.PlayerBuys)
			{
				if (this.guest != null && this.guest.joinStatus == JoinStatus.JoinAsSlave)
				{
					this.guest.SetGuestStatus(Faction.OfPlayer, RimWorld.GuestStatus.Slave);
				}
				else
				{
					Need_Mood mood = this.needs.mood;
					if (mood != null)
					{
						mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FreedFromSlavery, null, null);
					}
					this.SetFaction(Faction.OfPlayer, null);
				}
			}
			else if (action == TradeAction.PlayerSells)
			{
				if (this.RaceProps.Humanlike)
				{
					TaleRecorder.RecordTale(TaleDefOf.SoldPrisoner, new object[]
					{
						playerNegotiator,
						this,
						trader
					});
				}
				if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
				if (this.RaceProps.IsFlesh)
				{
					this.relations.Notify_PawnSold(playerNegotiator);
				}
			}
			this.ClearMind(false, false, true);
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0008C614 File Offset: 0x0008A814
		public void PreKidnapped(Pawn kidnapper)
		{
			Find.Storyteller.Notify_PawnEvent(this, AdaptationEvent.Kidnapped, null);
			if (this.IsColonist && kidnapper != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.KidnappedColonist, new object[]
				{
					kidnapper,
					this
				});
			}
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null && !this.guest.IsSlave)
			{
				this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKidnapped();
			}
			this.ClearMind(false, false, true);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0000249D File Offset: 0x0000069D
		public override bool ClaimableBy(Faction by, StringBuilder reason = null)
		{
			return false;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0008C6B0 File Offset: 0x0008A8B0
		public override bool AdoptableBy(Faction by, StringBuilder reason = null)
		{
			if (base.Faction == by)
			{
				return false;
			}
			Pawn_AgeTracker pawn_AgeTracker = this.ageTracker;
			bool flag;
			if (pawn_AgeTracker == null)
			{
				flag = false;
			}
			else
			{
				LifeStageDef curLifeStage = pawn_AgeTracker.CurLifeStage;
				bool? flag2 = (curLifeStage != null) ? new bool?(curLifeStage.claimable) : null;
				bool flag3 = false;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			return !flag && !base.FactionPreventsClaimingOrAdopting(base.Faction, false, reason);
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0008C720 File Offset: 0x0008A920
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (newFaction == base.Faction)
			{
				Log.Warning("Used SetFaction to change " + this.ToStringSafe<Pawn>() + " to same faction " + newFaction.ToStringSafe<Faction>());
				return;
			}
			Faction faction = base.Faction;
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (base.Spawned)
			{
				base.Map.mapPawns.DeRegisterPawn(this);
				base.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				base.Map.designationManager.RemoveAllDesignationsOn(this, false);
				base.Map.autoSlaughterManager.Notify_PawnChangedFaction();
			}
			if ((newFaction == Faction.OfPlayer || base.Faction == Faction.OfPlayer) && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.ChangedFaction, null);
			}
			if (PawnUtility.IsFactionLeader(this))
			{
				Faction factionLeaderFaction = PawnUtility.GetFactionLeaderFaction(this);
				if (newFaction != factionLeaderFaction && !this.HasExtraHomeFaction(factionLeaderFaction) && !this.HasExtraMiniFaction(factionLeaderFaction))
				{
					factionLeaderFaction.Notify_LeaderLost();
				}
			}
			if (newFaction == Faction.OfPlayer && this.RaceProps.Humanlike && !this.IsQuestLodger())
			{
				this.ChangeKind(newFaction.def.basicMemberKind);
			}
			base.SetFaction(newFaction, null);
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this, false);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				if (this.workSettings != null)
				{
					this.workSettings.EnableAndInitialize();
				}
				Find.StoryWatcher.watcherPopAdaptation.Notify_PawnEvent(this, PopAdaptationEvent.GainedColonist);
			}
			if (this.Drafted)
			{
				this.drafter.Drafted = false;
			}
			ReachabilityUtility.ClearCacheFor(this);
			this.health.surgeryBills.Clear();
			if (base.Spawned)
			{
				base.Map.mapPawns.RegisterPawn(this);
			}
			this.GenerateNecessaryName();
			if (this.playerSettings != null)
			{
				this.playerSettings.ResetMedicalCare();
			}
			this.ClearMind(true, false, true);
			if (!this.Dead && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (base.Spawned)
			{
				base.Map.attackTargetsCache.UpdateTarget(this);
			}
			Find.GameEnder.CheckOrUpdateGameOver();
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
			if (this.needs != null)
			{
				this.needs.AddOrRemoveNeedsAsAppropriate();
			}
			if (this.playerSettings != null)
			{
				this.playerSettings.Notify_FactionChanged();
			}
			if (this.relations != null)
			{
				this.relations.Notify_ChangedFaction();
			}
			if (this.RaceProps.Animal && newFaction == Faction.OfPlayer)
			{
				this.training.SetWantedRecursive(TrainableDefOf.Tameness, true);
				this.training.Train(TrainableDefOf.Tameness, recruiter, true);
				if (this.RaceProps.Roamer && this.mindState != null)
				{
					this.mindState.lastStartRoamCooldownTick = new int?(Find.TickManager.TicksGame);
				}
			}
			if (faction == Faction.OfPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this);
			}
			if (newFaction == Faction.OfPlayer)
			{
				Find.StoryWatcher.statsRecord.UpdateGreatestPopulation();
				Find.World.StoryState.RecordPopulationIncrease();
			}
			if (newFaction != null)
			{
				newFaction.Notify_PawnJoined(this);
			}
			if (this.Ideo != null)
			{
				this.Ideo.Notify_MemberChangedFaction(this, faction, newFaction);
			}
			Pawn_AgeTracker pawn_AgeTracker = this.ageTracker;
			if (pawn_AgeTracker != null)
			{
				pawn_AgeTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Recruited, false);
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null)
			{
				pawn_RopeTracker.BreakAllRopes();
			}
			if (ModsConfig.BiotechActive && this.mechanitor != null)
			{
				this.mechanitor.Notify_ChangedFaction();
			}
			if (faction != null)
			{
				Find.FactionManager.Notify_PawnLeftFaction(faction);
			}
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0008CAA0 File Offset: 0x0008ACA0
		public void ClearMind(bool ifLayingKeepLaying = false, bool clearInspiration = false, bool clearMentalState = true)
		{
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			if (this.mindState != null)
			{
				this.mindState.Reset(clearInspiration, clearMentalState);
			}
			if (this.jobs != null)
			{
				this.jobs.StopAll(ifLayingKeepLaying, true);
			}
			this.VerifyReservations();
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0008CAF0 File Offset: 0x0008ACF0
		public void ClearAllReservations(bool releaseDestinationsOnlyIfObsolete = true)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (releaseDestinationsOnlyIfObsolete)
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllObsoleteClaimedBy(this);
				}
				else
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				}
				maps[i].reservationManager.ReleaseAllClaimedBy(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllClaimedBy(this);
				maps[i].attackTargetReservationManager.ReleaseAllClaimedBy(this);
			}
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0008CB74 File Offset: 0x0008AD74
		public void ClearReservationsForJob(Job job)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].pawnDestinationReservationManager.ReleaseClaimedBy(this, job);
				maps[i].reservationManager.ReleaseClaimedBy(this, job);
				maps[i].physicalInteractionReservationManager.ReleaseClaimedBy(this, job);
				maps[i].attackTargetReservationManager.ReleaseClaimedBy(this, job);
			}
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0008CBE4 File Offset: 0x0008ADE4
		public void VerifyReservations()
		{
			if (this.jobs == null)
			{
				return;
			}
			if (this.CurJob != null || this.jobs.jobQueue.Count > 0 || this.jobs.startingNewJob)
			{
				return;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				LocalTargetInfo obj = maps[i].reservationManager.FirstReservationFor(this);
				if (obj.IsValid)
				{
					Log.ErrorOnce(string.Format("Reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj.ToStringSafe<LocalTargetInfo>()), 97771429 ^ this.thingIDNumber);
					flag = true;
				}
				LocalTargetInfo obj2 = maps[i].physicalInteractionReservationManager.FirstReservationFor(this);
				if (obj2.IsValid)
				{
					Log.ErrorOnce(string.Format("Physical interaction reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj2.ToStringSafe<LocalTargetInfo>()), 19586765 ^ this.thingIDNumber);
					flag = true;
				}
				IAttackTarget attackTarget = maps[i].attackTargetReservationManager.FirstReservationFor(this);
				if (attackTarget != null)
				{
					Log.ErrorOnce(string.Format("Attack target reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), attackTarget.ToStringSafe<IAttackTarget>()), 100495878 ^ this.thingIDNumber);
					flag = true;
				}
				IntVec3 obj3 = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationFor(this);
				if (obj3.IsValid)
				{
					Job job = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationJobFor(this);
					Log.ErrorOnce(string.Format("Pawn destination reservation manager failed to clean up properly; {0}/{1}/{2} still reserving {3}", new object[]
					{
						this.ToStringSafe<Pawn>(),
						job.ToStringSafe<Job>(),
						job.def.ToStringSafe<JobDef>(),
						obj3.ToStringSafe<IntVec3>()
					}), 1958674 ^ this.thingIDNumber);
					flag = true;
				}
			}
			if (flag)
			{
				this.ClearAllReservations(true);
			}
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x0008CD9C File Offset: 0x0008AF9C
		public void DropAndForbidEverything(bool keepInventoryAndEquipmentIfInBed = false, bool rememberPrimary = false)
		{
			if (this.kindDef.destroyGearOnDrop)
			{
				this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				this.apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (this.InContainerEnclosed)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					this.carryTracker.innerContainer.TryTransferToContainer(this.carryTracker.CarriedThing, this.holdingOwner, true);
				}
				if (this.equipment != null && this.equipment.Primary != null)
				{
					this.equipment.TryTransferEquipmentToContainer(this.equipment.Primary, this.holdingOwner);
				}
				if (this.inventory != null)
				{
					this.inventory.innerContainer.TryTransferAllToContainer(this.holdingOwner, true);
					return;
				}
			}
			else if (base.SpawnedOrAnyParentSpawned)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					Thing thing;
					this.carryTracker.TryDropCarriedThing(base.PositionHeld, ThingPlaceMode.Near, out thing, null);
				}
				if (!keepInventoryAndEquipmentIfInBed || !this.InBed())
				{
					if (this.equipment != null)
					{
						this.equipment.DropAllEquipment(base.PositionHeld, true, rememberPrimary);
					}
					if (this.inventory != null && this.inventory.innerContainer.TotalStackCount > 0)
					{
						this.inventory.DropAllNearPawn(base.PositionHeld, true, false);
					}
				}
			}
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0008CEF0 File Offset: 0x0008B0F0
		public void GenerateNecessaryName()
		{
			if (this.Name != null)
			{
				return;
			}
			if (base.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (this.RaceProps.Animal || (ModsConfig.BiotechActive && this.RaceProps.IsMechanoid))
			{
				this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Numeric, null, false, null);
			}
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0008CF48 File Offset: 0x0008B148
		public Verb TryGetAttackVerb(Thing target, bool allowManualCastWeapons = false, bool allowTurrets = false)
		{
			if (this.equipment != null && this.equipment.Primary != null && this.equipment.PrimaryEq.PrimaryVerb.Available() && (!this.equipment.PrimaryEq.PrimaryVerb.verbProps.onlyManualCast || (this.CurJob != null && this.CurJob.def != JobDefOf.Wait_Combat) || allowManualCastWeapons))
			{
				return this.equipment.PrimaryEq.PrimaryVerb;
			}
			if (allowManualCastWeapons && this.apparel != null)
			{
				Verb firstApparelVerb = this.apparel.FirstApparelVerb;
				if (firstApparelVerb != null && firstApparelVerb.Available())
				{
					return firstApparelVerb;
				}
			}
			if (allowTurrets)
			{
				List<ThingComp> allComps = base.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					CompTurretGun compTurretGun;
					if ((compTurretGun = (allComps[i] as CompTurretGun)) != null && !compTurretGun.TurretDestroyed && compTurretGun.GunCompEq.PrimaryVerb.Available())
					{
						return compTurretGun.GunCompEq.PrimaryVerb;
					}
				}
			}
			return this.meleeVerbs.TryGetMeleeVerb(target);
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x0008D058 File Offset: 0x0008B258
		public bool TryStartAttack(LocalTargetInfo targ)
		{
			if (this.stances.FullBodyBusy)
			{
				return false;
			}
			if (this.WorkTagIsDisabled(WorkTags.Violent))
			{
				return false;
			}
			bool allowManualCastWeapons = !this.IsColonist;
			Verb verb = this.TryGetAttackVerb(targ.Thing, allowManualCastWeapons, false);
			return verb != null && verb.TryStartCastOn(verb.verbProps.ai_RangedAlawaysShootGroundBelowTarget ? targ.Cell : targ, false, true, false, false);
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0008D0C4 File Offset: 0x0008B2C4
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.RaceProps.meatDef != null)
			{
				int num = GenMath.RoundRandom(this.GetStatValue(StatDefOf.MeatAmount, true, -1) * efficiency);
				if (num > 0)
				{
					Thing thing = ThingMaker.MakeThing(this.RaceProps.meatDef, null);
					thing.stackCount = num;
					yield return thing;
				}
			}
			foreach (Thing thing2 in base.ButcherProducts(butcher, efficiency))
			{
				yield return thing2;
			}
			IEnumerator<Thing> enumerator = null;
			if (this.RaceProps.leatherDef != null)
			{
				int num2 = GenMath.RoundRandom(this.GetStatValue(StatDefOf.LeatherAmount, true, -1) * efficiency);
				if (num2 > 0)
				{
					Thing thing3 = ThingMaker.MakeThing(this.RaceProps.leatherDef, null);
					thing3.stackCount = num2;
					yield return thing3;
				}
			}
			if (!this.RaceProps.Humanlike)
			{
				Pawn.<>c__DisplayClass264_0 CS$<>8__locals1 = new Pawn.<>c__DisplayClass264_0();
				CS$<>8__locals1.lifeStage = this.ageTracker.CurKindLifeStage;
				if (CS$<>8__locals1.lifeStage.butcherBodyPart != null && (this.gender == Gender.None || (this.gender == Gender.Male && CS$<>8__locals1.lifeStage.butcherBodyPart.allowMale) || (this.gender == Gender.Female && CS$<>8__locals1.lifeStage.butcherBodyPart.allowFemale)))
				{
					for (;;)
					{
						IEnumerable<BodyPartRecord> notMissingParts = this.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
						Func<BodyPartRecord, bool> predicate;
						if ((predicate = CS$<>8__locals1.<>9__0) == null)
						{
							predicate = (CS$<>8__locals1.<>9__0 = ((BodyPartRecord x) => x.IsInGroup(CS$<>8__locals1.lifeStage.butcherBodyPart.bodyPartGroup)));
						}
						BodyPartRecord bodyPartRecord = notMissingParts.Where(predicate).FirstOrDefault<BodyPartRecord>();
						if (bodyPartRecord == null)
						{
							break;
						}
						this.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this, bodyPartRecord), null, null, null);
						Thing thing4;
						if (CS$<>8__locals1.lifeStage.butcherBodyPart.thing != null)
						{
							thing4 = ThingMaker.MakeThing(CS$<>8__locals1.lifeStage.butcherBodyPart.thing, null);
						}
						else
						{
							thing4 = ThingMaker.MakeThing(bodyPartRecord.def.spawnThingOnRemoved, null);
						}
						yield return thing4;
					}
				}
				CS$<>8__locals1 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x0008D0E4 File Offset: 0x0008B2E4
		public TaggedString FactionDesc(TaggedString name, bool extraFactionsInfo, string nameLabel, string genderLabel)
		{
			Pawn.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(this, Pawn.tmpExtraFactions, null);
			GuestUtility.GetExtraFactionsFromGuestStatus(this, Pawn.tmpExtraFactions);
			TaggedString taggedString;
			if (base.Faction != null && !base.Faction.Hidden)
			{
				if (Pawn.tmpExtraFactions.Count == 0 && this.SlaveFaction == null)
				{
					taggedString = "PawnMainDescFactionedWrap".Translate(name, base.Faction.NameColored, nameLabel.Named("NAME"), genderLabel.Named("GENDER"));
				}
				else
				{
					taggedString = "PawnMainDescUnderFactionedWrap".Translate(name, base.Faction.NameColored);
				}
			}
			else
			{
				taggedString = name;
			}
			if (extraFactionsInfo)
			{
				for (int i = 0; i < Pawn.tmpExtraFactions.Count; i++)
				{
					if (base.Faction != Pawn.tmpExtraFactions[i].faction)
					{
						taggedString += string.Format("\n{0}: {1}", Pawn.tmpExtraFactions[i].factionType.GetLabel().CapitalizeFirst(), Pawn.tmpExtraFactions[i].faction.NameColored.Resolve());
					}
				}
			}
			Pawn.tmpExtraFactions.Clear();
			return taggedString;
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0008D220 File Offset: 0x0008B420
		public string MainDesc(bool writeFaction, bool writeGender = true)
		{
			bool flag = base.Faction == null || !base.Faction.IsPlayer;
			string text = writeGender ? ((this.gender == Gender.None) ? "" : this.gender.GetLabel(this.AnimalOrWildMan())) : "";
			string text2 = "";
			if (this.RaceProps.Animal || this.RaceProps.IsMechanoid)
			{
				text2 = GenLabel.BestKindLabel(this, false, true, false, -1);
				if (this.Name != null)
				{
					if (!text.NullOrEmpty())
					{
						text += " ";
					}
					text += text2;
				}
			}
			if (this.ageTracker != null)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "AgeIndicator".Translate(this.ageTracker.AgeNumberString);
			}
			if (!this.RaceProps.Animal && !this.RaceProps.IsMechanoid && flag)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text2 = GenLabel.BestKindLabel(this, false, true, false, -1);
				text += text2;
			}
			if (writeFaction)
			{
				text = this.FactionDesc(text, true, text2, this.gender.GetLabel(this.RaceProps.Animal)).Resolve();
			}
			return text.CapitalizeFirst();
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x0008D384 File Offset: 0x0008B584
		public string GetJobReport()
		{
			string result;
			try
			{
				Pawn_JobTracker pawn_JobTracker = this.jobs;
				if (((pawn_JobTracker != null) ? pawn_JobTracker.curJob : null) != null)
				{
					JobDriver curDriver = this.jobs.curDriver;
					result = ((curDriver != null) ? curDriver.GetReport().CapitalizeFirst() : null);
				}
				else
				{
					result = null;
				}
			}
			catch (Exception arg)
			{
				Log.Error("JobDriver.GetReport() exception: " + arg);
				result = null;
			}
			return result;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0008D3F0 File Offset: 0x0008B5F0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.MainDesc(true, true));
			Pawn_RoyaltyTracker pawn_RoyaltyTracker = this.royalty;
			RoyalTitle royalTitle = (pawn_RoyaltyTracker != null) ? pawn_RoyaltyTracker.MostSeniorTitle : null;
			if (royalTitle != null)
			{
				stringBuilder.AppendLine("PawnTitleDescWrap".Translate(royalTitle.def.GetLabelCapFor(this), royalTitle.faction.NameColored).Resolve());
			}
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			if (this.TraderKind != null)
			{
				stringBuilder.AppendLine(this.TraderKind.LabelCap);
			}
			if (this.InMentalState)
			{
				stringBuilder.AppendLine(this.MentalState.InspectLine);
			}
			Pawn.states.Clear();
			if (this.health != null && this.health.hediffSet != null)
			{
				List<Hediff> hediffs = this.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff hediff = hediffs[i];
					if (!hediff.def.battleStateLabel.NullOrEmpty())
					{
						Pawn.states.AddDistinct(hediff.def.battleStateLabel);
					}
				}
			}
			if (Pawn.states.Count > 0)
			{
				Pawn.states.Sort();
				stringBuilder.AppendLine(string.Format("{0}: {1}", "State".Translate(), Pawn.states.ToCommaList(false, false).CapitalizeFirst()));
				Pawn.states.Clear();
			}
			Pawn_StanceTracker pawn_StanceTracker = this.stances;
			if (((pawn_StanceTracker != null) ? pawn_StanceTracker.stunner : null) != null && this.stances.stunner.Stunned)
			{
				stringBuilder.AppendLine("StunLower".Translate().CapitalizeFirst() + ": " + this.stances.stunner.StunTicksLeft.ToStringSecondsFromTicks());
			}
			Pawn_StanceTracker pawn_StanceTracker2 = this.stances;
			if (((pawn_StanceTracker2 != null) ? pawn_StanceTracker2.stagger : null) != null && this.stances.stagger.Staggered)
			{
				stringBuilder.AppendLine("SlowedByDamage".Translate() + ": " + this.stances.stagger.StaggerTicksLeft.ToStringSecondsFromTicks());
			}
			if (this.Inspired)
			{
				stringBuilder.AppendLine(this.Inspiration.InspectLine);
			}
			if (this.equipment != null && this.equipment.Primary != null)
			{
				stringBuilder.AppendLine("Equipped".TranslateSimple() + ": " + ((this.equipment.Primary != null) ? this.equipment.Primary.Label : "EquippedNothing".TranslateSimple()).CapitalizeFirst());
			}
			if (this.abilities != null)
			{
				for (int j = 0; j < this.abilities.AllAbilitiesForReading.Count; j++)
				{
					string inspectString2 = this.abilities.AllAbilitiesForReading[j].GetInspectString();
					if (!inspectString2.NullOrEmpty())
					{
						stringBuilder.AppendLine(inspectString2);
					}
				}
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
			{
				stringBuilder.Append("Carrying".Translate() + ": ");
				stringBuilder.AppendLine(this.carryTracker.CarriedThing.LabelCap);
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null && pawn_RopeTracker.IsRoped)
			{
				stringBuilder.AppendLine(this.roping.InspectLine);
			}
			if (ModsConfig.BiotechActive && this.IsColonyMech && this.needs.energy != null)
			{
				TaggedString taggedString = "MechEnergy".Translate() + ": " + this.needs.energy.CurLevelPercentage.ToStringPercent();
				float maxLevel = this.needs.energy.MaxLevel;
				if (this.IsCharging())
				{
					taggedString += " (+" + "PerDay".Translate((50f / maxLevel).ToStringPercent()) + ")";
				}
				else if (this.IsSelfShutdown())
				{
					taggedString += " (+" + "PerDay".Translate((1f / maxLevel).ToStringPercent()) + ")";
				}
				else
				{
					taggedString += " (-" + "PerDay".Translate((this.needs.energy.FallPerDay / maxLevel).ToStringPercent()) + ")";
				}
				stringBuilder.AppendLine(taggedString);
			}
			if ((base.Faction == Faction.OfPlayer || this.HostFaction == Faction.OfPlayer) && !this.InMentalState)
			{
				Lord lord = this.GetLord();
				LordJob lordJob = (lord != null) ? lord.LordJob : null;
				string text = (lordJob != null) ? lordJob.GetReport(this) : null;
				string text2 = ((lordJob != null) ? lordJob.GetJobReport(this) : null) ?? this.GetJobReport();
				if (text.NullOrEmpty())
				{
					text = text2;
				}
				else if (!text2.NullOrEmpty())
				{
					text = text + ": " + text2;
				}
				if (!text.NullOrEmpty())
				{
					stringBuilder.AppendLine(text);
				}
			}
			Pawn_JobTracker pawn_JobTracker = this.jobs;
			if (((pawn_JobTracker != null) ? pawn_JobTracker.curJob : null) != null)
			{
				Pawn_JobTracker pawn_JobTracker2 = this.jobs;
				if (pawn_JobTracker2 != null && pawn_JobTracker2.jobQueue.Count > 0)
				{
					try
					{
						string text3 = this.jobs.jobQueue[0].job.GetReport(this).CapitalizeFirst();
						if (this.jobs.jobQueue.Count > 1)
						{
							text3 = string.Concat(new object[]
							{
								text3,
								" (+",
								this.jobs.jobQueue.Count - 1,
								")"
							});
						}
						stringBuilder.AppendLine("Queued".Translate() + ": " + text3);
					}
					catch (Exception arg)
					{
						Log.Error("JobDriver.GetReport() exception: " + arg);
					}
				}
			}
			if (ModsConfig.BiotechActive)
			{
				Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
				if (((pawn_NeedsTracker != null) ? pawn_NeedsTracker.energy : null) != null && this.needs.energy.IsLowEnergySelfShutdown)
				{
					stringBuilder.AppendLine("MustBeCarriedToRecharger".Translate());
				}
			}
			if (RestraintsUtility.ShouldShowRestraintsInfo(this))
			{
				stringBuilder.AppendLine("InRestraints".Translate());
			}
			if (this.guest != null && !this.guest.Recruitable && base.Faction != Faction.OfPlayer)
			{
				stringBuilder.AppendLine("Unrecruitable".Translate().CapitalizeFirst());
			}
			if (Prefs.DevMode && DebugSettings.showLocomotionUrgency && this.CurJob != null)
			{
				stringBuilder.AppendLine("Locomotion Urgency: " + this.CurJob.locomotionUrgency.ToString());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x0008DB3C File Offset: 0x0008BD3C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Lord lord2 = this.GetLord();
			LordJob_Ritual lordJob_Ritual;
			if ((this.IsColonistPlayerControlled || this.IsColonyMech) && (lord2 == null || (lordJob_Ritual = (lord2.LordJob as LordJob_Ritual)) == null || !lordJob_Ritual.BlocksDrafting))
			{
				if (this.drafter != null)
				{
					foreach (Gizmo gizmo2 in this.drafter.GetGizmos())
					{
						yield return gizmo2;
					}
					enumerator = null;
				}
				foreach (Gizmo gizmo3 in PawnAttackGizmoUtility.GetAttackGizmos(this))
				{
					yield return gizmo3;
				}
				enumerator = null;
			}
			if (this.equipment != null)
			{
				foreach (Gizmo gizmo4 in this.equipment.GetGizmos())
				{
					yield return gizmo4;
				}
				enumerator = null;
			}
			if (this.carryTracker != null)
			{
				foreach (Gizmo gizmo5 in this.carryTracker.GetGizmos())
				{
					yield return gizmo5;
				}
				enumerator = null;
			}
			if (this.needs != null)
			{
				foreach (Gizmo gizmo6 in this.needs.GetGizmos())
				{
					yield return gizmo6;
				}
				enumerator = null;
			}
			if (Find.Selector.SingleSelectedThing == this && this.psychicEntropy != null && this.psychicEntropy.NeedToShowGizmo())
			{
				yield return this.psychicEntropy.GetGizmo();
				if (DebugSettings.ShowDevGizmos)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Psyfocus -20%",
						action = delegate()
						{
							this.psychicEntropy.OffsetPsyfocusDirectly(-0.2f);
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "DEV: Psyfocus +20%",
						action = delegate()
						{
							this.psychicEntropy.OffsetPsyfocusDirectly(0.2f);
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "DEV: Neural heat -20",
						action = delegate()
						{
							this.psychicEntropy.TryAddEntropy(-20f, null, true, false);
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "DEV: Neural heat +20",
						action = delegate()
						{
							this.psychicEntropy.TryAddEntropy(20f, null, true, false);
						}
					};
				}
			}
			if (ModsConfig.BiotechActive)
			{
				if (MechanitorUtility.IsMechanitor(this))
				{
					foreach (Gizmo gizmo7 in this.mechanitor.GetGizmos())
					{
						yield return gizmo7;
					}
					enumerator = null;
				}
				if (this.RaceProps.IsMechanoid)
				{
					foreach (Gizmo gizmo8 in MechanitorUtility.GetMechGizmos(this))
					{
						yield return gizmo8;
					}
					enumerator = null;
				}
				if (this.RaceProps.Humanlike && this.ageTracker.AgeBiologicalYears < 13 && !this.Drafted && Find.Selector.SelectedPawns.Count < 2 && this.DevelopmentalStage.Child())
				{
					yield return new Gizmo_GrowthTier(this);
					if (DebugSettings.ShowDevGizmos)
					{
						yield return new Command_Action
						{
							defaultLabel = "DEV: Set growth tier",
							action = delegate()
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								for (int i = 0; i < GrowthUtility.GrowthTierPointsRequirements.Length; i++)
								{
									int tier = i;
									list.Add(new FloatMenuOption(tier.ToString(), delegate()
									{
										this.ageTracker.growthPoints = GrowthUtility.GrowthTierPointsRequirements[tier];
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
								Find.WindowStack.Add(new FloatMenu(list));
							}
						};
					}
				}
			}
			if (this.abilities != null)
			{
				foreach (Gizmo gizmo9 in this.abilities.GetGizmos())
				{
					yield return gizmo9;
				}
				enumerator = null;
			}
			if (this.IsColonistPlayerControlled || this.IsColonyMech || this.IsPrisonerOfColony)
			{
				if (this.playerSettings != null)
				{
					foreach (Gizmo gizmo10 in this.playerSettings.GetGizmos())
					{
						yield return gizmo10;
					}
					enumerator = null;
				}
				foreach (Gizmo gizmo11 in this.health.GetGizmos())
				{
					yield return gizmo11;
				}
				enumerator = null;
			}
			if (this.apparel != null)
			{
				foreach (Gizmo gizmo12 in this.apparel.GetGizmos())
				{
					yield return gizmo12;
				}
				enumerator = null;
			}
			if (this.inventory != null)
			{
				foreach (Gizmo gizmo13 in this.inventory.GetGizmos())
				{
					yield return gizmo13;
				}
				enumerator = null;
			}
			foreach (Gizmo gizmo14 in this.mindState.GetGizmos())
			{
				yield return gizmo14;
			}
			enumerator = null;
			if (this.royalty != null && this.IsColonistPlayerControlled)
			{
				bool anyPermitOnCooldown = false;
				foreach (FactionPermit factionPermit in this.royalty.AllFactionPermits)
				{
					if (factionPermit.OnCooldown)
					{
						anyPermitOnCooldown = true;
					}
					IEnumerable<Gizmo> pawnGizmos = factionPermit.Permit.Worker.GetPawnGizmos(this, factionPermit.Faction);
					if (pawnGizmos != null)
					{
						foreach (Gizmo gizmo15 in pawnGizmos)
						{
							yield return gizmo15;
						}
						enumerator = null;
					}
				}
				List<FactionPermit>.Enumerator enumerator2 = default(List<FactionPermit>.Enumerator);
				if (this.royalty.HasAidPermit)
				{
					yield return this.royalty.RoyalAidGizmo();
				}
				if (DebugSettings.ShowDevGizmos && anyPermitOnCooldown)
				{
					yield return new Command_Action
					{
						defaultLabel = "Reset permit cooldowns",
						action = delegate()
						{
							foreach (FactionPermit factionPermit2 in this.royalty.AllFactionPermits)
							{
								factionPermit2.ResetCooldown();
							}
						}
					};
				}
				foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
				{
					if (royalTitle.def.permits != null)
					{
						Faction faction = royalTitle.faction;
						foreach (RoyalTitlePermitDef royalTitlePermitDef in royalTitle.def.permits)
						{
							IEnumerable<Gizmo> pawnGizmos2 = royalTitlePermitDef.Worker.GetPawnGizmos(this, faction);
							if (pawnGizmos2 != null)
							{
								foreach (Gizmo gizmo16 in pawnGizmos2)
								{
									yield return gizmo16;
								}
								enumerator = null;
							}
						}
						List<RoyalTitlePermitDef>.Enumerator enumerator4 = default(List<RoyalTitlePermitDef>.Enumerator);
						faction = null;
					}
				}
				List<RoyalTitle>.Enumerator enumerator3 = default(List<RoyalTitle>.Enumerator);
			}
			foreach (Gizmo gizmo17 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo17;
			}
			enumerator = null;
			if (this.royalty != null && ModsConfig.RoyaltyActive)
			{
				foreach (Gizmo gizmo18 in this.royalty.GetGizmos())
				{
					yield return gizmo18;
				}
				enumerator = null;
			}
			if (this.connections != null && ModsConfig.IdeologyActive)
			{
				foreach (Gizmo gizmo19 in this.connections.GetGizmos())
				{
					yield return gizmo19;
				}
				enumerator = null;
			}
			if (this.genes != null)
			{
				foreach (Gizmo gizmo20 in this.genes.GetGizmos())
				{
					yield return gizmo20;
				}
				enumerator = null;
			}
			Lord lord = this.GetLord();
			if (lord != null && lord.LordJob != null)
			{
				foreach (Gizmo gizmo21 in lord.LordJob.GetPawnGizmos(this))
				{
					yield return gizmo21;
				}
				enumerator = null;
				if (lord.CurLordToil != null)
				{
					foreach (Gizmo gizmo22 in lord.CurLordToil.GetPawnGizmos(this))
					{
						yield return gizmo22;
					}
					enumerator = null;
				}
			}
			if (DebugSettings.ShowDevGizmos && ModsConfig.BiotechActive)
			{
				Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
				if (pawn_RelationsTracker != null && pawn_RelationsTracker.IsTryRomanceOnCooldown)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Reset try romance cooldown",
						action = delegate()
						{
							this.relations.romanceEnableTick = -1;
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x0008DB4C File Offset: 0x0008BD4C
		public virtual IEnumerable<FloatMenuOption> GetExtraFloatMenuOptionsFor(IntVec3 sq)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x0008DB54 File Offset: 0x0008BD54
		public override TipSignal GetTooltip()
		{
			string value = "";
			if (this.gender != Gender.None)
			{
				if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
				{
					value = "PawnTooltipGenderAndKindLabel".Translate(this.GetGenderLabel(), this.KindLabel);
				}
				else
				{
					value = this.GetGenderLabel();
				}
			}
			else if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
			{
				value = this.KindLabel;
			}
			string generalConditionLabel = HealthUtility.GetGeneralConditionLabel(this, false);
			bool flag = !string.IsNullOrEmpty(value);
			string text;
			if (this.equipment != null && this.equipment.Primary != null)
			{
				if (flag)
				{
					text = "PawnTooltipWithDescAndPrimaryEquip".Translate(this.LabelCap, value, this.equipment.Primary.LabelCap, generalConditionLabel);
				}
				else
				{
					text = "PawnTooltipWithPrimaryEquipNoDesc".Translate(this.LabelCap, value, generalConditionLabel);
				}
			}
			else if (flag)
			{
				text = "PawnTooltipWithDescNoPrimaryEquip".Translate(this.LabelCap, value, generalConditionLabel);
			}
			else
			{
				text = "PawnTooltipNoDescNoPrimaryEquip".Translate(this.LabelCap, generalConditionLabel);
			}
			return new TipSignal(text, this.thingIDNumber * 152317, TooltipPriority.Pawn);
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0008DCC4 File Offset: 0x0008BEC4
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (ModsConfig.BiotechActive && this.genes != null && this.genes.Xenotype != XenotypeDefOf.Baseliner)
			{
				string reportText = this.genes.UniqueXenotype ? "UniqueXenotypeDesc".Translate().ToString() : this.DescriptionFlavor;
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Race".Translate(), this.def.LabelCap + " (" + this.genes.XenotypeLabel + ")", reportText, 2100, null, this.genes.UniqueXenotype ? null : Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(this.genes.Xenotype, -1)), false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "BodySize".Translate(), this.BodySize.ToString("F2"), "Stat_Race_BodySize_Desc".Translate(), 500, null, null, false);
			if (this.RaceProps.lifeStageAges.Count > 1)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Growth".Translate(), this.ageTracker.Growth.ToStringPercent(), "Stat_Race_Growth_Desc".Translate(), 2206, null, null, false);
			}
			if (this.IsWildMan())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Wildness".Translate(), 0.75f.ToStringPercent(), TrainableUtility.GetWildnessExplanation(this.def), 2050, null, null, false);
			}
			if (ModsConfig.RoyaltyActive && this.RaceProps.intelligence == Intelligence.Humanlike)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "MeditationFocuses".Translate(), MeditationUtility.FocusTypesAvailableForPawnString(this).CapitalizeFirst(), ("MeditationFocusesPawnDesc".Translate() + "\n\n" + MeditationUtility.FocusTypeAvailableExplanation(this)).Resolve(), 99995, null, MeditationUtility.FocusObjectsForPawnHyperlinks(this), false);
			}
			if (this.apparel != null && !this.apparel.AllRequirements.EnumerableNullOrEmpty<ApparelRequirementWithSource>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ApparelRequirementWithSource apparelRequirementWithSource in this.apparel.AllRequirements)
				{
					string text = null;
					string t;
					if (!ApparelUtility.IsRequirementActive(apparelRequirementWithSource.requirement, apparelRequirementWithSource.Source, this, out t))
					{
						text = " [" + "ApparelRequirementDisabledLabel".Translate() + ": " + t + "]";
					}
					stringBuilder.Append("- ");
					bool flag = true;
					foreach (ThingDef thingDef in apparelRequirementWithSource.requirement.AllRequiredApparelForPawn(this, false, true))
					{
						if (!flag)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(thingDef.LabelCap);
						flag = false;
					}
					if (apparelRequirementWithSource.Source == ApparelRequirementSource.Title)
					{
						stringBuilder.Append(" ");
						stringBuilder.Append("ApparelRequirementOrAnyPsycasterOrPrestigeApparel".Translate());
					}
					stringBuilder.Append(" (");
					stringBuilder.Append("Source".Translate());
					stringBuilder.Append(": ");
					stringBuilder.Append(apparelRequirementWithSource.SourceLabelCap);
					stringBuilder.Append(")");
					if (text != null)
					{
						stringBuilder.Append(text);
					}
					stringBuilder.AppendLine();
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Stat_Pawn_RequiredApparel_Name".Translate(), "", "Stat_Pawn_RequiredApparel_Name".Translate() + ":\n\n" + stringBuilder.ToString(), 100, null, null, false);
			}
			if (ModsConfig.IdeologyActive && this.Ideo != null)
			{
				foreach (StatDrawEntry statDrawEntry2 in DarknessCombatUtility.GetStatEntriesForPawn(this))
				{
					yield return statDrawEntry2;
				}
				enumerator = null;
			}
			if (this.genes != null)
			{
				foreach (StatDrawEntry statDrawEntry3 in this.genes.SpecialDisplayStats())
				{
					yield return statDrawEntry3;
				}
				enumerator = null;
			}
			if (ModsConfig.BiotechActive)
			{
				if (this.RaceProps.Humanlike)
				{
					TaggedString taggedString = "DevelopmentStage_Adult".Translate();
					TaggedString taggedString2 = "StatsReport_DevelopmentStageDesc_Adult".Translate();
					if (this.ageTracker.CurLifeStage.developmentalStage == DevelopmentalStage.Child)
					{
						taggedString = "DevelopmentStage_Child".Translate();
						taggedString2 = "StatsReport_DevelopmentStageDesc_ChildPart1".Translate() + ":\n\n" + (from w in this.RaceProps.lifeStageWorkSettings
						where w.minAge > 0 && w.workType.visible
						select w into d
						select (d.workType.labelShort + " (" + "AgeIndicator".Translate(d.minAge) + ")").RawText).ToLineList("  - ", true) + "\n\n" + "StatsReport_DevelopmentStageDesc_ChildPart2".Translate();
					}
					else if (this.ageTracker.CurLifeStage.developmentalStage == DevelopmentalStage.Baby)
					{
						taggedString = "DevelopmentStage_Baby".Translate();
						taggedString2 = "StatsReport_DevelopmentStageDesc_Baby".Translate();
					}
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "StatsReport_DevelopmentStage".Translate(), taggedString, taggedString2, 4200, null, null, false);
				}
				if (this.IsFreeColonist && this.DevelopmentalStage.Child())
				{
					Need_Learning need_Learning = this.needs.learning;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0008DCD4 File Offset: 0x0008BED4
		public bool Sterile(bool forEmbryoImplantation = false)
		{
			if (!this.ageTracker.CurLifeStage.reproductive)
			{
				return true;
			}
			if (this.RaceProps.Humanlike)
			{
				if (!ModsConfig.BiotechActive)
				{
					return true;
				}
				if (this.GetStatValue(StatDefOf.Fertility, true, -1) <= 0f)
				{
					return true;
				}
			}
			return this.health.hediffSet.HasHediffPreventsPregnancy() || (!forEmbryoImplantation && this.health.hediffSet.HasHediff(HediffDefOf.Lactating, false)) || this.SterileGenes();
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x0008DD60 File Offset: 0x0008BF60
		public bool CurrentlyUsableForBills()
		{
			if (!this.InBed())
			{
				JobFailReason.Is(Pawn.NotSurgeryReadyTrans, null);
				return false;
			}
			if (!this.InteractionCell.IsValid)
			{
				JobFailReason.Is(Pawn.CannotReachTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x0008DDA0 File Offset: 0x0008BFA0
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x0008DDA8 File Offset: 0x0008BFA8
		public void Notify_BillDeleted(Bill bill)
		{
			Xenogerm xenogerm = bill.xenogerm;
			if (xenogerm == null)
			{
				return;
			}
			xenogerm.Notify_BillRemoved();
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0008DDBC File Offset: 0x0008BFBC
		public bool AnythingToStrip()
		{
			if (!this.kindDef.canStrip)
			{
				return false;
			}
			if (this.equipment != null && this.equipment.HasAnything())
			{
				return true;
			}
			if (this.inventory != null && this.inventory.innerContainer.Count > 0)
			{
				return true;
			}
			if (this.apparel != null)
			{
				if (base.Destroyed)
				{
					if (this.apparel.AnyApparel)
					{
						return true;
					}
				}
				else if (this.apparel.AnyApparelUnlocked)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0008DE3C File Offset: 0x0008C03C
		public void Strip()
		{
			Caravan caravan = this.GetCaravan();
			if (caravan != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(this, caravan.PawnsListForReading, null);
				if (this.apparel != null)
				{
					CaravanInventoryUtility.MoveAllApparelToSomeonesInventory(this, caravan.PawnsListForReading, base.Destroyed);
				}
				if (this.equipment != null)
				{
					CaravanInventoryUtility.MoveAllEquipmentToSomeonesInventory(this, caravan.PawnsListForReading);
				}
			}
			else
			{
				IntVec3 pos = (this.Corpse != null) ? this.Corpse.PositionHeld : base.PositionHeld;
				if (this.equipment != null)
				{
					this.equipment.DropAllEquipment(pos, false, false);
				}
				if (this.apparel != null)
				{
					this.apparel.DropAll(pos, false, base.Destroyed, null);
				}
				if (this.inventory != null)
				{
					this.inventory.DropAllNearPawn(pos, false, false);
				}
			}
			if (base.Faction != null)
			{
				base.Faction.Notify_MemberStripped(this, Faction.OfPlayer);
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060017FE RID: 6142 RVA: 0x0008DF0D File Offset: 0x0008C10D
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x0008DF1A File Offset: 0x0008C11A
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x0008DF28 File Offset: 0x0008C128
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0008DF38 File Offset: 0x0008C138
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x0008DF48 File Offset: 0x0008C148
		public void HearClamor(Thing source, ClamorDef type)
		{
			if (this.Dead || this.Downed || this.Deathresting || this.IsSelfShutdown())
			{
				return;
			}
			if (type == ClamorDefOf.Movement || type == ClamorDefOf.BabyCry)
			{
				Pawn pawn = source as Pawn;
				if (pawn != null)
				{
					this.CheckForDisturbedSleep(pawn);
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Harm && base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction == source.Faction && this.HostFaction == null)
			{
				this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
				if (this.CurJob != null)
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Construction && base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction != source.Faction && this.HostFaction == null)
			{
				this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
				if (this.CurJob != null)
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Ability && base.Faction != Faction.OfPlayer && base.Faction != source.Faction && this.HostFaction == null)
			{
				if (!this.Awake())
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null)
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Impact)
			{
				this.mindState.Notify_ClamorImpact(source);
				if (this.CurJob != null && !this.Awake())
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0008E120 File Offset: 0x0008C320
		private void NotifyLordOfClamor(Thing source, ClamorDef type)
		{
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_Clamor(source, type);
			}
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x0008E13F File Offset: 0x0008C33F
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			this.mindState.Notify_Explosion(explosion);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x0008E154 File Offset: 0x0008C354
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
			if (pawn_ApparelTracker == null)
			{
				return;
			}
			pawn_ApparelTracker.Notify_BulletImpactNearby(impactData);
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x0008E168 File Offset: 0x0008C368
		private void CheckForDisturbedSleep(Pawn source)
		{
			if (this.needs.mood == null)
			{
				return;
			}
			if (this.Awake())
			{
				return;
			}
			if (base.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (Find.TickManager.TicksGame < this.lastSleepDisturbedTick + 300)
			{
				return;
			}
			if (this.Deathresting)
			{
				return;
			}
			if (source != null)
			{
				if (LovePartnerRelationUtility.LovePartnerRelationExists(this, source))
				{
					return;
				}
				if (source.RaceProps.petness > 0f)
				{
					return;
				}
				if (source.relations != null)
				{
					if (source.relations.DirectRelations.Any((DirectPawnRelation dr) => dr.def == PawnRelationDefOf.Bond))
					{
						return;
					}
				}
			}
			this.lastSleepDisturbedTick = Find.TickManager.TicksGame;
			this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleepDisturbed, null, null);
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0008E248 File Offset: 0x0008C448
		public float GetAcceptArrestChance(Pawn arrester)
		{
			if (this.Downed || this.WorkTagIsDisabled(WorkTags.Violent) || (this.guilt != null && this.guilt.IsGuilty && this.IsColonist && !this.IsQuestLodger()))
			{
				return 1f;
			}
			return (StatDefOf.ArrestSuccessChance.Worker.IsDisabledFor(arrester) ? StatDefOf.ArrestSuccessChance.valueIfMissing : arrester.GetStatValue(StatDefOf.ArrestSuccessChance, true, -1)) * this.kindDef.acceptArrestChanceFactor;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0008E2C8 File Offset: 0x0008C4C8
		public bool CheckAcceptArrest(Pawn arrester)
		{
			Faction homeFaction = this.HomeFaction;
			if (homeFaction != null && homeFaction != arrester.factionInt)
			{
				homeFaction.Notify_MemberCaptured(this, arrester.Faction);
			}
			if (this.Downed)
			{
				return true;
			}
			if (this.WorkTagIsDisabled(WorkTags.Violent))
			{
				return true;
			}
			float acceptArrestChance = this.GetAcceptArrestChance(arrester);
			if (Rand.Value < acceptArrestChance)
			{
				return true;
			}
			Messages.Message("MessageRefusedArrest".Translate(this.LabelShort, this), this, MessageTypeDefOf.ThreatSmall, true);
			if (base.Faction == null || !arrester.HostileTo(this))
			{
				this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, false, false, null, false, false, false);
			}
			return false;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0008E37C File Offset: 0x0008C57C
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			if (!this.InMentalState && this.GetTraderCaravanRole() == TraderCaravanRole.Carrier && !(this.jobs.curDriver is JobDriver_AttackMelee))
			{
				return true;
			}
			if (this.mindState.duty != null && this.mindState.duty.def.threatDisabled)
			{
				return true;
			}
			if (!this.mindState.Active)
			{
				return true;
			}
			if (this.IsColonyMechRequiringMechanitor())
			{
				return true;
			}
			Pawn pawn = ((disabledFor != null) ? disabledFor.Thing : null) as Pawn;
			if (this.Downed)
			{
				if (disabledFor == null)
				{
					return true;
				}
				if (pawn == null || pawn.mindState == null || pawn.mindState.duty == null || !pawn.mindState.duty.attackDownedIfStarving || !pawn.Starving())
				{
					return true;
				}
			}
			return this.IsInvisible() || (pawn != null && (this.ThreatDisabledBecauseNonAggressiveRoamer(pawn) || pawn.ThreatDisabledBecauseNonAggressiveRoamer(this)));
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x0008E470 File Offset: 0x0008C670
		public bool ThreatDisabledBecauseNonAggressiveRoamer(Pawn otherPawn)
		{
			if (!this.RaceProps.Roamer || base.Faction != Faction.OfPlayer)
			{
				return false;
			}
			Lord lord = otherPawn.GetLord();
			return lord != null && !lord.CurLordToil.AllowAggressiveTargettingOfRoamers && !this.InAggroMentalState && !this.IsFighting() && Find.TickManager.TicksGame >= this.mindState.lastEngageTargetTick + 360;
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0008E4E4 File Offset: 0x0008C6E4
		public List<WorkTypeDef> GetDisabledWorkTypes(bool permanentOnly = false)
		{
			Pawn.<>c__DisplayClass299_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.permanentOnly = permanentOnly;
			if (CS$<>8__locals1.permanentOnly)
			{
				if (this.cachedDisabledWorkTypesPermanent == null)
				{
					this.cachedDisabledWorkTypesPermanent = new List<WorkTypeDef>();
				}
				this.<GetDisabledWorkTypes>g__FillList|299_0(this.cachedDisabledWorkTypesPermanent, ref CS$<>8__locals1);
				return this.cachedDisabledWorkTypesPermanent;
			}
			if (this.cachedDisabledWorkTypes == null)
			{
				this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
			}
			this.<GetDisabledWorkTypes>g__FillList|299_0(this.cachedDisabledWorkTypes, ref CS$<>8__locals1);
			return this.cachedDisabledWorkTypes;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0008E558 File Offset: 0x0008C758
		public List<string> GetReasonsForDisabledWorkType(WorkTypeDef workType)
		{
			if (this.cachedReasonsForDisabledWorkTypes != null && this.cachedReasonsForDisabledWorkTypes.ContainsKey(workType))
			{
				return this.cachedReasonsForDisabledWorkTypes[workType];
			}
			List<string> list = new List<string>();
			foreach (BackstoryDef backstoryDef in this.story.AllBackstories)
			{
				foreach (WorkTypeDef workTypeDef in backstoryDef.DisabledWorkTypes)
				{
					if (workType == workTypeDef)
					{
						list.Add("WorkDisabledByBackstory".Translate(backstoryDef.TitleCapFor(this.gender)));
						break;
					}
				}
			}
			for (int i = 0; i < this.story.traits.allTraits.Count; i++)
			{
				Trait trait = this.story.traits.allTraits[i];
				using (List<WorkTypeDef>.Enumerator enumerator2 = trait.GetDisabledWorkTypes().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current == workType && !trait.Suppressed)
						{
							list.Add("WorkDisabledByTrait".Translate(trait.LabelCap));
							break;
						}
					}
				}
			}
			if (this.royalty != null)
			{
				foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
				{
					if (royalTitle.conceited)
					{
						foreach (WorkTypeDef workTypeDef2 in royalTitle.def.DisabledWorkTypes)
						{
							if (workType == workTypeDef2)
							{
								list.Add("WorkDisabledByRoyalTitle".Translate(royalTitle.Label));
								break;
							}
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive && this.Ideo != null)
			{
				Precept_Role role = this.Ideo.GetRole(this);
				if (role != null)
				{
					foreach (WorkTypeDef workTypeDef3 in role.DisabledWorkTypes)
					{
						if (workType == workTypeDef3)
						{
							list.Add("WorkDisabledRole".Translate(role.LabelForPawn(this)));
							break;
						}
					}
				}
			}
			foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
			{
				foreach (WorkTypeDef workTypeDef4 in questPart_WorkDisabled.DisabledWorkTypes)
				{
					if (workType == workTypeDef4)
					{
						list.Add("WorkDisabledByQuest".Translate(questPart_WorkDisabled.quest.name));
						break;
					}
				}
			}
			if (this.guest != null && this.guest.IsSlave)
			{
				foreach (WorkTypeDef workTypeDef5 in this.guest.GetDisabledWorkTypes())
				{
					if (workType == workTypeDef5)
					{
						list.Add("WorkDisabledSlave".Translate());
						break;
					}
				}
			}
			int value;
			if (this.IsWorkTypeDisabledByAge(workType, out value))
			{
				list.Add("WorkDisabledAge".Translate(this, this.ageTracker.AgeBiologicalYears, workType.labelShort, value));
			}
			if (this.cachedReasonsForDisabledWorkTypes == null)
			{
				this.cachedReasonsForDisabledWorkTypes = new Dictionary<WorkTypeDef, List<string>>();
			}
			this.cachedReasonsForDisabledWorkTypes[workType] = list;
			return list;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0008E9B0 File Offset: 0x0008CBB0
		public bool WorkTypeIsDisabled(WorkTypeDef w)
		{
			return this.GetDisabledWorkTypes(false).Contains(w);
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0008E9C0 File Offset: 0x0008CBC0
		public bool OneOfWorkTypesIsDisabled(List<WorkTypeDef> wts)
		{
			for (int i = 0; i < wts.Count; i++)
			{
				if (this.WorkTypeIsDisabled(wts[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x0008E9F0 File Offset: 0x0008CBF0
		public void Notify_DisabledWorkTypesChanged()
		{
			this.cachedDisabledWorkTypes = null;
			this.cachedDisabledWorkTypesPermanent = null;
			this.cachedReasonsForDisabledWorkTypes = null;
			Pawn_WorkSettings pawn_WorkSettings = this.workSettings;
			if (pawn_WorkSettings != null)
			{
				pawn_WorkSettings.Notify_DisabledWorkTypesChanged();
			}
			Pawn_SkillTracker pawn_SkillTracker = this.skills;
			if (pawn_SkillTracker == null)
			{
				return;
			}
			pawn_SkillTracker.Notify_SkillDisablesChanged();
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x0008EA28 File Offset: 0x0008CC28
		public WorkTags CombinedDisabledWorkTags
		{
			get
			{
				WorkTags workTags = (this.story != null) ? this.story.DisabledWorkTagsBackstoryTraitsAndGenes : WorkTags.None;
				if (this.royalty != null)
				{
					foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
					{
						if (royalTitle.conceited)
						{
							workTags |= royalTitle.def.disabledWorkTags;
						}
					}
				}
				if (ModsConfig.IdeologyActive && this.Ideo != null)
				{
					Precept_Role role = this.Ideo.GetRole(this);
					if (role != null)
					{
						workTags |= role.def.roleDisabledWorkTags;
					}
				}
				if (this.health != null && this.health.hediffSet != null)
				{
					foreach (Hediff hediff in this.health.hediffSet.hediffs)
					{
						HediffStage curStage = hediff.CurStage;
						if (curStage != null)
						{
							workTags |= curStage.disabledWorkTags;
						}
					}
				}
				foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
				{
					workTags |= questPart_WorkDisabled.disabledWorkTags;
				}
				return workTags;
			}
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x0008EB94 File Offset: 0x0008CD94
		public bool WorkTagIsDisabled(WorkTags w)
		{
			return (this.CombinedDisabledWorkTags & w) > WorkTags.None;
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0008EBA4 File Offset: 0x0008CDA4
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (base.Faction.HostileTo(Faction.OfPlayer) && this.HostFaction == null && !this.Downed && !this.InMentalState)
			{
				reason = "Enemies".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x0008EBF2 File Offset: 0x0008CDF2
		public void ChangeKind(PawnKindDef newKindDef)
		{
			if (this.kindDef == newKindDef)
			{
				return;
			}
			this.kindDef = newKindDef;
			if (this.kindDef == PawnKindDefOf.WildMan)
			{
				this.mindState.WildManEverReachedOutside = false;
				ReachabilityUtility.ClearCacheFor(this);
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x0008EC24 File Offset: 0x0008CE24
		public bool HasPsylink
		{
			get
			{
				return this.psychicEntropy != null && this.psychicEntropy.Psylink != null;
			}
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x0008EE04 File Offset: 0x0008D004
		[CompilerGenerated]
		private void <GetDisabledWorkTypes>g__FillList|299_0(List<WorkTypeDef> list, ref Pawn.<>c__DisplayClass299_0 A_2)
		{
			if (this.story != null && !this.IsSlave)
			{
				foreach (BackstoryDef backstoryDef in this.story.AllBackstories)
				{
					foreach (WorkTypeDef item in backstoryDef.DisabledWorkTypes)
					{
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
				}
				for (int i = 0; i < this.story.traits.allTraits.Count; i++)
				{
					if (!this.story.traits.allTraits[i].Suppressed)
					{
						foreach (WorkTypeDef item2 in this.story.traits.allTraits[i].GetDisabledWorkTypes())
						{
							if (!list.Contains(item2))
							{
								list.Add(item2);
							}
						}
					}
				}
			}
			if (ModsConfig.BiotechActive && this.IsColonyMech)
			{
				List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					if (!this.RaceProps.mechEnabledWorkTypes.Contains(allDefsListForReading[j]) && !list.Contains(allDefsListForReading[j]))
					{
						list.Add(allDefsListForReading[j]);
					}
				}
			}
			if (!A_2.permanentOnly)
			{
				if (this.royalty != null && !this.IsSlave)
				{
					foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
					{
						if (royalTitle.conceited)
						{
							foreach (WorkTypeDef item3 in royalTitle.def.DisabledWorkTypes)
							{
								if (!list.Contains(item3))
								{
									list.Add(item3);
								}
							}
						}
					}
				}
				if (ModsConfig.IdeologyActive && this.Ideo != null)
				{
					Precept_Role role = this.Ideo.GetRole(this);
					if (role != null)
					{
						foreach (WorkTypeDef item4 in role.DisabledWorkTypes)
						{
							if (!list.Contains(item4))
							{
								list.Add(item4);
							}
						}
					}
				}
				if (ModsConfig.BiotechActive && this.genes != null)
				{
					foreach (Gene gene in this.genes.GenesListForReading)
					{
						foreach (WorkTypeDef item5 in gene.DisabledWorkTypes)
						{
							if (!list.Contains(item5))
							{
								list.Add(item5);
							}
						}
					}
				}
				foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
				{
					foreach (WorkTypeDef item6 in questPart_WorkDisabled.DisabledWorkTypes)
					{
						if (!list.Contains(item6))
						{
							list.Add(item6);
						}
					}
				}
				if (this.guest != null)
				{
					foreach (WorkTypeDef item7 in this.guest.GetDisabledWorkTypes())
					{
						if (!list.Contains(item7))
						{
							list.Add(item7);
						}
					}
				}
				for (int k = 0; k < this.RaceProps.lifeStageWorkSettings.Count; k++)
				{
					LifeStageWorkSettings lifeStageWorkSettings = this.RaceProps.lifeStageWorkSettings[k];
					if (lifeStageWorkSettings.IsDisabled(this) && !list.Contains(lifeStageWorkSettings.workType))
					{
						list.Add(lifeStageWorkSettings.workType);
					}
				}
			}
		}

		// Token: 0x040011DB RID: 4571
		public PawnKindDef kindDef;

		// Token: 0x040011DC RID: 4572
		private Name nameInt;

		// Token: 0x040011DD RID: 4573
		public Gender gender;

		// Token: 0x040011DE RID: 4574
		public Pawn_AgeTracker ageTracker;

		// Token: 0x040011DF RID: 4575
		public Pawn_HealthTracker health;

		// Token: 0x040011E0 RID: 4576
		public Pawn_RecordsTracker records;

		// Token: 0x040011E1 RID: 4577
		public Pawn_InventoryTracker inventory;

		// Token: 0x040011E2 RID: 4578
		public Pawn_MeleeVerbs meleeVerbs;

		// Token: 0x040011E3 RID: 4579
		public VerbTracker verbTracker;

		// Token: 0x040011E4 RID: 4580
		public Pawn_Ownership ownership;

		// Token: 0x040011E5 RID: 4581
		public Pawn_CarryTracker carryTracker;

		// Token: 0x040011E6 RID: 4582
		public Pawn_NeedsTracker needs;

		// Token: 0x040011E7 RID: 4583
		public Pawn_MindState mindState;

		// Token: 0x040011E8 RID: 4584
		public Pawn_SurroundingsTracker surroundings;

		// Token: 0x040011E9 RID: 4585
		public Pawn_Thinker thinker;

		// Token: 0x040011EA RID: 4586
		public Pawn_JobTracker jobs;

		// Token: 0x040011EB RID: 4587
		public Pawn_StanceTracker stances;

		// Token: 0x040011EC RID: 4588
		public Pawn_RotationTracker rotationTracker;

		// Token: 0x040011ED RID: 4589
		public Pawn_PathFollower pather;

		// Token: 0x040011EE RID: 4590
		public Pawn_NativeVerbs natives;

		// Token: 0x040011EF RID: 4591
		public Pawn_FilthTracker filth;

		// Token: 0x040011F0 RID: 4592
		public Pawn_RopeTracker roping;

		// Token: 0x040011F1 RID: 4593
		public Pawn_EquipmentTracker equipment;

		// Token: 0x040011F2 RID: 4594
		public Pawn_ApparelTracker apparel;

		// Token: 0x040011F3 RID: 4595
		public Pawn_SkillTracker skills;

		// Token: 0x040011F4 RID: 4596
		public Pawn_StoryTracker story;

		// Token: 0x040011F5 RID: 4597
		public Pawn_GuestTracker guest;

		// Token: 0x040011F6 RID: 4598
		public Pawn_GuiltTracker guilt;

		// Token: 0x040011F7 RID: 4599
		public Pawn_RoyaltyTracker royalty;

		// Token: 0x040011F8 RID: 4600
		public Pawn_AbilityTracker abilities;

		// Token: 0x040011F9 RID: 4601
		public Pawn_IdeoTracker ideo;

		// Token: 0x040011FA RID: 4602
		public Pawn_GeneTracker genes;

		// Token: 0x040011FB RID: 4603
		public Pawn_WorkSettings workSettings;

		// Token: 0x040011FC RID: 4604
		public Pawn_TraderTracker trader;

		// Token: 0x040011FD RID: 4605
		public Pawn_StyleTracker style;

		// Token: 0x040011FE RID: 4606
		public Pawn_StyleObserverTracker styleObserver;

		// Token: 0x040011FF RID: 4607
		public Pawn_ConnectionsTracker connections;

		// Token: 0x04001200 RID: 4608
		public Pawn_TrainingTracker training;

		// Token: 0x04001201 RID: 4609
		public Pawn_CallTracker caller;

		// Token: 0x04001202 RID: 4610
		public Pawn_PsychicEntropyTracker psychicEntropy;

		// Token: 0x04001203 RID: 4611
		public Pawn_RelationsTracker relations;

		// Token: 0x04001204 RID: 4612
		public Pawn_InteractionsTracker interactions;

		// Token: 0x04001205 RID: 4613
		public Pawn_PlayerSettings playerSettings;

		// Token: 0x04001206 RID: 4614
		public Pawn_OutfitTracker outfits;

		// Token: 0x04001207 RID: 4615
		public Pawn_DrugPolicyTracker drugs;

		// Token: 0x04001208 RID: 4616
		public Pawn_FoodRestrictionTracker foodRestriction;

		// Token: 0x04001209 RID: 4617
		public Pawn_TimetableTracker timetable;

		// Token: 0x0400120A RID: 4618
		public Pawn_InventoryStockTracker inventoryStock;

		// Token: 0x0400120B RID: 4619
		public Pawn_MechanitorTracker mechanitor;

		// Token: 0x0400120C RID: 4620
		public Pawn_LearningTracker learning;

		// Token: 0x0400120D RID: 4621
		public Pawn_DraftController drafter;

		// Token: 0x0400120E RID: 4622
		private Pawn_DrawTracker drawer;

		// Token: 0x0400120F RID: 4623
		public int becameWorldPawnTickAbs = -1;

		// Token: 0x04001210 RID: 4624
		public bool teleporting;

		// Token: 0x04001211 RID: 4625
		public bool forceNoDeathNotification;

		// Token: 0x04001212 RID: 4626
		public int showNamePromptOnTick = -1;

		// Token: 0x04001213 RID: 4627
		public int babyNamingDeadline = -1;

		// Token: 0x04001214 RID: 4628
		private Sustainer sustainerAmbient;

		// Token: 0x04001215 RID: 4629
		private const float HumanSizedHeatOutput = 0.3f;

		// Token: 0x04001216 RID: 4630
		private const float AnimalHeatOutputFactor = 0.6f;

		// Token: 0x04001217 RID: 4631
		public const int DefaultBabyNamingPeriod = 60000;

		// Token: 0x04001218 RID: 4632
		public const int DefaultGrowthMomentChoicePeriod = 120000;

		// Token: 0x04001219 RID: 4633
		private static string NotSurgeryReadyTrans;

		// Token: 0x0400121A RID: 4634
		private static string CannotReachTrans;

		// Token: 0x0400121B RID: 4635
		public const int MaxMoveTicks = 450;

		// Token: 0x0400121C RID: 4636
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x0400121D RID: 4637
		private static List<string> states = new List<string>();

		// Token: 0x0400121E RID: 4638
		private int lastSleepDisturbedTick;

		// Token: 0x0400121F RID: 4639
		private const int SleepDisturbanceMinInterval = 300;

		// Token: 0x04001220 RID: 4640
		private List<WorkTypeDef> cachedDisabledWorkTypes;

		// Token: 0x04001221 RID: 4641
		private List<WorkTypeDef> cachedDisabledWorkTypesPermanent;

		// Token: 0x04001222 RID: 4642
		private Dictionary<WorkTypeDef, List<string>> cachedReasonsForDisabledWorkTypes;
	}
}

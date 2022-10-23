using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200036A RID: 874
	public struct PawnGenerationRequest
	{
		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001865 RID: 6245 RVA: 0x000949CA File Offset: 0x00092BCA
		// (set) Token: 0x06001866 RID: 6246 RVA: 0x000949EC File Offset: 0x00092BEC
		public PawnKindDef KindDef
		{
			get
			{
				if (this.PawnKindDefGetter != null)
				{
					return this.PawnKindDefGetter(this.ForcedXenotype);
				}
				return this.kindDefInner;
			}
			set
			{
				this.kindDefInner = value;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001867 RID: 6247 RVA: 0x000949F5 File Offset: 0x00092BF5
		// (set) Token: 0x06001868 RID: 6248 RVA: 0x000949FD File Offset: 0x00092BFD
		public PawnGenerationContext Context { get; set; }

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001869 RID: 6249 RVA: 0x00094A06 File Offset: 0x00092C06
		// (set) Token: 0x0600186A RID: 6250 RVA: 0x00094A0E File Offset: 0x00092C0E
		public Faction Faction { get; set; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x0600186B RID: 6251 RVA: 0x00094A17 File Offset: 0x00092C17
		// (set) Token: 0x0600186C RID: 6252 RVA: 0x00094A1F File Offset: 0x00092C1F
		public int Tile { get; set; }

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x0600186D RID: 6253 RVA: 0x00094A28 File Offset: 0x00092C28
		// (set) Token: 0x0600186E RID: 6254 RVA: 0x00094A30 File Offset: 0x00092C30
		public bool ForceGenerateNewPawn { get; set; }

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600186F RID: 6255 RVA: 0x00094A39 File Offset: 0x00092C39
		// (set) Token: 0x06001870 RID: 6256 RVA: 0x00094A41 File Offset: 0x00092C41
		public BodyTypeDef ForceBodyType { get; set; }

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001871 RID: 6257 RVA: 0x00094A4A File Offset: 0x00092C4A
		// (set) Token: 0x06001872 RID: 6258 RVA: 0x00094A52 File Offset: 0x00092C52
		public bool AllowDead { get; set; }

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001873 RID: 6259 RVA: 0x00094A5B File Offset: 0x00092C5B
		// (set) Token: 0x06001874 RID: 6260 RVA: 0x00094A63 File Offset: 0x00092C63
		public bool AllowDowned { get; set; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001875 RID: 6261 RVA: 0x00094A6C File Offset: 0x00092C6C
		// (set) Token: 0x06001876 RID: 6262 RVA: 0x00094A74 File Offset: 0x00092C74
		public bool CanGeneratePawnRelations { get; set; }

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001877 RID: 6263 RVA: 0x00094A7D File Offset: 0x00092C7D
		// (set) Token: 0x06001878 RID: 6264 RVA: 0x00094A85 File Offset: 0x00092C85
		public bool MustBeCapableOfViolence { get; set; }

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001879 RID: 6265 RVA: 0x00094A8E File Offset: 0x00092C8E
		// (set) Token: 0x0600187A RID: 6266 RVA: 0x00094A96 File Offset: 0x00092C96
		public float ColonistRelationChanceFactor { get; set; }

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600187B RID: 6267 RVA: 0x00094A9F File Offset: 0x00092C9F
		// (set) Token: 0x0600187C RID: 6268 RVA: 0x00094AA7 File Offset: 0x00092CA7
		public bool ForceAddFreeWarmLayerIfNeeded { get; set; }

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x00094AB0 File Offset: 0x00092CB0
		// (set) Token: 0x0600187E RID: 6270 RVA: 0x00094AB8 File Offset: 0x00092CB8
		public bool AllowGay { get; set; }

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x00094AC1 File Offset: 0x00092CC1
		// (set) Token: 0x06001880 RID: 6272 RVA: 0x00094AC9 File Offset: 0x00092CC9
		public bool AllowPregnant { get; set; }

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001881 RID: 6273 RVA: 0x00094AD2 File Offset: 0x00092CD2
		// (set) Token: 0x06001882 RID: 6274 RVA: 0x00094ADA File Offset: 0x00092CDA
		public bool AllowFood { get; set; }

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001883 RID: 6275 RVA: 0x00094AE3 File Offset: 0x00092CE3
		// (set) Token: 0x06001884 RID: 6276 RVA: 0x00094AEB File Offset: 0x00092CEB
		public bool AllowAddictions { get; set; }

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x00094AF4 File Offset: 0x00092CF4
		// (set) Token: 0x06001886 RID: 6278 RVA: 0x00094AFC File Offset: 0x00092CFC
		public IEnumerable<TraitDef> ForcedTraits { get; set; }

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x00094B05 File Offset: 0x00092D05
		// (set) Token: 0x06001888 RID: 6280 RVA: 0x00094B0D File Offset: 0x00092D0D
		public IEnumerable<TraitDef> ProhibitedTraits { get; set; }

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x00094B16 File Offset: 0x00092D16
		// (set) Token: 0x0600188A RID: 6282 RVA: 0x00094B1E File Offset: 0x00092D1E
		public bool Inhabitant { get; set; }

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x00094B27 File Offset: 0x00092D27
		// (set) Token: 0x0600188C RID: 6284 RVA: 0x00094B2F File Offset: 0x00092D2F
		public bool CertainlyBeenInCryptosleep { get; set; }

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x0600188D RID: 6285 RVA: 0x00094B38 File Offset: 0x00092D38
		// (set) Token: 0x0600188E RID: 6286 RVA: 0x00094B40 File Offset: 0x00092D40
		public bool ForceRedressWorldPawnIfFormerColonist { get; set; }

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x0600188F RID: 6287 RVA: 0x00094B49 File Offset: 0x00092D49
		// (set) Token: 0x06001890 RID: 6288 RVA: 0x00094B51 File Offset: 0x00092D51
		public bool WorldPawnFactionDoesntMatter { get; set; }

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001891 RID: 6289 RVA: 0x00094B5A File Offset: 0x00092D5A
		// (set) Token: 0x06001892 RID: 6290 RVA: 0x00094B62 File Offset: 0x00092D62
		public float BiocodeWeaponChance { get; set; }

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001893 RID: 6291 RVA: 0x00094B6B File Offset: 0x00092D6B
		// (set) Token: 0x06001894 RID: 6292 RVA: 0x00094B73 File Offset: 0x00092D73
		public float BiocodeApparelChance { get; set; }

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001895 RID: 6293 RVA: 0x00094B7C File Offset: 0x00092D7C
		// (set) Token: 0x06001896 RID: 6294 RVA: 0x00094B84 File Offset: 0x00092D84
		public Pawn ExtraPawnForExtraRelationChance { get; set; }

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001897 RID: 6295 RVA: 0x00094B8D File Offset: 0x00092D8D
		// (set) Token: 0x06001898 RID: 6296 RVA: 0x00094B95 File Offset: 0x00092D95
		public float RelationWithExtraPawnChanceFactor { get; set; }

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001899 RID: 6297 RVA: 0x00094B9E File Offset: 0x00092D9E
		// (set) Token: 0x0600189A RID: 6298 RVA: 0x00094BA6 File Offset: 0x00092DA6
		public Predicate<Pawn> RedressValidator { get; set; }

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x0600189B RID: 6299 RVA: 0x00094BAF File Offset: 0x00092DAF
		// (set) Token: 0x0600189C RID: 6300 RVA: 0x00094BB7 File Offset: 0x00092DB7
		public Predicate<Pawn> ValidatorPreGear { get; set; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x0600189D RID: 6301 RVA: 0x00094BC0 File Offset: 0x00092DC0
		// (set) Token: 0x0600189E RID: 6302 RVA: 0x00094BC8 File Offset: 0x00092DC8
		public Predicate<Pawn> ValidatorPostGear { get; set; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600189F RID: 6303 RVA: 0x00094BD1 File Offset: 0x00092DD1
		// (set) Token: 0x060018A0 RID: 6304 RVA: 0x00094BD9 File Offset: 0x00092DD9
		public float? MinChanceToRedressWorldPawn { get; set; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060018A1 RID: 6305 RVA: 0x00094BE2 File Offset: 0x00092DE2
		// (set) Token: 0x060018A2 RID: 6306 RVA: 0x00094BEA File Offset: 0x00092DEA
		public float? FixedBiologicalAge { get; set; }

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x00094BF3 File Offset: 0x00092DF3
		// (set) Token: 0x060018A4 RID: 6308 RVA: 0x00094BFB File Offset: 0x00092DFB
		public float? FixedChronologicalAge { get; set; }

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x00094C04 File Offset: 0x00092E04
		// (set) Token: 0x060018A6 RID: 6310 RVA: 0x00094C0C File Offset: 0x00092E0C
		public Gender? FixedGender { get; set; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x060018A7 RID: 6311 RVA: 0x00094C15 File Offset: 0x00092E15
		// (set) Token: 0x060018A8 RID: 6312 RVA: 0x00094C1D File Offset: 0x00092E1D
		public string FixedLastName { get; set; }

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x00094C26 File Offset: 0x00092E26
		// (set) Token: 0x060018AA RID: 6314 RVA: 0x00094C2E File Offset: 0x00092E2E
		public string FixedBirthName { get; set; }

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x00094C37 File Offset: 0x00092E37
		// (set) Token: 0x060018AC RID: 6316 RVA: 0x00094C3F File Offset: 0x00092E3F
		public RoyalTitleDef FixedTitle { get; set; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x00094C48 File Offset: 0x00092E48
		// (set) Token: 0x060018AE RID: 6318 RVA: 0x00094C50 File Offset: 0x00092E50
		public bool ForbidAnyTitle { get; set; }

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x00094C59 File Offset: 0x00092E59
		// (set) Token: 0x060018B0 RID: 6320 RVA: 0x00094C61 File Offset: 0x00092E61
		public Ideo FixedIdeo { get; set; }

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x00094C6A File Offset: 0x00092E6A
		// (set) Token: 0x060018B2 RID: 6322 RVA: 0x00094C72 File Offset: 0x00092E72
		public bool ForceNoIdeo { get; set; }

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x00094C7B File Offset: 0x00092E7B
		// (set) Token: 0x060018B4 RID: 6324 RVA: 0x00094C83 File Offset: 0x00092E83
		public bool ForceNoBackstory { get; set; }

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x00094C8C File Offset: 0x00092E8C
		// (set) Token: 0x060018B6 RID: 6326 RVA: 0x00094C94 File Offset: 0x00092E94
		public bool ForceDead { get; set; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x00094C9D File Offset: 0x00092E9D
		// (set) Token: 0x060018B8 RID: 6328 RVA: 0x00094CA5 File Offset: 0x00092EA5
		public List<GeneDef> ForcedXenogenes { get; set; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x00094CAE File Offset: 0x00092EAE
		// (set) Token: 0x060018BA RID: 6330 RVA: 0x00094CB6 File Offset: 0x00092EB6
		public List<GeneDef> ForcedEndogenes { get; set; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x060018BB RID: 6331 RVA: 0x00094CBF File Offset: 0x00092EBF
		// (set) Token: 0x060018BC RID: 6332 RVA: 0x00094CC7 File Offset: 0x00092EC7
		public XenotypeDef ForcedXenotype { get; set; }

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x060018BD RID: 6333 RVA: 0x00094CD0 File Offset: 0x00092ED0
		// (set) Token: 0x060018BE RID: 6334 RVA: 0x00094CD8 File Offset: 0x00092ED8
		public List<XenotypeDef> AllowedXenotypes { get; set; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x00094CE1 File Offset: 0x00092EE1
		// (set) Token: 0x060018C0 RID: 6336 RVA: 0x00094CE9 File Offset: 0x00092EE9
		public float ForceBaselinerChance { get; set; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x060018C1 RID: 6337 RVA: 0x00094CF2 File Offset: 0x00092EF2
		// (set) Token: 0x060018C2 RID: 6338 RVA: 0x00094CFA File Offset: 0x00092EFA
		public CustomXenotype ForcedCustomXenotype { get; set; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x00094D03 File Offset: 0x00092F03
		// (set) Token: 0x060018C4 RID: 6340 RVA: 0x00094D0B File Offset: 0x00092F0B
		public DevelopmentalStage AllowedDevelopmentalStages { get; set; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x060018C5 RID: 6341 RVA: 0x00094D14 File Offset: 0x00092F14
		// (set) Token: 0x060018C6 RID: 6342 RVA: 0x00094D1C File Offset: 0x00092F1C
		public Func<XenotypeDef, PawnKindDef> PawnKindDefGetter { get; set; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x00094D25 File Offset: 0x00092F25
		// (set) Token: 0x060018C8 RID: 6344 RVA: 0x00094D2D File Offset: 0x00092F2D
		public FloatRange? ExcludeBiologicalAgeRange { get; set; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x00094D36 File Offset: 0x00092F36
		// (set) Token: 0x060018CA RID: 6346 RVA: 0x00094D3E File Offset: 0x00092F3E
		public FloatRange? BiologicalAgeRange { get; set; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x00094D47 File Offset: 0x00092F47
		// (set) Token: 0x060018CC RID: 6348 RVA: 0x00094D4F File Offset: 0x00092F4F
		public bool ForceRecruitable { get; set; }

		// Token: 0x060018CD RID: 6349 RVA: 0x00094D58 File Offset: 0x00092F58
		public PawnGenerationRequest(PawnKindDef kind, Faction faction = null, PawnGenerationContext context = PawnGenerationContext.NonPlayer, int tile = -1, bool forceGenerateNewPawn = false, bool allowDead = false, bool allowDowned = false, bool canGeneratePawnRelations = true, bool mustBeCapableOfViolence = false, float colonistRelationChanceFactor = 1f, bool forceAddFreeWarmLayerIfNeeded = false, bool allowGay = true, bool allowPregnant = false, bool allowFood = true, bool allowAddictions = true, bool inhabitant = false, bool certainlyBeenInCryptosleep = false, bool forceRedressWorldPawnIfFormerColonist = false, bool worldPawnFactionDoesntMatter = false, float biocodeWeaponChance = 0f, float biocodeApparelChance = 0f, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 1f, Predicate<Pawn> validatorPreGear = null, Predicate<Pawn> validatorPostGear = null, IEnumerable<TraitDef> forcedTraits = null, IEnumerable<TraitDef> prohibitedTraits = null, float? minChanceToRedressWorldPawn = null, float? fixedBiologicalAge = null, float? fixedChronologicalAge = null, Gender? fixedGender = null, string fixedLastName = null, string fixedBirthName = null, RoyalTitleDef fixedTitle = null, Ideo fixedIdeo = null, bool forceNoIdeo = false, bool forceNoBackstory = false, bool forbidAnyTitle = false, bool forceDead = false, List<GeneDef> forcedXenogenes = null, List<GeneDef> forcedEndogenes = null, XenotypeDef forcedXenotype = null, CustomXenotype forcedCustomXenotype = null, List<XenotypeDef> allowedXenotypes = null, float forceBaselinerChance = 0f, DevelopmentalStage developmentalStages = DevelopmentalStage.Adult, Func<XenotypeDef, PawnKindDef> pawnKindDefGetter = null, FloatRange? excludeBiologicalAgeRange = null, FloatRange? biologicalAgeRange = null, bool forceRecruitable = false)
		{
			this = default(PawnGenerationRequest);
			this._calledTheCorrectConstructor = true;
			this.KindDef = kind;
			this.Context = context;
			this.Faction = faction;
			this.Tile = tile;
			this.ForceGenerateNewPawn = forceGenerateNewPawn;
			this.AllowDead = allowDead;
			this.AllowDowned = allowDowned;
			this.CanGeneratePawnRelations = canGeneratePawnRelations;
			this.MustBeCapableOfViolence = mustBeCapableOfViolence;
			this.ColonistRelationChanceFactor = colonistRelationChanceFactor;
			this.ForceAddFreeWarmLayerIfNeeded = forceAddFreeWarmLayerIfNeeded;
			this.AllowGay = allowGay;
			this.AllowPregnant = allowPregnant;
			this.AllowFood = allowFood;
			this.AllowAddictions = allowAddictions;
			this.ForcedTraits = forcedTraits;
			this.ProhibitedTraits = prohibitedTraits;
			this.Inhabitant = inhabitant;
			this.CertainlyBeenInCryptosleep = certainlyBeenInCryptosleep;
			this.ForceRedressWorldPawnIfFormerColonist = forceRedressWorldPawnIfFormerColonist;
			this.WorldPawnFactionDoesntMatter = worldPawnFactionDoesntMatter;
			this.ExtraPawnForExtraRelationChance = extraPawnForExtraRelationChance;
			this.RelationWithExtraPawnChanceFactor = relationWithExtraPawnChanceFactor;
			this.BiocodeWeaponChance = biocodeWeaponChance;
			this.BiocodeApparelChance = biocodeApparelChance;
			this.ForceNoIdeo = forceNoIdeo;
			this.ForceNoBackstory = forceNoBackstory;
			this.ForbidAnyTitle = forbidAnyTitle;
			this.ValidatorPreGear = validatorPreGear;
			this.ValidatorPostGear = validatorPostGear;
			this.MinChanceToRedressWorldPawn = minChanceToRedressWorldPawn;
			this.FixedBiologicalAge = fixedBiologicalAge;
			this.FixedChronologicalAge = fixedChronologicalAge;
			this.FixedGender = fixedGender;
			this.FixedLastName = fixedLastName;
			this.FixedBirthName = fixedBirthName;
			this.FixedTitle = fixedTitle;
			this.FixedIdeo = fixedIdeo;
			this.ForceDead = forceDead;
			this.ForcedXenotype = forcedXenotype;
			this.ForcedCustomXenotype = forcedCustomXenotype;
			this.AllowedXenotypes = allowedXenotypes;
			this.ForceBaselinerChance = forceBaselinerChance;
			this.AllowedDevelopmentalStages = developmentalStages;
			this.PawnKindDefGetter = pawnKindDefGetter;
			this.ExcludeBiologicalAgeRange = excludeBiologicalAgeRange;
			this.BiologicalAgeRange = biologicalAgeRange;
			this.ForceRecruitable = forceRecruitable;
			if (forcedXenogenes != null)
			{
				foreach (GeneDef gene in forcedXenogenes)
				{
					this.AddForcedGene(gene, true);
				}
			}
			if (forcedEndogenes != null)
			{
				foreach (GeneDef gene2 in forcedEndogenes)
				{
					this.AddForcedGene(gene2, false);
				}
			}
			this.ValidateAndFix();
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x00094F80 File Offset: 0x00093180
		public void ValidateAndFix()
		{
			if (!this._calledTheCorrectConstructor)
			{
				Log.Error("This PawnGenerationRequest was not created through the correct constructor.");
				this._calledTheCorrectConstructor = true;
			}
			if (this.Context == PawnGenerationContext.All)
			{
				Log.Error("Should not generate pawns with context 'All'");
				this.Context = PawnGenerationContext.NonPlayer;
			}
			if (this.Inhabitant && (this.Tile == -1 || Current.Game.FindMap(this.Tile) == null))
			{
				Log.Error("Trying to generate an inhabitant but map is null.");
				this.Inhabitant = false;
			}
			if (this.ForceNoIdeo && this.FixedIdeo != null)
			{
				Log.Error("Trying to generate a pawn with no ideo and a fixed ideo.");
				this.ForceNoIdeo = false;
			}
			if ((this.AllowedDevelopmentalStages.Newborn() || this.AllowedDevelopmentalStages.Baby()) && this.FixedIdeo != null)
			{
				Log.Error("Trying to generate baby with specific ideology (babies have no ideology).");
				this.FixedIdeo = null;
			}
			string arg;
			if (!this.AllowDowned && this.AlwaysDownedLifeStages(out arg))
			{
				Log.Error(string.Format("Trying to generate a non-downed {0} {1} pawn but that would include these downed lifestages: {2}", this.AllowedDevelopmentalStages, this.KindDef.label, arg));
				this.AllowDowned = true;
			}
			if (this.AllowedDevelopmentalStages.Newborn() && this.AllowedDevelopmentalStages != DevelopmentalStage.Newborn)
			{
				Log.Error("Trying to generate a newborn and other developmental stages simultaneously.");
				this.AllowedDevelopmentalStages = DevelopmentalStage.Newborn;
			}
			if (this.ExcludeBiologicalAgeRange != null && this.FixedBiologicalAge != null)
			{
				Log.Error("Trying to generate a pawn with a fixed biological age and an excluded biological age range.");
				this.ExcludeBiologicalAgeRange = null;
			}
			if (this.BiologicalAgeRange != null && this.FixedBiologicalAge != null)
			{
				Log.Error("Trying to generate a pawn with a fixed biological age and a biological age range.");
				this.BiologicalAgeRange = null;
			}
			if (this.BiologicalAgeRange != null && this.ExcludeBiologicalAgeRange != null)
			{
				Log.Error("Trying to generate a pawn with both a include and exclude age range");
				this.BiologicalAgeRange = null;
				this.ExcludeBiologicalAgeRange = null;
			}
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00095168 File Offset: 0x00093368
		[Obsolete("Will be removed in 1.5; use the constructor instead.")]
		public static PawnGenerationRequest MakeDefault()
		{
			return new PawnGenerationRequest(null, null, PawnGenerationContext.NonPlayer, -1, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false);
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x000951F0 File Offset: 0x000933F0
		public void SetFixedLastName(string fixedLastName)
		{
			if (this.FixedLastName != null)
			{
				Log.Error("Last name is already a fixed value: " + this.FixedLastName + ".");
				return;
			}
			this.FixedLastName = fixedLastName;
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0009521C File Offset: 0x0009341C
		public void SetFixedBirthName(string fixedBirthName)
		{
			if (this.FixedBirthName != null)
			{
				Log.Error("birth name is already a fixed value: " + this.FixedBirthName + ".");
				return;
			}
			this.FixedBirthName = fixedBirthName;
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x00095248 File Offset: 0x00093448
		public void AddForcedGene(GeneDef gene, bool xenogene)
		{
			if (xenogene)
			{
				if (this.ForcedXenogenes == null)
				{
					this.ForcedXenogenes = new List<GeneDef>();
				}
				this.ForcedXenogenes.Add(gene);
				return;
			}
			if (this.ForcedEndogenes == null)
			{
				this.ForcedEndogenes = new List<GeneDef>();
			}
			this.ForcedEndogenes.Add(gene);
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00095298 File Offset: 0x00093498
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"kindDef=",
				this.KindDef,
				", context=",
				this.Context,
				", faction=",
				this.Faction,
				", tile=",
				this.Tile,
				", forceGenerateNewPawn=",
				this.ForceGenerateNewPawn.ToString(),
				", allowedDevelopmentalStages=",
				this.AllowedDevelopmentalStages,
				", allowDead=",
				this.AllowDead.ToString(),
				", allowDowned=",
				this.AllowDowned.ToString(),
				", canGeneratePawnRelations=",
				this.CanGeneratePawnRelations.ToString(),
				", mustBeCapableOfViolence=",
				this.MustBeCapableOfViolence.ToString(),
				", colonistRelationChanceFactor=",
				this.ColonistRelationChanceFactor,
				", forceAddFreeWarmLayerIfNeeded=",
				this.ForceAddFreeWarmLayerIfNeeded.ToString(),
				", allowGay=",
				this.AllowGay.ToString(),
				", prohibitedTraits=",
				this.ProhibitedTraits,
				", allowFood=",
				this.AllowFood.ToString(),
				", allowAddictions=",
				this.AllowAddictions.ToString(),
				", inhabitant=",
				this.Inhabitant.ToString(),
				", certainlyBeenInCryptosleep=",
				this.CertainlyBeenInCryptosleep.ToString(),
				", biocodeWeaponChance=",
				this.BiocodeWeaponChance,
				", validatorPreGear=",
				this.ValidatorPreGear,
				", validatorPostGear=",
				this.ValidatorPostGear,
				", fixedBiologicalAge=",
				this.FixedBiologicalAge,
				", fixedChronologicalAge=",
				this.FixedChronologicalAge,
				", fixedGender=",
				this.FixedGender,
				", fixedLastName=",
				this.FixedLastName,
				", fixedBirthName=",
				this.FixedBirthName
			});
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00095518 File Offset: 0x00093718
		private bool AlwaysDownedLifeStages(out string stages)
		{
			DevelopmentalStage developmentalStage = this.AllowedDevelopmentalStages;
			if (this.AllowedDevelopmentalStages.Newborn())
			{
				developmentalStage |= DevelopmentalStage.Baby;
			}
			StringBuilder stringBuilder = null;
			foreach (LifeStageAge lifeStageAge in this.KindDef.RaceProps.lifeStageAges)
			{
				if (lifeStageAge.def.alwaysDowned && developmentalStage.Has(lifeStageAge.def.developmentalStage))
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(lifeStageAge.def.label, 64);
					}
					else
					{
						stringBuilder.Append(", ");
						stringBuilder.Append(lifeStageAge.def.label);
					}
				}
			}
			if (stringBuilder == null)
			{
				stages = null;
				return false;
			}
			stages = stringBuilder.ToString();
			return true;
		}

		// Token: 0x04001240 RID: 4672
		private PawnKindDef kindDefInner;

		// Token: 0x04001274 RID: 4724
		private bool _calledTheCorrectConstructor;
	}
}

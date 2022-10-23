using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005A9 RID: 1449
	public abstract class Verb : ITargetingSource, IExposable, ILoadReferenceable
	{
		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002C19 RID: 11289 RVA: 0x001188E2 File Offset: 0x00116AE2
		public IVerbOwner DirectOwner
		{
			get
			{
				return this.verbTracker.directOwner;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002C1A RID: 11290 RVA: 0x001188EF File Offset: 0x00116AEF
		public ImplementOwnerTypeDef ImplementOwnerType
		{
			get
			{
				return this.verbTracker.directOwner.ImplementOwnerTypeDef;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002C1B RID: 11291 RVA: 0x00118901 File Offset: 0x00116B01
		public CompEquippable EquipmentCompSource
		{
			get
			{
				return this.DirectOwner as CompEquippable;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002C1C RID: 11292 RVA: 0x0011890E File Offset: 0x00116B0E
		public CompReloadable ReloadableCompSource
		{
			get
			{
				return this.DirectOwner as CompReloadable;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002C1D RID: 11293 RVA: 0x0011891B File Offset: 0x00116B1B
		public ThingWithComps EquipmentSource
		{
			get
			{
				if (this.EquipmentCompSource != null)
				{
					return this.EquipmentCompSource.parent;
				}
				if (this.ReloadableCompSource != null)
				{
					return this.ReloadableCompSource.parent;
				}
				return null;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002C1E RID: 11294 RVA: 0x00118946 File Offset: 0x00116B46
		public HediffComp_VerbGiver HediffCompSource
		{
			get
			{
				return this.DirectOwner as HediffComp_VerbGiver;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002C1F RID: 11295 RVA: 0x00118953 File Offset: 0x00116B53
		public Hediff HediffSource
		{
			get
			{
				if (this.HediffCompSource == null)
				{
					return null;
				}
				return this.HediffCompSource.parent;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002C20 RID: 11296 RVA: 0x0011896A File Offset: 0x00116B6A
		public Pawn_MeleeVerbs_TerrainSource TerrainSource
		{
			get
			{
				return this.DirectOwner as Pawn_MeleeVerbs_TerrainSource;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002C21 RID: 11297 RVA: 0x00118977 File Offset: 0x00116B77
		public TerrainDef TerrainDefSource
		{
			get
			{
				if (this.TerrainSource == null)
				{
					return null;
				}
				return this.TerrainSource.def;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002C22 RID: 11298 RVA: 0x0011898E File Offset: 0x00116B8E
		public virtual Thing Caster
		{
			get
			{
				return this.caster;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002C23 RID: 11299 RVA: 0x00118996 File Offset: 0x00116B96
		public virtual Pawn CasterPawn
		{
			get
			{
				return this.caster as Pawn;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002C24 RID: 11300 RVA: 0x0008A07A File Offset: 0x0008827A
		public virtual Verb GetVerb
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002C25 RID: 11301 RVA: 0x001189A3 File Offset: 0x00116BA3
		public virtual bool CasterIsPawn
		{
			get
			{
				return this.caster is Pawn;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002C26 RID: 11302 RVA: 0x001189B3 File Offset: 0x00116BB3
		public virtual bool Targetable
		{
			get
			{
				return this.verbProps.targetable;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002C27 RID: 11303 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002C28 RID: 11304 RVA: 0x0000249D File Offset: 0x0000069D
		public virtual bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002C29 RID: 11305 RVA: 0x001189C0 File Offset: 0x00116BC0
		public LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTarget;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002C2A RID: 11306 RVA: 0x001189C8 File Offset: 0x00116BC8
		public LocalTargetInfo CurrentDestination
		{
			get
			{
				return this.currentDestination;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002C2B RID: 11307 RVA: 0x001189D0 File Offset: 0x00116BD0
		public int LastShotTick
		{
			get
			{
				return this.lastShotTick;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002C2C RID: 11308 RVA: 0x001189D8 File Offset: 0x00116BD8
		public virtual TargetingParameters targetParams
		{
			get
			{
				return this.verbProps.targetParams;
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002C2D RID: 11309 RVA: 0x000029B0 File Offset: 0x00000BB0
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002C2E RID: 11310 RVA: 0x00002662 File Offset: 0x00000862
		protected virtual int ShotsPerBurst
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002C2F RID: 11311 RVA: 0x001189E8 File Offset: 0x00116BE8
		public virtual Texture2D UIIcon
		{
			get
			{
				if (this.verbProps.commandIcon != null)
				{
					if (this.commandIconCached == null)
					{
						this.commandIconCached = ContentFinder<Texture2D>.Get(this.verbProps.commandIcon, true);
					}
					return this.commandIconCached;
				}
				if (this.EquipmentSource != null)
				{
					return this.EquipmentSource.def.uiIcon;
				}
				return BaseContent.BadTex;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002C30 RID: 11312 RVA: 0x00118A4C File Offset: 0x00116C4C
		public bool Bursting
		{
			get
			{
				return this.burstShotsLeft > 0;
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002C31 RID: 11313 RVA: 0x00118A57 File Offset: 0x00116C57
		public virtual bool IsMeleeAttack
		{
			get
			{
				return this.verbProps.IsMeleeAttack;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06002C32 RID: 11314 RVA: 0x00118A64 File Offset: 0x00116C64
		public bool BuggedAfterLoading
		{
			get
			{
				return this.verbProps == null;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06002C33 RID: 11315 RVA: 0x00118A6F File Offset: 0x00116C6F
		public bool WarmingUp
		{
			get
			{
				return this.WarmupStance != null;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06002C34 RID: 11316 RVA: 0x00118A7C File Offset: 0x00116C7C
		public Stance_Warmup WarmupStance
		{
			get
			{
				if (this.CasterPawn == null || !this.CasterPawn.Spawned)
				{
					return null;
				}
				Stance_Warmup stance_Warmup;
				if ((stance_Warmup = (this.CasterPawn.stances.curStance as Stance_Warmup)) == null || stance_Warmup.verb != this)
				{
					return null;
				}
				return stance_Warmup;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002C35 RID: 11317 RVA: 0x00118AC5 File Offset: 0x00116CC5
		public int WarmupTicksLeft
		{
			get
			{
				if (this.WarmupStance == null)
				{
					return 0;
				}
				return this.WarmupStance.ticksLeft;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002C36 RID: 11318 RVA: 0x00118ADC File Offset: 0x00116CDC
		public float WarmupProgress
		{
			get
			{
				return 1f - this.WarmupTicksLeft.TicksToSeconds() / this.verbProps.warmupTime;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002C37 RID: 11319 RVA: 0x00118AFB File Offset: 0x00116CFB
		public virtual string ReportLabel
		{
			get
			{
				return this.verbProps.label;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002C38 RID: 11320 RVA: 0x00118B08 File Offset: 0x00116D08
		protected virtual float EffectiveRange
		{
			get
			{
				return this.verbProps.AdjustedRange(this, this.CasterPawn);
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002C39 RID: 11321 RVA: 0x00118B1C File Offset: 0x00116D1C
		public virtual float? AimAngleOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002C3A RID: 11322 RVA: 0x00118B32 File Offset: 0x00116D32
		public bool NonInterruptingSelfCast
		{
			get
			{
				return this.verbProps.nonInterruptingSelfCast || this.nonInterruptingSelfCast;
			}
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x00118B49 File Offset: 0x00116D49
		public bool IsStillUsableBy(Pawn pawn)
		{
			return this.Available() && this.DirectOwner.VerbsStillUsableBy(pawn) && this.verbProps.GetDamageFactorFor(this, pawn) != 0f;
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x00002662 File Offset: 0x00000862
		public virtual bool IsUsableOn(Thing target)
		{
			return true;
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x00118B7C File Offset: 0x00116D7C
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.loadID, "loadID", null, false);
			Scribe_Values.Look<VerbState>(ref this.state, "state", VerbState.Idle, false);
			Scribe_TargetInfo.Look(ref this.currentTarget, "currentTarget");
			Scribe_TargetInfo.Look(ref this.currentDestination, "currentDestination");
			Scribe_Values.Look<int>(ref this.burstShotsLeft, "burstShotsLeft", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextBurstShot, "ticksToNextBurstShot", 0, false);
			Scribe_Values.Look<int>(ref this.lastShotTick, "lastShotTick", 0, false);
			Scribe_Values.Look<bool>(ref this.surpriseAttack, "surpriseAttack", false, false);
			Scribe_Values.Look<bool>(ref this.canHitNonTargetPawnsNow, "canHitNonTargetPawnsNow", false, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
			Scribe_Values.Look<bool>(ref this.nonInterruptingSelfCast, "nonInterruptingSelfCast", false, false);
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x00118C4B File Offset: 0x00116E4B
		public string GetUniqueLoadID()
		{
			return "Verb_" + this.loadID;
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x00118C5D File Offset: 0x00116E5D
		public static string CalculateUniqueLoadID(IVerbOwner owner, Tool tool, ManeuverDef maneuver)
		{
			return string.Format("{0}_{1}_{2}", owner.UniqueVerbOwnerID(), (tool != null) ? tool.id : "NT", (maneuver != null) ? maneuver.defName : "NM");
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x00118C8F File Offset: 0x00116E8F
		public static string CalculateUniqueLoadID(IVerbOwner owner, int index)
		{
			return string.Format("{0}_{1}", owner.UniqueVerbOwnerID(), index);
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x00118CA7 File Offset: 0x00116EA7
		public bool TryStartCastOn(LocalTargetInfo castTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
		{
			return this.TryStartCastOn(castTarg, LocalTargetInfo.Invalid, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire, nonInterruptingSelfCast);
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x00118CBC File Offset: 0x00116EBC
		public virtual bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
		{
			if (this.caster == null)
			{
				Log.Error("Verb " + this.GetUniqueLoadID() + " needs caster to work (possibly lost during saving/loading).");
				return false;
			}
			if (!this.caster.Spawned)
			{
				return false;
			}
			if (this.state == VerbState.Bursting || !this.CanHitTarget(castTarg))
			{
				return false;
			}
			if (this.CausesTimeSlowdown(castTarg))
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
			this.surpriseAttack = surpriseAttack;
			this.canHitNonTargetPawnsNow = canHitNonTargetPawns;
			this.preventFriendlyFire = preventFriendlyFire;
			this.nonInterruptingSelfCast = nonInterruptingSelfCast;
			this.currentTarget = castTarg;
			this.currentDestination = destTarg;
			if (this.CasterIsPawn && this.verbProps.warmupTime > 0f)
			{
				ShootLine newShootLine;
				if (!this.TryFindShootLineFromTo(this.caster.Position, castTarg, out newShootLine))
				{
					return false;
				}
				this.CasterPawn.Drawer.Notify_WarmingCastAlongLine(newShootLine, this.caster.Position);
				float statValue = this.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true, -1);
				int ticks = (this.verbProps.warmupTime * statValue).SecondsToTicks();
				this.CasterPawn.stances.SetStance(new Stance_Warmup(ticks, castTarg, this));
				if (this.verbProps.stunTargetOnCastStart && castTarg.Pawn != null)
				{
					castTarg.Pawn.stances.stunner.StunFor(ticks, null, false, true);
				}
			}
			else
			{
				this.WarmupComplete();
			}
			return true;
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x00118E23 File Offset: 0x00117023
		public virtual void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.TryCastNextBurstShot();
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x00118E40 File Offset: 0x00117040
		public void VerbTick()
		{
			if (this.state == VerbState.Bursting)
			{
				Pawn pawn;
				if (!this.caster.Spawned || ((pawn = (this.caster as Pawn)) != null && pawn.stances.stunner.Stunned))
				{
					this.Reset();
				}
				else
				{
					this.ticksToNextBurstShot--;
					if (this.ticksToNextBurstShot <= 0)
					{
						this.TryCastNextBurstShot();
					}
					this.BurstingTick();
				}
			}
			for (int i = this.maintainedEffecters.Count - 1; i >= 0; i--)
			{
				Effecter item = this.maintainedEffecters[i].Item1;
				if (item.ticksLeft > 0)
				{
					TargetInfo item2 = this.maintainedEffecters[i].Item2;
					TargetInfo item3 = this.maintainedEffecters[i].Item3;
					item.EffectTick(item2, item3);
					item.ticksLeft--;
				}
				else
				{
					item.Cleanup();
					this.maintainedEffecters.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void BurstingTick()
		{
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x00118F34 File Offset: 0x00117134
		public void AddEffecterToMaintain(Effecter eff, IntVec3 pos, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			TargetInfo targetInfo = new TargetInfo(pos, map ?? this.caster.Map, false);
			this.maintainedEffecters.Add(new Tuple<Effecter, TargetInfo, TargetInfo>(eff, targetInfo, targetInfo));
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x00118F78 File Offset: 0x00117178
		public void AddEffecterToMaintain(Effecter eff, IntVec3 posA, IntVec3 posB, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			TargetInfo item = new TargetInfo(posA, map ?? this.caster.Map, false);
			TargetInfo item2 = new TargetInfo(posB, map ?? this.caster.Map, false);
			this.maintainedEffecters.Add(new Tuple<Effecter, TargetInfo, TargetInfo>(eff, item, item2));
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x00118FD4 File Offset: 0x001171D4
		public virtual bool Available()
		{
			if (this.verbProps.consumeFuelPerShot > 0f)
			{
				CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && compRefuelable.Fuel < this.verbProps.consumeFuelPerShot)
				{
					return false;
				}
			}
			ThingWithComps equipmentSource = this.EquipmentSource;
			CompReloadable compReloadable = (equipmentSource != null) ? equipmentSource.GetComp<CompReloadable>() : null;
			string text;
			return (compReloadable == null || compReloadable.CanBeUsed) && (!this.CasterIsPawn || this.EquipmentSource == null || !EquipmentUtility.RolePreventsFromUsing(this.CasterPawn, this.EquipmentSource, out text));
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x00119060 File Offset: 0x00117260
		protected void TryCastNextBurstShot()
		{
			LocalTargetInfo localTargetInfo = this.currentTarget;
			if (this.Available() && this.TryCastShot())
			{
				if (this.verbProps.muzzleFlashScale > 0.01f)
				{
					FleckMaker.Static(this.caster.Position, this.caster.Map, FleckDefOf.ShotFlash, this.verbProps.muzzleFlashScale);
				}
				if (this.verbProps.soundCast != null)
				{
					this.verbProps.soundCast.PlayOneShot(new TargetInfo(this.caster.Position, this.caster.MapHeld, false));
				}
				if (this.verbProps.soundCastTail != null)
				{
					this.verbProps.soundCastTail.PlayOneShotOnCamera(this.caster.Map);
				}
				if (this.CasterIsPawn)
				{
					if (this.CasterPawn.thinker != null)
					{
						this.CasterPawn.mindState.Notify_EngagedTarget();
					}
					if (this.CasterPawn.mindState != null)
					{
						this.CasterPawn.mindState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.CasterPawn.MentalState != null)
					{
						this.CasterPawn.MentalState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.TerrainDefSource != null)
					{
						this.CasterPawn.meleeVerbs.Notify_UsedTerrainBasedVerb();
					}
					if (this.CasterPawn.health != null)
					{
						this.CasterPawn.health.Notify_UsedVerb(this, localTargetInfo);
					}
					if (this.EquipmentSource != null)
					{
						this.EquipmentSource.Notify_UsedWeapon(this.CasterPawn);
					}
					if (!this.CasterPawn.Spawned)
					{
						this.Reset();
						return;
					}
				}
				if (this.verbProps.consumeFuelPerShot > 0f)
				{
					CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
					if (compRefuelable != null)
					{
						compRefuelable.ConsumeFuel(this.verbProps.consumeFuelPerShot);
					}
				}
				this.burstShotsLeft--;
			}
			else
			{
				this.burstShotsLeft = 0;
			}
			if (this.burstShotsLeft > 0)
			{
				this.ticksToNextBurstShot = this.verbProps.ticksBetweenBurstShots;
				if (this.CasterIsPawn && !this.NonInterruptingSelfCast)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.verbProps.ticksBetweenBurstShots + 1, this.currentTarget, this));
					return;
				}
			}
			else
			{
				this.state = VerbState.Idle;
				if (this.CasterIsPawn && !this.NonInterruptingSelfCast)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.verbProps.AdjustedCooldownTicks(this, this.CasterPawn), this.currentTarget, this));
				}
				if (this.castCompleteCallback != null)
				{
					this.castCompleteCallback();
				}
				if (this.verbProps.consumeFuelPerBurst > 0f)
				{
					CompRefuelable compRefuelable2 = this.caster.TryGetComp<CompRefuelable>();
					if (compRefuelable2 != null)
					{
						compRefuelable2.ConsumeFuel(this.verbProps.consumeFuelPerBurst);
					}
				}
			}
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x00119324 File Offset: 0x00117524
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
			if (this.verbProps.IsMeleeAttack)
			{
				Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
				job.playerForced = true;
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					job.killIncappedTarget = pawn.Downed;
				}
				this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				return;
			}
			float num = this.verbProps.EffectiveMinRange(target, this.CasterPawn);
			if ((float)this.CasterPawn.Position.DistanceToSquared(target.Cell) < num * num && this.CasterPawn.Position.AdjacentTo8WayOrInside(target.Cell))
			{
				Messages.Message("MessageCantShootInMelee".Translate(), this.CasterPawn, MessageTypeDefOf.RejectInput, false);
				return;
			}
			Job job2 = JobMaker.MakeJob(this.verbProps.ai_IsWeapon ? JobDefOf.AttackStatic : JobDefOf.UseVerbOnThing);
			job2.verbToUse = this;
			job2.targetA = target;
			job2.endIfCantShootInMelee = true;
			this.CasterPawn.jobs.TryTakeOrderedJob(job2, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06002C4B RID: 11339
		protected abstract bool TryCastShot();

		// Token: 0x06002C4C RID: 11340 RVA: 0x00119443 File Offset: 0x00117643
		public void Notify_PickedUp()
		{
			this.Reset();
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x0011944C File Offset: 0x0011764C
		public virtual void Reset()
		{
			this.state = VerbState.Idle;
			this.currentTarget = null;
			this.currentDestination = null;
			this.burstShotsLeft = 0;
			this.ticksToNextBurstShot = 0;
			this.castCompleteCallback = null;
			this.surpriseAttack = false;
			this.preventFriendlyFire = false;
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x0011949C File Offset: 0x0011769C
		public virtual void Notify_EquipmentLost()
		{
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Spawned)
				{
					Stance_Warmup stance_Warmup = casterPawn.stances.curStance as Stance_Warmup;
					if (stance_Warmup != null && stance_Warmup.verb == this)
					{
						casterPawn.stances.CancelBusyStanceSoft();
					}
					if (casterPawn.CurJob != null && casterPawn.CurJob.def == JobDefOf.AttackStatic)
					{
						casterPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					}
				}
			}
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x00119510 File Offset: 0x00117710
		public virtual float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 0f;
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x0011951C File Offset: 0x0011771C
		private bool CausesTimeSlowdown(LocalTargetInfo castTarg)
		{
			if (!this.verbProps.CausesTimeSlowdown)
			{
				return false;
			}
			if (!castTarg.HasThing)
			{
				return false;
			}
			Thing thing = castTarg.Thing;
			if (thing.def.category != ThingCategory.Pawn && (thing.def.building == null || !thing.def.building.IsTurret))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			bool flag = pawn != null && pawn.Downed;
			return (thing.Faction == Faction.OfPlayer && this.caster.HostileTo(Faction.OfPlayer)) || (this.caster.Faction == Faction.OfPlayer && thing.HostileTo(Faction.OfPlayer) && !flag);
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x001195D4 File Offset: 0x001177D4
		public virtual bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.caster != null && this.caster.Spawned && (targ == this.caster || this.CanHitTargetFrom(this.caster.Position, targ));
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x00119614 File Offset: 0x00117814
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn p;
			Pawn victim;
			Pawn pawn;
			return (!this.CasterIsPawn || (p = (target.Thing as Pawn)) == null || (!p.InSameExtraFaction(this.caster as Pawn, ExtraFactionType.HomeFaction, null) && !p.InSameExtraFaction(this.caster as Pawn, ExtraFactionType.MiniFaction, null))) && (!this.CasterIsPawn || (victim = (target.Thing as Pawn)) == null || !HistoryEventUtility.IsKillingInnocentAnimal(this.CasterPawn, victim) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, this.CasterPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo")) && (!this.CasterIsPawn || (pawn = (target.Thing as Pawn)) == null || this.CasterPawn.Ideo == null || !this.CasterPawn.Ideo.IsVeneratedAnimal(pawn) || new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, this.CasterPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"));
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x00119714 File Offset: 0x00117914
		public virtual void DrawHighlight(LocalTargetInfo target)
		{
			this.verbProps.DrawRadiusRing(this.caster.Position);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
				this.DrawHighlightFieldRadiusAroundTarget(target);
			}
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x00119744 File Offset: 0x00117944
		protected void DrawHighlightFieldRadiusAroundTarget(LocalTargetInfo target)
		{
			bool flag;
			float num = this.HighlightFieldRadiusAroundTarget(out flag);
			ShootLine shootLine;
			if (num > 0.2f && this.TryFindShootLineFromTo(this.caster.Position, target, out shootLine))
			{
				if (flag)
				{
					GenExplosion.RenderPredictedAreaOfEffect(shootLine.Dest, num);
					return;
				}
				GenDraw.DrawFieldEdges((from x in GenRadial.RadialCellsAround(shootLine.Dest, num, true)
				where x.InBounds(Find.CurrentMap)
				select x).ToList<IntVec3>());
			}
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x001197C8 File Offset: 0x001179C8
		public virtual void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				if (this.UIIcon != BaseContent.BadTex)
				{
					icon = this.UIIcon;
				}
				else
				{
					icon = TexCommand.Attack;
				}
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x00119810 File Offset: 0x00117A10
		public virtual bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			if (targ.Thing != null && targ.Thing == this.caster)
			{
				return this.targetParams.canTargetSelf;
			}
			ShootLine shootLine;
			return !this.ApparelPreventsShooting() && this.TryFindShootLineFromTo(root, targ, out shootLine);
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x00119855 File Offset: 0x00117A55
		public bool ApparelPreventsShooting()
		{
			return this.FirstApparelPreventingShooting() != null;
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x00119860 File Offset: 0x00117A60
		public Apparel FirstApparelPreventingShooting()
		{
			if (this.CasterIsPawn && this.CasterPawn.apparel != null)
			{
				List<Apparel> wornApparel = this.CasterPawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (!wornApparel[i].AllowVerbCast(this))
					{
						return wornApparel[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x001198BC File Offset: 0x00117ABC
		public bool TryFindShootLineFromTo(IntVec3 root, LocalTargetInfo targ, out ShootLine resultingLine)
		{
			if (targ.HasThing && targ.Thing.Map != this.caster.Map)
			{
				resultingLine = default(ShootLine);
				return false;
			}
			if (this.verbProps.IsMeleeAttack || this.EffectiveRange <= 1.42f)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return ReachabilityImmediate.CanReachImmediate(root, targ, this.caster.Map, PathEndMode.Touch, null);
			}
			CellRect occupiedRect = targ.HasThing ? targ.Thing.OccupiedRect() : CellRect.SingleCell(targ.Cell);
			if (this.OutOfRange(root, targ, occupiedRect))
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return false;
			}
			if (!this.verbProps.requireLineOfSight)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return true;
			}
			if (this.CasterIsPawn)
			{
				IntVec3 dest;
				if (this.CanHitFromCellIgnoringRange(root, targ, out dest))
				{
					resultingLine = new ShootLine(root, dest);
					return true;
				}
				ShootLeanUtility.LeanShootingSourcesFromTo(root, occupiedRect.ClosestCellTo(root), this.caster.Map, Verb.tempLeanShootSources);
				for (int i = 0; i < Verb.tempLeanShootSources.Count; i++)
				{
					IntVec3 intVec = Verb.tempLeanShootSources[i];
					if (this.CanHitFromCellIgnoringRange(intVec, targ, out dest))
					{
						resultingLine = new ShootLine(intVec, dest);
						return true;
					}
				}
			}
			else
			{
				foreach (IntVec3 intVec2 in this.caster.OccupiedRect())
				{
					IntVec3 dest;
					if (this.CanHitFromCellIgnoringRange(intVec2, targ, out dest))
					{
						resultingLine = new ShootLine(intVec2, dest);
						return true;
					}
				}
			}
			resultingLine = new ShootLine(root, targ.Cell);
			return false;
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x00119A9C File Offset: 0x00117C9C
		public bool OutOfRange(IntVec3 root, LocalTargetInfo targ, CellRect occupiedRect)
		{
			float num = this.verbProps.EffectiveMinRange(targ, this.caster);
			float num2 = occupiedRect.ClosestDistSquaredTo(root);
			return num2 > this.EffectiveRange * this.EffectiveRange || num2 < num * num;
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x00119AE0 File Offset: 0x00117CE0
		private bool CanHitFromCellIgnoringRange(IntVec3 sourceCell, LocalTargetInfo targ, out IntVec3 goodDest)
		{
			if (targ.Thing != null)
			{
				if (targ.Thing.Map != this.caster.Map)
				{
					goodDest = IntVec3.Invalid;
					return false;
				}
				ShootLeanUtility.CalcShootableCellsOf(Verb.tempDestList, targ.Thing);
				for (int i = 0; i < Verb.tempDestList.Count; i++)
				{
					if (this.CanHitCellFromCellIgnoringRange(sourceCell, Verb.tempDestList[i], targ.Thing.def.Fillage == FillCategory.Full))
					{
						goodDest = Verb.tempDestList[i];
						return true;
					}
				}
			}
			else if (this.CanHitCellFromCellIgnoringRange(sourceCell, targ.Cell, false))
			{
				goodDest = targ.Cell;
				return true;
			}
			goodDest = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x00119BB0 File Offset: 0x00117DB0
		private bool CanHitCellFromCellIgnoringRange(IntVec3 sourceSq, IntVec3 targetLoc, bool includeCorners = false)
		{
			if (this.verbProps.mustCastOnOpenGround && (!targetLoc.Standable(this.caster.Map) || this.caster.Map.thingGrid.CellContains(targetLoc, ThingCategory.Pawn)))
			{
				return false;
			}
			if (this.verbProps.requireLineOfSight)
			{
				if (!includeCorners)
				{
					if (!GenSight.LineOfSight(sourceSq, targetLoc, this.caster.Map, true, null, 0, 0))
					{
						return false;
					}
				}
				else if (!GenSight.LineOfSightToEdges(sourceSq, targetLoc, this.caster.Map, true, null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x00119C3C File Offset: 0x00117E3C
		public override string ToString()
		{
			string text;
			if (this.verbProps == null)
			{
				text = "null";
			}
			else if (!this.verbProps.label.NullOrEmpty())
			{
				text = this.verbProps.label;
			}
			else if (this.HediffCompSource != null)
			{
				text = this.HediffCompSource.Def.label;
			}
			else if (this.EquipmentSource != null)
			{
				text = this.EquipmentSource.def.label;
			}
			else if (this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool) != null)
			{
				text = this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool).defName;
			}
			else
			{
				text = "unknown";
			}
			if (this.tool != null)
			{
				text = text + "/" + this.loadID;
			}
			return base.GetType().ToString() + "(" + text + ")";
		}

		// Token: 0x04001D10 RID: 7440
		public VerbProperties verbProps;

		// Token: 0x04001D11 RID: 7441
		public VerbTracker verbTracker;

		// Token: 0x04001D12 RID: 7442
		public ManeuverDef maneuver;

		// Token: 0x04001D13 RID: 7443
		public Tool tool;

		// Token: 0x04001D14 RID: 7444
		public Thing caster;

		// Token: 0x04001D15 RID: 7445
		public MechanitorControlGroup controlGroup;

		// Token: 0x04001D16 RID: 7446
		public string loadID;

		// Token: 0x04001D17 RID: 7447
		public VerbState state;

		// Token: 0x04001D18 RID: 7448
		protected LocalTargetInfo currentTarget;

		// Token: 0x04001D19 RID: 7449
		protected LocalTargetInfo currentDestination;

		// Token: 0x04001D1A RID: 7450
		protected int burstShotsLeft;

		// Token: 0x04001D1B RID: 7451
		protected int ticksToNextBurstShot;

		// Token: 0x04001D1C RID: 7452
		protected int lastShotTick = -999999;

		// Token: 0x04001D1D RID: 7453
		protected bool surpriseAttack;

		// Token: 0x04001D1E RID: 7454
		protected bool canHitNonTargetPawnsNow = true;

		// Token: 0x04001D1F RID: 7455
		protected bool preventFriendlyFire;

		// Token: 0x04001D20 RID: 7456
		protected bool nonInterruptingSelfCast;

		// Token: 0x04001D21 RID: 7457
		public Action castCompleteCallback;

		// Token: 0x04001D22 RID: 7458
		private Texture2D commandIconCached;

		// Token: 0x04001D23 RID: 7459
		private List<Tuple<Effecter, TargetInfo, TargetInfo>> maintainedEffecters = new List<Tuple<Effecter, TargetInfo, TargetInfo>>();

		// Token: 0x04001D24 RID: 7460
		private static List<IntVec3> tempLeanShootSources = new List<IntVec3>();

		// Token: 0x04001D25 RID: 7461
		private static List<IntVec3> tempDestList = new List<IntVec3>();
	}
}

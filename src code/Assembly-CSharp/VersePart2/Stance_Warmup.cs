using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200037B RID: 891
	public class Stance_Warmup : Stance_Busy
	{
		// Token: 0x060019CA RID: 6602 RVA: 0x0009B67B File Offset: 0x0009987B
		public Stance_Warmup()
		{
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x0009B68C File Offset: 0x0009988C
		public Stance_Warmup(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
			if (focusTarg.HasThing && focusTarg.Thing is Pawn)
			{
				Pawn pawn = (Pawn)focusTarg.Thing;
				this.targetStartedDowned = pawn.Downed;
				if (pawn.apparel != null)
				{
					for (int i = 0; i < pawn.apparel.WornApparelCount; i++)
					{
						List<ThingComp> allComps = pawn.apparel.WornApparel[i].AllComps;
						for (int j = 0; j < allComps.Count; j++)
						{
							CompShield compShield;
							if ((compShield = (allComps[j] as CompShield)) != null)
							{
								compShield.KeepDisplaying();
							}
						}
					}
				}
			}
			this.InitEffects(false);
			this.drawAimPie = (verb != null && verb.verbProps.drawAimPie);
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x0009B758 File Offset: 0x00099958
		public void InitEffects(bool afterReload = false)
		{
			if (this.verb == null)
			{
				return;
			}
			VerbProperties verbProps = this.verb.verbProps;
			if (verbProps.soundAiming != null)
			{
				SoundInfo info = SoundInfo.InMap(this.verb.caster, MaintenanceType.PerTick);
				if (this.verb.CasterIsPawn)
				{
					info.pitchFactor = 1f / this.verb.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true, -1);
				}
				this.sustainer = verbProps.soundAiming.TrySpawnSustainer(info);
			}
			if (verbProps.warmupEffecter != null && this.verb.Caster != null)
			{
				this.effecter = verbProps.warmupEffecter.Spawn(this.verb.Caster, this.verb.Caster.Map, 1f);
				this.effecter.Trigger(this.verb.Caster, this.focusTarg.ToTargetInfo(this.verb.Caster.Map), -1);
			}
			if (this.verb.Caster != null)
			{
				Map map = this.verb.Caster.Map;
				if (verbProps.aimingLineMote != null)
				{
					Vector3 vector = this.TargetPos();
					IntVec3 cell = vector.ToIntVec3();
					this.aimLineMote = MoteMaker.MakeInteractionOverlay(verbProps.aimingLineMote, this.verb.Caster, new TargetInfo(cell, map, false), Vector3.zero, vector - cell.ToVector3Shifted());
					if (afterReload)
					{
						Mote mote = this.aimLineMote;
						if (mote != null)
						{
							mote.ForceSpawnTick(this.startedTick);
						}
					}
				}
				if (verbProps.aimingChargeMote != null)
				{
					this.aimChargeMote = MoteMaker.MakeStaticMote(this.verb.Caster.DrawPos, map, verbProps.aimingChargeMote, 1f, true);
					if (afterReload)
					{
						Mote mote2 = this.aimChargeMote;
						if (mote2 != null)
						{
							mote2.ForceSpawnTick(this.startedTick);
						}
					}
				}
				if (verbProps.aimingTargetMote != null)
				{
					this.aimTargetMote = MoteMaker.MakeStaticMote(this.focusTarg.CenterVector3, map, verbProps.aimingTargetMote, 1f, true);
					if (this.aimTargetMote != null)
					{
						this.aimTargetMote.exactRotation = this.AimDir().ToAngleFlat();
						if (afterReload)
						{
							this.aimTargetMote.ForceSpawnTick(this.startedTick);
						}
					}
				}
			}
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x0009B990 File Offset: 0x00099B90
		private Vector3 TargetPos()
		{
			VerbProperties verbProps = this.verb.verbProps;
			Vector3 result = this.focusTarg.CenterVector3;
			if (verbProps.aimingLineMoteFixedLength != null)
			{
				result = this.verb.Caster.DrawPos + this.AimDir() * verbProps.aimingLineMoteFixedLength.Value;
			}
			return result;
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x0009B9F0 File Offset: 0x00099BF0
		private Vector3 AimDir()
		{
			Vector3 result = this.focusTarg.CenterVector3 - this.verb.Caster.DrawPos;
			result.y = 0f;
			result.Normalize();
			return result;
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x0009BA32 File Offset: 0x00099C32
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.targetStartedDowned, "targetStartDowned", false, false);
			Scribe_Values.Look<bool>(ref this.drawAimPie, "drawAimPie", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.needsReInitAfterLoad = true;
			}
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x0009BA70 File Offset: 0x00099C70
		public override void StanceDraw()
		{
			if (this.drawAimPie && Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				GenDraw.DrawAimPie(this.stanceTracker.pawn, this.focusTarg, (int)((float)this.ticksLeft * this.pieSizeFactor), 0.2f);
			}
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x0009BAC8 File Offset: 0x00099CC8
		public override void StanceTick()
		{
			if (this.needsReInitAfterLoad)
			{
				this.InitEffects(true);
				this.needsReInitAfterLoad = false;
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.EffectTick(this.verb.Caster, this.focusTarg.ToTargetInfo(this.verb.Caster.Map));
			}
			Vector3 vector = this.AimDir();
			float exactRotation = vector.AngleFlat();
			bool stunned = this.stanceTracker.stunner.Stunned;
			if (this.aimLineMote != null)
			{
				this.aimLineMote.paused = stunned;
				this.aimLineMote.Maintain();
				Vector3 vector2 = this.TargetPos();
				IntVec3 cell = vector2.ToIntVec3();
				((MoteDualAttached)this.aimLineMote).UpdateTargets(this.verb.Caster, new TargetInfo(cell, this.verb.Caster.Map, false), Vector3.zero, vector2 - cell.ToVector3Shifted());
			}
			if (this.aimTargetMote != null)
			{
				this.aimTargetMote.paused = stunned;
				this.aimTargetMote.exactPosition = this.focusTarg.CenterVector3;
				this.aimTargetMote.exactRotation = exactRotation;
				this.aimTargetMote.Maintain();
			}
			if (this.aimChargeMote != null)
			{
				this.aimChargeMote.paused = stunned;
				this.aimChargeMote.exactRotation = exactRotation;
				this.aimChargeMote.exactPosition = this.verb.Caster.Position.ToVector3Shifted() + vector * this.verb.verbProps.aimingChargeMoteOffset;
				this.aimChargeMote.Maintain();
			}
			if (!this.stanceTracker.stunner.Stunned)
			{
				if (!this.targetStartedDowned && this.focusTarg.HasThing && this.focusTarg.Thing is Pawn && ((Pawn)this.focusTarg.Thing).Downed)
				{
					this.stanceTracker.SetStance(new Stance_Mobile());
					return;
				}
				if (this.focusTarg.HasThing && (!this.focusTarg.Thing.Spawned || this.verb == null || !this.verb.CanHitTargetFrom(base.Pawn.Position, this.focusTarg)))
				{
					this.stanceTracker.SetStance(new Stance_Mobile());
					return;
				}
				if (this.focusTarg == base.Pawn.mindState.enemyTarget)
				{
					base.Pawn.mindState.Notify_EngagedTarget();
				}
			}
			base.StanceTick();
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x0009BD7B File Offset: 0x00099F7B
		public void Interrupt()
		{
			base.Expire();
			Effecter effecter = this.effecter;
			if (effecter == null)
			{
				return;
			}
			effecter.Cleanup();
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x0009BD93 File Offset: 0x00099F93
		protected override void Expire()
		{
			Verb verb = this.verb;
			if (verb != null)
			{
				verb.WarmupComplete();
			}
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			base.Expire();
		}

		// Token: 0x040012CE RID: 4814
		private Sustainer sustainer;

		// Token: 0x040012CF RID: 4815
		protected Effecter effecter;

		// Token: 0x040012D0 RID: 4816
		private Mote aimLineMote;

		// Token: 0x040012D1 RID: 4817
		private Mote aimChargeMote;

		// Token: 0x040012D2 RID: 4818
		private Mote aimTargetMote;

		// Token: 0x040012D3 RID: 4819
		private bool targetStartedDowned;

		// Token: 0x040012D4 RID: 4820
		private bool drawAimPie = true;

		// Token: 0x040012D5 RID: 4821
		private bool needsReInitAfterLoad;
	}
}

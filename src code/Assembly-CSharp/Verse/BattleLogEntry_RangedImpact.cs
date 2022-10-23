using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200015F RID: 351
	public class BattleLogEntry_RangedImpact : LogEntry_DamageResult
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x0002D41C File Offset: 0x0002B61C
		private string InitiatorName
		{
			get
			{
				if (this.initiatorPawn != null)
				{
					return this.initiatorPawn.LabelShort;
				}
				if (this.initiatorThing != null)
				{
					return this.initiatorThing.defName;
				}
				return "null";
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x0002D44B File Offset: 0x0002B64B
		private string RecipientName
		{
			get
			{
				if (this.recipientPawn != null)
				{
					return this.recipientPawn.LabelShort;
				}
				if (this.recipientThing != null)
				{
					return this.recipientThing.defName;
				}
				return "null";
			}
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002C1CD File Offset: 0x0002A3CD
		public BattleLogEntry_RangedImpact() : base(null)
		{
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002D47C File Offset: 0x0002B67C
		public BattleLogEntry_RangedImpact(Thing initiator, Thing recipient, Thing originalTarget, ThingDef weaponDef, ThingDef projectileDef, ThingDef coverDef) : base(null)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (recipient is Pawn)
			{
				this.recipientPawn = (recipient as Pawn);
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			if (originalTarget is Pawn)
			{
				this.originalTargetPawn = (originalTarget as Pawn);
				this.originalTargetMobile = (!this.originalTargetPawn.Downed && !this.originalTargetPawn.Dead && this.originalTargetPawn.Awake());
			}
			else if (originalTarget != null)
			{
				this.originalTargetThing = originalTarget.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.coverDef = coverDef;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002D545 File Offset: 0x0002B745
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn || t == this.originalTargetPawn;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002D564 File Offset: 0x0002B764
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			if (this.originalTargetPawn != null)
			{
				yield return this.originalTargetPawn;
			}
			yield break;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002D574 File Offset: 0x0002B774
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipientPawn != null && ((pov == this.initiatorPawn && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiatorPawn)));
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0002D5C4 File Offset: 0x0002B7C4
		public override void ClickedFromPOV(Thing pov)
		{
			if (this.recipientPawn == null)
			{
				return;
			}
			if (pov == this.initiatorPawn)
			{
				CameraJumper.TryJumpAndSelect(this.recipientPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.recipientPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0002D615 File Offset: 0x0002B815
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				return null;
			}
			if (this.deflected)
			{
				return null;
			}
			if (pov == null || pov == this.recipientPawn)
			{
				return LogEntry.Blood;
			}
			if (pov == this.initiatorPawn)
			{
				return LogEntry.BloodTarget;
			}
			return null;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002D652 File Offset: 0x0002B852
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002D670 File Offset: 0x0002B870
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn != null || this.recipientThing != null)
			{
				result.Includes.Add(this.deflected ? RulePackDefOf.Combat_RangedDeflect : RulePackDefOf.Combat_RangedDamage);
			}
			else
			{
				result.Includes.Add(RulePackDefOf.Combat_RangedMiss);
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants, true, true));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			else
			{
				result.Constants["INITIATOR_missing"] = "True";
			}
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			else
			{
				result.Constants["RECIPIENT_missing"] = "True";
			}
			if (this.originalTargetPawn != this.recipientPawn || this.originalTargetThing != this.recipientThing)
			{
				if (this.originalTargetPawn != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForPawn("ORIGINALTARGET", this.originalTargetPawn, result.Constants, true, true));
					result.Constants["ORIGINALTARGET_mobile"] = this.originalTargetMobile.ToString();
				}
				else if (this.originalTargetThing != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef("ORIGINALTARGET", this.originalTargetThing));
				}
				else
				{
					result.Constants["ORIGINALTARGET_missing"] = "True";
				}
			}
			if (this.weaponDef != null)
			{
				result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", this.weaponDef, this.projectileDef));
			}
			else
			{
				result.Constants["WEAPON_missing"] = "True";
				if (this.projectileDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef("PROJECTILE", this.projectileDef));
				}
			}
			if (this.initiatorPawn != null && this.initiatorPawn.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiatorPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			result.Constants["COVER_missing"] = ((this.coverDef != null) ? "False" : "True");
			if (this.coverDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("COVER", this.coverDef));
			}
			return result;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002D980 File Offset: 0x0002BB80
		public override bool ShowInCompactView()
		{
			if (!this.deflected)
			{
				if (this.recipientPawn != null)
				{
					return true;
				}
				if (this.originalTargetThing != null && this.originalTargetThing == this.recipientThing)
				{
					return true;
				}
			}
			int num = 1;
			if (this.weaponDef != null && !this.weaponDef.Verbs.NullOrEmpty<VerbProperties>())
			{
				num = this.weaponDef.Verbs[0].burstShotCount;
			}
			return Rand.ChanceSeeded(BattleLogEntry_RangedImpact.DisplayChanceOnMiss / (float)num, this.logID);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002DA00 File Offset: 0x0002BC00
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_References.Look<Pawn>(ref this.originalTargetPawn, "originalTargetPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.originalTargetThing, "originalTargetThing");
			Scribe_Values.Look<bool>(ref this.originalTargetMobile, "originalTargetMobile", false, false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Defs.Look<ThingDef>(ref this.coverDef, "coverDef");
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0002DAB8 File Offset: 0x0002BCB8
		public override string ToString()
		{
			return "BattleLogEntry_RangedImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x040009AF RID: 2479
		private Pawn initiatorPawn;

		// Token: 0x040009B0 RID: 2480
		private ThingDef initiatorThing;

		// Token: 0x040009B1 RID: 2481
		private Pawn recipientPawn;

		// Token: 0x040009B2 RID: 2482
		private ThingDef recipientThing;

		// Token: 0x040009B3 RID: 2483
		private Pawn originalTargetPawn;

		// Token: 0x040009B4 RID: 2484
		private ThingDef originalTargetThing;

		// Token: 0x040009B5 RID: 2485
		private bool originalTargetMobile;

		// Token: 0x040009B6 RID: 2486
		private ThingDef weaponDef;

		// Token: 0x040009B7 RID: 2487
		private ThingDef projectileDef;

		// Token: 0x040009B8 RID: 2488
		private ThingDef coverDef;

		// Token: 0x040009B9 RID: 2489
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.25f;
	}
}

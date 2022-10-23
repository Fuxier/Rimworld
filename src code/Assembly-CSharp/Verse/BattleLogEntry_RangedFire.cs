using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200015E RID: 350
	public class BattleLogEntry_RangedFire : LogEntry
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0002CFB8 File Offset: 0x0002B1B8
		private string InitiatorName
		{
			get
			{
				if (this.initiatorPawn == null)
				{
					return "null";
				}
				return this.initiatorPawn.LabelShort;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0002CFD3 File Offset: 0x0002B1D3
		private string RecipientName
		{
			get
			{
				if (this.recipientPawn == null)
				{
					return "null";
				}
				return this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002C32E File Offset: 0x0002A52E
		public BattleLogEntry_RangedFire() : base(null)
		{
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002CFF0 File Offset: 0x0002B1F0
		public BattleLogEntry_RangedFire(Thing initiator, Thing target, ThingDef weaponDef, ThingDef projectileDef, bool burst) : base(null)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (target is Pawn)
			{
				this.recipientPawn = (target as Pawn);
			}
			else if (target != null)
			{
				this.recipientThing = target.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.burst = burst;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002D065 File Offset: 0x0002B265
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002D07B File Offset: 0x0002B27B
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
			yield break;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002D08C File Offset: 0x0002B28C
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipientPawn != null && ((pov == this.initiatorPawn && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiatorPawn)));
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002D0DC File Offset: 0x0002B2DC
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

		// Token: 0x06000943 RID: 2371 RVA: 0x0002D130 File Offset: 0x0002B330
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.initiatorPawn == null && this.initiatorThing == null)
			{
				Log.ErrorOnce("BattleLogEntry_RangedFire has a null initiator.", 60465709);
			}
			if (this.weaponDef != null && this.weaponDef.Verbs[0].rangedFireRulepack != null)
			{
				result.Includes.Add(this.weaponDef.Verbs[0].rangedFireRulepack);
			}
			else
			{
				result.Includes.Add(RulePackDefOf.Combat_RangedFire);
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
			result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", this.weaponDef, this.projectileDef));
			if (this.initiatorPawn != null && this.initiatorPawn.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiatorPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			result.Constants["BURST"] = this.burst.ToString();
			return result;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002D357 File Offset: 0x0002B557
		public override bool ShowInCompactView()
		{
			return Rand.ChanceSeeded(BattleLogEntry_RangedFire.DisplayChance, this.logID);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002D36C File Offset: 0x0002B56C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Values.Look<bool>(ref this.burst, "burst", false, false);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0002D3F3 File Offset: 0x0002B5F3
		public override string ToString()
		{
			return "BattleLogEntry_RangedFire: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x040009A7 RID: 2471
		private Pawn initiatorPawn;

		// Token: 0x040009A8 RID: 2472
		private ThingDef initiatorThing;

		// Token: 0x040009A9 RID: 2473
		private Pawn recipientPawn;

		// Token: 0x040009AA RID: 2474
		private ThingDef recipientThing;

		// Token: 0x040009AB RID: 2475
		private ThingDef weaponDef;

		// Token: 0x040009AC RID: 2476
		private ThingDef projectileDef;

		// Token: 0x040009AD RID: 2477
		private bool burst;

		// Token: 0x040009AE RID: 2478
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChance = 0.25f;
	}
}

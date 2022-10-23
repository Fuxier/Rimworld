using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200015B RID: 347
	public class BattleLogEntry_ExplosionImpact : LogEntry_DamageResult
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x0002C58E File Offset: 0x0002A78E
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

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x0002C5BD File Offset: 0x0002A7BD
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

		// Token: 0x0600091B RID: 2331 RVA: 0x0002C1CD File Offset: 0x0002A3CD
		public BattleLogEntry_ExplosionImpact() : base(null)
		{
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0002C5EC File Offset: 0x0002A7EC
		public BattleLogEntry_ExplosionImpact(Thing initiator, Thing recipient, ThingDef weaponDef, ThingDef projectileDef, DamageDef damageDef) : base(null)
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
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.damageDef = damageDef;
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0002C661 File Offset: 0x0002A861
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0002C677 File Offset: 0x0002A877
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

		// Token: 0x0600091F RID: 2335 RVA: 0x0002C688 File Offset: 0x0002A888
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.initiatorPawn && this.recipientPawn != null && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiatorPawn));
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0002C6D8 File Offset: 0x0002A8D8
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

		// Token: 0x06000921 RID: 2337 RVA: 0x0002C729 File Offset: 0x0002A929
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
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

		// Token: 0x06000922 RID: 2338 RVA: 0x0002C75C File Offset: 0x0002A95C
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0002C778 File Offset: 0x0002A978
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Includes.Add(RulePackDefOf.Combat_ExplosionImpact);
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
			if (this.projectileDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("PROJECTILE", this.projectileDef));
			}
			if (this.damageDef != null && this.damageDef.combatLogRules != null)
			{
				result.Includes.Add(this.damageDef.combatLogRules);
			}
			return result;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0002C8E8 File Offset: 0x0002AAE8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Defs.Look<DamageDef>(ref this.damageDef, "damageDef");
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0002C96D File Offset: 0x0002AB6D
		public override string ToString()
		{
			return "BattleLogEntry_ExplosionImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x04000995 RID: 2453
		private Pawn initiatorPawn;

		// Token: 0x04000996 RID: 2454
		private ThingDef initiatorThing;

		// Token: 0x04000997 RID: 2455
		private Pawn recipientPawn;

		// Token: 0x04000998 RID: 2456
		private ThingDef recipientThing;

		// Token: 0x04000999 RID: 2457
		private ThingDef weaponDef;

		// Token: 0x0400099A RID: 2458
		private ThingDef projectileDef;

		// Token: 0x0400099B RID: 2459
		private DamageDef damageDef;
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200015D RID: 349
	public class BattleLogEntry_MeleeCombat : LogEntry_DamageResult
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0002C9E9 File Offset: 0x0002ABE9
		private string InitiatorName
		{
			get
			{
				if (this.initiator == null)
				{
					return "null";
				}
				return this.initiator.LabelShort;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0002CA04 File Offset: 0x0002AC04
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

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0002CA1F File Offset: 0x0002AC1F
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x0002CA27 File Offset: 0x0002AC27
		public RulePackDef RuleDef
		{
			get
			{
				return this.ruleDef;
			}
			set
			{
				this.ruleDef = value;
				base.ResetCache();
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002C1CD File Offset: 0x0002A3CD
		public BattleLogEntry_MeleeCombat() : base(null)
		{
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002CA38 File Offset: 0x0002AC38
		public BattleLogEntry_MeleeCombat(RulePackDef ruleDef, bool alwaysShowInCompact, Pawn initiator, Thing recipient, ImplementOwnerTypeDef implementType, string toolLabel, ThingDef ownerEquipmentDef = null, HediffDef ownerHediffDef = null, LogEntryDef def = null) : base(def)
		{
			this.ruleDef = ruleDef;
			this.alwaysShowInCompact = alwaysShowInCompact;
			this.initiator = initiator;
			this.implementType = implementType;
			this.ownerEquipmentDef = ownerEquipmentDef;
			this.ownerHediffDef = ownerHediffDef;
			this.toolLabel = toolLabel;
			if (recipient is Pawn)
			{
				this.recipientPawn = (recipient as Pawn);
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			if (ownerEquipmentDef != null && ownerHediffDef != null)
			{
				Log.ErrorOnce(string.Format("Combat log owned by both equipment {0} and hediff {1}, may produce unexpected results", ownerEquipmentDef.label, ownerHediffDef.label), 96474669);
			}
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002CAD5 File Offset: 0x0002ACD5
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipientPawn;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0002CAEB File Offset: 0x0002ACEB
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			yield break;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0002CAFC File Offset: 0x0002ACFC
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.initiator && this.recipientPawn != null && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiator));
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002CB4C File Offset: 0x0002AD4C
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator && this.recipientPawn != null)
			{
				CameraJumper.TryJumpAndSelect(this.recipientPawn, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.recipientPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			if (this.recipientPawn != null)
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0002CBA8 File Offset: 0x0002ADA8
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				return this.def.iconMissTex;
			}
			if (this.deflected)
			{
				return this.def.iconMissTex;
			}
			if (pov == null || pov == this.recipientPawn)
			{
				return this.def.iconDamagedTex;
			}
			if (pov == this.initiator)
			{
				return this.def.iconDamagedFromInstigatorTex;
			}
			return this.def.iconDamagedTex;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0002CC1A File Offset: 0x0002AE1A
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0002CC38 File Offset: 0x0002AE38
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, result.Constants, true, true));
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			result.Includes.Add(this.ruleDef);
			if (!this.toolLabel.NullOrEmpty())
			{
				result.Rules.Add(new Rule_String("TOOL_label", this.toolLabel));
				result.Rules.Add(new Rule_String("TOOL_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.toolLabel, false, false)));
				result.Rules.Add(new Rule_String("TOOL_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.toolLabel, false, false)));
				result.Constants["TOOL_gender"] = LanguageDatabase.activeLanguage.ResolveGender(this.toolLabel, null, Gender.Male).ToString();
			}
			if (this.implementType != null && !this.implementType.implementOwnerRuleName.NullOrEmpty())
			{
				if (this.ownerEquipmentDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerEquipmentDef));
				}
				else if (this.ownerHediffDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerHediffDef));
				}
			}
			if (this.initiator != null && this.initiator.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiator.skills.GetSkill(SkillDefOf.Melee).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Melee).Level.ToStringCached();
			}
			if (this.implementType != null && !this.implementType.implementOwnerTypeValue.NullOrEmpty())
			{
				result.Constants["IMPLEMENTOWNER_type"] = this.implementType.implementOwnerTypeValue;
			}
			return result;
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0002CEB4 File Offset: 0x0002B0B4
		public override bool ShowInCompactView()
		{
			return this.alwaysShowInCompact || Rand.ChanceSeeded(BattleLogEntry_MeleeCombat.DisplayChanceOnMiss, this.logID);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0002CED0 File Offset: 0x0002B0D0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
			Scribe_Values.Look<bool>(ref this.alwaysShowInCompact, "alwaysShowInCompact", false, false);
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ImplementOwnerTypeDef>(ref this.implementType, "implementType");
			Scribe_Defs.Look<ThingDef>(ref this.ownerEquipmentDef, "ownerDef");
			Scribe_Values.Look<string>(ref this.toolLabel, "toolLabel", null, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0002CF6F File Offset: 0x0002B16F
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.ruleDef.defName,
				": ",
				this.InitiatorName,
				"->",
				this.RecipientName
			});
		}

		// Token: 0x0400099D RID: 2461
		private RulePackDef ruleDef;

		// Token: 0x0400099E RID: 2462
		private Pawn initiator;

		// Token: 0x0400099F RID: 2463
		private Pawn recipientPawn;

		// Token: 0x040009A0 RID: 2464
		private ThingDef recipientThing;

		// Token: 0x040009A1 RID: 2465
		private ImplementOwnerTypeDef implementType;

		// Token: 0x040009A2 RID: 2466
		private ThingDef ownerEquipmentDef;

		// Token: 0x040009A3 RID: 2467
		private HediffDef ownerHediffDef;

		// Token: 0x040009A4 RID: 2468
		private string toolLabel;

		// Token: 0x040009A5 RID: 2469
		public bool alwaysShowInCompact;

		// Token: 0x040009A6 RID: 2470
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.5f;
	}
}

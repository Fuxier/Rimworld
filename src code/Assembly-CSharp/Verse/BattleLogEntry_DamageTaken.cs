using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000159 RID: 345
	public class BattleLogEntry_DamageTaken : LogEntry_DamageResult
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x0002C1B2 File Offset: 0x0002A3B2
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

		// Token: 0x06000904 RID: 2308 RVA: 0x0002C1CD File Offset: 0x0002A3CD
		public BattleLogEntry_DamageTaken() : base(null)
		{
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0002C1D6 File Offset: 0x0002A3D6
		public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef, Pawn initiator = null) : base(null)
		{
			this.initiatorPawn = initiator;
			this.recipientPawn = recipient;
			this.ruleDef = ruleDef;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0002C1F4 File Offset: 0x0002A3F4
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0002C20A File Offset: 0x0002A40A
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

		// Token: 0x06000908 RID: 2312 RVA: 0x0002C21A File Offset: 0x0002A41A
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return CameraJumper.CanJump(this.recipientPawn);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0002C22C File Offset: 0x0002A42C
		public override void ClickedFromPOV(Thing pov)
		{
			CameraJumper.TryJumpAndSelect(this.recipientPawn, CameraJumper.MovementMode.Pan);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0002C23F File Offset: 0x0002A43F
		public override Texture2D IconFromPOV(Thing pov)
		{
			return LogEntry.Blood;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0002C246 File Offset: 0x0002A446
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0002C264 File Offset: 0x0002A464
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn == null)
			{
				Log.ErrorOnce("BattleLogEntry_DamageTaken has a null recipient.", 60465709);
			}
			result.Includes.Add(this.ruleDef);
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			return result;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0002C2C7 File Offset: 0x0002A4C7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0002C301 File Offset: 0x0002A501
		public override string ToString()
		{
			return "BattleLogEntry_DamageTaken: " + this.RecipientName;
		}

		// Token: 0x0400098D RID: 2445
		private Pawn initiatorPawn;

		// Token: 0x0400098E RID: 2446
		private Pawn recipientPawn;

		// Token: 0x0400098F RID: 2447
		private RulePackDef ruleDef;
	}
}

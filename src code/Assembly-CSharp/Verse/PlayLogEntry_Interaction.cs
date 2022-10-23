using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200016C RID: 364
	public class PlayLogEntry_Interaction : LogEntry
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x0002FD09 File Offset: 0x0002DF09
		protected string InitiatorName
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

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0002FD24 File Offset: 0x0002DF24
		private string RecipientName
		{
			get
			{
				if (this.recipient == null)
				{
					return "null";
				}
				return this.recipient.LabelShort;
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0002C32E File Offset: 0x0002A52E
		public PlayLogEntry_Interaction() : base(null)
		{
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0002FD3F File Offset: 0x0002DF3F
		public PlayLogEntry_Interaction(InteractionDef intDef, Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks) : base(null)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.recipient = recipient;
			this.extraSentencePacks = extraSentencePacks;
			this.initiatorFaction = initiator.Faction;
			this.initiatorIdeo = initiator.Ideo;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002FD7D File Offset: 0x0002DF7D
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipient;
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0002FD93 File Offset: 0x0002DF93
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.recipient != null)
			{
				yield return this.recipient;
			}
			yield break;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0002FDA3 File Offset: 0x0002DFA3
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.recipient && CameraJumper.CanJump(this.initiator)) || (pov == this.initiator && CameraJumper.CanJump(this.recipient));
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0002FDDD File Offset: 0x0002DFDD
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.recipient, CameraJumper.MovementMode.Pan);
				return;
			}
			if (pov == this.recipient)
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0002FE1A File Offset: 0x0002E01A
		public override Texture2D IconFromPOV(Thing pov)
		{
			return this.intDef.GetSymbol(this.initiatorFaction, this.initiatorIdeo);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0002FE33 File Offset: 0x0002E033
		public override Color? IconColorFromPOV(Thing pov)
		{
			return this.intDef.GetSymbolColor(this.initiatorFaction);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0002FE46 File Offset: 0x0002E046
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.initiatorFaction == faction)
			{
				this.initiatorFaction = null;
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0002FE58 File Offset: 0x0002E058
		public override void Notify_IdeoRemoved(Ideo ideo)
		{
			if (this.initiatorIdeo == ideo)
			{
				this.initiatorIdeo = null;
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0002FE6A File Offset: 0x0002E06A
		public override string GetTipString()
		{
			return this.intDef.LabelCap + "\n" + base.GetTipString();
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0002FE94 File Offset: 0x0002E094
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			if (this.initiator == null || this.recipient == null)
			{
				Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422);
				return "[" + this.intDef.label + " error: null pawn reference]";
			}
			Rand.PushState();
			Rand.Seed = this.logID;
			GrammarRequest request = base.GenerateGrammarRequest();
			string text;
			if (pov == this.initiator)
			{
				request.IncludesBare.Add(this.intDef.logRulesInitiator);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants, true, true));
				text = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog, null, null, null, true);
			}
			else if (pov == this.recipient)
			{
				if (this.intDef.logRulesRecipient != null)
				{
					request.IncludesBare.Add(this.intDef.logRulesRecipient);
				}
				else
				{
					request.IncludesBare.Add(this.intDef.logRulesInitiator);
				}
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants, true, true));
				text = GrammarResolver.Resolve("r_logentry", request, "interaction from recipient", forceLog, null, null, null, true);
			}
			else
			{
				Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251);
				text = this.ToString();
			}
			if (this.extraSentencePacks != null)
			{
				for (int i = 0; i < this.extraSentencePacks.Count; i++)
				{
					request.Clear();
					request.Includes.Add(this.extraSentencePacks[i]);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants, true, true));
					text = text + " " + GrammarResolver.Resolve(this.extraSentencePacks[i].FirstRuleKeyword, request, "extraSentencePack", forceLog, this.extraSentencePacks[i].FirstUntranslatedRuleKeyword, null, null, true);
				}
			}
			Rand.PopState();
			return text;
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0003010C File Offset: 0x0002E30C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<InteractionDef>(ref this.intDef, "intDef");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipient, "recipient", true);
			Scribe_Collections.Look<RulePackDef>(ref this.extraSentencePacks, "extras", LookMode.Undefined, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.initiatorFaction, "initiatorFaction", false);
			Scribe_References.Look<Ideo>(ref this.initiatorIdeo, "initiatorIdeo", false);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00030189 File Offset: 0x0002E389
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.intDef.label,
				": ",
				this.InitiatorName,
				"->",
				this.RecipientName
			});
		}

		// Token: 0x04000A09 RID: 2569
		protected InteractionDef intDef;

		// Token: 0x04000A0A RID: 2570
		protected Pawn initiator;

		// Token: 0x04000A0B RID: 2571
		protected Pawn recipient;

		// Token: 0x04000A0C RID: 2572
		protected List<RulePackDef> extraSentencePacks;

		// Token: 0x04000A0D RID: 2573
		public Faction initiatorFaction;

		// Token: 0x04000A0E RID: 2574
		public Ideo initiatorIdeo;
	}
}

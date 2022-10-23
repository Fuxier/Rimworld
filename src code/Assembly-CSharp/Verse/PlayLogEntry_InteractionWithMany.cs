using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200016D RID: 365
	public class PlayLogEntry_InteractionWithMany : PlayLogEntry_Interaction
	{
		// Token: 0x060009DB RID: 2523 RVA: 0x000301C6 File Offset: 0x0002E3C6
		public PlayLogEntry_InteractionWithMany()
		{
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x000301CE File Offset: 0x0002E3CE
		public PlayLogEntry_InteractionWithMany(InteractionDef intDef, Pawn initiator, List<Pawn> recipients, List<RulePackDef> extraSentencePacks)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.recipients = recipients;
			this.extraSentencePacks = extraSentencePacks;
			this.initiatorFaction = initiator.Faction;
			this.initiatorIdeo = initiator.Ideo;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0003020C File Offset: 0x0002E40C
		public override bool Concerns(Thing t)
		{
			Pawn item;
			return (item = (t as Pawn)) != null && (t == this.initiator || this.recipients.Contains(item));
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0003023C File Offset: 0x0002E43C
		public override IEnumerable<Thing> GetConcerns()
		{
			yield return this.initiator;
			foreach (Pawn pawn in this.recipients)
			{
				yield return pawn;
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0003024C File Offset: 0x0002E44C
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			if (this.initiator == null || this.recipients == null || this.recipients.Contains(null))
			{
				Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422);
				return "[" + this.intDef.label + " error: null pawn reference]";
			}
			Rand.PushState();
			Rand.Seed = this.logID;
			GrammarRequest request = base.GenerateGrammarRequest();
			string result;
			Pawn pawn;
			if (pov == this.initiator)
			{
				request.IncludesBare.Add(this.intDef.logRulesInitiator);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants, true, true));
				result = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog, null, null, null, true);
			}
			else if ((pawn = (pov as Pawn)) != null && this.recipients.Contains(pawn))
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
				request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", pawn, request.Constants, true, true));
				result = GrammarResolver.Resolve("r_logentry", request, "interaction from recipient", forceLog, null, null, null, true);
			}
			else
			{
				Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251);
				result = this.ToString();
			}
			Rand.PopState();
			return result;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x000303EC File Offset: 0x0002E5EC
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipients.Contains(pov as Pawn) && CameraJumper.CanJump(this.initiator);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00030414 File Offset: 0x0002E614
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.recipients.RandomElement<Pawn>(), CameraJumper.MovementMode.Pan);
				return;
			}
			if (this.recipients.Contains(pov as Pawn))
			{
				CameraJumper.TryJumpAndSelect(this.initiator, CameraJumper.MovementMode.Pan);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0003046B File Offset: 0x0002E66B
		public override string ToString()
		{
			return this.intDef.label + ": " + base.InitiatorName + "-> Many";
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00030490 File Offset: 0x0002E690
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.recipients, "recipients", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.recipients.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04000A0F RID: 2575
		private List<Pawn> recipients;
	}
}

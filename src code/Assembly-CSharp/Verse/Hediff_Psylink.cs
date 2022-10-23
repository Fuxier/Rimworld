using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200034F RID: 847
	public class Hediff_Psylink : Hediff_Level
	{
		// Token: 0x060016BE RID: 5822 RVA: 0x000857C4 File Offset: 0x000839C4
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.TryGiveAbilityOfLevel(this.level, !this.suppressPostAddLetter);
			Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.Notify_GainedPsylink();
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x000857F8 File Offset: 0x000839F8
		public void ChangeLevel(int levelOffset, bool sendLetter)
		{
			if (levelOffset > 0)
			{
				float num = Math.Min((float)levelOffset, this.def.maxSeverity - (float)this.level);
				int num2 = 0;
				while ((float)num2 < num)
				{
					int abilityLevel = this.level + 1 + num2;
					this.TryGiveAbilityOfLevel(abilityLevel, sendLetter);
					Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
					if (psychicEntropy != null)
					{
						psychicEntropy.Notify_GainedPsylink();
					}
					num2++;
				}
			}
			base.ChangeLevel(levelOffset);
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x00085861 File Offset: 0x00083A61
		public override void ChangeLevel(int levelOffset)
		{
			this.ChangeLevel(levelOffset, true);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x0008586C File Offset: 0x00083A6C
		public static string MakeLetterTextNewPsylinkLevel(Pawn pawn, int abilityLevel, IEnumerable<AbilityDef> newAbilities = null)
		{
			string text = ((abilityLevel == 1) ? "LetterPsylinkLevelGained_First" : "LetterPsylinkLevelGained_NotFirst").Translate(pawn.Named("USER"));
			if (!newAbilities.EnumerableNullOrEmpty<AbilityDef>())
			{
				text += "\n\n" + "LetterPsylinkLevelGained_PsycastLearned".Translate(pawn.Named("USER"), abilityLevel, (from a in newAbilities
				select a.LabelCap.Resolve()).ToLineList(null, false));
			}
			return text;
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x0008590C File Offset: 0x00083B0C
		public void TryGiveAbilityOfLevel(int abilityLevel, bool sendLetter = true)
		{
			string str = "LetterLabelPsylinkLevelGained".Translate() + ": " + this.pawn.LabelShortCap;
			string str2;
			if (!this.pawn.abilities.abilities.Any((Ability a) => a.def.level == abilityLevel))
			{
				AbilityDef abilityDef = (from a in DefDatabase<AbilityDef>.AllDefs
				where a.level == abilityLevel
				select a).RandomElement<AbilityDef>();
				this.pawn.abilities.GainAbility(abilityDef);
				str2 = Hediff_Psylink.MakeLetterTextNewPsylinkLevel(this.pawn, abilityLevel, Gen.YieldSingle<AbilityDef>(abilityDef));
			}
			else
			{
				str2 = Hediff_Psylink.MakeLetterTextNewPsylinkLevel(this.pawn, abilityLevel, null);
			}
			if (sendLetter && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Find.LetterStack.ReceiveLetter(str, str2, LetterDefOf.PositiveEvent, this.pawn, null, null, null, null);
			}
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00085A09 File Offset: 0x00083C09
		public override void PostRemoved()
		{
			base.PostRemoved();
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs == null)
			{
				return;
			}
			needs.AddOrRemoveNeedsAsAppropriate();
		}

		// Token: 0x040011A0 RID: 4512
		public bool suppressPostAddLetter;
	}
}

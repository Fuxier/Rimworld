using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020004A8 RID: 1192
	public class DeathLetter : ChoiceLetter
	{
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060023E0 RID: 9184 RVA: 0x000E56E0 File Offset: 0x000E38E0
		protected DiaOption Option_ReadMore
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("ReadMore".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target, CameraJumper.MovementMode.Pan);
					Find.LetterStack.RemoveLetter(this);
					InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Log));
				};
				diaOption.resolveTree = true;
				if (!target.IsValid)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060023E1 RID: 9185 RVA: 0x000E574E File Offset: 0x000E394E
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.Option_Close;
				if (this.lookTargets.IsValid())
				{
					yield return this.Option_ReadMore;
				}
				if (this.quest != null)
				{
					yield return base.Option_ViewInQuestsTab("ViewRelatedQuest", false);
				}
				yield break;
			}
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x000E5760 File Offset: 0x000E3960
		public override void OpenLetter()
		{
			Pawn targetPawn = this.lookTargets.TryGetPrimaryTarget().Thing as Pawn;
			TaggedString taggedString = base.Text;
			Func<LogEntry, bool> <>9__4;
			string text = (from entry in (from entry in (from battle in Find.BattleLog.Battles
			where battle.Concerns(targetPawn)
			select battle).SelectMany(delegate(Battle battle)
			{
				IEnumerable<LogEntry> entries = battle.Entries;
				Func<LogEntry, bool> predicate;
				if ((predicate = <>9__4) == null)
				{
					predicate = (<>9__4 = ((LogEntry entry) => entry.Concerns(targetPawn) && entry.ShowInCompactView()));
				}
				return entries.Where(predicate);
			})
			orderby entry.Age
			select entry).Take(5).Reverse<LogEntry>()
			select "  " + entry.ToGameStringFromPOV(null, false)).ToLineList(null, false);
			if (text.Length > 0)
			{
				taggedString = string.Format("{0}\n\n{1}\n{2}", taggedString, "LastEventsInLife".Translate(targetPawn.LabelDefinite(), targetPawn.Named("PAWN")).Resolve() + ":", text);
			}
			DiaNode diaNode = new DiaNode(taggedString);
			diaNode.options.AddRange(this.Choices);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, this.radioMode, this.title));
		}
	}
}

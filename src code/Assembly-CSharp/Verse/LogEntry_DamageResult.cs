using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000169 RID: 361
	public abstract class LogEntry_DamageResult : LogEntry
	{
		// Token: 0x060009B9 RID: 2489 RVA: 0x0002F9B1 File Offset: 0x0002DBB1
		public LogEntry_DamageResult(LogEntryDef def = null) : base(def)
		{
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0002F9BA File Offset: 0x0002DBBA
		public void FillTargets(List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed, bool deflected)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
			this.deflected = deflected;
			base.ResetCache();
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x000029B0 File Offset: 0x00000BB0
		protected virtual BodyDef DamagedBody()
		{
			return null;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0002F9D8 File Offset: 0x0002DBD8
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.DamagedBody(), this.damagedParts, this.damagedPartsDestroyed, result.Constants));
			result.Constants.Add("deflected", this.deflected.ToString());
			return result;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0002FA38 File Offset: 0x0002DC38
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<BodyPartRecord>(ref this.damagedParts, "damagedParts", LookMode.BodyPart, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.deflected, "deflected", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.damagedParts != null)
			{
				for (int i = this.damagedParts.Count - 1; i >= 0; i--)
				{
					if (this.damagedParts[i] == null)
					{
						this.damagedParts.RemoveAt(i);
						if (i < this.damagedPartsDestroyed.Count)
						{
							this.damagedPartsDestroyed.RemoveAt(i);
						}
					}
				}
			}
		}

		// Token: 0x04000A04 RID: 2564
		protected List<BodyPartRecord> damagedParts;

		// Token: 0x04000A05 RID: 2565
		protected List<bool> damagedPartsDestroyed;

		// Token: 0x04000A06 RID: 2566
		protected bool deflected;
	}
}

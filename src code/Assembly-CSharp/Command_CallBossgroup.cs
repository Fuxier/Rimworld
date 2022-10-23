using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

// Token: 0x02000016 RID: 22
public class Command_CallBossgroup : Command
{
	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000072 RID: 114 RVA: 0x0000521A File Offset: 0x0000341A
	private IEnumerable<FloatMenuOption> FloatMenuOptions
	{
		get
		{
			IEnumerable<BossgroupDef> allDefs = DefDatabase<BossgroupDef>.AllDefs;
			using (IEnumerator<BossgroupDef> enumerator = allDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BossgroupDef bg = enumerator.Current;
					AcceptanceReport report = CallBossgroupUtility.BossgroupEverCallable(this.mechanitor.Pawn, bg, true);
					if (!report)
					{
						yield return new FloatMenuOption("CannotSummon".Translate(bg.boss.kindDef.label) + ": " + report.Reason, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
					else
					{
						yield return new FloatMenuOption("Summon".Translate(bg.boss.kindDef.label), delegate()
						{
							CallBossgroupUtility.TryStartSummonBossgroupJob(bg, this.mechanitor.Pawn, true);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					}
				}
			}
			IEnumerator<BossgroupDef> enumerator = null;
			yield break;
			yield break;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000073 RID: 115 RVA: 0x0000522C File Offset: 0x0000342C
	public override string DescPostfix
	{
		get
		{
			string text = "";
			Dictionary<BossgroupDef, AcceptanceReport> source = DefDatabase<BossgroupDef>.AllDefs.ToDictionary((BossgroupDef b) => b, (BossgroupDef b) => CallBossgroupUtility.BossgroupEverCallable(this.mechanitor.Pawn, b, true));
			foreach (KeyValuePair<BossgroupDef, AcceptanceReport> keyValuePair in from b in source
			where b.Value
			select b)
			{
				text = text + "\n\n" + "ReadyToSummonThreat".Translate(keyValuePair.Key.boss.kindDef.label).Colorize(ColorLibrary.Green).CapitalizeFirst();
			}
			foreach (KeyValuePair<BossgroupDef, AcceptanceReport> keyValuePair2 in from b in source
			where !b.Value
			select b)
			{
				text = text + "\n\n" + ("CannotSummon".Translate(keyValuePair2.Key.boss.kindDef.label) + ": " + keyValuePair2.Value.Reason).Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x000053C0 File Offset: 0x000035C0
	public Command_CallBossgroup(Pawn_MechanitorTracker mechanitor)
	{
		this.mechanitor = mechanitor;
		this.defaultLabel = "CommandCallBossgroup".Translate();
		this.defaultDesc = "CommandCallBossgroupDesc".Translate();
		this.icon = Command_CallBossgroup.BossgroupTex.Texture;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00005417 File Offset: 0x00003617
	public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
	{
		this.disabled = this.IsDisabled(out this.disabledReason);
		return base.GizmoOnGUI(topLeft, maxWidth, parms);
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00005434 File Offset: 0x00003634
	private bool IsDisabled(out string reason)
	{
		int lastBossgroupCalled = Find.BossgroupManager.lastBossgroupCalled;
		int num = Find.TickManager.TicksGame - lastBossgroupCalled;
		if (!this.mechanitor.Pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		{
			reason = "Incapable".Translate().CapitalizeFirst();
			return true;
		}
		if (num < 120000)
		{
			reason = "BossgroupAvailableIn".Translate((120000 - num).ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst();
			return true;
		}
		if (this.FloatMenuOptions.All((FloatMenuOption f) => f.action == null))
		{
			reason = null;
			return true;
		}
		reason = null;
		return false;
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00005504 File Offset: 0x00003704
	public override void ProcessInput(Event ev)
	{
		base.ProcessInput(ev);
		List<FloatMenuOption> list = new List<FloatMenuOption>();
		list.AddRange(this.FloatMenuOptions);
		Find.WindowStack.Add(new FloatMenu(list));
	}

	// Token: 0x04000032 RID: 50
	private static readonly CachedTexture BossgroupTex = new CachedTexture("UI/Icons/SummonMechThreat");

	// Token: 0x04000033 RID: 51
	private Pawn_MechanitorTracker mechanitor;
}

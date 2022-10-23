using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200045C RID: 1116
	public static class DebugTools_Health
	{
		// Token: 0x06002253 RID: 8787 RVA: 0x000DB01C File Offset: 0x000D921C
		public static List<DebugMenuOption> Options_RestorePart(Pawn p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (BodyPartRecord localPart2 in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.health.RestorePart(localPart, null, true);
				}));
			}
			return list;
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000DB0D8 File Offset: 0x000D92D8
		public static List<DebugActionNode> Options_ApplyDamage()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (DamageDef localDef2 in DefDatabase<DamageDef>.AllDefs)
			{
				DamageDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, null, null)
				{
					actionType = DebugActionType.ToolMap,
					action = delegate()
					{
						Pawn pawn = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>().FirstOrDefault<Pawn>();
						if (pawn != null)
						{
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Damage_BodyParts(pawn, localDef)));
						}
					}
				});
			}
			return list;
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x000DB164 File Offset: 0x000D9364
		private static List<DebugMenuOption> Options_Damage_BodyParts(Pawn p, DamageDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.TakeDamage(new DamageInfo(def, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}));
			foreach (BodyPartRecord localPart2 in p.RaceProps.body.AllParts)
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.TakeDamage(new DamageInfo(def, 5f, 0f, -1f, null, localPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
				}));
			}
			return list;
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x000DB248 File Offset: 0x000D9448
		public static List<DebugMenuOption> Options_AddHediff(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (HediffDef localDef2 in from d in DefDatabase<HediffDef>.AllDefs
			orderby d.hediffClass.ToStringSafe<Type>()
			select d)
			{
				HediffDef localDef = localDef2;
				DebugMenuOptionMode mode = (pawn != null) ? DebugMenuOptionMode.Action : DebugMenuOptionMode.Tool;
				list.Add(new DebugMenuOption(localDef.LabelCap, mode, delegate()
				{
					Pawn pawn2;
					if (mode == DebugMenuOptionMode.Tool)
					{
						pawn2 = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).Where((Thing t) => t is Pawn).Cast<Pawn>().FirstOrDefault<Pawn>();
					}
					else
					{
						pawn2 = pawn;
					}
					if (pawn2 != null)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Hediff_BodyParts(pawn2, localDef)));
					}
				}));
			}
			return list;
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x000DB328 File Offset: 0x000D9528
		private static List<DebugMenuOption> Options_Hediff_BodyParts(Pawn p, HediffDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.health.AddHediff(def, null, null, null).PostDebugAdd();
			}));
			foreach (BodyPartRecord localPart2 in from pa in p.RaceProps.body.AllParts
			orderby pa.Label
			select pa)
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.health.AddHediff(def, localPart, null, null).PostDebugAdd();
				}));
			}
			return list;
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000DB428 File Offset: 0x000D9628
		public static List<DebugMenuOption> Options_RemoveHediff(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Hediff localH2 in pawn.health.hediffSet.hediffs)
			{
				Hediff localH = localH2;
				list.Add(new DebugMenuOption(localH.LabelCap + ((localH.Part != null) ? (" (" + localH.Part.def + ")") : ""), DebugMenuOptionMode.Action, delegate()
				{
					pawn.health.RemoveHediff(localH);
				}));
			}
			return list;
		}
	}
}

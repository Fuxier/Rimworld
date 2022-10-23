using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000331 RID: 817
	[StaticConstructorOnStartup]
	public class HediffComp_Immunizable : HediffComp_SeverityModifierBase
	{
		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x00081995 File Offset: 0x0007FB95
		public HediffCompProperties_Immunizable Props
		{
			get
			{
				return (HediffCompProperties_Immunizable)this.props;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x060015D1 RID: 5585 RVA: 0x000819A2 File Offset: 0x0007FBA2
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Hidden && this.FullyImmune)
				{
					return "DevelopedImmunityLower".Translate();
				}
				return null;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x000819C8 File Offset: 0x0007FBC8
		public override string CompTipStringExtra
		{
			get
			{
				if (!this.Hidden && base.Def.PossibleToDevelopImmunityNaturally() && !this.FullyImmune)
				{
					return "Immunity".Translate() + ": " + this.NaturalImmunity.ToStringPercent("0.#");
				}
				return null;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x00081A22 File Offset: 0x0007FC22
		public float NaturalImmunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def, true);
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x00081A40 File Offset: 0x0007FC40
		public float Immunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def, false);
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x060015D5 RID: 5589 RVA: 0x00081A5E File Offset: 0x0007FC5E
		public bool FullyImmune
		{
			get
			{
				return this.Immunity >= 1f;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x00081A70 File Offset: 0x0007FC70
		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (this.FullyImmune)
				{
					return HediffComp_Immunizable.IconImmune;
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x060015D7 RID: 5591 RVA: 0x00081A8A File Offset: 0x0007FC8A
		private bool Hidden
		{
			get
			{
				return (!Prefs.DevMode || !DebugSettings.godMode) && this.Props.hidden;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x00081AA8 File Offset: 0x0007FCA8
		private float SeverityFactorFromHediffs
		{
			get
			{
				float num = 1f;
				if (!this.Props.severityFactorsFromHediffs.NullOrEmpty<HediffDefFactor>())
				{
					for (int i = 0; i < this.Props.severityFactorsFromHediffs.Count; i++)
					{
						if (base.Pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.severityFactorsFromHediffs[i].HediffDef, false) != null)
						{
							num *= this.Props.severityFactorsFromHediffs[i].Factor;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00081B30 File Offset: 0x0007FD30
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDayNotImmuneRandomFactor = this.Props.severityPerDayNotImmuneRandomFactor.RandomInRange;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x00081B4F File Offset: 0x0007FD4F
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDayNotImmuneRandomFactor, "severityPerDayNotImmuneRandomFactor", 1f, false);
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x00081B6D File Offset: 0x0007FD6D
		public override float SeverityChangePerDay()
		{
			return (this.FullyImmune ? this.Props.severityPerDayImmune : (this.Props.severityPerDayNotImmune * this.severityPerDayNotImmuneRandomFactor)) * this.SeverityFactorFromHediffs;
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x00081BA0 File Offset: 0x0007FDA0
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.CompDebugString());
			if (this.severityPerDayNotImmuneRandomFactor != 1f)
			{
				stringBuilder.AppendLine("severityPerDayNotImmuneRandomFactor: " + this.severityPerDayNotImmuneRandomFactor.ToString("0.##"));
			}
			if (!base.Pawn.Dead)
			{
				ImmunityRecord immunityRecord = base.Pawn.health.immunity.GetImmunityRecord(base.Def);
				if (immunityRecord != null)
				{
					stringBuilder.AppendLine("immunity change per day: " + (immunityRecord.ImmunityChangePerTick(base.Pawn, true, this.parent) * 60000f).ToString("F3"));
					stringBuilder.AppendLine("pawn immunity gain speed: " + StatDefOf.ImmunityGainSpeed.ValueToString(base.Pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true, -1), ToStringNumberSense.Absolute, true));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001170 RID: 4464
		private float severityPerDayNotImmuneRandomFactor = 1f;

		// Token: 0x04001171 RID: 4465
		private static readonly Texture2D IconImmune = ContentFinder<Texture2D>.Get("UI/Icons/Medical/IconImmune", true);
	}
}

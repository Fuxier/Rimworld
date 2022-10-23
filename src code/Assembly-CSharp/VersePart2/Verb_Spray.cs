using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005B2 RID: 1458
	public abstract class Verb_Spray : Verb
	{
		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002CA9 RID: 11433 RVA: 0x0011AFF1 File Offset: 0x001191F1
		protected override int ShotsPerBurst
		{
			get
			{
				return this.verbProps.burstShotCount;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06002CAA RID: 11434 RVA: 0x0011BC6C File Offset: 0x00119E6C
		public override float? AimAngleOverride
		{
			get
			{
				if (this.state == VerbState.Bursting && this.Available())
				{
					return new float?((this.path[this.ShotsPerBurst - this.burstShotsLeft].ToVector3Shifted() - this.caster.DrawPos).AngleFlat());
				}
				return null;
			}
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x0011BCD0 File Offset: 0x00119ED0
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			ShootLine shootLine;
			bool flag = base.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out shootLine);
			if (this.verbProps.stopBurstWithoutLos && !flag)
			{
				return false;
			}
			if (base.EquipmentSource != null && this.burstShotsLeft <= 1)
			{
				CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
				if (comp != null)
				{
					comp.Notify_ProjectileLaunched();
				}
				CompReloadable comp2 = base.EquipmentSource.GetComp<CompReloadable>();
				if (comp2 != null)
				{
					comp2.UsedOnce();
				}
			}
			this.HitCell(this.path[this.ShotsPerBurst - this.burstShotsLeft]);
			this.lastShotTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x0011BDA1 File Offset: 0x00119FA1
		public override bool Available()
		{
			return this.ShotsPerBurst - this.burstShotsLeft >= 0;
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x0011BDB6 File Offset: 0x00119FB6
		public override void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.initialTargetPosition = this.currentTarget.CenterVector3;
			this.PreparePath();
			base.TryCastNextBurstShot();
		}

		// Token: 0x06002CAE RID: 11438
		protected abstract void PreparePath();

		// Token: 0x06002CAF RID: 11439 RVA: 0x0011BDE8 File Offset: 0x00119FE8
		protected virtual void HitCell(IntVec3 cell)
		{
			EffecterDef sprayEffecterDef = this.verbProps.sprayEffecterDef;
			if (sprayEffecterDef == null)
			{
				return;
			}
			sprayEffecterDef.Spawn(this.caster.Position, cell, this.caster.Map, 1f);
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x0011BE1C File Offset: 0x0011A01C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.path, "path", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<Vector3>(ref this.initialTargetPosition, "initialTargetPosition", default(Vector3), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.path == null)
			{
				this.path = new List<IntVec3>();
			}
		}

		// Token: 0x04001D35 RID: 7477
		protected List<IntVec3> path = new List<IntVec3>();

		// Token: 0x04001D36 RID: 7478
		protected Vector3 initialTargetPosition;
	}
}

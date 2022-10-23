using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000078 RID: 120
	public abstract class SubEffecter_DrifterEmote : SubEffecter
	{
		// Token: 0x060004AE RID: 1198 RVA: 0x0001A6A9 File Offset: 0x000188A9
		public SubEffecter_DrifterEmote(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001A6B4 File Offset: 0x000188B4
		protected void MakeMote(TargetInfo A, int overrideSpawnTick = -1)
		{
			Vector3 vector = A.HasThing ? A.Thing.DrawPos : A.Cell.ToVector3Shifted();
			if (vector.ShouldSpawnMotesAt(A.Map, true))
			{
				int randomInRange = this.def.burstCount.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					Mote mote = (Mote)ThingMaker.MakeThing(this.def.moteDef, null);
					mote.Scale = this.def.scale.RandomInRange;
					mote.exactPosition = vector + this.def.positionOffset + Gen.RandomHorizontalVector(this.def.positionRadius);
					mote.rotationRate = this.def.rotationRate.RandomInRange;
					mote.exactRotation = this.def.rotation.RandomInRange;
					if (overrideSpawnTick != -1)
					{
						mote.ForceSpawnTick(overrideSpawnTick);
					}
					MoteThrown moteThrown = mote as MoteThrown;
					if (moteThrown != null)
					{
						moteThrown.airTimeLeft = this.def.airTime.RandomInRange;
						moteThrown.SetVelocity(this.def.angle.RandomInRange, this.def.speed.RandomInRange);
					}
					if (A.HasThing)
					{
						mote.Attach(A);
					}
					GenSpawn.Spawn(mote, vector.ToIntVec3(), A.Map, WipeMode.Vanish);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F5 RID: 1013
	public class MoteAttached : Mote
	{
		// Token: 0x06001CD7 RID: 7383 RVA: 0x000AF009 File Offset: 0x000AD209
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000AF034 File Offset: 0x000AD234
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (this.link1.Linked)
			{
				bool flag = this.detachAfterTicks == -1 || Find.TickManager.TicksGame - this.spawnTick < this.detachAfterTicks;
				if (!this.link1.Target.ThingDestroyed && flag)
				{
					this.link1.UpdateDrawPos();
				}
				Vector3 b = this.def.mote.attachedDrawOffset;
				if (this.def.mote.attachedToHead)
				{
					Pawn pawn = this.link1.Target.Thing as Pawn;
					if (pawn != null)
					{
						bool humanlike = pawn.RaceProps.Humanlike;
						List<Vector3> headPosPerRotation = pawn.RaceProps.headPosPerRotation;
						Rot4 rotation = (pawn.GetPosture() == PawnPosture.Standing) ? (humanlike ? Rot4.North : pawn.Rotation) : pawn.Drawer.renderer.LayingFacing();
						if (humanlike)
						{
							b = pawn.Drawer.renderer.BaseHeadOffsetAt(rotation).RotatedBy(pawn.Drawer.renderer.BodyAngle());
						}
						else
						{
							float bodySizeFactor = pawn.ageTracker.CurLifeStage.bodySizeFactor;
							Vector2 vector = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize * bodySizeFactor;
							b = ((!headPosPerRotation.NullOrEmpty<Vector3>()) ? headPosPerRotation[rotation.AsInt].ScaledBy(new Vector3(vector.x, 1f, vector.y)) : (MoteAttached.animalHeadOffsets[rotation.AsInt] * pawn.BodySize));
						}
					}
				}
				this.exactPosition = this.link1.LastDrawPos + b;
				IntVec3 intVec = this.exactPosition.ToIntVec3();
				if (base.Spawned && !intVec.InBounds(base.Map))
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				base.Position = intVec;
			}
		}

		// Token: 0x04001478 RID: 5240
		private static readonly List<Vector3> animalHeadOffsets = new List<Vector3>
		{
			new Vector3(0f, 0f, 0.4f),
			new Vector3(0.4f, 0f, 0.25f),
			new Vector3(0f, 0f, 0.1f),
			new Vector3(-0.4f, 0f, 0.25f)
		};
	}
}

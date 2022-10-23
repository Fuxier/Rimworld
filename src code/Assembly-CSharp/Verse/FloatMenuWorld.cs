using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000487 RID: 1159
	public class FloatMenuWorld : FloatMenu
	{
		// Token: 0x06002325 RID: 8997 RVA: 0x000E0C3C File Offset: 0x000DEE3C
		public FloatMenuWorld(List<FloatMenuOption> options, string title, Vector2 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x000E0C50 File Offset: 0x000DEE50
		public override void DoWindowContents(Rect inRect)
		{
			Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
			if (caravan == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			if (Time.frameCount % 4 == 0)
			{
				List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(this.clickPos, caravan);
				List<FloatMenuOption> list2 = list;
				Vector2 vector = this.clickPos;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (!this.options[i].Disabled && !FloatMenuWorld.StillValid(this.options[i], list, caravan, ref list2, ref vector))
					{
						this.options[i].Disabled = true;
					}
				}
			}
			base.DoWindowContents(inRect);
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x000E0D00 File Offset: 0x000DEF00
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Caravan forCaravan)
		{
			List<FloatMenuOption> list = null;
			Vector2 vector = new Vector2(-9999f, -9999f);
			return FloatMenuWorld.StillValid(opt, curOpts, forCaravan, ref list, ref vector);
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x000E0D2C File Offset: 0x000DEF2C
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Caravan forCaravan, ref List<FloatMenuOption> cachedChoices, ref Vector2 cachedChoicesForPos)
		{
			if (opt.revalidateWorldClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.revalidateWorldClickTarget.Spawned)
				{
					return false;
				}
				Vector2 vector = opt.revalidateWorldClickTarget.ScreenPos();
				vector.y = (float)UI.screenHeight - vector.y;
				List<FloatMenuOption> list;
				if (vector == cachedChoicesForPos)
				{
					list = cachedChoices;
				}
				else
				{
					cachedChoices = FloatMenuMakerWorld.ChoicesAtFor(vector, forCaravan);
					cachedChoicesForPos = vector;
					list = cachedChoices;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, list[j]))
					{
						return !list[j].Disabled;
					}
				}
			}
			return false;
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x000E0DF0 File Offset: 0x000DEFF0
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
			if (!opt.Disabled && (caravan == null || !FloatMenuWorld.StillValid(opt, FloatMenuMakerWorld.ChoicesAtFor(this.clickPos, caravan), caravan)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x000E0214 File Offset: 0x000DE414
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x0400168D RID: 5773
		private Vector2 clickPos;

		// Token: 0x0400168E RID: 5774
		private const int RevalidateEveryFrame = 4;
	}
}

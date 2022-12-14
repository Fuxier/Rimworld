using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000484 RID: 1156
	public class FloatMenuMap : FloatMenu
	{
		// Token: 0x06002307 RID: 8967 RVA: 0x000DFF96 File Offset: 0x000DE196
		public FloatMenuMap(List<FloatMenuOption> options, string title, Vector3 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000DFFA8 File Offset: 0x000DE1A8
		public override void DoWindowContents(Rect inRect)
		{
			FloatMenuMap.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.selPawn = (Find.Selector.SingleSelectedThing as Pawn);
			if (CS$<>8__locals1.selPawn == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			bool flag = this.options.Count >= 3;
			if (Time.frameCount % 4 == 0 || this.lastOptionsForRevalidation == null)
			{
				this.lastOptionsForRevalidation = FloatMenuMakerMap.ChoicesAtFor(this.clickPos, CS$<>8__locals1.selPawn, false);
				FloatMenuMap.cachedChoices.Clear();
				FloatMenuMap.cachedChoices.Add(this.clickPos, this.lastOptionsForRevalidation);
				if (!flag)
				{
					for (int i = 0; i < this.options.Count; i++)
					{
						this.<DoWindowContents>g__RevalidateOption|6_0(this.options[i], ref CS$<>8__locals1);
					}
				}
			}
			else if (flag)
			{
				if (this.nextOptionToRevalidate >= this.options.Count)
				{
					this.nextOptionToRevalidate = 0;
				}
				int num = Mathf.CeilToInt((float)this.options.Count / 3f);
				int num2 = this.nextOptionToRevalidate;
				int num3 = 0;
				while (num2 < this.options.Count && num3 < num)
				{
					this.<DoWindowContents>g__RevalidateOption|6_0(this.options[num2], ref CS$<>8__locals1);
					this.nextOptionToRevalidate++;
					num2++;
					num3++;
				}
			}
			base.DoWindowContents(inRect);
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000E0108 File Offset: 0x000DE308
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Pawn forPawn)
		{
			if (opt.revalidateClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuMap.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.revalidateClickTarget.Spawned)
				{
					return false;
				}
				Vector3 key = opt.revalidateClickTarget.Position.ToVector3Shifted();
				List<FloatMenuOption> list;
				if (!FloatMenuMap.cachedChoices.TryGetValue(key, out list))
				{
					List<FloatMenuOption> list2 = FloatMenuMakerMap.ChoicesAtFor(key, forPawn, false);
					FloatMenuMap.cachedChoices.Add(key, list2);
					list = list2;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuMap.OptionsMatch(opt, list[j]))
					{
						return !list[j].Disabled;
					}
				}
			}
			return false;
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x000E01C8 File Offset: 0x000DE3C8
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (!opt.Disabled && (pawn == null || !FloatMenuMap.StillValid(opt, FloatMenuMakerMap.ChoicesAtFor(this.clickPos, pawn, false), pawn)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000E0214 File Offset: 0x000DE414
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x000E0238 File Offset: 0x000DE438
		[CompilerGenerated]
		private void <DoWindowContents>g__RevalidateOption|6_0(FloatMenuOption option, ref FloatMenuMap.<>c__DisplayClass6_0 A_2)
		{
			if (!option.Disabled && !FloatMenuMap.StillValid(option, this.lastOptionsForRevalidation, A_2.selPawn))
			{
				option.Disabled = true;
			}
		}

		// Token: 0x04001653 RID: 5715
		private Vector3 clickPos;

		// Token: 0x04001654 RID: 5716
		private static Dictionary<Vector3, List<FloatMenuOption>> cachedChoices = new Dictionary<Vector3, List<FloatMenuOption>>();

		// Token: 0x04001655 RID: 5717
		private List<FloatMenuOption> lastOptionsForRevalidation;

		// Token: 0x04001656 RID: 5718
		private int nextOptionToRevalidate;

		// Token: 0x04001657 RID: 5719
		public const int RevalidateEveryFrame = 4;
	}
}

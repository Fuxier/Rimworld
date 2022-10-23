using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004EE RID: 1262
	public class Dialog_NamePawn : Window
	{
		// Token: 0x0600268A RID: 9866 RVA: 0x000F73A0 File Offset: 0x000F55A0
		private Name BuildName()
		{
			if (this.pawn.Name is NameTriple)
			{
				string current = this.names[0].current;
				string first = (current != null) ? current.Trim() : null;
				string current2 = this.names[1].current;
				string nick = (current2 != null) ? current2.Trim() : null;
				string current3 = this.names[2].current;
				return new NameTriple(first, nick, (current3 != null) ? current3.Trim() : null);
			}
			if (this.pawn.Name is NameSingle)
			{
				string current4 = this.names[0].current;
				return new NameSingle((current4 != null) ? current4.Trim() : null, false);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x0600268B RID: 9867 RVA: 0x000F7458 File Offset: 0x000F5658
		// (set) Token: 0x0600268C RID: 9868 RVA: 0x000F74B4 File Offset: 0x000F56B4
		private string CurPawnNick
		{
			get
			{
				if (this.pawn.Name is NameTriple)
				{
					return this.names[1].current;
				}
				if (this.pawn.Name is NameSingle)
				{
					return this.names[0].current;
				}
				throw new InvalidOperationException();
			}
			set
			{
				int index;
				if (this.pawn.Name is NameTriple)
				{
					index = 1;
				}
				else
				{
					if (!(this.pawn.Name is NameSingle))
					{
						throw new InvalidOperationException();
					}
					index = 0;
				}
				this.names[index].current = value;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x0600268D RID: 9869 RVA: 0x000F7507 File Offset: 0x000F5707
		private string CurPawnTitle
		{
			get
			{
				if (!(this.names.Last<Dialog_NamePawn.NameContext>().textboxName == "Title"))
				{
					return null;
				}
				return this.names.Last<Dialog_NamePawn.NameContext>().current;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600268E RID: 9870 RVA: 0x000F7537 File Offset: 0x000F5737
		public override Vector2 InitialSize
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000F7540 File Offset: 0x000F5740
		public Dialog_NamePawn(Pawn pawn, NameFilter visibleNames, NameFilter editableNames, Dictionary<NameFilter, List<string>> suggestedNames, string initialFirstNameOverride = null, string initialNickNameOverride = null, string initialLastNameOverride = null, string initialTitleOverride = null)
		{
			this.pawn = pawn;
			if (pawn.RaceProps.Humanlike)
			{
				this.descriptionText = "{0}: {1}\n{2}: {3}".Formatted("Mother".Translate().CapitalizeFirst(), this.DescribePawn(pawn.GetMother()), "Father".Translate().CapitalizeFirst(), this.DescribePawn(pawn.GetFather()));
				this.renameText = "RenamePerson".Translate().CapitalizeFirst();
				this.portraitDirection = Rot4.South;
			}
			else
			{
				this.descriptionText = pawn.KindLabelIndefinite().CapitalizeFirst();
				this.renameText = ((ModsConfig.BiotechActive && pawn.RaceProps.IsMechanoid) ? "RenameMech" : "RenameAnimal").Translate().CapitalizeFirst();
				this.portraitDirection = Rot4.East;
				Vector3 extents = pawn.Drawer.renderer.graphics.nakedGraphic.MeshAt(this.portraitDirection).bounds.extents;
				float num = Math.Max(extents.x, extents.z);
				this.cameraZoom = 1f / num;
				this.portraitSize = Mathf.Min(128f, Mathf.Ceil(128f * num));
			}
			NameTriple nameTriple = pawn.Name as NameTriple;
			if (nameTriple != null && (visibleNames & NameFilter.First) > NameFilter.None)
			{
				this.names.Add(new Dialog_NamePawn.NameContext("FirstName", 0, initialFirstNameOverride ?? nameTriple.First, 12, (editableNames & NameFilter.First) > NameFilter.None, (suggestedNames != null) ? suggestedNames.GetWithFallback(NameFilter.First, null) : null));
			}
			if ((visibleNames & NameFilter.Nick) > NameFilter.None)
			{
				string text;
				if (nameTriple != null && !nameTriple.NickSet && (editableNames & NameFilter.Nick) > NameFilter.None)
				{
					text = "";
				}
				else
				{
					text = pawn.Name.ToStringShort;
				}
				this.names.Add(new Dialog_NamePawn.NameContext("NickName", 1, initialNickNameOverride ?? text, 16, (editableNames & NameFilter.Nick) > NameFilter.None, (suggestedNames != null) ? suggestedNames.GetWithFallback(NameFilter.Nick, null) : null));
			}
			if (nameTriple != null && (visibleNames & NameFilter.Last) > NameFilter.None)
			{
				this.names.Add(new Dialog_NamePawn.NameContext("LastName", 2, initialLastNameOverride ?? nameTriple.Last, 12, (editableNames & NameFilter.Last) > NameFilter.None, (suggestedNames != null) ? suggestedNames.GetWithFallback(NameFilter.Last, null) : null));
			}
			if (pawn.story != null && (visibleNames & NameFilter.Title) > NameFilter.None)
			{
				this.names.Add(new Dialog_NamePawn.NameContext("Title", -1, initialTitleOverride ?? (pawn.story.title ?? ""), 16, (editableNames & NameFilter.Title) > NameFilter.None, (suggestedNames != null) ? suggestedNames.GetWithFallback(NameFilter.Title, null) : null));
			}
			float num2 = this.names.Max((Dialog_NamePawn.NameContext name) => name.labelWidth);
			float num3 = this.names.Max((Dialog_NamePawn.NameContext name) => name.textboxWidth);
			foreach (Dialog_NamePawn.NameContext nameContext in this.names)
			{
				nameContext.labelWidth = num2;
				nameContext.textboxWidth = num3;
			}
			this.randomizeText = "Randomize".Translate().CapitalizeFirst();
			this.suggestedText = "Suggested".Translate().CapitalizeFirst() + "...";
			this.randomizeButtonWidth = Dialog_NamePawn.ButtonWidth(this.randomizeText.GetWidthCached());
			this.genderText = string.Format("{0}: {1}", "Gender".Translate().CapitalizeFirst(), pawn.GetGenderLabel().CapitalizeFirst());
			float x = 2f * this.Margin + num2 + num3 + this.randomizeButtonWidth + 34f;
			this.size = new Vector2(x, this.size.y);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this.closeOnAccept = false;
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x000F79E8 File Offset: 0x000F5BE8
		public override void DoWindowContents(Rect inRect)
		{
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			bool flag2 = false;
			bool forward = true;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
			{
				flag2 = true;
				forward = !Event.current.shift;
				Event.current.Use();
			}
			if (!this.firstCall && Event.current.type == EventType.Layout)
			{
				this.currentControl = GUI.GetNameOfFocusedControl();
			}
			RectAggregator rectAggregator = new RectAggregator(new Rect(inRect.x, inRect.y, inRect.width, 0f), 136098329, new Vector2?(new Vector2(17f, 4f)));
			if (this.renameHeight == null)
			{
				Text.Font = GameFont.Medium;
				this.renameHeight = new float?(Mathf.Ceil(this.renameText.RawText.GetHeightCached()));
				Text.Font = GameFont.Small;
			}
			this.descriptionHeight = new float?(this.descriptionHeight ?? Mathf.Ceil(Text.CalcHeight(this.descriptionText, rectAggregator.Rect.width - this.portraitSize - 17f)));
			float num = this.renameHeight.Value + 4f + this.descriptionHeight.Value;
			if (!this.pawn.RaceProps.Humanlike && this.portraitSize > num)
			{
				num = this.portraitSize;
			}
			RectDivider rectDivider = rectAggregator.NewRow(num, VerticalJustification.Bottom);
			Text.Font = GameFont.Medium;
			Pawn pawn = this.pawn;
			Vector2 vector = new Vector2(this.portraitSize, this.portraitSize);
			Rot4 rotation = this.portraitDirection;
			Vector3 cameraOffset = default(Vector3);
			PawnHealthState? healthStateOverride = new PawnHealthState?(PawnHealthState.Mobile);
			RenderTexture image = PortraitsCache.Get(pawn, vector, rotation, cameraOffset, this.cameraZoom, true, true, true, true, null, null, false, healthStateOverride);
			Rect position = rectDivider.NewCol(this.portraitSize, HorizontalJustification.Left);
			if (this.pawn.RaceProps.Humanlike)
			{
				position.y -= Mathf.Floor(18f);
			}
			position.height = this.portraitSize;
			GUI.DrawTexture(position, image);
			RectDivider rect = rectDivider.NewRow(this.renameHeight.Value, VerticalJustification.Top);
			Rect rect2 = rect.NewCol(this.renameHeight.Value, HorizontalJustification.Right);
			GUI.DrawTexture(rect2, this.pawn.gender.GetIcon());
			TooltipHandler.TipRegion(rect2, this.genderText);
			Widgets.Label(rect, this.renameText);
			Text.Font = GameFont.Small;
			Widgets.Label(rectDivider.NewRow(this.descriptionHeight.Value, VerticalJustification.Top), this.descriptionText);
			Text.Anchor = TextAnchor.MiddleLeft;
			foreach (Dialog_NamePawn.NameContext nameContext in this.names)
			{
				RectDivider rectDivider2 = rectAggregator.NewRow(30f, VerticalJustification.Bottom);
				nameContext.MakeRow(this.pawn, this.randomizeButtonWidth, this.randomizeText, this.suggestedText, ref rectDivider2, ref this.focusControlOverride);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			rectAggregator.NewRow(17.5f, VerticalJustification.Bottom);
			RectDivider rectDivider3 = rectAggregator.NewRow(35f, VerticalJustification.Bottom);
			float width = Mathf.Floor((rectDivider3.Rect.width - 17f) / 2f);
			if (Widgets.ButtonText(rectDivider3.NewCol(width, HorizontalJustification.Left), this.cancelText, true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(rectDivider3.NewCol(width, HorizontalJustification.Left), this.acceptText, true, true, true, null) || flag)
			{
				Name name = this.BuildName();
				if (!name.IsValid)
				{
					Messages.Message("NameInvalid".Translate(), this.pawn, MessageTypeDefOf.NeutralEvent, false);
				}
				else
				{
					this.pawn.Name = name;
					if (this.pawn.story != null)
					{
						this.pawn.story.Title = this.CurPawnTitle;
					}
					Find.WindowStack.TryRemove(this, true);
					string text;
					NameTriple nameTriple;
					if (this.pawn.def.race.Animal)
					{
						text = "AnimalGainsName".Translate(this.CurPawnNick);
					}
					else if (this.pawn.def.race.IsMechanoid)
					{
						text = "MechGainsName".Translate(this.CurPawnNick);
					}
					else if ((nameTriple = (name as NameTriple)) != null)
					{
						text = "PawnGainsName".Translate(nameTriple.Nick, this.pawn.story.Title, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
					}
					else
					{
						text = "PawnGainsName".Translate(this.CurPawnNick, this.pawn.story.Title, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
					}
					Messages.Message(text, this.pawn, MessageTypeDefOf.PositiveEvent, false);
					this.pawn.babyNamingDeadline = -1;
				}
			}
			this.size = new Vector2(this.size.x, Mathf.Ceil(this.size.y + (rectAggregator.Rect.height - inRect.height)));
			this.SetInitialSizeAndPosition();
			if (flag2 || this.firstCall)
			{
				this.FocusNextControl(this.currentControl, forward);
				this.firstCall = false;
			}
			if (Event.current.type == EventType.Layout && !string.IsNullOrEmpty(this.focusControlOverride))
			{
				GUI.FocusControl(this.focusControlOverride);
				this.focusControlOverride = null;
			}
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000F804C File Offset: 0x000F624C
		private void FocusNextControl(string currentControl, bool forward)
		{
			int num = this.names.FindIndex((Dialog_NamePawn.NameContext name) => name.textboxName == currentControl);
			int num2 = -1;
			if (forward)
			{
				for (int i = 1; i <= this.names.Count; i++)
				{
					int num3 = (num + i) % this.names.Count;
					if (this.names[num3].editable)
					{
						num2 = num3;
						break;
					}
				}
			}
			else
			{
				for (int j = 1; j <= this.names.Count; j++)
				{
					int num4 = (this.names.Count + num - j) % this.names.Count;
					if (this.names[num4].editable)
					{
						num2 = num4;
						break;
					}
				}
			}
			if (num2 >= 0)
			{
				this.focusControlOverride = this.names[num2].textboxName;
			}
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000F8134 File Offset: 0x000F6334
		private TaggedString DescribePawn(Pawn pawn)
		{
			if (pawn != null)
			{
				return pawn.FactionDesc(pawn.NameFullColored, false, pawn.NameFullColored, pawn.gender.GetLabel(pawn.RaceProps.Animal)).Resolve();
			}
			return "Unknown".Translate().Colorize(Color.gray);
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x000F8199 File Offset: 0x000F6399
		private static float ButtonWidth(float textWidth)
		{
			return Math.Max(114f, textWidth + 35f);
		}

		// Token: 0x0400191E RID: 6430
		private Pawn pawn;

		// Token: 0x0400191F RID: 6431
		private List<Dialog_NamePawn.NameContext> names = new List<Dialog_NamePawn.NameContext>(4);

		// Token: 0x04001920 RID: 6432
		private bool firstCall = true;

		// Token: 0x04001921 RID: 6433
		private string focusControlOverride;

		// Token: 0x04001922 RID: 6434
		private string currentControl;

		// Token: 0x04001923 RID: 6435
		private TaggedString descriptionText;

		// Token: 0x04001924 RID: 6436
		private float? descriptionHeight;

		// Token: 0x04001925 RID: 6437
		private float randomizeButtonWidth;

		// Token: 0x04001926 RID: 6438
		private Vector2 size = new Vector2(800f, 800f);

		// Token: 0x04001927 RID: 6439
		private float? renameHeight;

		// Token: 0x04001928 RID: 6440
		private Rot4 portraitDirection;

		// Token: 0x04001929 RID: 6441
		private float cameraZoom = 1f;

		// Token: 0x0400192A RID: 6442
		private float portraitSize = 128f;

		// Token: 0x0400192B RID: 6443
		private TaggedString cancelText = "Cancel".Translate().CapitalizeFirst();

		// Token: 0x0400192C RID: 6444
		private TaggedString acceptText = "Accept".Translate().CapitalizeFirst();

		// Token: 0x0400192D RID: 6445
		private TaggedString randomizeText;

		// Token: 0x0400192E RID: 6446
		private TaggedString suggestedText;

		// Token: 0x0400192F RID: 6447
		private TaggedString renameText;

		// Token: 0x04001930 RID: 6448
		private string genderText;

		// Token: 0x04001931 RID: 6449
		private const float ButtonHeight = 35f;

		// Token: 0x04001932 RID: 6450
		private const float NameFieldsHeight = 30f;

		// Token: 0x04001933 RID: 6451
		private const int MaximumNumberOfNames = 4;

		// Token: 0x04001934 RID: 6452
		private const float VerticalMargin = 4f;

		// Token: 0x04001935 RID: 6453
		private const float HorizontalMargin = 17f;

		// Token: 0x04001936 RID: 6454
		private const float PortraitSize = 128f;

		// Token: 0x04001937 RID: 6455
		private const float HeaderRowShrinkAmount = 36f;

		// Token: 0x04001938 RID: 6456
		private const int ContextHash = 136098329;

		// Token: 0x020020D9 RID: 8409
		private class NameContext
		{
			// Token: 0x0600C53F RID: 50495 RVA: 0x0043B7C8 File Offset: 0x004399C8
			public NameContext(string label, int nameIndex, string currentName, int maximumNameLength, bool editable, List<string> suggestedNames)
			{
				this.current = currentName;
				this.nameIndex = nameIndex;
				this.label = label.Translate().CapitalizeFirst() + ":";
				this.labelWidth = Mathf.Ceil(this.label.GetWidthCached());
				this.maximumNameLength = maximumNameLength;
				this.textboxWidth = Mathf.Ceil(Text.CalcSize(new string('W', maximumNameLength + 2)).x);
				this.textboxName = label;
				this.editable = editable;
				this.suggestedNames = suggestedNames;
				if (suggestedNames != null)
				{
					this.suggestedOptions = new List<FloatMenuOption>(suggestedNames.Count);
					using (List<string>.Enumerator enumerator = suggestedNames.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string suggestedName = enumerator.Current;
							this.suggestedOptions.Add(new FloatMenuOption(suggestedName, delegate()
							{
								this.current = suggestedName;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
					}
				}
			}

			// Token: 0x0600C540 RID: 50496 RVA: 0x0043B8F0 File Offset: 0x00439AF0
			public void MakeRow(Pawn pawn, float randomizeButtonWidth, TaggedString randomizeText, TaggedString suggestedText, ref RectDivider divider, ref string focusControlOverride)
			{
				Widgets.Label(divider.NewCol(this.labelWidth, HorizontalJustification.Left), this.label);
				RectDivider rect = divider.NewCol(this.textboxWidth, HorizontalJustification.Left);
				if (this.editable)
				{
					GUI.SetNextControlName(this.textboxName);
					CharacterCardUtility.DoNameInputRect(rect, ref this.current, this.maximumNameLength);
				}
				else
				{
					Widgets.Label(rect, this.current);
				}
				if (this.editable && this.nameIndex >= 0)
				{
					Rect rect2 = divider.NewCol(randomizeButtonWidth, HorizontalJustification.Left);
					if (this.suggestedNames != null)
					{
						List<string> list = this.suggestedNames;
						if (list != null && list.Count > 0 && Widgets.ButtonText(rect2, suggestedText, true, true, true, null))
						{
							Find.WindowStack.Add(new FloatMenu(this.suggestedOptions));
							return;
						}
					}
					else if (Widgets.ButtonText(rect2, randomizeText, true, true, true, null))
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						NameStyle style = NameStyle.Full;
						string forcedLastName = null;
						bool forceNoNick = false;
						Pawn_GeneTracker genes = pawn.genes;
						Name name = PawnBioAndNameGenerator.GeneratePawnName(pawn, style, forcedLastName, forceNoNick, (genes != null) ? genes.Xenotype : null);
						NameTriple nameTriple;
						if ((nameTriple = (name as NameTriple)) != null)
						{
							this.current = nameTriple[this.nameIndex];
							return;
						}
						NameSingle nameSingle;
						if ((nameSingle = (name as NameSingle)) != null)
						{
							this.current = nameSingle.Name;
						}
					}
				}
			}

			// Token: 0x04008266 RID: 33382
			public string current;

			// Token: 0x04008267 RID: 33383
			public TaggedString label;

			// Token: 0x04008268 RID: 33384
			public float labelWidth;

			// Token: 0x04008269 RID: 33385
			public int maximumNameLength;

			// Token: 0x0400826A RID: 33386
			public float textboxWidth;

			// Token: 0x0400826B RID: 33387
			public string textboxName;

			// Token: 0x0400826C RID: 33388
			public bool editable;

			// Token: 0x0400826D RID: 33389
			public int nameIndex;

			// Token: 0x0400826E RID: 33390
			public List<string> suggestedNames;

			// Token: 0x0400826F RID: 33391
			private List<FloatMenuOption> suggestedOptions;
		}
	}
}

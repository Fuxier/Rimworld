using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004A5 RID: 1189
	public sealed class LetterStack : IExposable
	{
		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x060023BC RID: 9148 RVA: 0x000E4D51 File Offset: 0x000E2F51
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x060023BD RID: 9149 RVA: 0x000E4D59 File Offset: 0x000E2F59
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x060023BE RID: 9150 RVA: 0x000E4D61 File Offset: 0x000E2F61
		public BundleLetter BundleLetter
		{
			get
			{
				if (this.bundleLetterCache == null)
				{
					this.bundleLetterCache = (BundleLetter)LetterMaker.MakeLetter(LetterDefOf.BundleLetter);
				}
				return this.bundleLetterCache;
			}
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x000E4D88 File Offset: 0x000E2F88
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction, quest, hyperlinkThingDefs);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x000E4DB0 File Offset: 0x000E2FB0
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, null, null);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x000E4DD4 File Offset: 0x000E2FD4
		public void ReceiveLetter(Letter let, string debugInfo = null)
		{
			if (!let.CanShowInLetterStack)
			{
				return;
			}
			let.def.arriveSound.PlayOneShotOnCamera(null);
			if (Prefs.AutomaticPauseMode >= let.def.pauseMode)
			{
				Find.TickManager.Pause();
			}
			else if (let.def.forcedSlowdown)
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			let.arrivalTime = Time.time;
			let.arrivalTick = Find.TickManager.TicksGame;
			let.debugInfo = debugInfo;
			this.letters.Add(let);
			Find.Archive.Add(let);
			let.Received();
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000E4E75 File Offset: 0x000E3075
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000E4E8C File Offset: 0x000E308C
		public void LettersOnGUI(float baseY)
		{
			float num = baseY;
			float num2 = baseY - Find.Alerts.AlertsHeight;
			float num3 = 42f;
			int num4 = Mathf.FloorToInt(num2 / num3);
			int num5 = Math.Max(this.letters.Count - num4, 0);
			if (num5 > 0)
			{
				num5++;
			}
			for (int i = this.letters.Count - 1; i >= num5; i--)
			{
				num -= 30f;
				this.letters[i].DrawButtonAt(num);
				num -= 12f;
			}
			if (num5 > 0)
			{
				this.tmpBundledLetters.Clear();
				this.tmpBundledLetters.AddRange(this.letters.Take(num5));
				num -= 30f;
				this.BundleLetter.SetLetters(this.tmpBundledLetters);
				this.BundleLetter.DrawButtonAt(num);
				num -= 12f;
				this.tmpBundledLetters.Clear();
			}
			this.lastTopYInt = num;
			if (Event.current.type == EventType.Repaint)
			{
				num = baseY;
				for (int j = this.letters.Count - 1; j >= num5; j--)
				{
					num -= 30f;
					this.letters[j].CheckForMouseOverTextAt(num);
					num -= 12f;
				}
				if (num5 > 0)
				{
					num -= 30f;
					this.BundleLetter.CheckForMouseOverTextAt(num);
					num -= 12f;
				}
			}
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x000E4FE4 File Offset: 0x000E31E4
		public void OpenAutomaticLetters()
		{
			if (!Find.WindowStack.WindowsForcePause)
			{
				Letter letter2 = this.letters.FirstOrFallback((Letter letter) => letter.ShouldAutomaticallyOpenLetter, null);
				if (letter2 == null)
				{
					return;
				}
				letter2.OpenLetter();
			}
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x000E5032 File Offset: 0x000E3232
		public void LetterStackTick()
		{
			this.OpenAutomaticLetters();
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x000E503C File Offset: 0x000E323C
		public void LetterStackUpdate()
		{
			if (this.mouseoverLetterIndex >= 0 && this.mouseoverLetterIndex < this.letters.Count)
			{
				this.letters[this.mouseoverLetterIndex].lookTargets.TryHighlight(true, true, false);
			}
			this.mouseoverLetterIndex = -1;
			for (int i = this.letters.Count - 1; i >= 0; i--)
			{
				if (!this.letters[i].CanShowInLetterStack)
				{
					this.RemoveLetter(this.letters[i]);
				}
			}
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x000E50C7 File Offset: 0x000E32C7
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x000E50DC File Offset: 0x000E32DC
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.letters.Count; i++)
			{
				if (this.letters[i].relatedFaction == faction)
				{
					this.letters[i].relatedFaction = null;
				}
			}
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x000E5128 File Offset: 0x000E3328
		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.letters.RemoveAll((Letter x) => x == null);
			}
		}

		// Token: 0x04001719 RID: 5913
		private List<Letter> letters = new List<Letter>();

		// Token: 0x0400171A RID: 5914
		private int mouseoverLetterIndex = -1;

		// Token: 0x0400171B RID: 5915
		private float lastTopYInt;

		// Token: 0x0400171C RID: 5916
		private BundleLetter bundleLetterCache;

		// Token: 0x0400171D RID: 5917
		private const float LettersBottomY = 350f;

		// Token: 0x0400171E RID: 5918
		public const float LetterSpacing = 12f;

		// Token: 0x0400171F RID: 5919
		private List<Letter> tmpBundledLetters = new List<Letter>();
	}
}

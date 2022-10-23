using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000138 RID: 312
	public class SoundDef : Def
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x000286C8 File Offset: 0x000268C8
		private bool HasSubSoundsOnCamera
		{
			get
			{
				for (int i = 0; i < this.subSounds.Count; i++)
				{
					if (this.subSounds[i].onCamera)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00028704 File Offset: 0x00026904
		public bool HasSubSoundsInWorld
		{
			get
			{
				for (int i = 0; i < this.subSounds.Count; i++)
				{
					if (!this.subSounds[i].onCamera)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x0002873D File Offset: 0x0002693D
		public int MaxSimultaneousSamples
		{
			get
			{
				return this.maxSimultaneous * this.subSounds.Count;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00028754 File Offset: 0x00026954
		public FloatRange Duration
		{
			get
			{
				float num = float.PositiveInfinity;
				float num2 = float.NegativeInfinity;
				foreach (SubSoundDef subSoundDef in this.subSounds)
				{
					num = Mathf.Min(num, subSoundDef.Duration.min);
					num2 = Mathf.Max(num2, subSoundDef.Duration.max);
				}
				return new FloatRange((num == float.PositiveInfinity) ? 0f : num, (num2 == float.NegativeInfinity) ? 0f : num2);
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x000287F8 File Offset: 0x000269F8
		public override void ResolveReferences()
		{
			for (int i = 0; i < this.subSounds.Count; i++)
			{
				this.subSounds[i].parentDef = this;
				this.subSounds[i].ResolveReferences();
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0002883E File Offset: 0x00026A3E
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.slot != "" && !this.HasSubSoundsOnCamera)
			{
				yield return "Sound slots only work for on-camera sounds.";
			}
			if (this.HasSubSoundsInWorld && this.context != SoundContext.MapOnly)
			{
				yield return "Sounds with non-on-camera subsounds should use MapOnly context.";
			}
			if (this.priorityMode == VoicePriorityMode.PrioritizeNewest && this.sustain)
			{
				yield return "PrioritizeNewest is not supported with sustainers.";
			}
			if (this.maxVoices < 1)
			{
				yield return "Max voices is less than 1.";
			}
			if (!this.sustain && (this.sustainStartSound != null || this.sustainStopSound != null))
			{
				yield return "Sustainer start and end sounds only work with sounds defined as sustainers.";
			}
			if (this.sustainFadeoutStartSound != null && this.sustainFadeoutTime <= 0f)
			{
				yield return "Sustainer fadeout sound is set, but fadeout time is not set.";
			}
			int num;
			if (!this.sustain)
			{
				for (int i = 0; i < this.subSounds.Count; i = num + 1)
				{
					if (this.subSounds[i].startDelayRange.TrueMax > 0.001f)
					{
						yield return "startDelayRange is only supported on sustainers.";
					}
					num = i;
				}
			}
			List<SoundDef> defs = DefDatabase<SoundDef>.AllDefsListForReading;
			for (int i = 0; i < defs.Count; i = num + 1)
			{
				if (!defs[i].eventNames.NullOrEmpty<string>())
				{
					for (int j = 0; j < defs[i].eventNames.Count; j = num + 1)
					{
						if (defs[i].eventNames[j] == this.defName)
						{
							yield return this.defName + " is also defined in the eventNames list for " + defs[i];
						}
						num = j;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00028850 File Offset: 0x00026A50
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (this.testSustainer == null)
			{
				if (widgetRow.ButtonIcon(TexButton.Play, null, null, null, null, true, -1f))
				{
					this.ResolveReferences();
					SoundInfo info;
					if (this.HasSubSoundsInWorld)
					{
						IntVec3 mapPosition = Find.CameraDriver.MapPosition;
						info = SoundInfo.InMap(new TargetInfo(mapPosition, Find.CurrentMap, false), MaintenanceType.PerFrame);
						for (int i = 0; i < 5; i++)
						{
							FleckMaker.ThrowDustPuff(mapPosition, Find.CurrentMap, 1.5f);
						}
					}
					else
					{
						info = SoundInfo.OnCamera(MaintenanceType.PerFrame);
					}
					info.testPlay = true;
					if (this.sustain)
					{
						this.testSustainer = this.TrySpawnSustainer(info);
						return;
					}
					this.PlayOneShot(info);
					return;
				}
			}
			else
			{
				this.testSustainer.Maintain();
				if (widgetRow.ButtonIcon(TexButton.Stop, null, null, null, null, true, -1f))
				{
					this.testSustainer.End();
					this.testSustainer = null;
				}
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00028960 File Offset: 0x00026B60
		public static SoundDef Named(string defName)
		{
			SoundDef namedSilentFail = DefDatabase<SoundDef>.GetNamedSilentFail(defName);
			if (namedSilentFail != null)
			{
				return namedSilentFail;
			}
			if (!Prefs.DevMode)
			{
				object obj = SoundDef.undefinedSoundDefsLock;
				lock (obj)
				{
					if (SoundDef.undefinedSoundDefs.ContainsKey(defName))
					{
						return SoundDef.UndefinedDefNamed(defName);
					}
				}
			}
			List<SoundDef> allDefsListForReading = DefDatabase<SoundDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].eventNames.Count > 0)
				{
					for (int j = 0; j < allDefsListForReading[i].eventNames.Count; j++)
					{
						if (allDefsListForReading[i].eventNames[j] == defName)
						{
							return allDefsListForReading[i];
						}
					}
				}
			}
			if (DefDatabase<SoundDef>.DefCount == 0)
			{
				Log.Warning("Tried to get SoundDef named " + defName + ", but sound defs aren't loaded yet (is it a static variable initialized before play data?).");
			}
			return SoundDef.UndefinedDefNamed(defName);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00028A64 File Offset: 0x00026C64
		private static SoundDef UndefinedDefNamed(string defName)
		{
			object obj = SoundDef.undefinedSoundDefsLock;
			SoundDef soundDef;
			lock (obj)
			{
				if (!SoundDef.undefinedSoundDefs.TryGetValue(defName, out soundDef))
				{
					soundDef = new SoundDef();
					soundDef.isUndefined = true;
					soundDef.defName = defName;
					SoundDef.undefinedSoundDefs.Add(defName, soundDef);
				}
			}
			return soundDef;
		}

		// Token: 0x04000813 RID: 2067
		[Description("If checked, this sound is a sustainer.\n\nSustainers are used for sounds with a defined beginning and end (as opposed to OneShots, which just fire at a given instant).\n\nThis value must match what the game expects from the SubSoundDef with this name.")]
		[DefaultValue(false)]
		public bool sustain;

		// Token: 0x04000814 RID: 2068
		[Description("When the sound is allowed to play: only when the map view is active, only when the world view is active, or always (map + world + main menu).")]
		[DefaultValue(SoundContext.Any)]
		public SoundContext context;

		// Token: 0x04000815 RID: 2069
		[Description("Event names for this sound. \n\nThe code will look up sounds to play them according to their name. If the code finds the event name it wants in this list, it will trigger this sound.\n\nThe Def name is also used as an event name. Obsolete")]
		public List<string> eventNames = new List<string>();

		// Token: 0x04000816 RID: 2070
		[Description("For one-shots, this is the number of individual sounds from this Def than can be playing at a time.\n\n For sustainers, this is the number of sustainers that can be running with this sound (each of which can have sub-sounds). Sustainers can fade in and out as you move the camera or objects move, to keep the nearest ones audible.\n\nThis setting may not work for on-camera sounds.")]
		[DefaultValue(4)]
		public int maxVoices = 4;

		// Token: 0x04000817 RID: 2071
		[Description("The number of instances of this sound that can play at almost exactly the same moment. Handles cases like six gunners all firing their identical guns at the same time because a target came into view of all of them at the same time. Ordinarily this would make a painfully loud sound, but you can reduce it with this.")]
		[DefaultValue(3)]
		public int maxSimultaneous = 3;

		// Token: 0x04000818 RID: 2072
		[Description("If the system has to not play some instances of this sound because of maxVoices, this determines which ones are ignored.\n\nYou should use PrioritizeNewest for things like gunshots, so older still-playing samples are overridden by newer, more important ones.\n\nSustained sounds should usually prioritize nearest, so if a new fire starts burning nearby it can override a more distant one.")]
		[DefaultValue(VoicePriorityMode.PrioritizeNewest)]
		public VoicePriorityMode priorityMode;

		// Token: 0x04000819 RID: 2073
		[Description("The special sound slot this sound takes. If a sound with this slot is playing, new sounds in this slot will not play.\n\nOnly works for on-camera sounds.")]
		[DefaultValue("")]
		public string slot = "";

		// Token: 0x0400081A RID: 2074
		[LoadAlias("sustainerStartSound")]
		[Description("The name of the SoundDef that will be played when this sustainer starts.")]
		[DefaultValue("")]
		public SoundDef sustainStartSound;

		// Token: 0x0400081B RID: 2075
		[LoadAlias("sustainerStopSound")]
		[Description("The name of the SoundDef that will be played when this sustainer ends.")]
		[DefaultValue("")]
		public SoundDef sustainStopSound;

		// Token: 0x0400081C RID: 2076
		[Description("After a sustainer is ended, the sound will fade out over this many real-time seconds.")]
		[DefaultValue(0f)]
		public float sustainFadeoutTime;

		// Token: 0x0400081D RID: 2077
		[LoadAlias("sustainerFadeoutStartSound")]
		[Description("The name of the SoundDef that will be played when this sustainer starts to fade out.")]
		[DefaultValue("")]
		public SoundDef sustainFadeoutStartSound;

		// Token: 0x0400081E RID: 2078
		[Description("All the sounds that will play when this set is triggered.")]
		public List<SubSoundDef> subSounds = new List<SubSoundDef>();

		// Token: 0x0400081F RID: 2079
		[Unsaved(false)]
		public bool isUndefined;

		// Token: 0x04000820 RID: 2080
		[Unsaved(false)]
		public Sustainer testSustainer;

		// Token: 0x04000821 RID: 2081
		private static Dictionary<string, SoundDef> undefinedSoundDefs = new Dictionary<string, SoundDef>();

		// Token: 0x04000822 RID: 2082
		private static object undefinedSoundDefsLock = new object();
	}
}

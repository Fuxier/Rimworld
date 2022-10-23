using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020003BD RID: 957
	public static class Scribe_TargetInfo
	{
		// Token: 0x06001B2A RID: 6954 RVA: 0x000A6F85 File Offset: 0x000A5185
		public static void Look(ref LocalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, LocalTargetInfo.Invalid, false);
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x000A6F95 File Offset: 0x000A5195
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, LocalTargetInfo.Invalid, true);
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x000A6FA5 File Offset: 0x000A51A5
		public static void Look(ref LocalTargetInfo value, string label, LocalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x000A6FB4 File Offset: 0x000A51B4
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label, LocalTargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.LocalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveLocalTargetInfo(value, label);
				}
			}
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x000A7048 File Offset: 0x000A5248
		public static void Look(ref TargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, TargetInfo.Invalid, false);
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000A7058 File Offset: 0x000A5258
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, TargetInfo.Invalid, true);
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x000A7068 File Offset: 0x000A5268
		public static void Look(ref TargetInfo value, string label, TargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x000A7074 File Offset: 0x000A5274
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label, TargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					if (!value.HasThing && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.TargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveTargetInfo(value, label);
				}
			}
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x000A7151 File Offset: 0x000A5351
		public static void Look(ref GlobalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, GlobalTargetInfo.Invalid, false);
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x000A7161 File Offset: 0x000A5361
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, GlobalTargetInfo.Invalid, true);
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x000A7171 File Offset: 0x000A5371
		public static void Look(ref GlobalTargetInfo value, string label, GlobalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000A7180 File Offset: 0x000A5380
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label, GlobalTargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					if (value.WorldObject != null && Scribe_References.CheckSaveReferenceToDestroyedWorldObject(value.WorldObject, label, saveDestroyedThings))
					{
						return;
					}
					if (!value.HasThing && !value.HasWorldObject && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.GlobalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveGlobalTargetInfo(value, label);
				}
			}
		}
	}
}

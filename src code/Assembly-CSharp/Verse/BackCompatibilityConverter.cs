using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200050A RID: 1290
	public abstract class BackCompatibilityConverter
	{
		// Token: 0x06002777 RID: 10103
		public abstract bool AppliesToVersion(int majorVer, int minorVer);

		// Token: 0x06002778 RID: 10104
		public abstract string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null);

		// Token: 0x06002779 RID: 10105
		public abstract Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node);

		// Token: 0x0600277A RID: 10106 RVA: 0x000FD4BF File Offset: 0x000FB6BF
		public virtual int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			return index;
		}

		// Token: 0x0600277B RID: 10107
		public abstract void PostExposeData(object obj);

		// Token: 0x0600277C RID: 10108 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PostCouldntLoadDef(string defName)
		{
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PreLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void PostLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000FD4C2 File Offset: 0x000FB6C2
		public bool AppliesToLoadedGameVersion(bool allowInactiveScribe = false)
		{
			return !ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty() && (allowInactiveScribe || Scribe.mode != LoadSaveMode.Inactive) && this.AppliesToVersion(ScribeMetaHeaderUtility.loadedGameVersionMajor, ScribeMetaHeaderUtility.loadedGameVersionMinor);
		}
	}
}

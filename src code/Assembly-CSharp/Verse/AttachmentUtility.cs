using System;

namespace Verse
{
	// Token: 0x02000412 RID: 1042
	public static class AttachmentUtility
	{
		// Token: 0x06001E9D RID: 7837 RVA: 0x000B724C File Offset: 0x000B544C
		public static Thing GetAttachment(this Thing t, ThingDef def)
		{
			CompAttachBase compAttachBase = t.TryGetComp<CompAttachBase>();
			if (compAttachBase == null)
			{
				return null;
			}
			return compAttachBase.GetAttachment(def);
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x000B726C File Offset: 0x000B546C
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}

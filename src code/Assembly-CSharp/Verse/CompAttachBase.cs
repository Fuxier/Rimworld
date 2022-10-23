using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000411 RID: 1041
	public class CompAttachBase : ThingComp
	{
		// Token: 0x06001E95 RID: 7829 RVA: 0x000B70D8 File Offset: 0x000B52D8
		public override void CompTick()
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					this.attachments[i].Position = this.parent.Position;
				}
			}
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x000B7120 File Offset: 0x000B5320
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.attachments != null)
			{
				for (int i = this.attachments.Count - 1; i >= 0; i--)
				{
					this.attachments[i].Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x000B7168 File Offset: 0x000B5368
		public override string CompInspectStringExtra()
		{
			if (this.attachments != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.attachments.Count; i++)
				{
					stringBuilder.AppendLine(this.attachments[i].InspectStringAddon);
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
			return null;
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x000B71C0 File Offset: 0x000B53C0
		public Thing GetAttachment(ThingDef def)
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					if (this.attachments[i].def == def)
					{
						return this.attachments[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x000B720D File Offset: 0x000B540D
		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x000B7219 File Offset: 0x000B5419
		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x000B723A File Offset: 0x000B543A
		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}

		// Token: 0x040014E5 RID: 5349
		public List<AttachableThing> attachments;
	}
}

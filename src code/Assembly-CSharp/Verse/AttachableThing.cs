using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C1 RID: 961
	public abstract class AttachableThing : Thing
	{
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001B40 RID: 6976 RVA: 0x000A79ED File Offset: 0x000A5BED
		public override Vector3 DrawPos
		{
			get
			{
				if (this.parent != null)
				{
					return this.parent.DrawPos + Vector3.up * 0.04054054f * 0.9f;
				}
				return base.DrawPos;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001B41 RID: 6977
		public abstract string InspectStringAddon { get; }

		// Token: 0x06001B42 RID: 6978 RVA: 0x000A7A27 File Offset: 0x000A5C27
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.parent != null)
			{
				this.AttachTo(this.parent);
			}
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x000A7A5C File Offset: 0x000A5C5C
		public virtual void AttachTo(Thing parent)
		{
			this.parent = parent;
			CompAttachBase compAttachBase = parent.TryGetComp<CompAttachBase>();
			if (compAttachBase == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot attach ",
					this,
					" to ",
					parent,
					": parent has no CompAttachBase."
				}));
				return;
			}
			compAttachBase.AddAttachment(this);
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x000A7AB2 File Offset: 0x000A5CB2
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.parent != null)
			{
				this.parent.TryGetComp<CompAttachBase>().RemoveAttachment(this);
			}
		}

		// Token: 0x040013C9 RID: 5065
		public Thing parent;
	}
}

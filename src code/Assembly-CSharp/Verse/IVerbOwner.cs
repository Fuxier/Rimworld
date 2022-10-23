using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005A6 RID: 1446
	public interface IVerbOwner
	{
		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002C12 RID: 11282
		VerbTracker VerbTracker { get; }

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002C13 RID: 11283
		List<VerbProperties> VerbProperties { get; }

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002C14 RID: 11284
		List<Tool> Tools { get; }

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002C15 RID: 11285
		ImplementOwnerTypeDef ImplementOwnerTypeDef { get; }

		// Token: 0x06002C16 RID: 11286
		string UniqueVerbOwnerID();

		// Token: 0x06002C17 RID: 11287
		bool VerbsStillUsableBy(Pawn p);

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002C18 RID: 11288
		Thing ConstantCaster { get; }
	}
}

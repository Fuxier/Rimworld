using System;

// Token: 0x02000006 RID: 6
public class JobDriver_BreastfeedCarryToDownedMom : JobDriver_BreastfeedCarryToMom
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600001F RID: 31 RVA: 0x00002662 File Offset: 0x00000862
	protected override bool MomMustBeImmobile
	{
		get
		{
			return true;
		}
	}
}

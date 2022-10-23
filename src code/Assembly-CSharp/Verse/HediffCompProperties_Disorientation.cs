using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002DC RID: 732
	public class HediffCompProperties_Disorientation : HediffCompProperties
	{
		// Token: 0x060014C1 RID: 5313 RVA: 0x0007DF6E File Offset: 0x0007C16E
		public HediffCompProperties_Disorientation()
		{
			this.compClass = typeof(HediffComp_Disorientation);
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0007DF98 File Offset: 0x0007C198
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.wanderMtbHours <= 0f)
			{
				yield return "wanderMtbHours must be greater than zero";
			}
			if (this.singleWanderDurationTicks <= 0)
			{
				yield return "singleWanderDurationTicks must be greater than zero";
			}
			if (this.wanderRadius <= 0f)
			{
				yield return "wanderRadius must be greater than zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x040010D2 RID: 4306
		public float wanderMtbHours = -1f;

		// Token: 0x040010D3 RID: 4307
		public float wanderRadius;

		// Token: 0x040010D4 RID: 4308
		public int singleWanderDurationTicks = -1;
	}
}

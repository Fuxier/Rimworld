using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200032B RID: 811
	public class HediffCompProperties_VerbGiver : HediffCompProperties
	{
		// Token: 0x060015B4 RID: 5556 RVA: 0x00081689 File Offset: 0x0007F889
		public HediffCompProperties_VerbGiver()
		{
			this.compClass = typeof(HediffComp_VerbGiver);
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x000816A4 File Offset: 0x0007F8A4
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x000816ED File Offset: 0x0007F8ED
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.tools != null)
			{
				Tool tool = this.tools.SelectMany((Tool lhs) => from rhs in this.tools
				where lhs != rhs && lhs.id == rhs.id
				select rhs).FirstOrDefault<Tool>();
				if (tool != null)
				{
					yield return string.Format("duplicate hediff tool id {0}", tool.id);
				}
				foreach (Tool tool2 in this.tools)
				{
					foreach (string text2 in tool2.ConfigErrors())
					{
						yield return text2;
					}
					enumerator = null;
				}
				List<Tool>.Enumerator enumerator2 = default(List<Tool>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04001163 RID: 4451
		public List<VerbProperties> verbs;

		// Token: 0x04001164 RID: 4452
		public List<Tool> tools;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000287 RID: 647
	public class PatchOperation
	{
		// Token: 0x060012D6 RID: 4822 RVA: 0x0006DAEC File Offset: 0x0006BCEC
		public bool Apply(XmlDocument xml)
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start(base.GetType().FullName + " Worker");
			}
			bool flag = this.ApplyWorker(xml);
			if (DeepProfiler.enabled)
			{
				DeepProfiler.End();
			}
			if (this.success == PatchOperation.Success.Always)
			{
				flag = true;
			}
			else if (this.success == PatchOperation.Success.Never)
			{
				flag = false;
			}
			else if (this.success == PatchOperation.Success.Invert)
			{
				flag = !flag;
			}
			if (flag)
			{
				this.neverSucceeded = false;
			}
			return flag;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0006DB65 File Offset: 0x0006BD65
		protected virtual bool ApplyWorker(XmlDocument xml)
		{
			Log.Error("Attempted to use PatchOperation directly; patch will always fail");
			return false;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0006DB74 File Offset: 0x0006BD74
		public virtual void Complete(string modIdentifier)
		{
			if (this.neverSucceeded)
			{
				string text = string.Format("[{0}] Patch operation {1} failed", modIdentifier, this);
				if (!string.IsNullOrEmpty(this.sourceFile))
				{
					text = text + "\nfile: " + this.sourceFile;
				}
				Log.Error(text);
			}
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00019CDC File Offset: 0x00017EDC
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x04000F8F RID: 3983
		public string sourceFile;

		// Token: 0x04000F90 RID: 3984
		private bool neverSucceeded = true;

		// Token: 0x04000F91 RID: 3985
		private PatchOperation.Success success;

		// Token: 0x02001DD8 RID: 7640
		private enum Success
		{
			// Token: 0x040075D4 RID: 30164
			Normal,
			// Token: 0x040075D5 RID: 30165
			Invert,
			// Token: 0x040075D6 RID: 30166
			Always,
			// Token: 0x040075D7 RID: 30167
			Never
		}
	}
}

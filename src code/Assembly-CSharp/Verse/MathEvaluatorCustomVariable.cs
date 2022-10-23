using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000052 RID: 82
	public class MathEvaluatorCustomVariable : IXsltContextVariable
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0000249D File Offset: 0x0000069D
		public bool IsLocal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0000249D File Offset: 0x0000069D
		public bool IsParam
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00015E8B File Offset: 0x0001408B
		public XPathResultType VariableType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00015E8E File Offset: 0x0001408E
		public MathEvaluatorCustomVariable(string prefix, string name)
		{
			this.prefix = prefix;
			this.name = name;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00015EA4 File Offset: 0x000140A4
		public object Evaluate(XsltContext xsltContext)
		{
			return ((MathEvaluatorCustomContext)xsltContext).ArgList.GetParam(this.name, this.prefix);
		}

		// Token: 0x0400011E RID: 286
		private string prefix;

		// Token: 0x0400011F RID: 287
		private string name;
	}
}

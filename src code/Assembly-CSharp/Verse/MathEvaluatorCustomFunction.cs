using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000051 RID: 81
	public class MathEvaluatorCustomFunction : IXsltContextFunction
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00015E40 File Offset: 0x00014040
		public XPathResultType[] ArgTypes
		{
			get
			{
				return this.argTypes;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00015E48 File Offset: 0x00014048
		public int Maxargs
		{
			get
			{
				return this.functionType.maxArgs;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00015E55 File Offset: 0x00014055
		public int Minargs
		{
			get
			{
				return this.functionType.minArgs;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0000249D File Offset: 0x0000069D
		public XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00015E62 File Offset: 0x00014062
		public MathEvaluatorCustomFunction(MathEvaluatorCustomFunctions.FunctionType functionType, XPathResultType[] argTypes)
		{
			this.functionType = functionType;
			this.argTypes = argTypes;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00015E78 File Offset: 0x00014078
		public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return this.functionType.func(args);
		}

		// Token: 0x0400011C RID: 284
		private XPathResultType[] argTypes;

		// Token: 0x0400011D RID: 285
		private MathEvaluatorCustomFunctions.FunctionType functionType;
	}
}

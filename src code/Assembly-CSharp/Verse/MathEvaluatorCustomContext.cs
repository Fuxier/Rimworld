using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200004F RID: 79
	public class MathEvaluatorCustomContext : XsltContext
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x00002662 File Offset: 0x00000862
		public override bool Whitespace
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0001591D File Offset: 0x00013B1D
		public XsltArgumentList ArgList
		{
			get
			{
				return this.argList;
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00015925 File Offset: 0x00013B25
		public MathEvaluatorCustomContext()
		{
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001592D File Offset: 0x00013B2D
		public MathEvaluatorCustomContext(NameTable nt, XsltArgumentList args) : base(nt)
		{
			this.argList = args;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00015940 File Offset: 0x00013B40
		public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes)
		{
			MathEvaluatorCustomFunctions.FunctionType[] functionTypes = MathEvaluatorCustomFunctions.FunctionTypes;
			for (int i = 0; i < functionTypes.Length; i++)
			{
				if (functionTypes[i].name == name)
				{
					return new MathEvaluatorCustomFunction(functionTypes[i], argTypes);
				}
			}
			return null;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001597C File Offset: 0x00013B7C
		public override IXsltContextVariable ResolveVariable(string prefix, string name)
		{
			if (this.ArgList.GetParam(name, prefix) != null)
			{
				return new MathEvaluatorCustomVariable(prefix, name);
			}
			return null;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000249D File Offset: 0x0000069D
		public override bool PreserveWhitespace(XPathNavigator node)
		{
			return false;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000249D File Offset: 0x0000069D
		public override int CompareDocument(string baseUri, string nextbaseUri)
		{
			return 0;
		}

		// Token: 0x0400011A RID: 282
		private XsltArgumentList argList;
	}
}

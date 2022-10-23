using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200004E RID: 78
	public static class MathEvaluator
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x000157AF File Offset: 0x000139AF
		private static XPathNavigator Navigator
		{
			get
			{
				if (MathEvaluator.doc == null)
				{
					MathEvaluator.doc = new XPathDocument(new StringReader("<root />"));
				}
				if (MathEvaluator.navigator == null)
				{
					MathEvaluator.navigator = MathEvaluator.doc.CreateNavigator();
				}
				return MathEvaluator.navigator;
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000157E8 File Offset: 0x000139E8
		public static double Evaluate(string expr)
		{
			if (expr.NullOrEmpty())
			{
				return 0.0;
			}
			expr = MathEvaluator.AddSpacesRegex.Replace(expr, " ${1} ");
			expr = expr.Replace("/", " div ");
			expr = expr.Replace("%", " mod ");
			double result;
			try
			{
				XPathExpression xpathExpression = XPathExpression.Compile("number(" + expr + ")");
				xpathExpression.SetContext(MathEvaluator.Context);
				double num = (double)MathEvaluator.Navigator.Evaluate(xpathExpression);
				if (double.IsNaN(num))
				{
					Log.ErrorOnce("Expression \"" + expr + "\" evaluated to NaN.", expr.GetHashCode() ^ 48337162);
					num = 0.0;
				}
				result = num;
			}
			catch (XPathException ex)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Could not evaluate expression \"",
					expr,
					"\". Error: ",
					ex
				}), expr.GetHashCode() ^ 980986121);
				result = 0.0;
			}
			return result;
		}

		// Token: 0x04000116 RID: 278
		private static XPathDocument doc;

		// Token: 0x04000117 RID: 279
		private static XPathNavigator navigator;

		// Token: 0x04000118 RID: 280
		private static readonly Regex AddSpacesRegex = new Regex("([\\+\\-\\*])");

		// Token: 0x04000119 RID: 281
		private static readonly MathEvaluatorCustomContext Context = new MathEvaluatorCustomContext(new NameTable(), new XsltArgumentList());
	}
}

using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x0200039B RID: 923
	public static class DirectXmlSaveFormatter
	{
		// Token: 0x06001A73 RID: 6771 RVA: 0x0009F7D0 File Offset: 0x0009D9D0
		public static void AddWhitespaceFromRoot(XElement root)
		{
			if (!root.Elements().Any<XElement>())
			{
				return;
			}
			foreach (XNode xnode in root.Elements().ToList<XElement>())
			{
				XText content = new XText("\n");
				xnode.AddAfterSelf(content);
			}
			root.Elements().First<XElement>().AddBeforeSelf(new XText("\n"));
			root.Elements().Last<XElement>().AddAfterSelf(new XText("\n"));
			foreach (XElement element in root.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element, 1);
			}
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x0009F8B8 File Offset: 0x0009DAB8
		private static void IndentXml(XElement element, int depth)
		{
			element.AddBeforeSelf(new XText(DirectXmlSaveFormatter.IndentString(depth, true)));
			bool startWithNewline = element.NextNode == null;
			element.AddAfterSelf(new XText(DirectXmlSaveFormatter.IndentString(depth - 1, startWithNewline)));
			foreach (XElement element2 in element.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element2, depth + 1);
			}
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x0009F940 File Offset: 0x0009DB40
		private static string IndentString(int depth, bool startWithNewline)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (startWithNewline)
			{
				stringBuilder.Append("\n");
			}
			for (int i = 0; i < depth; i++)
			{
				stringBuilder.Append("  ");
			}
			return stringBuilder.ToString();
		}
	}
}

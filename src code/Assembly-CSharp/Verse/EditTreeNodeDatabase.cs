using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004AD RID: 1197
	public static class EditTreeNodeDatabase
	{
		// Token: 0x0600240B RID: 9227 RVA: 0x000E5FBC File Offset: 0x000E41BC
		public static TreeNode_Editor RootOf(object obj)
		{
			for (int i = 0; i < EditTreeNodeDatabase.roots.Count; i++)
			{
				if (EditTreeNodeDatabase.roots[i].obj == obj)
				{
					return EditTreeNodeDatabase.roots[i];
				}
			}
			TreeNode_Editor treeNode_Editor = TreeNode_Editor.NewRootNode(obj);
			EditTreeNodeDatabase.roots.Add(treeNode_Editor);
			return treeNode_Editor;
		}

		// Token: 0x04001735 RID: 5941
		private static List<TreeNode_Editor> roots = new List<TreeNode_Editor>();
	}
}

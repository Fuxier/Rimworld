using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000456 RID: 1110
	public abstract class DebugTabMenu
	{
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002233 RID: 8755 RVA: 0x000DA0F4 File Offset: 0x000D82F4
		public int Count
		{
			get
			{
				return this.VisibleActions.Count;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002234 RID: 8756 RVA: 0x000DA104 File Offset: 0x000D8304
		protected List<DebugActionNode> VisibleActions
		{
			get
			{
				if (this.cachedVisibleActions == null)
				{
					this.cachedVisibleActions = new List<DebugActionNode>();
					List<DebugActionNode> children = this.dialog.CurrentNode.children;
					for (int i = 0; i < children.Count; i++)
					{
						DebugActionNode debugActionNode = children[i];
						if (debugActionNode.VisibleNow)
						{
							this.cachedVisibleActions.Add(debugActionNode);
						}
					}
				}
				return this.cachedVisibleActions;
			}
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000DA168 File Offset: 0x000D8368
		public DebugTabMenu(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode rootNode)
		{
			this.def = def;
			this.dialog = dialog;
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000DA17E File Offset: 0x000D837E
		public void Enter(DebugActionNode root)
		{
			this.myRoot = root;
			this.myRoot.Enter(this.dialog);
		}

		// Token: 0x06002237 RID: 8759
		public abstract DebugActionNode InitActions(DebugActionNode root);

		// Token: 0x06002238 RID: 8760 RVA: 0x000DA198 File Offset: 0x000D8398
		public int HighlightedIndex(int currentHighlightIndex, int prioritizedHighlightedIndex)
		{
			List<DebugActionNode> visibleActions = this.VisibleActions;
			if (visibleActions.NullOrEmpty<DebugActionNode>() || prioritizedHighlightedIndex >= visibleActions.Count)
			{
				return -1;
			}
			if (this.dialog.FilterAllows(visibleActions[prioritizedHighlightedIndex].LabelNow))
			{
				return prioritizedHighlightedIndex;
			}
			if (this.dialog.Filter.NullOrEmpty())
			{
				return 0;
			}
			for (int i = 0; i < visibleActions.Count; i++)
			{
				if (this.dialog.FilterAllows(visibleActions[i].LabelNow))
				{
					currentHighlightIndex = i;
					break;
				}
			}
			return currentHighlightIndex;
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000DA21E File Offset: 0x000D841E
		public string LabelAtIndex(int index)
		{
			return this.VisibleActions[index].LabelNow;
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x000DA234 File Offset: 0x000D8434
		public void ListOptions(int highlightedIndex)
		{
			string b = null;
			List<DebugActionNode> visibleActions = this.VisibleActions;
			for (int i = 0; i < visibleActions.Count; i++)
			{
				DebugActionNode debugActionNode = visibleActions[i];
				if (this.dialog.FilterAllows(debugActionNode.LabelNow))
				{
					if (debugActionNode.category != b)
					{
						this.dialog.DoGap(7f);
						this.dialog.DoLabel(debugActionNode.category);
						b = debugActionNode.category;
					}
					Log.openOnMessage = true;
					try
					{
						this.dialog.DrawNode(debugActionNode, highlightedIndex == i);
					}
					finally
					{
						Log.openOnMessage = false;
					}
				}
			}
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000DA2DC File Offset: 0x000D84DC
		public void Recache()
		{
			this.cachedVisibleActions = null;
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000DA2E5 File Offset: 0x000D84E5
		public void OnAcceptKeyPressed(int index)
		{
			if (index < 0)
			{
				return;
			}
			this.VisibleActions[index].Enter(this.dialog);
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x000DA303 File Offset: 0x000D8503
		public static DebugTabMenu CreateMenu(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root)
		{
			return (DebugTabMenu)Activator.CreateInstance(def.menuClass, new object[]
			{
				def,
				dialog,
				root
			});
		}

		// Token: 0x040015C1 RID: 5569
		protected DebugActionNode myRoot;

		// Token: 0x040015C2 RID: 5570
		public readonly DebugTabMenuDef def;

		// Token: 0x040015C3 RID: 5571
		protected readonly Dialog_Debug dialog;

		// Token: 0x040015C4 RID: 5572
		private List<DebugActionNode> cachedVisibleActions;
	}
}

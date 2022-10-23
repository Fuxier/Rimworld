using System;
using System.Collections.Generic;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000457 RID: 1111
	public class DebugTabMenu_Actions : DebugTabMenu
	{
		// Token: 0x0600223E RID: 8766 RVA: 0x000DA327 File Offset: 0x000D8527
		public DebugTabMenu_Actions(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root) : base(def, dialog, root)
		{
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x000DA334 File Offset: 0x000D8534
		public override DebugActionNode InitActions(DebugActionNode absRoot)
		{
			this.myRoot = new DebugActionNode("Actions", DebugActionType.Action, null, null);
			absRoot.AddChild(this.myRoot);
			this.moreActionsNode = new DebugActionNode("Show more actions", DebugActionType.Action, null, null)
			{
				category = "More debug actions",
				displayPriority = -999999
			};
			this.myRoot.AddChild(this.moreActionsNode);
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					DebugActionAttribute attribute;
					if (methodInfo.TryGetAttribute(out attribute))
					{
						this.GenerateCacheForMethod(methodInfo, attribute);
					}
					DebugActionYielderAttribute debugActionYielderAttribute;
					if (methodInfo.TryGetAttribute(out debugActionYielderAttribute))
					{
						foreach (DebugActionNode child in ((IEnumerable<DebugActionNode>)methodInfo.Invoke(null, null)))
						{
							this.myRoot.AddChild(child);
						}
					}
				}
			}
			this.myRoot.TrySort();
			return this.myRoot;
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000DA474 File Offset: 0x000D8674
		private void GenerateCacheForMethod(MethodInfo method, DebugActionAttribute attribute)
		{
			string text = string.IsNullOrEmpty(attribute.name) ? GenText.SplitCamelCase(method.Name) : attribute.name;
			if (attribute.actionType == DebugActionType.ToolMap || attribute.actionType == DebugActionType.ToolMapForPawns || attribute.actionType == DebugActionType.ToolWorld)
			{
				text = "T: " + text;
			}
			Type returnType = method.ReturnType;
			DebugActionNode debugActionNode;
			if (returnType.Equals(typeof(List<DebugActionNode>)))
			{
				debugActionNode = new DebugActionNode();
				debugActionNode.childGetter = (Delegate.CreateDelegate(typeof(Func<List<DebugActionNode>>), method) as Func<List<DebugActionNode>>);
				if (!text.EndsWith("..."))
				{
					text += "...";
				}
			}
			else if (returnType.Equals(typeof(DebugActionNode)))
			{
				debugActionNode = (DebugActionNode)method.Invoke(null, null);
				if (debugActionNode.children.Any<DebugActionNode>() && !text.EndsWith("..."))
				{
					text += "...";
				}
			}
			else
			{
				debugActionNode = new DebugActionNode();
				if (attribute.actionType == DebugActionType.ToolMapForPawns)
				{
					debugActionNode.pawnAction = (Delegate.CreateDelegate(typeof(Action<Pawn>), method) as Action<Pawn>);
				}
				else
				{
					debugActionNode.action = (Delegate.CreateDelegate(typeof(Action), method) as Action);
				}
			}
			if (debugActionNode.label.NullOrEmpty())
			{
				debugActionNode.label = text;
			}
			debugActionNode.label = debugActionNode.label.Replace("\\", "/");
			debugActionNode.category = (attribute.category ?? "General");
			debugActionNode.actionType = attribute.actionType;
			debugActionNode.displayPriority = attribute.displayPriority;
			debugActionNode.sourceAttribute = attribute;
			if (attribute.hideInSubMenu)
			{
				this.moreActionsNode.AddChild(debugActionNode);
				return;
			}
			this.myRoot.AddChild(debugActionNode);
		}

		// Token: 0x040015C5 RID: 5573
		private DebugActionNode moreActionsNode;
	}
}

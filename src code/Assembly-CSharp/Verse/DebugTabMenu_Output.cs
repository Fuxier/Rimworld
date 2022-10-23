using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000459 RID: 1113
	public class DebugTabMenu_Output : DebugTabMenu
	{
		// Token: 0x06002244 RID: 8772 RVA: 0x000DA327 File Offset: 0x000D8527
		public DebugTabMenu_Output(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root) : base(def, dialog, root)
		{
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x000DA674 File Offset: 0x000D8874
		public override DebugActionNode InitActions(DebugActionNode absRoot)
		{
			this.myRoot = new DebugActionNode("Outputs", DebugActionType.Action, null, null);
			absRoot.AddChild(this.myRoot);
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					DebugOutputAttribute attribute;
					if (methodInfo.TryGetAttribute(out attribute))
					{
						this.GenerateCacheForMethod(methodInfo, attribute);
					}
				}
			}
			this.myRoot.TrySort();
			return this.myRoot;
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000DA71C File Offset: 0x000D891C
		private void GenerateCacheForMethod(MethodInfo method, DebugOutputAttribute attribute)
		{
			string label = attribute.name ?? GenText.SplitCamelCase(method.Name);
			Action action = Delegate.CreateDelegate(typeof(Action), method) as Action;
			DebugActionNode debugActionNode = new DebugActionNode(label, DebugActionType.Action, action, null);
			debugActionNode.category = (attribute.category ?? "General");
			debugActionNode.visibilityGetter = (() => !attribute.onlyWhenPlaying || Current.ProgramState == ProgramState.Playing);
			this.myRoot.AddChild(debugActionNode);
		}

		// Token: 0x040015C9 RID: 5577
		public const string DefaultCategory = "General";
	}
}

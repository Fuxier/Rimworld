using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x0200045A RID: 1114
	public class DebugTabMenu_Settings : DebugTabMenu
	{
		// Token: 0x06002247 RID: 8775 RVA: 0x000DA327 File Offset: 0x000D8527
		public DebugTabMenu_Settings(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root) : base(def, dialog, root)
		{
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000DA7A8 File Offset: 0x000D89A8
		public override DebugActionNode InitActions(DebugActionNode absRoot)
		{
			this.myRoot = new DebugActionNode("Settings", DebugActionType.Action, null, null);
			absRoot.AddChild(this.myRoot);
			foreach (FieldInfo fi in typeof(DebugSettings).GetFields())
			{
				this.AddNode(fi, "General");
			}
			foreach (FieldInfo fi2 in typeof(DebugViewSettings).GetFields())
			{
				this.AddNode(fi2, "View");
			}
			return this.myRoot;
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x000DA838 File Offset: 0x000D8A38
		private void AddNode(FieldInfo fi, string categoryLabel)
		{
			if (fi.IsLiteral)
			{
				return;
			}
			DebugActionNode debugActionNode = new DebugActionNode(this.LegibleFieldName(fi), DebugActionType.Action, delegate()
			{
				bool flag = (bool)fi.GetValue(null);
				fi.SetValue(null, !flag);
				MethodInfo method = fi.DeclaringType.GetMethod(fi.Name + "Toggled", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}, null);
			debugActionNode.category = categoryLabel;
			debugActionNode.settingsField = fi;
			this.myRoot.AddChild(debugActionNode);
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x000DA89F File Offset: 0x000D8A9F
		private string LegibleFieldName(FieldInfo fi)
		{
			return GenText.SplitCamelCase(fi.Name).CapitalizeFirst();
		}
	}
}

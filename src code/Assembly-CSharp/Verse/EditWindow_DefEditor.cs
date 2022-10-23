using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047A RID: 1146
	internal class EditWindow_DefEditor : EditWindow
	{
		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060022B9 RID: 8889 RVA: 0x000B870C File Offset: 0x000B690C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x00002662 File Offset: 0x00000862
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x000DDD47 File Offset: 0x000DBF47
		public EditWindow_DefEditor(Def def)
		{
			this.def = def;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.optionalTitle = def.ToString();
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000DDD7C File Offset: 0x000DBF7C
		public override void DoWindowContents(Rect inRect)
		{
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Escape))
			{
				UI.UnfocusCurrentControl();
			}
			Rect rect = new Rect(0f, 0f, inRect.width, 16f);
			this.labelColumnWidth = Widgets.HorizontalSlider(rect, this.labelColumnWidth, 0f, inRect.width, false, null, null, null, -1f);
			Rect outRect = inRect.AtZero();
			outRect.yMin += 16f;
			Rect rect2 = new Rect(0f, 0f, outRect.width - 16f, this.viewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Listing_TreeDefs listing_TreeDefs = new Listing_TreeDefs(this.labelColumnWidth);
			listing_TreeDefs.Begin(rect2);
			TreeNode_Editor node = EditTreeNodeDatabase.RootOf(this.def);
			listing_TreeDefs.ContentLines(node, 0);
			listing_TreeDefs.End();
			if (Event.current.type == EventType.Layout)
			{
				this.viewHeight = listing_TreeDefs.CurHeight + 200f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0400160C RID: 5644
		public Def def;

		// Token: 0x0400160D RID: 5645
		private float viewHeight;

		// Token: 0x0400160E RID: 5646
		private Vector2 scrollPosition;

		// Token: 0x0400160F RID: 5647
		private float labelColumnWidth = 140f;

		// Token: 0x04001610 RID: 5648
		private const float TopAreaHeight = 16f;

		// Token: 0x04001611 RID: 5649
		private const float ExtraScrollHeight = 200f;
	}
}

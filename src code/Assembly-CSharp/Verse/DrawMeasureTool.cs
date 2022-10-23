using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000467 RID: 1127
	public class DrawMeasureTool
	{
		// Token: 0x060022A0 RID: 8864 RVA: 0x000DDB03 File Offset: 0x000DBD03
		public DrawMeasureTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000DDB20 File Offset: 0x000DBD20
		public DrawMeasureTool(string label, Action clickAction, Vector3 firstRectCorner)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = delegate()
			{
				Vector3 v = UI.MouseMapPosition();
				Vector2 start = firstRectCorner.MapToUIPosition();
				Vector2 end = v.MapToUIPosition();
				Widgets.DrawLine(start, end, Color.white, 0.25f);
			};
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x000DDB60 File Offset: 0x000DBD60
		public void DebugToolOnGUI()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					this.clickAction();
				}
				if (Event.current.button == 1)
				{
					DebugTools.curMeasureTool = null;
				}
				Event.current.Use();
			}
			Vector2 vector = Event.current.mousePosition + new Vector2(15f, 15f);
			Rect rect = new Rect(vector.x, vector.y, 999f, 999f);
			Text.Font = GameFont.Small;
			Widgets.Label(rect, this.label);
			if (this.onGUIAction != null)
			{
				this.onGUIAction();
			}
		}

		// Token: 0x04001601 RID: 5633
		private string label;

		// Token: 0x04001602 RID: 5634
		private Action clickAction;

		// Token: 0x04001603 RID: 5635
		private Action onGUIAction;
	}
}

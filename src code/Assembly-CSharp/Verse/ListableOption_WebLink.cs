using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004DC RID: 1244
	public class ListableOption_WebLink : ListableOption
	{
		// Token: 0x06002598 RID: 9624 RVA: 0x000EEE3E File Offset: 0x000ED03E
		public ListableOption_WebLink(string label, Texture2D image) : base(label, null, null)
		{
			this.minHeight = 24f;
			this.image = image;
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000EEE5B File Offset: 0x000ED05B
		public ListableOption_WebLink(string label, string url, Texture2D image) : this(label, image)
		{
			this.url = url;
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x000EEE6C File Offset: 0x000ED06C
		public ListableOption_WebLink(string label, Action action, Texture2D image) : this(label, image)
		{
			this.action = action;
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x000EEE80 File Offset: 0x000ED080
		public override float DrawOption(Vector2 pos, float width)
		{
			float num = width - ListableOption_WebLink.Imagesize.x - 3f;
			float num2 = Text.CalcHeight(this.label, num);
			float num3 = Mathf.Max(this.minHeight, num2);
			Rect rect = new Rect(pos.x, pos.y, width, num3);
			GUI.color = Color.white;
			if (this.image != null)
			{
				Rect position = new Rect(pos.x, pos.y + num3 / 2f - ListableOption_WebLink.Imagesize.y / 2f, ListableOption_WebLink.Imagesize.x, ListableOption_WebLink.Imagesize.y);
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
				GUI.DrawTexture(position, this.image);
			}
			Widgets.Label(new Rect(rect.xMax - num, pos.y, num, num2), this.label);
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.action != null)
				{
					this.action();
				}
				else
				{
					Application.OpenURL(this.url);
				}
			}
			return num3;
		}

		// Token: 0x04001812 RID: 6162
		public Texture2D image;

		// Token: 0x04001813 RID: 6163
		public string url;

		// Token: 0x04001814 RID: 6164
		private static readonly Vector2 Imagesize = new Vector2(24f, 18f);
	}
}

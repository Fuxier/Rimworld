using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200049B RID: 1179
	public class Command_Toggle : Command
	{
		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002394 RID: 9108 RVA: 0x000E3CE4 File Offset: 0x000E1EE4
		public override SoundDef CurActivateSound
		{
			get
			{
				if (this.isActive())
				{
					return this.turnOffSound;
				}
				return this.turnOnSound;
			}
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x000E3D00 File Offset: 0x000E1F00
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x000E3D14 File Offset: 0x000E1F14
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth, parms);
			Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
			Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
			Texture2D image = this.isActive() ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x000E3D94 File Offset: 0x000E1F94
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}

		// Token: 0x040016E0 RID: 5856
		public Func<bool> isActive;

		// Token: 0x040016E1 RID: 5857
		public Action toggleAction;

		// Token: 0x040016E2 RID: 5858
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		// Token: 0x040016E3 RID: 5859
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;

		// Token: 0x040016E4 RID: 5860
		public bool activateIfAmbiguous = true;
	}
}

using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200042E RID: 1070
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugActionAttribute : Attribute
	{
		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x000BB728 File Offset: 0x000B9928
		public bool IsAllowedInCurrentGameState
		{
			get
			{
				bool flag = (this.allowedGameStates & AllowedGameStates.Entry) == AllowedGameStates.Invalid || Current.ProgramState == ProgramState.Entry;
				bool flag2 = (this.allowedGameStates & AllowedGameStates.Playing) == AllowedGameStates.Invalid || Current.ProgramState == ProgramState.Playing;
				bool flag3 = (this.allowedGameStates & AllowedGameStates.WorldRenderedNow) == AllowedGameStates.Invalid || WorldRendererUtility.WorldRenderedNow;
				bool flag4 = (this.allowedGameStates & AllowedGameStates.IsCurrentlyOnMap) == AllowedGameStates.Invalid || (!WorldRendererUtility.WorldRenderedNow && Find.CurrentMap != null);
				bool flag5 = (this.allowedGameStates & AllowedGameStates.HasGameCondition) == AllowedGameStates.Invalid || (!WorldRendererUtility.WorldRenderedNow && Find.CurrentMap != null && Find.CurrentMap.gameConditionManager.ActiveConditions.Count > 0);
				return flag && flag2 && flag3 && flag4 && flag5 && (!this.requiresRoyalty || ModsConfig.RoyaltyActive) && (!this.requiresIdeology || ModsConfig.IdeologyActive) && (!this.requiresBiotech || ModsConfig.BiotechActive);
			}
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x000BB804 File Offset: 0x000B9A04
		public DebugActionAttribute(string category = null, string name = null, bool requiresRoyalty = false, bool requiresIdeology = false, bool requiresBiotech = false, int displayPriority = 0, bool hideInSubMenu = false)
		{
			this.name = name;
			this.requiresRoyalty = requiresRoyalty;
			this.requiresIdeology = requiresIdeology;
			this.requiresBiotech = requiresBiotech;
			this.displayPriority = displayPriority;
			this.hideInSubMenu = hideInSubMenu;
			if (!string.IsNullOrEmpty(category))
			{
				this.category = category;
			}
		}

		// Token: 0x0400156C RID: 5484
		public string name;

		// Token: 0x0400156D RID: 5485
		public string category = "General";

		// Token: 0x0400156E RID: 5486
		public AllowedGameStates allowedGameStates = AllowedGameStates.Playing;

		// Token: 0x0400156F RID: 5487
		public DebugActionType actionType;

		// Token: 0x04001570 RID: 5488
		public bool requiresRoyalty;

		// Token: 0x04001571 RID: 5489
		public bool requiresIdeology;

		// Token: 0x04001572 RID: 5490
		public bool requiresBiotech;

		// Token: 0x04001573 RID: 5491
		public int displayPriority;

		// Token: 0x04001574 RID: 5492
		public bool hideInSubMenu;
	}
}

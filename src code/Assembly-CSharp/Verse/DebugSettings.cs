using System;

namespace Verse
{
	// Token: 0x0200053E RID: 1342
	public static class DebugSettings
	{
		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06002943 RID: 10563 RVA: 0x00107EA0 File Offset: 0x001060A0
		public static bool ShowDevGizmos
		{
			get
			{
				return Prefs.DevMode && DebugSettings.godMode;
			}
		}

		// Token: 0x04001AC4 RID: 6852
		public const bool DebugBuild = false;

		// Token: 0x04001AC5 RID: 6853
		public static bool enableDamage = true;

		// Token: 0x04001AC6 RID: 6854
		public static bool enablePlayerDamage = true;

		// Token: 0x04001AC7 RID: 6855
		public static bool enableRandomMentalStates = true;

		// Token: 0x04001AC8 RID: 6856
		public static bool enableStoryteller = true;

		// Token: 0x04001AC9 RID: 6857
		public static bool enableRandomDiseases = true;

		// Token: 0x04001ACA RID: 6858
		public static bool godMode = false;

		// Token: 0x04001ACB RID: 6859
		public static bool devPalette = false;

		// Token: 0x04001ACC RID: 6860
		public static bool noAnimals = false;

		// Token: 0x04001ACD RID: 6861
		public static bool unlimitedPower = false;

		// Token: 0x04001ACE RID: 6862
		public static bool pathThroughWalls = false;

		// Token: 0x04001ACF RID: 6863
		public static bool instantRecruit = false;

		// Token: 0x04001AD0 RID: 6864
		public static bool alwaysSocialFight = false;

		// Token: 0x04001AD1 RID: 6865
		public static bool alwaysDoLovin = false;

		// Token: 0x04001AD2 RID: 6866
		public static bool detectRegionListersBugs = false;

		// Token: 0x04001AD3 RID: 6867
		public static bool instantVisitorsGift = false;

		// Token: 0x04001AD4 RID: 6868
		public static bool lowFPS = false;

		// Token: 0x04001AD5 RID: 6869
		public static bool allowUndraftedMechOrders = false;

		// Token: 0x04001AD6 RID: 6870
		public static bool editableGlowerColors = false;

		// Token: 0x04001AD7 RID: 6871
		public static bool fastResearch = false;

		// Token: 0x04001AD8 RID: 6872
		public static bool fastLearning = false;

		// Token: 0x04001AD9 RID: 6873
		public static bool fastEcology = false;

		// Token: 0x04001ADA RID: 6874
		public static bool fastEcologyRegrowRateOnly = false;

		// Token: 0x04001ADB RID: 6875
		public static bool fastCrafting = false;

		// Token: 0x04001ADC RID: 6876
		public static bool fastCaravans = false;

		// Token: 0x04001ADD RID: 6877
		public static bool fastMapUnpollution = false;

		// Token: 0x04001ADE RID: 6878
		public static bool activateAllBuildingDemands = false;

		// Token: 0x04001ADF RID: 6879
		public static bool activateAllIdeoRoles = false;

		// Token: 0x04001AE0 RID: 6880
		public static bool showLocomotionUrgency = false;

		// Token: 0x04001AE1 RID: 6881
		public static bool playRitualAmbience = true;

		// Token: 0x04001AE2 RID: 6882
		public static bool simulateUsingSteamDeck = false;

		// Token: 0x04001AE3 RID: 6883
		public static bool logRaidInfo = false;

		// Token: 0x04001AE4 RID: 6884
		public static bool logTranslationLookupErrors = false;
	}
}

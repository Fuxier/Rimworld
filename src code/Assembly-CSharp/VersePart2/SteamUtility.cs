using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020005A0 RID: 1440
	public static class SteamUtility
	{
		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002BD9 RID: 11225 RVA: 0x00116BB4 File Offset: 0x00114DB4
		public static string SteamPersonaName
		{
			get
			{
				if (SteamManager.Initialized && SteamUtility.cachedPersonaName == null)
				{
					SteamUtility.cachedPersonaName = SteamFriends.GetPersonaName();
				}
				if (SteamUtility.cachedPersonaName == null)
				{
					return "???";
				}
				return SteamUtility.cachedPersonaName;
			}
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x00116BE0 File Offset: 0x00114DE0
		public static void OpenUrl(string url)
		{
			if (SteamManager.Initialized && SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
				return;
			}
			Application.OpenURL(url);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x00116BFE File Offset: 0x00114DFE
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x00116C0B File Offset: 0x00114E0B
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("http://steamcommunity.com/workshop/browse/?appid=" + SteamUtils.GetAppID());
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x00116C26 File Offset: 0x00114E26
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			return "steam://url/CommunityFilePage/" + pfid;
		}

		// Token: 0x04001CE1 RID: 7393
		private static string cachedPersonaName;
	}
}

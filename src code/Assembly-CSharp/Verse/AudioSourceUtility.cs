using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C0 RID: 960
	public static class AudioSourceUtility
	{
		// Token: 0x06001B3E RID: 6974 RVA: 0x000A7878 File Offset: 0x000A5A78
		public static float GetSanitizedVolume(float volume, object debugInfo)
		{
			if (float.IsNegativeInfinity(volume))
			{
				Log.ErrorOnce("Volume was negative infinity (" + debugInfo + ")", 863653423);
				return 0f;
			}
			if (float.IsPositiveInfinity(volume))
			{
				Log.ErrorOnce("Volume was positive infinity (" + debugInfo + ")", 954354323);
				return 1f;
			}
			if (float.IsNaN(volume))
			{
				Log.ErrorOnce("Volume was NaN (" + debugInfo + ")", 231846572);
				return 1f;
			}
			return Mathf.Clamp(volume, 0f, 1000f);
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x000A7910 File Offset: 0x000A5B10
		public static float GetSanitizedPitch(float pitch, object debugInfo)
		{
			if (float.IsNegativeInfinity(pitch))
			{
				Log.ErrorOnce("Pitch was negative infinity (" + debugInfo + ")", 546475990);
				return 0.0001f;
			}
			if (float.IsPositiveInfinity(pitch))
			{
				Log.ErrorOnce("Pitch was positive infinity (" + debugInfo + ")", 309856435);
				return 1f;
			}
			if (float.IsNaN(pitch))
			{
				Log.ErrorOnce("Pitch was NaN (" + debugInfo + ")", 800635427);
				return 1f;
			}
			if (pitch < 0f)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Pitch was negative ",
					pitch,
					" (",
					debugInfo,
					")"
				}), 384765707);
				return 0.0001f;
			}
			return Mathf.Clamp(pitch, 0.0001f, 1000f);
		}
	}
}

using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000522 RID: 1314
	public static class DataExposeUtility
	{
		// Token: 0x060027ED RID: 10221 RVA: 0x00103D2C File Offset: 0x00101F2C
		public static void ByteArray(ref byte[] arr, string label)
		{
			if (Scribe.mode == LoadSaveMode.Saving && arr != null)
			{
				byte[] array = CompressUtility.Compress(arr);
				if (array.Length < arr.Length)
				{
					string text = DataExposeUtility.AddLineBreaksToLongString(Convert.ToBase64String(array));
					Scribe_Values.Look<string>(ref text, label + "Deflate", null, false);
				}
				else
				{
					string text2 = DataExposeUtility.AddLineBreaksToLongString(Convert.ToBase64String(arr));
					Scribe_Values.Look<string>(ref text2, label, null, false);
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				string text3 = null;
				Scribe_Values.Look<string>(ref text3, label + "Deflate", null, false);
				if (text3 != null)
				{
					arr = CompressUtility.Decompress(Convert.FromBase64String(DataExposeUtility.RemoveLineBreaks(text3)));
					return;
				}
				Scribe_Values.Look<string>(ref text3, label, null, false);
				if (text3 != null)
				{
					arr = Convert.FromBase64String(DataExposeUtility.RemoveLineBreaks(text3));
					return;
				}
				arr = null;
			}
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x00103DE4 File Offset: 0x00101FE4
		public static void BoolArray(ref bool[] arr, int elements, string label)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (arr.Length != elements)
				{
					Log.ErrorOnce(string.Format("Bool array length mismatch for {0}", label), 74135877);
				}
				elements = arr.Length;
			}
			int num = (elements + 7) / 8;
			byte[] array = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				array = new byte[num];
				int num2 = 0;
				byte b = 1;
				for (int i = 0; i < elements; i++)
				{
					if (arr[i])
					{
						byte[] array2 = array;
						int num3 = num2;
						array2[num3] |= b;
					}
					b *= 2;
					if (b == 0)
					{
						b = 1;
						num2++;
					}
				}
			}
			DataExposeUtility.ByteArray(ref array, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (arr == null)
				{
					arr = new bool[elements];
				}
				if (array != null && array.Length != 0)
				{
					if (array.Length != num)
					{
						int num4 = 0;
						byte b2 = 1;
						for (int j = 0; j < elements; j++)
						{
							arr[j] = ((array[num4] & b2) > 0);
							b2 *= 2;
							if (b2 > 32)
							{
								b2 = 1;
								num4++;
							}
						}
						return;
					}
					int num5 = 0;
					byte b3 = 1;
					for (int k = 0; k < elements; k++)
					{
						arr[k] = ((array[num5] & b3) > 0);
						b3 *= 2;
						if (b3 == 0)
						{
							b3 = 1;
							num5++;
						}
					}
				}
			}
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x00103F0C File Offset: 0x0010210C
		public static string AddLineBreaksToLongString(string str)
		{
			StringBuilder stringBuilder = new StringBuilder(str.Length + (str.Length / 100 + 3) * 2 + 1);
			stringBuilder.AppendLine();
			for (int i = 0; i < str.Length; i++)
			{
				if (i % 100 == 0 && i != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(str[i]);
			}
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x00103F78 File Offset: 0x00102178
		public static string RemoveLineBreaks(string str)
		{
			return str.Replace("\n", "").Replace("\r", "");
		}

		// Token: 0x04001A6B RID: 6763
		private const int NewlineInterval = 100;
	}
}

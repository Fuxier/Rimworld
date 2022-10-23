using System;

namespace Verse
{
	// Token: 0x02000523 RID: 1315
	public static class DataSerializeUtility
	{
		// Token: 0x060027F1 RID: 10225 RVA: 0x00103F9C File Offset: 0x0010219C
		public static byte[] SerializeByte(int elements, Func<int, byte> reader)
		{
			byte[] array = new byte[elements];
			for (int i = 0; i < elements; i++)
			{
				array[i] = reader(i);
			}
			return array;
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x0008A07A File Offset: 0x0008827A
		public static byte[] SerializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x0008A07A File Offset: 0x0008827A
		public static byte[] DeserializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x00103FC8 File Offset: 0x001021C8
		public static void LoadByte(byte[] arr, int elements, Action<int, byte> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, arr[i]);
			}
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x00103FF4 File Offset: 0x001021F4
		public static byte[] SerializeUshort(int elements, Func<int, ushort> reader)
		{
			byte[] array = new byte[elements * 2];
			for (int i = 0; i < elements; i++)
			{
				ushort num = reader(i);
				array[i * 2] = (byte)(num & 255);
				array[i * 2 + 1] = (byte)(num >> 8 & 255);
			}
			return array;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x00104040 File Offset: 0x00102240
		public static byte[] SerializeUshort(ushort[] data)
		{
			return DataSerializeUtility.SerializeUshort(data.Length, (int i) => data[i]);
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x00104074 File Offset: 0x00102274
		public static ushort[] DeserializeUshort(byte[] data)
		{
			ushort[] result = new ushort[data.Length / 2];
			DataSerializeUtility.LoadUshort(data, result.Length, delegate(int i, ushort dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x001040B8 File Offset: 0x001022B8
		public static void LoadUshort(byte[] arr, int elements, Action<int, ushort> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (ushort)((int)arr[i * 2] | (int)arr[i * 2 + 1] << 8));
			}
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x001040F0 File Offset: 0x001022F0
		public static byte[] SerializeInt(int elements, Func<int, int> reader)
		{
			byte[] array = new byte[elements * 4];
			for (int i = 0; i < elements; i++)
			{
				int num = reader(i);
				array[i * 4] = (byte)(num & 255);
				array[i * 4 + 1] = (byte)(num >> 8 & 255);
				array[i * 4 + 2] = (byte)(num >> 16 & 255);
				array[i * 4 + 3] = (byte)(num >> 24 & 255);
			}
			return array;
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x00104160 File Offset: 0x00102360
		public static byte[] SerializeInt(int[] data)
		{
			return DataSerializeUtility.SerializeInt(data.Length, (int i) => data[i]);
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x00104194 File Offset: 0x00102394
		public static int[] DeserializeInt(byte[] data)
		{
			int[] result = new int[data.Length / 4];
			DataSerializeUtility.LoadInt(data, result.Length, delegate(int i, int dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x001041D8 File Offset: 0x001023D8
		public static void LoadInt(byte[] arr, int elements, Action<int, int> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (int)arr[i * 4] | (int)arr[i * 4 + 1] << 8 | (int)arr[i * 4 + 2] << 16 | (int)arr[i * 4 + 3] << 24);
			}
		}
	}
}

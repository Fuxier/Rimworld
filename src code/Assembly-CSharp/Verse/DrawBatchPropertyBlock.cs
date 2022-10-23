using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001C5 RID: 453
	public class DrawBatchPropertyBlock
	{
		// Token: 0x06000CB6 RID: 3254 RVA: 0x00047769 File Offset: 0x00045969
		public void Clear()
		{
			this.properties.Clear();
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00047776 File Offset: 0x00045976
		public void SetFloat(string name, float val)
		{
			this.SetFloat(Shader.PropertyToID(name), val);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00047788 File Offset: 0x00045988
		public void SetFloat(int propertyId, float val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Float,
				floatVal = val
			});
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x000477C1 File Offset: 0x000459C1
		public void SetColor(string name, Color val)
		{
			this.SetColor(Shader.PropertyToID(name), val);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x000477D0 File Offset: 0x000459D0
		public void SetColor(int propertyId, Color val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Color,
				vectorVal = val
			});
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0004780E File Offset: 0x00045A0E
		public void SetVector(string name, Vector4 val)
		{
			this.SetVector(Shader.PropertyToID(name), val);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00047820 File Offset: 0x00045A20
		public void SetVector(int propertyId, Vector4 val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Vector,
				vectorVal = val
			});
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0004785C File Offset: 0x00045A5C
		public void Write(MaterialPropertyBlock propertyBlock)
		{
			foreach (DrawBatchPropertyBlock.Property property in this.properties)
			{
				property.Write(propertyBlock);
			}
		}

		// Token: 0x04000B94 RID: 2964
		private List<DrawBatchPropertyBlock.Property> properties = new List<DrawBatchPropertyBlock.Property>();

		// Token: 0x04000B95 RID: 2965
		public string leakDebugString;

		// Token: 0x02001D55 RID: 7509
		private enum PropertyType
		{
			// Token: 0x040073D6 RID: 29654
			Float,
			// Token: 0x040073D7 RID: 29655
			Color,
			// Token: 0x040073D8 RID: 29656
			Vector
		}

		// Token: 0x02001D56 RID: 7510
		private struct Property
		{
			// Token: 0x0600B40E RID: 46094 RVA: 0x0040F734 File Offset: 0x0040D934
			public void Write(MaterialPropertyBlock propertyBlock)
			{
				switch (this.type)
				{
				case DrawBatchPropertyBlock.PropertyType.Float:
					propertyBlock.SetFloat(this.propertyId, this.floatVal);
					return;
				case DrawBatchPropertyBlock.PropertyType.Color:
					propertyBlock.SetColor(this.propertyId, this.vectorVal);
					return;
				case DrawBatchPropertyBlock.PropertyType.Vector:
					propertyBlock.SetVector(this.propertyId, this.vectorVal);
					return;
				default:
					return;
				}
			}

			// Token: 0x040073D9 RID: 29657
			public int propertyId;

			// Token: 0x040073DA RID: 29658
			public DrawBatchPropertyBlock.PropertyType type;

			// Token: 0x040073DB RID: 29659
			public float floatVal;

			// Token: 0x040073DC RID: 29660
			public Vector4 vectorVal;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000204 RID: 516
	public class Section
	{
		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x00053374 File Offset: 0x00051574
		public CellRect CellRect
		{
			get
			{
				if (!this.foundRect)
				{
					this.calculatedRect = new CellRect(this.botLeft.x, this.botLeft.z, 17, 17);
					this.calculatedRect.ClipInsideMap(this.map);
					this.foundRect = true;
				}
				return this.calculatedRect;
			}
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x000533D0 File Offset: 0x000515D0
		public Section(IntVec3 sectCoords, Map map)
		{
			this.botLeft = sectCoords * 17;
			this.map = map;
			foreach (Type type in typeof(SectionLayer).AllSubclassesNonAbstract())
			{
				SectionLayer sectionLayer = (SectionLayer)Activator.CreateInstance(type, new object[]
				{
					this
				});
				this.layers.Add(sectionLayer);
				SectionLayer_SunShadows sectionLayer_SunShadows;
				if ((sectionLayer_SunShadows = (sectionLayer as SectionLayer_SunShadows)) != null)
				{
					this.layerSunShadows = sectionLayer_SunShadows;
				}
			}
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0005347C File Offset: 0x0005167C
		public void DrawSection(bool drawSunShadowsOnly)
		{
			if (drawSunShadowsOnly)
			{
				this.layerSunShadows.DrawLayer();
			}
			else
			{
				int count = this.layers.Count;
				for (int i = 0; i < count; i++)
				{
					this.layers[i].DrawLayer();
				}
			}
			if (!drawSunShadowsOnly && DebugViewSettings.drawSectionEdges)
			{
				GenDraw.DrawLineBetween(this.botLeft.ToVector3(), this.botLeft.ToVector3() + new Vector3(0f, 0f, 17f));
				GenDraw.DrawLineBetween(this.botLeft.ToVector3(), this.botLeft.ToVector3() + new Vector3(17f, 0f, 0f));
			}
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00053534 File Offset: 0x00051734
		public void RegenerateAllLayers()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i].Visible)
				{
					try
					{
						this.layers[i].Regenerate();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not regenerate layer ",
							this.layers[i].ToStringSafe<SectionLayer>(),
							": ",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x000535C8 File Offset: 0x000517C8
		public void RegenerateLayers(MapMeshFlag changeType)
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				SectionLayer sectionLayer = this.layers[i];
				if ((sectionLayer.relevantChangeTypes & changeType) != MapMeshFlag.None)
				{
					try
					{
						sectionLayer.Regenerate();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not regenerate layer ",
							sectionLayer.ToStringSafe<SectionLayer>(),
							": ",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0005364C File Offset: 0x0005184C
		public SectionLayer GetLayer(Type type)
		{
			return (from sect in this.layers
			where sect.GetType() == type
			select sect).FirstOrDefault<SectionLayer>();
		}

		// Token: 0x04000D72 RID: 3442
		public IntVec3 botLeft;

		// Token: 0x04000D73 RID: 3443
		public Map map;

		// Token: 0x04000D74 RID: 3444
		public MapMeshFlag dirtyFlags;

		// Token: 0x04000D75 RID: 3445
		private List<SectionLayer> layers = new List<SectionLayer>();

		// Token: 0x04000D76 RID: 3446
		private bool foundRect;

		// Token: 0x04000D77 RID: 3447
		private CellRect calculatedRect;

		// Token: 0x04000D78 RID: 3448
		private SectionLayer_SunShadows layerSunShadows;

		// Token: 0x04000D79 RID: 3449
		public const int Size = 17;
	}
}

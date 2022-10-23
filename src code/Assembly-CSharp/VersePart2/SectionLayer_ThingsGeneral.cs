using System;

namespace Verse
{
	// Token: 0x0200021A RID: 538
	public class SectionLayer_ThingsGeneral : SectionLayer_Things
	{
		// Token: 0x06000F62 RID: 3938 RVA: 0x000592F0 File Offset: 0x000574F0
		public SectionLayer_ThingsGeneral(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Things;
			this.requireAddToMapMesh = true;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00059308 File Offset: 0x00057508
		protected override void TakePrintFrom(Thing t)
		{
			try
			{
				t.Print(this);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception printing ",
					t,
					" at ",
					t.Position,
					": ",
					ex.ToString()
				}));
			}
		}
	}
}

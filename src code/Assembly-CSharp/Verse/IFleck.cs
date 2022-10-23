using System;

namespace Verse
{
	// Token: 0x020001C9 RID: 457
	public interface IFleck
	{
		// Token: 0x06000CCB RID: 3275
		void Setup(FleckCreationData creationData);

		// Token: 0x06000CCC RID: 3276
		bool TimeInterval(float deltaTime, Map map);

		// Token: 0x06000CCD RID: 3277
		void Draw(DrawBatch batch);
	}
}

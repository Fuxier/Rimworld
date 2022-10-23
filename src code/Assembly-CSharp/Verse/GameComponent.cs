using System;

namespace Verse
{
	// Token: 0x02000162 RID: 354
	public abstract class GameComponent : IExposable
	{
		// Token: 0x06000982 RID: 2434 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void ExposeData()
		{
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x000034B7 File Offset: 0x000016B7
		public virtual void LoadedGame()
		{
		}
	}
}

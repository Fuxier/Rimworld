using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001C8 RID: 456
	public sealed class FleckManager : IExposable
	{
		// Token: 0x06000CC3 RID: 3267 RVA: 0x00047960 File Offset: 0x00045B60
		public FleckManager()
		{
			foreach (FleckDef fleckDef in DefDatabase<FleckDef>.AllDefsListForReading)
			{
				FleckSystem fleckSystem;
				if (!this.systems.TryGetValue(fleckDef.fleckSystemClass, out fleckSystem))
				{
					fleckSystem = (FleckSystem)Activator.CreateInstance(fleckDef.fleckSystemClass);
					fleckSystem.parent = this;
					this.systems.Add(fleckDef.fleckSystemClass, fleckSystem);
				}
				fleckSystem.handledDefs.Add(fleckDef);
			}
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00047A14 File Offset: 0x00045C14
		public FleckManager(Map parent) : this()
		{
			this.parent = parent;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00047A24 File Offset: 0x00045C24
		public void CreateFleck(FleckCreationData fleckData)
		{
			FleckSystem fleckSystem;
			if (!this.systems.TryGetValue(fleckData.def.fleckSystemClass, out fleckSystem))
			{
				throw new Exception("No system to handle MoteDef " + fleckData.def + " found!?");
			}
			fleckData.spawnPosition.y = fleckData.def.altitudeLayer.AltitudeFor(fleckData.def.altitudeLayerIncOffset);
			fleckSystem.CreateFleck(fleckData);
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00047A94 File Offset: 0x00045C94
		public void FleckManagerUpdate()
		{
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.Update();
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00047AEC File Offset: 0x00045CEC
		public void FleckManagerTick()
		{
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.Tick();
			}
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00047B44 File Offset: 0x00045D44
		public void FleckManagerDraw()
		{
			try
			{
				foreach (FleckSystem fleckSystem in this.systems.Values)
				{
					fleckSystem.Draw(this.drawBatch);
				}
			}
			finally
			{
				this.drawBatch.Flush(true);
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00047BB8 File Offset: 0x00045DB8
		public void FleckManagerOnGUI()
		{
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.OnGUI();
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x000034B7 File Offset: 0x000016B7
		public void ExposeData()
		{
		}

		// Token: 0x04000BBA RID: 3002
		public readonly Map parent;

		// Token: 0x04000BBB RID: 3003
		private Dictionary<Type, FleckSystem> systems = new Dictionary<Type, FleckSystem>();

		// Token: 0x04000BBC RID: 3004
		private DrawBatch drawBatch = new DrawBatch();
	}
}

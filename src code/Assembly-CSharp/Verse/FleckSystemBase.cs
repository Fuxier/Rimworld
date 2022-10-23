using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001CC RID: 460
	public class FleckSystemBase<TFleck> : FleckSystem where TFleck : struct, IFleck
	{
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0000249D File Offset: 0x0000069D
		protected virtual bool ParallelizedDrawing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00047CF0 File Offset: 0x00045EF0
		public override void Update()
		{
			try
			{
				for (int i = this.dataRealtime.Count - 1; i >= 0; i--)
				{
					TFleck value = this.dataRealtime[i];
					if (value.TimeInterval(Time.deltaTime, this.parent.parent))
					{
						this.tmpRemoveIndices.Add(i);
					}
					else
					{
						this.dataRealtime[i] = value;
					}
				}
				this.dataRealtime.RemoveBatchUnordered(this.tmpRemoveIndices);
			}
			finally
			{
				this.tmpRemoveIndices.Clear();
			}
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00047D8C File Offset: 0x00045F8C
		public override void Tick()
		{
			try
			{
				for (int i = this.dataGametime.Count - 1; i >= 0; i--)
				{
					TFleck value = this.dataGametime[i];
					if (value.TimeInterval(0.016666668f, this.parent.parent))
					{
						this.tmpRemoveIndices.Add(i);
					}
					else
					{
						this.dataGametime[i] = value;
					}
				}
				this.dataGametime.RemoveBatchUnordered(this.tmpRemoveIndices);
			}
			finally
			{
				this.tmpRemoveIndices.Clear();
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00047E28 File Offset: 0x00046028
		private static void DrawParallel(object state)
		{
			FleckParallelizationInfo fleckParallelizationInfo = (FleckParallelizationInfo)state;
			try
			{
				List<FleckThrown> list = (List<FleckThrown>)fleckParallelizationInfo.data;
				for (int i = fleckParallelizationInfo.startIndex; i < fleckParallelizationInfo.endIndex; i++)
				{
					list[i].Draw(fleckParallelizationInfo.drawBatch);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			finally
			{
				fleckParallelizationInfo.doneEvent.Set();
			}
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00047EAC File Offset: 0x000460AC
		public override void Draw(DrawBatch drawBatch)
		{
			FleckSystemBase<TFleck>.<>c__DisplayClass11_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.drawBatch = drawBatch;
			foreach (FleckDef fleckDef in this.handledDefs)
			{
				if (fleckDef.graphicData != null)
				{
					fleckDef.graphicData.ExplicitlyInitCachedGraphic();
				}
				if (fleckDef.randomGraphics != null)
				{
					foreach (GraphicData graphicData in fleckDef.randomGraphics)
					{
						graphicData.ExplicitlyInitCachedGraphic();
					}
				}
			}
			if (this.ParallelizedDrawing)
			{
				if (this.CachedDrawParallelWaitCallback == null)
				{
					this.CachedDrawParallelWaitCallback = new WaitCallback(FleckSystemBase<TFleck>.DrawParallel);
				}
				FleckSystemBase<TFleck>.<>c__DisplayClass11_1 CS$<>8__locals2;
				CS$<>8__locals2.parallelizationDegree = Environment.ProcessorCount;
				this.<Draw>g__Process|11_1(this.dataRealtime, ref CS$<>8__locals1, ref CS$<>8__locals2);
				this.<Draw>g__Process|11_1(this.dataGametime, ref CS$<>8__locals1, ref CS$<>8__locals2);
				return;
			}
			this.<Draw>g__Process|11_0(this.dataRealtime, ref CS$<>8__locals1);
			this.<Draw>g__Process|11_0(this.dataGametime, ref CS$<>8__locals1);
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00047FD0 File Offset: 0x000461D0
		public override void CreateFleck(FleckCreationData creationData)
		{
			TFleck item = Activator.CreateInstance<TFleck>();
			item.Setup(creationData);
			if (creationData.def.realTime)
			{
				this.dataRealtime.Add(item);
				return;
			}
			this.dataGametime.Add(item);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x000034B7 File Offset: 0x000016B7
		public override void ExposeData()
		{
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00048058 File Offset: 0x00046258
		[CompilerGenerated]
		private void <Draw>g__Process|11_1(List<TFleck> data, ref FleckSystemBase<TFleck>.<>c__DisplayClass11_0 A_2, ref FleckSystemBase<TFleck>.<>c__DisplayClass11_1 A_3)
		{
			if (data.Count > 0)
			{
				try
				{
					this.tmpParallelizationSlices.Clear();
					GenThreading.SliceWorkNoAlloc(0, data.Count, A_3.parallelizationDegree, this.tmpParallelizationSlices);
					foreach (GenThreading.Slice slice in this.tmpParallelizationSlices)
					{
						FleckParallelizationInfo parallelizationInfo = FleckUtility.GetParallelizationInfo();
						parallelizationInfo.startIndex = slice.fromInclusive;
						parallelizationInfo.endIndex = slice.toExclusive;
						parallelizationInfo.data = data;
						ThreadPool.QueueUserWorkItem(this.CachedDrawParallelWaitCallback, parallelizationInfo);
						this.tmpParallelizationInfo.Add(parallelizationInfo);
					}
					foreach (FleckParallelizationInfo fleckParallelizationInfo in this.tmpParallelizationInfo)
					{
						fleckParallelizationInfo.doneEvent.WaitOne();
						A_2.drawBatch.MergeWith(fleckParallelizationInfo.drawBatch);
					}
				}
				finally
				{
					foreach (FleckParallelizationInfo info in this.tmpParallelizationInfo)
					{
						FleckUtility.ReturnParallelizationInfo(info);
					}
					this.tmpParallelizationInfo.Clear();
				}
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x000481C4 File Offset: 0x000463C4
		[CompilerGenerated]
		private void <Draw>g__Process|11_0(List<TFleck> data, ref FleckSystemBase<TFleck>.<>c__DisplayClass11_0 A_2)
		{
			for (int i = data.Count - 1; i >= 0; i--)
			{
				TFleck tfleck = data[i];
				tfleck.Draw(A_2.drawBatch);
			}
		}

		// Token: 0x04000BC3 RID: 3011
		private List<TFleck> dataRealtime = new List<TFleck>();

		// Token: 0x04000BC4 RID: 3012
		private List<TFleck> dataGametime = new List<TFleck>();

		// Token: 0x04000BC5 RID: 3013
		private List<GenThreading.Slice> tmpParallelizationSlices = new List<GenThreading.Slice>();

		// Token: 0x04000BC6 RID: 3014
		private List<FleckParallelizationInfo> tmpParallelizationInfo = new List<FleckParallelizationInfo>();

		// Token: 0x04000BC7 RID: 3015
		private List<int> tmpRemoveIndices = new List<int>();

		// Token: 0x04000BC8 RID: 3016
		private WaitCallback CachedDrawParallelWaitCallback;
	}
}

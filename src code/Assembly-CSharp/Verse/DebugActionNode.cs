using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000432 RID: 1074
	public class DebugActionNode
	{
		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x000BB9FB File Offset: 0x000B9BFB
		public bool IsRoot
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001F8D RID: 8077 RVA: 0x000BBA08 File Offset: 0x000B9C08
		public bool On
		{
			get
			{
				if (this.cachedCheckOn == null)
				{
					if (this.settingsField == null)
					{
						this.cachedCheckOn = new bool?(false);
					}
					else
					{
						this.cachedCheckOn = new bool?((bool)this.settingsField.GetValue(null));
					}
				}
				return this.cachedCheckOn.Value;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x000BBA68 File Offset: 0x000B9C68
		public string LabelNow
		{
			get
			{
				if (Time.frameCount >= this.lastLabelCacheFrame + 30)
				{
					this.DirtyLabelCache();
				}
				if (this.cachedLabelNow == null)
				{
					if (this.labelGetter != null)
					{
						try
						{
							this.cachedLabelNow = this.labelGetter();
							goto IL_66;
						}
						catch (Exception arg)
						{
							Log.Error("Exception getting label for DebugActionNode: " + arg);
							this.cachedLabelNow = null;
							return this.label;
						}
					}
					this.cachedLabelNow = this.label;
				}
				IL_66:
				return this.cachedLabelNow;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x000BBAF4 File Offset: 0x000B9CF4
		public bool ActiveNow
		{
			get
			{
				DebugActionType debugActionType = this.actionType;
				if (debugActionType - DebugActionType.ToolMap > 1)
				{
					return debugActionType != DebugActionType.ToolWorld || WorldRendererUtility.WorldRenderedNow;
				}
				return !WorldRendererUtility.WorldRenderedNow;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x000BBB22 File Offset: 0x000B9D22
		public bool VisibleNow
		{
			get
			{
				return (this.sourceAttribute == null || this.sourceAttribute.IsAllowedInCurrentGameState) && (this.visibilityGetter == null || this.visibilityGetter());
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x000BBB54 File Offset: 0x000B9D54
		public string Path
		{
			get
			{
				if (this.cachedPath == null)
				{
					if (this.parent != null && !this.parent.IsRoot)
					{
						this.cachedPath = this.parent.Path + "\\" + this.label;
					}
					else
					{
						this.cachedPath = this.label;
					}
				}
				return this.cachedPath;
			}
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x000BBBB3 File Offset: 0x000B9DB3
		public DebugActionNode()
		{
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x000BBBCD File Offset: 0x000B9DCD
		public DebugActionNode(string label, DebugActionType actionType = DebugActionType.Action, Action action = null, Action<Pawn> pawnAction = null)
		{
			this.label = label;
			this.actionType = actionType;
			this.action = action;
			this.pawnAction = pawnAction;
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x000BBC04 File Offset: 0x000B9E04
		public void AddChild(DebugActionNode child)
		{
			child.SetParent(this);
			this.children.Add(child);
			this.sorted = false;
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x000BBC20 File Offset: 0x000B9E20
		public void SetParent(DebugActionNode newParent)
		{
			this.parent = newParent;
			this.DirtyPath();
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x000BBC30 File Offset: 0x000B9E30
		private void DirtyPath()
		{
			this.cachedPath = null;
			foreach (DebugActionNode debugActionNode in this.children)
			{
				debugActionNode.DirtyPath();
			}
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x000BBC88 File Offset: 0x000B9E88
		public void DirtyLabelCache()
		{
			this.cachedLabelNow = null;
			this.cachedCheckOn = null;
			this.lastLabelCacheFrame = Time.frameCount;
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000BBCA8 File Offset: 0x000B9EA8
		public void TrySort()
		{
			if (this.sorted)
			{
				return;
			}
			if (!this.children.NullOrEmpty<DebugActionNode>())
			{
				this.children = (from r in this.children
				orderby DebugActionCategories.GetOrderFor(r.category), r.category, -r.displayPriority
				select r).ToList<DebugActionNode>();
			}
			this.sorted = true;
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x000BBD50 File Offset: 0x000B9F50
		public void TrySetupChildren()
		{
			if (!this.childrenSetup && this.childGetter != null)
			{
				foreach (DebugActionNode child in this.childGetter())
				{
					this.AddChild(child);
				}
			}
			this.childrenSetup = true;
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x000BBDC0 File Offset: 0x000B9FC0
		private static IEnumerable<Pawn> PawnsInside(IThingHolder holder)
		{
			Pawn pawn;
			if ((pawn = (holder as Pawn)) != null)
			{
				yield return pawn;
			}
			ThingOwner directlyHeldThings = holder.GetDirectlyHeldThings();
			if (directlyHeldThings == null)
			{
				yield break;
			}
			using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)directlyHeldThings).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IThingHolder holder2;
					if ((holder2 = (enumerator.Current as IThingHolder)) != null)
					{
						foreach (Pawn pawn2 in DebugActionNode.PawnsInside(holder2))
						{
							yield return pawn2;
						}
						IEnumerator<Pawn> enumerator2 = null;
					}
				}
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x000BBDD0 File Offset: 0x000B9FD0
		private static IEnumerable<Pawn> PawnsAt(IntVec3 cell, Map map)
		{
			using (IEnumerator<Thing> enumerator = map.thingGrid.ThingsAt(cell).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IThingHolder holder;
					if ((holder = (enumerator.Current as IThingHolder)) != null)
					{
						foreach (Pawn pawn in DebugActionNode.PawnsInside(holder))
						{
							yield return pawn;
						}
						IEnumerator<Pawn> enumerator2 = null;
					}
				}
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x000BBDE8 File Offset: 0x000B9FE8
		public void Enter(Dialog_Debug dialog)
		{
			this.TrySetupChildren();
			if (this.children.Any<DebugActionNode>())
			{
				this.TrySort();
				if (dialog == null)
				{
					dialog = new Dialog_Debug();
					Find.WindowStack.Add(dialog);
				}
				dialog.SetCurrentNode(this);
				return;
			}
			if (dialog != null)
			{
				dialog.Close(true);
			}
			switch (this.actionType)
			{
			case DebugActionType.Action:
				this.action();
				break;
			case DebugActionType.ToolMap:
			case DebugActionType.ToolWorld:
				DebugTools.curTool = new DebugTool(this.LabelNow, this.action, null);
				break;
			case DebugActionType.ToolMapForPawns:
			{
				DebugActionNode instance = this;
				DebugTools.curTool = new DebugTool(this.LabelNow, delegate()
				{
					if (UI.MouseCell().InBounds(Find.CurrentMap))
					{
						foreach (Pawn obj in DebugActionNode.PawnsAt(UI.MouseCell(), Find.CurrentMap).ToList<Pawn>())
						{
							instance.pawnAction(obj);
						}
					}
				}, null);
				break;
			}
			}
			this.DirtyLabelCache();
		}

		// Token: 0x0400158F RID: 5519
		public string label;

		// Token: 0x04001590 RID: 5520
		public DebugActionType actionType;

		// Token: 0x04001591 RID: 5521
		public Action action;

		// Token: 0x04001592 RID: 5522
		public Action<Pawn> pawnAction;

		// Token: 0x04001593 RID: 5523
		public Func<List<DebugActionNode>> childGetter;

		// Token: 0x04001594 RID: 5524
		public DebugActionAttribute sourceAttribute;

		// Token: 0x04001595 RID: 5525
		public Func<bool> visibilityGetter;

		// Token: 0x04001596 RID: 5526
		public Func<string> labelGetter;

		// Token: 0x04001597 RID: 5527
		public string category;

		// Token: 0x04001598 RID: 5528
		public int displayPriority;

		// Token: 0x04001599 RID: 5529
		public FieldInfo settingsField;

		// Token: 0x0400159A RID: 5530
		private bool childrenSetup;

		// Token: 0x0400159B RID: 5531
		public DebugActionNode parent;

		// Token: 0x0400159C RID: 5532
		public List<DebugActionNode> children = new List<DebugActionNode>();

		// Token: 0x0400159D RID: 5533
		private string cachedPath;

		// Token: 0x0400159E RID: 5534
		private string cachedLabelNow;

		// Token: 0x0400159F RID: 5535
		private bool sorted;

		// Token: 0x040015A0 RID: 5536
		private bool? cachedCheckOn;

		// Token: 0x040015A1 RID: 5537
		private int lastLabelCacheFrame = -1;

		// Token: 0x040015A2 RID: 5538
		private const int LabelRecacheFrameCount = 30;
	}
}

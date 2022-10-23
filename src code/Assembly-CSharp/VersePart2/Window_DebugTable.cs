using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200047D RID: 1149
	public class Window_DebugTable : Window
	{
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060022CE RID: 8910 RVA: 0x00004E17 File Offset: 0x00003017
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000DE5F8 File Offset: 0x000DC7F8
		public Window_DebugTable(string[,] tables)
		{
			this.tableRaw = tables;
			this.colVisible = new bool[this.tableRaw.GetLength(0)];
			for (int i = 0; i < this.colVisible.Length; i++)
			{
				this.colVisible[i] = true;
			}
			this.doCloseButton = true;
			this.doCloseX = true;
			Text.Font = GameFont.Tiny;
			this.BuildTableSorted();
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x000DE688 File Offset: 0x000DC888
		private void BuildTableSorted()
		{
			if (this.sortMode == Window_DebugTable.SortMode.Off)
			{
				this.tableSorted = this.tableRaw;
			}
			else
			{
				List<List<string>> list = new List<List<string>>();
				for (int i = 1; i < this.tableRaw.GetLength(1); i++)
				{
					list.Add(new List<string>());
					for (int j = 0; j < this.tableRaw.GetLength(0); j++)
					{
						list[i - 1].Add(this.tableRaw[j, i]);
					}
				}
				NumericStringComparer comparer = new NumericStringComparer();
				switch (this.sortMode)
				{
				case Window_DebugTable.SortMode.Off:
					throw new Exception();
				case Window_DebugTable.SortMode.Ascending:
					list = list.OrderBy((List<string> x) => x[this.sortColumn], comparer).ToList<List<string>>();
					break;
				case Window_DebugTable.SortMode.Descending:
					list = list.OrderByDescending((List<string> x) => x[this.sortColumn], comparer).ToList<List<string>>();
					break;
				}
				this.tableSorted = new string[this.tableRaw.GetLength(0), this.tableRaw.GetLength(1)];
				for (int k = 0; k < this.tableRaw.GetLength(1); k++)
				{
					for (int l = 0; l < this.tableRaw.GetLength(0); l++)
					{
						if (k == 0)
						{
							this.tableSorted[l, k] = this.tableRaw[l, k];
						}
						else
						{
							this.tableSorted[l, k] = list[k - 1][l];
						}
					}
				}
			}
			this.colWidths.Clear();
			for (int m = 0; m < this.tableRaw.GetLength(0); m++)
			{
				float item;
				if (this.colVisible[m])
				{
					float num = 0f;
					for (int n = 0; n < this.tableRaw.GetLength(1); n++)
					{
						float x2 = Text.CalcSize(this.tableRaw[m, n]).x;
						if (x2 > num)
						{
							num = x2;
						}
					}
					item = num + 2f;
				}
				else
				{
					item = 10f;
				}
				this.colWidths.Add(item);
			}
			this.rowHeights.Clear();
			for (int num2 = 0; num2 < this.tableSorted.GetLength(1); num2++)
			{
				float num3 = 0f;
				for (int num4 = 0; num4 < this.tableSorted.GetLength(0); num4++)
				{
					float y = Text.CalcSize(this.tableSorted[num4, num2]).y;
					if (y > num3)
					{
						num3 = y;
					}
				}
				this.rowHeights.Add(num3 + 2f);
			}
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x000DE920 File Offset: 0x000DCB20
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			Rect outRect = inRect;
			outRect.yMax -= 40f;
			Rect viewRect = new Rect(0f, 0f, this.colWidths.Sum(), this.rowHeights.Sum());
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			for (int i = 0; i < this.tableSorted.GetLength(0); i++)
			{
				float num2 = 0f;
				for (int j = 0; j < this.tableSorted.GetLength(1); j++)
				{
					if (num2 + this.rowHeights[j] < outRect.yMin + this.scrollPosition.y || num2 > outRect.yMax + this.scrollPosition.y)
					{
						num2 += this.rowHeights[j];
					}
					else
					{
						Rect rect = new Rect(num, num2, this.colWidths[i], this.rowHeights[j]);
						Rect rect2 = rect;
						rect2.xMin -= 999f;
						rect2.xMax += 999f;
						if (Mouse.IsOver(rect2) || i % 2 == 0)
						{
							Widgets.DrawHighlight(rect);
						}
						if (j == 0 && Mouse.IsOver(rect))
						{
							rect.x += 2f;
							rect.y += 2f;
						}
						if (i == 0 || this.colVisible[i])
						{
							Widgets.Label(rect, this.tableSorted[i, j]);
						}
						if (j == 0)
						{
							MouseoverSounds.DoRegion(rect);
							if (Mouse.IsOver(rect) && Event.current.type == EventType.MouseDown)
							{
								if (Event.current.button == 0)
								{
									if (i != this.sortColumn)
									{
										this.sortMode = Window_DebugTable.SortMode.Off;
									}
									switch (this.sortMode)
									{
									case Window_DebugTable.SortMode.Off:
										this.sortMode = Window_DebugTable.SortMode.Descending;
										this.sortColumn = i;
										SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
										break;
									case Window_DebugTable.SortMode.Ascending:
										this.sortMode = Window_DebugTable.SortMode.Off;
										this.sortColumn = -1;
										SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
										break;
									case Window_DebugTable.SortMode.Descending:
										this.sortMode = Window_DebugTable.SortMode.Ascending;
										this.sortColumn = i;
										SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
										break;
									}
									this.BuildTableSorted();
								}
								else if (Event.current.button == 1)
								{
									this.colVisible[i] = !this.colVisible[i];
									SoundDefOf.Crunch.PlayOneShotOnCamera(null);
									this.BuildTableSorted();
								}
								Event.current.Use();
							}
						}
						num2 += this.rowHeights[j];
					}
				}
				num += this.colWidths[i];
			}
			Widgets.EndScrollView();
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(new Rect(inRect.xMax - 120f, inRect.yMax - 30f, 120f, 30f), "Copy CSV", true, true, true, null))
			{
				this.CopyCSVToClipboard();
				Messages.Message("Copied table data to clipboard in CSV format.", MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x000DEC54 File Offset: 0x000DCE54
		private void MouseOverHeaderRowCell(Rect cell, int col)
		{
			MouseoverSounds.DoRegion(cell);
			if (Mouse.IsOver(cell) && Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (col != this.sortColumn)
					{
						this.sortMode = Window_DebugTable.SortMode.Off;
					}
					switch (this.sortMode)
					{
					case Window_DebugTable.SortMode.Off:
						this.sortMode = Window_DebugTable.SortMode.Descending;
						this.sortColumn = col;
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						break;
					case Window_DebugTable.SortMode.Ascending:
						this.sortMode = Window_DebugTable.SortMode.Off;
						this.sortColumn = -1;
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						break;
					case Window_DebugTable.SortMode.Descending:
						this.sortMode = Window_DebugTable.SortMode.Ascending;
						this.sortColumn = col;
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						break;
					}
					this.BuildTableSorted();
				}
				else if (Event.current.button == 1)
				{
					this.colVisible[col] = !this.colVisible[col];
					SoundDefOf.Crunch.PlayOneShotOnCamera(null);
					this.BuildTableSorted();
				}
				Event.current.Use();
			}
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x000DED50 File Offset: 0x000DCF50
		private static void MouseCenterButtonDrag(ref Vector2 scroll)
		{
			if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
			{
				Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
				Event.current.Use();
				if (currentEventDelta != Vector2.zero)
				{
					scroll += -1f * currentEventDelta;
				}
			}
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x000DEDB0 File Offset: 0x000DCFB0
		private static bool IsMouseOverRow(Rect cell)
		{
			cell.xMin -= 9999999f;
			cell.xMax += 9999999f;
			return Mouse.IsOver(cell);
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x000DEDDE File Offset: 0x000DCFDE
		private static bool IsMouseOverCol(Rect cell)
		{
			cell.yMin -= 9999999f;
			cell.yMax += 9999999f;
			return Mouse.IsOver(cell);
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x000DEE0C File Offset: 0x000DD00C
		private static void DrawOpaqueLabel(Rect r, string label, bool highlighted = true, bool selectedRow = false, bool selectedCol = false, bool offsetLabel = false)
		{
			if (offsetLabel)
			{
				r.height += 2f;
			}
			Color color = GUI.color;
			GUI.color = Widgets.WindowBGFillColor;
			GUI.DrawTexture(r, BaseContent.WhiteTex);
			GUI.color = color;
			if (selectedRow)
			{
				Widgets.DrawHighlightSelected(r);
			}
			if (selectedCol)
			{
				Widgets.DrawHighlightSelected(r);
			}
			if (highlighted)
			{
				Widgets.DrawHighlight(r);
			}
			else
			{
				Widgets.DrawLightHighlight(r);
			}
			if (offsetLabel)
			{
				r.y += 2f;
			}
			Widgets.Label(r, label);
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x000DEE94 File Offset: 0x000DD094
		private void CopyCSVToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.tableSorted.GetLength(1); i++)
			{
				for (int j = 0; j < this.tableSorted.GetLength(0); j++)
				{
					if (j != 0)
					{
						stringBuilder.Append(",");
					}
					string text = this.tableSorted[j, i] ?? "";
					stringBuilder.Append("\"" + text.Replace("\n", " ") + "\"");
				}
				stringBuilder.Append("\n");
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x04001626 RID: 5670
		private string[,] tableRaw;

		// Token: 0x04001627 RID: 5671
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001628 RID: 5672
		private string[,] tableSorted;

		// Token: 0x04001629 RID: 5673
		private List<float> colWidths = new List<float>();

		// Token: 0x0400162A RID: 5674
		private List<float> rowHeights = new List<float>();

		// Token: 0x0400162B RID: 5675
		private int sortColumn = -1;

		// Token: 0x0400162C RID: 5676
		private Window_DebugTable.SortMode sortMode;

		// Token: 0x0400162D RID: 5677
		private bool[] colVisible;

		// Token: 0x0400162E RID: 5678
		private const float ColExtraWidth = 2f;

		// Token: 0x0400162F RID: 5679
		private const float RowExtraHeight = 2f;

		// Token: 0x04001630 RID: 5680
		private const float HiddenColumnWidth = 10f;

		// Token: 0x04001631 RID: 5681
		private const float MouseoverOffset = 2f;

		// Token: 0x02002092 RID: 8338
		private enum SortMode
		{
			// Token: 0x040081A3 RID: 33187
			Off,
			// Token: 0x040081A4 RID: 33188
			Ascending,
			// Token: 0x040081A5 RID: 33189
			Descending
		}
	}
}

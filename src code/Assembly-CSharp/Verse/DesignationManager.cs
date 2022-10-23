using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using UnityEngine.Rendering;

namespace Verse
{
	// Token: 0x020001C2 RID: 450
	public sealed class DesignationManager : IExposable
	{
		// Token: 0x06000C8E RID: 3214 RVA: 0x00046154 File Offset: 0x00044354
		public DesignationManager(Map map)
		{
			this.map = map;
			this.designationsAtCell = new List<Designation>[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x000461BC File Offset: 0x000443BC
		public List<Designation> AllDesignations
		{
			get
			{
				DesignationManager.tmpDesignations.Clear();
				foreach (KeyValuePair<DesignationDef, List<Designation>> keyValuePair in this.designationsByDef)
				{
					DesignationManager.tmpDesignations.AddRange(keyValuePair.Value);
				}
				return DesignationManager.tmpDesignations;
			}
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00046224 File Offset: 0x00044424
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.saveDesignations.Clear();
				this.saveDesignations.AddRange(this.AllDesignations);
			}
			Scribe_Collections.Look<Designation>(ref this.saveDesignations, "allDesignations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.saveDesignations.RemoveAll((Designation x) => x == null) != 0)
				{
					Log.Warning("Some designations were null after loading.");
				}
				if (this.saveDesignations.RemoveAll((Designation x) => x.def == null) != 0)
				{
					Log.Warning("Some designations had null def after loading.");
				}
				foreach (Designation designation in this.saveDesignations)
				{
					designation.designationManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				int i = this.saveDesignations.Count - 1;
				while (i >= 0)
				{
					TargetType targetType = this.saveDesignations[i].def.targetType;
					if (targetType != TargetType.Thing)
					{
						if (targetType != TargetType.Cell)
						{
							goto IL_19F;
						}
						if (this.saveDesignations[i].target.Cell.IsValid)
						{
							goto IL_19F;
						}
						Log.Error("Cell-needing designation " + this.saveDesignations[i] + " had no cell target. Removing...");
					}
					else
					{
						if (this.saveDesignations[i].target.HasThing)
						{
							goto IL_19F;
						}
						Log.Error("Thing-needing designation " + this.saveDesignations[i] + " had no thing target. Removing...");
					}
					IL_1B1:
					i--;
					continue;
					IL_19F:
					this.IndexDesignation(this.saveDesignations[i]);
					goto IL_1B1;
				}
				this.saveDesignations.Clear();
			}
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00046408 File Offset: 0x00044608
		public void DrawDesignations()
		{
			CellRect cellRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(3);
			if (this.tmpPropertyBlock == null)
			{
				this.tmpPropertyBlock = new MaterialPropertyBlock();
			}
			foreach (DesignationDef designationDef in DefDatabase<DesignationDef>.AllDefsListForReading)
			{
				if (designationDef.targetType == TargetType.Cell && designationDef.shouldBatchDraw && SystemInfo.supportsInstancing)
				{
					int count = this.designationsByDef[designationDef].Count;
					if (this.designationsByDef[designationDef].Count != 0)
					{
						this.CalculateCellDesignationDrawMatricies(designationDef);
						List<Matrix4x4[]> list = this.cellDesignationDrawMatricies[designationDef];
						designationDef.iconMat.enableInstancing = true;
						int num = count / 1023;
						for (int i = 0; i < num; i++)
						{
							Graphics.DrawMeshInstanced(MeshPool.plane10, 0, designationDef.iconMat, list[i], 1023, this.tmpPropertyBlock, ShadowCastingMode.Off, true, 0);
						}
						int num2 = count % 1023;
						if (num2 > 0)
						{
							Graphics.DrawMeshInstanced(MeshPool.plane10, 0, designationDef.iconMat, list[num], num2, this.tmpPropertyBlock, ShadowCastingMode.Off, true, 0);
						}
					}
				}
				else
				{
					foreach (Designation designation in this.designationsByDef[designationDef])
					{
						if ((!designation.target.HasThing || designation.target.Thing.Map == this.map) && cellRect.Contains(designation.target.Cell))
						{
							designation.DesignationDraw();
						}
					}
				}
			}
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00046608 File Offset: 0x00044808
		private void DirtyCellDesignationsCache(DesignationDef def)
		{
			if (def.targetType != TargetType.Cell)
			{
				return;
			}
			this.designationMatriciesDirty[def] = true;
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00046624 File Offset: 0x00044824
		public void AddDesignation(Designation newDes)
		{
			if (newDes.def.targetType == TargetType.Cell && this.DesignationAt(newDes.target.Cell, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation at location " + newDes.target);
				return;
			}
			if (newDes.def.targetType == TargetType.Thing && this.DesignationOn(newDes.target.Thing, newDes.def) != null)
			{
				Log.Error("Tried to double-add designation on Thing " + newDes.target);
				return;
			}
			if (newDes.def.targetType == TargetType.Thing)
			{
				newDes.target.Thing.SetForbidden(false, false);
			}
			this.IndexDesignation(newDes);
			newDes.designationManager = this;
			newDes.Notify_Added();
			Map map = newDes.target.HasThing ? newDes.target.Thing.Map : this.map;
			if (map != null)
			{
				FleckMaker.ThrowMetaPuffs(newDes.target.ToTargetInfo(map));
			}
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00046724 File Offset: 0x00044924
		private void IndexDesignation(Designation designation)
		{
			this.designationsByDef[designation.def].Add(designation);
			this.DirtyCellDesignationsCache(designation.def);
			if (designation.def.targetType == TargetType.Thing)
			{
				Thing thing = designation.target.Thing;
				if (!this.thingDesignations.ContainsKey(thing))
				{
					this.thingDesignations[thing] = new List<Designation>();
				}
				this.thingDesignations[thing].Add(designation);
				return;
			}
			if (designation.def.targetType != TargetType.Cell)
			{
				Log.Error(string.Format("Tried to index unexpected designation type: {0}", designation.def.targetType));
				return;
			}
			List<Designation> list;
			if (this.TryGetCellDesignations(designation.target.Cell, out list, true))
			{
				list.Add(designation);
				return;
			}
			Log.Error(string.Format("Tried to create cell target designation at invalid cell: {0}", designation.target.Cell));
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0004680C File Offset: 0x00044A0C
		private void CalculateCellDesignationDrawMatricies(DesignationDef def)
		{
			if (def.targetType != TargetType.Cell || !this.designationMatriciesDirty[def])
			{
				return;
			}
			List<Designation> list = this.designationsByDef[def];
			int count = list.Count;
			while (this.cellDesignationDrawMatricies[def].Count * 1023 < count)
			{
				this.cellDesignationDrawMatricies[def].Add(new Matrix4x4[1023]);
			}
			for (int i = 0; i < count; i++)
			{
				Designation designation = list[i];
				int index = i / 1023;
				this.cellDesignationDrawMatricies[def][index][i % 1023] = Matrix4x4.TRS(designation.DrawLoc(), Quaternion.identity, Vector3.one);
			}
			this.designationMatriciesDirty[def] = false;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x000468DC File Offset: 0x00044ADC
		public Designation DesignationOn(Thing t)
		{
			List<Designation> list;
			this.thingDesignations.TryGetValue(t, out list);
			if (list.NullOrEmpty<Designation>())
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0004690C File Offset: 0x00044B0C
		public Designation DesignationOn(Thing t, DesignationDef def)
		{
			if (def.targetType == TargetType.Cell)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by location only and you are trying to get one on a Thing.");
				return null;
			}
			List<Designation> list;
			this.thingDesignations.TryGetValue(t, out list);
			if (list.NullOrEmpty<Designation>())
			{
				return null;
			}
			foreach (Designation designation in list)
			{
				if (designation.def == def)
				{
					return designation;
				}
			}
			return null;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x000469A4 File Offset: 0x00044BA4
		public Designation DesignationAt(IntVec3 c, DesignationDef def)
		{
			if (def.targetType == TargetType.Thing)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by Thing only and you are trying to get one on a location.");
				return null;
			}
			List<Designation> list;
			if (this.TryGetCellDesignations(c, out list, false))
			{
				for (int i = 0; i < list.Count; i++)
				{
					Designation designation = list[i];
					if (designation.def == def)
					{
						return designation;
					}
				}
			}
			return null;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00046A06 File Offset: 0x00044C06
		public List<Designation> AllDesignationsOn(Thing t)
		{
			if (!this.thingDesignations.ContainsKey(t))
			{
				return DesignationManager.EmptyList;
			}
			return this.thingDesignations[t];
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00046A28 File Offset: 0x00044C28
		public List<Designation> AllDesignationsAt(IntVec3 c)
		{
			DesignationManager.tmpDesignations.Clear();
			List<Designation> collection;
			if (this.TryGetCellDesignations(c, out collection, false))
			{
				DesignationManager.tmpDesignations.AddRange(collection);
			}
			foreach (Thing t in this.map.thingGrid.ThingsListAt(c))
			{
				DesignationManager.tmpDesignations.AddRange(this.AllDesignationsOn(t));
			}
			return DesignationManager.tmpDesignations;
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x00046AB8 File Offset: 0x00044CB8
		public bool HasMapDesignationAt(IntVec3 c)
		{
			List<Designation> list;
			return this.TryGetCellDesignations(c, out list, false) && !list.NullOrEmpty<Designation>();
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00046ADC File Offset: 0x00044CDC
		public bool HasMapDesignationOn(Thing t)
		{
			return this.DesignationOn(t) != null;
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00046AE8 File Offset: 0x00044CE8
		public IEnumerable<Designation> SpawnedDesignationsOfDef(DesignationDef def)
		{
			foreach (Designation designation in this.designationsByDef[def])
			{
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map))
				{
					yield return designation;
				}
			}
			List<Designation>.Enumerator enumerator = default(List<Designation>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00046B00 File Offset: 0x00044D00
		public bool AnySpawnedDesignationOfDef(DesignationDef def)
		{
			foreach (Designation designation in this.designationsByDef[def])
			{
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00046B88 File Offset: 0x00044D88
		private bool TryGetCellDesignations(IntVec3 cell, out List<Designation> foundDesignations, bool initializeIfNull = false)
		{
			int num = this.map.cellIndices.CellToIndex(cell);
			if (num < 0 || num >= this.designationsAtCell.Length)
			{
				foundDesignations = null;
				return false;
			}
			foundDesignations = this.designationsAtCell[num];
			if (foundDesignations == null)
			{
				if (initializeIfNull)
				{
					foundDesignations = new List<Designation>();
					this.designationsAtCell[num] = foundDesignations;
				}
				else
				{
					foundDesignations = DesignationManager.EmptyList;
				}
			}
			return true;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00046BE8 File Offset: 0x00044DE8
		public void RemoveDesignation(Designation des)
		{
			des.Notify_Removing();
			if (des.def.targetType == TargetType.Cell)
			{
				List<Designation> list;
				if (this.TryGetCellDesignations(des.target.Cell, out list, false))
				{
					list.Remove(des);
				}
				else
				{
					Log.Warning(string.Format("Tried to remove designation with target cell that couldn't be found in index: {0}", des.target.Cell));
				}
			}
			else if (des.def.targetType == TargetType.Thing)
			{
				Thing thing = des.target.Thing;
				if (this.thingDesignations.ContainsKey(thing))
				{
					List<Designation> list2 = this.thingDesignations[thing];
					list2.Remove(des);
					if (list2.Count == 0)
					{
						this.thingDesignations.Remove(des.target.Thing);
					}
				}
				else
				{
					Log.Warning("Tried to remove thing designation that wasn't indexed");
				}
			}
			else
			{
				Log.Error(string.Format("Tried to remove designation with unexpected type: {0}", des.def.targetType));
			}
			this.designationsByDef[des.def].Remove(des);
			this.DirtyCellDesignationsCache(des.def);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00046CFC File Offset: 0x00044EFC
		public void TryRemoveDesignation(IntVec3 c, DesignationDef def)
		{
			Designation designation = this.DesignationAt(c, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00046D1C File Offset: 0x00044F1C
		public void RemoveAllDesignationsOn(Thing t, bool standardCanceling = false)
		{
			List<Designation> list = this.AllDesignationsOn(t);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Designation designation = list[i];
				if (!standardCanceling || designation.def.designateCancelable)
				{
					this.RemoveDesignation(list[i]);
				}
			}
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00046D6C File Offset: 0x00044F6C
		public void TryRemoveDesignationOn(Thing t, DesignationDef def)
		{
			Designation designation = this.DesignationOn(t, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00046D8C File Offset: 0x00044F8C
		public void RemoveAllDesignationsOfDef(DesignationDef def)
		{
			List<Designation> list = this.designationsByDef[def];
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.RemoveDesignation(list[i]);
			}
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00046DC8 File Offset: 0x00044FC8
		public void Notify_BuildingDespawned(Thing b)
		{
			CellRect cellRect = b.OccupiedRect();
			foreach (KeyValuePair<DesignationDef, List<Designation>> keyValuePair in this.designationsByDef)
			{
				if (keyValuePair.Key.removeIfBuildingDespawned)
				{
					List<Designation> value = keyValuePair.Value;
					for (int i = value.Count - 1; i >= 0; i--)
					{
						if (cellRect.Contains(value[i].target.Cell))
						{
							this.RemoveDesignation(value[i]);
						}
					}
				}
			}
		}

		// Token: 0x04000B7C RID: 2940
		public Map map;

		// Token: 0x04000B7D RID: 2941
		public DefMap<DesignationDef, List<Designation>> designationsByDef = new DefMap<DesignationDef, List<Designation>>();

		// Token: 0x04000B7E RID: 2942
		private DefMap<DesignationDef, List<Matrix4x4[]>> cellDesignationDrawMatricies = new DefMap<DesignationDef, List<Matrix4x4[]>>();

		// Token: 0x04000B7F RID: 2943
		private DefMap<DesignationDef, bool> designationMatriciesDirty = new DefMap<DesignationDef, bool>();

		// Token: 0x04000B80 RID: 2944
		private List<Designation>[] designationsAtCell;

		// Token: 0x04000B81 RID: 2945
		private Dictionary<Thing, List<Designation>> thingDesignations = new Dictionary<Thing, List<Designation>>();

		// Token: 0x04000B82 RID: 2946
		private MaterialPropertyBlock tmpPropertyBlock;

		// Token: 0x04000B83 RID: 2947
		private List<Designation> saveDesignations = new List<Designation>();

		// Token: 0x04000B84 RID: 2948
		private static readonly List<Designation> EmptyList = new List<Designation>();

		// Token: 0x04000B85 RID: 2949
		private static List<Designation> tmpDesignations = new List<Designation>();
	}
}

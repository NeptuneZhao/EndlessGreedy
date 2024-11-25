using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000A4C RID: 2636
public class RequiresFoundation : KGameObjectComponentManager<RequiresFoundation.Data>, IKComponentManager
{
	// Token: 0x06004C6B RID: 19563 RVA: 0x001B4808 File Offset: 0x001B2A08
	public HandleVector<int>.Handle Add(GameObject go)
	{
		BuildingDef def = go.GetComponent<Building>().Def;
		int cell = Grid.PosToCell(go.transform.GetPosition());
		RequiresFoundation.Data data = new RequiresFoundation.Data
		{
			cell = cell,
			width = def.WidthInCells,
			height = def.HeightInCells,
			buildRule = def.BuildLocationRule,
			solid = true,
			go = go
		};
		HandleVector<int>.Handle h = base.Add(go, data);
		if (def.ContinuouslyCheckFoundation)
		{
			data.changeCallback = delegate(object d)
			{
				this.OnSolidChanged(h);
			};
			Rotatable component = data.go.GetComponent<Rotatable>();
			Orientation orientation = (component != null) ? component.GetOrientation() : Orientation.Neutral;
			int num = -(def.WidthInCells - 1) / 2;
			int x = def.WidthInCells / 2;
			CellOffset offset = new CellOffset(num, -1);
			CellOffset offset2 = new CellOffset(x, -1);
			if (def.BuildLocationRule == BuildLocationRule.OnCeiling || def.BuildLocationRule == BuildLocationRule.InCorner)
			{
				offset.y = def.HeightInCells;
				offset2.y = def.HeightInCells;
			}
			else if (def.BuildLocationRule == BuildLocationRule.OnWall)
			{
				offset = new CellOffset(num - 1, 0);
				offset2 = new CellOffset(num - 1, def.HeightInCells);
			}
			else if (def.BuildLocationRule == BuildLocationRule.WallFloor)
			{
				offset = new CellOffset(num - 1, -1);
				offset2 = new CellOffset(x, def.HeightInCells - 1);
			}
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offset, orientation);
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(offset2, orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			Vector2I vector2I = Grid.CellToXY(cell2);
			Vector2I vector2I2 = Grid.CellToXY(cell3);
			float xmin = (float)Mathf.Min(vector2I.x, vector2I2.x);
			float xmax = (float)Mathf.Max(vector2I.x, vector2I2.x);
			float ymin = (float)Mathf.Min(vector2I.y, vector2I2.y);
			float ymax = (float)Mathf.Max(vector2I.y, vector2I2.y);
			Rect rect = Rect.MinMaxRect(xmin, ymin, xmax, ymax);
			data.solidPartitionerEntry = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", go, (int)rect.x, (int)rect.y, (int)rect.width + 1, (int)rect.height + 1, GameScenePartitioner.Instance.solidChangedLayer, data.changeCallback);
			data.buildingPartitionerEntry = GameScenePartitioner.Instance.Add("RequiresFoundation.Add", go, (int)rect.x, (int)rect.y, (int)rect.width + 1, (int)rect.height + 1, GameScenePartitioner.Instance.objectLayers[1], data.changeCallback);
			if (def.BuildLocationRule == BuildLocationRule.BuildingAttachPoint || def.BuildLocationRule == BuildLocationRule.OnFloorOrBuildingAttachPoint)
			{
				AttachableBuilding component2 = data.go.GetComponent<AttachableBuilding>();
				component2.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(component2.onAttachmentNetworkChanged, data.changeCallback);
			}
			base.SetData(h, data);
			this.OnSolidChanged(h);
			data = base.GetData(h);
			this.UpdateSolidState(data.solid, ref data, true);
		}
		return h;
	}

	// Token: 0x06004C6C RID: 19564 RVA: 0x001B4B2C File Offset: 0x001B2D2C
	protected override void OnCleanUp(HandleVector<int>.Handle h)
	{
		RequiresFoundation.Data data = base.GetData(h);
		GameScenePartitioner.Instance.Free(ref data.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref data.buildingPartitionerEntry);
		AttachableBuilding component = data.go.GetComponent<AttachableBuilding>();
		if (!component.IsNullOrDestroyed())
		{
			AttachableBuilding attachableBuilding = component;
			attachableBuilding.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachableBuilding.onAttachmentNetworkChanged, data.changeCallback);
		}
		base.SetData(h, data);
	}

	// Token: 0x06004C6D RID: 19565 RVA: 0x001B4B9C File Offset: 0x001B2D9C
	private void OnSolidChanged(HandleVector<int>.Handle h)
	{
		RequiresFoundation.Data data = base.GetData(h);
		SimCellOccupier component = data.go.GetComponent<SimCellOccupier>();
		if (component == null || component.IsReady())
		{
			Rotatable component2 = data.go.GetComponent<Rotatable>();
			Orientation orientation = (component2 != null) ? component2.GetOrientation() : Orientation.Neutral;
			bool flag = BuildingDef.CheckFoundation(data.cell, orientation, data.buildRule, data.width, data.height, default(Tag));
			if (!flag && (data.buildRule == BuildLocationRule.BuildingAttachPoint || data.buildRule == BuildLocationRule.OnFloorOrBuildingAttachPoint))
			{
				List<GameObject> list = new List<GameObject>();
				AttachableBuilding.GetAttachedBelow(data.go.GetComponent<AttachableBuilding>(), ref list);
				if (list.Count > 0)
				{
					Operational component3 = list.Last<GameObject>().GetComponent<Operational>();
					if (component3 != null && component3.GetFlag(RequiresFoundation.solidFoundation))
					{
						flag = true;
					}
				}
			}
			this.UpdateSolidState(flag, ref data, false);
			base.SetData(h, data);
		}
	}

	// Token: 0x06004C6E RID: 19566 RVA: 0x001B4C94 File Offset: 0x001B2E94
	private void UpdateSolidState(bool is_solid, ref RequiresFoundation.Data data, bool forceUpdate = false)
	{
		if (data.solid != is_solid || forceUpdate)
		{
			data.solid = is_solid;
			Operational component = data.go.GetComponent<Operational>();
			if (component != null)
			{
				component.SetFlag(RequiresFoundation.solidFoundation, is_solid);
			}
			AttachableBuilding component2 = data.go.GetComponent<AttachableBuilding>();
			if (component2 != null)
			{
				List<GameObject> buildings = new List<GameObject>();
				AttachableBuilding.GetAttachedAbove(component2, ref buildings);
				AttachableBuilding.NotifyBuildingsNetworkChanged(buildings, null);
			}
			data.go.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.MissingFoundation, !is_solid, this);
		}
	}

	// Token: 0x040032DD RID: 13021
	public static readonly Operational.Flag solidFoundation = new Operational.Flag("solid_foundation", Operational.Flag.Type.Functional);

	// Token: 0x02001A51 RID: 6737
	public struct Data
	{
		// Token: 0x04007C08 RID: 31752
		public int cell;

		// Token: 0x04007C09 RID: 31753
		public int width;

		// Token: 0x04007C0A RID: 31754
		public int height;

		// Token: 0x04007C0B RID: 31755
		public BuildLocationRule buildRule;

		// Token: 0x04007C0C RID: 31756
		public HandleVector<int>.Handle solidPartitionerEntry;

		// Token: 0x04007C0D RID: 31757
		public HandleVector<int>.Handle buildingPartitionerEntry;

		// Token: 0x04007C0E RID: 31758
		public bool solid;

		// Token: 0x04007C0F RID: 31759
		public GameObject go;

		// Token: 0x04007C10 RID: 31760
		public Action<object> changeCallback;
	}
}

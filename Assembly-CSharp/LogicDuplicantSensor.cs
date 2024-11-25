using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000706 RID: 1798
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicDuplicantSensor : Switch, ISim1000ms, ISim200ms
{
	// Token: 0x06002E61 RID: 11873 RVA: 0x00103868 File Offset: 0x00101A68
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06002E62 RID: 11874 RVA: 0x00103878 File Offset: 0x00101A78
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.RefreshReachableCells();
		this.wasOn = this.switchedOn;
		Vector2I vector2I = Grid.CellToXY(this.NaturalBuildingCell());
		int cell = Grid.XYToCell(vector2I.x, vector2I.y + this.pickupRange / 2);
		CellOffset rotatedCellOffset = new CellOffset(0, this.pickupRange / 2);
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(rotatedCellOffset);
			if (Grid.IsCellOffsetValid(this.NaturalBuildingCell(), rotatedCellOffset))
			{
				cell = Grid.OffsetCell(this.NaturalBuildingCell(), rotatedCellOffset);
			}
		}
		this.pickupableExtents = new Extents(cell, this.pickupRange / 2);
		this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("DuplicantSensor.PickupablesChanged", base.gameObject, this.pickupableExtents, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupablesChanged));
		this.pickupablesDirty = true;
	}

	// Token: 0x06002E63 RID: 11875 RVA: 0x0010397A File Offset: 0x00101B7A
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
		MinionGroupProber.Get().ReleaseProber(this);
		base.OnCleanUp();
	}

	// Token: 0x06002E64 RID: 11876 RVA: 0x0010399E File Offset: 0x00101B9E
	public void Sim1000ms(float dt)
	{
		this.RefreshReachableCells();
	}

	// Token: 0x06002E65 RID: 11877 RVA: 0x001039A6 File Offset: 0x00101BA6
	public void Sim200ms(float dt)
	{
		this.RefreshPickupables();
	}

	// Token: 0x06002E66 RID: 11878 RVA: 0x001039B0 File Offset: 0x00101BB0
	private void RefreshReachableCells()
	{
		ListPool<int, LogicDuplicantSensor>.PooledList pooledList = ListPool<int, LogicDuplicantSensor>.Allocate(this.reachableCells);
		this.reachableCells.Clear();
		int num;
		int num2;
		Grid.CellToXY(this.NaturalBuildingCell(), out num, out num2);
		int num3 = num - this.pickupRange / 2;
		for (int i = num2; i < num2 + this.pickupRange + 1; i++)
		{
			for (int j = num3; j < num3 + this.pickupRange + 1; j++)
			{
				int num4 = Grid.XYToCell(j, i);
				CellOffset rotatedCellOffset = new CellOffset(j - num, i - num2);
				if (this.rotatable)
				{
					rotatedCellOffset = this.rotatable.GetRotatedCellOffset(rotatedCellOffset);
					if (Grid.IsCellOffsetValid(this.NaturalBuildingCell(), rotatedCellOffset))
					{
						num4 = Grid.OffsetCell(this.NaturalBuildingCell(), rotatedCellOffset);
						Vector2I vector2I = Grid.CellToXY(num4);
						if (Grid.IsValidCell(num4) && Grid.IsPhysicallyAccessible(num, num2, vector2I.x, vector2I.y, true))
						{
							this.reachableCells.Add(num4);
						}
					}
				}
				else if (Grid.IsValidCell(num4) && Grid.IsPhysicallyAccessible(num, num2, j, i, true))
				{
					this.reachableCells.Add(num4);
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06002E67 RID: 11879 RVA: 0x00103AE3 File Offset: 0x00101CE3
	public bool IsCellReachable(int cell)
	{
		return this.reachableCells.Contains(cell);
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x00103AF4 File Offset: 0x00101CF4
	private void RefreshPickupables()
	{
		if (!this.pickupablesDirty)
		{
			return;
		}
		this.duplicants.Clear();
		ListPool<ScenePartitionerEntry, LogicDuplicantSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicDuplicantSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(this.pickupableExtents.x, this.pickupableExtents.y, this.pickupableExtents.width, this.pickupableExtents.height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		int cell_a = Grid.PosToCell(this);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			int pickupableCell = this.GetPickupableCell(pickupable);
			int cellRange = Grid.GetCellRange(cell_a, pickupableCell);
			if (this.IsPickupableRelevantToMyInterestsAndReachable(pickupable) && cellRange <= this.pickupRange)
			{
				this.duplicants.Add(pickupable);
			}
		}
		this.SetState(this.duplicants.Count > 0);
		this.pickupablesDirty = false;
	}

	// Token: 0x06002E69 RID: 11881 RVA: 0x00103BD4 File Offset: 0x00101DD4
	private void OnPickupablesChanged(object data)
	{
		Pickupable pickupable = data as Pickupable;
		if (pickupable && this.IsPickupableRelevantToMyInterests(pickupable))
		{
			this.pickupablesDirty = true;
		}
	}

	// Token: 0x06002E6A RID: 11882 RVA: 0x00103C00 File Offset: 0x00101E00
	private bool IsPickupableRelevantToMyInterests(Pickupable pickupable)
	{
		return pickupable.KPrefabID.HasTag(GameTags.DupeBrain);
	}

	// Token: 0x06002E6B RID: 11883 RVA: 0x00103C18 File Offset: 0x00101E18
	private bool IsPickupableRelevantToMyInterestsAndReachable(Pickupable pickupable)
	{
		if (!this.IsPickupableRelevantToMyInterests(pickupable))
		{
			return false;
		}
		int pickupableCell = this.GetPickupableCell(pickupable);
		return this.IsCellReachable(pickupableCell);
	}

	// Token: 0x06002E6C RID: 11884 RVA: 0x00103C44 File Offset: 0x00101E44
	private int GetPickupableCell(Pickupable pickupable)
	{
		return pickupable.cachedCell;
	}

	// Token: 0x06002E6D RID: 11885 RVA: 0x00103C4C File Offset: 0x00101E4C
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002E6E RID: 11886 RVA: 0x00103C5B File Offset: 0x00101E5B
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002E6F RID: 11887 RVA: 0x00103C7C File Offset: 0x00101E7C
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002E70 RID: 11888 RVA: 0x00103D04 File Offset: 0x00101F04
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001B0D RID: 6925
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04001B0E RID: 6926
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001B0F RID: 6927
	public int pickupRange = 4;

	// Token: 0x04001B10 RID: 6928
	private bool wasOn;

	// Token: 0x04001B11 RID: 6929
	private List<Pickupable> duplicants = new List<Pickupable>();

	// Token: 0x04001B12 RID: 6930
	private HandleVector<int>.Handle pickupablesChangedEntry;

	// Token: 0x04001B13 RID: 6931
	private bool pickupablesDirty;

	// Token: 0x04001B14 RID: 6932
	private Extents pickupableExtents;

	// Token: 0x04001B15 RID: 6933
	private List<int> reachableCells = new List<int>(100);
}

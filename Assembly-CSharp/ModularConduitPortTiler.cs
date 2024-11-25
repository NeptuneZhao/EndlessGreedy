using System;
using UnityEngine;

// Token: 0x02000737 RID: 1847
public class ModularConduitPortTiler : KMonoBehaviour
{
	// Token: 0x0600310D RID: 12557 RVA: 0x0010ECC4 File Offset: 0x0010CEC4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.ModularConduitPort, true);
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				GameTags.ModularConduitPort
			};
		}
	}

	// Token: 0x0600310E RID: 12558 RVA: 0x0010ED14 File Offset: 0x0010CF14
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.leftCapDefault = new KAnimSynchronizedController(component2, (Grid.SceneLayer)(component2.GetLayer() + this.leftCapDefaultSceneLayerAdjust), ModularConduitPortTiler.leftCapDefaultStr);
		if (this.manageLeftCap)
		{
			this.leftCapLaunchpad = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.leftCapLaunchpadStr);
			this.leftCapConduit = new KAnimSynchronizedController(component2, component2.GetLayer() + Grid.SceneLayer.Backwall, ModularConduitPortTiler.leftCapConduitStr);
		}
		this.rightCapDefault = new KAnimSynchronizedController(component2, (Grid.SceneLayer)(component2.GetLayer() + this.rightCapDefaultSceneLayerAdjust), ModularConduitPortTiler.rightCapDefaultStr);
		if (this.manageRightCap)
		{
			this.rightCapLaunchpad = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.rightCapLaunchpadStr);
			this.rightCapConduit = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.rightCapConduitStr);
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y, this.extents.width + 2, this.extents.height);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ModularConduitPort.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		this.UpdateEndCaps();
		this.CorrectAdjacentLaunchPads();
	}

	// Token: 0x0600310F RID: 12559 RVA: 0x0010EE6A File Offset: 0x0010D06A
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003110 RID: 12560 RVA: 0x0010EE84 File Offset: 0x0010D084
	private void UpdateEndCaps()
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int cellLeft = this.GetCellLeft();
		int cellRight = this.GetCellRight();
		if (Grid.IsValidCell(cellLeft))
		{
			if (this.HasTileableNeighbour(cellLeft))
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Conduit;
			}
			else if (this.HasLaunchpadNeighbour(cellLeft))
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Launchpad;
			}
			else
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Default;
			}
		}
		if (Grid.IsValidCell(cellRight))
		{
			if (this.HasTileableNeighbour(cellRight))
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Conduit;
			}
			else if (this.HasLaunchpadNeighbour(cellRight))
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Launchpad;
			}
			else
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Default;
			}
		}
		if (this.manageLeftCap)
		{
			this.leftCapDefault.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Default);
			this.leftCapConduit.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
			this.leftCapLaunchpad.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
		}
		if (this.manageRightCap)
		{
			this.rightCapDefault.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Default);
			this.rightCapConduit.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
			this.rightCapLaunchpad.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
		}
	}

	// Token: 0x06003111 RID: 12561 RVA: 0x0010EF9C File Offset: 0x0010D19C
	private int GetCellLeft()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(this.extents.x - num - 1, 0);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06003112 RID: 12562 RVA: 0x0010EFD8 File Offset: 0x0010D1D8
	private int GetCellRight()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(this.extents.x - num + this.extents.width, 0);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x0010F01C File Offset: 0x0010D21C
	private bool HasTileableNeighbour(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.HasAnyTags(this.tags))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x0010F068 File Offset: 0x0010D268
	private bool HasLaunchpadNeighbour(int neighbour_cell)
	{
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		return gameObject != null && gameObject.GetComponent<LaunchPad>() != null;
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x0010F0A1 File Offset: 0x0010D2A1
	private void OnNeighbourCellsUpdated(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		if (this.partitionerEntry.IsValid())
		{
			this.UpdateEndCaps();
		}
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x0010F0D0 File Offset: 0x0010D2D0
	private void CorrectAdjacentLaunchPads()
	{
		int cellRight = this.GetCellRight();
		if (Grid.IsValidCell(cellRight) && this.HasLaunchpadNeighbour(cellRight))
		{
			Grid.Objects[cellRight, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
		}
		int cellLeft = this.GetCellLeft();
		if (Grid.IsValidCell(cellLeft) && this.HasLaunchpadNeighbour(cellLeft))
		{
			Grid.Objects[cellLeft, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
		}
	}

	// Token: 0x04001CCC RID: 7372
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001CCD RID: 7373
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04001CCE RID: 7374
	public Tag[] tags;

	// Token: 0x04001CCF RID: 7375
	public bool manageLeftCap = true;

	// Token: 0x04001CD0 RID: 7376
	public bool manageRightCap = true;

	// Token: 0x04001CD1 RID: 7377
	public int leftCapDefaultSceneLayerAdjust;

	// Token: 0x04001CD2 RID: 7378
	public int rightCapDefaultSceneLayerAdjust;

	// Token: 0x04001CD3 RID: 7379
	private Extents extents;

	// Token: 0x04001CD4 RID: 7380
	private ModularConduitPortTiler.AnimCapType leftCapSetting;

	// Token: 0x04001CD5 RID: 7381
	private ModularConduitPortTiler.AnimCapType rightCapSetting;

	// Token: 0x04001CD6 RID: 7382
	private static readonly string leftCapDefaultStr = "#cap_left_default";

	// Token: 0x04001CD7 RID: 7383
	private static readonly string leftCapLaunchpadStr = "#cap_left_launchpad";

	// Token: 0x04001CD8 RID: 7384
	private static readonly string leftCapConduitStr = "#cap_left_conduit";

	// Token: 0x04001CD9 RID: 7385
	private static readonly string rightCapDefaultStr = "#cap_right_default";

	// Token: 0x04001CDA RID: 7386
	private static readonly string rightCapLaunchpadStr = "#cap_right_launchpad";

	// Token: 0x04001CDB RID: 7387
	private static readonly string rightCapConduitStr = "#cap_right_conduit";

	// Token: 0x04001CDC RID: 7388
	private KAnimSynchronizedController leftCapDefault;

	// Token: 0x04001CDD RID: 7389
	private KAnimSynchronizedController leftCapLaunchpad;

	// Token: 0x04001CDE RID: 7390
	private KAnimSynchronizedController leftCapConduit;

	// Token: 0x04001CDF RID: 7391
	private KAnimSynchronizedController rightCapDefault;

	// Token: 0x04001CE0 RID: 7392
	private KAnimSynchronizedController rightCapLaunchpad;

	// Token: 0x04001CE1 RID: 7393
	private KAnimSynchronizedController rightCapConduit;

	// Token: 0x0200159A RID: 5530
	private enum AnimCapType
	{
		// Token: 0x04006D67 RID: 28007
		Default,
		// Token: 0x04006D68 RID: 28008
		Conduit,
		// Token: 0x04006D69 RID: 28009
		Launchpad
	}
}

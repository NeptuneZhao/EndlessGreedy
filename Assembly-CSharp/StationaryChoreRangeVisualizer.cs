using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005C2 RID: 1474
[AddComponentMenu("KMonoBehaviour/scripts/StationaryChoreRangeVisualizer")]
[Obsolete("Deprecated, use RangeVisualizer")]
public class StationaryChoreRangeVisualizer : KMonoBehaviour
{
	// Token: 0x06002346 RID: 9030 RVA: 0x000C4E20 File Offset: 0x000C3020
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate);
		if (this.movable)
		{
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "StationaryChoreRangeVisualizer.OnSpawn");
			base.Subscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate);
		}
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000C4E80 File Offset: 0x000C3080
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.Unsubscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate, false);
		base.Unsubscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate, false);
		this.ClearVisualizers();
		base.OnCleanUp();
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000C4ED8 File Offset: 0x000C30D8
	private void OnSelect(object data)
	{
		if ((bool)data)
		{
			SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_form", false), base.transform.position, 1f);
			this.UpdateVisualizers();
			return;
		}
		SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_disappear", false), base.transform.position, 1f);
		this.ClearVisualizers();
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x000C4F3C File Offset: 0x000C313C
	private void OnRotated(object data)
	{
		this.UpdateVisualizers();
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x000C4F44 File Offset: 0x000C3144
	private void OnCellChange()
	{
		this.UpdateVisualizers();
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000C4F4C File Offset: 0x000C314C
	private void UpdateVisualizers()
	{
		this.newCells.Clear();
		CellOffset rotatedCellOffset = this.vision_offset;
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.vision_offset);
		}
		int cell = Grid.PosToCell(base.transform.gameObject);
		int num;
		int num2;
		Grid.CellToXY(Grid.OffsetCell(cell, rotatedCellOffset), out num, out num2);
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				CellOffset rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
				if (this.rotatable)
				{
					rotatedCellOffset2 = this.rotatable.GetRotatedCellOffset(rotatedCellOffset2);
				}
				int num3 = Grid.OffsetCell(cell, rotatedCellOffset2);
				if (Grid.IsValidCell(num3))
				{
					int x;
					int y;
					Grid.CellToXY(num3, out x, out y);
					if (Grid.TestLineOfSight(num, num2, x, y, this.blocking_cb, this.blocking_tile_visible, false))
					{
						this.newCells.Add(num3);
					}
				}
			}
		}
		for (int k = this.visualizers.Count - 1; k >= 0; k--)
		{
			if (this.newCells.Contains(this.visualizers[k].cell))
			{
				this.newCells.Remove(this.visualizers[k].cell);
			}
			else
			{
				this.DestroyEffect(this.visualizers[k].controller);
				this.visualizers.RemoveAt(k);
			}
		}
		for (int l = 0; l < this.newCells.Count; l++)
		{
			KBatchedAnimController controller = this.CreateEffect(this.newCells[l]);
			this.visualizers.Add(new StationaryChoreRangeVisualizer.VisData
			{
				cell = this.newCells[l],
				controller = controller
			});
		}
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x000C513C File Offset: 0x000C333C
	private void ClearVisualizers()
	{
		for (int i = 0; i < this.visualizers.Count; i++)
		{
			this.DestroyEffect(this.visualizers[i].controller);
		}
		this.visualizers.Clear();
	}

	// Token: 0x0600234D RID: 9037 RVA: 0x000C5184 File Offset: 0x000C3384
	private KBatchedAnimController CreateEffect(int cell)
	{
		KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(StationaryChoreRangeVisualizer.AnimName, Grid.CellToPosCCC(cell, this.sceneLayer), null, false, this.sceneLayer, true);
		kbatchedAnimController.destroyOnAnimComplete = false;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController.Play(StationaryChoreRangeVisualizer.PreAnims, KAnim.PlayMode.Loop);
		return kbatchedAnimController;
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x000C51D6 File Offset: 0x000C33D6
	private void DestroyEffect(KBatchedAnimController controller)
	{
		controller.destroyOnAnimComplete = true;
		controller.Play(StationaryChoreRangeVisualizer.PostAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0400140D RID: 5133
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x0400140E RID: 5134
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x0400140F RID: 5135
	public int x;

	// Token: 0x04001410 RID: 5136
	public int y;

	// Token: 0x04001411 RID: 5137
	public int width;

	// Token: 0x04001412 RID: 5138
	public int height;

	// Token: 0x04001413 RID: 5139
	public bool movable;

	// Token: 0x04001414 RID: 5140
	public Grid.SceneLayer sceneLayer = Grid.SceneLayer.FXFront;

	// Token: 0x04001415 RID: 5141
	public CellOffset vision_offset;

	// Token: 0x04001416 RID: 5142
	public Func<int, bool> blocking_cb = new Func<int, bool>(Grid.PhysicalBlockingCB);

	// Token: 0x04001417 RID: 5143
	public bool blocking_tile_visible = true;

	// Token: 0x04001418 RID: 5144
	private static readonly string AnimName = "transferarmgrid_kanim";

	// Token: 0x04001419 RID: 5145
	private static readonly HashedString[] PreAnims = new HashedString[]
	{
		"grid_pre",
		"grid_loop"
	};

	// Token: 0x0400141A RID: 5146
	private static readonly HashedString PostAnim = "grid_pst";

	// Token: 0x0400141B RID: 5147
	private List<StationaryChoreRangeVisualizer.VisData> visualizers = new List<StationaryChoreRangeVisualizer.VisData>();

	// Token: 0x0400141C RID: 5148
	private List<int> newCells = new List<int>();

	// Token: 0x0400141D RID: 5149
	private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnSelectDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>(delegate(StationaryChoreRangeVisualizer component, object data)
	{
		component.OnSelect(data);
	});

	// Token: 0x0400141E RID: 5150
	private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>(delegate(StationaryChoreRangeVisualizer component, object data)
	{
		component.OnRotated(data);
	});

	// Token: 0x020013B8 RID: 5048
	private struct VisData
	{
		// Token: 0x040067A7 RID: 26535
		public int cell;

		// Token: 0x040067A8 RID: 26536
		public KBatchedAnimController controller;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200091A RID: 2330
public class SandboxDestroyerTool : BrushTool
{
	// Token: 0x0600439B RID: 17307 RVA: 0x0017FA91 File Offset: 0x0017DC91
	public static void DestroyInstance()
	{
		SandboxDestroyerTool.instance = null;
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x0600439C RID: 17308 RVA: 0x0017FA99 File Offset: 0x0017DC99
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x0600439D RID: 17309 RVA: 0x0017FAA5 File Offset: 0x0017DCA5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxDestroyerTool.instance = this;
		this.affectFoundation = true;
	}

	// Token: 0x0600439E RID: 17310 RVA: 0x0017FABA File Offset: 0x0017DCBA
	protected override string GetDragSound()
	{
		return "SandboxTool_Delete_Add";
	}

	// Token: 0x0600439F RID: 17311 RVA: 0x0017FAC1 File Offset: 0x0017DCC1
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060043A0 RID: 17312 RVA: 0x0017FACE File Offset: 0x0017DCCE
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	// Token: 0x060043A1 RID: 17313 RVA: 0x0017FB05 File Offset: 0x0017DD05
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x060043A2 RID: 17314 RVA: 0x0017FB20 File Offset: 0x0017DD20
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.radiusIndicatorColor));
		}
	}

	// Token: 0x060043A3 RID: 17315 RVA: 0x0017FBD8 File Offset: 0x0017DDD8
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Delete", false));
	}

	// Token: 0x060043A4 RID: 17316 RVA: 0x0017FBF1 File Offset: 0x0017DDF1
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x060043A5 RID: 17317 RVA: 0x0017FBFC File Offset: 0x0017DDFC
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.SandBoxTool, 0f, 0f, byte.MaxValue, 0, index);
		HashSetPool<GameObject, SandboxDestroyerTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxDestroyerTool>.Allocate();
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			if (Grid.PosToCell(pickupable) == cell)
			{
				pooledHashSet.Add(pickupable.gameObject);
			}
		}
		foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
		{
			if (Grid.PosToCell(buildingComplete) == cell)
			{
				pooledHashSet.Add(buildingComplete.gameObject);
			}
		}
		if (Grid.Objects[cell, 1] != null)
		{
			pooledHashSet.Add(Grid.Objects[cell, 1]);
		}
		foreach (Crop crop in Components.Crops.Items)
		{
			if (Grid.PosToCell(crop) == cell)
			{
				pooledHashSet.Add(crop.gameObject);
			}
		}
		foreach (Health health in Components.Health.Items)
		{
			if (Grid.PosToCell(health) == cell)
			{
				pooledHashSet.Add(health.gameObject);
			}
		}
		foreach (Comet comet in Components.Meteors.GetItems((int)Grid.WorldIdx[cell]))
		{
			if (!comet.IsNullOrDestroyed() && Grid.PosToCell(comet) == cell)
			{
				pooledHashSet.Add(comet.gameObject);
			}
		}
		foreach (GameObject original in pooledHashSet)
		{
			Util.KDestroyGameObject(original);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x04002C65 RID: 11365
	public static SandboxDestroyerTool instance;

	// Token: 0x04002C66 RID: 11366
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04002C67 RID: 11367
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
}

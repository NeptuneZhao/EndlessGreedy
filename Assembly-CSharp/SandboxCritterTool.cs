using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000919 RID: 2329
public class SandboxCritterTool : BrushTool
{
	// Token: 0x06004390 RID: 17296 RVA: 0x0017F885 File Offset: 0x0017DA85
	public static void DestroyInstance()
	{
		SandboxCritterTool.instance = null;
	}

	// Token: 0x06004391 RID: 17297 RVA: 0x0017F88D File Offset: 0x0017DA8D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxCritterTool.instance = this;
	}

	// Token: 0x06004392 RID: 17298 RVA: 0x0017F89B File Offset: 0x0017DA9B
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06004393 RID: 17299 RVA: 0x0017F8A2 File Offset: 0x0017DAA2
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004394 RID: 17300 RVA: 0x0017F8AF File Offset: 0x0017DAAF
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue(6f, true);
	}

	// Token: 0x06004395 RID: 17301 RVA: 0x0017F8E6 File Offset: 0x0017DAE6
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06004396 RID: 17302 RVA: 0x0017F900 File Offset: 0x0017DB00
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06004397 RID: 17303 RVA: 0x0017F968 File Offset: 0x0017DB68
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06004398 RID: 17304 RVA: 0x0017F971 File Offset: 0x0017DB71
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06004399 RID: 17305 RVA: 0x0017F98C File Offset: 0x0017DB8C
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		HashSetPool<GameObject, SandboxCritterTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxCritterTool>.Allocate();
		foreach (Health health in Components.Health.Items)
		{
			if (Grid.PosToCell(health) == cell && health.GetComponent<KPrefabID>().HasTag(GameTags.Creature))
			{
				pooledHashSet.Add(health.gameObject);
			}
		}
		foreach (GameObject gameObject in pooledHashSet)
		{
			KFMOD.PlayOneShot(this.soundPath, gameObject.gameObject.transform.GetPosition(), 1f);
			Util.KDestroyGameObject(gameObject);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x04002C63 RID: 11363
	public static SandboxCritterTool instance;

	// Token: 0x04002C64 RID: 11364
	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor", false);
}

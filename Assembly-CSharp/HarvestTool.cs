using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000910 RID: 2320
public class HarvestTool : DragTool
{
	// Token: 0x06004310 RID: 17168 RVA: 0x0017D460 File Offset: 0x0017B660
	public static void DestroyInstance()
	{
		HarvestTool.Instance = null;
	}

	// Token: 0x06004311 RID: 17169 RVA: 0x0017D468 File Offset: 0x0017B668
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		HarvestTool.Instance = this;
		this.options.Add("HARVEST_WHEN_READY", ToolParameterMenu.ToggleState.On);
		this.options.Add("DO_NOT_HARVEST", ToolParameterMenu.ToggleState.Off);
		this.viewMode = OverlayModes.Harvest.ID;
	}

	// Token: 0x06004312 RID: 17170 RVA: 0x0017D4A4 File Offset: 0x0017B6A4
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			foreach (HarvestDesignatable harvestDesignatable in Components.HarvestDesignatables.Items)
			{
				OccupyArea area = harvestDesignatable.area;
				if (Grid.PosToCell(harvestDesignatable) == cell || (area != null && area.CheckIsOccupying(cell)))
				{
					if (this.options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On)
					{
						harvestDesignatable.SetHarvestWhenReady(true);
					}
					else if (this.options["DO_NOT_HARVEST"] == ToolParameterMenu.ToggleState.On)
					{
						Harvestable component = harvestDesignatable.GetComponent<Harvestable>();
						if (component != null)
						{
							component.Trigger(2127324410, null);
						}
						harvestDesignatable.SetHarvestWhenReady(false);
					}
					Prioritizable component2 = harvestDesignatable.GetComponent<Prioritizable>();
					if (component2 != null)
					{
						component2.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x06004313 RID: 17171 RVA: 0x0017D5A4 File Offset: 0x0017B7A4
	public void Update()
	{
		MeshRenderer componentInChildren = this.visualizer.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null)
		{
			if (this.options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On)
			{
				componentInChildren.material.mainTexture = this.visualizerTextures[0];
				return;
			}
			if (this.options["DO_NOT_HARVEST"] == ToolParameterMenu.ToggleState.On)
			{
				componentInChildren.material.mainTexture = this.visualizerTextures[1];
			}
		}
	}

	// Token: 0x06004314 RID: 17172 RVA: 0x0017D611 File Offset: 0x0017B811
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
	}

	// Token: 0x06004315 RID: 17173 RVA: 0x0017D61A File Offset: 0x0017B81A
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
		ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.options);
	}

	// Token: 0x06004316 RID: 17174 RVA: 0x0017D647 File Offset: 0x0017B847
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
		ToolMenu.Instance.toolParameterMenu.ClearMenu();
	}

	// Token: 0x04002C2B RID: 11307
	public GameObject Placer;

	// Token: 0x04002C2C RID: 11308
	public static HarvestTool Instance;

	// Token: 0x04002C2D RID: 11309
	public Texture2D[] visualizerTextures;

	// Token: 0x04002C2E RID: 11310
	private Dictionary<string, ToolParameterMenu.ToggleState> options = new Dictionary<string, ToolParameterMenu.ToggleState>();
}

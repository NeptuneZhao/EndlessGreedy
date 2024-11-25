using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200091D RID: 2333
public class SandboxHeatTool : BrushTool
{
	// Token: 0x060043C2 RID: 17346 RVA: 0x00180674 File Offset: 0x0017E874
	public static void DestroyInstance()
	{
		SandboxHeatTool.instance = null;
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x060043C3 RID: 17347 RVA: 0x0018067C File Offset: 0x0017E87C
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x060043C4 RID: 17348 RVA: 0x00180688 File Offset: 0x0017E888
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxHeatTool.instance = this;
		this.viewMode = OverlayModes.Temperature.ID;
	}

	// Token: 0x060043C5 RID: 17349 RVA: 0x001806A1 File Offset: 0x0017E8A1
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x060043C6 RID: 17350 RVA: 0x001806A8 File Offset: 0x0017E8A8
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060043C7 RID: 17351 RVA: 0x001806B8 File Offset: 0x0017E8B8
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureAdditiveSlider.row.SetActive(true);
	}

	// Token: 0x060043C8 RID: 17352 RVA: 0x0018070F File Offset: 0x0017E90F
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x060043C9 RID: 17353 RVA: 0x00180728 File Offset: 0x0017E928
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

	// Token: 0x060043CA RID: 17354 RVA: 0x001807E0 File Offset: 0x0017E9E0
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x060043CB RID: 17355 RVA: 0x001807EC File Offset: 0x0017E9EC
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		if (this.recentlyAffectedCells.Contains(cell))
		{
			return;
		}
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		float num = Grid.Temperature[cell];
		num += SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.TemperatureAdditive");
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		if (temperatureUnit != GameUtil.TemperatureUnit.Celsius)
		{
			if (temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
			{
				num -= 255.372f;
			}
		}
		else
		{
			num -= 273.15f;
		}
		num = Mathf.Clamp(num, 1f, 9999f);
		int cell2 = cell;
		SimHashes id = Grid.Element[cell].id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float mass = Grid.Mass[cell];
		float temperature = num;
		int callbackIdx = index;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, mass, temperature, Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell], callbackIdx);
		float currentValue = SandboxToolParameterMenu.instance.temperatureAdditiveSlider.inputField.currentValue;
		KFMOD.PlayUISoundWithLabeledParameter(GlobalAssets.GetSound("SandboxTool_HeatGun", false), "TemperatureSetting", (currentValue <= 0f) ? "Cooling" : "Heating");
	}

	// Token: 0x04002C71 RID: 11377
	public static SandboxHeatTool instance;

	// Token: 0x04002C72 RID: 11378
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04002C73 RID: 11379
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
}

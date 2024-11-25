using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200091B RID: 2331
public class SandboxFOWTool : BrushTool
{
	// Token: 0x060043A7 RID: 17319 RVA: 0x0017FF2E File Offset: 0x0017E12E
	public static void DestroyInstance()
	{
		SandboxFOWTool.instance = null;
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x060043A8 RID: 17320 RVA: 0x0017FF36 File Offset: 0x0017E136
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x060043A9 RID: 17321 RVA: 0x0017FF42 File Offset: 0x0017E142
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxFOWTool.instance = this;
	}

	// Token: 0x060043AA RID: 17322 RVA: 0x0017FF50 File Offset: 0x0017E150
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x060043AB RID: 17323 RVA: 0x0017FF57 File Offset: 0x0017E157
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060043AC RID: 17324 RVA: 0x0017FF64 File Offset: 0x0017E164
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	// Token: 0x060043AD RID: 17325 RVA: 0x0017FF9B File Offset: 0x0017E19B
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.ev.release();
	}

	// Token: 0x060043AE RID: 17326 RVA: 0x0017FFC0 File Offset: 0x0017E1C0
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

	// Token: 0x060043AF RID: 17327 RVA: 0x00180078 File Offset: 0x0017E278
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x060043B0 RID: 17328 RVA: 0x00180081 File Offset: 0x0017E281
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		Grid.Reveal(cell, byte.MaxValue, true);
	}

	// Token: 0x060043B1 RID: 17329 RVA: 0x00180098 File Offset: 0x0017E298
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		int intSetting = this.settings.GetIntSetting("SandboxTools.BrushSize");
		this.ev = KFMOD.CreateInstance(GlobalAssets.GetSound("SandboxTool_Reveal", false));
		this.ev.setParameterByName("BrushSize", (float)intSetting, false);
		this.ev.start();
	}

	// Token: 0x060043B2 RID: 17330 RVA: 0x001800F3 File Offset: 0x0017E2F3
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
		this.ev.stop(STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x04002C68 RID: 11368
	public static SandboxFOWTool instance;

	// Token: 0x04002C69 RID: 11369
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04002C6A RID: 11370
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x04002C6B RID: 11371
	private EventInstance ev;
}

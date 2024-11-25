using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000918 RID: 2328
public class SandboxClearFloorTool : BrushTool
{
	// Token: 0x06004384 RID: 17284 RVA: 0x0017F60D File Offset: 0x0017D80D
	public static void DestroyInstance()
	{
		SandboxClearFloorTool.instance = null;
	}

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06004385 RID: 17285 RVA: 0x0017F615 File Offset: 0x0017D815
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06004386 RID: 17286 RVA: 0x0017F621 File Offset: 0x0017D821
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxClearFloorTool.instance = this;
	}

	// Token: 0x06004387 RID: 17287 RVA: 0x0017F62F File Offset: 0x0017D82F
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06004388 RID: 17288 RVA: 0x0017F636 File Offset: 0x0017D836
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004389 RID: 17289 RVA: 0x0017F644 File Offset: 0x0017D844
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.BrushSize"), true);
	}

	// Token: 0x0600438A RID: 17290 RVA: 0x0017F6A7 File Offset: 0x0017D8A7
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x0600438B RID: 17291 RVA: 0x0017F6C0 File Offset: 0x0017D8C0
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x0600438C RID: 17292 RVA: 0x0017F728 File Offset: 0x0017D928
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x0600438D RID: 17293 RVA: 0x0017F731 File Offset: 0x0017D931
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x0600438E RID: 17294 RVA: 0x0017F74C File Offset: 0x0017D94C
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		bool flag = false;
		using (List<Pickupable>.Enumerator enumerator = Components.Pickupables.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Pickupable pickup = enumerator.Current;
				if (!(pickup.storage != null) && Grid.PosToCell(pickup) == cell && Components.LiveMinionIdentities.Items.Find((MinionIdentity match) => match.gameObject == pickup.gameObject) == null)
				{
					if (!flag)
					{
						KFMOD.PlayOneShot(this.soundPath, pickup.gameObject.transform.GetPosition(), 1f);
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.SANDBOXTOOLS.CLEARFLOOR.DELETED, pickup.transform, 1.5f, false);
						flag = true;
					}
					Util.KDestroyGameObject(pickup.gameObject);
				}
			}
		}
	}

	// Token: 0x04002C61 RID: 11361
	public static SandboxClearFloorTool instance;

	// Token: 0x04002C62 RID: 11362
	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor", false);
}

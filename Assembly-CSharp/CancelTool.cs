using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000902 RID: 2306
public class CancelTool : FilteredDragTool
{
	// Token: 0x06004279 RID: 17017 RVA: 0x0017A7FB File Offset: 0x001789FB
	public static void DestroyInstance()
	{
		CancelTool.Instance = null;
	}

	// Token: 0x0600427A RID: 17018 RVA: 0x0017A803 File Offset: 0x00178A03
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CancelTool.Instance = this;
	}

	// Token: 0x0600427B RID: 17019 RVA: 0x0017A811 File Offset: 0x00178A11
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		base.GetDefaultFilters(filters);
		filters.Add(ToolParameterMenu.FILTERLAYERS.CLEANANDCLEAR, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.DIGPLACER, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x0600427C RID: 17020 RVA: 0x0017A832 File Offset: 0x00178A32
	protected override string GetConfirmSound()
	{
		return "Tile_Confirm_NegativeTool";
	}

	// Token: 0x0600427D RID: 17021 RVA: 0x0017A839 File Offset: 0x00178A39
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x0600427E RID: 17022 RVA: 0x0017A840 File Offset: 0x00178A40
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
				if (base.IsActiveLayer(filterLayerFromGameObject))
				{
					gameObject.Trigger(2127324410, null);
				}
			}
		}
	}

	// Token: 0x0600427F RID: 17023 RVA: 0x0017A890 File Offset: 0x00178A90
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		AttackTool.MarkForAttack(regularizedPos, regularizedPos2, false);
		CaptureTool.MarkForCapture(regularizedPos, regularizedPos2, false);
	}

	// Token: 0x04002BFB RID: 11259
	public static CancelTool Instance;
}

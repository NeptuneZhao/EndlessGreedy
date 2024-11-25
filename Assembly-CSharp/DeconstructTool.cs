using System;
using UnityEngine;

// Token: 0x02000908 RID: 2312
public class DeconstructTool : FilteredDragTool
{
	// Token: 0x060042AD RID: 17069 RVA: 0x0017B6BD File Offset: 0x001798BD
	public static void DestroyInstance()
	{
		DeconstructTool.Instance = null;
	}

	// Token: 0x060042AE RID: 17070 RVA: 0x0017B6C5 File Offset: 0x001798C5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DeconstructTool.Instance = this;
	}

	// Token: 0x060042AF RID: 17071 RVA: 0x0017B6D3 File Offset: 0x001798D3
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060042B0 RID: 17072 RVA: 0x0017B6E0 File Offset: 0x001798E0
	protected override string GetConfirmSound()
	{
		return "Tile_Confirm_NegativeTool";
	}

	// Token: 0x060042B1 RID: 17073 RVA: 0x0017B6E7 File Offset: 0x001798E7
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x060042B2 RID: 17074 RVA: 0x0017B6EE File Offset: 0x001798EE
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		this.DeconstructCell(cell);
	}

	// Token: 0x060042B3 RID: 17075 RVA: 0x0017B6F8 File Offset: 0x001798F8
	public void DeconstructCell(int cell)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
				if (base.IsActiveLayer(filterLayerFromGameObject))
				{
					gameObject.Trigger(-790448070, null);
					Prioritizable component = gameObject.GetComponent<Prioritizable>();
					if (component != null)
					{
						component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x060042B4 RID: 17076 RVA: 0x0017B76A File Offset: 0x0017996A
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x060042B5 RID: 17077 RVA: 0x0017B782 File Offset: 0x00179982
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04002C07 RID: 11271
	public static DeconstructTool Instance;
}

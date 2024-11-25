using System;
using UnityEngine;

// Token: 0x02000904 RID: 2308
public class ClearTool : DragTool
{
	// Token: 0x06004286 RID: 17030 RVA: 0x0017AA69 File Offset: 0x00178C69
	public static void DestroyInstance()
	{
		ClearTool.Instance = null;
	}

	// Token: 0x06004287 RID: 17031 RVA: 0x0017AA71 File Offset: 0x00178C71
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClearTool.Instance = this;
		this.interceptNumberKeysForPriority = true;
	}

	// Token: 0x06004288 RID: 17032 RVA: 0x0017AA86 File Offset: 0x00178C86
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004289 RID: 17033 RVA: 0x0017AA94 File Offset: 0x00178C94
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		GameObject gameObject = Grid.Objects[cell, 3];
		if (gameObject == null)
		{
			return;
		}
		ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
		while (objectLayerListItem != null)
		{
			GameObject gameObject2 = objectLayerListItem.gameObject;
			objectLayerListItem = objectLayerListItem.nextItem;
			if (!(gameObject2 == null) && !(gameObject2.GetComponent<MinionIdentity>() != null) && gameObject2.GetComponent<Clearable>().isClearable)
			{
				gameObject2.GetComponent<Clearable>().MarkForClear(false, false);
				Prioritizable component = gameObject2.GetComponent<Prioritizable>();
				if (component != null)
				{
					component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
				}
			}
		}
	}

	// Token: 0x0600428A RID: 17034 RVA: 0x0017AB2D File Offset: 0x00178D2D
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x0600428B RID: 17035 RVA: 0x0017AB45 File Offset: 0x00178D45
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04002BFC RID: 11260
	public static ClearTool Instance;
}

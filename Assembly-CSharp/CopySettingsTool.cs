using System;
using UnityEngine;

// Token: 0x02000906 RID: 2310
public class CopySettingsTool : DragTool
{
	// Token: 0x06004299 RID: 17049 RVA: 0x0017ADE8 File Offset: 0x00178FE8
	public static void DestroyInstance()
	{
		CopySettingsTool.Instance = null;
	}

	// Token: 0x0600429A RID: 17050 RVA: 0x0017ADF0 File Offset: 0x00178FF0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CopySettingsTool.Instance = this;
	}

	// Token: 0x0600429B RID: 17051 RVA: 0x0017ADFE File Offset: 0x00178FFE
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x0600429C RID: 17052 RVA: 0x0017AE0B File Offset: 0x0017900B
	public void SetSourceObject(GameObject sourceGameObject)
	{
		this.sourceGameObject = sourceGameObject;
	}

	// Token: 0x0600429D RID: 17053 RVA: 0x0017AE14 File Offset: 0x00179014
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.sourceGameObject == null)
		{
			return;
		}
		if (Grid.IsValidCell(cell))
		{
			CopyBuildingSettings.ApplyCopy(cell, this.sourceGameObject);
		}
	}

	// Token: 0x0600429E RID: 17054 RVA: 0x0017AE3A File Offset: 0x0017903A
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
	}

	// Token: 0x0600429F RID: 17055 RVA: 0x0017AE42 File Offset: 0x00179042
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		this.sourceGameObject = null;
	}

	// Token: 0x04002C02 RID: 11266
	public static CopySettingsTool Instance;

	// Token: 0x04002C03 RID: 11267
	public GameObject Placer;

	// Token: 0x04002C04 RID: 11268
	private GameObject sourceGameObject;
}

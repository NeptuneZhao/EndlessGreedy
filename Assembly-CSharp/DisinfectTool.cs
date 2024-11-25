using System;
using UnityEngine;

// Token: 0x0200090B RID: 2315
public class DisinfectTool : DragTool
{
	// Token: 0x060042D1 RID: 17105 RVA: 0x0017BF13 File Offset: 0x0017A113
	public static void DestroyInstance()
	{
		DisinfectTool.Instance = null;
	}

	// Token: 0x060042D2 RID: 17106 RVA: 0x0017BF1B File Offset: 0x0017A11B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DisinfectTool.Instance = this;
		this.interceptNumberKeysForPriority = true;
		this.viewMode = OverlayModes.Disease.ID;
	}

	// Token: 0x060042D3 RID: 17107 RVA: 0x0017BF3B File Offset: 0x0017A13B
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060042D4 RID: 17108 RVA: 0x0017BF48 File Offset: 0x0017A148
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				Disinfectable component = gameObject.GetComponent<Disinfectable>();
				if (component != null && component.GetComponent<PrimaryElement>().DiseaseCount > 0)
				{
					component.MarkForDisinfect(false);
				}
			}
		}
	}

	// Token: 0x04002C10 RID: 11280
	public static DisinfectTool Instance;
}

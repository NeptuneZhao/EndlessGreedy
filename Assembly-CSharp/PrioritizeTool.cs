using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000916 RID: 2326
public class PrioritizeTool : FilteredDragTool
{
	// Token: 0x06004369 RID: 17257 RVA: 0x0017EC4C File Offset: 0x0017CE4C
	public static void DestroyInstance()
	{
		PrioritizeTool.Instance = null;
	}

	// Token: 0x0600436A RID: 17258 RVA: 0x0017EC54 File Offset: 0x0017CE54
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.interceptNumberKeysForPriority = true;
		PrioritizeTool.Instance = this;
		this.visualizer = Util.KInstantiate(this.visualizer, null, null);
		this.viewMode = OverlayModes.Priorities.ID;
		Game.Instance.prioritizableRenderer.currentTool = this;
	}

	// Token: 0x0600436B RID: 17259 RVA: 0x0017ECA4 File Offset: 0x0017CEA4
	public override string GetFilterLayerFromGameObject(GameObject input)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (input.GetComponent<Diggable>())
		{
			flag = true;
		}
		if (input.GetComponent<Constructable>() || (input.GetComponent<Deconstructable>() && input.GetComponent<Deconstructable>().IsMarkedForDeconstruction()))
		{
			flag2 = true;
		}
		if (input.GetComponent<Clearable>() || input.GetComponent<Moppable>() || input.GetComponent<StorageLocker>())
		{
			flag3 = true;
		}
		if (flag2)
		{
			return ToolParameterMenu.FILTERLAYERS.CONSTRUCTION;
		}
		if (flag)
		{
			return ToolParameterMenu.FILTERLAYERS.DIG;
		}
		if (flag3)
		{
			return ToolParameterMenu.FILTERLAYERS.CLEAN;
		}
		return ToolParameterMenu.FILTERLAYERS.OPERATE;
	}

	// Token: 0x0600436C RID: 17260 RVA: 0x0017ED38 File Offset: 0x0017CF38
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.CONSTRUCTION, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.DIG, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.CLEAN, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.OPERATE, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x0600436D RID: 17261 RVA: 0x0017ED78 File Offset: 0x0017CF78
	private bool TryPrioritizeGameObject(GameObject target, PrioritySetting priority)
	{
		string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(target);
		if (base.IsActiveLayer(filterLayerFromGameObject))
		{
			Prioritizable component = target.GetComponent<Prioritizable>();
			if (component != null && component.showIcon && component.IsPrioritizable())
			{
				component.SetMasterPriority(priority);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600436E RID: 17262 RVA: 0x0017EDC0 File Offset: 0x0017CFC0
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		PrioritySetting lastSelectedPriority = ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority();
		int num = 0;
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				if (gameObject.GetComponent<Pickupable>())
				{
					ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
					while (objectLayerListItem != null)
					{
						GameObject gameObject2 = objectLayerListItem.gameObject;
						objectLayerListItem = objectLayerListItem.nextItem;
						if (!(gameObject2 == null) && !(gameObject2.GetComponent<MinionIdentity>() != null) && this.TryPrioritizeGameObject(gameObject2, lastSelectedPriority))
						{
							num++;
						}
					}
				}
				else if (this.TryPrioritizeGameObject(gameObject, lastSelectedPriority))
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			PriorityScreen.PlayPriorityConfirmSound(lastSelectedPriority);
		}
	}

	// Token: 0x0600436F RID: 17263 RVA: 0x0017EE7C File Offset: 0x0017D07C
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.ShowDiagram(true);
		ToolMenu.Instance.PriorityScreen.Show(true);
		ToolMenu.Instance.PriorityScreen.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
	}

	// Token: 0x06004370 RID: 17264 RVA: 0x0017EED8 File Offset: 0x0017D0D8
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
		ToolMenu.Instance.PriorityScreen.ShowDiagram(false);
		ToolMenu.Instance.PriorityScreen.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x06004371 RID: 17265 RVA: 0x0017EF34 File Offset: 0x0017D134
	public void Update()
	{
		PrioritySetting lastSelectedPriority = ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority();
		int num = 0;
		if (lastSelectedPriority.priority_class >= PriorityScreen.PriorityClass.high)
		{
			num += 9;
		}
		if (lastSelectedPriority.priority_class >= PriorityScreen.PriorityClass.topPriority)
		{
			num = num;
		}
		num += lastSelectedPriority.priority_value;
		Texture2D mainTexture = this.cursors[num - 1];
		MeshRenderer componentInChildren = this.visualizer.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null)
		{
			componentInChildren.material.mainTexture = mainTexture;
		}
	}

	// Token: 0x04002C5A RID: 11354
	public GameObject Placer;

	// Token: 0x04002C5B RID: 11355
	public static PrioritizeTool Instance;

	// Token: 0x04002C5C RID: 11356
	public Texture2D[] cursors;
}

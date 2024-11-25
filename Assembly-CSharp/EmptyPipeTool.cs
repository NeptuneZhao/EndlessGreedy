using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200090D RID: 2317
public class EmptyPipeTool : FilteredDragTool
{
	// Token: 0x060042F6 RID: 17142 RVA: 0x0017CCD0 File Offset: 0x0017AED0
	public static void DestroyInstance()
	{
		EmptyPipeTool.Instance = null;
	}

	// Token: 0x060042F7 RID: 17143 RVA: 0x0017CCD8 File Offset: 0x0017AED8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		EmptyPipeTool.Instance = this;
	}

	// Token: 0x060042F8 RID: 17144 RVA: 0x0017CCE8 File Offset: 0x0017AEE8
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			if (base.IsActiveLayer((ObjectLayer)i))
			{
				GameObject gameObject = Grid.Objects[cell, i];
				if (!(gameObject == null))
				{
					IEmptyConduitWorkable component = gameObject.GetComponent<IEmptyConduitWorkable>();
					if (!component.IsNullOrDestroyed())
					{
						if (DebugHandler.InstantBuildMode)
						{
							component.EmptyContents();
						}
						else
						{
							component.MarkForEmptying();
							Prioritizable component2 = gameObject.GetComponent<Prioritizable>();
							if (component2 != null)
							{
								component2.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060042F9 RID: 17145 RVA: 0x0017CD6A File Offset: 0x0017AF6A
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x060042FA RID: 17146 RVA: 0x0017CD82 File Offset: 0x0017AF82
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x060042FB RID: 17147 RVA: 0x0017CD9B File Offset: 0x0017AF9B
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x04002C22 RID: 11298
	public static EmptyPipeTool Instance;
}

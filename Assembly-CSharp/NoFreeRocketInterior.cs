using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A90 RID: 2704
public class NoFreeRocketInterior : SelectModuleCondition
{
	// Token: 0x06004F4B RID: 20299 RVA: 0x001C8428 File Offset: 0x001C6628
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		int num = 0;
		using (IEnumerator<WorldContainer> enumerator = ClusterManager.Instance.WorldContainers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsModuleInterior)
				{
					num++;
				}
			}
		}
		return num < ClusterManager.MAX_ROCKET_INTERIOR_COUNT;
	}

	// Token: 0x06004F4C RID: 20300 RVA: 0x001C8488 File Offset: 0x001C6688
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.PASSENGER_MODULE_AVAILABLE.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.PASSENGER_MODULE_AVAILABLE.FAILED;
	}
}

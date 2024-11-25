using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A8A RID: 2698
public class LimitOneCommandModule : SelectModuleCondition
{
	// Token: 0x06004F39 RID: 20281 RVA: 0x001C7DDC File Offset: 0x001C5FDC
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null)
		{
			return true;
		}
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(existingModule.GetComponent<AttachableBuilding>()))
		{
			if (selectionContext != SelectModuleCondition.SelectionContext.ReplaceModule || !(gameObject == existingModule.gameObject))
			{
				if (gameObject.GetComponent<RocketCommandConditions>() != null)
				{
					return false;
				}
				if (gameObject.GetComponent<BuildingUnderConstruction>() != null && gameObject.GetComponent<BuildingUnderConstruction>().Def.BuildingComplete.GetComponent<RocketCommandConditions>() != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06004F3A RID: 20282 RVA: 0x001C7E90 File Offset: 0x001C6090
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_COMMAND_PER_ROCKET.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_COMMAND_PER_ROCKET.FAILED;
	}
}

using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A8C RID: 2700
public class EngineOnBottom : SelectModuleCondition
{
	// Token: 0x06004F3F RID: 20287 RVA: 0x001C7F8C File Offset: 0x001C618C
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null || existingModule.GetComponent<LaunchPad>() != null)
		{
			return true;
		}
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule)
		{
			return existingModule.GetComponent<AttachableBuilding>().GetAttachedTo() == null;
		}
		return selectionContext == SelectModuleCondition.SelectionContext.AddModuleBelow && existingModule.GetComponent<AttachableBuilding>().GetAttachedTo() == null;
	}

	// Token: 0x06004F40 RID: 20288 RVA: 0x001C7FE9 File Offset: 0x001C61E9
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ENGINE_AT_BOTTOM.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ENGINE_AT_BOTTOM.FAILED;
	}
}

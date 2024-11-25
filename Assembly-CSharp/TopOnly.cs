using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A8D RID: 2701
public class TopOnly : SelectModuleCondition
{
	// Token: 0x06004F42 RID: 20290 RVA: 0x001C800C File Offset: 0x001C620C
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		global::Debug.Assert(existingModule != null, "Existing module is null in top only condition");
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule)
		{
			global::Debug.Assert(existingModule.GetComponent<LaunchPad>() == null, "Trying to replace launch pad with rocket module");
			return existingModule.GetComponent<BuildingAttachPoint>() == null || existingModule.GetComponent<BuildingAttachPoint>().points[0].attachedBuilding == null;
		}
		return existingModule.GetComponent<LaunchPad>() != null || (existingModule.GetComponent<BuildingAttachPoint>() != null && existingModule.GetComponent<BuildingAttachPoint>().points[0].attachedBuilding == null);
	}

	// Token: 0x06004F43 RID: 20291 RVA: 0x001C80AD File Offset: 0x001C62AD
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.TOP_ONLY.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.TOP_ONLY.FAILED;
	}
}

using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000A88 RID: 2696
public class ResearchCompleted : SelectModuleCondition
{
	// Token: 0x06004F31 RID: 20273 RVA: 0x001C7C98 File Offset: 0x001C5E98
	public override bool IgnoreInSanboxMode()
	{
		return true;
	}

	// Token: 0x06004F32 RID: 20274 RVA: 0x001C7C9C File Offset: 0x001C5E9C
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null)
		{
			return true;
		}
		TechItem techItem = Db.Get().TechItems.TryGet(selectedPart.PrefabID);
		return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
	}

	// Token: 0x06004F33 RID: 20275 RVA: 0x001C7CE8 File Offset: 0x001C5EE8
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.RESEARCHED.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.RESEARCHED.FAILED;
	}
}

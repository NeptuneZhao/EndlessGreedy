using System;
using UnityEngine;

// Token: 0x02000A87 RID: 2695
public abstract class SelectModuleCondition
{
	// Token: 0x06004F2D RID: 20269
	public abstract bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext);

	// Token: 0x06004F2E RID: 20270
	public abstract string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart);

	// Token: 0x06004F2F RID: 20271 RVA: 0x001C7C8D File Offset: 0x001C5E8D
	public virtual bool IgnoreInSanboxMode()
	{
		return false;
	}

	// Token: 0x02001ABD RID: 6845
	public enum SelectionContext
	{
		// Token: 0x04007D86 RID: 32134
		AddModuleAbove,
		// Token: 0x04007D87 RID: 32135
		AddModuleBelow,
		// Token: 0x04007D88 RID: 32136
		ReplaceModule
	}
}

using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000AF7 RID: 2807
public class CargoBayIsEmpty : ProcessCondition
{
	// Token: 0x060053CE RID: 21454 RVA: 0x001E08E4 File Offset: 0x001DEAE4
	public CargoBayIsEmpty(CommandModule module)
	{
		this.commandModule = module;
	}

	// Token: 0x060053CF RID: 21455 RVA: 0x001E08F4 File Offset: 0x001DEAF4
	public override ProcessCondition.Status EvaluateCondition()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			CargoBay component = gameObject.GetComponent<CargoBay>();
			if (component != null && component.storage.MassStored() != 0f)
			{
				return ProcessCondition.Status.Failure;
			}
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x060053D0 RID: 21456 RVA: 0x001E0974 File Offset: 0x001DEB74
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return UI.STARMAP.CARGOEMPTY.NAME;
	}

	// Token: 0x060053D1 RID: 21457 RVA: 0x001E0980 File Offset: 0x001DEB80
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return UI.STARMAP.CARGOEMPTY.TOOLTIP;
	}

	// Token: 0x060053D2 RID: 21458 RVA: 0x001E098C File Offset: 0x001DEB8C
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003728 RID: 14120
	private CommandModule commandModule;
}

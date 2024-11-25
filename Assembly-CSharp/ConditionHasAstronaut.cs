using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000AFB RID: 2811
public class ConditionHasAstronaut : ProcessCondition
{
	// Token: 0x060053EC RID: 21484 RVA: 0x001E12CB File Offset: 0x001DF4CB
	public ConditionHasAstronaut(CommandModule module)
	{
		this.module = module;
	}

	// Token: 0x060053ED RID: 21485 RVA: 0x001E12DC File Offset: 0x001DF4DC
	public override ProcessCondition.Status EvaluateCondition()
	{
		List<MinionStorage.Info> storedMinionInfo = this.module.GetComponent<MinionStorage>().GetStoredMinionInfo();
		if (storedMinionInfo.Count > 0 && storedMinionInfo[0].serializedMinion != null)
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x060053EE RID: 21486 RVA: 0x001E1314 File Offset: 0x001DF514
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUT_TITLE;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
	}

	// Token: 0x060053EF RID: 21487 RVA: 0x001E132F File Offset: 0x001DF52F
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HASASTRONAUT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
	}

	// Token: 0x060053F0 RID: 21488 RVA: 0x001E134A File Offset: 0x001DF54A
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003733 RID: 14131
	private CommandModule module;
}

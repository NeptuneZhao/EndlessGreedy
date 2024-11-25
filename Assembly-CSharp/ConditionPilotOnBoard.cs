using System;
using STRINGS;

// Token: 0x02000B06 RID: 2822
public class ConditionPilotOnBoard : ProcessCondition
{
	// Token: 0x06005425 RID: 21541 RVA: 0x001E201F File Offset: 0x001E021F
	public ConditionPilotOnBoard(PassengerRocketModule module)
	{
		this.module = module;
		this.rocketModule = module.GetComponent<RocketModuleCluster>();
	}

	// Token: 0x06005426 RID: 21542 RVA: 0x001E203A File Offset: 0x001E023A
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.CheckPilotBoarded())
		{
			return ProcessCondition.Status.Ready;
		}
		if (this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005427 RID: 21543 RVA: 0x001E2068 File Offset: 0x001E0268
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.READY;
		}
		if (status == ProcessCondition.Status.Warning && this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.ROBO_PILOT_WARNING;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.FAILURE;
	}

	// Token: 0x06005428 RID: 21544 RVA: 0x001E20B8 File Offset: 0x001E02B8
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.READY;
		}
		if (status == ProcessCondition.Status.Warning && this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.ROBO_PILOT_WARNING;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.FAILURE;
	}

	// Token: 0x06005429 RID: 21545 RVA: 0x001E2105 File Offset: 0x001E0305
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003740 RID: 14144
	private PassengerRocketModule module;

	// Token: 0x04003741 RID: 14145
	private RocketModuleCluster rocketModule;
}

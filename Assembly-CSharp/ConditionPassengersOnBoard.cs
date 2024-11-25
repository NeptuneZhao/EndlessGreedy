using System;
using STRINGS;

// Token: 0x02000B05 RID: 2821
public class ConditionPassengersOnBoard : ProcessCondition
{
	// Token: 0x06005420 RID: 21536 RVA: 0x001E1EFF File Offset: 0x001E00FF
	public ConditionPassengersOnBoard(PassengerRocketModule module)
	{
		this.module = module;
	}

	// Token: 0x06005421 RID: 21537 RVA: 0x001E1F10 File Offset: 0x001E0110
	public override ProcessCondition.Status EvaluateCondition()
	{
		global::Tuple<int, int> crewBoardedFraction = this.module.GetCrewBoardedFraction();
		if (crewBoardedFraction.first != crewBoardedFraction.second)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005422 RID: 21538 RVA: 0x001E1F3A File Offset: 0x001E013A
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.FAILURE;
	}

	// Token: 0x06005423 RID: 21539 RVA: 0x001E1F58 File Offset: 0x001E0158
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		global::Tuple<int, int> crewBoardedFraction = this.module.GetCrewBoardedFraction();
		if (status == ProcessCondition.Status.Ready)
		{
			if (crewBoardedFraction.second != 0)
			{
				return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.READY, crewBoardedFraction.first, crewBoardedFraction.second);
			}
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.NONE, crewBoardedFraction.first, crewBoardedFraction.second);
		}
		else
		{
			if (crewBoardedFraction.first == 0)
			{
				return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.FAILURE, crewBoardedFraction.first, crewBoardedFraction.second);
			}
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.WARNING, crewBoardedFraction.first, crewBoardedFraction.second);
		}
	}

	// Token: 0x06005424 RID: 21540 RVA: 0x001E201C File Offset: 0x001E021C
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400373F RID: 14143
	private PassengerRocketModule module;
}

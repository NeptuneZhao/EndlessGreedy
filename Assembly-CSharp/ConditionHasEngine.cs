using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000AFF RID: 2815
public class ConditionHasEngine : ProcessCondition
{
	// Token: 0x06005401 RID: 21505 RVA: 0x001E162D File Offset: 0x001DF82D
	public ConditionHasEngine(ILaunchableRocket launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x06005402 RID: 21506 RVA: 0x001E163C File Offset: 0x001DF83C
	public override ProcessCondition.Status EvaluateCondition()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchable.LaunchableGameObject.GetComponent<AttachableBuilding>()))
		{
			if (gameObject.GetComponent<RocketEngine>() != null || gameObject.GetComponent<RocketEngineCluster>())
			{
				return ProcessCondition.Status.Ready;
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005403 RID: 21507 RVA: 0x001E16BC File Offset: 0x001DF8BC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06005404 RID: 21508 RVA: 0x001E16FC File Offset: 0x001DF8FC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005405 RID: 21509 RVA: 0x001E173C File Offset: 0x001DF93C
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003737 RID: 14135
	private ILaunchableRocket launchable;
}

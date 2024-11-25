using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000AF8 RID: 2808
public class ConditionAllModulesComplete : ProcessCondition
{
	// Token: 0x060053D3 RID: 21459 RVA: 0x001E098F File Offset: 0x001DEB8F
	public ConditionAllModulesComplete(ILaunchableRocket launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x060053D4 RID: 21460 RVA: 0x001E09A0 File Offset: 0x001DEBA0
	public override ProcessCondition.Status EvaluateCondition()
	{
		using (List<GameObject>.Enumerator enumerator = AttachableBuilding.GetAttachedNetwork(this.launchable.LaunchableGameObject.GetComponent<AttachableBuilding>()).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetComponent<Constructable>() != null)
				{
					return ProcessCondition.Status.Failure;
				}
			}
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x060053D5 RID: 21461 RVA: 0x001E0A10 File Offset: 0x001DEC10
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x060053D6 RID: 21462 RVA: 0x001E0A50 File Offset: 0x001DEC50
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x060053D7 RID: 21463 RVA: 0x001E0A90 File Offset: 0x001DEC90
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003729 RID: 14121
	private ILaunchableRocket launchable;
}

﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x02000CDC RID: 3292
public class MeterScreen_Sickness : MeterScreen_VTD_DuplicantIterator
{
	// Token: 0x060065CA RID: 26058 RVA: 0x0025F384 File Offset: 0x0025D584
	protected override void InternalRefresh()
	{
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		int num = this.CountSickDupes(worldMinionIdentities);
		this.Label.text = num.ToString();
	}

	// Token: 0x060065CB RID: 26059 RVA: 0x0025F3B4 File Offset: 0x0025D5B4
	protected override string OnTooltip()
	{
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		int num = this.CountSickDupes(worldMinionIdentities);
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_SICK_DUPES, num.ToString()), this.ToolTipStyle_Header);
		for (int i = 0; i < worldMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = worldMinionIdentities[i];
			if (!minionIdentity.IsNullOrDestroyed())
			{
				string text = minionIdentity.GetComponent<KSelectable>().GetName();
				Sicknesses sicknesses = minionIdentity.GetComponent<MinionModifiers>().sicknesses;
				if (sicknesses.IsInfected())
				{
					text += " (";
					int num2 = 0;
					foreach (SicknessInstance sicknessInstance in sicknesses)
					{
						text = text + ((num2 > 0) ? ", " : "") + sicknessInstance.modifier.Name;
						num2++;
					}
					text += ")";
				}
				bool selected = i == this.lastSelectedDuplicantIndex;
				base.AddToolTipLine(text, selected);
			}
		}
		return "";
	}

	// Token: 0x060065CC RID: 26060 RVA: 0x0025F4F0 File Offset: 0x0025D6F0
	private int CountSickDupes(List<MinionIdentity> minions)
	{
		int num = 0;
		foreach (MinionIdentity minionIdentity in minions)
		{
			if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetComponent<MinionModifiers>().sicknesses.IsInfected())
			{
				num++;
			}
		}
		return num;
	}
}

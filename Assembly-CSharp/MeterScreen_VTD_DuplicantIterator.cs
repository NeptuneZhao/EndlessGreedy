using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000CDE RID: 3294
public abstract class MeterScreen_VTD_DuplicantIterator : MeterScreen_ValueTrackerDisplayer
{
	// Token: 0x060065D2 RID: 26066 RVA: 0x0025F748 File Offset: 0x0025D948
	protected virtual void UpdateDisplayInfo(BaseEventData base_ev_data, IList<MinionIdentity> minions)
	{
		PointerEventData pointerEventData = base_ev_data as PointerEventData;
		if (pointerEventData == null)
		{
			return;
		}
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		PointerEventData.InputButton button = pointerEventData.button;
		if (button != PointerEventData.InputButton.Left)
		{
			if (button != PointerEventData.InputButton.Right)
			{
				return;
			}
			this.lastSelectedDuplicantIndex = -1;
		}
		else
		{
			if (worldMinionIdentities.Count < this.lastSelectedDuplicantIndex)
			{
				this.lastSelectedDuplicantIndex = -1;
			}
			if (worldMinionIdentities.Count > 0)
			{
				this.lastSelectedDuplicantIndex = (this.lastSelectedDuplicantIndex + 1) % worldMinionIdentities.Count;
				MinionIdentity minionIdentity = minions[this.lastSelectedDuplicantIndex];
				SelectTool.Instance.SelectAndFocus(minionIdentity.transform.GetPosition(), minionIdentity.GetComponent<KSelectable>(), Vector3.zero);
				return;
			}
		}
	}

	// Token: 0x060065D3 RID: 26067 RVA: 0x0025F7E0 File Offset: 0x0025D9E0
	public override void OnClick(BaseEventData base_ev_data)
	{
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		this.UpdateDisplayInfo(base_ev_data, worldMinionIdentities);
		this.OnTooltip();
		this.Tooltip.forceRefresh = true;
	}

	// Token: 0x060065D4 RID: 26068 RVA: 0x0025F80F File Offset: 0x0025DA0F
	protected void AddToolTipLine(string str, bool selected)
	{
		if (selected)
		{
			this.Tooltip.AddMultiStringTooltip("<color=#F0B310FF>" + str + "</color>", this.ToolTipStyle_Property);
			return;
		}
		this.Tooltip.AddMultiStringTooltip(str, this.ToolTipStyle_Property);
	}

	// Token: 0x060065D5 RID: 26069 RVA: 0x0025F848 File Offset: 0x0025DA48
	protected void AddToolTipAmountPercentLine(AmountInstance amount, MinionIdentity id, bool selected)
	{
		string str = id.GetComponent<KSelectable>().GetName() + ":  " + Mathf.Round(amount.value).ToString() + "%";
		this.AddToolTipLine(str, selected);
	}

	// Token: 0x040044B8 RID: 17592
	protected int lastSelectedDuplicantIndex = -1;
}

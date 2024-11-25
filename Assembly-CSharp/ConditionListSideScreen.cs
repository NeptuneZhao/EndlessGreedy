using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D5B RID: 3419
public class ConditionListSideScreen : SideScreenContent
{
	// Token: 0x06006BBC RID: 27580 RVA: 0x0028936C File Offset: 0x0028756C
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x06006BBD RID: 27581 RVA: 0x0028936F File Offset: 0x0028756F
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target != null)
		{
			this.targetConditionSet = target.GetComponent<IProcessConditionSet>();
		}
	}

	// Token: 0x06006BBE RID: 27582 RVA: 0x0028938D File Offset: 0x0028758D
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.Refresh();
		}
	}

	// Token: 0x06006BBF RID: 27583 RVA: 0x002893A0 File Offset: 0x002875A0
	private void Refresh()
	{
		bool flag = false;
		List<ProcessCondition> conditionSet = this.targetConditionSet.GetConditionSet(ProcessCondition.ProcessConditionType.All);
		foreach (ProcessCondition key in conditionSet)
		{
			if (!this.rows.ContainsKey(key))
			{
				flag = true;
				break;
			}
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.rows)
		{
			if (!conditionSet.Contains(keyValuePair.Key))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.Rebuild();
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair2 in this.rows)
		{
			ConditionListSideScreen.SetRowState(keyValuePair2.Value, keyValuePair2.Key);
		}
	}

	// Token: 0x06006BC0 RID: 27584 RVA: 0x002894B4 File Offset: 0x002876B4
	public static void SetRowState(GameObject row, ProcessCondition condition)
	{
		HierarchyReferences component = row.GetComponent<HierarchyReferences>();
		ProcessCondition.Status status = condition.EvaluateCondition();
		component.GetReference<LocText>("Label").text = condition.GetStatusMessage(status);
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.failedColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.failedColor;
			break;
		case ProcessCondition.Status.Warning:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.warningColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.warningColor;
			break;
		case ProcessCondition.Status.Ready:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.readyColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.readyColor;
			break;
		}
		component.GetReference<Image>("Check").gameObject.SetActive(status == ProcessCondition.Status.Ready);
		component.GetReference<Image>("Dash").gameObject.SetActive(false);
		row.GetComponent<ToolTip>().SetSimpleTooltip(condition.GetStatusTooltip(status));
	}

	// Token: 0x06006BC1 RID: 27585 RVA: 0x002895C0 File Offset: 0x002877C0
	private void Rebuild()
	{
		this.ClearRows();
		this.BuildRows();
	}

	// Token: 0x06006BC2 RID: 27586 RVA: 0x002895D0 File Offset: 0x002877D0
	private void ClearRows()
	{
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
	}

	// Token: 0x06006BC3 RID: 27587 RVA: 0x00289634 File Offset: 0x00287834
	private void BuildRows()
	{
		foreach (ProcessCondition processCondition in this.targetConditionSet.GetConditionSet(ProcessCondition.ProcessConditionType.All))
		{
			if (processCondition.ShowInUI())
			{
				GameObject value = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
				this.rows.Add(processCondition, value);
			}
		}
	}

	// Token: 0x0400497E RID: 18814
	public GameObject rowPrefab;

	// Token: 0x0400497F RID: 18815
	public GameObject rowContainer;

	// Token: 0x04004980 RID: 18816
	[Tooltip("This list is indexed by the ProcessCondition.Status enum")]
	public static Color readyColor = Color.black;

	// Token: 0x04004981 RID: 18817
	public static Color failedColor = Color.red;

	// Token: 0x04004982 RID: 18818
	public static Color warningColor = new Color(1f, 0.3529412f, 0f, 1f);

	// Token: 0x04004983 RID: 18819
	private IProcessConditionSet targetConditionSet;

	// Token: 0x04004984 RID: 18820
	private Dictionary<ProcessCondition, GameObject> rows = new Dictionary<ProcessCondition, GameObject>();
}

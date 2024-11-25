using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000D4B RID: 3403
public class AutomatableSideScreen : SideScreenContent
{
	// Token: 0x06006B18 RID: 27416 RVA: 0x002852D1 File Offset: 0x002834D1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006B19 RID: 27417 RVA: 0x002852DC File Offset: 0x002834DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allowManualToggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.AUTOMATABLE_SIDE_SCREEN.ALLOWMANUALBUTTONTOOLTIP);
		this.allowManualToggle.onValueChanged += this.OnAllowManualChanged;
	}

	// Token: 0x06006B1A RID: 27418 RVA: 0x0028532A File Offset: 0x0028352A
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Automatable>() != null;
	}

	// Token: 0x06006B1B RID: 27419 RVA: 0x00285338 File Offset: 0x00283538
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetAutomatable = target.GetComponent<Automatable>();
		if (this.targetAutomatable == null)
		{
			global::Debug.LogError("The target provided does not have an Automatable component");
			return;
		}
		this.allowManualToggle.isOn = !this.targetAutomatable.GetAutomationOnly();
		this.allowManualToggleCheckMark.enabled = this.allowManualToggle.isOn;
	}

	// Token: 0x06006B1C RID: 27420 RVA: 0x002853B4 File Offset: 0x002835B4
	private void OnAllowManualChanged(bool value)
	{
		this.targetAutomatable.SetAutomationOnly(!value);
		this.allowManualToggleCheckMark.enabled = value;
	}

	// Token: 0x04004901 RID: 18689
	public KToggle allowManualToggle;

	// Token: 0x04004902 RID: 18690
	public KImage allowManualToggleCheckMark;

	// Token: 0x04004903 RID: 18691
	public GameObject content;

	// Token: 0x04004904 RID: 18692
	private GameObject target;

	// Token: 0x04004905 RID: 18693
	public LocText DescriptionText;

	// Token: 0x04004906 RID: 18694
	private Automatable targetAutomatable;
}

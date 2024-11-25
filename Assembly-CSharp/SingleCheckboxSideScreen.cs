using System;
using UnityEngine;

// Token: 0x02000DA5 RID: 3493
public class SingleCheckboxSideScreen : SideScreenContent
{
	// Token: 0x06006E44 RID: 28228 RVA: 0x00297907 File Offset: 0x00295B07
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006E45 RID: 28229 RVA: 0x0029790F File Offset: 0x00295B0F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.toggle.onValueChanged += this.OnValueChanged;
	}

	// Token: 0x06006E46 RID: 28230 RVA: 0x0029792E File Offset: 0x00295B2E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ICheckboxControl>() != null || target.GetSMI<ICheckboxControl>() != null;
	}

	// Token: 0x06006E47 RID: 28231 RVA: 0x00297944 File Offset: 0x00295B44
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.target = target.GetComponent<ICheckboxControl>();
		if (this.target == null)
		{
			this.target = target.GetSMI<ICheckboxControl>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The target provided does not have an ICheckboxControl component");
			return;
		}
		this.label.text = this.target.CheckboxLabel;
		this.toggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(this.target.CheckboxTooltip);
		this.titleKey = this.target.CheckboxTitleKey;
		this.toggle.isOn = this.target.GetCheckboxValue();
		this.toggleCheckMark.enabled = this.toggle.isOn;
	}

	// Token: 0x06006E48 RID: 28232 RVA: 0x00297A17 File Offset: 0x00295C17
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.target = null;
	}

	// Token: 0x06006E49 RID: 28233 RVA: 0x00297A26 File Offset: 0x00295C26
	private void OnValueChanged(bool value)
	{
		this.target.SetCheckboxValue(value);
		this.toggleCheckMark.enabled = value;
	}

	// Token: 0x04004B41 RID: 19265
	public KToggle toggle;

	// Token: 0x04004B42 RID: 19266
	public KImage toggleCheckMark;

	// Token: 0x04004B43 RID: 19267
	public LocText label;

	// Token: 0x04004B44 RID: 19268
	private ICheckboxControl target;
}

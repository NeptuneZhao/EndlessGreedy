using System;
using UnityEngine;

// Token: 0x02000D8F RID: 3471
public class ProgressBarSideScreen : SideScreenContent, IRender1000ms
{
	// Token: 0x06006D65 RID: 28005 RVA: 0x0029267F File Offset: 0x0029087F
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06006D66 RID: 28006 RVA: 0x00292687 File Offset: 0x00290887
	public override int GetSideScreenSortOrder()
	{
		return -10;
	}

	// Token: 0x06006D67 RID: 28007 RVA: 0x0029268B File Offset: 0x0029088B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IProgressBarSideScreen>() != null;
	}

	// Token: 0x06006D68 RID: 28008 RVA: 0x00292696 File Offset: 0x00290896
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetObject = target.GetComponent<IProgressBarSideScreen>();
		this.RefreshBar();
	}

	// Token: 0x06006D69 RID: 28009 RVA: 0x002926B4 File Offset: 0x002908B4
	private void RefreshBar()
	{
		this.progressBar.SetMaxValue(this.targetObject.GetProgressBarMaxValue());
		this.progressBar.SetFillPercentage(this.targetObject.GetProgressBarFillPercentage());
		this.progressBar.label.SetText(this.targetObject.GetProgressBarLabel());
		this.label.SetText(this.targetObject.GetProgressBarTitleLabel());
		this.progressBar.GetComponentInChildren<ToolTip>().SetSimpleTooltip(this.targetObject.GetProgressBarTooltip());
	}

	// Token: 0x06006D6A RID: 28010 RVA: 0x00292739 File Offset: 0x00290939
	public void Render1000ms(float dt)
	{
		this.RefreshBar();
	}

	// Token: 0x04004AA1 RID: 19105
	public LocText label;

	// Token: 0x04004AA2 RID: 19106
	public GenericUIProgressBar progressBar;

	// Token: 0x04004AA3 RID: 19107
	public IProgressBarSideScreen targetObject;
}

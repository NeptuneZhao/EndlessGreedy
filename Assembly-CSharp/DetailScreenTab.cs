using System;
using UnityEngine;

// Token: 0x02000C39 RID: 3129
public abstract class DetailScreenTab : TargetPanel
{
	// Token: 0x06006004 RID: 24580
	public abstract override bool IsValidForTarget(GameObject target);

	// Token: 0x06006005 RID: 24581 RVA: 0x0023AD9C File Offset: 0x00238F9C
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
	}

	// Token: 0x06006006 RID: 24582 RVA: 0x0023ADA8 File Offset: 0x00238FA8
	protected CollapsibleDetailContentPanel CreateCollapsableSection(string title = null)
	{
		CollapsibleDetailContentPanel collapsibleDetailContentPanel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		if (!string.IsNullOrEmpty(title))
		{
			collapsibleDetailContentPanel.SetTitle(title);
		}
		return collapsibleDetailContentPanel;
	}

	// Token: 0x06006007 RID: 24583 RVA: 0x0023ADDC File Offset: 0x00238FDC
	private void Update()
	{
		this.Refresh(false);
	}

	// Token: 0x06006008 RID: 24584 RVA: 0x0023ADE5 File Offset: 0x00238FE5
	protected virtual void Refresh(bool force = false)
	{
	}
}

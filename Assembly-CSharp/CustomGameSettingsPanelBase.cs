using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C26 RID: 3110
public abstract class CustomGameSettingsPanelBase : MonoBehaviour
{
	// Token: 0x06005F58 RID: 24408 RVA: 0x00236883 File Offset: 0x00234A83
	public virtual void Init()
	{
	}

	// Token: 0x06005F59 RID: 24409 RVA: 0x00236885 File Offset: 0x00234A85
	public virtual void Uninit()
	{
	}

	// Token: 0x06005F5A RID: 24410 RVA: 0x00236887 File Offset: 0x00234A87
	private void OnEnable()
	{
		this.isDirty = true;
	}

	// Token: 0x06005F5B RID: 24411 RVA: 0x00236890 File Offset: 0x00234A90
	private void Update()
	{
		if (this.isDirty)
		{
			this.isDirty = false;
			this.Refresh();
		}
	}

	// Token: 0x06005F5C RID: 24412 RVA: 0x002368A7 File Offset: 0x00234AA7
	protected void AddWidget(CustomGameSettingWidget widget)
	{
		widget.onSettingChanged += this.OnWidgetChanged;
		this.widgets.Add(widget);
	}

	// Token: 0x06005F5D RID: 24413 RVA: 0x002368C7 File Offset: 0x00234AC7
	private void OnWidgetChanged(CustomGameSettingWidget widget)
	{
		this.isDirty = true;
	}

	// Token: 0x06005F5E RID: 24414 RVA: 0x002368D0 File Offset: 0x00234AD0
	public virtual void Refresh()
	{
		foreach (CustomGameSettingWidget customGameSettingWidget in this.widgets)
		{
			customGameSettingWidget.Refresh();
		}
	}

	// Token: 0x0400402C RID: 16428
	protected List<CustomGameSettingWidget> widgets = new List<CustomGameSettingWidget>();

	// Token: 0x0400402D RID: 16429
	private bool isDirty;
}

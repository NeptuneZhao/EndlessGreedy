using System;
using UnityEngine;

// Token: 0x02000D14 RID: 3348
[AddComponentMenu("KMonoBehaviour/scripts/PriorityButton")]
public class PriorityButton : KMonoBehaviour
{
	// Token: 0x1700076E RID: 1902
	// (get) Token: 0x06006877 RID: 26743 RVA: 0x00271CD0 File Offset: 0x0026FED0
	// (set) Token: 0x06006878 RID: 26744 RVA: 0x00271CD8 File Offset: 0x0026FED8
	public PrioritySetting priority
	{
		get
		{
			return this._priority;
		}
		set
		{
			this._priority = value;
			if (this.its != null)
			{
				if (this.priority.priority_class == PriorityScreen.PriorityClass.high)
				{
					this.its.colorStyleSetting = this.highStyle;
				}
				else
				{
					this.its.colorStyleSetting = this.normalStyle;
				}
				this.its.RefreshColorStyle();
				this.its.ResetColor();
			}
		}
	}

	// Token: 0x06006879 RID: 26745 RVA: 0x00271D42 File Offset: 0x0026FF42
	protected override void OnPrefabInit()
	{
		this.toggle.onClick += this.OnClick;
	}

	// Token: 0x0600687A RID: 26746 RVA: 0x00271D5B File Offset: 0x0026FF5B
	private void OnClick()
	{
		if (this.playSelectionSound)
		{
			PriorityScreen.PlayPriorityConfirmSound(this.priority);
		}
		if (this.onClick != null)
		{
			this.onClick(this.priority);
		}
	}

	// Token: 0x040046A4 RID: 18084
	public KToggle toggle;

	// Token: 0x040046A5 RID: 18085
	public LocText text;

	// Token: 0x040046A6 RID: 18086
	public ToolTip tooltip;

	// Token: 0x040046A7 RID: 18087
	[MyCmpGet]
	private ImageToggleState its;

	// Token: 0x040046A8 RID: 18088
	public ColorStyleSetting normalStyle;

	// Token: 0x040046A9 RID: 18089
	public ColorStyleSetting highStyle;

	// Token: 0x040046AA RID: 18090
	public bool playSelectionSound = true;

	// Token: 0x040046AB RID: 18091
	public Action<PrioritySetting> onClick;

	// Token: 0x040046AC RID: 18092
	private PrioritySetting _priority;
}

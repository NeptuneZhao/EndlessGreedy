using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D30 RID: 3376
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockButton")]
public class ScheduleBlockButton : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06006A2B RID: 27179 RVA: 0x00280010 File Offset: 0x0027E210
	public void Setup(int hour)
	{
		if (hour < TRAITS.EARLYBIRD_SCHEDULEBLOCK)
		{
			base.GetComponent<HierarchyReferences>().GetReference<RectTransform>("MorningIcon").gameObject.SetActive(true);
		}
		else if (hour >= 21)
		{
			base.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightIcon").gameObject.SetActive(true);
		}
		base.gameObject.name = "ScheduleBlock_" + hour.ToString();
		this.ToggleHighlight(false);
	}

	// Token: 0x06006A2C RID: 27180 RVA: 0x00280088 File Offset: 0x0027E288
	public void SetBlockTypes(List<ScheduleBlockType> blockTypes)
	{
		ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(blockTypes);
		if (scheduleGroup != null)
		{
			this.image.color = scheduleGroup.uiColor;
			this.toolTip.SetSimpleTooltip(scheduleGroup.Name);
			return;
		}
		this.toolTip.SetSimpleTooltip("UNKNOWN");
	}

	// Token: 0x06006A2D RID: 27181 RVA: 0x002800DC File Offset: 0x0027E2DC
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleHighlight(true);
	}

	// Token: 0x06006A2E RID: 27182 RVA: 0x002800E5 File Offset: 0x0027E2E5
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleHighlight(false);
	}

	// Token: 0x06006A2F RID: 27183 RVA: 0x002800EE File Offset: 0x0027E2EE
	private void ToggleHighlight(bool on)
	{
		this.highlightObject.SetActive(on);
	}

	// Token: 0x0400485B RID: 18523
	[SerializeField]
	private Image image;

	// Token: 0x0400485C RID: 18524
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x0400485D RID: 18525
	[SerializeField]
	private GameObject highlightObject;
}

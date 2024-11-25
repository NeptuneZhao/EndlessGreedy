using System;
using UnityEngine;

// Token: 0x02000DD8 RID: 3544
public class TimeOfDayPositioner : KMonoBehaviour
{
	// Token: 0x060070A4 RID: 28836 RVA: 0x002AA67C File Offset: 0x002A887C
	public void SetTargetTimetable(GameObject TimetableRow)
	{
		if (TimetableRow == null)
		{
			this.targetRect = null;
			base.transform.SetParent(null);
			return;
		}
		RectTransform rectTransform = TimetableRow.GetComponent<HierarchyReferences>().GetReference<RectTransform>("BlockContainer").rectTransform();
		this.targetRect = rectTransform;
		base.transform.SetParent(this.targetRect.transform);
	}

	// Token: 0x060070A5 RID: 28837 RVA: 0x002AA6DC File Offset: 0x002A88DC
	private void Update()
	{
		if (base.transform.parent != this.targetRect.transform)
		{
			base.transform.parent = this.targetRect.transform;
		}
		float f = GameClock.Instance.GetCurrentCycleAsPercentage() * this.targetRect.rect.width;
		(base.transform as RectTransform).anchoredPosition = new Vector2(Mathf.Round(f), 0f);
	}

	// Token: 0x04004D6C RID: 19820
	private RectTransform targetRect;
}

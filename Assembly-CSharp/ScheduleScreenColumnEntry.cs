using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D35 RID: 3381
public class ScheduleScreenColumnEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler
{
	// Token: 0x06006A57 RID: 27223 RVA: 0x00280CF8 File Offset: 0x0027EEF8
	public void OnPointerEnter(PointerEventData event_data)
	{
		this.RunCallbacks();
	}

	// Token: 0x06006A58 RID: 27224 RVA: 0x00280D00 File Offset: 0x0027EF00
	private void RunCallbacks()
	{
		if (Input.GetMouseButton(0) && this.onLeftClick != null)
		{
			this.onLeftClick();
		}
	}

	// Token: 0x06006A59 RID: 27225 RVA: 0x00280D1D File Offset: 0x0027EF1D
	public void OnPointerDown(PointerEventData event_data)
	{
		this.RunCallbacks();
	}

	// Token: 0x04004870 RID: 18544
	public Image image;

	// Token: 0x04004871 RID: 18545
	public System.Action onLeftClick;
}

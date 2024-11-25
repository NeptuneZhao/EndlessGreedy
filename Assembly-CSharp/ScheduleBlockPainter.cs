using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000D31 RID: 3377
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockPainter")]
public class ScheduleBlockPainter : KMonoBehaviour, IPointerDownHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	// Token: 0x06006A31 RID: 27185 RVA: 0x00280104 File Offset: 0x0027E304
	public void SetEntry(ScheduleScreenEntry entry)
	{
		this.entry = entry;
	}

	// Token: 0x06006A32 RID: 27186 RVA: 0x0028010D File Offset: 0x0027E30D
	public void OnBeginDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x06006A33 RID: 27187 RVA: 0x00280116 File Offset: 0x0027E316
	public void OnDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x06006A34 RID: 27188 RVA: 0x0028011F File Offset: 0x0027E31F
	public void OnEndDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x06006A35 RID: 27189 RVA: 0x00280128 File Offset: 0x0027E328
	public void OnPointerDown(PointerEventData eventData)
	{
		ScheduleBlockPainter.paintCounter = 0;
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x06006A36 RID: 27190 RVA: 0x00280138 File Offset: 0x0027E338
	private void PaintBlocksBelow(PointerEventData eventData)
	{
		if (ScheduleScreen.Instance.SelectedPaint.IsNullOrWhiteSpace())
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, list);
		if (list != null && list.Count > 0)
		{
			ScheduleBlockButton component = list[0].gameObject.GetComponent<ScheduleBlockButton>();
			if (component != null)
			{
				if (this.entry.PaintBlock(component))
				{
					string sound = GlobalAssets.GetSound("ScheduleMenu_Select", false);
					if (sound != null)
					{
						EventInstance instance = SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition(), 1f, false);
						instance.setParameterByName("Drag_Count", (float)ScheduleBlockPainter.paintCounter, false);
						ScheduleBlockPainter.paintCounter++;
						SoundEvent.EndOneShot(instance);
						this.previousBlockTriedPainted = component.gameObject;
						return;
					}
				}
				else if (this.previousBlockTriedPainted != component.gameObject)
				{
					this.previousBlockTriedPainted = component.gameObject;
					string sound2 = GlobalAssets.GetSound("ScheduleMenu_Select_none", false);
					if (sound2 != null)
					{
						SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound2, SoundListenerController.Instance.transform.GetPosition(), 1f, false));
					}
				}
			}
		}
	}

	// Token: 0x0400485E RID: 18526
	private ScheduleScreenEntry entry;

	// Token: 0x0400485F RID: 18527
	private static int paintCounter;

	// Token: 0x04004860 RID: 18528
	private GameObject previousBlockTriedPainted;
}

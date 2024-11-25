using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000D39 RID: 3385
public class SelectablePanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x06006A79 RID: 27257 RVA: 0x00281C2F File Offset: 0x0027FE2F
	public void OnDeselect(BaseEventData evt)
	{
		base.gameObject.SetActive(false);
	}
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000DF4 RID: 3572
public class DialogPanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x06007160 RID: 29024 RVA: 0x002AE08C File Offset: 0x002AC28C
	public void OnDeselect(BaseEventData eventData)
	{
		if (this.destroyOnDeselect)
		{
			foreach (object obj in base.transform)
			{
				Util.KDestroyGameObject(((Transform)obj).gameObject);
			}
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04004E0C RID: 19980
	public bool destroyOnDeselect = true;
}

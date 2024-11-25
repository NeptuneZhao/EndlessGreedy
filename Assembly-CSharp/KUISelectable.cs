using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C78 RID: 3192
[AddComponentMenu("KMonoBehaviour/scripts/KUISelectable")]
public class KUISelectable : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06006217 RID: 25111 RVA: 0x00249BC4 File Offset: 0x00247DC4
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x06006218 RID: 25112 RVA: 0x00249BC6 File Offset: 0x00247DC6
	protected override void OnSpawn()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06006219 RID: 25113 RVA: 0x00249BE4 File Offset: 0x00247DE4
	public void SetTarget(GameObject target)
	{
		this.target = target;
	}

	// Token: 0x0600621A RID: 25114 RVA: 0x00249BED File Offset: 0x00247DED
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.target != null)
		{
			SelectTool.Instance.SetHoverOverride(this.target.GetComponent<KSelectable>());
		}
	}

	// Token: 0x0600621B RID: 25115 RVA: 0x00249C12 File Offset: 0x00247E12
	public void OnPointerExit(PointerEventData eventData)
	{
		SelectTool.Instance.SetHoverOverride(null);
	}

	// Token: 0x0600621C RID: 25116 RVA: 0x00249C1F File Offset: 0x00247E1F
	private void OnClick()
	{
		if (this.target != null)
		{
			SelectTool.Instance.Select(this.target.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x0600621D RID: 25117 RVA: 0x00249C45 File Offset: 0x00247E45
	protected override void OnCmpDisable()
	{
		if (SelectTool.Instance != null)
		{
			SelectTool.Instance.SetHoverOverride(null);
		}
	}

	// Token: 0x04004283 RID: 17027
	private GameObject target;
}

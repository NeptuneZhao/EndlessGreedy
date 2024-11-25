using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DDD RID: 3549
public class Tween : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060070C8 RID: 28872 RVA: 0x002AB188 File Offset: 0x002A9388
	private void Awake()
	{
		this.Selectable = base.GetComponent<Selectable>();
	}

	// Token: 0x060070C9 RID: 28873 RVA: 0x002AB196 File Offset: 0x002A9396
	public void OnPointerEnter(PointerEventData data)
	{
		this.Direction = 1f;
	}

	// Token: 0x060070CA RID: 28874 RVA: 0x002AB1A3 File Offset: 0x002A93A3
	public void OnPointerExit(PointerEventData data)
	{
		this.Direction = -1f;
	}

	// Token: 0x060070CB RID: 28875 RVA: 0x002AB1B0 File Offset: 0x002A93B0
	private void Update()
	{
		if (this.Selectable.interactable)
		{
			float x = base.transform.localScale.x;
			float num = x + this.Direction * Time.unscaledDeltaTime * Tween.ScaleSpeed;
			num = Mathf.Min(num, Tween.Scale);
			num = Mathf.Max(num, 1f);
			if (num != x)
			{
				base.transform.localScale = new Vector3(num, num, 1f);
			}
		}
	}

	// Token: 0x04004D89 RID: 19849
	private static float Scale = 1.025f;

	// Token: 0x04004D8A RID: 19850
	private static float ScaleSpeed = 0.5f;

	// Token: 0x04004D8B RID: 19851
	private Selectable Selectable;

	// Token: 0x04004D8C RID: 19852
	private float Direction = -1f;
}

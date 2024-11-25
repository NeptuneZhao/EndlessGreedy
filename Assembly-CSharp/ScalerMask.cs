using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C38 RID: 3128
[AddComponentMenu("KMonoBehaviour/scripts/ScalerMask")]
public class ScalerMask : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x1700072A RID: 1834
	// (get) Token: 0x06005FF9 RID: 24569 RVA: 0x0023AB36 File Offset: 0x00238D36
	private RectTransform ThisTransform
	{
		get
		{
			if (this._thisTransform == null)
			{
				this._thisTransform = base.GetComponent<RectTransform>();
			}
			return this._thisTransform;
		}
	}

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x06005FFA RID: 24570 RVA: 0x0023AB58 File Offset: 0x00238D58
	private LayoutElement ThisLayoutElement
	{
		get
		{
			if (this._thisLayoutElement == null)
			{
				this._thisLayoutElement = base.GetComponent<LayoutElement>();
			}
			return this._thisLayoutElement;
		}
	}

	// Token: 0x06005FFB RID: 24571 RVA: 0x0023AB7C File Offset: 0x00238D7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		DetailsScreen componentInParent = base.GetComponentInParent<DetailsScreen>();
		if (componentInParent)
		{
			DetailsScreen detailsScreen = componentInParent;
			detailsScreen.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(detailsScreen.pointerEnterActions, new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent));
			DetailsScreen detailsScreen2 = componentInParent;
			detailsScreen2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(detailsScreen2.pointerExitActions, new KScreen.PointerExitActions(this.OnPointerExitGrandparent));
		}
	}

	// Token: 0x06005FFC RID: 24572 RVA: 0x0023ABE4 File Offset: 0x00238DE4
	protected override void OnCleanUp()
	{
		DetailsScreen componentInParent = base.GetComponentInParent<DetailsScreen>();
		if (componentInParent)
		{
			DetailsScreen detailsScreen = componentInParent;
			detailsScreen.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Remove(detailsScreen.pointerEnterActions, new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent));
			DetailsScreen detailsScreen2 = componentInParent;
			detailsScreen2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Remove(detailsScreen2.pointerExitActions, new KScreen.PointerExitActions(this.OnPointerExitGrandparent));
		}
		base.OnCleanUp();
	}

	// Token: 0x06005FFD RID: 24573 RVA: 0x0023AC4C File Offset: 0x00238E4C
	private void Update()
	{
		if (this.SourceTransform != null)
		{
			this.SourceTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.ThisTransform.rect.width);
		}
		if (this.SourceTransform != null && (!this.hoverLock || !this.grandparentIsHovered || this.isHovered || this.queuedSizeUpdate))
		{
			this.ThisLayoutElement.minHeight = this.SourceTransform.rect.height + this.topPadding + this.bottomPadding;
			this.SourceTransform.anchoredPosition = new Vector2(0f, -this.topPadding);
			this.queuedSizeUpdate = false;
		}
		if (this.hoverIndicator != null)
		{
			if (this.SourceTransform != null && this.SourceTransform.rect.height > this.ThisTransform.rect.height)
			{
				this.hoverIndicator.SetActive(true);
				return;
			}
			this.hoverIndicator.SetActive(false);
		}
	}

	// Token: 0x06005FFE RID: 24574 RVA: 0x0023AD60 File Offset: 0x00238F60
	public void UpdateSize()
	{
		this.queuedSizeUpdate = true;
	}

	// Token: 0x06005FFF RID: 24575 RVA: 0x0023AD69 File Offset: 0x00238F69
	public void OnPointerEnterGrandparent(PointerEventData eventData)
	{
		this.grandparentIsHovered = true;
	}

	// Token: 0x06006000 RID: 24576 RVA: 0x0023AD72 File Offset: 0x00238F72
	public void OnPointerExitGrandparent(PointerEventData eventData)
	{
		this.grandparentIsHovered = false;
	}

	// Token: 0x06006001 RID: 24577 RVA: 0x0023AD7B File Offset: 0x00238F7B
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isHovered = true;
	}

	// Token: 0x06006002 RID: 24578 RVA: 0x0023AD84 File Offset: 0x00238F84
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isHovered = false;
	}

	// Token: 0x040040C7 RID: 16583
	public RectTransform SourceTransform;

	// Token: 0x040040C8 RID: 16584
	private RectTransform _thisTransform;

	// Token: 0x040040C9 RID: 16585
	private LayoutElement _thisLayoutElement;

	// Token: 0x040040CA RID: 16586
	public GameObject hoverIndicator;

	// Token: 0x040040CB RID: 16587
	public bool hoverLock;

	// Token: 0x040040CC RID: 16588
	private bool grandparentIsHovered;

	// Token: 0x040040CD RID: 16589
	private bool isHovered;

	// Token: 0x040040CE RID: 16590
	private bool queuedSizeUpdate = true;

	// Token: 0x040040CF RID: 16591
	public float topPadding;

	// Token: 0x040040D0 RID: 16592
	public float bottomPadding;
}

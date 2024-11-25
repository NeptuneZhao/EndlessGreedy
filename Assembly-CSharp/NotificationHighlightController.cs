using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CF9 RID: 3321
public class NotificationHighlightController : KMonoBehaviour
{
	// Token: 0x06006704 RID: 26372 RVA: 0x00267966 File Offset: 0x00265B66
	protected override void OnSpawn()
	{
		this.highlightBox = Util.KInstantiateUI<RectTransform>(this.highlightBoxPrefab.gameObject, base.gameObject, false);
		this.HideBox();
	}

	// Token: 0x06006705 RID: 26373 RVA: 0x0026798C File Offset: 0x00265B8C
	[ContextMenu("Force Update")]
	protected void LateUpdate()
	{
		bool flag = false;
		if (this.activeTargetNotification != null)
		{
			foreach (NotificationHighlightTarget notificationHighlightTarget in this.targets)
			{
				if (notificationHighlightTarget.targetKey == this.activeTargetNotification.highlightTarget)
				{
					this.SnapBoxToTarget(notificationHighlightTarget);
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			this.HideBox();
		}
	}

	// Token: 0x06006706 RID: 26374 RVA: 0x00267A10 File Offset: 0x00265C10
	public void AddTarget(NotificationHighlightTarget target)
	{
		this.targets.Add(target);
	}

	// Token: 0x06006707 RID: 26375 RVA: 0x00267A1E File Offset: 0x00265C1E
	public void RemoveTarget(NotificationHighlightTarget target)
	{
		this.targets.Remove(target);
	}

	// Token: 0x06006708 RID: 26376 RVA: 0x00267A2D File Offset: 0x00265C2D
	public void SetActiveTarget(ManagementMenuNotification notification)
	{
		this.activeTargetNotification = notification;
	}

	// Token: 0x06006709 RID: 26377 RVA: 0x00267A36 File Offset: 0x00265C36
	public void ClearActiveTarget(ManagementMenuNotification checkNotification)
	{
		if (checkNotification == this.activeTargetNotification)
		{
			this.activeTargetNotification = null;
		}
	}

	// Token: 0x0600670A RID: 26378 RVA: 0x00267A48 File Offset: 0x00265C48
	public void ClearActiveTarget()
	{
		this.activeTargetNotification = null;
	}

	// Token: 0x0600670B RID: 26379 RVA: 0x00267A51 File Offset: 0x00265C51
	public void TargetViewed(NotificationHighlightTarget target)
	{
		if (this.activeTargetNotification != null && this.activeTargetNotification.highlightTarget == target.targetKey)
		{
			this.activeTargetNotification.View();
		}
	}

	// Token: 0x0600670C RID: 26380 RVA: 0x00267A80 File Offset: 0x00265C80
	private void SnapBoxToTarget(NotificationHighlightTarget target)
	{
		RectTransform rectTransform = target.rectTransform();
		Vector3 position = rectTransform.GetPosition();
		this.highlightBox.sizeDelta = rectTransform.rect.size;
		this.highlightBox.SetPosition(position + new Vector3(rectTransform.rect.position.x, rectTransform.rect.position.y, 0f));
		RectMask2D componentInParent = rectTransform.GetComponentInParent<RectMask2D>();
		if (componentInParent != null)
		{
			RectTransform rectTransform2 = componentInParent.rectTransform();
			Vector3 a = rectTransform2.TransformPoint(rectTransform2.rect.min);
			Vector3 a2 = rectTransform2.TransformPoint(rectTransform2.rect.max);
			Vector3 b = this.highlightBox.TransformPoint(this.highlightBox.rect.min);
			Vector3 b2 = this.highlightBox.TransformPoint(this.highlightBox.rect.max);
			Vector3 vector = a - b;
			Vector3 vector2 = a2 - b2;
			if (vector.x > 0f)
			{
				this.highlightBox.anchoredPosition = this.highlightBox.anchoredPosition + new Vector2(vector.x, 0f);
				this.highlightBox.sizeDelta -= new Vector2(vector.x, 0f);
			}
			else if (vector.y > 0f)
			{
				this.highlightBox.anchoredPosition = this.highlightBox.anchoredPosition + new Vector2(0f, vector.y);
				this.highlightBox.sizeDelta -= new Vector2(0f, vector.y);
			}
			if (vector2.x < 0f)
			{
				this.highlightBox.sizeDelta += new Vector2(vector2.x, 0f);
			}
			if (vector2.y < 0f)
			{
				this.highlightBox.sizeDelta += new Vector2(0f, vector2.y);
			}
		}
		this.highlightBox.gameObject.SetActive(this.highlightBox.sizeDelta.x > 0f && this.highlightBox.sizeDelta.y > 0f);
	}

	// Token: 0x0600670D RID: 26381 RVA: 0x00267D0F File Offset: 0x00265F0F
	private void HideBox()
	{
		this.highlightBox.gameObject.SetActive(false);
	}

	// Token: 0x04004586 RID: 17798
	public RectTransform highlightBoxPrefab;

	// Token: 0x04004587 RID: 17799
	private RectTransform highlightBox;

	// Token: 0x04004588 RID: 17800
	private List<NotificationHighlightTarget> targets = new List<NotificationHighlightTarget>();

	// Token: 0x04004589 RID: 17801
	private ManagementMenuNotification activeTargetNotification;
}

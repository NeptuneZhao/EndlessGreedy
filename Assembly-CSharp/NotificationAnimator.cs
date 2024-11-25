using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CCE RID: 3278
public class NotificationAnimator : MonoBehaviour
{
	// Token: 0x06006567 RID: 25959 RVA: 0x0025DFDE File Offset: 0x0025C1DE
	public void Begin(bool startOffset = true)
	{
		this.Reset();
		this.animating = true;
		if (startOffset)
		{
			this.layoutElement.minWidth = 100f;
			return;
		}
		this.layoutElement.minWidth = 1f;
		this.speed = -10f;
	}

	// Token: 0x06006568 RID: 25960 RVA: 0x0025E01C File Offset: 0x0025C21C
	private void Reset()
	{
		this.bounceCount = 2;
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.layoutElement.minWidth = 0f;
		this.speed = 1f;
	}

	// Token: 0x06006569 RID: 25961 RVA: 0x0025E04C File Offset: 0x0025C24C
	public void Stop()
	{
		this.Reset();
		this.animating = false;
	}

	// Token: 0x0600656A RID: 25962 RVA: 0x0025E05C File Offset: 0x0025C25C
	private void LateUpdate()
	{
		if (!this.animating)
		{
			return;
		}
		this.layoutElement.minWidth -= this.speed;
		this.speed += 0.5f;
		if (this.layoutElement.minWidth <= 0f)
		{
			if (this.bounceCount > 0)
			{
				this.bounceCount--;
				this.speed = -this.speed / Mathf.Pow(2f, (float)(2 - this.bounceCount));
				this.layoutElement.minWidth = -this.speed;
				return;
			}
			this.layoutElement.minWidth = 0f;
			this.Stop();
		}
	}

	// Token: 0x04004488 RID: 17544
	private const float START_SPEED = 1f;

	// Token: 0x04004489 RID: 17545
	private const float ACCELERATION = 0.5f;

	// Token: 0x0400448A RID: 17546
	private const float BOUNCE_DAMPEN = 2f;

	// Token: 0x0400448B RID: 17547
	private const int BOUNCE_COUNT = 2;

	// Token: 0x0400448C RID: 17548
	private const float OFFSETX = 100f;

	// Token: 0x0400448D RID: 17549
	private float speed = 1f;

	// Token: 0x0400448E RID: 17550
	private int bounceCount = 2;

	// Token: 0x0400448F RID: 17551
	private LayoutElement layoutElement;

	// Token: 0x04004490 RID: 17552
	[SerializeField]
	private bool animating = true;
}
